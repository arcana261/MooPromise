using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class SingleAlgorithm
    {
        public static IPromise<T> Single<T>(IPromiseEnumerator<T> items)
        {
            return items.MoveNext().Then(result =>
            {
                if (result == null)
                {
                    throw new IndexOutOfRangeException();
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

        public static IPromise<T> Single<T>(IPromiseEnumerator<T> items, Func<T, int, IPromise<bool>> predicate)
        {
            return Single(WhereEnumerator.Create(items, predicate));
        }

        public static IPromise<T> Single<T>(IPromiseEnumerator<T> items, Func<T, int, bool> predicate)
        {
            return Single(WhereEnumerator.Create(items, predicate));
        }

        public static IPromise<T> Single<T>(IPromiseEnumerator<T> items, Func<T, IPromise<bool>> predicate)
        {
            return Single(WhereEnumerator.Create(items, predicate));
        }

        public static IPromise<T> Single<T>(IPromiseEnumerator<T> items, Func<T, bool> predicate)
        {
            return Single(WhereEnumerator.Create(items, predicate));
        }
    }
}
