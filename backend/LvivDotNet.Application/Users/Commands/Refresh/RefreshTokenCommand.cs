using LvivDotNet.Application.Users.Models;
using MediatR;

namespace LvivDotNet.Application.Users.Commands.Refresh
{
    /// <summary>
    /// Token refresh command.
    /// </summary>
    public class RefreshTokenCommand : IRequest<AuthTokensModel>
    {
        /// <summary>
        /// Gets or sets refresh token.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets Jwt token.
        /// </summary>
        public string JwtToken { get; set; }
    }
}
