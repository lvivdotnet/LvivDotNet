module User.Auth
    open NBomber.FSharp
    open FSharp.Control.Tasks.V2.ContextInsensitive
    open NBomber.Contracts
    open FSharp.Data
    open FSharp.Data.HttpRequestHeaders
    open Newtonsoft.Json
    open System

    let register api = 
        Step.create("Register User", ConnectionPool.none, fun context -> task {
            let body = Fakers.RegisterUserCommand.Generate() |> JsonConvert.SerializeObject |> TextRequest
            let! registerResponse = 
                Http
                    .AsyncRequest("http://" + api + "/api/users/register",
                        httpMethod = HttpMethod.Post,
                        body = body,
                        headers = [ ContentType HttpContentTypes.Json ])

            let f (response: HttpResponse) =
                match response.StatusCode with
                | 200 -> Response.Ok(response)
                | _ -> Response.Fail()

            return f <| registerResponse
        })

    let Scenario api = 
        [register]
        |> List.map(fun step -> api |> step)
        |> Scenario.create "Auth Scenario"
        |> Scenario.withConcurrentCopies 4
        |> Scenario.withDuration(TimeSpan.FromSeconds(10.0))