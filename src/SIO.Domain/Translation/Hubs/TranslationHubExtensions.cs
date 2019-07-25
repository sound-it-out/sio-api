using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SIO.Domain.Translation.Events;

namespace SIO.Domain.Translation.Hubs
{
    internal static class TranslationHubExtensions
    {
        public static Task NotifyAsync(this IHubContext<TranslationHub> source, TranslationStarted @event)
        {
            return source.Clients.User(@event.UserId).SendAsync(nameof(TranslationStarted), @event);
        }
        public static Task NotifyAsync(this IHubContext<TranslationHub> source, TranslationQueued @event)
        {
            return source.Clients.User(@event.UserId).SendAsync(nameof(TranslationQueued), @event);
        }
        public static Task NotifyAsync(this IHubContext<TranslationHub> source, TranslationSucceded @event)
        {
            return source.Clients.User(@event.UserId).SendAsync(nameof(TranslationSucceded), @event);
        }
        public static Task NotifyAsync(this IHubContext<TranslationHub> source, TranslationFailed @event)
        {
            return source.Clients.User(@event.UserId).SendAsync(nameof(TranslationFailed), @event);
        }
        public static Task NotifyAsync(this IHubContext<TranslationHub> source, TranslationCharactersProcessed @event)
        {
            return source.Clients.User(@event.UserId).SendAsync(nameof(TranslationCharactersProcessed), @event);
        }
    }
}
