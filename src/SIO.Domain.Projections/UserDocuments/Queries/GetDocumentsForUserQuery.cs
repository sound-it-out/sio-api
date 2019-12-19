using System;
using OpenEventSourcing.Queries;

namespace SIO.Domain.Projections.UserDocuments.Queries
{
    public class GetDocumentsForUserQuery : Query<UserDocumentsQueryResult>
    {
        public Guid QueriedUserId { get; set; }
        public GetDocumentsForUserQuery(Guid correlationId, string userId, Guid queriedUserId) : base(correlationId, userId)
        {
            QueriedUserId = queriedUserId;
        }
    }
}
