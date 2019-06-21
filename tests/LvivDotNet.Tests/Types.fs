module Types

    type Sex = Female=0|Male=1

    module Commands =
        
        type RegisterUserCommand = { FirstName: string; LastName: string; Email: string; Phone: string; Sex: Sex; Age: int; Avatar: string; Password: string }

        type LogoutCommand = { RefreshToken: string; Token: string }

        type LoginCommand = { Email: string; Password: string }

        type RefreshTokenCommand = { RefreshToken: string; JwtToken: string }

    module Responses =
        
        type AuthResponse = { JwtToken: string; RefreshToken: string; FirstName: string; LastName: string; Email: string; Role: string; }

    module StepResponses =
        
        type RegisterStepResponse = { Email: string; Password: string; JwtToken: string; RefreshToken: string; }