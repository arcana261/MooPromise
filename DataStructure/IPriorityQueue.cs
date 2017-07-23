using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.DataStructure
{
    internal interface IPriorityQueue<T> : IQueue<T>
    {
        void Add(T item, int priority);
        bool TryPop(out T value, out int priority);
    }
}
