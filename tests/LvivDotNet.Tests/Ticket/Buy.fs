module Ticket.Buy
    open NBomber.FSharp
    open FSharp.Data
    open FSharp.Data.HttpRequestHeaders
    open FSharp.Control.Tasks.V2.ContextInsensitive
    open Newtonsoft.Json
    open Types
    open Types.Responses
    open Types.Commands
    open Types.StepResponses
    open NBomber.Contracts
    open System
    open Common
    open System.Threading.Tasks

    let toTextRequest command =
        command |> JsonConvert.SerializeObject |> TextRequest

    let registerUser api = task {
        let registerUserCommand = Fakers.RegisterUserCommand.Generate()
        let! registerResponce =
            Http
                .AsyncRequest(Address.User.Register api,
                    httpMethod = HttpMethod.Post,
                    body = (toTextRequest <| registerUserCommand),
                    headers = [ ContentType HttpContentTypes.Json ])

        match (registerResponce.StatusCode, registerResponce.Body) with
        | (200, Text text) ->
            let response = text |> JsonConvert.DeserializeObject<AuthResponse>
            return { Email = registerUserCommand.Email; Password = registerUserCommand.Password; JwtToken = response.JwtToken; RefreshToken = response.RefreshToken } |> Response.Ok
        | _ -> return Response.Fail()
    }

    let createEventAndTicketTemplate api = task {
        let addEventCommand = Fakers.AddEventCommand.Generate()
        let! addEventResponse =
            Http
                .AsyncRequest(Address.Event.Add api,
                    httpMethod = HttpMethod.Post,
                    body = (toTextRequest <| addEventCommand),
                    headers = [ ContentType HttpContentTypes.Json])
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
                        headers = [ ContentType HttpContentTypes.Json ])
            match (addTicketTemplateResponse.StatusCode, addTicketTemplateResponse.Body) with
            | (200, Text _) -> return id |> Some
            | _ -> return None
        | None -> return None
    }

    let prepareSteps api = task {
        let! authResponse = api |>  registerUser
        let auth = authResponse.Payload :?> RegisterStepResponse;
        let! eventId = createEventAndTicketTemplate api

        match eventId with
        | Some id ->
            return [Step.create("Buy Ticket", ConnectionPool.none, fun _ -> task {
                let buyTicketCommand = { UserEmail = auth.Email; EventId = id }
                let! buyTicketResponse =
                    Http
                        .AsyncRequest(Address.Ticket.Buy api,
                            httpMethod = HttpMethod.Post,
                            body = (toTextRequest <| buyTicketCommand),
                            headers = [ ContentType HttpContentTypes.Json; Authorization ("Bearer " + auth.JwtToken) ])

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
                            headers = [ ContentType HttpContentTypes.Json; Authorization ("Bearer " + auth.JwtToken) ])

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
            |> Scenario.create "Buy Ticket Scenario"
            |> Scenario.withWarmUpDuration(TimeSpan.FromSeconds(5.0))
            |> Scenario.withConcurrentCopies 5
            |> Scenario.withDuration(TimeSpan.FromSeconds(30.0))
        }
        |> Async.AwaitTask
        |> Async.RunSynchronously