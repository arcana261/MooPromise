using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal interface IPromiseEnumerator<T>
    {
        IPromise<IPromiseEnumerator<T>> MoveNext();
        T Current { get; }
    }
}
