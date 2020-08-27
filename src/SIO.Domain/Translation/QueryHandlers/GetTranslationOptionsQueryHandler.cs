using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenEventSourcing.Queries;
using SIO.Domain.Translation.Queries;
using SIO.Infrastructure.Translations;

namespace SIO.Domain.Translation.QueryHandlers
{
    internal class GetTranslationOptionsQueryHandler : IQueryHandler<GetTranslationOptionsQuery, TranslationOptionQueryResult>
    {
        private readonly IEnumerable<ITranslationOptionsRetriever> _translationOptionsRetrievers;

        public GetTranslationOptionsQueryHandler(IEnumerable<ITranslationOptionsRetriever> translationOptionsRetrievers)
        {
            if (translationOptionsRetrievers == null)
                throw new ArgumentNullException(nameof(translationOptionsRetrievers));

            _translationOptionsRetrievers = translationOptionsRetrievers;
        }

        public async Task<TranslationOptionQueryResult> RetrieveAsync(GetTranslationOptionsQuery query)
        {
            var translationOptions = new ConcurrentBag<TranslationOption>();

            await Task.WhenAll(_translationOptionsRetrievers.Select(tor => RetrieveOptions(tor, translationOptions)));

            return new TranslationOptionQueryResult(translationOptions.OrderBy(to => to.Type));
        }

        private async Task RetrieveOptions(ITranslationOptionsRetriever translationOptionsRetriever, ConcurrentBag<TranslationOption> translationOptions)
        {
            var options = await translationOptionsRetriever.RetrieveAsync();

            foreach (var option in options)
                translationOptions.Add(option);
        }
    }
}
