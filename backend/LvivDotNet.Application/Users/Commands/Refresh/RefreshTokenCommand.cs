using LvivDotNet.Application.Users.Models;
using MediatR;

namespace LvivDotNet.Application.Users.Commands.Refresh
{
    public class RefreshTokenCommand : IRequest<AuthTokensModel>
    {
        public string RefreshToken { get; set; }

        public string JwtToken { get; set; }
    }
}
