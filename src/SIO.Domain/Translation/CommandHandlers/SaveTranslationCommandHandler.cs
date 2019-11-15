using System;
using System.Linq;
using System.Threading.Tasks;
using OpenEventSourcing.Commands;
using OpenEventSourcing.Domain;
using OpenEventSourcing.Events;
using SIO.Domain.Document;
using SIO.Domain.Translation.Commands;

namespace SIO.Domain.Translation.CommandHandlers
{
    internal class SaveTranslationCommandHandler : ICommandHandler<SaveTranslationCommand>
    {
        private readonly IEventBusPublisher _eventBusPublisher;
        private readonly IAggregateRepository _aggregateRepository;

        public SaveTranslationCommandHandler(IEventBusPublisher eventBusPublisher, IAggregateRepository aggregateRepository)
        {
            if (eventBusPublisher == null)
                throw new ArgumentNullException(nameof(eventBusPublisher));
            if (aggregateRepository == null)
                throw new ArgumentNullException(nameof(aggregateRepository));

            _eventBusPublisher = eventBusPublisher;
            _aggregateRepository = aggregateRepository;
        }

        public async Task ExecuteAsync(SaveTranslationCommand command)
        {
            var aggregate = await _aggregateRepository.GetAsync<Document.Document, DocumentState>(command.CorrelationId);
            aggregate.AcceptTranslation(command.CorrelationId, command.Version);

            var events = aggregate.GetUncommittedEvents();

            foreach (var @event in events)
                @event.UpdateFrom(command);

            events = events.ToList();

            await _aggregateRepository.SaveAsync<DocumentState>(aggregate, command.Version);
            await _eventBusPublisher.PublishAsync(events);
        }
    }
}
