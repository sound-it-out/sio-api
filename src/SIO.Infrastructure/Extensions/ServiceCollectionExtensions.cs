using System;
using Microsoft.Extensions.DependencyInjection;

namespace SIO.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static ISIOInfrastructureBuilder AddSIOInfrastructure(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            return new SIOInfrastructureBuilder(services);
        }
    }
}
