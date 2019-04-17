using System;

namespace Lviv_.NET_Platform.Application.Exceptions
{
    public class RefreshTokenExpiredException : Exception
    {
        public RefreshTokenExpiredException() : base("Refresh token expired") { }
    }
}
