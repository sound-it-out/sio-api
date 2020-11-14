using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIO.Infrastructure.Translations
{
    public interface ITranslationOptionsRetriever
    {
        Task<IEnumerable<TranslationOption>> RetrieveAsync();
    }
}
