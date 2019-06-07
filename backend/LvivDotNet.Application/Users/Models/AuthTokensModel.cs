namespace LvivDotNet.Application.Users.Models
{
    public class AuthTokensModel
    {
        public string JwtToken { get; set; }

        public string RefreshToken { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
}