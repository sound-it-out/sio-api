using System.Collections.Generic;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.Documents.Projections
{
    public class UserDocuments : IProjection
    {
        public string UserId { get; set; }
        public IEnumerable<UserDocument> Documents { get; set; }
    }
}
