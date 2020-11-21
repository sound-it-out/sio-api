using System;
using OpenEventSourcing.Queries;

namespace SIO.Domain.Projections.Users.Queries
{
    public class GetUserByIdQuery : Query<UserQueryResult>
    {
        public Guid AggregateId { get; }
        public GetUserByIdQuery(Guid correlationId, string userId, Guid aggregateId) : base(correlationId, userId)
        {
            AggregateId = aggregateId;
        }
    }
}
