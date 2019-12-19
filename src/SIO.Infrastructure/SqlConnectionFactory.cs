using System;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SIO.Infrastructure
{
    internal class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _configuration = configuration;
        }

        public DbConnection CreateApiConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("Api"));
        }

        public DbConnection CreateProjecitonConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("Projection"));
        }

        public DbConnection CreateStoreConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("Store"));
        }
    }
}
