using SIO.Domain.Documents;

namespace SIO.Domain.Projections.Documents
{
    public class DocumentData
    {        
        public DocumentCondition Condition { get; set; }
        public string FileName { get; set; }
        public long TranslationCharactersProcessed { get; set; }
        public long TranslationCharactersTotal { get; set; }
    }
}
