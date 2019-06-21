using Dapper;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Common;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LvivDotNet.Application.Events.Commands.AddEvent
{
    public class AddEventCommandHandler : BaseHandler<AddEventCommand, int>
    {
        // public IDateTimeService DateTime { get; }

        public AddEventCommandHandler(IDbConnectionFactory dbConnectionFactory, IDateTimeService dateTime)
            : base(dbConnectionFactory)
        {
        }


        protected override async Task<int> Handle(AddEventCommand request, CancellationToken cancellationToken, IDbConnection connection, IDbTransaction transaction)
        {
            await connection.ExecuteAsync(
                "insert into dbo.event(Name, StartDate, EndDate, PostDate, Address, Title, Description, MaxAttendees) " +
                "values (@Name, @StartDate, @EndDate, @PostDate, @Address, @Title, @Description, @MaxAttendees)",
                new
                {
                    request.Name, request.StartDate, request.EndDate, PostDate = DateTime.UtcNow, request.Address,
                    request.Title, request.Description, request.MaxAttendees
                }, transaction);

            var eventId = await DatabaseHelpers.GetLastIdentity(connection, transaction);

            return eventId;
        }
    }
}
