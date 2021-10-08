using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SIO.Domain.Translations.Queries;
using SIO.Domain.Translations.Services;
using SIO.Infrastructure.Queries;

namespace SIO.Domain.Translations.QueryHandlers
{
    internal class GetTranslationOptionsQueryHandler : IQueryHandler<GetTranslationOptionsQuery, GetTranslationOptionsQueryResult>
    {
        private readonly ILogger<GetTranslationOptionsQueryHandler> _logger;
        private readonly IMemoryCache _cache;
        private readonly IEnumerable<ITranslationOptionsRetriever> _translationOptionsRetrievers;

        public GetTranslationOptionsQueryHandler(ILogger<GetTranslationOptionsQueryHandler> logger,
            IMemoryCache cache,
            IEnumerable<ITranslationOptionsRetriever> translationOptionsRetrievers)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));
            if (translationOptionsRetrievers == null)
                throw new ArgumentNullException(nameof(translationOptionsRetrievers));

            _logger = logger;
            _cache = cache;
            _translationOptionsRetrievers = translationOptionsRetrievers;
        }

        public async Task<GetTranslationOptionsQueryResult> RetrieveAsync(GetTranslationOptionsQuery query, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(GetTranslationOptionsQueryHandler)}.{nameof(RetrieveAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            var cacheKey = nameof(GetTranslationOptionsQuery);

            if (_cache.TryGetValue(cacheKey, out GetTranslationOptionsQueryResult result))
                return result;

            var translationOptions = new ConcurrentBag<TranslationOption>();

            await Task.WhenAll(_translationOptionsRetrievers.Select(tor => RetrieveOptionsAsync(tor, translationOptions, cancellationToken)));

            result = new GetTranslationOptionsQueryResult(translationOptions.OrderBy(to => to.TranslationType));
            _cache.Set(cacheKey, result);

            return result;
        }

        private async Task RetrieveOptionsAsync(ITranslationOptionsRetriever translationOptionsRetriever, ConcurrentBag<TranslationOption> translationOptions, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(GetTranslationOptionsQueryHandler)}.{nameof(RetrieveOptionsAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            var options = await translationOptionsRetriever.GetTranslationsAsync(cancellationToken);

            foreach (var option in options)
                translationOptions.Add(option);
        }
    }
}
