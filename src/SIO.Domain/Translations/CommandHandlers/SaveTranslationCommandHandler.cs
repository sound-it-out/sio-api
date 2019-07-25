using System;
using System.Threading.Tasks;
using OpenEventSourcing.Commands;
using OpenEventSourcing.Events;
using SIO.Domain.Translation.Commands;
using SIO.Domain.Translation.Events;

namespace SIO.Domain.Translation.CommandHandlers
{
    internal class SaveTranslationCommandHandler : ICommandHandler<SaveTranslationCommand>
    {
        private readonly IEventBus _eventBus;

        public SaveTranslationCommandHandler(IEventBus eventBus)
        {
            if (eventBus == null)
                throw new ArgumentNullException(nameof(eventBus));

            _eventBus = eventBus;
        }

        public async Task ExecuteAsync(SaveTranslationCommand command)
        {
            var @event = new TranslationSucceded(
                aggregateId: command.AggregateId,
                version: command.Version,
                ""
            );

            @event.UpdateFrom(command);

            await _eventBus.PublishAsync(@event);
        }
    }
}
