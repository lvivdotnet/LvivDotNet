using System;

namespace Lviv_.NET_Platform.Application.Exceptions
{
    public class AuthException: Exception
    {
        public AuthException(): base("Incorrect email or password") { }
    }
}