using System.Collections.Generic;
using LvivDotNet.Application.Tickets.Models;
using MediatR;

namespace LvivDotNet.Application.Tickets.Queries.GetUserTickets
{
    /// <summary>
    /// Get all user tickets query.
    /// </summary>
    public class GetUserTicketsQuery : IRequest<IEnumerable<TicketModel>>
    {
        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public int UserId { get; set; }
    }
}
