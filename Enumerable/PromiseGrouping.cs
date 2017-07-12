using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal class PromiseGrouping<TSource, TKey, TValue> : PromiseEnumerable<TValue>, IPromiseGrouping<TKey, TValue>
    {
        public PromiseGrouping(TKey key, IPromiseEnumerator<Tuple<TKey, TValue>> items, IEqualityComparer<TKey> comparer)
            : base(items.Factory.Value(SelectEnumerator.Create(WhereEnumerator.Create(items, item => comparer.Equals(key, item.Item1)), item => item.Item2)))
        {
            this.Key = key;
        }

        public TKey Key
        {
            get;
            private set;
        }
    }
}
