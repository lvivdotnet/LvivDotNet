using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Common;

namespace LvivDotNet.Application.Events.Commands.AddEvent
{
    /// <summary>
    /// Add event command handler.
    /// </summary>
    public class AddEventCommandHandler : BaseHandler<AddEventCommand, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddEventCommandHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        public AddEventCommandHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }

        /// <inheritdoc/>
        protected override async Task<int> Handle(AddEventCommand request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            await connection.ExecuteAsync(
                "insert into dbo.event(Name, StartDate, EndDate, PostDate, Address, Title, Description, MaxAttendees) " +
                "values (@Name, @StartDate, @EndDate, @PostDate, @Address, @Title, @Description, @MaxAttendees)",
                new { request.Name, request.StartDate, request.EndDate, PostDate = DateTime.UtcNow, request.Address, request.Title, request.Description, request.MaxAttendees },
                transaction)
                .ConfigureAwait(false);

            var eventId = await DatabaseHelpers
                .GetLastIdentity(connection, transaction)
                .ConfigureAwait(false);

            return eventId;
        }
    }
}
