using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Exceptions;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.Users.Models;
using LvivDotNet.Common;
using Microsoft.Extensions.Configuration;

namespace LvivDotNet.Application.Users.Commands.Login
{
    /// <summary>
    /// Login command handler.
    /// </summary>
    public class LoginCommandHandler : BaseHandler<LoginCommand, AuthTokensModel>
    {
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginCommandHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        /// <param name="configuration"> Configuration. </param>
        public LoginCommandHandler(IDbConnectionFactory dbConnectionFactory, IConfiguration configuration)
            : base(dbConnectionFactory)
        {
            this.configuration = configuration;
        }

        /// <inheritdoc/>
        protected override async Task<AuthTokensModel> Handle(LoginCommand request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var user = await connection.QueryFirstAsync<UserModel>(
                            "select [user].*, [role].[name] as 'RoleName', [role].Id as 'RoleId' from dbo.[user] " +
                            "join dbo.[role] on [role].Id = [user].RoleId " +
                            "where Email = @Email",
                            new { request.Email },
                            transaction)
                .ConfigureAwait(false);

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
            var jwtToken = SecurityHelpers.GenerateJwtToken(user.Id, this.configuration["Secret"], user.RoleName);

            await connection.ExecuteAsync(
                    "insert into dbo.[refresh_token](UserId, RefreshToken, Expires) " +
                    "values (@UserId, @RefreshToken, @Expires)",
                    new { UserId = user.Id, RefreshToken = refreshToken, Expires = DateTime.UtcNow.AddDays(14) },
                    transaction)
                .ConfigureAwait(false);

            return new AuthTokensModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                JwtToken = jwtToken,
                RefreshToken = refreshToken,
                Role = user.RoleName,
            };
        }
    }
}
