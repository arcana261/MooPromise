using MooPromise.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal class PromiseEnumerator<T> : IPromiseEnumerator<T>
    {
        private IEnumerator<IPromise<T>> _items;
        private int _index;
        private IList<T> _itemCache;

        public PromiseEnumerator(PromiseFactory factory, int index, IList<T> itemCache, IEnumerator<IPromise<T>> items)
        {
            this.Factory = factory;
            _items = items;
            _index = index;
            _itemCache = itemCache;
        }

        public PromiseEnumerator(PromiseFactory factory, IEnumerator<IPromise<T>> items)
        {
            this.Factory = factory;
            _items = items;
            _index = 0;
            _itemCache = new List<T>();
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

                return _itemCache[at];
            }
        }

        public IPromise<IPromiseEnumerator<T>> MoveNext()
        {
            if (_index < _itemCache.Count)
            {
                return Factory.Value((IPromiseEnumerator<T>)(new PromiseEnumerator<T>(Factory, _index + 1, _itemCache, _items)));
            }

            if (_items.MoveNext())
            {
                return _items.Current.Then(x =>
                {
                    _itemCache.Add(x);
                    return (IPromiseEnumerator<T>)(new PromiseEnumerator<T>(Factory, _index + 1, _itemCache, _items));
                });
            }
            else
            {
                return Factory.StartNew((IPromiseEnumerator<T>)null);
            }
        }


        public PromiseFactory Factory
        {
            get;
            private set;
        }
    }

    internal static class PromiseEnumerator
    {
        public static IPromise<IPromiseEnumerator<T>> Create<T>(IPromise<IEnumerable<IPromise<T>>> items)
        {
            return items.Then(x => (IPromiseEnumerator<T>)(new PromiseEnumerator<T>(items.Factory, x.GetEnumerator())));
        }

        public static IPromise<IPromiseEnumerator<T>> Create<T>(IPromise<IEnumerable<T>> items)
        {
            return Create(items.Then(x => x.Select(y => items.Factory.StartNew(y))));
        }

        public static IPromise<IPromiseEnumerator<T>> Create<T>(PromiseFactory promiseFactory, IEnumerable<IPromise<T>> items)
        {
            return Create(promiseFactory.StartNew(items));
        }

        public static IPromise<IPromiseEnumerator<T>> Create<T>(PromiseFactory promiseFactory, IEnumerable<T> items)
        {
            return Create(promiseFactory.StartNew(items));
        }
    }
}
