using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SIO.Domain.Translations.Events;
using SIO.Infrastructure.Notifications.Hubs;

namespace SIO.Domain.Translations.Hubs
{
    internal static class NotificationHubHubExtensions
    {
        public static async Task NotifyAsync(this IHubContext<NotificationHub> source, TranslationStarted @event)
        {
            await source.Clients.User(@event.UserId).SendAsync(nameof(TranslationStarted), @event);
        }
        public static async Task NotifyAsync(this IHubContext<NotificationHub> source, TranslationQueued @event)
        {
            await source.Clients.User(@event.UserId).SendAsync(nameof(TranslationQueued), @event);
        }
        public static async Task NotifyAsync(this IHubContext<NotificationHub> source, TranslationSucceded @event)
        {
            await source.Clients.User(@event.UserId).SendAsync(nameof(TranslationSucceded), @event);
        }
        public static async Task NotifyAsync(this IHubContext<NotificationHub> source, TranslationFailed @event)
        {
            await source.Clients.User(@event.UserId).SendAsync(nameof(TranslationFailed), @event);
        }
        public static async Task NotifyAsync(this IHubContext<NotificationHub> source, TranslationCharactersProcessed @event)
        {
            await source.Clients.User(@event.UserId).SendAsync(nameof(TranslationCharactersProcessed), @event);
        }
    }
}
