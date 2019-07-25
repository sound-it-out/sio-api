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
        private readonly IEventBus _eventBus;
        private readonly IAggregateRepository _aggregateRepository;

        public SaveTranslationCommandHandler(IEventBus eventBus, IAggregateRepository aggregateRepository)
        {
            if (eventBus == null)
                throw new ArgumentNullException(nameof(eventBus));
            if (aggregateRepository == null)
                throw new ArgumentNullException(nameof(aggregateRepository));

            _eventBus = eventBus;
            _aggregateRepository = aggregateRepository;
        }

        public async Task ExecuteAsync(SaveTranslationCommand command)
        {
            var aggregate = await _aggregateRepository.GetAsync<Document.Document, DocumentState>(command.CorrelationId);
            aggregate.AcceptTranslation(command.CorrelationId, command.Version, "");

            var events = aggregate.GetUncommittedEvents();

            foreach (var @event in events)
                @event.UpdateFrom(command);

            events = events.ToList();

            await _aggregateRepository.SaveAsync<Document.Document, DocumentState>(aggregate, 0);
            await _eventBus.PublishAsync(events);
        }
    }
}
