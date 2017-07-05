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

        public PromiseEnumerator(PromiseFactory factory, T current, IEnumerator<IPromise<T>> items)
        {
            this.Factory = factory;
            _current = new NullableResult<T>(current);
            _items = items;
        }

        public PromiseEnumerator(PromiseFactory factory, IEnumerator<IPromise<T>> items)
        {
            this.Factory = factory;
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
                return _items.Current.Then(x => (IPromiseEnumerator<T>)(new PromiseEnumerator<T>(Factory, x, _items)));
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
