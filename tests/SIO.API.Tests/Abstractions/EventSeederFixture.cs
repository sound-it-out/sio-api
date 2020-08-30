using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenEventSourcing.EntityFrameworkCore;
using OpenEventSourcing.EntityFrameworkCore.DbContexts;
using OpenEventSourcing.Events;

namespace SIO.API.Tests.Abstractions
{
    public class EventSeederFixture : IEventSeeder
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private Task _seedTask;
        private readonly object _lockObj = new object();

        private IServiceProvider _serviceProvider;
        public void Init(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public async Task SeedAsync(params IEvent[] events)
        {
            lock (_lockObj)
            {
                if (_seedTask == null)
                {
                    _seedTask = Task.Run(async () => await InternalSeedAsync(events), _cts.Token);
                }
            }

            await _seedTask;
        }

        private async Task InternalSeedAsync(params IEvent[] events)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory>();
                var eventModelFactory = scope.ServiceProvider.GetRequiredService<IEventModelFactory>();


                using (var context = dbContextFactory.Create())
                {
                    foreach (var @event in events)
                        await context.Events.AddAsync(eventModelFactory.Create(@event));

                    await context.SaveChangesAsync();
                }

                // Wait for polling projections to finish
                await Task.Delay(2000);
            }
        }
    }
}
