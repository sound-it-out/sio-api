using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SIO.Domain.Documents.Events;
using SIO.Domain.User.Events;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.Documents.Projections.Managers
{
    internal sealed class UserDocumentsProjectionManager : ProjectionManager<UserDocuments>
    {
        private readonly IEnumerable<IProjectionWriter<UserDocuments>> _projectionWriters;

        public UserDocumentsProjectionManager(ILogger<ProjectionManager<UserDocuments>> logger,
            IEnumerable<IProjectionWriter<UserDocuments>> projectionWriters) : base(logger)
        {
            if (projectionWriters == null)
                throw new ArgumentNullException(nameof(projectionWriters));

            _projectionWriters = projectionWriters;

            Handle<UserVerified>(HandleAsync);
            Handle<DocumentUploaded>(HandleAsync);
            Handle<DocumentDeleted>(HandleAsync);
        }

        private async Task HandleAsync(UserVerified @event, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(DocumentProjectionManager)}.{nameof(HandleAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.AddAsync(@event.Subject, () => new UserDocuments
            {
                Documents = new List<UserDocument>()
            }, cancellationToken)));
        }

        private async Task HandleAsync(DocumentUploaded @event, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(DocumentProjectionManager)}.{nameof(HandleAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.UpdateAsync(@event.User, userDocuments =>
            {
                var documents = userDocuments.Documents.ToList();
                documents.Add(new UserDocument 
                {
                    DocumentId = @event.Subject, 
                    FileName = @event.FileName
                });
                userDocuments.Documents = documents;
            }, cancellationToken)));
        }

        private async Task HandleAsync(DocumentDeleted @event, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(DocumentProjectionManager)}.{nameof(HandleAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.UpdateAsync(@event.User, userDocuments =>
            {
                var documents = userDocuments.Documents.ToList();
                documents.RemoveAll(d => d.DocumentId == @event.Subject);
                userDocuments.Documents = documents;
            }, cancellationToken)));
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
