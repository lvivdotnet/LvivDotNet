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

            var refreshToken = await connection.QueryFirstAsync<RefreshToken>(
                            "select * from dbo.[refresh_token] " +
                            "where UserId = @UserId and RefreshToken = @RefreshToken",
                            new { UserId = userId, request.RefreshToken },
                            transaction)
                .ConfigureAwait(false);

            if (refreshToken == null)
            {
                throw new InvalidRefreshTokenException();
            }

            await connection.ExecuteAsync(
                    "delete from dbo.[refresh_token]" +
                    "where Id = @Id",
                    new { refreshToken.Id },
                    transaction)
                .ConfigureAwait(false);

            return Unit.Value;
        }
    }
}
