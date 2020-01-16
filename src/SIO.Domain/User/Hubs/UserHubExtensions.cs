using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SIO.Domain.User.Events;

namespace SIO.Domain.User.Hubs
{
    internal static class UserHubExtensions
    {
        public static Task NotifyAsync(this IHubContext<UserHub> source, UserPurchasedCharacterTokens @event)
        {
            return source.Clients.User(@event.UserId).SendAsync(nameof(UserPurchasedCharacterTokens), @event);
        }

        public static Task NotifyAsync(this IHubContext<UserHub> source, UserEmailChanged @event)
        {
            return source.Clients.User(@event.AggregateId.ToString()).SendAsync(nameof(UserEmailChanged), @event);
        }
    }
}
