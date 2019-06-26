namespace LvivDotNet.Domain.Entities
{
    /// <summary>
    /// Order entity.
    /// </summary>
    public class Order : BaseEntity
    {
        /// <summary>
        /// Gets or sets order product id.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets order user id.
        /// </summary>
        public int UserId { get; set; }
    }
}
