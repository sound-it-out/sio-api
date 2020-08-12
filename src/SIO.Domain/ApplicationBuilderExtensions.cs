using Microsoft.AspNetCore.Builder;
using SIO.Domain.Document.Hubs;
using SIO.Domain.Translation.Hubs;
using SIO.Domain.User.Hubs;

namespace SIO.Domain
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDomain(this IApplicationBuilder app)
        {
            app.UseSignalR(routes =>
            {
                routes.MapHub<DocumentHub>($"/v1/{nameof(DocumentHub).ToLower()}");
                routes.MapHub<TranslationHub>($"/v1/{nameof(TranslationHub).ToLower()}");
                routes.MapHub<UserHub>($"/v1/{nameof(UserHub).ToLower()}");
            });

            return app;
        }
    }
}
