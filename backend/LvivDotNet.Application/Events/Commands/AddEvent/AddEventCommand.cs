using MediatR;
using System;

namespace LvivDotNet.Application.Events.Commands.AddEvent
{
    public class AddEventCommand : IRequest<int>
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Address { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int MaxAttendees { get; set; }
    }
}
