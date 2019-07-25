using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OpenEventSourcing.Commands;
using OpenEventSourcing.Events;
using SIO.Domain.Document.Events;
using SIO.Domain.Document.Hubs;
using SIO.Domain.Translation.Commands;

namespace SIO.Domain.Document.EventHandlers
{
    internal class DocumentUploadedEventHandler : IEventHandler<DocumentUploaded>
    {
        private readonly IHubContext<DocumentHub> _hubContext;
        private readonly ICommandDispatcher _commandDispatcher;

        public DocumentUploadedEventHandler(IHubContext<DocumentHub> hubContext, ICommandDispatcher commandDispatcher)
        {
            if (hubContext == null)
                throw new ArgumentNullException(nameof(hubContext));
            if (commandDispatcher == null)
                throw new ArgumentNullException(nameof(commandDispatcher));

            _hubContext = hubContext;
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(DocumentUploaded @event)
        {
            await _hubContext.NotifyAsync(@event);

            await _commandDispatcher.DispatchAsync(new QueueTranslationCommand(
                aggregateId: @event.AggregateId,
                correlationId: @event.CorrelationId.Value,
                version: @event.Version + 1,
                userId: @event.UserId,
                translationType: @event.TranslationType
            ));
        }
    }
}
