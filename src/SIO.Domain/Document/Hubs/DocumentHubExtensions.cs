using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SIO.Domain.Document.Events;

namespace SIO.Domain.Document.Hubs
{
    internal static class DocumentHubExtensions
    {
        public static Task NotifyAsync(this IHubContext<DocumentHub> source, DocumentUploaded @event)
        {
            return source.Clients.User(@event.UserId).SendAsync(nameof(DocumentUploaded), @event);
        }
    }
}
