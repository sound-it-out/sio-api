using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using IdentityModel.AspNetCore.OAuth2Introspection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenEventSourcing.Azure.ServiceBus.Extensions;
using OpenEventSourcing.EntityFrameworkCore.SqlServer;
using OpenEventSourcing.Extensions;
using OpenEventSourcing.Serialization.Json.Extensions;
using SIO.API.V1;
using SIO.Domain.Documents.Events;
using SIO.Domain.Projections.Extensions;
using SIO.Domain.Translations.Events;
using SIO.Domain.Users.Events;
using SIO.Infrastructure.AWS;
using SIO.Infrastructure.AWS.Extensions;
using SIO.Infrastructure.Extensions;
using SIO.Infrastructure.Google.Extensions;
using SIO.Infrastructure.Notifications.Hubs;

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
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
                options.Authority = _configuration.GetValue<string>("Identity:Authority");
                options.Audience = _configuration.GetValue<string>("Identity:ApiResource");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = TokenRetrieval.FromAuthorizationHeader()(context.Request);

                        if (string.IsNullOrEmpty(token))
                            token = TokenRetrieval.FromQueryString()(context.Request);

                        context.Token = token;

                        return Task.CompletedTask;
                    }
                };
           }); 
           
            services.AddAuthorization(options =>
           {
               options.AddPolicy("ApiScope", policy =>
               {
                   policy.RequireAuthenticatedUser();
                   policy.RequireClaim("scope", "api");
               });
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

            var infrastructure = services.AddSIOInfrastructure()
                .AddEvents()
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
                endpoints.MapHub<NotificationHub>($"/v1/{nameof(NotificationHub).ToLower()}");
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
