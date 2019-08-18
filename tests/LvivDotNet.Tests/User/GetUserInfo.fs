module User.GetUserInfo
    open NBomber.FSharp
    open FSharp.Data
    open Common.Functions
    open FSharp.Control.Tasks
    open FSharp.Data.HttpRequestHeaders
    open Common
    open NBomber.Contracts
    open System
    open Types.StepResponses
    
    let register api =
        Step.create("Register User", ConnectionPool.none, fun context -> registerUser api)
    
    let requestUserInfo api =
        Step.create("Request User Info", ConnectionPool.none, fun context -> task {
            let auth = context.Payload :?> RegisterStepResponse;

            let! response =
                Http
                    .AsyncRequest(Address.User.GetUserInfo api,
                        httpMethod = HttpMethod.Get,
                        headers = [ ContentType HttpContentTypes.Json; Authorization ("Bearer " + auth.JwtToken) ])

            match response.StatusCode with
            | 200 -> return Response.Ok()
            | _ -> return Response.Fail()
        })

    let Scenario api =
        [register; requestUserInfo]
        |> List.map(fun step -> step api)
        |> Scenario.create "Get User Info"
        |> Scenario.withWarmUpDuration(TimeSpan.FromSeconds(15.0))
        |> Scenario.withConcurrentCopies 5
        |> Scenario.withDuration(TimeSpan.FromSeconds(30.0))