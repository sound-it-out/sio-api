using System;
using System.Linq;
using System.Threading.Tasks;
using OpenEventSourcing.Commands;
using OpenEventSourcing.Domain;
using OpenEventSourcing.Events;
using SIO.Domain.Document.Commands;

namespace SIO.Domain.Document.CommandHandlers
{
    internal class UploadDocumentCommandHandler : ICommandHandler<UploadDocumentCommand>
    {
        private readonly IEventBus _eventBus;
        private readonly IAggregateRepository _aggregateRepository;
        private readonly IAggregateFactory _aggregateFactory;

        public UploadDocumentCommandHandler(IEventBus eventBus, IAggregateRepository aggregateRepository, IAggregateFactory aggregateFactory)
        {
            if (eventBus == null)
                throw new ArgumentNullException(nameof(eventBus));
            if (aggregateRepository == null)
                throw new ArgumentNullException(nameof(aggregateRepository));
            if (aggregateFactory == null)
                throw new ArgumentNullException(nameof(aggregateFactory));

            _eventBus = eventBus;
            _aggregateRepository = aggregateRepository;
            _aggregateFactory = aggregateFactory;
        }

        public async Task ExecuteAsync(UploadDocumentCommand command)
        {
            // TODO(Matt): 
            // 1. Write file to blob storage

            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var aggregate = _aggregateFactory.FromHistory<Document, DocumentState>(Enumerable.Empty<IEvent>());

            if(aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));

            aggregate.UploadDocument(translationType: command.TranslationType, filePath: "");

            var events = aggregate.GetUncommittedEvents();

            foreach (var @event in events)
                @event.UpdateFrom(command);

            events = events.ToList();

            await _aggregateRepository.SaveAsync<Document, DocumentState>(aggregate, 0);
            await _eventBus.PublishAsync(events);
        }
    }
}
