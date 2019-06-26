using System;

namespace LvivDotNet.Domain.Entities
{
    /// <summary>
    /// Ticket template entity.
    /// </summary>
    public class TicketTemplate : BaseEntity
    {
        /// <summary>
        /// Gets or sets ticket template name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets ticket template event id.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets ticket template price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets ticket template selling start date and time.
        /// </summary>
        public DateTime From { get; set; }

        /// <summary>
        /// Gets or sets ticket template selling end date and time.
        /// </summary>
        public DateTime To { get; set; }
    }
}
