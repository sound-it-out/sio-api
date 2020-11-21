using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SIO.Domain.Documents.Events;
using SIO.Domain.Translations.Events;
using SIO.Domain.Users.Events;
using SIO.Infrastructure.Notifications.Hubs;

namespace SIO.Domain.Extenions
{
    internal static class NotificationHubExtensions
    {
        public static async Task NotifyAsync(this IHubContext<NotificationHub> source, UserPurchasedCharacterTokens @event)
        {
            await source.Clients.User(@event.UserId).SendAsync(nameof(UserPurchasedCharacterTokens), @event);
        }

        public static async Task NotifyAsync(this IHubContext<NotificationHub> source, UserEmailChanged @event)
        {
            await source.Clients.User(@event.AggregateId.ToString()).SendAsync(nameof(UserEmailChanged), @event);
        }

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

        public static async Task NotifyAsync(this IHubContext<NotificationHub> source, DocumentUploaded @event)
        {
            await source.Clients.User(@event.UserId).SendAsync(nameof(DocumentUploaded), @event);
            await source.Clients.User(@event.UserId).SendAsync(nameof(DocumentUploaded), @event);
        }

        public static async Task NotifyAsync(this IHubContext<NotificationHub> source, DocumentDeleted @event)
        {
            await source.Clients.User(@event.UserId).SendAsync(nameof(DocumentDeleted), @event);
        }
    }
}
