using System;
using MediatR;

namespace LvivDotNet.Application.Events.Commands.UpdateEvent
{
    /// <summary>
    /// Update event command.
    /// </summary>
    public class UpdateEventCommand : IRequest
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
        /// Gets or sets event max. attendees count.
        /// </summary>
        public int MaxAttendees { get; set; }
    }
}
