using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using IdentityModel.AspNetCore.OAuth2Introspection;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using OpenEventSourcing.Azure.ServiceBus.Extensions;
using OpenEventSourcing.EntityFrameworkCore.SqlServer;
using OpenEventSourcing.Extensions;
using OpenEventSourcing.RabbitMQ.Extensions;
using OpenEventSourcing.Serialization.Json.Extensions;
using SIO.API.V1;
using SIO.Domain;
using SIO.Domain.Document.Events;
using SIO.Domain.Document.Hubs;
using SIO.Domain.Projections;
using SIO.Domain.Translation.Events;
using SIO.Domain.Translation.Hubs;
using SIO.Domain.User.Events;
using SIO.Domain.User.Hubs;
using SIO.Infrastructure;
using SIO.Infrastructure.AWS;
using SIO.Infrastructure.AWS.Extensions;
using SIO.Infrastructure.Extensions;
using SIO.Infrastructure.Google.Extensions;

namespace SIO.API
{
    public class Startup
    {
        private readonly IHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public Startup(IHostEnvironment env,
            IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = _configuration.GetValue<string>("Identity:Authority");
                        options.ApiName = _configuration.GetValue<string>("Identity:ApiResource");
                        options.EnableCaching = true;
                        options.CacheDuration = TimeSpan.FromMinutes(10);
#if DEBUG
                        options.RequireHttpsMetadata = false;
                        IdentityModelEventSource.ShowPII = true;
#endif
                        options.TokenRetriever = (request) =>
                        {
                            var token = TokenRetrieval.FromAuthorizationHeader()(request);

                            if (string.IsNullOrEmpty(token))
                                token = TokenRetrieval.FromQueryString()(request);

                            return token;
                        };
                        options.JwtBearerEvents.OnAuthenticationFailed = (ctx) =>
                        {
                            return Task.CompletedTask;
                        };
                        options.JwtBearerEvents.OnTokenValidated = (ctx) =>
                        {
                            return Task.CompletedTask;
                        };
                        options.JwtBearerEvents.OnChallenge = (ctx) =>
                        {
                            return Task.CompletedTask;
                        };
                        options.JwtBearerEvents.OnMessageReceived = (ctx) =>
                        {
                            return Task.CompletedTask;
                        };
                        options.JwtBearerEvents.OnForbidden = (ctx) =>
                        {
                            return Task.CompletedTask;
                        };
                    });

            JobStorage.Current = new SqlServerStorage(_configuration.GetConnectionString("DefaultConnection"));

            services.AddOpenEventSourcing()
                .AddEntityFrameworkCoreSqlServer(options => {
                    options.MigrationsAssembly("SIO.Migrations");
                })
                .AddAzureServiceBus(options =>
                {
                    options.UseConnection(_configuration.GetValue<string>("Azure:ServiceBus:ConnectionString"))
                    .UseTopic(e =>
                    {
                        e.WithName(_configuration.GetValue<string>("Azure:ServiceBus:Topic"));
                    })
                    .AddSubscription(s =>
                    {
                        s.UseName(_configuration.GetValue<string>("Azure:ServiceBus:Subscription"));
                        s.ForEvent<DocumentUploaded>();
                        s.ForEvent<DocumentDeleted>();
                        s.ForEvent<TranslationCharactersProcessed>();
                        s.ForEvent<TranslationFailed>();
                        s.ForEvent<TranslationQueued>();
                        s.ForEvent<TranslationStarted>();
                        s.ForEvent<TranslationSucceded>();
                        s.ForEvent<UserPurchasedCharacterTokens>();
                    });
                })
                .AddCommands()
                .AddEvents()
                .AddQueries()
                .AddJsonSerializers();

            services.AddProjections();
            services.AddHostedService<SIOEventConsumer>();

            var infrastructure = services.AddSIOInfrastructure()
                .AddAWSConfiguration(_configuration)
                .AddAWSTranslations()
                .AddGoogleInfrastructure(_configuration)
                .AddSqlConnections();

            if (_env.IsDevelopment())
                infrastructure.AddLocalInfrastructure();
            else
                infrastructure.AddAWSFiles();             

            services.AddSingleton<IUserIdProvider, SubjectUserIdProvider>();
            services.AddSignalR();

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddApiVersioning(options => options.ReportApiVersions = true);

            services.AddSwaggerGen(options =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    var info = new OpenApiInfo()
                    {
                        Title = $"Sound It Out Api v{description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                        Description = $"Sound It Out Api v{description.ApiVersion}",
                        Contact = new OpenApiContact { Name = "Support", Email = "support@sound-it-out.com" },
                    };

                    if (description.IsDeprecated)
                        info.Description += "- This Api version has been deprecated.";

                    options.SwaggerDoc(description.GroupName, info);
                }

                options.DescribeAllParametersInCamelCase();

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\".\n\nIn the value below you will need to include \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
            services.AddMvcCore()
                   .AddApiExplorer();

            services.AddMvc()
                .AddRazorRuntimeCompilation()
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddVersionedApiExplorer(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = false;
                options.GroupNameFormat = "VVVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            if (!env.IsDevelopment())
                app.UseHttpsRedirection();

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());


            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<DocumentHub>($"/v1/{nameof(DocumentHub).ToLower()}");
                endpoints.MapHub<TranslationHub>($"/v1/{nameof(TranslationHub).ToLower()}");
                endpoints.MapHub<UserHub>($"/v1/{nameof(UserHub).ToLower()}");
                endpoints.MapControllers();
            });

            var apiVersionDescriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger(options => options.RouteTemplate = "/api-docs/{documentName}/swagger.json");

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "api-docs";
                options.DocumentTitle = "Sound it out Api";
                options.DisplayRequestDuration();
                options.OAuthClientId("sio-api-docs-client");

                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                    options.SwaggerEndpoint($"/api-docs/{description.GroupName}/swagger.json", $"v{description.GroupName.ToUpperInvariant()}");
            });
        }
    }
}
