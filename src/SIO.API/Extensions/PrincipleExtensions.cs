using IdentityModel;
using System;
using System.Linq;
using System.Security.Claims;

namespace SIO.Api.Extensions
{
    internal static class PrincipleExtensions
    {
        public static string Subject(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var claim = principal.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject);

            if (claim == null)
                throw new InvalidOperationException("sub claim is missing");

            return claim.Value;
        }
    }
}
