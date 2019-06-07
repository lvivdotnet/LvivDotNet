using MediatR;

namespace LvivDotNet.Application.Users.Commands.Logout
{
    public class LogoutCommand : IRequest
    {
        public string RefreshToken { get; set; }

        public int UserId { get; set; }
    }
}
