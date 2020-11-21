using System;
using System.Collections.Generic;
using System.Text;
using SIO.Domain.Documents;

namespace SIO.Domain.Projections.UsersDocuments
{
    public class UserDocumentData
    {
        public string FileName { get; set; }
        public DocumentCondition Condition { get; set; }
    }
}
