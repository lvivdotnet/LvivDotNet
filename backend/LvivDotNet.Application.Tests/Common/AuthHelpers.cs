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
        /// Create JWT token with authorized user based on environment configuration.
        /// </summary>
        /// <param name="serviceProvider"> <see cref="IServiceProvider"/>. </param>
        /// <returns> Jwt token with authorized user based on environment configuration. </returns>
        public static async Task<string> GetAuthorizedJwtToken(this IServiceProvider serviceProvider)
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

            return auth.JwtToken;
        }

        /// <summary>
        /// Generates <see cref="ClaimsPrincipal"/> with assigned user id based on JWT token.
        /// </summary>
        /// <param name="token"> Jwt token on string representation. </param>
        /// <returns> See <see cref="ClaimsPrincipal"/>. </returns>
        public static ClaimsPrincipal GenerateClaims(string token)
        {
            if (token == null)
            {
                return null;
            }

            var jwtTocken = SecurityHelpers.DecodeJwtToken(token);
            return new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        jwtTocken.Claims.GetClaim("id"),
                        jwtTocken.Claims.GetClaim(ClaimTypes.Role),
                    }));
        }
    }
}
