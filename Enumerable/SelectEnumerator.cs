using MooPromise.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal class SelectEnumerator<T, E> : IPromiseEnumerator<E>
    {
        private int _index;
        private NullableResult<E> _current;
        private IPromiseEnumerator<T> _items;
        private Func<T, int, IPromise<E>> _predicate;

        private SelectEnumerator(int index, E current, IPromiseEnumerator<T> items, Func<T, int, IPromise<E>> predicate)
        {
            _index = index;
            _current = new NullableResult<E>(current);
            _items = items;
            _predicate = predicate;
        }

        public SelectEnumerator(IPromiseEnumerator<T> items, Func<T, int, IPromise<E>> predicate)
        {
            _index = 0;
            _current = new NullableResult<E>();
            _items = items;
            _predicate = predicate;
        }

        public E Current
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

        public IPromise<IPromiseEnumerator<E>> MoveNext()
        {
            return _items.MoveNext().Then(next =>
            {
                if (next == null)
                {
                    return Promise.Factory.StartNew((IPromiseEnumerator<E>)null);
                }

                return _predicate(next.Current, _index).Then(newValue =>
                {
                    return (IPromiseEnumerator<E>)(new SelectEnumerator<T, E>(_index + 1, newValue, _items, _predicate));
                });
            });
        }
    }
}
