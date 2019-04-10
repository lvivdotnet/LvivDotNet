using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Lviv_.NET_Platform.Application.Interfaces;
using Lviv_.NET_Platform.Application.Users.Models;
using Lviv_.NET_Platform.Common;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Lviv_.NET_Platform.Application.Users.Commands.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthTokensModel>
    {
        private readonly IDbConnectionFactory dbConnectionFactory;
        private readonly IConfiguration configuration;

        public RegisterUserCommandHandler(IDbConnectionFactory dbConnectionFactory, IConfiguration configuration)
        {
            this.dbConnectionFactory = dbConnectionFactory;
            this.configuration = configuration;
        }

        public async Task<AuthTokensModel> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            using (var connection = dbConnectionFactory.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var salt = SecurityHelpers.GetRandomBytes(32);

                    var passwordHash = SecurityHelpers.GetPasswordHash(request.Password, salt);

                    var refreshToken = Convert.ToBase64String(SecurityHelpers.GetRandomBytes(32));

                    await connection.ExecuteAsync(
                        "insert into dbo.[User](FirstName, LastName, Email, Phone, Sex, Age, Avatar, Password, Salt, RoleId)" +
                        "values (@FirstName, @LastName, @Email, @Phone, @Sex, @Age, @Avatar, @Password, @Salt, (select top 1 Id from dbo.[role] where [name] = 'User'))",
                        new { request.FirstName, request.LastName, request.Email, request.Phone, request.Sex, request.Age, request.Avatar, Password = passwordHash, Salt = Convert.ToBase64String(salt) },
                        transaction
                    );

                    var userId = await DatabaseHelpers.GetLastIdentity(connection, transaction);

                    await connection.ExecuteAsync(
                        "insert into dbo.[refresh_token](UserId, RefreshToken, Expires) values (@UserId, @RefreshToken, @Expires)",
                        new { UserId = userId, RefreshToken = refreshToken, Expires = DateTime.UtcNow.AddDays(14) },
                        transaction
                    );

                    var token = SecurityHelpers.GenerateJwtToken(userId, configuration["Secret"], "User");

                    transaction.Commit();
                    connection.Close();

                    return new AuthTokensModel
                    {
                        Email = request.Email,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        RefreshToken = refreshToken,
                        JwtToken = token
                    };
                }
            }
        }
    }
}