using LvivDotNet.Application.Tickets.Models;
using MediatR;

namespace LvivDotNet.Application.Tickets.Queries.GetTicket
{
    /// <summary>
    /// Get ticket by id query.
    /// </summary>
    public class GetTicketQuery : IRequest<TicketModel>
    {
        /// <summary>
        /// gets or sets ticket id.
        /// </summary>
        public int TicketId { get; set; }
    }
}
