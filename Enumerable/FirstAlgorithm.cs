using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class FirstAlgorithm
    {
        public static IPromise<T> First<T>(IPromiseEnumerator<T> items)
        {
            return items.MoveNext().Then(result =>
            {
                if (result == null)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return result.Current;
            });
        }

        public static IPromise<T> First<T>(IPromiseEnumerator<T> items, Func<T, int, IPromise<bool>> predicate)
        {
            return First(WhereEnumerator.Create(items, predicate));
        }

        public static IPromise<T> First<T>(IPromiseEnumerator<T> items, Func<T, int, bool> predicate)
        {
            return First(WhereEnumerator.Create(items, predicate));
        }

        public static IPromise<T> First<T>(IPromiseEnumerator<T> items, Func<T, IPromise<bool>> predicate)
        {
            return First(WhereEnumerator.Create(items, predicate));
        }

        public static IPromise<T> First<T>(IPromiseEnumerator<T> items, Func<T, bool> predicate)
        {
            return First(WhereEnumerator.Create(items, predicate));
        }
    }
}
