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
                routes.MapHub<DocumentHub>($"/{nameof(DocumentHub).ToLower()}");
                routes.MapHub<TranslationHub>($"/{nameof(TranslationHub).ToLower()}");
                routes.MapHub<UserHub>($"/{nameof(UserHub).ToLower()}");
            });

            return app;
        }
    }
}
