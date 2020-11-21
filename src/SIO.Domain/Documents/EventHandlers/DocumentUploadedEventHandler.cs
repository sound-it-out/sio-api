using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OpenEventSourcing.Events;
using SIO.Domain.Documents.Events;
using SIO.Domain.Extenions;
using SIO.Infrastructure.Notifications.Hubs;

namespace SIO.Domain.Documents.EventHandlers
{
    internal class DocumentUploadedEventHandler : IEventHandler<DocumentUploaded>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public DocumentUploadedEventHandler(IHubContext<NotificationHub> hubContext)
        {
            if (hubContext == null)
                throw new ArgumentNullException(nameof(hubContext));

            _hubContext = hubContext;
        }

        public async Task HandleAsync(DocumentUploaded @event)
        {
            await _hubContext.NotifyAsync(@event);
        }
    }
}
