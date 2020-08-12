using System.Data.Common;

namespace SIO.Infrastructure
{
    public interface IDbConnectionFactory
    {
        DbConnection CreateProjecitonConnection();
        DbConnection CreateStoreConnection();
        DbConnection CreateApiConnection();
    }
}
