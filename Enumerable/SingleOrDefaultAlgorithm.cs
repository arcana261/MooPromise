using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class SingleOrDefaultAlgorithm
    {
        public static IPromise<T> SingleOrDefault<T>(IPromiseEnumerator<T> items, T defaultValue)
        {
            return items.MoveNext().Then(result =>
            {
                if (result == null)
                {
                    return items.Factory.Value(defaultValue);
                }

                return result.MoveNext().Then(rest =>
                {
                    if (rest != null)
                    {
                        throw new InvalidOperationException();
                    }

                    return result.Current;
                });
            });
        }

        public static IPromise<T> SingleOrDefault<T>(IPromiseEnumerator<T> items)
        {
            return SingleOrDefault(items, default(T));
        }

        public static IPromise<T> SingleOrDefault<T>(IPromiseEnumerator<T> items, Func<T, int, IPromise<bool>> predicate, T defaultValue)
        {
            return SingleOrDefault(WhereEnumerator.Create(items, predicate), defaultValue);
        }

        public static IPromise<T> SingleOrDefault<T>(IPromiseEnumerator<T> items, Func<T, int, bool> predicate, T defaultValue)
        {
            return SingleOrDefault(WhereEnumerator.Create(items, predicate), defaultValue);
        }

        public static IPromise<T> SingleOrDefault<T>(IPromiseEnumerator<T> items, Func<T, IPromise<bool>> predicate, T defaultValue)
        {
            return SingleOrDefault(WhereEnumerator.Create(items, predicate), defaultValue);
        }

        public static IPromise<T> SingleOrDefault<T>(IPromiseEnumerator<T> items, Func<T, bool> predicate, T defaultValue)
        {
            return SingleOrDefault(WhereEnumerator.Create(items, predicate), defaultValue);
        }

        public static IPromise<T> SingleOrDefault<T>(IPromiseEnumerator<T> items, Func<T, int, IPromise<bool>> predicate)
        {
            return SingleOrDefault(WhereEnumerator.Create(items, predicate));
        }

        public static IPromise<T> SingleOrDefault<T>(IPromiseEnumerator<T> items, Func<T, int, bool> predicate)
        {
            return SingleOrDefault(WhereEnumerator.Create(items, predicate));
        }

        public static IPromise<T> SingleOrDefault<T>(IPromiseEnumerator<T> items, Func<T, IPromise<bool>> predicate)
        {
            return SingleOrDefault(WhereEnumerator.Create(items, predicate));
        }

        public static IPromise<T> SingleOrDefault<T>(IPromiseEnumerator<T> items, Func<T, bool> predicate)
        {
            return SingleOrDefault(WhereEnumerator.Create(items, predicate));
        }
    }
}
