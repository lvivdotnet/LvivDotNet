using System.Collections.Generic;

namespace LvivDotNet.Domain.Entities
{
    /// <summary>
    /// Addition entity.
    /// </summary>
    public class Addition : BaseEntity
    {
        /// <summary>
        /// Gets or sets addition value represented as bytes array.
        /// </summary>
        public IEnumerable<byte> Blob { get; set; }

        /// <summary>
        /// Gets or sets addition title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets addition event id.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets addition post id.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets addition product id.
        /// </summary>
        public int ProductId { get; set; }
    }
}
