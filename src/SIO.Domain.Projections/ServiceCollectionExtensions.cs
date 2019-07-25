using System;
using Microsoft.Extensions.DependencyInjection;
using OpenEventSourcing.Projections;

namespace SIO.Domain.Projections
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProjections(this IServiceCollection source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            source.Scan(scan =>
            {
                scan.FromApplicationDependencies()
                    .AddClasses(classes => classes.AssignableTo(typeof(IProjection)))
                    .AsSelfWithInterfaces()
                    .WithScopedLifetime();
            });

            source.AddSingleton(typeof(IProjector<>), typeof(PollingProjector<>));

            return source;
        }
    }
}
