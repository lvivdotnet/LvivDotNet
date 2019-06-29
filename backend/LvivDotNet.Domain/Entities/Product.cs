namespace LvivDotNet.Domain.Entities
{
    /// <summary>
    /// Product entity.
    /// </summary>
    public class Product : BaseEntity
    {
        /// <summary>
        /// Gets or sets product name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets product description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets products count.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets product price.
        /// </summary>
        public decimal Price { get; set; }
    }
}
