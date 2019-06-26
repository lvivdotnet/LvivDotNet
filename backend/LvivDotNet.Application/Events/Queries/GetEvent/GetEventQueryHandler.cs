using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Events.Models;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.TicketTemplates.Models;

namespace LvivDotNet.Application.Events.Queries.GetEvent
{
    /// <summary>
    /// Get event query handler.
    /// </summary>
    public class GetEventQueryHandler : BaseHandler<GetEventQuery, EventModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetEventQueryHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        public GetEventQueryHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }

        /// <inheritdoc/>
        protected override async Task<EventModel> Handle(GetEventQuery request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new System.ArgumentNullException(nameof(request));
            }

            var eventDictionary = new Dictionary<int, EventModel>();
            var result = await connection.QueryAsync<EventModel, TicketTemplateModel, EventModel>(
                    "select * from dbo.[event] as event " +
                    "left join dbo.[ticket_template] as ticket_event on event.Id = ticket_event.EventId " +
                    "where event.Id = @EventId",
                    (@event, tickerTemplate) =>
                    {
                        if (!eventDictionary.ContainsKey(@event.Id))
                        {
                            eventDictionary.Add(@event.Id, @event);
                        }

                        if (tickerTemplate != null)
                        {
                            eventDictionary[@event.Id].TickerTemplates.Add(tickerTemplate);
                        }

                        return @event;
                    },
                    new { request.EventId },
                    transaction)
                .ConfigureAwait(false);

            return result.First();
        }
    }
}