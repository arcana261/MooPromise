using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MooPromise.TaskRunner;

namespace MooPromise.PromiseImpl
{
    internal class PriorityPromise : BasePromise
    {
        private int _priority;

        public PriorityPromise(ITaskFactory factory, ITaskResult task, int priority) : base(factory, task)
        {
            this._priority = priority;
        }

        public override IPromise Immediately
        {
            get
            {
                return new ImmediatePromise(TaskFactory, TaskResult);
            }
        }

        public override IPromise Priority(PromisePriority priority)
        {
            return new PriorityPromise(TaskFactory, TaskResult, (int)priority);
        }

        protected override ITaskResult ProcessTaskResult(ITaskResult result)
        {
            return result.WithPriority(_priority);
        }
    }

    internal class PriorityPromise<T> : BasePromise<T>
    {
        private int _priority;

        public PriorityPromise(ITaskFactory factory, ITaskResult task, int priority) : base(factory, task)
        {
            this._priority = priority;
        }

        public override IPromise<T> Immediately
        {
            get
            {
                return new ImmediatePromise<T>(TaskFactory, TaskResult);
            }
        }

        public override IPromise<T> Priority(PromisePriority priority)
        {
            return new PriorityPromise<T>(TaskFactory, TaskResult, (int)priority);
        }

        protected override ITaskResult ProcessTaskResult(ITaskResult result)
        {
            return result.WithPriority(_priority);
        }
    }
}
