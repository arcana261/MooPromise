using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise
{
    public interface IEnumerablePromise<T> : IPromise<IEnumerable<IPromise<T>>>
    {
        IPromise<IPromiseEnumerator<T>> GetEnumerator();
    }
}
