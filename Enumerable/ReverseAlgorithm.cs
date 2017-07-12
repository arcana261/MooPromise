using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class ReverseAlgorithm
    {
        public static IPromise<IPromiseEnumerator<T>> Reverse<T>(IPromiseEnumerator<T> items)
        {
            return items.MoveNext().Then(result =>
            {
                if (result == null)
                {
                    return null;
                }

                return PromiseEnumerator.Create(items.Factory, new T[] { result.Current }).Then(right =>
                {
                    return Reverse(result).Then(left =>
                    {
                        return (IPromiseEnumerator<T>)(ConcatEnumerator.Create(left, right));
                    });
                });
            });
        }
    }
}
