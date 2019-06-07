using System;

namespace LvivDotNet.Application.TicketTemplates.Models
{
    public class TicketTemplateModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int EventId { get; set; }

        public decimal Price { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
