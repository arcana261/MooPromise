using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.DataStructure
{
    internal interface IQueue<T> : ICollection<T>
    {
        T Pop();
        T Peek();
        bool TryPop(out T value);
    }
}
