using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class ReduceAlgorithm
    {
        private static IPromise<E> Reduce<T, E>(IPromiseEnumerator<T> items, int index, Func<E, T, int, IPromise<E>> fn, E seed)
        {
            return items.MoveNext().Then(newItems =>
            {
                if (newItems == null)
                {
                    return items.Factory.StartNew(seed);
                }
                else
                {
                    return fn(seed, newItems.Current, index).Then(next => Reduce(newItems, index + 1, fn, next));
                }
            });
        }

        public static IPromise<E> Reduce<T, E>(IPromiseEnumerator<T> items, Func<E, T, int, IPromise<E>> fn, E seed)
        {
            return Reduce(items, 0, fn, seed);
        }

        public static IPromise<E> Reduce<T, E>(IPromiseEnumerator<T> items, Func<E, T, int, E> fn, E seed)
        {
            return Reduce(items, (prev, current, index) => items.Factory.StartNew(fn(prev, current, index)), seed);
        }

        public static IPromise<E> Reduce<T, E>(IPromiseEnumerator<T> items, Func<E, T, IPromise<E>> fn, E seed)
        {
            return Reduce(items, (prev, current, index) => fn(prev, current), seed);
        }

        public static IPromise<E> Reduce<T, E>(IPromiseEnumerator<T> items, Func<E, T, E> fn, E seed)
        {
            return Reduce(items, (prev, current, index) => fn(prev, current), seed);
        }

        public static IPromise<E> Reduce<T, E>(IPromiseEnumerator<T> items, Func<E, T, int, IPromise<E>> fn)
        {
            return Reduce(items, fn, default(E));
        }

        public static IPromise<E> Reduce<T, E>(IPromiseEnumerator<T> items, Func<E, T, int, E> fn)
        {
            return Reduce(items, fn, default(E));
        }

        public static IPromise<E> Reduce<T, E>(IPromiseEnumerator<T> items, Func<E, T, IPromise<E>> fn)
        {
            return Reduce(items, fn, default(E));
        }

        public static IPromise<E> Reduce<T, E>(IPromiseEnumerator<T> items, Func<E, T, E> fn)
        {
            return Reduce(items, fn, default(E));
        }
    }
}
