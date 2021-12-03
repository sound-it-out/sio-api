using SIO.Domain.Documents.Events;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.TranslationOptions.Projections
{
    public class TranslationOption : IProjection
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public TranslationType TranslationType {  get; set; }
    }
}
