namespace Lviv_.NET_Platform.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public Sex Sex { get; set; }

        public int Age { get; set; }

        public string Avatar { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; }
    }
}
