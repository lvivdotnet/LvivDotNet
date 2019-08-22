using System.Threading.Tasks;
using LvivDotNet.Application.Tests.Common;
using LvivDotNet.Application.Users.Queries.GetUserInfo;
using LvivDotNet.WebApi.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LvivDotNet.Application.Tests.Users.Queries
{
    /// <summary>
    /// Tests of <see cref="GetUserInfoQuery"/>.
    /// </summary>
    [TestFixture]
    public class GetUserInfoTest : BaseTest
    {
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
        /// Successful path for <see cref="GetUserInfoQuery"/>.
        /// </summary>
        /// <returns> Task representing asynchronous operation. </returns>
        [Test]
        public async Task GetUserInfoSuccessful()
        {
            // Arrange
            var registeUserCommand = Fakers.RegisterUserCommand.Generate();
            var auth = await this.UsersController.Register(registeUserCommand);
            this.UsersController.ControllerContext = AuthHelpers.GenerateControllerContext(auth.JwtToken);

            // Act
            var userInfo = await this.UsersController.GetUserInfo();

            // Assert
            Assert.AreEqual(registeUserCommand.Age, userInfo.Age);
            Assert.AreEqual(registeUserCommand.Avatar, userInfo.Avatar);
            Assert.AreEqual(registeUserCommand.Email, userInfo.Email);
            Assert.AreEqual(registeUserCommand.Sex, userInfo.Sex);
            Assert.AreEqual(registeUserCommand.FirstName, userInfo.FirstName);
            Assert.AreEqual(registeUserCommand.LastName, userInfo.LastName);
            Assert.AreEqual(registeUserCommand.Phone, userInfo.Phone);
            Assert.AreEqual(auth.Role, userInfo.RoleName);
        }
    }
}
