using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal class SkipWhileEnumerator<T> : IPromiseEnumerator<T>
    {
        private int _index;
        private IPromiseEnumerator<T> _items;
        private Func<T, int, IPromise<bool>> _predicate;

        private SkipWhileEnumerator(int index, IPromiseEnumerator<T> items, Func<T, int, IPromise<bool>> predicate)
        {
            _index = index;
            _items = items;
            _predicate = predicate;
        }

        public SkipWhileEnumerator(IPromiseEnumerator<T> items, Func<T, int, IPromise<bool>> predicate)
            : this(0, items, predicate)
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
                    return this.Factory.StartNew((IPromiseEnumerator<T>)null);
                }

                return _predicate(result.Current, _index).Then(skip =>
                {
                    if (skip)
                    {
                        return (new SkipWhileEnumerator<T>(_index + 1, result, _predicate)).MoveNext();
                    }

                    return this.Factory.StartNew(result);
                });
            });
        }
    }

    internal static class SkipWhileEnumerator
    {
        public static IPromiseEnumerator<T> Create<T>(IPromiseEnumerator<T> items, Func<T, int, IPromise<bool>> predicate)
        {
            return new SkipWhileEnumerator<T>(items, predicate);
        }

        public static IPromiseEnumerator<T> Create<T>(IPromiseEnumerator<T> items, Func<T, int, bool> predicate)
        {
            return Create(items, (x, y) => items.Factory.StartNew(predicate(x, y)));
        }

        public static IPromiseEnumerator<T> Create<T>(IPromiseEnumerator<T> items, Func<T, IPromise<bool>> predicate)
        {
            return Create(items, (x, y) => predicate(x));
        }

        public static IPromiseEnumerator<T> Create<T>(IPromiseEnumerator<T> items, Func<T, bool> predicate)
        {
            return Create(items, (x, y) => predicate(x));
        }

        public static IPromiseEnumerator<T> Create<T>(IPromiseEnumerator<T> items, int count)
        {
            return Create(items, (x, index) => index < count);
        }
    }
}
