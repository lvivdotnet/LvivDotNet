module Ticket.BuyUnauthorized
    open NBomber.FSharp
    open FSharp.Data
    open FSharp.Data.HttpRequestHeaders
    open FSharp.Control.Tasks.V2.ContextInsensitive
    open Types.Commands
    open NBomber.Contracts
    open System
    open Common
    open Common.Functions
    open Types.StepResponses

    let private prepareSteps api = task {
        let! authResponse = api |>  loginAdmin
        let auth = authResponse.Payload :?> RegisterStepResponse
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