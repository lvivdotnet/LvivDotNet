using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Exceptions;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.Users.Models;
using LvivDotNet.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace LvivDotNet.Application.Users.Commands.Refresh
{
    /// <summary>
    /// Token refresh command handler.
    /// </summary>
    public class RefreshTokenCommandHandler : BaseHandler<RefreshTokenCommand, AuthTokensModel>
    {
        /// <summary>
        /// Get refresh token expires date sql command.
        /// </summary>
        private const string GetRefreshTokenExpiresDateSqlCommand =
                    @"select ""Expires"" from public.refresh_token " +
                    @"where ""UserId"" = @UserId and ""RefreshToken"" = @RefreshToken";

        /// <summary>
        /// Get user and update refresh token sql command.
        /// </summary>
        private const string GetUserAndUpdateRefreshTokenSqlCommand =
                    @"select ""user"".*, ""role"".""Name"" as RoleName, ""role"".""Id"" as RoleId from public.user " + // Select user by user id
                    @"join public.role on ""role"".""Id"" = ""user"".""RoleId"" " +
                    @"where ""user"".""Id"" = cast(@UserId as integer);" +

                    "delete from public.refresh_token " + // Delete old refresh token
                    @"where ""UserId"" = cast(@UserId as integer) and ""RefreshToken"" = @RefreshToken;" +

                    @"insert into public.refresh_token(""UserId"", ""RefreshToken"", ""Expires"") " + // Insert new refresh token
                    "values (@UserId, @NewRefreshToken, @Expires);";

        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenCommandHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        /// <param name="httpContextAccessor"> See <see cref="IHttpContextAccessor"/>. </param>
        /// <param name="configuration"> See <see cref="IConfiguration"/>. </param>
        public RefreshTokenCommandHandler(IDbConnectionFactory dbConnectionFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
            : base(dbConnectionFactory, httpContextAccessor)
        {
            this.configuration = configuration;
        }

        /// <inheritdoc/>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "We already have a not-null check for request in MediatR")]
        protected override async Task<AuthTokensModel> Handle(RefreshTokenCommand request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            var token = SecurityHelpers.DecodeJwtToken(request.JwtToken);

            var userId = int.Parse(token.Claims.First(claim => claim.Type == "id").Value, System.Globalization.NumberFormatInfo.CurrentInfo);

            var refreshTokenExpires = await connection.QueryAsync<DateTime>(GetRefreshTokenExpiresDateSqlCommand, new { UserId = userId, request.RefreshToken }, transaction)
                .ConfigureAwait(false);

            var refreshTokenExists = refreshTokenExpires.Count() == 1;

            if (!refreshTokenExists)
            {
                throw new InvalidRefreshTokenException();
            }

            if (refreshTokenExpires.First() < DateTime.UtcNow)
            {
                throw new RefreshTokenExpiredException();
            }

            var newRefreshToken = Convert.ToBase64String(SecurityHelpers.GetRandomBytes(32));

            var user = await connection.QuerySingleAsync<UserModel>(GetUserAndUpdateRefreshTokenSqlCommand, new { UserId = userId, request.RefreshToken, NewRefreshToken = newRefreshToken, Expires = DateTime.UtcNow.AddDays(14) }, transaction)
                .ConfigureAwait(false);

            var newToken = SecurityHelpers.GenerateJwtToken(userId, this.configuration["Secret"], user.RoleName);

            return new AuthTokensModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RefreshToken = newRefreshToken,
                JwtToken = newToken,
                Role = token.Claims.First(claim => claim.Type == "role").Value,
            };
        }
    }
}
