using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SIO.Domain.Documents.Aggregates;
using SIO.Domain.Documents.Commands;
using SIO.Infrastructure.Commands;
using SIO.Infrastructure.Domain;
using SIO.Infrastructure.Events;
using SIO.Infrastructure.Files;

namespace SIO.Domain.Documents.CommandHandlers
{
    internal class UploadDocumentCommandHandler : ICommandHandler<UploadDocumentCommand>
    {
        private readonly ILogger<UploadDocumentCommandHandler> _logger;
        private readonly IAggregateRepository _aggregateRepository;
        private readonly IAggregateFactory _aggregateFactory;
        private readonly IFileClient _fileClient;

        public UploadDocumentCommandHandler(ILogger<UploadDocumentCommandHandler> logger,
            IAggregateRepository aggregateRepository,
            IAggregateFactory aggregateFactory,
            IFileClient fileClient)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (aggregateRepository == null)
                throw new ArgumentNullException(nameof(aggregateRepository));
            if (aggregateFactory == null)
                throw new ArgumentNullException(nameof(aggregateFactory));
            if (fileClient == null)
                throw new ArgumentNullException(nameof(fileClient));

            _logger = logger;
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

            await _aggregateRepository.SaveAsync(aggregate, command, 0, cancellationToken);
        }
    }
}
