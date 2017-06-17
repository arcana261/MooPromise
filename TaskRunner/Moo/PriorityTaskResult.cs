using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MooPromise.ThreadPool;

namespace MooPromise.TaskRunner.Moo
{
    internal class PriorityTaskResult : BaseTaskResult
    {
        private int _priority;

        public PriorityTaskResult(BaseTaskResult owner, IThreadPool threadpool, IThreadPoolResult result, int priority) : base(threadpool, result)
        {
            this._priority = priority;
            owner.OnResult(value =>
            {
                this.Result = value;
            });
        }

        public override ITaskResult Immediately
        {
            get
            {
                return new BoundTaskResult(this, new ImmediateTaskResult(this, ThreadPool, ThreadPoolResult));
            }
        }

        public override ITaskResult WithPriority(int priority)
        {
            return new BoundTaskResult(this, new PriorityTaskResult(this, ThreadPool, ThreadPoolResult, priority));
        }

        protected override IThreadPoolResult CreateResult(Action action)
        {
            return ThreadPool.Create(action, _priority);
        }
    }
}
