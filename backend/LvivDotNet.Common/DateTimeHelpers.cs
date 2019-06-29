using System;

namespace LvivDotNet.Common
{
    /// <summary>
    /// DateTime helpers.
    /// </summary>
    public static class DateTimeHelpers
    {
        /// <summary>
        /// Truncate millisecond of DateTime.
        /// </summary>
        /// <param name="dateTime">The DateTime. </param>
        /// <returns> Truncated DateTime. </returns>
        public static DateTime Truncate(this DateTime dateTime) => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);

        /// <summary>
        /// Compare two DateTime`s.
        /// </summary>
        /// <param name="d1"> First DateTime. </param>
        /// <param name="d2"> Second DateTime. </param>
        /// <returns> Return the result of equation. </returns>
        public static bool IsEqual(this DateTime d1, DateTime d2) => Math.Abs((d1 - d2).TotalSeconds) < 1;
    }
}
