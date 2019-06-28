using System;
using System.Linq;
using System.Threading.Tasks;
using LvivDotNet.Common;
using LvivDotNet.Controllers;
using LvivDotNet.WebApi.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LvivDotNet.Application.Tests.TicketTemplates
{
    /// <summary>
    /// Ticket templates workflow test.
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class FullTicketTemplatesTest : BaseTest
    {
        private int EventId { get; set; }

        private TicketTemplatesController TicketTemplatesController { get; set; }

        private EventsController EventsController { get; set; }

        /// <summary>
        /// One-time test setup. Executed exactly once before all tests.
        /// Initialize Events and TicketTemplates controllers, add new event in database and save event id.
        /// </summary>
        /// <returns> Task representing asynchronous operation. </returns>
        [OneTimeSetUp]
        public async Task RunBeforeAnyTests()
        {
            this.TicketTemplatesController = new TicketTemplatesController(ServiceProvider.GetRequiredService<IMediator>());
            this.EventsController = new EventsController(ServiceProvider.GetRequiredService<IMediator>());
            var addEventCommand = Fakers.AddEventCommand.Generate();
            this.EventId = await this.EventsController.AddEvent(addEventCommand);
        }

        /// <summary>
        /// Runs thought all logic connected to ticket templates.
        /// Creates 3 ticket templates, request them and delete.
        /// </summary>
        /// <returns> Task representing asynchronous operation. </returns>
        [Test]
        [Repeat(100)]
        public async Task FullTicketTemplates()
        {
            var addTicketTemplateCommands = Fakers.AddTicketTemplateCommand.Generate(3);

            var ids = await Task.WhenAll(addTicketTemplateCommands.Select(async command =>
            {
                command.EventId = this.EventId;
                return await this.TicketTemplatesController.AddTicketTemplate(command);
            }));

            var ticketTemplates = (await this.EventsController.GetTicketTemplates(this.EventId)).ToList();

            Assert.AreEqual(addTicketTemplateCommands.Count, ticketTemplates.Count);

            foreach (var (command, i) in addTicketTemplateCommands.Zip(ids, (command, id) => (command, id)))
            {
                var template = ticketTemplates.FirstOrDefault(t => t.Id == i);

                Assert.NotNull(template);
                Assert.AreEqual(command.EventId, template.EventId);
                Assert.AreEqual(command.Name, template.Name);
                Assert.IsTrue(command.From.IsEqual(template.From));
                Assert.IsTrue(command.From.IsEqual(template.From));
                Assert.IsTrue(Math.Abs(command.Price - template.Price) < 0.0001m);
            }

            await Task.WhenAll(ticketTemplates.Select(async template => await this.TicketTemplatesController.DeleteTicketTemplate(template.Id)));

            ticketTemplates = (await this.EventsController.GetTicketTemplates(this.EventId)).ToList();

            Assert.Zero(ticketTemplates.Count);
        }
    }
}