namespace SIO.Domain.Translations.Events
{
    public class TranslationQueued : TranslationEvent
    {
        public TranslationQueued(string subject, int version, string documentSubject) : base(subject, version, documentSubject)
        {
        }
    }
}
