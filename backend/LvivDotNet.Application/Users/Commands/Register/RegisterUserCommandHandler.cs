using Dapper;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.Users.Models;
using LvivDotNet.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace LvivDotNet.Application.Users.Commands.Register
{
    public class RegisterUserCommandHandler : BaseHandler<RegisterUserCommand, AuthTokensModel>
    {
        private readonly IConfiguration configuration;

        public RegisterUserCommandHandler(IDbConnectionFactory dbConnectionFactory, IConfiguration configuration)
            : base(dbConnectionFactory)
        {
            this.configuration = configuration;
        }

        protected override async Task<AuthTokensModel> Handle(RegisterUserCommand request, CancellationToken cancellationToken, IDbConnection connection, IDbTransaction transaction)
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

            return new AuthTokensModel
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                RefreshToken = refreshToken,
                JwtToken = token,
                Role = "User"
            };
        }
    }
}