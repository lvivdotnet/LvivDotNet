using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using LvivDotNet.Application.Users.Commands.Login;
using LvivDotNet.Application.Users.Commands.Register;
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
        /// <returns> <see cref="ControllerContext"/> with authorized user based on environment configuration. </returns>
        public static async Task<ControllerContext> GetAuthorizedContext(this IServiceProvider serviceProvider)
        {
            var mediator = serviceProvider.GetRequiredService<IMediator>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var userController = new UsersController(mediator);

            AuthTokensModel auth;

            auth = await userController.Login(new LoginCommand
            {
                Email = configuration["AdministratorEmail"],
                Password = configuration["AdministratorPassword"],
            });

            return GenerateControllerContext(auth.JwtToken);
        }

        /// <summary>
        /// Create <see cref="ControllerContext"/> with authorized user based on environment configuration.
        /// </summary>
        /// <param name="serviceProvider"> <see cref="IServiceProvider"/>. </param>
        /// <param name="registerUserCommand"> See <see cref="RegisterUserCommand"/>. </param>
        /// <returns> <see cref="ControllerContext"/> with authorized user based on environment configuration. </returns>
        public static async Task<ControllerContext> GetAuthorizedContext(this IServiceProvider serviceProvider, RegisterUserCommand registerUserCommand = null)
        {
            var mediator = serviceProvider.GetRequiredService<IMediator>();

            var userController = new UsersController(mediator);

            var auth = await userController.Register(registerUserCommand ?? Fakers.RegisterUserCommand.Generate());

            return GenerateControllerContext(auth.JwtToken);
        }

        /// <summary>
        /// Generate authorized controller context base on provided jwt token.
        /// </summary>
        /// <param name="token"> Jwt token on string representation. </param>
        /// <returns> See <see cref="ControllerContext"/>. </returns>
        public static ControllerContext GenerateControllerContext(string token)
        {
            var jwtTocken = SecurityHelpers.DecodeJwtToken(token);

            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        jwtTocken.Claims.GetClaim("id"),
                        jwtTocken.Claims.GetClaim(ClaimTypes.Role),
                    })),
                },
            };
        }
    }
}
