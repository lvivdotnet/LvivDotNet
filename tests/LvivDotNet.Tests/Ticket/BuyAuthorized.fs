module Ticket.BuyAuthorized
    open NBomber.FSharp
    open FSharp.Data
    open FSharp.Data.HttpRequestHeaders
    open FSharp.Control.Tasks.V2.ContextInsensitive
    open Newtonsoft.Json
    open Types.Responses
    open Types.Commands
    open Types.StepResponses
    open NBomber.Contracts
    open System
    open Common.Functions
    open Ticket.Common
    open Common

    let private registerUser api = task {
        let registerUserCommand = Fakers.RegisterUserCommand.Generate()
        let! registerResponse =
            Http
                .AsyncRequest(Address.User.Register api,
                    httpMethod = HttpMethod.Post,
                    body = (toTextRequest <| registerUserCommand),
                    headers = [ ContentType HttpContentTypes.Json ])

        match (registerResponse.StatusCode, registerResponse.Body) with
        | (200, Text text) ->
            let response = text |> JsonConvert.DeserializeObject<AuthResponse>
            return { Email = registerUserCommand.Email; Password = registerUserCommand.Password; JwtToken = response.JwtToken; RefreshToken = response.RefreshToken } |> Response.Ok
        | _ -> return Response.Fail()
    }

    let prepareSteps api = task {
        let! adminAuthResponse = api |> loginAdmin
        let adminAuth = adminAuthResponse.Payload :?> RegisterStepResponse;
        let! eventId = createEventAndTicketTemplate api adminAuth

        let! authResponse = api |>  registerUser
        let auth = authResponse.Payload :?> RegisterStepResponse;

        match eventId with
        | Some id ->
            return [Step.create("Buy Ticket", ConnectionPool.none, fun _ -> task {
                let eventId = id.ToString()
                let! buyTicketResponse =
                    Http
                        .AsyncRequest(Address.Ticket.BuyAuthorized api eventId,
                            httpMethod = HttpMethod.Post,
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
            |> Scenario.create "Buy Ticket By Authorized User Scenario"
            |> Scenario.withWarmUpDuration(TimeSpan.FromSeconds(15.0))
            |> Scenario.withConcurrentCopies 5
            |> Scenario.withDuration(TimeSpan.FromSeconds(30.0))
        }
        |> Async.AwaitTask
        |> Async.RunSynchronously