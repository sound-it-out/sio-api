using System;
using SIO.Domain.Document;

namespace SIO.Domain.Projections.UserDocuments
{
    public class UserDocument
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public DocumentCondition Condition { get; set; }
    }
}
