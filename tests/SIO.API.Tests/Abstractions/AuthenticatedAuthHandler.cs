﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SIO.API.Tests.Abstractions
{
    public class AuthenticatedAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IList<Claim> _claims;

        public AuthenticatedAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
                               UrlEncoder encoder, ISystemClock clock, TestClaimsProvider claimsProvider) : base(options, logger, encoder, clock)
        {
            _claims = claimsProvider.Claims;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var identity = new ClaimsIdentity(_claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }

}
