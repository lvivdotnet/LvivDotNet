namespace LvivDotNet.Application.Users.Models
{
    /// <summary>
    /// Authorization response model.
    /// </summary>
    public class AuthTokensModel
    {
        /// <summary>
        /// Gets or sets token.
        /// </summary>
        public string JwtToken { get; set; }

        /// <summary>
        /// Gets or sets refresh token.
        /// </summary>
        public string RefreshToken { get; set; }

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
        /// Gets or sets role.
        /// </summary>
        public string Role { get; set; }
    }
}