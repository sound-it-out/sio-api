using Microsoft.AspNetCore.Mvc;
using SIO.Api.Extensions;
using SIO.Infrastructure;

namespace SIO.API.V1
{
    [ApiController, ApiVersion("1.0"), Route("v{version:apiVersion}/[controller]")]
    public abstract class SIOController : ControllerBase
    {
        protected string CurrentUserSubject => User.Subject();
        protected Actor CurrentActor => Actor.From(CurrentUserSubject);
    }
}
