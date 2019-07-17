module Common
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
        module Event =
            let Add api =
                "http://" + api + "/api/events"
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