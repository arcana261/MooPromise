using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal class PromiseEnumerable<T> : IPromiseEnumerable<T>
    {
        private IPromise<IPromiseEnumerator<T>> _items;

        private PromiseEnumerable(IPromise<IPromiseEnumerator<T>> items)
        {
            _items = items;
        }

        public PromiseEnumerable(IPromise<IEnumerable<T>> items)
            : this(PromiseEnumerator.Create(items))
        {

        }

        public PromiseEnumerable(IEnumerable<T> items)
            : this(PromiseEnumerator.Create(items))
        {

        }

        public IPromise<List<T>> ToList()
        {
            return _items.Then(list => ToListAlgorithm.ToList(list));
        }

        public IPromiseEnumerable<T> Where(Func<T, IPromise<bool>> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => WhereEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<T> Where(Func<T, bool> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => WhereEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<T> Where(Func<T, int, bool> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => WhereEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<T> Where(Func<T, int, IPromise<bool>> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => WhereEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<E> Select<E>(Func<T, int, IPromise<E>> action)
        {
            return new PromiseEnumerable<E>(_items.Then(list => SelectEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<E> Select<E>(Func<T, int, E> action)
        {
            return new PromiseEnumerable<E>(_items.Then(list => SelectEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<E> Select<E>(Func<T, IPromise<E>> action)
        {
            return new PromiseEnumerable<E>(_items.Then(list => SelectEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<E> Select<E>(Func<T, E> action)
        {
            return new PromiseEnumerable<E>(_items.Then(list => SelectEnumerator.Create(list, action)));
        }

        public IPromise<E> Aggregate<E>(Func<E, T, int, IPromise<E>> action, E seed)
        {
            return _items.Then(list => ReduceAlgorithm.Reduce(list, action, seed));
        }

        public IPromise<E> Aggregate<E>(Func<E, T, int, E> action, E seed)
        {
            return _items.Then(list => ReduceAlgorithm.Reduce(list, action, seed));
        }

        public IPromise<E> Aggregate<E>(Func<E, T, IPromise<E>> action, E seed)
        {
            return _items.Then(list => ReduceAlgorithm.Reduce(list, action, seed));
        }

        public IPromise<E> Aggregate<E>(Func<E, T, E> action, E seed)
        {
            return _items.Then(list => ReduceAlgorithm.Reduce(list, action, seed));
        }

        public IPromise<E> Aggregate<E>(Func<E, T, int, IPromise<E>> action)
        {
            return _items.Then(list => ReduceAlgorithm.Reduce(list, action));
        }

        public IPromise<E> Aggregate<E>(Func<E, T, int, E> action)
        {
            return _items.Then(list => ReduceAlgorithm.Reduce(list, action));
        }

        public IPromise<E> Aggregate<E>(Func<E, T, IPromise<E>> action)
        {
            return _items.Then(list => ReduceAlgorithm.Reduce(list, action));
        }

        public IPromise<E> Aggregate<E>(Func<E, T, E> action)
        {
            return _items.Then(list => ReduceAlgorithm.Reduce(list, action));
        }

        public IPromiseEnumerable<T> Each(Func<T, int, IPromise> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => EachAlgorithm.Each(list, action)));
        }

        public IPromiseEnumerable<T> Each(Action<T, int> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => EachAlgorithm.Each(list, action)));
        }

        public IPromiseEnumerable<T> Each(Func<T, IPromise> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => EachAlgorithm.Each(list, action)));
        }

        public IPromiseEnumerable<T> Each(Action<T> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => EachAlgorithm.Each(list, action)));
        }

        public IPromiseEnumerable<T> Catch(Action<Exception> action)
        {
            return new PromiseEnumerable<T>(_items.Catch(action));
        }

        public IPromiseEnumerable<T> Catch(Action action)
        {
            return new PromiseEnumerable<T>(_items.Catch(action));
        }

        public IPromiseEnumerable<T> Finally(Action<Exception> action)
        {
            return new PromiseEnumerable<T>(_items.Finally(action));
        }

        public IPromiseEnumerable<T> Finally(Action action)
        {
            return new PromiseEnumerable<T>(_items.Finally(action));
        }
    }
}
