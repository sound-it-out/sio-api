using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SIO.Domain.Documents.Events;
using SIO.Infrastructure.Notifications.Hubs;

namespace SIO.Domain.Documents.Hubs
{
    internal static class NotificationHubExtensions
    {
        public static async Task NotifyAsync(this IHubContext<NotificationHub> source, DocumentUploaded @event)
        {
            await source.Clients.User(@event.UserId).SendAsync(nameof(DocumentUploaded), @event);
        }

        public static async Task NotifyAsync(this IHubContext<NotificationHub> source, DocumentDeleted @event)
        {
            await source.Clients.User(@event.UserId).SendAsync(nameof(DocumentDeleted), @event);
        }
    }
}
