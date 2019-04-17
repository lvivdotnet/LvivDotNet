using Dapper;
using Lviv_.NET_Platform.Application.Exceptions;
using Lviv_.NET_Platform.Application.Interfaces;
using Lviv_.NET_Platform.Application.Users.Models;
using Lviv_.NET_Platform.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Lviv_.NET_Platform.Application.Users.Commands.Login
{
    public class LoginCommandHandler : BaseHandler<LoginCommand, AuthTokensModel>
    {
        private readonly IConfiguration configuration;

        public LoginCommandHandler(IDbConnectionFactory dbConnectionFactory, IConfiguration configuration)
            : base(dbConnectionFactory)
        {
            this.configuration = configuration;
        }

        protected override async Task<AuthTokensModel> Handle(LoginCommand request, CancellationToken cancellationToken, IDbConnection connection, IDbTransaction transaction)
        {
            var user = await connection.QueryFirstAsync<UserModel>(
                            "select [user].*, [role].[name] as 'RoleName', [role].Id as 'RoleId' from dbo.[user] " +
                            "join dbo.[role] on [role].Id = [user].RoleId " +
                            "where Email = @Email",
                            new { request.Email },
                            transaction
                        );

            if (user == null)
            {
                throw new NotFoundException("User", request.Email);
            }

            var passwordHash = SecurityHelpers.GetPasswordHash(request.Password, Convert.FromBase64String(user.Salt));

            if (passwordHash != user.Password)
            {
                throw new AuthException();
            }

            var refreshToken = Convert.ToBase64String(SecurityHelpers.GetRandomBytes(32));
            var jwtToken = SecurityHelpers.GenerateJwtToken(user.Id, configuration["Secret"], user.RoleName);

            await connection.ExecuteAsync(
                    "insert into dbo.[refresh_token](UserId, RefreshToken, Expires) " +
                    "values (@UserId, @RefreshToken, @Expires)",
                    new { UserId = user.Id, RefreshToken = refreshToken, Expires = DateTime.UtcNow.AddDays(14) },
                    transaction
                );

            return new AuthTokensModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                JwtToken = jwtToken,
                RefreshToken = refreshToken,
                Role = user.RoleName
            };
        }
    }
}
