using System;

namespace LvivDotNet.Application.TicketTemplates.Models
{
    /// <summary>
    /// Ticket template model.
    /// </summary>
    public class TicketTemplateModel
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets event id.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets date from.
        /// </summary>
        public DateTime From { get; set; }

        /// <summary>
        /// Gets or sets date to.
        /// </summary>
        public DateTime To { get; set; }
    }
}
