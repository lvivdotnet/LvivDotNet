using Lviv_.NET_Platform.Application.Users.Models;
using MediatR;

namespace Lviv_.NET_Platform.Application.Users.Commands.Refresh
{
    public class RefreshTokenCommand : IRequest<AuthTokensModel>
    {
        public string RefreshToken { get; set; }

        public string JwnToken { get; set; }
    }
}
