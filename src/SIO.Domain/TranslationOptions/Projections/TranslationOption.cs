using SIO.Infrastructure.Projections;
using SIO.IntegrationEvents.Documents;

namespace SIO.Domain.TranslationOptions.Projections
{
    public class TranslationOption : IProjection
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public TranslationType TranslationType {  get; set; }
    }
}
