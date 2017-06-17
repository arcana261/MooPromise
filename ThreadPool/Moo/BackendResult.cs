using MooPromise.Backend;
using MooPromise.ThreadPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.ThreadPool.Moo
{
    internal class BackendResult : BaseBackendResult
    {
        public BackendResult(IBackend threadPool, Action action)
            : base(threadPool, action)
        {

        }

        protected override void DoStart(IBackend threadPool, Action action)
        {
            threadPool.Add(action);
        }
    }
}
