using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise
{
    public interface IPromiseEnumerator<T>
    {
        IPromise<IPromiseEnumerator<T>> MoveNext();
        T Current { get; }
    }
}
