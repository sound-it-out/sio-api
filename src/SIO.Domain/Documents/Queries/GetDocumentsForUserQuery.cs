using SIO.Infrastructure;
using SIO.Infrastructure.Queries;

namespace SIO.Domain.Documents.Queries
{
    public class GetDocumentsForUserQuery : Query<GetDocumentsForUserQueryResult>
    {
        public int Page { get; }
        public int PageSize { get; set; }

        public GetDocumentsForUserQuery(CorrelationId? correlationId, Actor actor, int page, int pageSize) : base(correlationId, actor)
        {
            Page = page;
            PageSize = pageSize;
        }
    }
}
