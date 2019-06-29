namespace LvivDotNet.Domain.Entities
{
    /// <summary>
    /// Event attendee entity.
    /// </summary>
    public class Attendee : BaseEntity
    {
        /// <summary>
        /// Gets or sets attendee first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets attendee last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets attendee email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets attendee phone number.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets attendee sex.
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// Gets or sets attendee age.
        /// </summary>
        public int Age { get; set; }
    }
}
