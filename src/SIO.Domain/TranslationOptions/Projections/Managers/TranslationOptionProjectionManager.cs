using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SIO.Domain.TranslationOptions.Events;
using SIO.Infrastructure;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.TranslationOptions.Projections.Managers
{
    internal class TranslationOptionProjectionManager : ProjectionManager<TranslationOption>
    {
        private readonly IEnumerable<IProjectionWriter<TranslationOption>> _projectionWriters;

        public TranslationOptionProjectionManager(ILogger<ProjectionManager<TranslationOption>> logger,
            IEnumerable<IProjectionWriter<TranslationOption>> projectionWriters) : base(logger)
        {
            if (projectionWriters == null)
                throw new ArgumentNullException(nameof(projectionWriters));

            _projectionWriters = projectionWriters;

            Handle<TranslationOptionImported>(HandleAsync);
        }

        public async Task HandleAsync(TranslationOptionImported @event, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(TranslationOptionProjectionManager)}.{nameof(HandleAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.AddAsync(@event.Subject, () => new TranslationOption
            {
                Id = Subject.New(),
                TranslationType = @event.TranslationType,
                Subject = @event.Subject
            }, cancellationToken)));
        }

        public override async Task ResetAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(TranslationOptionProjectionManager)}.{nameof(ResetAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.WhenAll(_projectionWriters.Select(pw => pw.ResetAsync(cancellationToken)));
        }
    }
}
