using System;

namespace LvivDotNet.Domain.Entities
{
    /// <summary>
    /// Event entity.
    /// </summary>
    public class Event : BaseEntity
    {
        /// <summary>
        /// Gets or sets event name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets event start date and time.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets event end date and time.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets event post date and time.
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// Gets or sets event address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets event title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets event description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets event max attendees amount.
        /// </summary>
        public int MaxAttendees { get; set; }
    }
}
