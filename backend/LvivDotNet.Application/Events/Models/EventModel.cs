using System;
using System.Collections.Generic;
using LvivDotNet.Application.TicketTemplates.Models;

namespace LvivDotNet.Application.Events.Models
{
    /// <summary>
    /// Event model.
    /// </summary>
    public class EventModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets event start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets event end date.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets event post date.
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// Gets or sets address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets max. attendees count.
        /// </summary>
        public int MaxAttendees { get; set; }

        /// <summary>
        /// Gets ticket templates collection.
        /// </summary>
        public ICollection<TicketTemplateModel> TickerTemplates { get; private set; } = new List<TicketTemplateModel>();
    }
}
