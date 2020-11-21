using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OpenEventSourcing.Events;
using SIO.Domain.Documents.Events;
using SIO.Domain.Documents.Hubs;

namespace SIO.Domain.Documents.EventHandlers
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
