using System;
using System.IO;
using System.Threading.Tasks;
using Clipboard;
using OpenEventSourcing.Commands;
using OpenEventSourcing.Events;
using SIO.Domain.Translations.Commands;

namespace SIO.Domain.Translations.CommandHandlers
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

            switch (command.TranslationType)
            {
                case TranslationType.Google:
                    await _commandDispatcher.DispatchAsync(new GenerateGoogleTranslationCommand(
                        aggregateId: command.AggregateId,
                        correlationId: command.CorrelationId,
                        version: command.Version,
                        userId: command.UserId,
                        content: content
                    ));
                    break;
                case TranslationType.AWS:
                    throw new NotImplementedException();
                case TranslationType.Microsoft:
                    throw new NotImplementedException();
            }
        }
    }
}
