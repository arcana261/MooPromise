using MooPromise.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal class WhereEnumerator<T> : IPromiseEnumerator<T>
    {
        private int _index;
        private IPromiseEnumerator<T> _items;
        private Func<T, int, IPromise<bool>> _predicate;

        private WhereEnumerator(int index, IPromiseEnumerator<T> items, Func<T, int, IPromise<bool>> predicate)
        {
            _index = index;
            _items = items;
            _predicate = predicate;
        }

        public WhereEnumerator(IPromiseEnumerator<T> items, Func<T, int, IPromise<bool>> predicate)
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

        public IPromise<IPromiseEnumerator<T>> MoveNext()
        {
            return _items.MoveNext().Then(result =>
            {
                if (result == null)
                {
                    return this.Factory.StartNew((IPromiseEnumerator<T>)null);
                }

                return _predicate(result.Current, _index).Then(take =>
                {
                    IPromiseEnumerator<T> ret = new WhereEnumerator<T>(_index + 1, result, _predicate);

                    if (take)
                    {
                        return this.Factory.StartNew(ret);
                    }

                    return ret.MoveNext();
                });
            });
        }

        public PromiseFactory Factory
        {
            get
            {
                return _items.Factory;
            }
        }
    }

    internal static class WhereEnumerator
    {
        public static IPromiseEnumerator<T> Create<T>(IPromiseEnumerator<T> items, Func<T, int, IPromise<bool>> predicate)
        {
            return new WhereEnumerator<T>(items, predicate);
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
    }
}
