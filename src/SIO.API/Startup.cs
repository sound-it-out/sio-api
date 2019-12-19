﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenEventSourcing.EntityFrameworkCore.DbContexts;
using OpenEventSourcing.EntityFrameworkCore.SqlServer;
using OpenEventSourcing.Extensions;
using OpenEventSourcing.RabbitMQ.Extensions;
using OpenEventSourcing.Serialization.Json.Extensions;
using SIO.Domain.Projections;
using SIO.Domain.Projections.UserDocuments;
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddOpenEventSourcing()
                .AddEntityFrameworkCoreSqlServer()
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
                            s.ForEvent<UserRegistered>();
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
        }
    }
}
