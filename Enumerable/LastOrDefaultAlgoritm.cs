using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class LastOrDefaultAlgoritm
    {
        public static IPromise<T> LastOrDefault<T>(IPromiseEnumerator<T> items, T defaultValue)
        {
            return ReduceAlgorithm.Reduce(items, (prev, current) => current, defaultValue);
        }

        public static IPromise<T> LastOrDefault<T>(IPromiseEnumerator<T> items)
        {
            return LastOrDefault(items, default(T));
        }

        public static IPromise<T> LastOrDefault<T>(IPromiseEnumerator<T> items, Func<T, int, IPromise<bool>> predicate, T defaultValue)
        {
            return LastOrDefault(WhereEnumerator.Create(items, predicate), defaultValue);
        }

        public static IPromise<T> LastOrDefault<T>(IPromiseEnumerator<T> items, Func<T, int, bool> predicate, T defaultValue)
        {
            return LastOrDefault(WhereEnumerator.Create(items, predicate), defaultValue);
        }

        public static IPromise<T> LastOrDefault<T>(IPromiseEnumerator<T> items, Func<T, IPromise<bool>> predicate, T defaultValue)
        {
            return LastOrDefault(WhereEnumerator.Create(items, predicate), defaultValue);
        }

        public static IPromise<T> LastOrDefault<T>(IPromiseEnumerator<T> items, Func<T, bool> predicate, T defaultValue)
        {
            return LastOrDefault(WhereEnumerator.Create(items, predicate), defaultValue);
        }

        public static IPromise<T> LastOrDefault<T>(IPromiseEnumerator<T> items, Func<T, int, IPromise<bool>> predicate)
        {
            return LastOrDefault(WhereEnumerator.Create(items, predicate));
        }

        public static IPromise<T> LastOrDefault<T>(IPromiseEnumerator<T> items, Func<T, int, bool> predicate)
        {
            return LastOrDefault(WhereEnumerator.Create(items, predicate));
        }

        public static IPromise<T> LastOrDefault<T>(IPromiseEnumerator<T> items, Func<T, IPromise<bool>> predicate)
        {
            return LastOrDefault(WhereEnumerator.Create(items, predicate));
        }

        public static IPromise<T> LastOrDefault<T>(IPromiseEnumerator<T> items, Func<T, bool> predicate)
        {
            return LastOrDefault(WhereEnumerator.Create(items, predicate));
        }
    }
}
