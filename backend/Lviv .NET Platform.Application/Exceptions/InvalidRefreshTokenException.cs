using System;

namespace Lviv_.NET_Platform.Application.Exceptions
{
    public class InvalidRefreshTokenException : Exception
    {
        public InvalidRefreshTokenException() : base("Invalid refresh token") { }
    }
}
