using System.Threading.Tasks;
using LvivDotNet.Application.Tests.Common;
using LvivDotNet.WebApi.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LvivDotNet.Application.Tests.Users.Commands
{
    /// <summary>
    /// Update user command test.
    /// </summary>
    [TestFixture]
    public class UpdateUserCommandTest : BaseTest
    {
        /// <summary>
        /// Gets or sets users controller.
        /// </summary>
        private UsersController UsersController { get; set; }

        /// <summary>
        /// One-time test setup. Executed exactly once before all tests.
        /// Initialize Events controller, creates new event ans save event id.
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            this.UsersController = new UsersController(ServiceProvider.GetRequiredService<IMediator>());
        }

        /// <summary>
        /// Successful path for update user command test.
        /// </summary>
        /// <returns> Task representing asynchronous operation. </returns>
        [Test]
        [Repeat(50)]
        public async Task UpdateUserCommandSuccessfulTest()
        {
            // Arrange
            var registerUserCommand = Fakers.RegisterUserCommand.Generate();

            var auth = await this.UsersController.Register(registerUserCommand);

            this.UsersController.ControllerContext = AuthHelpers.GenerateControllerContext(auth.JwtToken);

            var updateUserCommand = Fakers.UpdateUserCommand.Generate();

            // Act
            await this.UsersController.UpdateUser(updateUserCommand);

            var user = await this.UsersController.GetUserInfo();

            // Assert
            Assert.AreEqual(updateUserCommand.FirstName, user.FirstName);
            Assert.AreEqual(updateUserCommand.LastName, user.LastName);
            Assert.AreEqual(updateUserCommand.Email, user.Email);
            Assert.AreEqual(updateUserCommand.Phone, user.Phone);
            Assert.AreEqual(updateUserCommand.Sex, user.Sex);
            Assert.AreEqual(updateUserCommand.Age, user.Age);
            Assert.AreEqual(updateUserCommand.Avatar, user.Avatar);
        }
    }
}
