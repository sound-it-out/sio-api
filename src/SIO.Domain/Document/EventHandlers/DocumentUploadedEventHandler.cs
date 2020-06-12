using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OpenEventSourcing.Commands;
using OpenEventSourcing.Events;
using SIO.Domain.Document.Events;
using SIO.Domain.Document.Hubs;

namespace SIO.Domain.Document.EventHandlers
{
    internal class DocumentUploadedEventHandler : IEventHandler<DocumentUploaded>
    {
        private readonly IHubContext<DocumentHub> _hubContext;

        public DocumentUploadedEventHandler(IHubContext<DocumentHub> hubContext)
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
