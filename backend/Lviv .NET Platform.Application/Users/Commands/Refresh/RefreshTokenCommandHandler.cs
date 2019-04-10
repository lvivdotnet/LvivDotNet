using Dapper;
using Lviv_.NET_Platform.Application.Exceptions;
using Lviv_.NET_Platform.Application.Interfaces;
using Lviv_.NET_Platform.Application.Users.Models;
using Lviv_.NET_Platform.Common;
using Lviv_.NET_Platform.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Lviv_.NET_Platform.Application.Users.Commands.Refresh
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthTokensModel>
    {
        private readonly IDbConnectionFactory dbConnectionFactory;
        private readonly IConfiguration configuration;

        public RefreshTokenCommandHandler(IDbConnectionFactory dbConnectionFactory, IConfiguration configuration)
        {
            this.dbConnectionFactory = dbConnectionFactory;
            this.configuration = configuration;
        }

        public async Task<AuthTokensModel> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            using (var connection = dbConnectionFactory.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var token = SecurityHelpers.DecodeJwtToken(request.JwnToken);

                    var userId = int.Parse(token.Claims.First(claim => claim.Type == "id").Value);

                    var refreshTokenExpires = await connection.QueryAsync<DateTime>(
                            "select Expires from dbo.[refresh_token] " +
                            "where UserId = @UserId and RefreshToken = @RefreshToken",
                            new { UserId = userId, request.RefreshToken },
                            transaction
                        );
                    var refreshTokenExists = refreshTokenExpires.Count() == 1;

                    if (!refreshTokenExists)
                    {
                        throw new InvalidRefreshTokenException(); 
                    }
                    if (refreshTokenExpires.First() < DateTime.UtcNow)
                    {
                        throw new RefreshTokenExpiredException();
                    }

                    var user = await connection.QuerySingleAsync<UserModel>(
                            "select [user].*, [role].[name] as 'RoleName', [role].Id as 'RoleId' from dbo.[user] " +
                            "join dbo.[role] on [role].Id = [user].RoleId" +
                            "where Id = @Id",
                            new { Id = userId },
                            transaction
                        );

                    await connection.ExecuteAsync(
                            "delete from dbo.refresh_token " +
                            "where UserId = @UserId and RefreshToken = @RefreshToken",
                            new { UserId = userId, request.RefreshToken },
                            transaction
                        );

                    var newRefreshToken = Convert.ToBase64String(SecurityHelpers.GetRandomBytes(32));
                    var newToken = SecurityHelpers.GenerateJwtToken(userId, configuration["Secret"], user.RoleName);

                    await connection.ExecuteAsync(
                            "insert into dbo.refresh_token(UserId, RefreshToken, Expires) " +
                            "values (@UserId, @RefreshToken, @Expires)",
                            new { UserId = userId, RefreshToken = newRefreshToken, Expires = DateTime.UtcNow.AddDays(14) },
                            transaction
                        );

                    transaction.Commit();
                    connection.Close();

                    return new AuthTokensModel
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        RefreshToken = newRefreshToken,
                        JwtToken = newToken
                    };
                }
            }
        }
    }
}
