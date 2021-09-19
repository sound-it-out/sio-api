namespace SIO.Domain.Translations.Events
{
    public class TranslationSucceded : TranslationEvent
    {
        public TranslationSucceded(string subject, int version, string documentSubject) : base(subject, version, documentSubject)
        {
        }
    }
}
