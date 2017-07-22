using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.ThreadPool.Moo
{
    internal class FutureBackendResult : BackendResult
    {
        protected int DueTickCount
        {
            get;
            private set;
        }

        public FutureBackendResult(IBackend threadPool, int dueTickCount, Action action) : base(threadPool, action)
        {
            this.DueTickCount = dueTickCount;
        }

        protected override void DoStart(IBackend threadPool, Action action)
        {
            threadPool.AddFuture(DueTickCount, action);
        }
    }
}
