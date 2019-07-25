using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Exceptions;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Common;
using LvivDotNet.Domain.Entities;
using MediatR;

namespace LvivDotNet.Application.Users.Commands.Logout
{
    /// <summary>
    /// Logout command handler.
    /// </summary>
    public class LogoutCommandHandler : BaseHandler<LogoutCommand>
    {
        /// <summary>
        /// Get refresh token sql query.
        /// </summary>
        private const string GetRefreshTokenSqlQuery =
                            "select * from public.refresh_token " +
                            @"where ""UserId"" = cast(@UserId as integer) and ""RefreshToken"" = @RefreshToken";

        /// <summary>
        /// Delete refresh token sql command.
        /// </summary>
        private const string DeleteRefreshTokenSqlCommand =
                    "delete from public.refresh_token " +
                    @"where ""Id"" = cast(@Id as integer)";

        /// <summary>
        /// Initializes a new instance of the <see cref="LogoutCommandHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        public LogoutCommandHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }

        /// <inheritdoc/>
        protected override async Task<Unit> Handle(LogoutCommand request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var userId = SecurityHelpers.DecodeJwtToken(request.Token).Claims.First(claim => claim.Type == "id").Value;

            var refreshToken = await connection.QueryFirstAsync<RefreshToken>(GetRefreshTokenSqlQuery, new { UserId = userId, request.RefreshToken }, transaction)
                .ConfigureAwait(false);

            if (refreshToken == null)
            {
                throw new InvalidRefreshTokenException();
            }

            await connection.ExecuteAsync(DeleteRefreshTokenSqlCommand, new { refreshToken.Id }, transaction)
                .ConfigureAwait(false);

            return Unit.Value;
        }
    }
}
