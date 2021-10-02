using MessagePack;
using SIO.Domain.Documents.Events;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.Documents.Projections
{
    public class Document : IProjection
    {
        public string Id { get; set; }
        public TranslationType TranslationType { get; set; }
        public string TranslationSubject { get; set; }
        public string FileName { get; set; }
        public long CharactersProcessed { get; set; }
        public long TotalCharacters { get; set; }
        public TranslationProgress TranslationProgress { get; set; }
        public int Version { get; set; }
    }
}
