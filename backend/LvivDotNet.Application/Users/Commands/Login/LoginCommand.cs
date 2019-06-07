using LvivDotNet.Application.Users.Models;
using MediatR;

namespace LvivDotNet.Application.Users.Commands.Login
{
    public class LoginCommand : IRequest<AuthTokensModel>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}