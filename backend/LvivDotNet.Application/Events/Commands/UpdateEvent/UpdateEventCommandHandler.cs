using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LvivDotNet.Application.Events.Commands.UpdateEvent
{
    /// <summary>
    /// Update event command handler.
    /// </summary>
    public class UpdateEventCommandHandler : BaseHandler<UpdateEventCommand>
    {
        /// <summary>
        /// Update event sql command.
        /// </summary>
        private const string UpdateEventSqlCommand =
                    "update public.event " +
                    @"set ""Name"" = @Name, " +
                    @"""StartDate"" = @StartDate, " +
                    @"""EndDate"" = @EndDate, " +
                    @"""Address"" = @Address, " +
                    @"""Title"" = @Title, " +
                    @"""Description"" = @Description, " +
                    @"""MaxAttendees"" = @MaxAttendees " +
                    @"where ""Id"" = cast(@Id as integer)";

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateEventCommandHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        /// <param name="httpContextAccessor"> See <see cref="IHttpContextAccessor"/>. </param>
        public UpdateEventCommandHandler(IDbConnectionFactory dbConnectionFactory, IHttpContextAccessor httpContextAccessor)
            : base(dbConnectionFactory, httpContextAccessor)
        {
        }

        /// <inheritdoc/>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "We already have a not-null check for request in MediatR")]
        protected override async Task<Unit> Handle(UpdateEventCommand request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            await connection.ExecuteAsync(UpdateEventSqlCommand, request, transaction)
                .ConfigureAwait(false);

            return Unit.Value;
        }
    }
}
