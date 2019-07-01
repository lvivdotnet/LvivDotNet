using System;

namespace LvivDotNet.Domain.Entities
{
    /// <summary>
    /// Post entity.
    /// </summary>
    public class Post : BaseEntity
    {
        /// <summary>
        /// Gets or sets post title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets post body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets post creation date and time.
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// Gets or sets post author id.
        /// </summary>
        public int AuthorId { get; set; }
    }
}
