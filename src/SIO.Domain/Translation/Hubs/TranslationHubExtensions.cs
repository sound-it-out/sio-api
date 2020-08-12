using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SIO.Domain.Translation.Events;

namespace SIO.Domain.Translation.Hubs
{
    internal static class TranslationHubExtensions
    {
        public static async Task NotifyAsync(this IHubContext<TranslationHub> source, TranslationStarted @event)
        {
            await source.Clients.User(@event.UserId).SendAsync(nameof(TranslationStarted), @event);
        }
        public static async Task NotifyAsync(this IHubContext<TranslationHub> source, TranslationQueued @event)
        {
            await source.Clients.User(@event.UserId).SendAsync(nameof(TranslationQueued), @event);
        }
        public static async Task NotifyAsync(this IHubContext<TranslationHub> source, TranslationSucceded @event)
        {
            await source.Clients.User(@event.UserId).SendAsync(nameof(TranslationSucceded), @event);
        }
        public static async Task NotifyAsync(this IHubContext<TranslationHub> source, TranslationFailed @event)
        {
            await source.Clients.User(@event.UserId).SendAsync(nameof(TranslationFailed), @event);
        }
        public static async Task NotifyAsync(this IHubContext<TranslationHub> source, TranslationCharactersProcessed @event)
        {
            await source.Clients.User(@event.UserId).SendAsync(nameof(TranslationCharactersProcessed), @event);
        }
    }
}
