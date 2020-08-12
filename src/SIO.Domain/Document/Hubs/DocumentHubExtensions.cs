using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SIO.Domain.Document.Events;

namespace SIO.Domain.Document.Hubs
{
    internal static class DocumentHubExtensions
    {
        public static async Task NotifyAsync(this IHubContext<DocumentHub> source, DocumentUploaded @event)
        {
            await source.Clients.User(@event.UserId).SendAsync(nameof(DocumentUploaded), @event);
        }

        public static async Task NotifyAsync(this IHubContext<DocumentHub> source, DocumentDeleted @event)
        {
            await source.Clients.User(@event.UserId).SendAsync(nameof(DocumentDeleted), @event);
        }
    }
}
