using System;
using LvivDotNet.Application.Interfaces;

namespace LvivDotNet.Infrastructure
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}