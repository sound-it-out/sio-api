using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenEventSourcing.EntityFrameworkCore.DbContexts;
using OpenEventSourcing.EntityFrameworkCore.Entities;
using OpenEventSourcing.Events;
using OpenEventSourcing.Projections;

namespace SIO.Domain.Projections
{
    internal class PollingProjector<TProjection> : BackgroundProjector<TProjection>, IHostedService
       where TProjection : IProjection
    {
        private readonly TProjection _projection;
        private readonly IServiceScope _scope;
        private readonly IEventStore _eventStore;
        private readonly ILogger<PollingProjector<TProjection>> _logger;
        private readonly IOptionsSnapshot<PollingProjectorOptions> _options;
        private readonly IProjectionDbContextFactory _projectionDbContextFactory;

        public PollingProjector(IServiceScopeFactory serviceScopeFactory,
                                ILogger<PollingProjector<TProjection>> logger)
        {
            if (serviceScopeFactory == null)
                throw new ArgumentNullException(nameof(serviceScopeFactory));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _scope = serviceScopeFactory.CreateScope();
            _logger = logger;

            _projection = _scope.ServiceProvider.GetRequiredService<TProjection>();
            _eventStore = _scope.ServiceProvider.GetRequiredService<IEventStore>();            
            _options = _scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<PollingProjectorOptions>>();
            _projectionDbContextFactory = _scope.ServiceProvider.GetRequiredService<IProjectionDbContextFactory>();
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => PollAsync(cancellationToken));
        }

        private async Task PollAsync(CancellationToken cancellationToken)
        {
            var projectionName = _projection.GetType().FullName;
            using (var context = _projectionDbContextFactory.Create())
            {
                var state = context.ProjectionStates.FirstOrDefault(s => s.Name == projectionName);

                if(state == null)
                {
                    state = new ProjectionState
                    {
                        Name = projectionName,
                        CreatedDate = DateTimeOffset.UtcNow,
                        Position = 0
                    };

                    context.ProjectionStates.Add(state);

                    await context.SaveChangesAsync();
                }                

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var page = await _eventStore.GetEventsAsync(state.Position);
                        var projection = _scope.ServiceProvider.GetRequiredService<TProjection>();

                        foreach (var @event in page.Events)
                            await _projection.HandleAsync(@event);

                        if (state.Position == page.Offset)
                        {
                            await Task.Delay(_options.Value.Interval);
                        }                            
                        else
                        {
                            state.Position = page.Offset;
                            state.LastModifiedDate = DateTimeOffset.UtcNow;

                            await context.SaveChangesAsync();
                        }                        
                    }
                    catch (Exception ex)
                    {
                        _logger.LogCritical(ex, $"Projection '{typeof(TProjection).Name}' failed at postion '{state.Position}' due to an unexpected error. See exception details for more information.");
                        break;
                    }
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _scope.Dispose();

            return base.StopAsync(cancellationToken);
        }
    }
}
