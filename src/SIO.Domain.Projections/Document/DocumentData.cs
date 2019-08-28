using SIO.Domain.Document;

namespace SIO.Domain.Projections.Document
{
    public class DocumentData
    {        
        public DocumentCondition Condition { get; set; }
        public string FileName { get; set; }
        public long TranslationCharactersProcessed { get; set; }
        public long TranslationCharactersTotal { get; set; }
    }
}
