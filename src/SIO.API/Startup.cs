using System;
using IdentityModel.AspNetCore.OAuth2Introspection;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using OpenEventSourcing.EntityFrameworkCore.DbContexts;
using OpenEventSourcing.EntityFrameworkCore.SqlServer;
using OpenEventSourcing.Extensions;
using OpenEventSourcing.RabbitMQ.Extensions;
using OpenEventSourcing.Serialization.Json.Extensions;
using SIO.Domain;
using SIO.Domain.Document.Events;
using SIO.Domain.Projections;
using SIO.Domain.Projections.UserDocuments;
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
            app.UseMvc();

            app.UseDomain();
        }
    }
}
