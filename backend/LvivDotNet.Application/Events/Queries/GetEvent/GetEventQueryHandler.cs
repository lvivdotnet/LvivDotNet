using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Events.Models;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.TicketTemplates.Models;
using Microsoft.AspNetCore.Http;

namespace LvivDotNet.Application.Events.Queries.GetEvent
{
    /// <summary>
    /// Get event query handler.
    /// </summary>
    public class GetEventQueryHandler : BaseHandler<GetEventQuery, EventModel>
    {
        /// <summary>
        /// Get event sql query.
        /// </summary>
        private const string GetEventSqlQuery =
                    "select * from public.event as event " +
                    @"left join public.ticket_template as ticket_event on ""event"".""Id"" = ""ticket_event"".""EventId"" " +
                    @"where ""event"".""Id"" = cast(@EventId as integer)";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetEventQueryHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        /// <param name="httpContextAccessor"> See <see cref="IHttpContextAccessor"/>. </param>
        public GetEventQueryHandler(IDbConnectionFactory dbConnectionFactory, IHttpContextAccessor httpContextAccessor)
            : base(dbConnectionFactory, httpContextAccessor)
        {
        }

        /// <inheritdoc/>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification= "We already have a not-null check for request in MediatR")]
        protected override async Task<EventModel> Handle(GetEventQuery request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            var eventDictionary = new Dictionary<int, EventModel>();
            var result = await connection.QueryAsync<EventModel, TicketTemplateModel, EventModel>(
                    GetEventSqlQuery,
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