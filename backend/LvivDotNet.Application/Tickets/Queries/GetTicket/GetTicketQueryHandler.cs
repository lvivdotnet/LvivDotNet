using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Exceptions;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.Tickets.Models;

namespace LvivDotNet.Application.Tickets.Queries.GetTicket
{
    /// <summary>
    /// Get ticket by id query handler.
    /// </summary>
    public class GetTicketQueryHandler : BaseHandler<GetTicketQuery, TicketModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetTicketQueryHandler"/> class.
        /// </summary>
        /// <param name="dbConnectionFactory"> Database connection factory. </param>
        public GetTicketQueryHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }

        /// <inheritdoc/>
        protected override async Task<TicketModel> Handle(GetTicketQuery request, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var tickets = await connection.QueryAsync<TicketModel>(
                "select " +
                "event.Name as 'EventName'," +
                "event.StartDate as 'Start'," +
                "event.EndDate as 'End'," +
                "ticket.CreatedDate as 'Bought'," +
                "ticket_template.Price as 'Price' " +
                "from dbo.[ticket] as ticket " +
                "join dbo.[ticket_template] as ticket_template on ticket.TicketTemplateId = ticket_template.Id " +
                "join dbo.[event] as event on event.Id = ticket_template.EventId " +
                "where ticket.Id = @TicketId and iif(ticket.UserId is not NULL, iif(ticket.UserId = @UserId, 1, 0), 1) = 1",
                request,
                transaction)
                .ConfigureAwait(true);

            if (tickets.Count() == 1)
            {
                return tickets.Single();
            }

            throw new NotFoundException("Ticket", request.TicketId);
        }
    }
}
