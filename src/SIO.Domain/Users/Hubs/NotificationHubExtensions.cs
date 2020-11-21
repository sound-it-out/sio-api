using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SIO.Domain.Users.Events;
using SIO.Infrastructure.Notifications.Hubs;

namespace SIO.Domain.Users.Hubs
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
    }
}
