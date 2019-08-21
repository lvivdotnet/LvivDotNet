using System;

namespace LvivDotNet.Application.Exceptions
{
    /// <summary>
    /// Exception for sold out situation.
    /// </summary>
    public class SoldOutException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoldOutException"/> class.
        /// </summary>
        /// <param name="eventName"> Event name. </param>
        public SoldOutException(string eventName)
            : base($"Ticket for event \"{eventName}\" runs out.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoldOutException"/> class.
        /// </summary>
        /// <param name="message"> Exception message. </param>
        /// <param name="innerException"> Inner exception. </param>
        public SoldOutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoldOutException"/> class.
        /// </summary>
        public SoldOutException()
        {
        }
    }
}
