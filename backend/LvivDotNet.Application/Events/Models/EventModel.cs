using LvivDotNet.Application.TicketTemplates.Models;
using System;
using System.Collections.Generic;

namespace LvivDotNet.Application.Events.Models
{
    public class EventModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime PostDate { get; set; }

        public string Address { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int MaxAttendees { get; set; }

        public ICollection<TicketTemplateModel> TickerTemplates { get; set; }
    }
}
