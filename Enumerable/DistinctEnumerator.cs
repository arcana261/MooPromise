using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal class DistinctEnumerator<T> : IPromiseEnumerator<T>
    {
        private IPromiseEnumerator<T> _items;
        private HashSet<T> _hash;
        
        public DistinctEnumerator(IPromiseEnumerator<T> items, HashSet<T> hash)
        {
            _items = items;
            _hash = hash;
        }

        public DistinctEnumerator(IPromiseEnumerator<T> items, IEqualityComparer<T> comparer)
            : this(items, new HashSet<T>(comparer))
        {
            
        }

        public T Current
        {
            get
            {
                return _items.Current;
            }
        }

        public PromiseFactory Factory
        {
            get
            {
                return _items.Factory;
            }
        }

        public IPromise<IPromiseEnumerator<T>> MoveNext()
        {
            return _items.MoveNext().Then(result =>
            {
                if (result == null)
                {
                    return null;
                }

                var ret = (IPromiseEnumerator<T>)(new DistinctEnumerator<T>(result, _hash));

                if (_hash.Contains(result.Current))
                {
                    return ret.MoveNext();
                }

                _hash.Add(result.Current);
                return this.Factory.Value(ret);
            });
        }
    }

    internal static class DistinctEnumerator
    {
        public static IPromiseEnumerator<T> Create<T>(IPromiseEnumerator<T> items, IEqualityComparer<T> comparer)
        {
            return new DistinctEnumerator<T>(items, comparer);
        }

        public static IPromiseEnumerator<T> Create<T>(IPromiseEnumerator<T> items)
        {
            return Create(items, EqualityComparer<T>.Default);
        }
    }
}
