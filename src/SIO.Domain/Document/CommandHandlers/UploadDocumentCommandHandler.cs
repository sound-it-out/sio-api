using System;
using System.Threading.Tasks;
using OpenEventSourcing.Commands;
using OpenEventSourcing.Events;
using SIO.Domain.Document.Commands;
using SIO.Domain.Document.Events;

namespace SIO.Domain.Document.CommandHandlers
{
    internal class UploadDocumentCommandHandler : ICommandHandler<UploadDocumentCommand>
    {
        private readonly IEventBus _eventBus;

        public UploadDocumentCommandHandler(IEventBus eventBus)
        {
            if (eventBus == null)
                throw new ArgumentNullException(nameof(eventBus));

            _eventBus = eventBus;
        }

        public async Task ExecuteAsync(UploadDocumentCommand command)
        {
            // TODO(Matt): 
            // 1. Write file to blob storage

            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var @event = new DocumentUploaded(command.AggregateId, command.Version, command.TranslationType, "");
            @event.UpdateFrom(command);

            await _eventBus.PublishAsync(@event);
        }
    }
}
