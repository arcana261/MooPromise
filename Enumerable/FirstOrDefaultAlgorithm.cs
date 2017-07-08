using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class FirstOrDefaultAlgorithm
    {
        public static IPromise<T> FirstOrDefault<T>(IPromiseEnumerator<T> items, T defaultValue)
        {
            return items.MoveNext().Then(result =>
            {
                if (result == null)
                {
                    return defaultValue;
                }

                return result.Current;
            });
        }

        public static IPromise<T> FirstOrDefault<T>(IPromiseEnumerator<T> items)
        {
            return FirstOrDefault(items, default(T));
        }

        public static IPromise<T> FirstOrDefault<T>(IPromiseEnumerator<T> items, Func<T, int, IPromise<bool>> predicate, T defaultValue)
        {
            return FirstOrDefault(WhereEnumerator.Create(items, predicate), defaultValue);
        }

        public static IPromise<T> FirstOrDefault<T>(IPromiseEnumerator<T> items, Func<T, int, bool> predicate, T defaultValue)
        {
            return FirstOrDefault(WhereEnumerator.Create(items, predicate), defaultValue);
        }

        public static IPromise<T> FirstOrDefault<T>(IPromiseEnumerator<T> items, Func<T, IPromise<bool>> predicate, T defaultValue)
        {
            return FirstOrDefault(WhereEnumerator.Create(items, predicate), defaultValue);
        }

        public static IPromise<T> FirstOrDefault<T>(IPromiseEnumerator<T> items, Func<T, bool> predicate, T defaultValue)
        {
            return FirstOrDefault(WhereEnumerator.Create(items, predicate), defaultValue);
        }

        public static IPromise<T> FirstOrDefault<T>(IPromiseEnumerator<T> items, Func<T, int, IPromise<bool>> predicate)
        {
            return FirstOrDefault(items, predicate, default(T));
        }

        public static IPromise<T> FirstOrDefault<T>(IPromiseEnumerator<T> items, Func<T, int, bool> predicate)
        {
            return FirstOrDefault(items, predicate, default(T));
        }

        public static IPromise<T> FirstOrDefault<T>(IPromiseEnumerator<T> items, Func<T, IPromise<bool>> predicate)
        {
            return FirstOrDefault(items, predicate, default(T));
        }

        public static IPromise<T> FirstOrDefault<T>(IPromiseEnumerator<T> items, Func<T, bool> predicate)
        {
            return FirstOrDefault(items, predicate, default(T));
        }
    }
}
