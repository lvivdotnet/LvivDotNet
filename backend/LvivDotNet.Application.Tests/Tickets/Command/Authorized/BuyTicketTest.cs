using LvivDotNet.Application.Tickets.Commands.BuyTicket.Authorized;
using LvivDotNet.Common.Extensions;
using LvivDotNet.WebApi.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LvivDotNet.Application.Tests.Tickets.Command.Authorized
{
    /// <summary>
    /// Buy ticket logic test.
    /// </summary>
    [TestFixture]
    public class BuyTicketTest : BaseTest
    {
        private TicketsController TicketsController { get; set; }

        private EventsController EventsController { get; set; }

        private TicketTemplatesController TicketTemplatesController { get; set; }

        private string UserEmail { get; set; }

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

            addTicketTempaltesCommands
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
