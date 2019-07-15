module Fakers
    open Types
    open Bogus;
    open Types.Commands
    open System

    let RegisterUserCommand =
        Bogus
            .Faker<RegisterUserCommand>()
            .CustomInstantiator(fun f -> 
                {
                    FirstName = f.Name.FindName()
                    LastName = f.Name.LastName()
                    Email = f.Internet.Email()
                    Sex = enum<Sex> <| f.Random.Number(1)
                    Age = f.Random.Number(1, 100)
                    Avatar = f.Internet.Avatar()
                    Password = f.Internet.Password()
                    Phone = f.Phone.PhoneNumber()})

    let AddEventCommand =
        Bogus
            .Faker<AddEventCommand>()
            .CustomInstantiator(fun f -> 
                {
                    Name = f.Lorem.Word()
                    StartDate = f.Date.Between(DateTime.Now.AddMonths(-1), DateTime.Now.AddMonths(-1).AddHours(2.0))
                    EndDate = f.Date.Between(DateTime.Now.AddMonths(1), DateTime.Now.AddMonths(1).AddHours(2.0))
                    Address = f.Address.FullAddress()
                    Title = f.Lorem.Sentence(wordCount = Nullable(3))
                    Description = f.Lorem.Text()
                    MaxAttendees = f.Random.Number(Number.MaxValue - 1, Number.MaxValue)})

    let AddTicketTemplateCommand =
        Bogus
            .Faker<AddTicketTemplateCommand>()
            .CustomInstantiator(fun f ->
                {
                    Name = f.Lorem.Word()
                    Price = f.Random.Decimal(50m, 200m)
                    From = f.Date.Between(DateTime.Now.AddMonths(1), DateTime.Now.AddMonths(1).AddDays(1.0))
                    To = f.Date.Between(DateTime.Now.AddMonths(2), DateTime.Now.AddMonths(2).AddDays(1.0))
                    EventId = 0})