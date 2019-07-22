using System;
using System.Security.Claims;
using System.Threading.Tasks;
using LvivDotNet.Application.Users.Commands.Login;
using LvivDotNet.Application.Users.Models;
using LvivDotNet.Common;
using LvivDotNet.Common.Extensions;
using LvivDotNet.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LvivDotNet.Application.Tests.Common
{
    /// <summary>
    /// Extension methods for tests authorization.
    /// </summary>
    public static class AuthHelpers
    {
        /// <summary>
        /// Create <see cref="ControllerContext"/> with authorized user based on environment configuration.
        /// </summary>
        /// <param name="serviceProvider"> <see cref="IServiceProvider"/>. </param>
        /// <param name="useEnvironment"> Indicates whenever use default administrator user from environment variables or create new plain user. </param>
        /// <returns> <see cref="ControllerContext"/> with authorized user based on environment configuration. </returns>
        public static async Task<ControllerContext> GetAuthorizedContext(this IServiceProvider serviceProvider, bool useEnvironment = true)
        {
            var mediator = serviceProvider.GetRequiredService<IMediator>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var userController = new UsersController(mediator);

            AuthTokensModel auth;

            if (useEnvironment)
            {
                auth = await userController.Login(new LoginCommand
                {
                    Email = configuration["AdministratorEmail"],
                    Password = configuration["AdministratorPassword"],
                });
            }
            else
            {
                auth = await userController.Register(Fakers.RegisterUserCommand.Generate());
            }

            var token = SecurityHelpers.DecodeJwtToken(auth.JwtToken);

            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        token.Claims.GetClaim("id"),
                        token.Claims.GetClaim(ClaimTypes.Role),
                    })),
                },
            };
        }
    }
}
