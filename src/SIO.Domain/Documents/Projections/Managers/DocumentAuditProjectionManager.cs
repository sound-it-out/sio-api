using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SIO.Domain.Audits.Projections;
using SIO.Infrastructure;
using SIO.Infrastructure.Projections;
using SIO.IntegrationEvents.Documents;

namespace SIO.Domain.Documents.Projections.Managers
{
    internal sealed class DocumentAuditProjectionManager : ProjectionManager<DocumentAudit>, IProjectionManager<DocumentAudit>
    {
        private readonly IEnumerable<IProjectionWriter<Audit>> _projectionWriters;
        public DocumentAuditProjectionManager(ILogger<ProjectionManager<DocumentAudit>> logger,
            IEnumerable<IProjectionWriter<Audit>> projectionWriters) : base(logger)
        {
            if (projectionWriters == null)
                throw new ArgumentNullException(nameof(Exception));

            _projectionWriters = projectionWriters;

            Handle<DocumentUploaded>(HandleAsync);
            Handle<DocumentDeleted>(HandleAsync);
        }

        private async Task HandleAsync(DocumentUploaded @event, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(DocumentProjectionManager)}.{nameof(HandleAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.AddAsync(@event.Subject, () => new Audit
            {
                Subject = @event.Subject,
                Events = new AuditEvent[]
                {
                    new AuditEvent 
                    {
                        Id = Subject.New(),
                        TimeStamp = @event.Timestamp, 
                        User = @event.User, 
                        Message = "Document uploaded" 
                    }
                }
            }, cancellationToken)));
        }

        private async Task HandleAsync(DocumentDeleted @event, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(DocumentProjectionManager)}.{nameof(HandleAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.UpdateAsync(@event.Subject, audit =>
            {
                var events = audit.Events.ToList();
                events.Add(new AuditEvent
                {
                    Id = Subject.New(),
                    TimeStamp = @event.Timestamp,
                    User = @event.User,
                    Message = "Document removed"
                });
                audit.Events = events;
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
