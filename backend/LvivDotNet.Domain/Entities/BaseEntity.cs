namespace LvivDotNet.Domain.Entities
{
    /// <summary>
    /// Base class for entities.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets entity id.
        /// </summary>
        public int Id { get; set; }
    }
}
