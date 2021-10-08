using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SIO.Domain.Documents.Events;
using SIO.Domain.Translations.Events;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.Documents.Projections.Managers
{
    internal sealed class DocumentProjectionManager : ProjectionManager<Document>
    {
        private readonly IEnumerable<IProjectionWriter<Document>> _projectionWriters;

        public DocumentProjectionManager(ILogger<ProjectionManager<Document>> logger,
            IEnumerable<IProjectionWriter<Document>> projectionWriters) : base(logger)
        {
            if (projectionWriters == null)
                throw new ArgumentNullException(nameof(projectionWriters));

            _projectionWriters = projectionWriters;

            Handle<DocumentUploaded>(HandleAsync);
            Handle<TranslationQueued>(HandleAsync);
            Handle<TranslationStarted>(HandleAsync);
            Handle<TranslationSucceded>(HandleAsync);
            Handle<TranslationFailed>(HandleAsync);
            Handle<TranslationCharactersProcessed>(HandleAsync);
            Handle<DocumentDeleted>(HandleAsync);
        }

        private async Task HandleAsync(DocumentUploaded @event, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(DocumentProjectionManager)}.{nameof(HandleAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.AddAsync(@event.Subject, () => new Document 
            {
                Id = @event.Subject, 
                TranslationType = @event.TranslationType,
                FileName = @event.FileName,
                Version = 1
            }, cancellationToken)));
        }

        private async Task HandleAsync(TranslationQueued @event, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(DocumentProjectionManager)}.{nameof(HandleAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.UpdateAsync(@event.DocumentSubject, document =>
            {
                document.TranslationSubject = @event.Subject;
                document.TranslationProgress = TranslationProgress.Queued;
                document.Version = @event.Version;
            }, cancellationToken)));
        }

        private async Task HandleAsync(TranslationStarted @event, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(DocumentProjectionManager)}.{nameof(HandleAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.UpdateAsync(@event.DocumentSubject, document =>
            {
                document.TranslationProgress = TranslationProgress.Started;
                document.TotalCharacters = @event.CharacterCount;
                document.Version = @event.Version;
            }, cancellationToken)));
        }

        private async Task HandleAsync(TranslationSucceded @event, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(DocumentProjectionManager)}.{nameof(HandleAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.UpdateAsync(@event.DocumentSubject, document =>
            {
                document.TranslationProgress = TranslationProgress.Completed;
                document.Version = @event.Version;
            }, cancellationToken)));
        }

        private async Task HandleAsync(TranslationFailed @event, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(DocumentProjectionManager)}.{nameof(HandleAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.UpdateAsync(@event.DocumentSubject, document =>
            {
                document.TranslationProgress = TranslationProgress.Failed;
                document.Version = @event.Version;
            }, cancellationToken)));
        }

        private async Task HandleAsync(TranslationCharactersProcessed @event, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(DocumentProjectionManager)}.{nameof(HandleAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.UpdateAsync(@event.DocumentSubject, document =>
            {
                document.TranslationProgress = TranslationProgress.Completed;
                document.CharactersProcessed += @event.CharactersProcessed; 
                document.Version = @event.Version;
            }, cancellationToken)));
        }

        private async Task HandleAsync(DocumentDeleted @event, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(DocumentProjectionManager)}.{nameof(HandleAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.RemoveAsync(@event.Subject, cancellationToken)));
        }

        public override async Task ResetAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(DocumentAuditProjectionManager)}.{nameof(ResetAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.ResetAsync(cancellationToken)));
        }
    }
}
