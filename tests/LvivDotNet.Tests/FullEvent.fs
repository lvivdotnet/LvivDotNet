module FullEvent

    open FSharp.Control.Tasks
    open Common.Functions
    open Types.StepResponses
    open NBomber.FSharp
    open FSharp.Data
    open Common
    open Types
    open FSharp.Data.HttpRequestHeaders
    open NBomber.Contracts
    open System

    let private prepareSteps api = task {
        let! authResponse = api |> loginAdmin
        let auth = authResponse.Payload :?> RegisterStepResponse

        let createGetEventStep name =
            Step.create(name, ConnectionPool.none, fun context -> task {
                let eventId = context.Payload :?> string;
                let! getEventResponse =
                    Http
                        .AsyncRequest(Address.Event.Get api eventId,
                        httpMethod = HttpMethod.Get)
                match getEventResponse.StatusCode with
                | 200 -> return eventId |> Response.Ok
                | _ -> return Response.Fail()
            });
        
        return [Step.create("Create Event", ConnectionPool.none, fun context -> task {
            let addEventCommand = Fakers.AddEventCommand.Generate()
            let! addEventResponse =
                Http
                    .AsyncRequest(Address.Event.Add api,
                        httpMethod = HttpMethod.Post,
                        body = (toTextRequest <| addEventCommand),
                        headers = [ ContentType HttpContentTypes.Json; Authorization ("Bearer " + auth.JwtToken) ])

            match (addEventResponse.StatusCode, addEventResponse.Body) with
            | (200, Text text) -> return text |> Response.Ok
            | _ -> return Response.Fail()
        });
        createGetEventStep "Get Event";
        Step.create("Update Event", ConnectionPool.none, fun context -> task {
            let updateEventCommand = { Fakers.UpdateEventCommand.Generate() with Id = context.Payload :?> string |> Number.Parse }
            let! updateEventResponse =
                Http
                    .AsyncRequest(Address.Event.Update api,
                    httpMethod = HttpMethod.Put,
                    body = (toTextRequest <| updateEventCommand),
                    headers = [ ContentType HttpContentTypes.Json; Authorization ("Bearer " + auth.JwtToken) ])

            match updateEventResponse.StatusCode with 
            | 200 -> return  context.Payload |> Response.Ok
            | _ -> return Response.Fail()
        });
        createGetEventStep "Get Updated Event"];
    }

    let Scenario api =
        task {
            let! steps = prepareSteps api
            
            return steps
            |> Scenario.create "Full Event"
            |> Scenario.withWarmUpDuration(TimeSpan.FromSeconds(15.0))
            |> Scenario.withConcurrentCopies 5
            |> Scenario.withDuration(TimeSpan.FromSeconds(30.0))
        }
        |> Async.AwaitTask
        |> Async.RunSynchronously
