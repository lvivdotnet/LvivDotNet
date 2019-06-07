using System;

namespace LvivDotNet.Application.Events.Models
{
    public class EventShortModel
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Address { get; set; }

        public string Title { get; set; }
    }
}