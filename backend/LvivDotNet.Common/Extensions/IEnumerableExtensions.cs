using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// Apply function to each element of collection.
        /// </summary>
        /// <typeparam name="T"> Enumerable items type. </typeparam>
        /// <param name="enumerable"> Enumerable. </param>
        /// <param name="func"> Action to apply to enumerable items. </param>
        /// <returns> Task representing asynchronous operation. </returns>
        public static async Task ForEach<T>(this IEnumerable<T> enumerable, Func<T, Task> func)
        {
            if (enumerable == null)
            {
                return;
            }

            var enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                await func(enumerator.Current);
            }
        }
    }
}
