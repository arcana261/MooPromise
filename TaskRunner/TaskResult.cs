using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MooPromise.ThreadPool;
using MooPromise.TaskRunner.Moo;

namespace MooPromise.TaskRunner
{
    internal class TaskResult : BaseTaskResult
    {
        public TaskResult(IThreadPool threadpool, IThreadPoolResult result)
            : base(threadpool, result)
        {

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
            return ThreadPool.Create(action);
        }

        public void SetResult(object result)
        {
            Result = result;
        }
    }
}
