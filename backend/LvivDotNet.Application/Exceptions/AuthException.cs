using System;

namespace LvivDotNet.Application.Exceptions
{
    /// <summary>
    /// Authorization or authentication exception.
    /// </summary>
    public class AuthException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthException"/> class.
        /// </summary>
        public AuthException()
            : base("Incorrect email or password")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthException"/> class.
        /// </summary>
        /// <param name="message"> Exception message. </param>
        public AuthException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthException"/> class.
        /// </summary>
        /// <param name="message"> Exception message. </param>
        /// <param name="innerException"> Inner exception. </param>
        public AuthException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}