using System;
using Hangfire;
using Hangfire.SqlServer;
using IdentityModel.AspNetCore.OAuth2Introspection;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using OpenEventSourcing.EntityFrameworkCore.SqlServer;
using OpenEventSourcing.Extensions;
using OpenEventSourcing.RabbitMQ.Extensions;
using OpenEventSourcing.Serialization.Json.Extensions;
using SIO.Domain;
using SIO.Domain.Document.Events;
using SIO.Domain.Projections;
using SIO.Domain.Translation.Events;
using SIO.Domain.User.Events;
using SIO.Infrastructure;
using SIO.Infrastructure.AWS;
using SIO.Infrastructure.Google;

namespace SIO.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = Configuration.GetValue<string>("Identity:Authority");
                        options.ApiName = Configuration.GetValue<string>("Identity:ApiResource");
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
                    });

            JobStorage.Current = new SqlServerStorage(Configuration.GetConnectionString("DefaultConnection"));

            services.AddOpenEventSourcing()
                .AddEntityFrameworkCoreSqlServer(options => {
                    options.MigrationsAssembly("SIO.Migrations");
                })
                .AddRabbitMq(options =>
                {
                    options.UseConnection(Configuration.GetValue<string>("RabbitMQ:Connection"))
                        .UseExchange(e =>
                        {
                            e.WithName(Configuration.GetValue<string>("RabbitMQ:Exchange:Name"));
                            e.UseExchangeType(Configuration.GetValue<string>("RabbitMQ:Exchange:Type"));
                        })
                        .AddSubscription(s =>
                        {

                            s.ForEvent<DocumentUploaded>();
                            s.ForEvent<TranslationCharactersProcessed>();
                            s.ForEvent<TranslationFailed>();
                            s.ForEvent<TranslationQueued>();
                            s.ForEvent<TranslationStarted>();
                            s.ForEvent<TranslationSucceded>();
                            s.ForEvent<UserPurchasedCharacterTokens>();
                            s.UseName("sio-api");
                        })
                        .UseManagementApi(m =>
                        {
                            m.WithEndpoint(Configuration.GetValue<string>("RabbitMQ:ManagementApi:Endpoint"));
                            m.WithCredentials(Configuration.GetValue<string>("RabbitMQ:ManagementApi:Username"), Configuration.GetValue<string>("RabbitMQ:ManagementApi:Password"));
                        });
                })
                .AddCommands()
                .AddEvents()
                .AddQueries()
                .AddJsonSerializers();

            services.AddProjections();
            services.AddHostedService<SIOEventConsumer>();

            services.AddSIOInfrastructure()
                .AddSqlConnections()
                .AddS3FileStorage()
                .AddGoogleSpeechToText();

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

            services.AddMvcCore()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddApiExplorer()
                .AddVersionedApiExplorer(options =>
               {
                   options.AssumeDefaultVersionWhenUnspecified = false;
                   options.GroupNameFormat = "VVVV";
                   options.SubstituteApiVersionInUrl = true;
               })
                .AddJsonFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(o =>
            {
                o.AllowAnyHeader()
                .AllowCredentials()
                .AllowAnyMethod()
                .WithOrigins(new[] { "http://localhost:8080"});
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            var apiVersionDescriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseSwagger(options => options.RouteTemplate = "/api-docs/{documentName}/swagger.json");

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = string.Empty;
                options.DocumentTitle = "Sound It Out Api";

                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                    options.SwaggerEndpoint($"/api-docs/{description.GroupName}/swagger.json", $"v{description.GroupName.ToUpperInvariant()}");
            });

            app.UseDomain();
            app.UseMvc();
        }
    }
}
