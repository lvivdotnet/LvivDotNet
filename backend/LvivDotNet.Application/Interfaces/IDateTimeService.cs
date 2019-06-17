using System;

namespace LvivDotNet.Application.Interfaces
{
    public interface IDateTimeService
    {
        DateTime UtcNow { get; }
    }
}