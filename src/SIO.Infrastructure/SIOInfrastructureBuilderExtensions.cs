using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace SIO.Infrastructure
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
