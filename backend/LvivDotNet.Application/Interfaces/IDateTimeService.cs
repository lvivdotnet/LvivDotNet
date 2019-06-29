using System;

namespace LvivDotNet.Application.Interfaces
{
    /// <summary>
    /// Date and time service.
    /// </summary>
    public interface IDateTimeService
    {
        /// <summary>
        /// Gets UTC time now.
        /// </summary>
        DateTime UtcNow { get; }
    }
}