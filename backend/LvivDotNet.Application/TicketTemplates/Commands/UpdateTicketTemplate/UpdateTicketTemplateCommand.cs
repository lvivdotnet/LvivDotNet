using System;
using LvivDotNet.Application.TicketTemplates.Models;
using MediatR;

namespace LvivDotNet.Application.TicketTemplates.Commands.UpdateTicketTemplate
{
    /// <summary>
    /// Update ticket template command.
    /// </summary>
    public class UpdateTicketTemplateCommand : IRequest<TicketTemplateModel>
    {
        /// <summary>
        /// Gets or sets ticket template id.
        /// </summary>
        public int Id { get; set; }

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
