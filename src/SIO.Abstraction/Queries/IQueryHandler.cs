using System.Threading.Tasks;

namespace SIO.Abstraction.Queries
{
    public interface IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
        where TResult : IQueryResult
    {
        Task<TResult> RetrieveAsync(TQuery query);
    }
}
