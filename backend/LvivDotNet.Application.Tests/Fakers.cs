using System;
using Bogus;
using LvivDotNet.Application.Events.Commands.AddEvent;
using LvivDotNet.Application.Events.Commands.UpdateEvent;
using LvivDotNet.Application.TicketTemplates.Commands.AddTicketTemplate;
using LvivDotNet.Application.Users.Commands.Register;
using LvivDotNet.Domain.Entities;

namespace LvivDotNet.Application.Tests
{
    public static class Fakers
    {
        public static readonly Faker<AddEventCommand> AddEventCommand = new Faker<AddEventCommand>()
            .RuleFor(c => c.Name, f => f.Lorem.Word())
            .RuleFor(c => c.StartDate, f => f.Date.Between(DateTime.Now.AddMonths(1), DateTime.Now.AddMonths(1).AddHours(2)))
            .RuleFor(c => c.EndDate, f => f.Date.Between(DateTime.Now.AddMonths(1).AddHours(2), DateTime.Now.AddMonths(1).AddHours(3)))
            .RuleFor(c => c.Address, f => f.Address.FullAddress())
            .RuleFor(c => c.Title, f => f.Lorem.Sentence(3))
            .RuleFor(c => c.Description, f => f.Lorem.Text())
            .RuleFor(c => c.MaxAttendees, f => f.Random.Number(50, 150));

        public static readonly Faker<AddTicketTemplateCommand> AddTicketTemplateCommand = new Faker<AddTicketTemplateCommand>()
            .RuleFor(c => c.Name, (f, c) => f.Lorem.Word())
            .RuleFor(c => c.Price, (f, c) => f.Random.Decimal(50, 200))
            .RuleFor(c => c.From, (f, c) => f.Date.Between(DateTime.Now.AddMonths(1), DateTime.Now.AddMonths(1).AddDays(1)))
            .RuleFor(c => c.To, (f, c) => f.Date.Between(DateTime.Now.AddMonths(2), DateTime.Now.AddMonths(2).AddDays(1)));

        public static readonly Faker<UpdateEventCommand> UpdateEventCommand = new Faker<UpdateEventCommand>()
            .RuleFor(c => c.Id, (f, c) => f.Random.Number())
            .RuleFor(c => c.Name, f => f.Lorem.Word())
            .RuleFor(c => c.StartDate, f => f.Date.Between(DateTime.Now.AddMonths(1), DateTime.Now.AddMonths(1).AddHours(2)))
            .RuleFor(c => c.EndDate, f => f.Date.Between(DateTime.Now.AddMonths(1).AddHours(2), DateTime.Now.AddMonths(1).AddHours(3)))
            .RuleFor(c => c.Address, f => f.Address.FullAddress())
            .RuleFor(c => c.Title, f => f.Lorem.Sentence(3))
            .RuleFor(c => c.Description, f => f.Lorem.Text())
            .RuleFor(c => c.MaxAttendees, f => f.Random.Number(50, 150));

        public static readonly Faker<RegisterUserCommand> RegisterUserCommand = new Faker<RegisterUserCommand>()
            .RuleFor(c => c.FirstName, (f, c) => f.Name.FirstName())
            .RuleFor(c => c.LastName, (f, c) => f.Name.LastName())
            .RuleFor(c => c.Email, (f, c) => f.Internet.Email())
            .RuleFor(c => c.Phone, (f, c) => f.Phone.PhoneNumber())
            .RuleFor(c => c.Sex, (f, c) => (Sex)f.Random.Number(1))
            .RuleFor(c => c.Age, (f, c) => f.Random.Number(1, 120))
            .RuleFor(c => c.Avatar, (f, c) => f.Internet.Avatar())
            .RuleFor(c => c.Password, (f, c) => f.Internet.Password());
    }
}