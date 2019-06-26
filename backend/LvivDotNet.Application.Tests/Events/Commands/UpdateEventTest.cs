using System.Threading.Tasks;
using LvivDotNet.Common;
using LvivDotNet.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LvivDotNet.Application.Tests.Events.Commands
{
    /// <summary>
    /// Update event test.
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class UpdateEventTest : BaseTest
    {
        private int EventId { get; set; }

        private EventsController Controller { get; set; }

        /// <summary>
        /// Test setup.
        /// </summary>
        /// <returns> Void. </returns>
        [OneTimeSetUp]
        public async Task SetUp()
        {
            this.Controller = new EventsController(ServiceProvider.GetRequiredService<IMediator>());

            var command = Fakers.AddEventCommand.Generate();

            this.EventId = await this.Controller.AddEvent(command);
        }

        /// <summary>
        /// Test.
        /// </summary>
        /// <returns> Void. </returns>
        [Test]
        [Repeat(500)]
        public async Task UpdateEvent()
        {
            var command = Fakers.UpdateEventCommand.Generate();
            command.Id = this.EventId;

            await this.Controller.UpdateEvent(command);

            var result = await this.Controller.GetEvent(this.EventId);

            Assert.AreEqual(command.Address, result.Address);
            Assert.AreEqual(command.Description, result.Description);
            Assert.IsTrue(command.EndDate.IsEqual(result.EndDate));
            Assert.IsTrue(command.StartDate.IsEqual(result.StartDate));
            Assert.AreEqual(command.MaxAttendees, result.MaxAttendees);
            Assert.AreEqual(command.Name, result.Name);
            Assert.AreEqual(command.Title, result.Title);
        }
    }
}
