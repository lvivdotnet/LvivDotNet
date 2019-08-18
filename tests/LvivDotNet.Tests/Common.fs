module Common

    open Newtonsoft.Json
    open FSharp.Data
    open FSharp.Control.Tasks
    open Types.Commands
    open Types.StepResponses
    open System
    open NBomber.Contracts
    open Types.Responses
    open FSharp.Data.HttpRequestHeaders

    module Address =
        let Ping api =
            "http://" + api + "/api/ping"

        module User = 
            let Register api =
                "http://" + api + "/api/users/register"
            let Logout api =
                "http://" + api + "/api/users/logout"
            let Refresh api = 
                "http://" + api + "/api/users/refresh"
            let Login api =
                "http://" + api + "/api/users/login"
            let GetUserInfo api =
                "http://" + api + "/api/users"

        module Event =
            let Add api =
                "http://" + api + "/api/events"
            let Update api =
                "http://" + api + "/api/events"
            let Get api eventId =
                "http://" + api + "/api/events/" + eventId

        module TicketTemplate =
            let Add api =
                "http://" + api + "/api/tickettemplates"

        module Ticket =
            let BuyAuthorized api eventId = 
                "http://" + api + "/api/tickets/" + eventId
            let BuyUnauthorized api =
                "http://" + api + "/api/tickets/unauthorized"
            let Get api id =
                "http://" + api + "/api/tickets/" + id

    module Functions =
        let toTextRequest command =
            command |> JsonConvert.SerializeObject |> TextRequest

        let loginAdmin api = task {
            let loginUserCommand =  { Email = Environment.GetEnvironmentVariable "AdministratorEmail"; Password = Environment.GetEnvironmentVariable "AdministratorPassword" }
            let! registerResponse =
                Http
                    .AsyncRequest(Address.User.Login api,
                        httpMethod = HttpMethod.Post,
                        body = (toTextRequest <| loginUserCommand),
                        headers = [ ContentType HttpContentTypes.Json ])
        
            match (registerResponse.StatusCode, registerResponse.Body) with
            | (200, Text text) ->
                let response = text |> JsonConvert.DeserializeObject<AuthResponse>
                return { Email = loginUserCommand.Email; Password = loginUserCommand.Password; JwtToken = response.JwtToken; RefreshToken = response.RefreshToken } |> Response.Ok
            | _ -> return Response.Fail()
        }

        let registerUser api = task {
                let registerCommand = Fakers.RegisterUserCommand.Generate()

                let! registerResponse = 
                    Http
                        .AsyncRequest(Address.User.Register api,
                            httpMethod = HttpMethod.Post,
                            body = (toTextRequest <| registerCommand),
                            headers = [ ContentType HttpContentTypes.Json ])
        
                match (registerResponse.StatusCode, registerResponse.Body) with
                | (200, Text text) -> 
                    let response = text |> JsonConvert.DeserializeObject<AuthResponse>
                    return { Email = registerCommand.Email; Password = registerCommand.Password; JwtToken = response.JwtToken; RefreshToken = response.RefreshToken } |> Response.Ok
                | _ -> return Response.Fail()
            }