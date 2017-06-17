using MooPromise.Backend;
using MooPromise.ExceptionHandling;
using MooPromise.ThreadPool;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MooPromise.TaskRunner.Moo
{
    internal abstract class BaseTaskResult : ITaskResult
    {
        private volatile object _result;
        private volatile bool _hasResult;
        private IList<Action<object>> _onResultList;

        public BaseTaskResult(IThreadPool threadpool, IThreadPoolResult result)
        {
            this.ThreadPool = threadpool;
            this.ThreadPoolResult = result;
            this._result = null;
            this._hasResult = false;
            this._onResultList = null;
        }

        protected IThreadPool ThreadPool
        {
            get;
            private set;
        }

        protected IThreadPoolResult ThreadPoolResult
        {
            get;
            private set;
        }

        private TaskResult CreateTask(Action action)
        {
            return CreateTask(CreateResult(action));
        }

        private TaskResult CreateTask(IThreadPoolResult result)
        {
            return new TaskResult(ThreadPool, result);
        }

        protected abstract IThreadPoolResult CreateResult(Action action);

        public Exception Error
        {
            get
            {
                return ThreadPoolResult.Error;
            }
        }

        public abstract ITaskResult Immediately { get; }
        public AsyncState State
        {
            get
            {
                return ThreadPoolResult.State;
            }
        }

        public object Result
        {
            get
            {
                lock (this)
                {
                    if (!_hasResult)
                    {
                        throw new InvalidOperationException("there is no result associated");
                    }

                    return _result;
                }
            }

            protected set
            {
                lock (this)
                {
                    _result = value;
                    _hasResult = true;

                    if (_onResultList != null)
                    {
                        foreach (var callback in _onResultList)
                        {
                            callback(value);
                        }
                    }
                }
            }
        }

        public bool HasResult
        {
            get
            {
                lock (this)
                {
                    return _hasResult;
                }
            }
        }

        public void OnResult(Action<object> callback)
        {
            lock (this)
            {
                if (HasResult)
                {
                    callback(Result);
                }
                else
                {
                    if (_onResultList == null)
                    {
                        _onResultList = new List<Action<object>>();
                    }

                    _onResultList.Add(callback);
                }
            }
        }

        public ITaskResult Catch(Action<Exception> action)
        {
            var ret = new ManualThreadPoolResult();
            ret.Start();
            var task = CreateTask(ret);

            ThreadPoolResult.OnFailed(error =>
            {
                var subResult = CreateResult(() =>
                {
                    if (!(error is FailureProcessedException))
                    {
                        action(error);
                    }
                });

                subResult.OnCompleted(() =>
                {
                    if (!(error is FailureProcessedException))
                    {
                        ret.SetFailed(new FailureProcessedException("failure caught", error));
                    }
                    else
                    {
                        ret.SetFailed(error);
                    }
                });

                subResult.OnFailed(nested =>
                {
                    ret.SetFailed(ProcessException(ExceptionUtility.AggregateExceptions("catch handler failed", nested, error)));
                });

                subResult.Start();
            });

            ThreadPoolResult.OnCompleted(() =>
            {
                if (HasResult)
                {
                    task.Result = Result;
                }
                ret.SetCompleted();
            });

            return new BoundTaskResult(this, task);
        }

        public ITaskResult Finally(Action action)
        {
            return Finally(error =>
            {
                action();
            });
        }

        public ITaskResult Finally(Action<Exception> action)
        {
            var ret = new ManualThreadPoolResult();
            ret.Start();
            var task = CreateTask(ret);

            ThreadPoolResult.OnCompleted(() =>
            {
                var subResult = CreateResult(() =>
                {
                    action(null);
                });

                subResult.OnCompleted(() =>
                {
                    if (HasResult)
                    {
                        task.Result = Result;
                    }
                    ret.SetCompleted();
                });

                subResult.OnFailed(error =>
                {
                    ret.SetFailed(error);
                });

                subResult.Start();
            });

            ThreadPoolResult.OnFailed(error =>
            {
                var subResult = CreateResult(() =>
                {
                    action(error);
                });

                subResult.OnCompleted(() =>
                {
                    ret.SetFailed(error);
                });

                subResult.OnFailed(nested =>
                {
                    ret.SetFailed(ProcessException(ExceptionUtility.AggregateExceptions("finally handler failed", nested, error)));
                });

                subResult.Start();
            });

            return new BoundTaskResult(this, task);
        }

        public ITaskResult Then(Action action)
        {
            return Then(() =>
            {
                action();
                return (ITaskResult)null;
            });
        }

        public ITaskResult Then(Func<ITaskResult> action)
        {
            var ret = new ManualThreadPoolResult();
            ret.Start();
            var task = CreateTask(ret);

            ThreadPoolResult.OnCompleted(() =>
            {
                var subResult = CreateResult(() =>
                {
                    var next = action();

                    if (next != null)
                    {
                        next.Then(x =>
                        {
                            if (x.HasResult)
                            {
                                task.Result = x.Result;
                            }
                            ret.SetCompleted();
                        }).Finally(error =>
                        {
                            ret.SetFailed(error);
                        }).Start();
                    }
                    else
                    {
                        if (HasResult)
                        {
                            task.Result = Result;
                        }
                        ret.SetCompleted();
                    }
                });

                subResult.OnFailed(error =>
                {
                    ret.SetFailed(error);
                });

                subResult.Start();
            });

            ThreadPoolResult.OnFailed(error =>
            {
                ret.SetFailed(error);
            });

            return new BoundTaskResult(this, task);
        }

        public abstract ITaskResult WithPriority(int priority);
        public void Start()
        {
            ThreadPoolResult.Start();
        }

        public bool Cancel()
        {
            return ThreadPoolResult.Cancel();
        }

        public ITaskResult Then(Action<NullableResult<object>> action)
        {
            return Then(() =>
            {
                if (HasResult)
                {
                    action(new NullableResult<object>(Result));
                }
                else
                {
                    action(new NullableResult<object>());
                }
            });
        }

        public ITaskResult Then(Func<NullableResult<object>, ITaskResult> action)
        {
            return Then(() =>
            {
                if (HasResult)
                {
                    return action(new NullableResult<object>(Result));
                }
                else
                {
                    return action(new NullableResult<object>());
                }
            });
        }

        public ITaskResult Catch(Action action)
        {
            return Catch(error =>
            {
                action();
            });
        }

        public ITaskResult Then(Func<object> action)
        {
            return Then(() =>
            {
                Result = action();
            });
        }

        public ITaskResult Then(Func<NullableResult<object>, object> action)
        {
            return Then(x =>
            {
                Result = action(x);
            });
        }

        private Exception ProcessException(AggregateException exception)
        {
            var newInners = exception.InnerExceptions.Where(x => !(x is FailureProcessedException));

            if (newInners.Count() == 1)
            {
                return newInners.First();
            }

            return new AggregateException(exception.Message, newInners);
        }
    }
}
