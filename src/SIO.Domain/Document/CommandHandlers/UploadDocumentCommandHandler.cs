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
    internal class UploadDocumentCommandHandler : ICommandHandler<UploadDocumentCommand>
    {
        private readonly IEventBusPublisher _eventBusPublisher;
        private readonly IAggregateRepository _aggregateRepository;
        private readonly IAggregateFactory _aggregateFactory;
        private readonly IFileClient _fileClient;

        public UploadDocumentCommandHandler(IEventBusPublisher eventBusPublisher, 
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

        public async Task ExecuteAsync(UploadDocumentCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            using(var stream = command.File.OpenReadStream())
            {
                await _fileClient.UploadAsync(
                    fileName: $"{command.AggregateId}{Path.GetExtension(command.File.FileName)}",
                    userId: command.UserId,
                    stream: stream
                );
            }            

            var aggregate = _aggregateFactory.FromHistory<Document, DocumentState>(Enumerable.Empty<IEvent>());

            if(aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));

            aggregate.Upload(
                aggregateId: command.AggregateId,
                userId: new Guid(command.UserId),
                translationOption: command.TranslationOption, 
                fileName: command.File.FileName
            );

            var events = aggregate.GetUncommittedEvents();

            foreach (var @event in events)
                @event.UpdateFrom(command);

            events = events.ToList();

            await _aggregateRepository.SaveAsync<DocumentState>(aggregate, 0);
            await _eventBusPublisher.PublishAsync(events);
        }
    }
}
