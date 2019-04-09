using Dapper;
using System.Threading;
using System.Threading.Tasks;
using Lviv_.NET_Platform.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Lviv_.NET_Platform.Domain.Entities;
using System;
using Lviv_.NET_Platform.Application.Exceptions;

namespace Lviv_.NET_Platform.Application.Users.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public LogoutCommandHandler(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            using (var connection = dbConnectionFactory.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
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

                    transaction.Commit();
                    connection.Close();
                    return Unit.Value;
                }
            }
        }
    }
}
