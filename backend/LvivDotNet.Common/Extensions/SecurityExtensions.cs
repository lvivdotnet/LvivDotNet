using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace LvivDotNet.Common.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ClaimsPrincipal"/> class.
    /// </summary>
    public static class SecurityExtensions
    {
        /// <summary>
        /// Get claim value of type 'id' from <see cref="ClaimsPrincipal"/>.
        /// </summary>
        /// <param name="claimsPrincipal"><see cref="ClaimsPrincipal"/>.</param>
        /// <returns> Claim value of type 'id'. </returns>
        public static int GetId(this ClaimsPrincipal claimsPrincipal)
            => int.Parse(claimsPrincipal.Get("id"), Thread.CurrentThread.CurrentCulture);

        /// <summary>
        /// Gets claim value based on type.
        /// </summary>
        /// <param name="claimsPrincipal"><see cref="ClaimsPrincipal"/>.</param>
        /// <param name="type"> Claim type. </param>
        /// <returns> Claim value. </returns>
        public static string Get(this ClaimsPrincipal claimsPrincipal, string type)
        {
            if (claimsPrincipal == null)
            {
                throw new ArgumentNullException(nameof(claimsPrincipal));
            }

            return claimsPrincipal.Claims.Get(type);
        }

        /// <summary>
        /// Gets claim value based on type.
        /// </summary>
        /// <param name="claims"><see cref="IEnumerable{Claim}"/>.</param>
        /// <param name="type"> Claim type. </param>
        /// <returns> Claim value. </returns>
        public static string Get(this IEnumerable<Claim> claims, string type)
            => claims.GetClaim(type)?.Value;

        /// <summary>
        /// Gets claim based on type.
        /// </summary>
        /// <param name="claims"><see cref="IEnumerable{Claim}"/>.</param>
        /// <param name="type"> Claim type. </param>
        /// <returns> <see cref="Claim"/>. </returns>
        public static Claim GetClaim(this IEnumerable<Claim> claims, string type)
        {
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            return claims.FirstOrDefault(c => c.Type == type);
        }
    }
}
