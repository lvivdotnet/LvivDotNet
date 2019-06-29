using System;
using System.Collections.Generic;

namespace LvivDotNet.Common.Extensions
{
    /// <summary>
    /// Extension methods for IEnumerable interface.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Apply action to each element of collection.
        /// </summary>
        /// <typeparam name="T"> Enumerable items type. </typeparam>
        /// <param name="enumerable"> Enumerable. </param>
        /// <param name="action"> Action to apply to enumerable items. </param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null)
            {
                return;
            }

            var enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                action(enumerator.Current);
            }
        }
    }
}
