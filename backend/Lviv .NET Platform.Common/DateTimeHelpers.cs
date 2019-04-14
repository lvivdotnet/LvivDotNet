using System;

namespace Lviv_.NET_Platform.Common
{
    public static class DateTimeHelpers
    {
        public static DateTime Truncate(this DateTime dateTime) => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);

        public static bool IsEqual(this DateTime d1, DateTime d2) => Math.Abs((d1 - d2).TotalSeconds) < 1;
    }
}
