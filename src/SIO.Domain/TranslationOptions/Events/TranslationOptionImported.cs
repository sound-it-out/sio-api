using SIO.Domain.Documents.Events;
using SIO.Infrastructure.Events;

namespace SIO.Domain.TranslationOptions.Events
{
    public class TranslationOptionImported : Event
    {
        public TranslationType TranslationType { get; set; }

        public TranslationOptionImported(string subject, int version, TranslationType translationType) : base(subject, version)
        {
            TranslationType = translationType;
        }
    }
}
