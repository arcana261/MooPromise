using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class AnyAlgorithm
    {
        public static IPromise<bool> Any<T>(IPromiseEnumerator<T> items)
        {
            return EmptyAlgorithm.Empty(items).Then(result => !result);
        }

        public static IPromise<bool> Any<T>(IPromiseEnumerator<T> items, Func<T, int, IPromise<bool>> predicate)
        {
            return EmptyAlgorithm.Empty(items, predicate).Immediately.Then(result => !result);
        }

        public static IPromise<bool> Any<T>(IPromiseEnumerator<T> items, Func<T, int, bool> predicate)
        {
            return EmptyAlgorithm.Empty(items, predicate).Immediately.Then(result => !result);
        }

        public static IPromise<bool> Any<T>(IPromiseEnumerator<T> items, Func<T, IPromise<bool>> predicate)
        {
            return EmptyAlgorithm.Empty(items, predicate).Immediately.Then(result => !result);
        }

        public static IPromise<bool> Any<T>(IPromiseEnumerator<T> items, Func<T, bool> predicate)
        {
            return EmptyAlgorithm.Empty(items, predicate).Immediately.Then(result => !result);
        }
    }
}
