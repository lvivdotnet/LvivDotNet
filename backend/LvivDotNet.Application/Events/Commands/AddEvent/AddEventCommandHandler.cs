using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Interfaces;

namespace LvivDotNet.Application.Events.Commands.AddEvent
{
    /// <summary>
    /// Add event command handler.
    /// </summary>
    public class AddEventCommandHandler : BaseHandler<AddEventCommand, int>
    {
        /// <summary>
        /// Insert event sql command.
        /// </summary>
        private const string InsertEventSqlCommand =
               @"insert into public.event(""Name"", ""StartDate"", ""EndDate"", ""PostDate"", ""Address"", ""Title"", ""Description"", ""MaxAttendees"") " +
                "values (@Name, @StartDate, @EndDate, @PostDate, @Address, @Title, @Description, @MaxAttendees) " +
                @"returning ""Id""";

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
            var insertEventParams = new { request.Name, request.StartDate, request.EndDate, PostDate = DateTime.UtcNow, request.Address, request.Title, request.Description, request.MaxAttendees };
            return await connection.QuerySingleAsync<int>(InsertEventSqlCommand, insertEventParams, transaction)
                .ConfigureAwait(false);
        }
    }
}
