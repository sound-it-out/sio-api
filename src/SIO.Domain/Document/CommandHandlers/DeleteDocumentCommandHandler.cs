using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OpenEventSourcing.Commands;
using OpenEventSourcing.Domain;
using OpenEventSourcing.Events;
using SIO.Domain.Document.Commands;
using SIO.Infrastructure.Files;

namespace SIO.Domain.Document.CommandHandlers
{
    internal class DeleteDocumentCommandHandler : ICommandHandler<DeleteDocumentCommand>
    {
        private readonly IEventBusPublisher _eventBusPublisher;
        private readonly IAggregateRepository _aggregateRepository;
        private readonly IAggregateFactory _aggregateFactory;
        private readonly IFileClient _fileClient;

        public DeleteDocumentCommandHandler(IEventBusPublisher eventBusPublisher, 
            IAggregateRepository aggregateRepository, 
            IAggregateFactory aggregateFactory,
            IFileClient fileClient)
        {
            if (eventBusPublisher == null)
                throw new ArgumentNullException(nameof(eventBusPublisher));
            if (aggregateRepository == null)
                throw new ArgumentNullException(nameof(aggregateRepository));
            if (aggregateFactory == null)
                throw new ArgumentNullException(nameof(aggregateFactory));
            if (fileClient == null)
                throw new ArgumentNullException(nameof(fileClient));

            _eventBusPublisher = eventBusPublisher;
            _aggregateRepository = aggregateRepository;
            _aggregateFactory = aggregateFactory;
            _fileClient = fileClient;
        }

        public async Task ExecuteAsync(DeleteDocumentCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var aggregate = await _aggregateRepository.GetAsync<Document, DocumentState>(command.AggregateId);

            await _fileClient.DeleteAsync(
                fileName: $"{command.AggregateId}{Path.GetExtension(aggregate.GetState().FileName)}",
                userId: command.UserId
            );

            if(aggregate.GetState().TranslationId.HasValue)
            {
                await _fileClient.DeleteAsync(
                    fileName: $"{aggregate.GetState().TranslationId}.mp3",
                    userId: command.UserId
                );
            }

            aggregate.Delete(
                aggregateId: command.AggregateId,
                version: aggregate.Version.Value
            );

            var events = aggregate.GetUncommittedEvents();

            foreach (var @event in events)
                @event.UpdateFrom(command);

            events = events.ToList();

            await _aggregateRepository.SaveAsync(aggregate, command.Version);
            await _eventBusPublisher.PublishAsync(events);
        }
    }
}
