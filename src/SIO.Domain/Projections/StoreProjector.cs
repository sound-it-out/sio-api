using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SIO.Infrastructure.EntityFrameworkCore.DbContexts;
using SIO.Infrastructure.EntityFrameworkCore.Entities;
using SIO.Infrastructure.Events;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.Processes
{
    internal sealed class StoreProjector<TProjection> : IProjector<TProjection>
        where TProjection : class, IProjection
    {
        private Task _executingTask;
        private CancellationTokenSource StoppingCts { get; set; }
        private readonly IProjectionManager<TProjection> _projectionManager;
        private readonly IServiceScope _scope;
        private readonly IEventStore<SIOStoreDbContext> _eventStore;
        private readonly ILogger<StoreProjector<TProjection>> _logger;
        private readonly IOptionsSnapshot<StoreProjectorOptions> _options;
        private readonly ISIOProjectionDbContextFactory _projectionDbContextFactory;
        private readonly string _name;

        public StoreProjector(IServiceScopeFactory serviceScopeFactory,
            ILogger<StoreProjector<TProjection>> logger)
        {
            if (serviceScopeFactory == null)
                throw new ArgumentNullException(nameof(serviceScopeFactory));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _scope = serviceScopeFactory.CreateScope();
            _logger = logger;

            _projectionManager = _scope.ServiceProvider.GetRequiredService<IProjectionManager<TProjection>>();
            _eventStore = _scope.ServiceProvider.GetRequiredService<IEventStore<SIOStoreDbContext>>();
            _options = _scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<StoreProjectorOptions>>();
            _projectionDbContextFactory = _scope.ServiceProvider.GetRequiredService<ISIOProjectionDbContextFactory>();

            _name = typeof(TProjection).FullName;
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(StoreProjector<TProjection>)}.{nameof(StartAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            _logger.LogInformation($"{nameof(StoreProjector<TProjection>)} starting");
            StoppingCts = new();

            _executingTask = ExecuteAsync(StoppingCts.Token);

            _logger.LogInformation($"{nameof(StoreProjector<TProjection>)} started");

            if (_executingTask.IsCompleted)
                return _executingTask;

            return Task.CompletedTask;
        }
        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(StoreProjector<TProjection>)}.{nameof(StopAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            _logger.LogInformation($"{nameof(StoreProjector<TProjection>)} stopping");

            if (_executingTask == null)
                return;

            try
            {
                StoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
                _logger.LogInformation($"{nameof(StoreProjector<TProjection>)} stopped");
            }
        }

        public async Task ResetAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(StoreProjector<TProjection>)}.{nameof(ResetAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            using (var context = _projectionDbContextFactory.Create())
            {
                var state = await context.ProjectionStates.FindAsync(_name);

                if (state != null)
                {
                    state.Position = 0;
                    state.LastModifiedDate = DateTimeOffset.UtcNow;

                    await context.SaveChangesAsync(cancellationToken);
                }

                await _projectionManager.ResetAsync(cancellationToken);
            }
        }

        private async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(StoreProjector<TProjection>)}.{nameof(ExecuteAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            using (var context = _projectionDbContextFactory.Create())
            {
                var state = await context.ProjectionStates.FindAsync(_name);

                if (state == null)
                {
                    state = new ProjectionState
                    {
                        Name = _name,
                        CreatedDate = DateTimeOffset.UtcNow,
                        Position = 0
                    };

                    context.ProjectionStates.Add(state);

                    await context.SaveChangesAsync(cancellationToken);
                }

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        await context.Entry(state).ReloadAsync();

                        var page = await _eventStore.GetEventsAsync(state.Position);

                        foreach (var @event in page.Events)
                            await _projectionManager.HandleAsync(@event.Payload);

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
                        _logger.LogCritical(ex, $"Process '{typeof(TProjection).Name}' failed at postion '{state.Position}' due to an unexpected error. See exception details for more information.");
                        break;
                    }
                }
            }
        }

        public void Dispose()
        {
            StoppingCts.Cancel();
        }
    }
}
