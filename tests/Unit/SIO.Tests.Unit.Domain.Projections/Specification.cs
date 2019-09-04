using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using OpenEventSourcing.EntityFrameworkCore.DbContexts;
using OpenEventSourcing.EntityFrameworkCore.InMemory;
using OpenEventSourcing.Events;
using OpenEventSourcing.Extensions;
using OpenEventSourcing.Projections;

namespace SIO.Tests.Unit.Domain.Projections
{
    public abstract class Specification<TProjection>
            where TProjection : IProjection
    {
        private readonly IServiceProvider _serviceProvider;

        protected TProjection Projection { get; }
        protected OpenEventSourcingProjectionDbContext Context { get; }

        protected abstract IEnumerable<IEvent> Given();
        //protected abstract void When();

        protected Specification()
        {
            var services = new ServiceCollection();

            services.AddOpenEventSourcing()
                    .AddEntityFrameworkCoreInMemory()
                    .Services.Scan(scan =>
                    {
                        scan.FromApplicationDependencies()
                            .AddClasses(classes => classes.AssignableTo(typeof(IProjection)))
                            .AsSelf()
                            .AsImplementedInterfaces()
                            .WithScopedLifetime()

                            .AddClasses(classes => classes.AssignableTo(typeof(IProjector<>)))
                            .AsImplementedInterfaces()
                            .WithScopedLifetime();
                    });

            _serviceProvider = services.BuildServiceProvider();

            Projection = _serviceProvider.GetRequiredService<TProjection>();
            Context = _serviceProvider.GetRequiredService<IProjectionDbContextFactory>().Create();

            var events = Given().ToList();

            foreach (var @event in @events)
            {
                Projection.HandleAsync(@event).GetAwaiter().GetResult();
            }
        }
    }
}
