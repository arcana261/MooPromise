using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise
{
    public interface IPromiseGrouping<TKey, TValue> : IPromiseEnumerable<TValue>
    {
        TKey Key { get; }
    }
}
