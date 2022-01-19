using SIO.Infrastructure;
using SIO.Infrastructure.Queries;

namespace SIO.Domain.Documents.Queries
{
    public class GetDocumentStreamQuery : Query<GetDocumentStreamQueryResult>
    {
        public string Subject {  get; set; }

        public GetDocumentStreamQuery(CorrelationId? correlationId, Actor actor, string subject) : base(correlationId, actor)
        {
            Subject = subject;
        }
    }
}
