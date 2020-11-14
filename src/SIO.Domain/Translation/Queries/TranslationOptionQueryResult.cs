using System;
using System.Collections.Generic;
using OpenEventSourcing.Queries;
using SIO.Infrastructure.Translations;

namespace SIO.Domain.Translation.Queries
{
    public class TranslationOptionQueryResult : IQueryResult
    {
        public IEnumerable<TranslationOption> TranslationOptions { get; }

        public TranslationOptionQueryResult(IEnumerable<TranslationOption> translationOptions)
        {
            if (translationOptions == null)
                throw new ArgumentNullException(nameof(translationOptions));

            TranslationOptions = translationOptions;
        }
    }
}
