using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenEventSourcing.Queries;
using SIO.API.V1.Translation.Responses;
using SIO.Domain.Translations.Queries;

namespace SIO.API.V1.Translation
{
    [Authorize]
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
        public async Task<ListOptionsResponse> ListOptions()
        {
            var translationOptionsResult = await _queryDispatcher.DispatchAsync(new GetTranslationOptionsQuery(Guid.NewGuid(), Guid.NewGuid().ToString()));
            return new ListOptionsResponse(translationOptionsResult.TranslationOptions);
        }
    }
}
