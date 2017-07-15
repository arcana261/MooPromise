using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class IntersectAlgorithm
    {
        public static IPromise<IPromiseEnumerator<T>> Intersect<T>(IPromiseEnumerator<T> left, IPromiseEnumerator<T> right, IEqualityComparer<T> comparer)
        {
            return CountAlgorithm.Count(left).Then(leftCount => CountAlgorithm.Count(right).Then(rightCount =>
            {
                if (leftCount < rightCount)
                {
                    return Tuple.Create(left, right);
                }

                return Tuple.Create(right, left);
            })).Then(result =>
            {
                var x = result.Item1;
                var y = result.Item2;

                return ToSetAlgorithm.ToSet(x, comparer).Then(lookup => WhereEnumerator.Create(y, q => lookup.Contains(q, comparer)));
            });
        }

        public static IPromise<IPromiseEnumerator<T>> Intersect<T>(IPromiseEnumerator<T> left, IPromiseEnumerator<T> right)
        {
            return Intersect(left, right, EqualityComparer<T>.Default);
        }
    }
}
