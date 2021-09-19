using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIO.Infrastructure.Projections;
using SIO.Redis.Cache;
using SIO.Redis.Projections;
using System;

namespace SIO.Redis.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, Action<RedisOptions> options)
        {
            services.AddSingleton<IRedisCache, RedisCache>();
            services.Configure(options);
            services.AddScoped(typeof(IProjectionWriter<>), typeof(RedisProjectionWriter<>));

            return services;
        }
    }
}
