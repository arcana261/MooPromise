using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class CountAlgorithm
    {
        public static IPromise<int> Count<T>(IPromiseEnumerator<T> items, Func<T, int, IPromise<bool>> filter)
        {
            return LongCountAlgorithm.LongCount(items, filter).Then(result => (int)result);
        }

        public static IPromise<int> Count<T>(IPromiseEnumerator<T> items, Func<T, int, bool> filter)
        {
            return LongCountAlgorithm.LongCount(items, filter).Then(result => (int)result);
        }

        public static IPromise<int> Count<T>(IPromiseEnumerator<T> items, Func<T, IPromise<bool>> filter)
        {
            return LongCountAlgorithm.LongCount(items, filter).Then(result => (int)result);
        }

        public static IPromise<int> Count<T>(IPromiseEnumerator<T> items, Func<T, bool> filter)
        {
            return LongCountAlgorithm.LongCount(items, filter).Then(result => (int)result);
        }

        public static IPromise<int> Count<T>(IPromiseEnumerator<T> items)
        {
            return LongCountAlgorithm.LongCount(items).Then(result => (int)result);
        }
    }
}
