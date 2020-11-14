using System.Collections.Generic;
using System.Threading.Tasks;
using SIO.Infrastructure.Translations;

namespace SIO.Infrastructure.Local.Translations
{
    internal sealed class LocalTranslationOptionsRetriever : ITranslationOptionsRetriever
    {
        public Task<IEnumerable<TranslationOption>> RetrieveAsync()
        {
            return Task.FromResult(RetrieveVoices());
        }

        private IEnumerable<TranslationOption> RetrieveVoices()
        {
            yield return new TranslationOption("Local", TranslationType.Local);
        }
    }
}
