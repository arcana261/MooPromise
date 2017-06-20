using MooPromise.TaskRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.PromiseImpl
{
    internal static class PromiseHelpers
    {
        public static ITaskResult ConvertPromiseToTaskResult(ITaskFactory taskFactory, IPromise promise)
        {
            if (promise != null)
            {
                var task = new ManualTaskResult(taskFactory.ThreadPool);
                task.Start();

                promise.Then(() =>
                {
                    task.SetCompleted();
                }).Finally(error =>
                {
                    if (error != null)
                    {
                        task.SetFailed(error);
                    }
                }).Start();

                return task;
            }

            return null;
        }

        public static ITaskResult ConvertPromiseToTaskResult<T>(ITaskFactory taskFactory, IPromise<T> promise)
        {
            if (promise != null)
            {
                var task = new ManualTaskResult(taskFactory.ThreadPool);
                task.Start();

                promise.Then(x =>
                {
                    task.SetResult(x);
                    task.SetCompleted();
                }).Finally(error =>
                {
                    if (error != null)
                    {
                        task.SetFailed(error);
                    }
                }).Start();

                return task;
            }

            return null;
        }
    }
}
