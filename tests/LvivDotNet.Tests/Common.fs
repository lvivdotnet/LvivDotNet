module Common
    module Address =
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
            let Buy api =
                "http://" + api + "/api/tickets"