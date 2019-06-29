using System;

namespace LvivDotNet.Application.Exceptions
{
    /// <summary>
    /// Invalid refresh token exception.
    /// </summary>
    public class InvalidRefreshTokenException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRefreshTokenException"/> class.
        /// </summary>
        public InvalidRefreshTokenException()
            : base("Invalid refresh token")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRefreshTokenException"/> class.
        /// </summary>
        /// <param name="message"> Exception message. </param>
        public InvalidRefreshTokenException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRefreshTokenException"/> class.
        /// </summary>
        /// <param name="message"> Exception message. </param>
        /// <param name="innerException"> Inner exception. </param>
        public InvalidRefreshTokenException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
