using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SIO.Domain.User.Events;

namespace SIO.Domain.User.Hubs
{
    internal static class UserHubExtensions
    {
        public static async Task NotifyAsync(this IHubContext<UserHub> source, UserPurchasedCharacterTokens @event)
        {
            await source.Clients.User(@event.UserId).SendAsync(nameof(UserPurchasedCharacterTokens), @event);
        }

        public static async Task NotifyAsync(this IHubContext<UserHub> source, UserEmailChanged @event)
        {
            await source.Clients.User(@event.AggregateId.ToString()).SendAsync(nameof(UserEmailChanged), @event);
        }
    }
}
