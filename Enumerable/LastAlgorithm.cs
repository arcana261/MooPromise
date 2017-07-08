﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class LastAlgorithm
    {
        public static IPromise<T> Last<T>(IPromiseEnumerator<T> items)
        {
            return items.MoveNext().Then(result =>
            {
                if (result == null)
                {
                    throw new IndexOutOfRangeException();
                }

                return LastOrDefaultAlgoritm.LastOrDefault(items);
            });
        }

        public static IPromise<T> Last<T>(IPromiseEnumerator<T> items, Func<T, int, IPromise<bool>> predicate)
        {
            return Last(WhereEnumerator.Create(items, predicate));
        }

        public static IPromise<T> Last<T>(IPromiseEnumerator<T> items, Func<T, int, bool> predicate)
        {
            return Last(WhereEnumerator.Create(items, predicate));
        }

        public static IPromise<T> Last<T>(IPromiseEnumerator<T> items, Func<T, IPromise<bool>> predicate)
        {
            return Last(WhereEnumerator.Create(items, predicate));
        }

        public static IPromise<T> Last<T>(IPromiseEnumerator<T> items, Func<T, bool> predicate)
        {
            return Last(WhereEnumerator.Create(items, predicate));
        }
    }
}
