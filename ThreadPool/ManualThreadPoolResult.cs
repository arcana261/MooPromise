using MooPromise.ThreadPool.Moo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.ThreadPool
{
    internal class ManualThreadPoolResult : BaseThreadPoolResult
    {
        protected override void DoStart()
        {
            
        }

        public new void SetCompleted()
        {
            base.SetCompleted();
        }

        public new void SetFailed(Exception error)
        {
            base.SetFailed(error);
        }
    }
}
