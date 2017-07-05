using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class LongCountAlgorithm
    {
        public static IPromise<long> LongCount<T>(IPromiseEnumerator<T> items, Func<T, int, IPromise<bool>> filter)
        {
            return ReduceAlgorithm.Reduce<T, long>(items, (prev, current, index) =>
            {
                return filter(current, index).Then(pick =>
                {
                    if (pick)
                    {
                        return prev + 1;
                    }

                    return prev;
                });
            }, 0);
        }

        public static IPromise<long> LongCount<T>(IPromiseEnumerator<T> items, Func<T, int, bool> filter)
        {
            return LongCount(items, (item, index) => items.Factory.StartNew(filter(item, index)));
        }

        public static IPromise<long> LongCount<T>(IPromiseEnumerator<T> items, Func<T, IPromise<bool>> filter)
        {
            return LongCount(items, (item, index) => filter(item));
        }

        public static IPromise<long> LongCount<T>(IPromiseEnumerator<T> items, Func<T, bool> filter)
        {
            return LongCount(items, (item, index) => filter(item));
        }

        public static IPromise<long> LongCount<T>(IPromiseEnumerator<T> items)
        {
            return LongCount(items, item => true);
        }
    }
}
