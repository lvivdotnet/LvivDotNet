using System;

namespace Lviv_.NET_Platform.Domain.Entities
{
    public class Ticket: BaseEntity
    {
        public int TicketTemplateId { get; set; }

        public int AttendeeId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UserId { get; set; }
    }
}
