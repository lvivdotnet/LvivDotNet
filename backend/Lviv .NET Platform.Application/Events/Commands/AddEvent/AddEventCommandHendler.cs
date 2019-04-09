using Dapper;
using Lviv_.NET_Platform.Application.Events.Commands.AddEvent;
using Lviv_.NET_Platform.Application.Interfaces;
using Lviv_.NET_Platform.Common;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lviv_.NET_Platform.Application.Events.Commands.AddEvent
{
    public class AddEventCommandHendler : IRequestHandler<AddEventCommand, int>
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public AddEventCommandHendler(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<int> Handle(AddEventCommand request, CancellationToken cancellationToken)
        {
            using (var connection = dbConnectionFactory.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync("insert into dbo.event(Name, StartDate, EndDate, PostDate, Address, Title, Description, MaxAttendees) " +
                                       "values (@Name, @StartDate, @EndDate, @PostDate, @Address, @Title, @Description, @MaxAttendees)", request, transaction);

                    var eventId = await DatabaseHelpers.GetLastIdentity(connection, transaction);

                    await Task.WhenAll(request.TicketTemplates.Select(template => new
                    {
                        sql = "insert into dbo.ticket_template([Name], [EventId], [Price], [To], [From]) values (@Name, @EventId, @Price, @To, @From)",
                        value = new { template.Name, EventId = eventId, template.Price, template.To, template.From }
                    }).Select(command => connection.ExecuteAsync(command.sql, command.value, transaction)));

                    transaction.Commit();
                    connection.Close();

                    return eventId;
                }
            }
        }
    }
}
