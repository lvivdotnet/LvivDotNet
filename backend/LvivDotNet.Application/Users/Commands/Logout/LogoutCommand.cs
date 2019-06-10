using MediatR;

namespace LvivDotNet.Application.Users.Commands.Logout
{
    public class LogoutCommand : IRequest
    {
        public string RefreshToken { get; set; }

        public string Token { get; set; }
    }
}
