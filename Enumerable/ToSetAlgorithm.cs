using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class ToSetAlgorithm
    {
        public static IPromise<ISet<T>> ToSet<T>(IPromiseEnumerator<T> items, IEqualityComparer<T> comparer)
        {
            return ReduceAlgorithm.Reduce(items, (prev, current) =>
            {
                prev.Add(current);
                return prev;
            }, (ISet<T>)(new HashSet<T>(comparer)));
        }

        public static IPromise<ISet<T>> ToSet<T>(IPromiseEnumerator<T> items)
        {
            return ToSet(items, EqualityComparer<T>.Default);
        }
    }
}
