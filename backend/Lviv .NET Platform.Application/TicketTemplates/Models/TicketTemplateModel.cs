using System;

namespace Lviv_.NET_Platform.Application.TicketTemplates.Models
{
    public class TicketTemplateModel
    {
        public string Name { get; set; }

        public int EventId { get; set; }

        public decimal Price { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
