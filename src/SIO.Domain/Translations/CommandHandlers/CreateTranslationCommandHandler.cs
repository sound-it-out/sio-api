using System;
using System.Threading.Tasks;
using OpenEventSourcing.Commands;
using OpenEventSourcing.Events;
using SIO.Domain.Translations.Commands;
using SIO.Domain.Translations.Events;

namespace SIO.Domain.Translations.CommandHandlers
{
    internal class CreateTranslationCommandHandler : ICommandHandler<CreateTranslationCommand>
    {
        private readonly IEventBus _eventBus;

        public CreateTranslationCommandHandler(IEventBus eventBus)
        {
            if (eventBus == null)
                throw new ArgumentNullException(nameof(eventBus));

            _eventBus = eventBus;
        }

        public async Task ExecuteAsync(CreateTranslationCommand command)
        {
            // TODO(Matt): 
            // 1. Write file to blob storage
            // 2. Create DB entry

            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var @event = new TranslationCreated(command.AggregateId, command.Version, command.TranslationType);
            @event.UpdateFrom(command);

            await _eventBus.PublishAsync(@event);
        }
    }
}
