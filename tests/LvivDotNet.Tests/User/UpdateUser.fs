module User.UpdateUser

    open NBomber.FSharp
    open Common
    open FSharp.Data
    open Common.Functions
    open Types.StepResponses
    open FSharp.Control.Tasks
    open NBomber.Contracts
    open System
    open FSharp.Data.HttpRequestHeaders

    let register api =
        Step.create("Register User", ConnectionPool.none, fun context -> registerUser api)

    let updateUser api =
        Step.create("Update User Info", ConnectionPool.none, fun context -> task {
            let auth = context.Payload :?> RegisterStepResponse;

            let! response =
                Http
                    .AsyncRequest(Address.User.UpdateUser api,
                        httpMethod = HttpMethod.Put,
                        body = (toTextRequest <| Fakers.UpdateUserCommand.Generate()),
                        headers = [ ContentType HttpContentTypes.Json; Authorization ("Bearer " + auth.JwtToken) ])

            match response.StatusCode with
            | 200 -> return Response.Ok()
            | _ -> return Response.Fail()
        })

    let Scenario api =
        [register; updateUser]
        |> List.map(fun step -> step api)
        |> Scenario.create "Update User"
        |> Scenario.withWarmUpDuration(TimeSpan.FromSeconds(15.0))
        |> Scenario.withConcurrentCopies 5
        |> Scenario.withDuration(TimeSpan.FromSeconds(30.0))