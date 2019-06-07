using System;

namespace LvivDotNet.Application.Exceptions
{
    public class InvalidRefreshTokenException : Exception
    {
        public InvalidRefreshTokenException() : base("Invalid refresh token") { }
    }
}
