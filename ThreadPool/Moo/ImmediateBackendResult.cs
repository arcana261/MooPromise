using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MooPromise.ThreadPool;
using MooPromise.Backend;

namespace MooPromise.ThreadPool.Moo
{
    internal class ImmediateBackendResult : BaseBackendResult
    {
        public ImmediateBackendResult(IBackend threadPool, Action action)
            : base(threadPool, action)
        {

        }

        protected override void DoStart(IBackend threadPool, Action action)
        {
            threadPool.AddImmediately(action);
        }
    }
}
