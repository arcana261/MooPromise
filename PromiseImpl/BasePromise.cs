using MooPromise.TaskRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MooPromise.PromiseImpl
{
    internal abstract class BasePromise : IPromise
    {
        private ManualResetEvent _waitHandle;

        public BasePromise(PromiseFactory promiseFactory, ITaskFactory factory, ITaskResult task)
        {
            this.TaskFactory = factory;
            this.TaskResult = task;
            this._waitHandle = null;
            this.Factory = promiseFactory;
        }

        public PromiseFactory Factory
        {
            get;
            private set;
        }

        protected ITaskResult TaskResult
        {
            get;
            private set;
        }

        protected ITaskFactory TaskFactory
        {
            get;
            private set;
        }

        private IPromise CreatePromise(ITaskResult result)
        {
            return new NormalPromise(Factory, TaskFactory, result);
        }

        private IPromise<T> CreatePromise<T>(ITaskResult result)
        {
            return new NormalPromise<T>(Factory, TaskFactory, result);
        }

        protected abstract ITaskResult ProcessTaskResult(ITaskResult result);

        public Exception Error
        {
            get
            {
                return TaskResult.Error;
            }
        }

        public abstract IPromise Immediately { get; }

        public AsyncState State
        {
            get
            {
                return TaskResult.State;
            }
        }

        public bool Cancel()
        {
            return TaskResult.Cancel();
        }

        public IPromise Catch(Action action)
        {
            return CreatePromise(ProcessTaskResult(TaskResult).Catch(action));
        }

        public IPromise Catch(Action<Exception> action)
        {
            return CreatePromise(ProcessTaskResult(TaskResult).Catch(action));
        }

        public IPromise Finally(Action<Exception> action)
        {
            return CreatePromise(ProcessTaskResult(TaskResult).Finally(action));
        }

        public IPromise Finally(Action action)
        {
            return CreatePromise(ProcessTaskResult(TaskResult).Finally(action));
        }

        public abstract IPromise Priority(PromisePriority priority);

        public void Start()
        {
            TaskResult.Start();
        }

        private ITaskResult ConvertPromiseToTaskResult(IPromise promise)
        {
            return PromiseHelpers.ConvertPromiseToTaskResult(TaskFactory, promise);
        }

        private ITaskResult ConvertPromiseToTaskResult<T>(IPromise<T> promise)
        {
            return PromiseHelpers.ConvertPromiseToTaskResult(TaskFactory, promise);
        }

        public IPromise Then(Func<IPromise> action)
        {
            return CreatePromise(ProcessTaskResult(TaskResult).Then(() => ConvertPromiseToTaskResult(action())));
        }

        public IPromise Then(Action action)
        {
            return CreatePromise(ProcessTaskResult(TaskResult).Then(action));
        }

        public IPromise<T> Then<T>(Func<IPromise<T>> action)
        {
            return CreatePromise<T>(ProcessTaskResult(TaskResult).Then(() => ConvertPromiseToTaskResult<T>(action())));
        }

        public IPromise<T> Then<T>(Func<T> action)
        {
            return CreatePromise<T>(ProcessTaskResult(TaskResult).Then(() => (object)action()));
        }


        public System.Threading.WaitHandle AsyncWaitHandle
        {
            get
            {
                lock (this)
                {
                    if (_waitHandle == null)
                    {
                        _waitHandle = new ManualResetEvent(false);

                        this.Immediately.Then(() =>
                        {
                            _waitHandle.Set();
                        }).Finally(() =>
                        {
                            _waitHandle.Set();
                        });
                    }

                    return _waitHandle;
                }
            }
        }

        public void Join()
        {
            while (!AsyncWaitHandle.WaitOne(500))
            {
                if (TaskFactory.IsDisposed)
                {
                    throw new OperationCanceledException();
                }
            }

            if (State == AsyncState.Failed)
            {
                throw Error;
            }
        }
    }

    internal abstract class BasePromise<T> : IPromise<T>
    {
        private ManualResetEvent _waitHandle;

        public BasePromise(PromiseFactory promiseFactory, ITaskFactory factory, ITaskResult task)
        {
            this.TaskFactory = factory;
            this.TaskResult = task;
            this._waitHandle = null;
            this.Factory = promiseFactory;
        }

        public PromiseFactory Factory
        {
            get;
            private set;
        }

        protected ITaskResult TaskResult
        {
            get;
            private set;
        }

        protected ITaskFactory TaskFactory
        {
            get;
            private set;
        }

        private IPromise CreatePromise(ITaskResult result)
        {
            return new NormalPromise(Factory, TaskFactory, result);
        }

        private IPromise<F> CreatePromise<F>(ITaskResult result)
        {
            return new NormalPromise<F>(Factory, TaskFactory, result);
        }

        protected abstract ITaskResult ProcessTaskResult(ITaskResult result);

        public Exception Error
        {
            get
            {
                return TaskResult.Error;
            }
        }

        public abstract IPromise<T> Immediately { get; }

        public AsyncState State
        {
            get
            {
                return TaskResult.State;
            }
        }

        public T Result
        {
            get
            {
                return (T)TaskResult.Result;
            }
        }

        public bool Cancel()
        {
            return TaskResult.Cancel();
        }

        public IPromise Cast()
        {
            return CreatePromise(TaskResult);
        }

        public IPromise<F> Cast<F>()
        {
            return CreatePromise<F>(TaskResult);
        }

        public IPromise<T> Catch(Action action)
        {
            return CreatePromise<T>(ProcessTaskResult(TaskResult).Catch(action));
        }

        public IPromise<T> Catch(Action<Exception> action)
        {
            return CreatePromise<T>(ProcessTaskResult(TaskResult).Catch(action));
        }

        public IPromise<T> Finally(Action action)
        {
            return CreatePromise<T>(ProcessTaskResult(TaskResult).Finally(action));
        }

        public IPromise<T> Finally(Action<Exception> action)
        {
            return CreatePromise<T>(ProcessTaskResult(TaskResult).Finally(action));
        }

        public abstract IPromise<T> Priority(PromisePriority priority);

        public void Start()
        {
            TaskResult.Start();
        }

        private ITaskResult ConvertPromiseToTaskResult(IPromise promise)
        {
            return PromiseHelpers.ConvertPromiseToTaskResult(TaskFactory, promise);
        }

        private ITaskResult ConvertPromiseToTaskResult<F>(IPromise<F> promise)
        {
            return PromiseHelpers.ConvertPromiseToTaskResult(TaskFactory, promise);
        }

        public IPromise Then(Func<T, IPromise> action)
        {
            return CreatePromise(ProcessTaskResult(TaskResult).Then(x => ConvertPromiseToTaskResult(action(x.HasResult ? (T)x.Result : default(T)))));
        }

        public IPromise Then(Action action)
        {
            return CreatePromise(ProcessTaskResult(TaskResult).Then(action));
        }

        public IPromise Then(Func<IPromise> action)
        {
            return CreatePromise(ProcessTaskResult(TaskResult).Then(() => ConvertPromiseToTaskResult(action())));
        }

        public IPromise Then(Action<T> action)
        {
            return CreatePromise(ProcessTaskResult(TaskResult).Then(x =>
            {
                action(x.HasResult ? (T)x.Result : default(T));
            }));
        }

        public IPromise<F> Then<F>(Func<F> action)
        {
            return CreatePromise<F>(ProcessTaskResult(TaskResult).Then(() => (object)action()));
        }

        public IPromise<F> Then<F>(Func<IPromise<F>> action)
        {
            return CreatePromise<F>(ProcessTaskResult(TaskResult).Then(() => ConvertPromiseToTaskResult<F>(action())));
        }

        public IPromise<F> Then<F>(Func<T, IPromise<F>> action)
        {
            return CreatePromise<F>(ProcessTaskResult(TaskResult).Then(x => ConvertPromiseToTaskResult<F>(action(x.HasResult ? (T)x.Result : default(T)))));
        }

        public IPromise<F> Then<F>(Func<T, F> action)
        {
            return CreatePromise<F>(ProcessTaskResult(TaskResult).Then(x => (object)action(x.HasResult ? (T)x.Result : default(T))));
        }

        public System.Threading.WaitHandle AsyncWaitHandle
        {
            get
            {
                lock (this)
                {
                    if (_waitHandle == null)
                    {
                        _waitHandle = new ManualResetEvent(false);

                        this.Immediately.Finally(() =>
                        {
                            _waitHandle.Set();
                        });
                    }

                    return _waitHandle;
                }
            }
        }

        public IPromiseEnumerable<T> Enumerable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public T Join()
        {
            while (!AsyncWaitHandle.WaitOne(500))
            {
                if (TaskFactory.IsDisposed)
                {
                    throw new OperationCanceledException();
                }
            }

            if (State == AsyncState.Failed)
            {
                throw Error;
            }

            return Result;
        }
    }
}
