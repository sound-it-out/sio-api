using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIO.API.V1;
using SIO.Domain.Translations.Queries;
using SIO.Domain.Translations.Services;
using SIO.Infrastructure;
using SIO.Infrastructure.Queries;

namespace SIO.Api.V1.Translations
{
    //[Authorize]
    public class TranslationController : SIOController
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public TranslationController(IQueryDispatcher queryDispatcher)
        {
            if (queryDispatcher == null)
                throw new ArgumentNullException(nameof(queryDispatcher));

            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("listoptions")]
        public async Task<IEnumerable<TranslationOption>> ListOptions()
        {
            var translationOptionsResult = await _queryDispatcher.DispatchAsync(new GetTranslationOptionsQuery(CorrelationId.New(), CurrentActor));
            return translationOptionsResult.TranslationOptions;
        }
    }
}
