using System;
using MediatR;

namespace LvivDotNet.Application.TicketTemplates.Commands.AddTicketTemplate
{
    /// <summary>
    /// Add ticket template command.
    /// </summary>
    public class AddTicketTemplateCommand : IRequest<int>
    {
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
        /// Gets or sets from date.
        /// </summary>
        public DateTime From { get; set; }

        /// <summary>
        /// Gets or sets date to.
        /// </summary>
        public DateTime To { get; set; }
    }
}
