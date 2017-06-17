using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MooPromise.ThreadPool;

namespace MooPromise.TaskRunner.Moo
{
    internal class ImmediateTaskResult : BaseTaskResult
    {
        public ImmediateTaskResult(IThreadPool threadpool, IThreadPoolResult result) : base(threadpool, result)
        {
        }

        public override ITaskResult Immediately
        {
            get
            {
                return this;
            }
        }

        public override ITaskResult WithPriority(int priority)
        {
            return new BoundTaskResult(this, new PriorityTaskResult(ThreadPool, ThreadPoolResult, priority));
        }

        protected override IThreadPoolResult CreateResult(Action action)
        {
            return ThreadPool.CreateImmediately(action);
        }
    }
}
