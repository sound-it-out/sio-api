using Microsoft.AspNetCore.Authorization;
using SIO.API.V1.Controllers;
using SIO.Domain.TranslationOptions.Projections;
using SIO.Infrastructure.Projections;

namespace SIO.Api.V1.TranslationOptions
{
    [Authorize]
    public class TranslationOptionProjectionController : ProjectionController<TranslationOption>
    {
        public TranslationOptionProjectionController(IProjector<TranslationOption> projector) : base(projector)
        {
        }
    }
}
