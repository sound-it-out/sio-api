using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenEventSourcing.Events;
using OpenEventSourcing.Projections;

namespace SIO.Domain.Projections
{
    internal class PollingProjector<TProjection> : BackgroundProjector<TProjection>
       where TProjection : IProjection
    {
        private readonly TProjection _projection;
        private readonly IEventStore _eventStore;
        private readonly ILogger<PollingProjector<TProjection>> _logger;
        private readonly IOptionsSnapshot<PollingProjectorOptions> _options;
        private long _offset;

        public PollingProjector(TProjection projection,
                                IOptionsSnapshot<PollingProjectorOptions> options,
                                IEventStore eventStore,
                                ILogger<PollingProjector<TProjection>> logger)
        {
            if (projection == null)
                throw new ArgumentNullException(nameof(projection));
            if (eventStore == null)
                throw new ArgumentNullException(nameof(eventStore));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _projection = projection;
            _eventStore = eventStore;
            _logger = logger;
            _options = options;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => PollAsync(cancellationToken));
        }

        private async Task PollAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var page = await _eventStore.GetEventsAsync(_offset);

                    foreach (var @event in page.Events)
                        await _projection.HandleAsync(@event);

                    if (_offset == page.Offset)
                        await Task.Delay(_options.Value.Interval);

                    _offset = page.Offset;
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, $"Projection '{typeof(TProjection).Name}' failed at postion '{_offset}' due to an unexpected error. See exception details for more information.");
                    break;
                }
            }
        }
    }
}
