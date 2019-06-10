using Bogus;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using LvivDotNet.Application.TicketTemplates.Commands.AddTicketTemplate;
using LvivDotNet.Common;
using LvivDotNet.Controllers;
using LvivDotNet.WebApi.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LvivDotNet.Application.Tests.TicketTemplates.Commands
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class AddEventAndTicketsTemplateTest : BaseTest
    {
        private int EventId { get; set; }

        [OneTimeSetUp]
        public async Task RunBeforeAnyTests()
        {
            var eventsController = new EventsController(ServiceProvider.GetRequiredService<IMediator>());
            var addEventCommand = Fakers.AddEventCommand.Generate();
            this.EventId = await eventsController.AddEvent(addEventCommand);
        }
        
        [Test]
        [Repeat(500)]
        public async Task Test()
        {
            var ticketTemplatesController = new TicketTemplatesController(ServiceProvider.GetRequiredService<IMediator>());
            var addTicketTemplateCommand = Fakers.AddTicketTemplateCommand.Generate();

            addTicketTemplateCommand.EventId = EventId;

            var id = await ticketTemplatesController.AddTicketTemplate(addTicketTemplateCommand);

            var result = await ticketTemplatesController.GetTicketTemplate(id);

            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(addTicketTemplateCommand.EventId, result.EventId);
            Assert.IsTrue(Math.Abs(addTicketTemplateCommand.Price - result.Price) < 0.0001m);
            Assert.IsTrue(addTicketTemplateCommand.From.IsEqual(result.From));
            Assert.IsTrue(addTicketTemplateCommand.To.IsEqual(result.To));
        }
    }
}
