using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SIO.Infrastructure.Events;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.Projections
{
    internal sealed class InMemoryProjector<TView> : IInMemoryProjector<TView>
        where TView : class, IProjection
    {
        private readonly IEventStore _eventStore;
        private readonly IProjectionManagerFactory<TView> _projectionManagerFactory;
        private readonly ILogger<InMemoryProjector<TView>> _logger;

        public InMemoryProjector(IEventStore eventStore,
            IProjectionManagerFactory<TView> projectionManagerFactory,
            ILogger<InMemoryProjector<TView>> logger)
        {
            if (eventStore == null)
                throw new ArgumentNullException(nameof(eventStore));
            if (projectionManagerFactory == null)
                throw new ArgumentNullException(nameof(projectionManagerFactory));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _eventStore = eventStore;
            _projectionManagerFactory = projectionManagerFactory;
            _logger = logger;
        }

        public async Task<TView> ApplyAsync(TView view, IEvent @event, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(InMemoryProjector<TView>)}.{nameof(ApplyAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            var subject = Guid.NewGuid().ToString();
            var writer = await InMemoryProjectionWriter<TView>.BuildAsync(subject, view, cancellationToken);

            var manager = _projectionManagerFactory.Create(writer);
            await manager.HandleAsync(@event, cancellationToken);

            return await writer.RetrieveAsync(subject, cancellationToken);
        }

        public async Task<TView> ProjectAsync(string subject, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(InMemoryProjector<TView>)}.{nameof(ProjectAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            return await ProjectAsync(subject, DateTimeOffset.UtcNow, cancellationToken);
        }

        public async Task<TView> ProjectAsync(string subject, DateTimeOffset timestamp, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(InMemoryProjector<TView>)}.{nameof(ProjectAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            var events = await _eventStore.GetEventsAsync(StreamId.From(subject), timestamp, cancellationToken);

            var writer = await InMemoryProjectionWriter<TView>.BuildAsync(cancellationToken);

            var manager = _projectionManagerFactory.Create(writer);

            foreach(var @event in events)
                await manager.HandleAsync(@event.Payload, cancellationToken);

            return await writer.RetrieveAsync(subject, cancellationToken);
        }
    }
}
