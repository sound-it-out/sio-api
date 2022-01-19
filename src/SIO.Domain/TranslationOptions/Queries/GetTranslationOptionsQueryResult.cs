using System;
using System.Collections.Generic;
using SIO.Domain.TranslationOptions.Projections;
using SIO.Infrastructure.Queries;

namespace SIO.Domain.TranslationOptions.Queries
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
