using IdentityModel.AspNetCore.OAuth2Introspection;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using SIO.Api.OpenApi;
using SIO.Domain;
using SIO.Infrastructure.Azure.ServiceBus.Extensions;
using SIO.Infrastructure.Azure.Storage.Extensions;
using SIO.Infrastructure.EntityFrameworkCore.SqlServer.Extensions;
using SIO.Infrastructure.Extensions;
using SIO.Infrastructure.Serialization.Json.Extensions;
using SIO.Infrastructure.Serialization.MessagePack.Extensions;
using System;
using System.Collections.Generic;

namespace SIO.Api.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = configuration.GetValue<string>("Identity:Authority");
                        options.ApiName = configuration.GetValue<string>("Identity:ApiResource");
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

            services.AddAuthorization();

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSIOInfrastructure()
                .AddEntityFrameworkCoreSqlServer(options => {
                    options.AddStore(configuration.GetConnectionString("Store"), o => o.MigrationsAssembly($"{nameof(SIO)}.{nameof(Migrations)}"));
                    options.AddProjections(configuration.GetConnectionString("Projection"), o => o.MigrationsAssembly($"{nameof(SIO)}.{nameof(Migrations)}"));
                })
                .AddAzureServiceBus(options =>
                {
                    options.UseConnection(configuration.GetConnectionString("AzureServiceBus"))
                    .UseTopic(e =>
                    {
                        e.WithName(configuration.GetValue<string>("Azure:ServiceBus:Topic"));
                    })
                    .AddSubscription(s =>
                    {
                        s.UseName(configuration.GetValue<string>("Azure:ServiceBus:Subscription"));
                        s.ForEvents(EventHelper.AllEvents);
                    });
                })
                .AddAzureStorage(o => o.ConnectionString = configuration.GetConnectionString("AzureStorage"))
                .AddCommands()
                .AddEvents(options =>
                {
                    options.Register(EventHelper.AllEvents);
                })
                .AddQueries()
                .AddJsonSerializers();

            return services;
        }

        public static IServiceCollection AddOpenApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

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

                var securityScheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Query,
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{configuration.GetValue<string>("Identity:Authority")}/connect/authorize"),
                            TokenUrl = new Uri($"{configuration.GetValue<string>("Identity:Authority")}/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "api", "api" }
                            },

                        }
                    },
                    Scheme = "Bearer",
                    Name = "Authorization",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "oauth2"
                    }
                };
                var securityRequirement = new OpenApiSecurityRequirement();

                securityRequirement.Add(securityScheme, new[] { "Bearer" });
                options.OperationFilter<AuthorizeOperationFilter>();
                options.AddSecurityDefinition("oauth2", securityScheme);
                options.AddSecurityRequirement(securityRequirement);
            });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = false;
                options.GroupNameFormat = "VVVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}
