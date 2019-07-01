using MediatR;

namespace LvivDotNet.Application.Users.Commands.Logout
{
    /// <summary>
    /// Logout command.
    /// </summary>
    public class LogoutCommand : IRequest
    {
        /// <summary>
        /// Gets or sets refresh token.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets token.
        /// </summary>
        public string Token { get; set; }
    }
}
