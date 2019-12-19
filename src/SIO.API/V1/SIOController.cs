using System;
using Microsoft.AspNetCore.Mvc;

namespace SIO.API.V1
{
    public class SIOController : ControllerBase
    {
        protected Guid? CurrentUserId => User.UserId();
    }
}
