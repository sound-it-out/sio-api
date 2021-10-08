using SIO.Domain.Documents.Events;

namespace SIO.Domain.Translations.Services
{
    public sealed record TranslationOption(string Subject, TranslationType TranslationType);
}
