using System.Collections.Generic;
using OpenEventSourcing.Queries;

namespace SIO.Domain.Projections.UserDocuments.Queries
{
    public class UserDocumentsQueryResult : IQueryResult
    {
        public IEnumerable<UserDocument> UserDocuments { get; }

        public UserDocumentsQueryResult(IEnumerable<UserDocument> userDocuments)
        {
            UserDocuments = userDocuments;
        }
    }
}
