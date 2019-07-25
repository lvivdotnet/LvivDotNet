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
        /// <summary>
        /// Get user sql query.
        /// </summary>
        private const string GetUserSqlQuery =
                            @"select ""user"".*, ""role"".""Name"" as RoleName, ""role"".""Id"" as RoleId from public.user " +
                            @"join public.role on ""role"".""Id"" = ""user"".""RoleId"" " +
                            @"where ""Email"" = @Email";

        /// <summary>
        /// Insert refresh token command.
        /// </summary>
        private const string InsertRefreshTokenCommand =
                    @"insert into public.refresh_token(""UserId"", ""RefreshToken"", ""Expires"") " +
                    "values (@UserId, @RefreshToken, @Expires)";

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
            var user = await connection.QueryFirstAsync<UserModel>(GetUserSqlQuery, new { request.Email }, transaction)
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

            await connection.ExecuteAsync(InsertRefreshTokenCommand, new { UserId = user.Id, RefreshToken = refreshToken, Expires = DateTime.UtcNow.AddDays(14) }, transaction)
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
