using MediatR;

namespace LvivDotNet.Application.Tickets.Commands.BuyTicket.Authorized
{
    /// <summary>
    /// But ticket for authorized user command.
    /// </summary>
    public class BuyAuthorizedTicketCommand : IRequest<int>
    {
        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets event id.
        /// </summary>
        public int EventId { get; set; }
    }
}
