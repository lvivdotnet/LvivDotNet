using System;

namespace Lviv_.NET_Platform.Domain.Entities
{
    public class Event: BaseEntity
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime PostDate { get; set; }

        public string Address { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int MaxAttendees { get; set; }
    }
}
