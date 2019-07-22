module Ticket.BuyUnauthorized
    open NBomber.FSharp
    open FSharp.Data
    open FSharp.Data.HttpRequestHeaders
    open FSharp.Control.Tasks.V2.ContextInsensitive
    open Newtonsoft.Json
    open Types
    open Types.Commands
    open NBomber.Contracts
    open System
    open Common
    open Types.Responses
    open Types.StepResponses

    let toTextRequest command =
        command |> JsonConvert.SerializeObject |> TextRequest

    let loginAdmin api = task {
        let loginUserCommand =  { Email = Environment.GetEnvironmentVariable "AdministratorEmail"; Password = Environment.GetEnvironmentVariable "AdministratorPassword" }
        let! registerResponce =
            Http
                .AsyncRequest(Address.User.Login api,
                    httpMethod = HttpMethod.Post,
                    body = (toTextRequest <| loginUserCommand),
                    headers = [ ContentType HttpContentTypes.Json ])

        match (registerResponce.StatusCode, registerResponce.Body) with
        | (200, Text text) ->
            let response = text |> JsonConvert.DeserializeObject<AuthResponse>
            return { Email = loginUserCommand.Email; Password = loginUserCommand.Password; JwtToken = response.JwtToken; RefreshToken = response.RefreshToken } |> Response.Ok
        | _ -> return Response.Fail()
    }
    
    let createEventAndTicketTemplate api auth = task {
        let addEventCommand = Fakers.AddEventCommand.Generate()
        let! addEventResponse =
            Http
                .AsyncRequest(Address.Event.Add api,
                    httpMethod = HttpMethod.Post,
                    body = (toTextRequest <| addEventCommand),
                    headers = [ ContentType HttpContentTypes.Json; Authorization ("Bearer " + auth.JwtToken) ])
        let eventId =
            match (addEventResponse.StatusCode, addEventResponse.Body) with
            | (200, Text text) -> text |> Number.Parse |> Some
            | _ -> None
    
        match eventId with
        | Some id ->
            let addTicketTemplateCommand = { Fakers.AddTicketTemplateCommand.Generate() with EventId = id; From = addEventCommand.StartDate; To = addEventCommand.EndDate }
    
            let! addTicketTemplateResponse =
                Http
                    .AsyncRequest(Address.TicketTemplate.Add api,
                        httpMethod = HttpMethod.Post,
                        body = (toTextRequest <| addTicketTemplateCommand),
                        headers = [ ContentType HttpContentTypes.Json; Authorization ("Bearer " + auth.JwtToken) ])
            match (addTicketTemplateResponse.StatusCode, addTicketTemplateResponse.Body) with
            | (200, Text _) -> return id |> Some
            | _ -> return None
        | None -> return None
    }
    
    let prepareSteps api = task {
        let! authResponse = api |>  loginAdmin
        let auth = authResponse.Payload :?> RegisterStepResponse;
        let! eventId = createEventAndTicketTemplate api auth
    
        match eventId with
        | Some id ->
            return [Step.create("Buy Ticket", ConnectionPool.none, fun _ -> task {
                let buyTicketCommand = { Fakers.BuyTicketByUnauthorizedCommand.Generate() with EventId = id }
                let! buyTicketResponse =
                    Http
                        .AsyncRequest(Address.Ticket.BuyUnauthorized api,
                            httpMethod = HttpMethod.Post,
                            body = (toTextRequest <| buyTicketCommand),
                            headers = [ ContentType HttpContentTypes.Json ])
    
                match (buyTicketResponse.StatusCode, buyTicketResponse.Body) with
                | (200, Text text) ->
                    match System.Int32.TryParse(text.ToString()) with
                    | (true, _) -> return Response.Ok(text.ToString());
                    | _ -> return Response.Fail();
                | _ -> return Response.Fail()
            });
            Step.create("Get Ticket", ConnectionPool.none, fun context -> task {
                let ticketId = context.Payload :?> string
    
                let! getTicketResponse =
                    Http
                        .AsyncRequest(Address.Ticket.Get api ticketId,
                            httpMethod = HttpMethod.Get,
                            headers = [ ContentType HttpContentTypes.Json ])
    
                match getTicketResponse.StatusCode with
                | 200 -> return Response.Ok()
                | _ -> return Response.Fail()
            })]
        | None -> return []
    }
    
    let Scenario api =
        task {
            let! steps = prepareSteps(api)
    
            return steps
            |> Scenario.create "Buy Ticket By Unauthorized User Scenario"
            |> Scenario.withWarmUpDuration(TimeSpan.FromSeconds(15.0))
            |> Scenario.withConcurrentCopies 5
            |> Scenario.withDuration(TimeSpan.FromSeconds(30.0))
        }
        |> Async.AwaitTask
        |> Async.RunSynchronously