open NBomber.FSharp
open System
open System.Threading
open System.Net.Http
open System.Net

let tryGet (api: string) =
    let http = new HttpClient()
    try
        let resp = http.GetAsync("http://" + api + "/api/ping")
        resp.Result.StatusCode = HttpStatusCode.OK
    with
    | _ -> false

let rec waitForApi apiGetter =
    Thread.Sleep 100
    match apiGetter() |> tryGet  with
    | true -> apiGetter()
    | false -> waitForApi(apiGetter)

[<EntryPoint>]
let main _ =
    let api = waitForApi(fun () -> Environment.GetEnvironmentVariable("API"))

    [User.Auth.Scenario; Ticket.Buy.Scenario]
    |> List.map(fun scenario -> api |> scenario)
    |> NBomberRunner.registerScenarios
    |> NBomberRunner.withReportFileName("report")
    |> NBomberRunner.runInConsole
    |> ignore
    0