using System;
using System.Linq;
using System.Threading.Tasks;
using LvivDotNet.Common;
using LvivDotNet.Common.Extensions;
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
        /// Creates 3 ticket templates, request and updates them and delete.
        /// </summary>
        /// <returns> Task representing asynchronous operation. </returns>
        [Test]
        [Repeat(100)]
        public async Task FullTicketTemplates()
        {
            var addTicketTemplateCommands = Fakers.AddTicketTemplateCommand.Generate(3);

            var ids = await Task.WhenAll(addTicketTemplateCommands
                .Select(command =>
                {
                    command.EventId = this.EventId;
                    return command;
                })
                .Select(this.TicketTemplatesController.AddTicketTemplate));

            var ticketTemplates = (await this.EventsController.GetTicketTemplates(this.EventId)).ToList();

            Assert.AreEqual(addTicketTemplateCommands.Count, ticketTemplates.Count);

            addTicketTemplateCommands
                .Zip(ids, (command, id) => (command, id))
                .ForEach(tuple =>
                {
                    (var command, var i) = tuple;
                    var template = ticketTemplates.FirstOrDefault(t => t.Id == i);

                    Assert.NotNull(template);
                    Assert.AreEqual(command.EventId, template.EventId);
                    Assert.AreEqual(command.Name, template.Name);
                    Assert.IsTrue(command.From.IsEqual(template.From));
                    Assert.IsTrue(command.From.IsEqual(template.From));
                    Assert.IsTrue(Math.Abs(command.Price - template.Price) < 0.0001m);
                });

            var updateTicketTemplateCommands = Fakers.UpdateTicketTemplateCommand.Generate(3);

            await Task.WhenAll(updateTicketTemplateCommands
                .Zip(ticketTemplates, (updateCommand, ticketTemplate) => (updateCommand, ticketTemplate))
                .Select(async tuple =>
                {
                    tuple.updateCommand.Id = tuple.ticketTemplate.Id;

                    await this.TicketTemplatesController.UpdateTicketTemplate(tuple.updateCommand);

                    var result = await this.TicketTemplatesController.GetTicketTemplate(tuple.updateCommand.Id);

                    Assert.AreEqual(tuple.ticketTemplate.EventId, result.EventId);
                    Assert.AreEqual(tuple.updateCommand.Name, result.Name);
                    Assert.IsTrue(tuple.updateCommand.From.IsEqual(result.From));
                    Assert.IsTrue(tuple.updateCommand.From.IsEqual(result.From));
                    Assert.IsTrue(Math.Abs(tuple.updateCommand.Price - result.Price) < 0.0001m);
                }));

            await Task.WhenAll(ticketTemplates.Select(template => template.Id).Select(this.TicketTemplatesController.DeleteTicketTemplate));

            var requestedTicketTemplates = await this.EventsController.GetTicketTemplates(this.EventId);

            Assert.Zero(requestedTicketTemplates.Count());
        }
    }
}