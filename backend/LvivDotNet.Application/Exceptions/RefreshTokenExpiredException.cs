using System;

namespace LvivDotNet.Application.Exceptions
{
    public class RefreshTokenExpiredException : Exception
    {
        public RefreshTokenExpiredException() : base("Refresh token expired") { }
    }
}
