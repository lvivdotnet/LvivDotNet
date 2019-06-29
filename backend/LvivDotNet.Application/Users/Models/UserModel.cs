using LvivDotNet.Domain.Entities;

namespace LvivDotNet.Application.Users.Models
{
    /// <summary>
    /// User model.
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets phone number.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets sex.
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// Gets or sets age.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets avatar.
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets salt.
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// Gets or sets role name.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets role id.
        /// </summary>
        public int RoleId { get; set; }
    }
}
