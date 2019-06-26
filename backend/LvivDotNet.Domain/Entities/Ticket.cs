using System;

namespace LvivDotNet.Domain.Entities
{
    /// <summary>
    /// Ticket entity.
    /// </summary>
    public class Ticket : BaseEntity
    {
        /// <summary>
        /// Gets or sets ticket template id.
        /// </summary>
        public int TicketTemplateId { get; set; }

        /// <summary>
        /// Gets or sets attendee id.
        /// </summary>
        public int AttendeeId { get; set; }

        /// <summary>
        /// Gets or sets ticket creating date and time.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets ticket owner user id.
        /// </summary>
        public int UserId { get; set; }
    }
}
