using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class EnumeratorExtensions
    {
        public static IPromise<IPromiseEnumerator<T>> Advance<T>(this IPromiseEnumerator<T> items, int count)
        {
            if (count < 1)
            {
                return items.Factory.Value(items);
            }

            return items.MoveNext().Then(next => next != null ? next.Advance(count - 1) : items.Factory.Value<IPromiseEnumerator<T>>(null));
        }
    }
}
