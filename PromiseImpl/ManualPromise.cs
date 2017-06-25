using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MooPromise.TaskRunner;

namespace MooPromise.PromiseImpl
{
    internal class ManualPromise : NormalPromise, IManualPromise
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

        public void Resolve()
        {
            lock (this)
            {
                Start();
                SetCompleted();
            }
        }

        public void Reject(Exception error)
        {
            lock (this)
            {
                Start();
                SetFailed(error);
            }
        }
    }

    internal class ManualPromise<T> : NormalPromise<T>, IManualPromise<T>
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

        public void Resolve(T result)
        {
            lock (this)
            {
                Start();
                SetResult(result);
                SetCompleted();
            }
        }

        public void Reject(Exception error)
        {
            lock (this)
            {
                Start();
                SetFailed(error);
            }
        }
    }
}
