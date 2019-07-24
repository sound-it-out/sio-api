using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SIO.Domain.Translations.Events;

namespace SIO.Domain.Translations.Hubs
{
    internal static class TranslationHubExtensions
    {
        public static Task NotifyAsync(this IHubContext<TranslationHub> source, TranslationCreated @event)
        {
            return source.Clients.User(@event.UserId).SendAsync(nameof(TranslationCreated), @event);
        }
        public static Task NotifyAsync(this IHubContext<TranslationHub> source, TranslationGenerated @event)
        {
            return source.Clients.User(@event.UserId).SendAsync(nameof(TranslationGenerated), @event);
        }
    }
}
