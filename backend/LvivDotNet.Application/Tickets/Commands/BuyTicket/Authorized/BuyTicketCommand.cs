using MediatR;

namespace LvivDotNet.Application.Tickets.Commands.BuyTicket.Authorized
{
    /// <summary>
    /// But ticket for authorized user command.
    /// </summary>
    public class BuyTicketCommand : IRequest
    {
        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// Gets or sets event id.
        /// </summary>
        public int EventId { get; set; }
    }
}
