using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SIO.Domain.Translations.Services
{
    public interface ITranslationOptionsRetriever
    {
        Task<IEnumerable<TranslationOption>> GetTranslationsAsync(CancellationToken cancellationToken = default);
    }
}
