using System;
using OpenEventSourcing.Queries;

namespace SIO.Domain.Projections.UsersDocuments.Queries
{
    public class GetDocumentsForUserQuery : Query<UserDocumentsQueryResult>
    {
        public Guid QueriedUserId { get; }
        public int Page { get; }
        public int PageSize { get; set; }

        public GetDocumentsForUserQuery(Guid correlationId, string userId, Guid queriedUserId, int page, int pageSize) : base(correlationId, userId)
        {
            QueriedUserId = queriedUserId;
            Page = page;
            PageSize = pageSize;
        }
    }
}
