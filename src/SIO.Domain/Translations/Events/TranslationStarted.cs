namespace SIO.Domain.Translations.Events
{
    public class TranslationStarted : TranslationEvent
    {
        public long CharacterCount { get; set; }

        public TranslationStarted(string subject, int version, string documentSubject, long characterCount) : base(subject, version, documentSubject)
        {
            CharacterCount = characterCount;
        }
    }
}
