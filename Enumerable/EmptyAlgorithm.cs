﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class EmptyAlgorithm
    {
        public static IPromise<bool> Empty<T>(IPromiseEnumerator<T> items)
        {
            return items.MoveNext().Then(result =>
            {
                if (result == null)
                {
                    return true;
                }

                return false;
            });
        }

        public static IPromise<bool> Empty<T>(IPromiseEnumerator<T> items, Func<T, int, IPromise<bool>> predicate)
        {
            return Empty(WhereEnumerator.Create(items, predicate));
        }

        public static IPromise<bool> Empty<T>(IPromiseEnumerator<T> items, Func<T, int, bool> predicate)
        {
            return Empty(WhereEnumerator.Create(items, predicate));
        }

        public static IPromise<bool> Empty<T>(IPromiseEnumerator<T> items, Func<T, IPromise<bool>> predicate)
        {
            return Empty(WhereEnumerator.Create(items, predicate));
        }

        public static IPromise<bool> Empty<T>(IPromiseEnumerator<T> items, Func<T, bool> predicate)
        {
            return Empty(WhereEnumerator.Create(items, predicate));
        }
    }
}
