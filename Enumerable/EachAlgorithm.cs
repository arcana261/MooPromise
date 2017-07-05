using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class EachAlgorithm
    {
        public static IPromise<IPromiseEnumerator<T>> Each<T>(IPromiseEnumerator<T> items, Func<T, int, IPromise> action)
        {
            return ReduceAlgorithm.Reduce(items, (prev, current, index) => action(current, index).Then(() => items), items);
        }

        public static IPromise<IPromiseEnumerator<T>> Each<T>(IPromiseEnumerator<T> items, Action<T, int> action)
        {
            return Each(items, (x, index) =>
            {
                action(x, index);
                return items.Factory.StartNew();
            });
        }

        public static IPromise<IPromiseEnumerator<T>> Each<T>(IPromiseEnumerator<T> items, Func<T, IPromise> action)
        {
            return Each(items, (x, index) => action(x));
        }

        public static IPromise<IPromiseEnumerator<T>> Each<T>(IPromiseEnumerator<T> items, Action<T> action)
        {
            return Each(items, (x, index) => action(x));
        }
    }
}
