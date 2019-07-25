using System;
using System.Collections.Generic;
using System.Text;
using SIO.Domain.Document;

namespace SIO.Domain.Projections.Document
{
    public class DocumentData
    {
        public DocumentCondition Condition { get; set; }
        public string FilePath { get; set; }
        public string TranslationPath { get; set; }
    }
}
