using MediatR;

namespace Lviv_.NET_Platform.Application.Users.Commands.Logout
{
    public class LogoutCommand: IRequest
    {
        public string RefreshToken { get; set; }

        public string UserId { get; set; }
    }
}
