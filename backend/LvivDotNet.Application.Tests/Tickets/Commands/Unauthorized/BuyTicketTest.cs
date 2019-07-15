using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LvivDotNet.Application.Exceptions;
using LvivDotNet.Application.Tickets.Commands.BuyTicket.Unauthorized;
using LvivDotNet.Common;
using LvivDotNet.Common.Extensions;
using LvivDotNet.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LvivDotNet.Application.Tests.Tickets.Commands.Unauthorized
{
    /// <summary>
    /// Buy ticket by unauthorized user logic test.
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
        /// Gets or sets created user email.
        /// </summary>
        private string UserEmail { get; set; }

        /// <summary>
        /// One-time test setup. Executed exactly once before all tests.
        /// Initialize Events, TicketTemplates and Tickets controllers.
        /// </summary>
        [OneTimeSetUp]
        public void SetUp()
        {
            var mediator = ServiceProvider.GetRequiredService<IMediator>();
            this.TicketsController = new TicketsController(mediator);
            this.EventsController = new EventsController(mediator);
            this.TicketTemplatesController = new TicketTemplatesController(mediator);
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

            var buyTicketCommand = Fakers.BuyUnauthorizedTicketCommand.Generate();
            buyTicketCommand.EventId = eventId;

            var ticketId = await this.TicketsController.BuyTicket(buyTicketCommand);

            var ticket = await this.TicketsController.GetTicketById(ticketId);

            Assert.AreEqual(addEventCommand.Name, ticket.EventName);
            Assert.IsTrue(addEventCommand.StartDate.IsEqual(ticket.Start));
            Assert.IsTrue(addEventCommand.EndDate.IsEqual(ticket.End));
            Assert.Less(DateTime.UtcNow.Subtract(ticket.Bought), TimeSpan.FromSeconds(5));
            Assert.Less(Math.Abs(addTicketTempaltesCommands[1].Price - ticket.Price), 0.0001m);
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

            var buyTicketCommand = Fakers.BuyUnauthorizedTicketCommand.Generate();
            buyTicketCommand.EventId = eventId;

            Assert.ThrowsAsync<TicketsNotAvailable>(async () => await this.TicketsController.BuyTicket(buyTicketCommand));
        }

        /// <summary>
        /// Creates event with ticket template, buy ticket and throws <see cref="SouldOutException"/> exception.
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

            var buyTicketCommand = Fakers.BuyUnauthorizedTicketCommand.Generate();
            buyTicketCommand.EventId = eventId;

            var ticketId = await this.TicketsController.BuyTicket(buyTicketCommand);

            var ticket = await this.TicketsController.GetTicketById(ticketId);

            Assert.AreEqual(addEventCommand.Name, ticket.EventName);
            Assert.IsTrue(addEventCommand.StartDate.IsEqual(ticket.Start));
            Assert.IsTrue(addEventCommand.EndDate.IsEqual(ticket.End));
            Assert.Less(DateTime.UtcNow.Subtract(ticket.Bought), TimeSpan.FromSeconds(5));
            Assert.Less(Math.Abs(addTicketTempaltesCommands[1].Price - ticket.Price), 0.0001m);

            buyTicketCommand = Fakers.BuyUnauthorizedTicketCommand.Generate();
            buyTicketCommand.EventId = eventId;

            Assert.ThrowsAsync<SouldOutException>(async () => await this.TicketsController.BuyTicket(buyTicketCommand));
        }
    }
}
