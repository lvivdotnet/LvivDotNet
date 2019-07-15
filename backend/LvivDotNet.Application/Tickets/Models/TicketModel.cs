using System;

namespace LvivDotNet.Application.Tickets.Models
{
    /// <summary>
    /// Model representing ticket.
    /// </summary>
    public class TicketModel
    {
        /// <summary>
        /// Gets or sets ticket event title.
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// Gets or sets ticket event start date.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Gets or sets ticket event end date.
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// Gets or sets when the ticket was bought.
        /// </summary>
        public DateTime Bought { get; set; }

        /// <summary>
        /// Gets or sets ticket price.
        /// </summary>
        public decimal Price { get; set; }
    }
}
