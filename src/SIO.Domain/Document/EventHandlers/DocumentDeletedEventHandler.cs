using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OpenEventSourcing.Events;
using SIO.Domain.Document.Events;
using SIO.Domain.Document.Hubs;

namespace SIO.Domain.Document.EventHandlers
{
    internal class DocumentDeletedEventHandler : IEventHandler<DocumentDeleted>
    {
        private readonly IHubContext<DocumentHub> _hubContext;

        public DocumentDeletedEventHandler(IHubContext<DocumentHub> hubContext)
        {
            if (hubContext == null)
                throw new ArgumentNullException(nameof(hubContext));

            _hubContext = hubContext;
        }

        public async Task HandleAsync(DocumentDeleted @event)
        {
            await _hubContext.NotifyAsync(@event);
        }
    }
}
