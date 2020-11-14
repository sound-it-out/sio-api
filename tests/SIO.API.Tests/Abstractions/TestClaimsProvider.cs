using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;

namespace SIO.API.Tests.Abstractions
{
    public class TestClaimsProvider
    {
        public readonly static Guid UserId = Guid.NewGuid();
        public IList<Claim> Claims { get; }

        public TestClaimsProvider(IList<Claim> claims)
        {
            Claims = claims;
        }

        public TestClaimsProvider()
        {
            Claims = new List<Claim>();
        }

        public static TestClaimsProvider WithAdminClaims()
        {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(JwtClaimTypes.Subject, UserId.ToString()));
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, UserId.ToString()));
            provider.Claims.Add(new Claim(ClaimTypes.Name, "Admin user"));
            provider.Claims.Add(new Claim(ClaimTypes.Role, "Admin"));

            return provider;
        }

        public static TestClaimsProvider WithUserClaims()
        {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(JwtClaimTypes.Subject, UserId.ToString()));
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, UserId.ToString()));
            provider.Claims.Add(new Claim(ClaimTypes.Name, "User"));

            return provider;
        }
    }
}
