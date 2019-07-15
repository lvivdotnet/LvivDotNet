using System;
using System.Threading.Tasks;
using LvivDotNet.Application.Events.Commands.AddEvent;
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
        /// <summary>
        /// Tests new event creation.
        /// <see cref="AddEventCommand"/>.
        /// </summary>
        /// <returns> Task representing asynchronous operation. </returns>
        [Test]
        [Repeat(500)]
        public async Task AddEvent()
        {
            var controller = new EventsController(ServiceProvider.GetRequiredService<IMediator>());
            var command = Fakers.AddEventCommand.Generate();

            var result = await controller.AddEvent(command);

            var @event = await controller.GetEvent(result);

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
