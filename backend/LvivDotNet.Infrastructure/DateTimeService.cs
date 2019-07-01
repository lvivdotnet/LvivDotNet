using System;
using LvivDotNet.Application.Interfaces;

namespace LvivDotNet.Infrastructure
{
    /// <inheritdoc />
    public class DateTimeService : IDateTimeService
    {
        /// <inheritdoc />
        public DateTime UtcNow => DateTime.UtcNow;
    }
}