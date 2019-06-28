using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LvivDotNet.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> functor)
        {
            var enumerator = enumerable.GetEnumerator();
            while(enumerator.MoveNext())
            {
                functor(enumerator.Current);
            }
        }
    }
}
