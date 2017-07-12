using MooPromise.Enumerable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise
{
    public static class EnumerableExtensions
    {
        public static IPromiseEnumerable<T> Promesify<T>(this IEnumerable<T> items, PromiseFactory factory)
        {
            return new PromiseEnumerable<T>(factory, items);
        }

        public static IPromiseEnumerable<T> Promesify<T>(this IEnumerable<T> items)
        {
            return items.Promesify(Promise.Factory);
        }

        public static IPromise JoinParallel(this IEnumerable<IPromise> items)
        {
            return Promise.JoinParallel(items);
        }

        public static IPromise JoinParallel<T>(this IEnumerable<IPromise<T>> items)
        {
            return Promise.JoinParallel(items);
        }

        public static IPromise JoinParallel(this IEnumerable<IPromise> items, PromiseFactory factory)
        {
            return factory.JoinParallel(items);
        }

        public static IPromise JoinParallel<T>(this IEnumerable<IPromise<T>> items, PromiseFactory factory)
        {
            return factory.JoinParallel(items);
        }

        public static IPromise JoinSerial(this IEnumerable<IPromise> items)
        {
            return Promise.JoinSerial(items);
        }

        public static IPromise JoinSerial<T>(this IEnumerable<IPromise<T>> items)
        {
            return Promise.JoinSerial(items);
        }

        public static IPromise JoinSerial(this IEnumerable<IPromise> items, PromiseFactory factory)
        {
            return factory.JoinSerial(items);
        }

        public static IPromise JoinSerial<T>(this IEnumerable<IPromise<T>> items, PromiseFactory factory)
        {
            return factory.JoinSerial(items);
        }
    }
}
