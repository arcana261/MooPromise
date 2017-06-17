using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MooPromise.TaskRunner;

namespace MooPromise.PromiseImpl
{
    internal class ManualPromise : NormalPromise
    {
        public ManualPromise(ITaskFactory factory) : base(factory, new ManualTaskResult(factory.ThreadPool))
        {
        }

        public void SetCompleted()
        {
            ((ManualTaskResult)TaskResult).SetCompleted();
        }

        public void SetFailed(Exception error)
        {
            ((ManualTaskResult)TaskResult).SetFailed(error);
        }
    }

    internal class ManualPromise<T> : NormalPromise<T>
    {
        public ManualPromise(ITaskFactory factory) : base(factory, new ManualTaskResult(factory.ThreadPool))
        {
        }

        public void SetResult(T result)
        {
            ((ManualTaskResult)TaskResult).SetResult((object)result);
        }

        public void SetCompleted()
        {
            ((ManualTaskResult)TaskResult).SetCompleted();
        }

        public void SetFailed(Exception error)
        {
            ((ManualTaskResult)TaskResult).SetFailed(error);
        }
    }
}
