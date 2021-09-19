using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace SIO.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseOpenApi(this IApplicationBuilder app)
        {
            var apiVersionDescriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger(options => options.RouteTemplate = "/api-docs/{documentName}/swagger.json");

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "api-docs";
                options.DocumentTitle = "Sound it out Api";
                options.DisplayRequestDuration();
                options.OAuthClientId("sio-api-docs-client");
                options.OAuthScopeSeparator(" ");
                options.OAuthUsePkce();

                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                    options.SwaggerEndpoint($"/api-docs/{description.GroupName}/swagger.json", $"v{description.GroupName.ToUpperInvariant()}");
            });

            return app;
        }
    }
}
