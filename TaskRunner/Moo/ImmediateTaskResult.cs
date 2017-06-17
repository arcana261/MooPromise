using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MooPromise.ThreadPool;

namespace MooPromise.TaskRunner.Moo
{
    internal class ImmediateTaskResult : BaseTaskResult
    {
        public ImmediateTaskResult(BaseTaskResult owner, IThreadPool threadpool, IThreadPoolResult result) : base(threadpool, result)
        {
            owner.OnResult(value =>
            {
                this.Result = value;
            });
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
            return new BoundTaskResult(this, new PriorityTaskResult(this, ThreadPool, ThreadPoolResult, priority));
        }

        protected override IThreadPoolResult CreateResult(Action action)
        {
            return ThreadPool.CreateImmediately(action);
        }
    }
}
