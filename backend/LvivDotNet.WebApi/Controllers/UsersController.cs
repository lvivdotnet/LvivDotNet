using LvivDotNet.Application.Users.Commands.Login;
using LvivDotNet.Application.Users.Commands.Refresh;
using LvivDotNet.Application.Users.Commands.Register;
using LvivDotNet.Application.Users.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LvivDotNet.Application.Users.Commands.Logout;

namespace LvivDotNet.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
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

        [HttpPost("logout")]
        public Task<LogoutCommand> Logout([FromBody] LogoutCommand command)
            => mediator.Send(command)'

    }
}