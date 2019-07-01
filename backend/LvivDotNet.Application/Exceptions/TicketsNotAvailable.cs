using System;
using System.Collections.Generic;
using System.Text;

namespace LvivDotNet.Application.Exceptions
{
    /// <summary>
    /// Tickets not available exception.
    /// </summary>
    public class TicketsNotAvailable : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TicketsNotAvailable"/> class.
        /// </summary>
        /// <param name="message"> Exception message. </param>
        public TicketsNotAvailable(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketsNotAvailable"/> class.
        /// </summary>
        /// <param name="message"> Exception message. </param>
        /// <param name="innerException"> Inner exception. </param>
        public TicketsNotAvailable(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketsNotAvailable"/> class.
        /// </summary>
        public TicketsNotAvailable()
            : base("Tickets are not available at the moment")
        {
        }
    }
}
