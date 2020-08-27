using Microsoft.Extensions.DependencyInjection;

namespace SIO.Infrastructure.Extensions
{
    public static class SIOInfrastructureBuilderExtensions
    {
        public static ISIOInfrastructureBuilder AddSqlConnections(this ISIOInfrastructureBuilder builder)
        {
            builder.Services.AddTransient<IDbConnectionFactory, SqlConnectionFactory>();

            return builder;
        }
    }
}
