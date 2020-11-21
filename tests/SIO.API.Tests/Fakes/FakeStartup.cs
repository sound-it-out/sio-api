using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenEventSourcing.Extensions;
using OpenEventSourcing.Serialization.Json.Extensions;
using SIO.API.Tests.Abstractions;
using SIO.API.V1;
using SIO.Domain.Documents.Hubs;
using SIO.Domain.Projections.Extensions;
using SIO.Domain.Translations.Hubs;
using SIO.Domain.Users.Hubs;
using SIO.Infrastructure.AWS;
using SIO.Infrastructure.AWS.Extensions;
using SIO.Infrastructure.Extensions;
using SIO.Infrastructure.Google.Extensions;
using SIO.Testing.Extensions;

namespace SIO.API.Tests.Fakes
{
    public class FakeStartup
    {
        private readonly IHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public FakeStartup(IHostEnvironment env,
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

            GlobalConfiguration.Configuration.UseMemoryStorage();

            services.AddOpenEventSourcing()
                .AddCommands()
                .AddEvents()
                .AddQueries()
                .AddJsonSerializers();

            services.AddInMemoryDatabase()
                .AddInMemoryFiles()
                .AddInMemoryEventBus()
                .AddProjections()
                .AddSingleton<IUserIdProvider, SubjectUserIdProvider>()
                .AddTransient<IEventSeeder, EventSeeder>()
                .Configure<RouteOptions>(options => options.LowercaseUrls = true)
                .AddApiVersioning(options => options.ReportApiVersions = true);

            services.AddSIOInfrastructure()
                .AddEvents()
                .AddAWSConfiguration(_configuration)
                .AddAWSTranslations()
                .AddLocalTranslations()
                .AddGoogleInfrastructure(_configuration)
                .AddSqlConnections();

            services.AddSignalR();

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
