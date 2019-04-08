using Lviv_.NET_Platform.Application.Users.Commands.Login;
using Lviv_.NET_Platform.Application.Users.Commands.Refresh;
using Lviv_.NET_Platform.Application.Users.Commands.Register;
using Lviv_.NET_Platform.Application.Users.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Lviv_.NET_Platform.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController: Controller
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator) {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public Task<AuthTokensModel> Register([FromBody] RegisterUserCommand command)
            => mediator.Send(command);

        [AllowAnonymous]
        [HttpPost("refresh")]
        public Task<AuthTokensModel> Refresh([FromBody] RefreshTokenCommand command)
            => mediator.Send(command);

        [AllowAnonymous]
        [HttpPost("login")]
        public Task<AuthTokensModel> Login([FromBody] LoginCommand command)
            => mediator.Send(command);

        [HttpGet("test")]
        public void Test()
        {
            Console.WriteLine(User.Claims.ToString());
        }

    }
}