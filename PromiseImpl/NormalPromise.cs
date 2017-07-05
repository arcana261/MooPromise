using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MooPromise.TaskRunner;

namespace MooPromise.PromiseImpl
{
    internal class NormalPromise : BasePromise
    {
        public NormalPromise(PromiseFactory promiseFactory, ITaskFactory factory, ITaskResult task) : base(promiseFactory, factory, task)
        {
        }

        public override IPromise Immediately
        {
            get
            {
                return new ImmediatePromise(Factory, TaskFactory, TaskResult);
            }
        }

        public override IPromise Priority(PromisePriority priority)
        {
            return new PriorityPromise(Factory, TaskFactory, TaskResult, priority);
        }

        protected override ITaskResult ProcessTaskResult(ITaskResult result)
        {
            return result;
        }
    }

    internal class NormalPromise<T> : BasePromise<T>
    {
        public NormalPromise(PromiseFactory promiseFactory, ITaskFactory factory, ITaskResult task) : base(promiseFactory, factory, task)
        {
        }

        public override IPromise<T> Immediately
        {
            get
            {
                return new ImmediatePromise<T>(Factory, TaskFactory, TaskResult);
            }
        }

        public override IPromise<T> Priority(PromisePriority priority)
        {
            return new PriorityPromise<T>(Factory, TaskFactory, TaskResult, priority);
        }

        protected override ITaskResult ProcessTaskResult(ITaskResult result)
        {
            return result;
        }
    }
}
