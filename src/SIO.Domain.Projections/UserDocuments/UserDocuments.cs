using System;

namespace SIO.Domain.Projections.UserDocuments
{
    public class UserDocuments
    {
        public Guid UserId { get; set; }
        public UserDocumentsData Data { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? LastModifiedDate { get; set; }
        public int Version { get; set; }
    }
}
