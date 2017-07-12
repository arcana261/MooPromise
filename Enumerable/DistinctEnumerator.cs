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
        private IList<Tuple<T, IPromiseEnumerator<T>>> _itemCache;
        private int _index;
        
        public DistinctEnumerator(IPromiseEnumerator<T> items, HashSet<T> hash, IList<Tuple<T, IPromiseEnumerator<T>>> itemCache, int index)
        {
            _items = items;
            _hash = hash;
            _itemCache = itemCache;
            _index = index;
        }

        public DistinctEnumerator(IPromiseEnumerator<T> items, IEqualityComparer<T> comparer)
            : this(items, new HashSet<T>(comparer), new List<Tuple<T, IPromiseEnumerator<T>>>(), 0)
        {
            
        }

        public T Current
        {
            get
            {
                int at = _index - 1;

                if (at < 0 || at >= _itemCache.Count)
                {
                    throw new NullReferenceException();
                }

                return _itemCache[at].Item1;
            }
        }

        public PromiseFactory Factory
        {
            get
            {
                return _items.Factory;
            }
        }

        private IPromise<IPromiseEnumerator<T>> FindNext(IPromiseEnumerator<T> items)
        {
            return items.MoveNext().Then(result =>
            {
                if (result == null)
                {
                    return null;
                }

                if (_hash.Contains(result.Current))
                {
                    return FindNext(result);
                }

                return Factory.Value(result);
            });
        }

        public IPromise<IPromiseEnumerator<T>> MoveNext()
        {
            if (_index < _itemCache.Count)
            {
                return Factory.Value((IPromiseEnumerator<T>)(new DistinctEnumerator<T>(_itemCache[_index].Item2, _hash, _itemCache, _index + 1)));
            }

            return FindNext(_items).Then(result =>
            {
                if (result == null)
                {
                    return null;
                }

                _hash.Add(result.Current);
                _itemCache.Add(new Tuple<T, IPromiseEnumerator<T>>(result.Current, result));

                return (IPromiseEnumerator<T>)(new DistinctEnumerator<T>(result, _hash, _itemCache, _index + 1));
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
