using Lviv_.NET_Platform.Application.Users.Models;
using Lviv_.NET_Platform.Domain.Entities;
using MediatR;

namespace Lviv_.NET_Platform.Application.Users.Commands.Register
{
    public class RegisterUserCommand: IRequest<AuthTokensModel>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public Male Male { get; set; }

        public int Age { get; set; }

        public string Avatar { get; set; }

        public string Password { get; set; }
    }
}