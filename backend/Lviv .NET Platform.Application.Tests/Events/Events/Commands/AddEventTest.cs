using Bogus;
using Lviv_.NET_Platform.Application.Events.Commands.AddEvent;
using Lviv_.NET_Platform.Application.Interfaces;
using Lviv_.NET_Platform.Application.TicketTemplates.Models;
using Lviv_.NET_Platform.Controllers;
using Lviv_.NET_Platform.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using ServiceStack.OrmLite;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Lviv.NET_Platform.Application.Tests.Events.Events.Commands
{
    [TestFixture]
    public class AddEventTest: BaseTest
    {
        private static Faker<TicketTemplateModel> ticketTemplateFaker = new Faker<TicketTemplateModel>()
            .RuleFor(c => c.Name, (f, c) => f.Lorem.Word())
            .RuleFor(c => c.Price, (f, c) => f.Random.Decimal(50, 200))
            .RuleFor(c => c.From, (f, c) => f.Date.Between(DateTime.Now.AddMonths(1), DateTime.Now.AddMonths(1).AddDays(1)))
            .RuleFor(c => c.To, (f, c) => f.Date.Between(DateTime.Now.AddMonths(2), DateTime.Now.AddMonths(2).AddDays(1)));

        private static Faker<AddEventCommand> commandFaker = new Faker<AddEventCommand>()
            .RuleFor(c => c.Name, f => f.Lorem.Word())
            .RuleFor(c => c.StartDate, f => f.Date.Between(DateTime.Now.AddMonths(1), DateTime.Now.AddMonths(1).AddHours(2)))
            .RuleFor(c => c.EndDate, f => f.Date.Between(DateTime.Now.AddMonths(1).AddHours(2), DateTime.Now.AddMonths(1).AddHours(3)))
            .RuleFor(c => c.PostDate, f => f.Date.Recent())
            .RuleFor(c => c.Address, f => f.Address.FullAddress())
            .RuleFor(c => c.Title, f => f.Lorem.Sentence(3))
            .RuleFor(c => c.Description, f => f.Lorem.Text())
            .RuleFor(c => c.MaxAttendees, f => f.Random.Number(50, 150))
            .RuleFor(c => c.TicketTemplates, () => ticketTemplateFaker.Generate(3));

        [Test]
        public async Task Test()
        {
            var controller = new EventsController(ServiceProvider.GetRequiredService<IMediator>());
            var command = commandFaker.Generate();

            try
            {
                var result = await controller.AddEvent(command);
            }
            catch(Exception e) { }
        }

    }
}
