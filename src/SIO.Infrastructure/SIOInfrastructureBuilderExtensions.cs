using Microsoft.Extensions.DependencyInjection;
using SIO.Infrastructure.File;
using SIO.Infrastructure.File.Local;

namespace SIO.Infrastructure
{
    public static class SIOInfrastructureBuilderExtensions
    {
        public static ISIOInfrastructureBuilder AddSqlConnections(this ISIOInfrastructureBuilder builder)
        {
            builder.Services.AddTransient<IDbConnectionFactory, SqlConnectionFactory>();

            return builder;
        }

        public static ISIOInfrastructureBuilder AddLocalFileStorage(this ISIOInfrastructureBuilder builder)
        {
            builder.Services.AddTransient<IFileClient, LocalFileClient>();

            return builder;
        }
    }
}
