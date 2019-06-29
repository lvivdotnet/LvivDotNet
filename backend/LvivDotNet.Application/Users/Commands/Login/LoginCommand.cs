using LvivDotNet.Application.Users.Models;
using MediatR;

namespace LvivDotNet.Application.Users.Commands.Login
{
    /// <summary>
    /// Login command.
    /// </summary>
    public class LoginCommand : IRequest<AuthTokensModel>
    {
        /// <summary>
        /// Gets or sets email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        public string Password { get; set; }
    }
}