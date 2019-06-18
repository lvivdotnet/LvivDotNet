module Fakers
    open Bogus;
    type Sex = Female=0|Male=1
    type RegisterUserCommand = { FirstName: string; LastName: string; Email: string; Phone: string; Sex: Sex; Age: int; Avatar: string; Password: string }
        

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
