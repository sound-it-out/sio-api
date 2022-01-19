using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIO.Api.V1.TranslationOptions.Response;
using SIO.API.V1;
using SIO.Domain.TranslationOptions.Queries;
using SIO.Infrastructure;
using SIO.Infrastructure.Queries;

namespace SIO.Api.V1.Translations
{
    [Authorize]
    public class TranslationOptionController : SIOController
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public TranslationOptionController(IQueryDispatcher queryDispatcher)
        {
            if (queryDispatcher == null)
                throw new ArgumentNullException(nameof(queryDispatcher));

            _queryDispatcher = queryDispatcher;
        }

        [HttpGet()]
        public async Task<IEnumerable<TranslationOptionResponse>> ListOptions(CancellationToken cancellationToken = default)
        {
            var translationOptionsResult = await _queryDispatcher.DispatchAsync(new GetTranslationOptionsQuery(CorrelationId.New(), CurrentActor), cancellationToken);
            return translationOptionsResult.TranslationOptions.Select(to => new TranslationOptionResponse(to.Id, to.Subject, to.TranslationType));
        }
    }
}
