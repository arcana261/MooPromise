using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise
{
    public interface IBackend : IDisposable
    {
        void Add(Action action);
        void Add(Action action, int priority);
        void AddImmediately(Action action);
        bool IsCurrentThreadManagedByBackend();
        void AddFuture(int dueTickTime, Action action);
        void AddFuture(int dueTickTime, Action action, int priority);
        void WaitUntilDisposed();
        bool WaitUntilDisposed(int waitMs);
#if DEBUG
        IEnumerable<int> ManagedThreadIds { get; }
#endif
    }
}
