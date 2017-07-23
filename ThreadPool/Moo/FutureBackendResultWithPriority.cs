using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.ThreadPool.Moo
{
    internal class FutureBackendResultWithPriority : FutureBackendResult
    {
        private int _priority;

        public FutureBackendResultWithPriority(IBackend threadPool, int dueTickCount, Action action, int priority) : base(threadPool, dueTickCount, action)
        {
            this._priority = priority;
        }

        protected override void DoStart(IBackend threadPool, Action action)
        {
            threadPool.AddFuture(DueTickCount, action, _priority);
        }
    }
}
