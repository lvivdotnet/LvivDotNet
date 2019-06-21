module Fakers
    open Types
    open Bogus;

    let RegisterUserCommand =
        Bogus
            .Faker<Commands.RegisterUserCommand>()
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
