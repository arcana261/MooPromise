using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class DefaultIfEmptyAlgorithm
    {
        public static IPromise<IPromiseEnumerator<T>> DefaultIfEmpty<T>(IPromiseEnumerator<T> items, T defaultValue)
        {
            return items.MoveNext().Then(result =>
            {
                if (result == null)
                {
                    return PromiseEnumerator.Create(items.Factory, new T[] { defaultValue });
                }

                return items.Factory.Value(items);
            });
        }

        public static IPromise<IPromiseEnumerator<T>> DefaultIfEmpty<T>(IPromiseEnumerator<T> items)
        {
            return DefaultIfEmpty(items, default(T));
        }
    }
}
