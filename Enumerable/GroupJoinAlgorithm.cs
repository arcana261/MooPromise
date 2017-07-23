using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class GroupJoinAlgorithm
    {
        public static IPromise<IPromiseEnumerator<Result>> GroupJoin<Inner, Outer, Key, Result>(
            IPromiseEnumerator<Inner> inner, IPromiseEnumerator<Outer> outer,
            Func<Inner, int, IPromise<Key>> innerKey, Func<Outer, int, IPromise<Key>> outerKey,
            Func<Outer, IPromiseEnumerable<Inner>, IPromise<Result>> result,
            IEqualityComparer<Key> comparer)
        {
            var innerKeys = SelectEnumerator.Create(inner, (x, i) => innerKey(x, i).Then(v => Tuple.Create(v, x)));

            return inner.Factory.Value(SelectEnumerator.Create(outer, (x, i) => outerKey(x, i).Then(
                k => result(x, new PromiseEnumerable<Inner>(
                    inner.Factory.Value(SelectEnumerator.Create(WhereEnumerator.Create(innerKeys, y => comparer.Equals(y.Item1, k)), y => y.Item2))
            )))));
        }
    }
}
