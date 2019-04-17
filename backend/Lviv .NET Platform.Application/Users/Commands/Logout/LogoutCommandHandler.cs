using Dapper;
using Lviv_.NET_Platform.Application.Exceptions;
using Lviv_.NET_Platform.Application.Interfaces;
using Lviv_.NET_Platform.Domain.Entities;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Lviv_.NET_Platform.Application.Users.Commands.Logout
{
    public class LogoutCommandHandler : BaseHandler<LogoutCommand>
    {
        public LogoutCommandHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory) { }

        protected override async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken, IDbConnection connection, IDbTransaction transaction)
        {
            var refreshToken = await connection.QueryFirstAsync<RefreshToken>(
                            "select * from dbo.[refresh_token] " +
                            "where UserId = @UserId and RefreshToken = @RefreshToken",
                            new { request.UserId, request.RefreshToken },
                            transaction
                        );

            if (refreshToken == null)
            {
                throw new InvalidRefreshTokenException();
            }

            await connection.ExecuteAsync(
                    "delete from dbo.[refresh_token]" +
                    "where Id = @Id",
                    new { refreshToken.Id },
                    transaction
                );

            return Unit.Value;
        }
    }
}
