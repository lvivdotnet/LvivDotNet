module User.Auth
    open NBomber.FSharp
    open FSharp.Control.Tasks.V2.ContextInsensitive
    open NBomber.Contracts
    open FSharp.Data
    open FSharp.Data.HttpRequestHeaders
    open Newtonsoft.Json
    open System
    open Types.Commands
    open Types.Responses
    open Types.StepResponses

    let register api = 
        Step.create("Register", ConnectionPool.none, fun context -> task {
            let registerCommand = Fakers.RegisterUserCommand.Generate()
            let body = registerCommand |> JsonConvert.SerializeObject |> TextRequest
            let! registerResponse = 
                Http
                    .AsyncRequest("http://" + api + "/api/users/register",
                        httpMethod = HttpMethod.Post,
                        body = body,
                        headers = [ ContentType HttpContentTypes.Json ])

            match (registerResponse.StatusCode, registerResponse.Body) with
            | (200, Text text) -> 
                let response = text |> JsonConvert.DeserializeObject<AuthResponse>
                return { Email = registerCommand.Email; Password = registerCommand.Password; JwtToken = response.JwtToken; RefreshToken = response.RefreshToken } |> Response.Ok
            | _ -> return Response.Fail()
        })

    let logout num api =
        Step.create("Logout " + num, ConnectionPool.none, fun context -> task {
            let payload = context.Payload :?> RegisterStepResponse;
            let body = { RefreshToken = payload.RefreshToken; Token = payload.JwtToken } |> JsonConvert.SerializeObject |> TextRequest
            let! logoutResponse =
                Http
                    .AsyncRequest("http://" + api + "/api/users/logout",
                        httpMethod = HttpMethod.Post,
                        body = body,
                        headers = [ ContentType HttpContentTypes.Json; Authorization ("Bearer " + payload.JwtToken) ])

            match logoutResponse.StatusCode with
            | 200 -> return Response.Ok(payload)
            | _ -> return Response.Fail()
        })

    let login api =
        Step.create("Login", ConnectionPool.none, fun context -> task {
            let payload = context.Payload :?> RegisterStepResponse
            let body = { Email = payload.Email; Password = payload.Password } |> JsonConvert.SerializeObject |> TextRequest
            let! loginResonse =
                Http
                    .AsyncRequest("http://" + api + "/api/users/login",
                        httpMethod = HttpMethod.Post,
                        body = body,
                        headers = [ ContentType HttpContentTypes.Json ])
            match (loginResonse.StatusCode, loginResonse.Body) with
            | (200, Text text) -> 
                let response = text |> JsonConvert.DeserializeObject<AuthResponse>
                return { Password = payload.Password; Email = payload.Email; RefreshToken = response.RefreshToken; JwtToken = response.JwtToken } |> Response.Ok
            | _ -> return Response.Fail()
        })

    let refresh num api =
        Step.create("Refresh " + num, ConnectionPool.none, fun context -> task {
            let payload = context.Payload :?> RegisterStepResponse
            let body = { RefreshTokenCommand.RefreshToken = payload.RefreshToken; JwtToken = payload.JwtToken } |> JsonConvert.SerializeObject |> TextRequest
            let! refreshResponse =
                Http
                    .AsyncRequest("http://" + api + "/api/users/refresh",
                        httpMethod = HttpMethod.Post,
                        body = body,
                        headers = [ ContentType HttpContentTypes.Json ])

            match (refreshResponse.StatusCode, refreshResponse.Body) with
            | (200, Text text) ->
                let response = text |> JsonConvert.DeserializeObject<AuthResponse>
                return { Email = payload.Email; Password = payload.Password; JwtToken = response.JwtToken; RefreshToken = response.RefreshToken } |> Response.Ok
            | _ -> return Response.Fail()
        })

    let Scenario api = 
        [register; logout "1"; login; refresh "1"; refresh "2"; logout "2"]
        |> List.map(fun step -> api |> step)
        |> Scenario.create "Auth Scenario"
        |> Scenario.withWarmUpDuration(TimeSpan.FromSeconds(5.0))
        |> Scenario.withConcurrentCopies 5
        |> Scenario.withDuration(TimeSpan.FromSeconds(30.0))