using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SIO.Domain.Documents.Aggregates;
using SIO.Domain.Documents.Commands;
using SIO.Infrastructure;
using SIO.Infrastructure.Azure.Storage;
using SIO.Infrastructure.Commands;
using SIO.Infrastructure.Domain;
using SIO.Infrastructure.Events;
using SIO.Infrastructure.Files;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIO.Domain.Documents.CommandHandlers
{
    internal class UploadDocumentCommandHandler : ICommandHandler<UploadDocumentCommand>
    {
        private readonly ILogger<UploadDocumentCommandHandler> _logger;
        private readonly IEventBusPublisher _eventBusPublisher;
        private readonly IAggregateRepository _aggregateRepository;
        private readonly IAggregateFactory _aggregateFactory;
        private readonly IFileClient _fileClient;

        public UploadDocumentCommandHandler(ILogger<UploadDocumentCommandHandler> logger,
            IEventBusPublisher eventBusPublisher,
            IAggregateRepository aggregateRepository,
            IAggregateFactory aggregateFactory,
            IFileClient fileClient)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (eventBusPublisher == null)
                throw new ArgumentNullException(nameof(eventBusPublisher));
            if (aggregateRepository == null)
                throw new ArgumentNullException(nameof(aggregateRepository));
            if (aggregateFactory == null)
                throw new ArgumentNullException(nameof(aggregateFactory));
            if (fileClient == null)
                throw new ArgumentNullException(nameof(fileClient));

            _logger = logger;
            _eventBusPublisher = eventBusPublisher;
            _aggregateRepository = aggregateRepository;
            _aggregateFactory = aggregateFactory;
            _fileClient = fileClient;
        }
        public async Task ExecuteAsync(UploadDocumentCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(UploadDocumentCommandHandler)}.{nameof(ExecuteAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            using (var stream = command.File.OpenReadStream())
            {
                await _fileClient.UploadAsync(
                    fileName: $"{command.Subject}{Path.GetExtension(command.File.FileName)}",
                    userId: command.Actor,
                    stream: stream,
                    cancellationToken: cancellationToken
                );
            }

            var aggregate = _aggregateFactory.FromHistory<Document, DocumentState>(Enumerable.Empty<IEvent>());

            if (aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));

            aggregate.Upload(
                subject: command.Subject,
                user: command.Actor,
                translationType: command.TranslationType,
                fileName: command.File.FileName
            );

            var events = aggregate.GetUncommittedEvents();

            events = events.ToArray();

            await _aggregateRepository.SaveAsync(aggregate, command, 0, cancellationToken);
            await _eventBusPublisher.PublishAsync(
                events.Select(@event => 
                    new EventNotification<IEvent>(
                        aggregate.Id,
                        @event,
                        command.CorrelationId,
                        CausationId.From(command.Id),
                        @event.Timestamp, command.Actor)
                    )
                , cancellationToken);
        }
    }
}
