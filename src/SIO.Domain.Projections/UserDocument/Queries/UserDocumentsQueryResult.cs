using System.Collections.Generic;
using OpenEventSourcing.Queries;

namespace SIO.Domain.Projections.UserDocument.Queries
{
    public class UserDocumentsQueryResult : IQueryResult
    {
        public IEnumerable<UserDocument> Documents { get; }

        public UserDocumentsQueryResult(IEnumerable<UserDocument> documents)
        {
            Documents = documents;
        }
    }
}
