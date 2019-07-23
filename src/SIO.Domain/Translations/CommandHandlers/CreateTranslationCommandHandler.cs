using System;
using System.Linq;
using System.Threading.Tasks;
using OpenEventSourcing.Commands;
using OpenEventSourcing.Domain;
using OpenEventSourcing.Events;
using SIO.Domain.Translations.Aggregates;
using SIO.Domain.Translations.Commands;

namespace SIO.Domain.Translations.CommandHandlers
{
    internal class CreateTranslationCommandHandler : ICommandHandler<CreateTranslationCommand>
    {
        private readonly IAggregateRepository _aggregateRepository;
        private readonly IAggregateFactory _aggregateFactory;
        private readonly IEventBus _eventBus;

        public CreateTranslationCommandHandler(IAggregateRepository aggregateRepository,
            IAggregateFactory aggregateFactory,
            IEventBus eventBus)
        {
            if (aggregateRepository == null)
                throw new ArgumentNullException(nameof(aggregateRepository));
            if (aggregateFactory == null)
                throw new ArgumentNullException(nameof(aggregateFactory));
            if (eventBus == null)
                throw new ArgumentNullException(nameof(eventBus));

            _aggregateRepository = aggregateRepository;
            _aggregateFactory = aggregateFactory;
            _eventBus = eventBus;
        }

        public async Task ExecuteAsync(CreateTranslationCommand command)
        {
            // TODO(Matt): 
            // 1. Write file to blob storage
            // 2. Create DB entry

            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var aggregate = _aggregateFactory.FromHistory <Translation, TranslationState>(Enumerable.Empty<IEvent>());

            aggregate.Create(Guid.NewGuid(), "");

            var events = aggregate.GetUncommittedEvents().ToList();

            foreach (var @event in events)
                @event.UpdateFrom(command);

            await _aggregateRepository.SaveAsync<Translation, TranslationState>(aggregate, 0);
            await _eventBus.PublishAsync(events);
        }
    }
}
