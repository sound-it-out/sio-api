namespace SIO.Infrastructure.Translations
{
    public class TranslationOption
    {
        public string Subject { get; set; }
        public TranslationType Type { get; set; }

        public TranslationOption()
        {

        }

        public TranslationOption(string subject, TranslationType type)
        {
            Subject = subject;
            Type = type;
        }
    }
}
