using SIO.Infrastructure.Events;

namespace SIO.Domain.Translations.Events
{
    public abstract class TranslationEvent : Event
    {
        public string DocumentSubject { get; set; }

        public TranslationEvent(string subject, int version, string documentSubject) : base(subject, version)
        {
            DocumentSubject = documentSubject;
        }
    }
}
