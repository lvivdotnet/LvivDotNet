using LvivDotNet.Domain.Entities;
using MediatR;

namespace LvivDotNet.Application.Tickets.Commands.BuyTicket.Unauthorized
{
    /// <summary>
    /// Buy ticket by unauthorized user command.
    /// </summary>
    public class BuyUnauthorizedTicketCommand : IRequest<int>
    {
        /// <summary>
        /// Gets or sets event id.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets male.
        /// </summary>
        public Sex? Male { get; set; }

        /// <summary>
        /// Gets or sets age.
        /// </summary>
        public int? Age { get; set; }
    }
}
