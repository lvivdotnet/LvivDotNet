using System;
using LvivDotNet.Common;
using LvivDotNet.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace LvivDotNet.Application.Tests.Events.Commands
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class AddEventTest : BaseTest
    {
        [Test]
        [Repeat(500)]
        public async Task AddEvent()
        {
            var controller = new EventsController(ServiceProvider.GetRequiredService<IMediator>());
            var command = Fakers.AddEventCommand.Generate();;

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
