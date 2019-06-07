using LvivDotNet.Application.Users.Models;
using LvivDotNet.Domain.Entities;
using MediatR;

namespace LvivDotNet.Application.Users.Commands.Register
{
    public class RegisterUserCommand : IRequest<AuthTokensModel>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public Sex Sex { get; set; }

        public int Age { get; set; }

        public string Avatar { get; set; }

        public string Password { get; set; }
    }
}