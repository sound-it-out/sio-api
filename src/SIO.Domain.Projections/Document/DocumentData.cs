using SIO.Domain.Document;

namespace SIO.Domain.Projections.Document
{
    public class DocumentData
    {
        public DocumentCondition Condition { get; set; }
        public long TranslationCharactersTotal { get; set; }
        public long TranslationCharactersProcessed { get; set; }
        public string FilePath { get; set; }
        public string TranslationPath { get; set; }
    }
}
