using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class ToListAlgorithm
    {
        public static IPromise<List<T>> ToList<T>(IPromiseEnumerator<T> items)
        {
            return ReduceAlgorithm.Reduce(items, (prev, current) =>
            {
                prev.Add(current);
                return prev;
            }, new List<T>());
        }
    }
}
