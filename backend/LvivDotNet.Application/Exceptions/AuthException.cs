using System;

namespace LvivDotNet.Application.Exceptions
{
    public class AuthException : Exception
    {
        public AuthException() : base("Incorrect email or password") { }
    }
}