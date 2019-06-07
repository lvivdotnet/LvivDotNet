using Bogus;
using LvivDotNet.Application.Events.Commands.AddEvent;
using LvivDotNet.Application.TicketTemplates.Models;
using LvivDotNet.Common;
using LvivDotNet.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LvivDotNet.Application.Tests.Events.Events.Commands
{
    [TestFixture]
    public class AddEventTest : BaseTest
    {
        private static readonly Faker<TicketTemplateModel> ticketTemplateFaker = new Faker<TicketTemplateModel>()
            .RuleFor(c => c.Name, (f, c) => f.Lorem.Word())
            .RuleFor(c => c.Price, (f, c) => f.Random.Decimal(50, 200))
            .RuleFor(c => c.From, (f, c) => f.Date.Between(DateTime.Now.AddMonths(1), DateTime.Now.AddMonths(1).AddDays(1)))
            .RuleFor(c => c.To, (f, c) => f.Date.Between(DateTime.Now.AddMonths(2), DateTime.Now.AddMonths(2).AddDays(1)));

        private static readonly Faker<AddEventCommand> commandFaker = new Faker<AddEventCommand>()
            .RuleFor(c => c.Name, f => f.Lorem.Word())
            .RuleFor(c => c.StartDate, f => f.Date.Between(DateTime.Now.AddMonths(1), DateTime.Now.AddMonths(1).AddHours(2)))
            .RuleFor(c => c.EndDate, f => f.Date.Between(DateTime.Now.AddMonths(1).AddHours(2), DateTime.Now.AddMonths(1).AddHours(3)))
            .RuleFor(c => c.Address, f => f.Address.FullAddress())
            .RuleFor(c => c.Title, f => f.Lorem.Sentence(3))
            .RuleFor(c => c.Description, f => f.Lorem.Text())
            .RuleFor(c => c.MaxAttendees, f => f.Random.Number(50, 150))
            .RuleFor(c => c.TicketTemplates, () => ticketTemplateFaker.Generate(3));

        [Test]
        [Repeat(1000)]
        public async Task Test()
        {
            var controller = new EventsController(ServiceProvider.GetRequiredService<IMediator>());
            var command = commandFaker.Generate();

            var result = await controller.AddEvent(command);

            var @event = await controller.GetEvent(result);

            Assert.AreEqual(command.Address, @event.Address);
            Assert.AreEqual(command.Description, @event.Description);
            Assert.IsTrue(command.EndDate.IsEqual(@event.EndDate));
            Assert.IsTrue(command.StartDate.IsEqual(@event.StartDate));
            Assert.AreEqual(command.MaxAttendees, @event.MaxAttendees);
            Assert.AreEqual(command.Name, @event.Name);
            Assert.AreEqual(command.Title, @event.Title);

            foreach (var ticketTemplate in command.TicketTemplates)
            {
                Assert.True(@event.TickerTemplates.Any(expected => @event.TickerTemplates.Any(
                        actual => expected.From.IsEqual(actual.From) &&
                        expected.To.IsEqual(actual.To) &&
                        expected.Name == actual.Name &&
                        Math.Round(actual.Price, 4) == Math.Round(expected.Price, 4) &&
                        expected.EventId != 0 &&
                        expected.Id != 0
                    )));
            }
        }

    }
}
