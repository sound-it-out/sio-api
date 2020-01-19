using System;
using Microsoft.AspNetCore.Mvc;

namespace SIO.API.V1
{
    [ApiController, ApiVersion("1.0"), Route("v{version:apiVersion}/[controller]")]
    public class SIOController : ControllerBase
    {
        protected Guid? CurrentUserId => User.UserId();
    }
}
