using System.Threading.Tasks;
using LvivDotNet.Application.Users.Commands.Login;
using LvivDotNet.Application.Users.Commands.Logout;
using LvivDotNet.Application.Users.Commands.Refresh;
using LvivDotNet.WebApi.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LvivDotNet.Application.Tests.Users
{
    /// <summary>
    /// User authorization and authentication workflow test.
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class UserAuthorizationTest : BaseTest
    {
        private UsersController Controller { get; set; }

        /// <summary>
        /// One-time test setup. Executed exactly once before all tests.
        /// </summary>
        [OneTimeSetUp]
        public void SetUp()
        {
            this.Controller = new UsersController(ServiceProvider.GetRequiredService<IMediator>());
        }

        /// <summary>
        /// Runs thought all auth logic and tests results.
        /// At first it will try to register new user and logout him.
        /// Then it will login this user, try to refresh jwt token two times.
        /// And in the end it will logout him one more time.
        /// </summary>
        /// <returns> Void. </returns>
        [Test]
        [Repeat(100)]
        public async Task UserAuthorization()
        {
            var registerUserCommand = Fakers.RegisterUserCommand.Generate();

            var registerAuthModel = await this.Controller.Register(registerUserCommand);

            Assert.AreEqual(registerUserCommand.Email, registerAuthModel.Email);
            Assert.AreEqual(registerUserCommand.FirstName, registerAuthModel.FirstName);
            Assert.AreEqual(registerUserCommand.LastName, registerAuthModel.LastName);
            Assert.AreEqual("User", registerAuthModel.Role);

            var logoutCommand = new LogoutCommand
            {
                RefreshToken = registerAuthModel.RefreshToken,
                Token = registerAuthModel.JwtToken,
            };

            await this.Controller.Logout(logoutCommand);

            var loginCommand = new LoginCommand
            {
                Email = registerUserCommand.Email,
                Password = registerUserCommand.Password,
            };

            var loginAuthModel = await this.Controller.Login(loginCommand);

            Assert.AreEqual(loginAuthModel.Email, registerAuthModel.Email);
            Assert.AreEqual(loginAuthModel.FirstName, registerAuthModel.FirstName);
            Assert.AreEqual(loginAuthModel.LastName, registerAuthModel.LastName);
            Assert.AreEqual("User", loginAuthModel.Role);

            var refreshTokenCommand = new RefreshTokenCommand
            {
                RefreshToken = loginAuthModel.RefreshToken,
                JwtToken = loginAuthModel.JwtToken,
            };

            var refreshAuthModel = await this.Controller.Refresh(refreshTokenCommand);

            Assert.AreEqual(refreshAuthModel.Email, registerAuthModel.Email);
            Assert.AreEqual(refreshAuthModel.FirstName, registerAuthModel.FirstName);
            Assert.AreEqual(refreshAuthModel.LastName, registerAuthModel.LastName);
            Assert.AreEqual("User", refreshAuthModel.Role);

            var secondRefreshTokenCommand = new RefreshTokenCommand
            {
                RefreshToken = refreshAuthModel.RefreshToken,
                JwtToken = refreshAuthModel.JwtToken,
            };

            var secondRefreshAuthModel = await this.Controller.Refresh(secondRefreshTokenCommand);

            Assert.AreEqual(secondRefreshAuthModel.Email, registerAuthModel.Email);
            Assert.AreEqual(secondRefreshAuthModel.FirstName, registerAuthModel.FirstName);
            Assert.AreEqual(secondRefreshAuthModel.LastName, registerAuthModel.LastName);
            Assert.AreEqual("User", secondRefreshAuthModel.Role);

            var secondLogoutCommand = new LogoutCommand
            {
                RefreshToken = secondRefreshAuthModel.RefreshToken,
                Token = secondRefreshAuthModel.JwtToken,
            };

            await this.Controller.Logout(secondLogoutCommand);
        }
    }
}