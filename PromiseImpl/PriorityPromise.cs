﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MooPromise.TaskRunner;

namespace MooPromise.PromiseImpl
{
    internal class PriorityPromise : BasePromise
    {
        private PromisePriority _priority;

        public PriorityPromise(PromiseFactory promiseFactory, ITaskFactory factory, ITaskResult task, PromisePriority priority) : base(promiseFactory, factory, task)
        {
            this._priority = priority;
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
            if (_priority == PromisePriority.Immediate)
            {
                return result.Immediately;
            }
            else
            {
                return result.WithPriority((int)_priority);
            }
        }
    }

    internal class PriorityPromise<T> : BasePromise<T>
    {
        private PromisePriority _priority;

        public PriorityPromise(PromiseFactory promiseFactory, ITaskFactory factory, ITaskResult task, PromisePriority priority) : base(promiseFactory, factory, task)
        {
            this._priority = priority;
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
            if (_priority == PromisePriority.Immediate)
            {
                return result.Immediately;
            }
            else
            {
                return result.WithPriority((int)_priority);
            }
        }
    }
}
