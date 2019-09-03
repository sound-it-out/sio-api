using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Clipboard;
using Hangfire;
using OpenEventSourcing.Commands;
using OpenEventSourcing.Domain;
using OpenEventSourcing.Events;
using SIO.Domain.Document;
using SIO.Domain.Translation.Commands;

namespace SIO.Domain.Translation.CommandHandlers
{
    internal class QueueTranslationCommandHandler : ICommandHandler<QueueTranslationCommand>
    {
        private readonly IEventBus _eventBus;
        private readonly IAggregateRepository _aggregateRepository;
        private readonly ICommandDispatcher _commandDispatcher;

        public QueueTranslationCommandHandler(IEventBus eventBus, IAggregateRepository aggregateRepository, ICommandDispatcher commandDispatcher)
        {
            if (eventBus == null)
                throw new ArgumentNullException(nameof(eventBus));
            if (aggregateRepository == null)
                throw new ArgumentNullException(nameof(aggregateRepository));
            if (commandDispatcher == null)
                throw new ArgumentNullException(nameof(commandDispatcher));

            _eventBus = eventBus;
            _aggregateRepository = aggregateRepository;
            _commandDispatcher = commandDispatcher;
        }
        public async Task ExecuteAsync(QueueTranslationCommand command)
        {
            var aggregate = await _aggregateRepository.GetAsync<Document.Document, DocumentState>(command.CorrelationId);

            aggregate.QueueTranslation(command.AggregateId, command.Version);

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
                            command.UserId
                        ))
                    );
                    break;
                case TranslationType.AWS:
                    throw new NotImplementedException();
                case TranslationType.Microsoft:
                    throw new NotImplementedException();
            }

            var events = aggregate.GetUncommittedEvents();

            foreach (var @event in events)
                @event.UpdateFrom(command);

            events = events.ToList();

            await _aggregateRepository.SaveAsync<DocumentState>(aggregate, command.Version);
            await _eventBus.PublishAsync(events);
        }
    }
}
