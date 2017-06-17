using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MooPromise.ThreadPool;
using MooPromise.Backend;

namespace MooPromise.ThreadPool.Moo
{
    internal class BackendResultWithPriority : BaseBackendResult
    {
        private int _priority;

        public BackendResultWithPriority(IBackend threadPool, Action action, int priority)
            : base(threadPool, action)
        {
            _priority = priority;
        }

        protected override void DoStart(IBackend threadPool, Action action)
        {
            threadPool.Add(action, _priority);
        }
    }
}
