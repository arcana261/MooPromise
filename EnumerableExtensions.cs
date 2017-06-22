using MooPromise.Enumerable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise
{
    public static class EnumerableExtensions
    {
        public static IPromiseEnumerable<T> Promesify<T>(this IEnumerable<T> items)
        {
            return new PromiseEnumerable<T>(items);
        }
    }
}
