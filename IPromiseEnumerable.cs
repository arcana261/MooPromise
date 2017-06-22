using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise
{
    public interface IPromiseEnumerable<T>
    {
        IPromiseEnumerable<T> Where(Func<T, int, IPromise<bool>> action);
        IPromiseEnumerable<T> Where(Func<T, int, bool> action);
        IPromiseEnumerable<T> Where(Func<T, IPromise<bool>> action);
        IPromiseEnumerable<T> Where(Func<T, bool> action);
        IPromiseEnumerable<E> Select<E>(Func<T, int, IPromise<E>> action);
        IPromiseEnumerable<E> Select<E>(Func<T, int, E> action);
        IPromiseEnumerable<E> Select<E>(Func<T, IPromise<E>> action);
        IPromiseEnumerable<E> Select<E>(Func<T, E> action);
        IPromise<E> Aggregate<E>(Func<E, T, int, IPromise<E>> action, E seed);
        IPromise<E> Aggregate<E>(Func<E, T, int, E> action, E seed);
        IPromise<E> Aggregate<E>(Func<E, T, IPromise<E>> action, E seed);
        IPromise<E> Aggregate<E>(Func<E, T, E> action, E seed);
        IPromise<E> Aggregate<E>(Func<E, T, int, IPromise<E>> action);
        IPromise<E> Aggregate<E>(Func<E, T, int, E> action);
        IPromise<E> Aggregate<E>(Func<E, T, IPromise<E>> action);
        IPromise<E> Aggregate<E>(Func<E, T, E> action);
        IPromiseEnumerable<T> Each(Func<T, int, IPromise> action);
        IPromiseEnumerable<T> Each(Action<T, int> action);
        IPromiseEnumerable<T> Each(Func<T, IPromise> action);
        IPromiseEnumerable<T> Each(Action<T> action);
        IPromise<List<T>> ToList();
        IPromiseEnumerable<T> Catch(Action<Exception> action);
        IPromiseEnumerable<T> Catch(Action action);
        IPromiseEnumerable<T> Finally(Action<Exception> action);
        IPromiseEnumerable<T> Finally(Action action);
    }
}
