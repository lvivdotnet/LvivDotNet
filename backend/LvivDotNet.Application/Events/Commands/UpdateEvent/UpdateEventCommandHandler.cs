using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Interfaces;
using MediatR;

namespace LvivDotNet.Application.Events.Commands.UpdateEvent
{
    /// <summary>
    /// Update event command handler.
    /// </summary>
    public class UpdateEventCommandHandler : BaseHandler<UpdateEventCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateEventCommandHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory.</param>
        public UpdateEventCommandHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }

        /// <inheritdoc/>
        protected override async Task<Unit> Handle(UpdateEventCommand request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            await connection.ExecuteAsync(
                    "update dbo.[event] " +
                    "set Name = @Name, " +
                    "StartDate = @StartDate, " +
                    "EndDate = @EndDate, " +
                    "Address = @Address, " +
                    "Title = @Title, " +
                    "Description = @Description, " +
                    "MaxAttendees = @MaxAttendees " +
                    "where Id = @Id",
                    request,
                    transaction)
                .ConfigureAwait(false);

            return Unit.Value;
        }
    }
}
