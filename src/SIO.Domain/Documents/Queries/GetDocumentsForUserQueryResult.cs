using System.Collections.Generic;
using SIO.Domain.Documents.Projections;
using SIO.Infrastructure.Queries;

namespace SIO.Domain.Documents.Queries
{
    public class GetDocumentsForUserQueryResult : IQueryResult
    {
        public IEnumerable<UserDocument> Documents { get; }

        public GetDocumentsForUserQueryResult(IEnumerable<UserDocument> documents)
        {
            Documents = documents;
        }
    }
}
