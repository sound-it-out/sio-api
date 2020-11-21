using System;
using System.Collections.Generic;
using System.Text;

namespace SIO.Domain.Projections.UsersDocuments
{
    public class UserDocument
    {
        public Guid DocumentId { get; set; }
        public Guid UserId { get; set; }
        public UserDocumentData Data { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? LastModifiedDate { get; set; }
        public int Version { get; set; }
    }
}
