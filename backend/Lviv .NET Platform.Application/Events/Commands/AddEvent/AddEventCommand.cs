using Lviv_.NET_Platform.Application.TicketTemplates.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace Lviv_.NET_Platform.Application.Events.Commands.AddEvent
{
    public class AddEventCommand : IRequest
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime PostDate { get; set; }

        public string Address { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int MaxAttendees { get; set; }

        public IEnumerable<TicketTemplateModel> TicketTemplates { get; set; }
    }
}
