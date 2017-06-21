using MooPromise.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal class PromiseEnumerator<T> : IPromiseEnumerator<T>
    {
        private NullableResult<T> _current;
        private IEnumerator<IPromise<T>> _items;

        public PromiseEnumerator(T current, IEnumerator<IPromise<T>> items)
        {
            _current = new NullableResult<T>(current);
            _items = items;
        }

        public PromiseEnumerator(IEnumerator<IPromise<T>> items)
        {
            _current = new NullableResult<T>();
            _items = items;
        }

        public T Current
        {
            get
            {
                if (!_current.HasResult)
                {
                    throw new NullReferenceException();
                }

                return _current.Result;
            }
        }

        public IPromise<IPromiseEnumerator<T>> MoveNext()
        {
            if (_items.MoveNext())
            {
                return _items.Current.Then(x => (IPromiseEnumerator<T>)(new PromiseEnumerator<T>(x, _items)));
            }
            else
            {
                return Promise.Factory.StartNew((IPromiseEnumerator<T>)null);
            }
        }
    }

    internal static class PromiseEnumerator
    {
        public static IPromise<IPromiseEnumerator<T>> Create<T>(IPromise<IEnumerable<IPromise<T>>> items)
        {
            return items.Then(x => (IPromiseEnumerator<T>)(new PromiseEnumerator<T>(x.GetEnumerator())));
        }

        public static IPromise<IPromiseEnumerator<T>> Create<T>(IPromise<IEnumerable<T>> items)
        {
            return Create(items.Then(x => x.Select(y => Promise.Factory.StartNew(y))));
        }

        public static IPromise<IPromiseEnumerator<T>> Create<T>(IEnumerable<IPromise<T>> items)
        {
            return Create(Promise.Factory.StartNew(items));
        }

        public static IPromise<IPromiseEnumerator<T>> Create<T>(IEnumerable<T> items)
        {
            return Create(Promise.Factory.StartNew(items));
        }
    }
}
