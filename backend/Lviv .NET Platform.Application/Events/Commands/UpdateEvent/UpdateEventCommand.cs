using MediatR;
using System;

namespace Lviv_.NET_Platform.Application.Events.Commands.UpdateEvent
{
    public class UpdateEventCommand : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Address { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int MaxAttendees { get; set; }
    }
}
