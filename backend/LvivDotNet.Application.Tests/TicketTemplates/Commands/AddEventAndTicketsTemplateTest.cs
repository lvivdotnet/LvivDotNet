using System;
using System.Threading.Tasks;
using LvivDotNet.Application.Tests.Common;
using LvivDotNet.Application.TicketTemplates.Commands.AddTicketTemplate;
using LvivDotNet.Common;
using LvivDotNet.WebApi.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LvivDotNet.Application.Tests.TicketTemplates.Commands
{
    /// <summary>
    /// Add ticket template to event test.
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class AddEventAndTicketsTemplateTest : BaseTest
    {
        private int EventId { get; set; }

        private TicketTemplatesController TicketTemplatesController { get; set; }

        /// <summary>
        /// One-time test setup. Executed exactly once before all tests.
        /// Initialize Events controller, creates new event ans save event id.
        /// </summary>
        /// <returns> Task representing asynchronous operation. </returns>
        [OneTimeSetUp]
        public async Task RunBeforeAnyTests()
        {
            var eventsController = new EventsController(ServiceProvider.GetRequiredService<IMediator>());
            var addEventCommand = Fakers.AddEventCommand.Generate();
            this.EventId = await eventsController.AddEvent(addEventCommand);
            this.TicketTemplatesController = new TicketTemplatesController(ServiceProvider.GetRequiredService<IMediator>());
            this.TicketTemplatesController.ControllerContext = await ServiceProvider.GetAuthorizedContext();
        }

        /// <summary>
        /// Test ticket template creation.
        /// <see cref="AddTicketTemplateCommand"/>.
        /// </summary>
        /// <returns> Task representing asynchronous operation. </returns>
        [Test]
        [Repeat(500)]
        public async Task AddTicketsTemplate()
        {
            var addTicketTemplateCommand = Fakers.AddTicketTemplateCommand.Generate();

            addTicketTemplateCommand.EventId = this.EventId;

            var id = await this.TicketTemplatesController.AddTicketTemplate(addTicketTemplateCommand);

            var result = await this.TicketTemplatesController.GetTicketTemplate(id);

            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(addTicketTemplateCommand.EventId, result.EventId);
            Assert.IsTrue(Math.Abs(addTicketTemplateCommand.Price - result.Price) < 0.01m);
            Assert.IsTrue(addTicketTemplateCommand.From.IsEqual(result.From));
            Assert.IsTrue(addTicketTemplateCommand.To.IsEqual(result.To));
        }
    }
}
