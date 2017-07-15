using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class GroupByAlgorithm
    {
        public static IPromiseEnumerator<IPromiseGrouping<TKey, TValue>> GroupBy<T, TKey, TValue>(
            IPromiseEnumerator<T> items,
            Func<T, int, IPromise<TKey>> key,
            Func<T, int, IPromise<TValue>> value,
            IEqualityComparer<TKey> comparer)
        {
            var tuples = SelectEnumerator.Create(items, (item, index) => key(item, index).Then(k => value(item, index).Then(v => Tuple.Create(k, v))));
            var keys = DistinctEnumerator.Create(SelectEnumerator.Create(tuples, x => x.Item1));
            return SelectEnumerator.Create(keys, k => (IPromiseGrouping<TKey, TValue>)(new PromiseGrouping<T, TKey, TValue>(k, tuples, comparer)));
        }
    }
}
