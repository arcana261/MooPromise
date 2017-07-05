using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MooPromise.TaskRunner;

namespace MooPromise.PromiseImpl
{
    internal class ImmediatePromise : BasePromise
    {
        public ImmediatePromise(PromiseFactory promiseFactory, ITaskFactory factory, ITaskResult task) : base(promiseFactory, factory, task)
        {
        }

        public override IPromise Immediately
        {
            get
            {
                return this;
            }
        }

        public override IPromise Priority(PromisePriority priority)
        {
            return new PriorityPromise(Factory, TaskFactory, TaskResult, priority);
        }

        protected override ITaskResult ProcessTaskResult(ITaskResult result)
        {
            return result.Immediately;
        }
    }

    internal class ImmediatePromise<T> : BasePromise<T>
    {
        public ImmediatePromise(PromiseFactory promiseFactory, ITaskFactory factory, ITaskResult task) : base(promiseFactory, factory, task)
        {
        }

        public override IPromise<T> Immediately
        {
            get
            {
                return this;
            }
        }

        public override IPromise<T> Priority(PromisePriority priority)
        {
            return new PriorityPromise<T>(Factory, TaskFactory, TaskResult, priority);
        }

        protected override ITaskResult ProcessTaskResult(ITaskResult result)
        {
            return result.Immediately;
        }
    }
}
