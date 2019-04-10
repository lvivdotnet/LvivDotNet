using Lviv_.NET_Platform.Domain.Entities;

namespace Lviv_.NET_Platform.Application.Users.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public Sex Sex { get; set; }

        public int Age { get; set; }

        public string Avatar { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public string RoleName { get; set; }

        public int RoleId { get; set; }
    }
}
