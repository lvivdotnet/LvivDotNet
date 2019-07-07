using System;
using System.Linq;
using System.Threading.Tasks;
using LvivDotNet.Application.Tickets.Commands.BuyTicket.Authorized;
using LvivDotNet.Common.Extensions;
using LvivDotNet.WebApi.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LvivDotNet.Application.Tests.Tickets.Command.Authorized
{
    /// <summary>
    /// Buy ticket logic test.
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

            var userController = new UsersController(mediator);
            var registerUserCommand = Fakers.RegisterUserCommand.Generate();

            await userController.Register(registerUserCommand);

            this.UserEmail = registerUserCommand.Email;
        }

        /// <summary>
        /// Creates event with ti
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

            var buyTicketCommand = new BuyTicketCommand { UserEmail = this.UserEmail, EventId = eventId };

            await this.TicketsController.BuyTicket(buyTicketCommand);
        }
    }
}
