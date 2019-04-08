using Dapper;
using Lviv_.NET_Platform.Application.Interfaces;
using Lviv_.NET_Platform.Application.Users.Models;
using Lviv_.NET_Platform.Common;
using Lviv_.NET_Platform.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lviv_.NET_Platform.Application.Users.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthTokensModel>
    {
        private readonly IDbConnectionFactory dbConnectionFactory;
        private readonly IConfiguration configuration;

        public LoginCommandHandler(IDbConnectionFactory dbConnectionFactory, IConfiguration configuration)
        {
            this.dbConnectionFactory = dbConnectionFactory;
            this.configuration = configuration;
        }

        public async Task<AuthTokensModel> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            using (var connection = dbConnectionFactory.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var user = await connection.QuerySingleAsync<User>(
                            "select * from dbo.[user] " +
                            "where Email = @Email",
                            new { request.Email },
                            transaction
                        );

                    var passwordHash = SecurityHelpers.GetPasswordHash(request.Password, Convert.FromBase64String(user.Salt));

                    if (passwordHash != user.Password)
                    {
                        throw new Exception("Invalid password");
                    }

                    var refreshToken = Convert.ToBase64String(SecurityHelpers.GetRandomBytes(32));
                    var jwtToken = SecurityHelpers.GenerateJwtToken(user.Id, configuration["Secret"]);

                    await connection.ExecuteAsync(
                            "insert into dbo.[refresh_token](UserId, RefreshToken, Expires) " +
                            "values (@UserId, @RefreshToken, @Expires)",
                            new { UserId = user.Id, RefreshToken = refreshToken, Expires = DateTime.UtcNow.AddDays(14) },
                            transaction
                        );

                    transaction.Commit();
                    connection.Close();

                    return new AuthTokensModel
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        JwtToken = jwtToken,
                        RefreshToken = refreshToken
                    };
                }
            }
        }
    }
}
