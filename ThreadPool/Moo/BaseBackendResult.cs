using MooPromise.Backend;
using MooPromise.ExceptionHandling;
using MooPromise.ThreadPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.ThreadPool.Moo
{
    internal abstract class BaseBackendResult : BaseThreadPoolResult
    {
        private IBackend _backend;
        private Action _action;

        public BaseBackendResult(IBackend backend, Action action)
        {
            _backend = backend;
            _action = action;
        }

        protected abstract void DoStart(IBackend backend, Action action);

        protected override void DoStart()
        {
            Action convertedAction = new Action(() =>
            {
                try
                {
                    lock (SyncRoot)
                    {
                        if (State == AsyncState.Canceled)
                        {
                            return;
                        }
                    }

                    SetRunning();
                    _action();

                    lock (SyncRoot)
                    {
                        if (State == AsyncState.Canceled)
                        {
                            return;
                        }
                    }

                    SetCompleted();
                }
                catch(Exception e)
                {
                    try
                    {
                        SetFailed(e);
                    }
                    catch (Exception nested)
                    {
                        Environment.FailFast("Error occured while calling error handler", ExceptionUtility.AggregateExceptions("Error occured while calling error handler", nested, e));
                    }
                }
            });

            DoStart(_backend, convertedAction);
        }
    }
}
