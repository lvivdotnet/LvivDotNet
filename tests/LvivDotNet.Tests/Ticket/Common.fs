module Ticket.Common
open FSharp.Data
open FSharp.Data.HttpRequestHeaders
open Newtonsoft.Json
open Types.Responses
open Types.Commands
open Types.StepResponses
open NBomber.Contracts
open System
open Common
open Common.Functions
open FSharp.Control.Tasks
open Types

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