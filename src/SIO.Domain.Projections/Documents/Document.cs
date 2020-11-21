using System;

namespace SIO.Domain.Projections.Documents
{
    public class Document
    {
        public Guid Id { get; set; }
        public Guid? TranslationId { get; set; }
        public DocumentData Data { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? LastModifiedDate { get; set; }
        public int Version { get; set; }
    }
}
