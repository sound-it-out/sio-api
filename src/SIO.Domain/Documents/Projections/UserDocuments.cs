using System.Collections.Generic;
using MessagePack;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.Documents.Projections
{
    public class UserDocuments : IProjection
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public IEnumerable<UserDocument> Documents { get; set; }
    }
}
