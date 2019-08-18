using System.Threading.Tasks;
using LvivDotNet.Application.Users.Commands.Login;
using LvivDotNet.Application.Users.Commands.Logout;
using LvivDotNet.Application.Users.Commands.Refresh;
using LvivDotNet.Application.Users.Commands.Register;
using LvivDotNet.Application.Users.Models;
using LvivDotNet.Application.Users.Queries.GetUserInfo;
using LvivDotNet.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LvivDotNet.WebApi.Controllers
{
    /// <summary>
    /// Users controller.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="mediator"> Mediator service. </param>
        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Register user.
        /// </summary>
        /// <param name="command"> User registration command. </param>
        /// <returns> User auth model. </returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public Task<AuthTokensModel> Register([FromBody] RegisterUserCommand command)
            => this.mediator.Send(command);

        /// <summary>
        /// Refresh JWT token.
        /// </summary>
        /// <param name="command"> Refresh JWT token command. </param>
        /// <returns> User auth model. </returns>
        [AllowAnonymous]
        [HttpPost("refresh")]
        public Task<AuthTokensModel> Refresh([FromBody] RefreshTokenCommand command)
            => this.mediator.Send(command);

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="command"> User login command. </param>
        /// <returns> User auth model. </returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public Task<AuthTokensModel> Login([FromBody] LoginCommand command)
            => this.mediator.Send(command);

        /// <summary>
        /// Logout user.
        /// </summary>
        /// <param name="command"> User logout command. </param>
        /// <returns> Empty task. </returns>
        [HttpPost("logout")]
        public Task Logout([FromBody] LogoutCommand command)
            => this.mediator.Send(command);

        /// <summary>
        /// Returns information about current user.
        /// </summary>
        /// <returns> See <see cref="UserInfoModel"/>. </returns>
        [HttpGet]
        public Task<UserInfoModel> GetUserInfo()
            => this.mediator.Send(new GetUserInfoQuery { UserId = this.User.GetId() });
    }
}