using System;

namespace LvivDotNet.Domain.Entities
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime PostDate { get; set; }

        public int AuthorId { get; set; }
    }
}
