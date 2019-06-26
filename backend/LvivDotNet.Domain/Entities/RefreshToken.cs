using System;

namespace LvivDotNet.Domain.Entities
{
    /// <summary>
    /// Refresh token entity.
    /// </summary>
    public class RefreshToken : BaseEntity
    {
        /// <summary>
        /// Gets or sets refresh token user id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets refresh token expiration date and time.
        /// </summary>
        public DateTime Expires { get; set; }
    }
}
