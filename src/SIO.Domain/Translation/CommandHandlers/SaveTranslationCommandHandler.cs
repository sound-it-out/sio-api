using System;
using System.Linq;
using System.Threading.Tasks;
using OpenEventSourcing.Commands;
using OpenEventSourcing.Domain;
using OpenEventSourcing.Events;
using SIO.Domain.Document;
using SIO.Domain.Translation.Commands;
using SIO.Infrastructure.File;

namespace SIO.Domain.Translation.CommandHandlers
{
    internal class SaveTranslationCommandHandler : ICommandHandler<SaveTranslationCommand>
    {
        private readonly IEventBusPublisher _eventBusPublisher;
        private readonly IAggregateRepository _aggregateRepository;
        private readonly IFileClient _fileClient;

        public SaveTranslationCommandHandler(IEventBusPublisher eventBusPublisher, IAggregateRepository aggregateRepository, IFileClient fileClient)
        {
            if (eventBusPublisher == null)
                throw new ArgumentNullException(nameof(eventBusPublisher));
            if (aggregateRepository == null)
                throw new ArgumentNullException(nameof(aggregateRepository));
            if (fileClient == null)
                throw new ArgumentNullException(nameof(fileClient));

            _eventBusPublisher = eventBusPublisher;
            _aggregateRepository = aggregateRepository;
            _fileClient = fileClient;
        }

        public async Task ExecuteAsync(SaveTranslationCommand command)
        {
            await _fileClient.UploadAsync($"{command.AggregateId}.mp3", command.UserId, command.Stream);

            var aggregate = await _aggregateRepository.GetAsync<Document.Document, DocumentState>(command.CorrelationId);
            aggregate.AcceptTranslation(command.AggregateId, command.Version);

            var events = aggregate.GetUncommittedEvents();

            foreach (var @event in events)
                @event.UpdateFrom(command);

            events = events.ToList();

            await _aggregateRepository.SaveAsync<DocumentState>(aggregate, command.Version);
            await _eventBusPublisher.PublishAsync(events);
        }
    }
}
