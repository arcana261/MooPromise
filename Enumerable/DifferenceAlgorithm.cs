using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class DifferenceAlgorithm
    {
        public static IPromise<IPromiseEnumerator<T>> Difference<T>(IPromiseEnumerator<T> left, IPromiseEnumerator<T> right, IEqualityComparer<T> comparer)
        {
            return ToSetAlgorithm.ToSet(right, comparer).Then(lookup => WhereEnumerator.Create(left, q => !lookup.Contains(q, comparer)));
        }

        public static IPromise<IPromiseEnumerator<T>> Difference<T>(IPromiseEnumerator<T> left, IPromiseEnumerator<T> right)
        {
            return Difference(left, right, EqualityComparer<T>.Default);
        }
    }
}
