using System;
using System.Collections.Generic;
using System.Text;

namespace LvivDotNet.Application.Exceptions
{
    /// <summary>
    /// Exception for sold out situation.
    /// </summary>
    public class SouldOutException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SouldOutException"/> class.
        /// </summary>
        /// <param name="eventName"> Event name. </param>
        public SouldOutException(string eventName)
            : base($"Ticket for event \"{eventName}\" runs out.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SouldOutException"/> class.
        /// </summary>
        /// <param name="message"> Exception message. </param>
        /// <param name="innerException"> Inner exception. </param>
        public SouldOutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SouldOutException"/> class.
        /// </summary>
        public SouldOutException()
        {
        }
    }
}
