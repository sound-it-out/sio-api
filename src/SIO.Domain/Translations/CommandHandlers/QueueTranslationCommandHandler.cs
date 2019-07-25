using System;
using System.IO;
using System.Threading.Tasks;
using Clipboard;
using Hangfire;
using OpenEventSourcing.Commands;
using OpenEventSourcing.Events;
using SIO.Domain.Translation.Commands;
using SIO.Domain.Translation.Events;

namespace SIO.Domain.Translation.CommandHandlers
{
    internal class QueueTranslationCommandHandler : ICommandHandler<QueueTranslationCommand>
    {
        private readonly IEventBus _eventBus;
        private readonly ICommandDispatcher _commandDispatcher;

        public QueueTranslationCommandHandler(IEventBus eventBus, ICommandDispatcher commandDispatcher)
        {
            if (eventBus == null)
                throw new ArgumentNullException(nameof(eventBus));
            if (commandDispatcher == null)
                throw new ArgumentNullException(nameof(commandDispatcher));

            _eventBus = eventBus;
            _commandDispatcher = commandDispatcher;
        }
        public async Task ExecuteAsync(QueueTranslationCommand command)
        {
            // TODO(matt): get this from blob storage
            var content = "";
            var fileStream = File.Open("", FileMode.Open);

            using (var extractor = TextExtractor.Open(fileStream))
            {
                content = await extractor.ExtractAsync();
            }

            // Need to enqueue using hangfire as azure service bus has a peek lock of 5 mins and this could be a long running command.
            switch (command.TranslationType)
            {
                case TranslationType.Google:
                    BackgroundJob.Enqueue(() => _commandDispatcher.DispatchAsync(new StartGoogleTranslationCommand(
                            command.AggregateId,
                            command.CorrelationId,
                            command.Version + 1,
                            command.UserId,
                            content
                        ))
                    );
                    break;
                case TranslationType.AWS:
                    throw new NotImplementedException();
                case TranslationType.Microsoft:
                    throw new NotImplementedException();
            }

            var @event = new TranslationQueued(
                aggregateId: command.AggregateId,
                version: command.Version
            );

            @event.UpdateFrom(command);

            await _eventBus.PublishAsync(@event);
        }
    }
}
