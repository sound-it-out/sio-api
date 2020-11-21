using System;
using SIO.Domain.Documents;

namespace SIO.API.V1.Document.Responses
{
    public class UserDocumentResponse
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public DocumentCondition Condition { get; set; }
        public int Version { get; set; }
    }
}
