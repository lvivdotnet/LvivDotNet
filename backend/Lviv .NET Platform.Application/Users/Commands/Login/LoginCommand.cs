using Lviv_.NET_Platform.Application.Users.Models;
using MediatR;

namespace Lviv_.NET_Platform.Application.Users.Commands.Login
{
    public class LoginCommand : IRequest<AuthTokensModel>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}