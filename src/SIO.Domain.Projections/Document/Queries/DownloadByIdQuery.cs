using System;
using OpenEventSourcing.Queries;

namespace SIO.Domain.Projections.Document.Queries
{
    public class DownloadByIdQuery : Query<DownloadByIdQueryResult>
    {
        public Guid AggregateId { get; }
        public DownloadByIdQuery(Guid aggregateId, Guid correlationId, string userId) : base(correlationId, userId)
        {
            AggregateId = aggregateId;
        }
    }
}
