using System.Threading.Tasks;
using LvivDotNet.Common;
using LvivDotNet.WebApi.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LvivDotNet.Application.Tests.Users
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class UserAuthorizationTest : BaseTest
    {
        private UsersController Controller { get; set; }

        [OneTimeSetUp]
        public void SetUp()
        {
            this.Controller = new UsersController(this.ServiceProvider.GetRequiredService<IMediator>());
        }

        [Test]
        public async Task Test()
        {
            var registerUserCommand = Fakers.RegisterUserCommand.Generate();

            var authModel = await this.Controller.Register(registerUserCommand);

            Assert.AreEqual(registerUserCommand.Email, authModel.Email);
            Assert.AreEqual(registerUserCommand.FirstName, authModel.FirstName);
            Assert.AreEqual(registerUserCommand.LastName, authModel.LastName);
            Assert.AreEqual("User", authModel.Role);
    }
}