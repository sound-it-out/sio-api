using System;
using System.Collections.Generic;
using SIO.Domain.Translations.Services;
using SIO.Infrastructure.Queries;

namespace SIO.Domain.Translations.Queries
{
    public class GetTranslationOptionsQueryResult : IQueryResult
    {
        public IEnumerable<TranslationOption> TranslationOptions { get; }

        public GetTranslationOptionsQueryResult(IEnumerable<TranslationOption> translationOptions)
        {
            if (translationOptions == null)
                throw new ArgumentNullException(nameof(translationOptions));

            TranslationOptions = translationOptions;
        }
    }
}
