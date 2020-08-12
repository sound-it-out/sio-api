using System;
using System.Collections.Generic;
using System.Text;
using SIO.Domain.Document;

namespace SIO.Domain.Projections.UserDocument
{
    public class UserDocumentData
    {
        public string FileName { get; set; }
        public DocumentCondition Condition { get; set; }
    }
}
