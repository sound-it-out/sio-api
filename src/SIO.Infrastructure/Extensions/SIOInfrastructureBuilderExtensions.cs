using Microsoft.Extensions.DependencyInjection;
using SIO.Infrastructure.Events;

namespace SIO.Infrastructure.Extensions
{
    public static class SIOInfrastructureBuilderExtensions
    {
        public static ISIOInfrastructureBuilder AddSqlConnections(this ISIOInfrastructureBuilder builder)
        {
            builder.Services.AddTransient<IDbConnectionFactory, SqlConnectionFactory>();

            return builder;
        }

        public static ISIOInfrastructureBuilder AddEvents(this ISIOInfrastructureBuilder builder)
        {
            builder.Services.AddHostedService<SIOEventConsumer>();

            return builder;
        }
    }
}
