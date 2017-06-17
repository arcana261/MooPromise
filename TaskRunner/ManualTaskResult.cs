using MooPromise.TaskRunner.Moo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MooPromise.ThreadPool;

namespace MooPromise.TaskRunner
{
    internal class ManualTaskResult : TaskResult
    {
        public ManualTaskResult(IThreadPool threadpool) : base(threadpool, new ManualThreadPoolResult())
        {

        }

        public void SetCompleted()
        {
            ((ManualThreadPoolResult)ThreadPoolResult).SetCompleted();
        }

        public void SetFailed(Exception error)
        {
            ((ManualThreadPoolResult)ThreadPoolResult).SetFailed(error);
        }
    }
}
