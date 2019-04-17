using System;

namespace Lviv_.NET_Platform.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public int UserId { get; set; }

        public DateTime Expires { get; set; }
    }
}
