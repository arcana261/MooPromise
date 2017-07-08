using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class UnionAlgorithm
    {
        public static IPromiseEnumerator<T> Union<T>(IPromiseEnumerator<T> left, IPromiseEnumerator<T> right, IEqualityComparer<T> comparer)
        {
            return DistinctEnumerator.Create(ConcatEnumerator.Create(left, right), comparer);
        }

        public static IPromiseEnumerator<T> Union<T>(IPromiseEnumerator<T> left, IPromiseEnumerator<T> right)
        {
            return DistinctEnumerator.Create(ConcatEnumerator.Create(left, right));
        }
    }
}
