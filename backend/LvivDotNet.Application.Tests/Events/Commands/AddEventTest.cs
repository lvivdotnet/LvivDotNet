using System;
using System.Threading.Tasks;
using LvivDotNet.Application.Events.Commands.AddEvent;
using LvivDotNet.Application.Tests.Common;
using LvivDotNet.Common;
using LvivDotNet.WebApi.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LvivDotNet.Application.Tests.Events.Commands
{
    /// <summary>
    /// Add event test.
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class AddEventTest : BaseTest
    {
        private EventsController EventsController { get; set; }

        /// <summary>
        /// One-time test setup. Executed exactly once before all tests.
        /// </summary>
        /// <returns> Task representing asynchronous operation. </returns>
        [OneTimeSetUp]
        public async Task SetUp()
        {
            this.EventsController = new EventsController(ServiceProvider.GetRequiredService<IMediator>());
            JwtToken = await ServiceProvider.GetAuthorizedJwtToken();
        }

        /// <summary>
        /// Tests new event creation.
        /// <see cref="AddEventCommand"/>.
        /// </summary>
        /// <returns> Task representing asynchronous operation. </returns>
        [Test]
        [Repeat(500)]
        public async Task AddEvent()
        {
            var command = Fakers.AddEventCommand.Generate();

            var result = await this.EventsController.AddEvent(command);

            var @event = await this.EventsController.GetEvent(result);

            Assert.AreEqual(command.Address, @event.Address);
            Assert.AreEqual(command.Description, @event.Description);
            Assert.IsTrue(command.EndDate.IsEqual(@event.EndDate));
            Assert.IsTrue(command.StartDate.IsEqual(@event.StartDate));
            Assert.AreEqual(command.MaxAttendees, @event.MaxAttendees);
            Assert.AreEqual(command.Name, @event.Name);
            Assert.AreEqual(command.Title, @event.Title);
            Assert.Less(DateTime.UtcNow.Subtract(@event.PostDate), TimeSpan.FromSeconds(5));
            Assert.IsEmpty(@event.TickerTemplates);
        }
    }
}
