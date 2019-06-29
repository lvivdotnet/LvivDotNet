using System;

namespace LvivDotNet.Application.Exceptions
{
    /// <summary>
    /// Refresh token expired exception.
    /// </summary>
    public class RefreshTokenExpiredException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenExpiredException"/> class.
        /// </summary>
        public RefreshTokenExpiredException()
            : base("Refresh token expired")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenExpiredException"/> class.
        /// </summary>
        /// <param name="message"> Exception message. </param>
        public RefreshTokenExpiredException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenExpiredException"/> class.
        /// </summary>
        /// <param name="message"> Exception message. </param>
        /// <param name="innerException"> Inner exception. </param>
        public RefreshTokenExpiredException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
