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
        public AddEventCommandHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory) { }


        protected override async Task<int> Handle(AddEventCommand request, CancellationToken cancellationToken, IDbConnection connection, IDbTransaction transaction)
        {
            await connection.ExecuteAsync("insert into dbo.event(Name, StartDate, EndDate, PostDate, Address, Title, Description, MaxAttendees) " +
                                       "values (@Name, @StartDate, @EndDate, @PostDate, @Address, @Title, @Description, @MaxAttendees)",
                                       new { request.Name, request.StartDate, request.EndDate, PostDate = DateTime.UtcNow, request.Address, request.Title, request.Description, request.MaxAttendees }, transaction);

            var eventId = await DatabaseHelpers.GetLastIdentity(connection, transaction);

            await Task.WhenAll(request.TicketTemplates.Select(template => new
            {
                sql = "insert into dbo.ticket_template([Name], [EventId], [Price], [To], [From]) values (@Name, @EventId, @Price, @To, @From)",
                value = new { template.Name, EventId = eventId, template.Price, template.To, template.From }
            }).Select(command => connection.ExecuteAsync(command.sql, command.value, transaction)));

            return eventId;
        }
    }
}
