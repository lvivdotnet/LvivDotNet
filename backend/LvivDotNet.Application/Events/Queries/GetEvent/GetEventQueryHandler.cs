using Dapper;
using LvivDotNet.Application.Events.Models;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.TicketTemplates.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LvivDotNet.Application.Events.Queries.GetEvent
{
    public class GetEventQueryHandler : BaseHandler<GetEventQuery, EventModel>
    {
        public GetEventQueryHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory) { }

        protected override async Task<EventModel> Handle(GetEventQuery request, CancellationToken cancellationToken, IDbConnection connection, IDbTransaction transaction)
        {
            var eventDictionary = new Dictionary<int, EventModel>();
            var result = await connection.QueryAsync<EventModel, TicketTemplateModel, EventModel>(
                    "select * from dbo.[event] as event " +
                    "left join dbo.[ticket_template] as ticket_event on event.Id = ticket_event.EventId " +
                    "where event.Id = @EventId",
                    (@event, tickerTemplate) =>
                    {
                        if (eventDictionary.TryGetValue(@event.Id, out var e))
                        {
                            e.TickerTemplates.Add(tickerTemplate);
                            return e;
                        }

                        @event.TickerTemplates = tickerTemplate != null
                            ? new List<TicketTemplateModel> { tickerTemplate }
                            : new List<TicketTemplateModel>();
                        eventDictionary.Add(@event.Id, @event);

                        return @event;

                    },
                    new { request.EventId },
                    transaction
                );

            return result.First();
        }
    }
}