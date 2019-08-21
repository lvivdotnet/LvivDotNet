using System;
using System.Linq;
using System.Threading.Tasks;
using LvivDotNet.Application.Exceptions;
using LvivDotNet.Application.Tests.Common;
using LvivDotNet.Common;
using LvivDotNet.Common.Extensions;
using LvivDotNet.WebApi.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LvivDotNet.Application.Tests.Tickets.Commands.Authorized
{
    /// <summary>
    /// Buy ticket by authorized user logic test.
    /// </summary>
    [TestFixture]
    public class BuyTicketTest : BaseTest
    {
        /// <summary>
        /// Gets or sets tickets controller.
        /// </summary>
        private TicketsController TicketsController { get; set; }

        /// <summary>
        /// Gets or sets events controller.
        /// </summary>
        private EventsController EventsController { get; set; }

        /// <summary>
        /// Gets or sets ticket templates controller.
        /// </summary>
        private TicketTemplatesController TicketTemplatesController { get; set; }

        /// <summary>
        /// One-time test setup. Executed exactly once before all tests.
        /// Initialize Events, TicketTemplates, and Tickets controllers, register new user and save email.
        /// </summary>
        /// <returns> Task representing asynchronous operation. </returns>
        [OneTimeSetUp]
        public async Task SetUp()
        {
            var mediator = ServiceProvider.GetRequiredService<IMediator>();
            this.TicketsController = new TicketsController(mediator);
            this.EventsController = new EventsController(mediator);
            this.TicketTemplatesController = new TicketTemplatesController(mediator);

            var authorizedContext = await ServiceProvider.GetAuthorizedContext();

            this.TicketsController.ControllerContext = authorizedContext;
            this.EventsController.ControllerContext = authorizedContext;
            this.TicketTemplatesController.ControllerContext = authorizedContext;
        }

        /// <summary>
        /// Creates event with ticket template, buy ticket, get this ticket and check.
        /// </summary>
        /// <returns> Task representing asynchronous operation. </returns>
        [Test]
        [Repeat(20)]
        public async Task BuyTicket()
        {
            var addEventCommand = Fakers.AddEventCommand.Generate();
            addEventCommand.MaxAttendees = 2;

            var eventId = await this.EventsController.AddEvent(addEventCommand);

            var addTicketTempaltesCommands = Fakers.AddTicketTemplateCommand.Generate(3);
            addTicketTempaltesCommands[0].From = DateTime.UtcNow.AddDays(-10);
            addTicketTempaltesCommands[0].To = DateTime.UtcNow.AddDays(-3);
            addTicketTempaltesCommands[1].From = DateTime.UtcNow.AddDays(-3);
            addTicketTempaltesCommands[1].To = DateTime.UtcNow.AddDays(3);
            addTicketTempaltesCommands[2].From = DateTime.UtcNow.AddDays(3);
            addTicketTempaltesCommands[2].To = DateTime.UtcNow.AddDays(10);

            await addTicketTempaltesCommands
                .Select(command =>
                {
                    command.EventId = eventId;
                    return command;
                })
                .ForEach(async command => await this.TicketTemplatesController.AddTicketTemplate(command));

            var ticketId = await this.TicketsController.BuyTicket(eventId);

            var ticket = await this.TicketsController.GetTicketById(ticketId);

            Assert.AreEqual(addEventCommand.Name, ticket.EventName);
            Assert.IsTrue(addEventCommand.StartDate.IsEqual(ticket.Start));
            Assert.IsTrue(addEventCommand.EndDate.IsEqual(ticket.End));
            Assert.Less(DateTime.UtcNow.Subtract(ticket.Bought), TimeSpan.FromSeconds(5));
            Assert.Less(Math.Abs(addTicketTempaltesCommands[1].Price - ticket.Price), 0.01m);
        }

        /// <summary>
        /// Creates event with ticket template, buy ticket and throws <see cref="TicketsNotAvailable"/> exception.
        /// </summary>
        /// <returns> Task representing asynchronous operation. </returns>
        [Test]
        [Repeat(20)]
        public async Task BuyTicketWrongTime()
        {
            var addEventCommand = Fakers.AddEventCommand.Generate();
            addEventCommand.MaxAttendees = 2;

            var eventId = await this.EventsController.AddEvent(addEventCommand);

            var addTicketTempaltesCommands = Fakers.AddTicketTemplateCommand.Generate(2);
            addTicketTempaltesCommands[0].From = DateTime.UtcNow.AddDays(-10);
            addTicketTempaltesCommands[0].To = DateTime.UtcNow.AddDays(-3);
            addTicketTempaltesCommands[1].From = DateTime.UtcNow.AddDays(3);
            addTicketTempaltesCommands[1].To = DateTime.UtcNow.AddDays(10);

            await addTicketTempaltesCommands
                .Select(command =>
                {
                    command.EventId = eventId;
                    return command;
                })
                .ForEach(async command => await this.TicketTemplatesController.AddTicketTemplate(command));

            Assert.ThrowsAsync<TicketsNotAvailable>(async () => await this.TicketsController.BuyTicket(eventId));
        }

        /// <summary>
        /// Creates event with ticket template, buy ticket and throws <see cref="SoldOutException"/> exception.
        /// </summary>
        /// <returns> Task representing asynchronous operation. </returns>
        [Test]
        [Repeat(20)]
        public async Task BuyTicketMaxAttendees()
        {
            var addEventCommand = Fakers.AddEventCommand.Generate();
            addEventCommand.MaxAttendees = 1;

            var eventId = await this.EventsController.AddEvent(addEventCommand);

            var addTicketTempaltesCommands = Fakers.AddTicketTemplateCommand.Generate(3);
            addTicketTempaltesCommands[0].From = DateTime.UtcNow.AddDays(-10);
            addTicketTempaltesCommands[0].To = DateTime.UtcNow.AddDays(-3);
            addTicketTempaltesCommands[1].From = DateTime.UtcNow.AddDays(-3);
            addTicketTempaltesCommands[1].To = DateTime.UtcNow.AddDays(3);
            addTicketTempaltesCommands[2].From = DateTime.UtcNow.AddDays(3);
            addTicketTempaltesCommands[2].To = DateTime.UtcNow.AddDays(10);

            await addTicketTempaltesCommands
                .Select(command =>
                {
                    command.EventId = eventId;
                    return command;
                })
                .ForEach(async command => await this.TicketTemplatesController.AddTicketTemplate(command));

            var ticketId = await this.TicketsController.BuyTicket(eventId);

            var ticket = await this.TicketsController.GetTicketById(ticketId);

            Assert.AreEqual(addEventCommand.Name, ticket.EventName);
            Assert.IsTrue(addEventCommand.StartDate.IsEqual(ticket.Start));
            Assert.IsTrue(addEventCommand.EndDate.IsEqual(ticket.End));
            Assert.Less(DateTime.UtcNow.Subtract(ticket.Bought), TimeSpan.FromSeconds(5));
            Assert.Less(Math.Abs(addTicketTempaltesCommands[1].Price - ticket.Price), 0.01m);

            Assert.ThrowsAsync<SoldOutException>(async () => await this.TicketsController.BuyTicket(eventId));
        }
    }
}