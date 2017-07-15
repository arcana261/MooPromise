using MooPromise.Backend;
using MooPromise.TaskRunner.Moo;
using MooPromise.ThreadPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.TaskRunner
{
    internal class TaskFactory : ITaskFactory
    {
        private IThreadPool _threadPool;
        private volatile bool _disposed;

        public TaskFactory(IThreadPool threadPool)
        {
            this._threadPool = threadPool;
            this._disposed = false;
        }

        public TaskFactory(IBackend backend)
            : this (new MooPromise.ThreadPool.ThreadPool(backend))
        {

        }

        ~TaskFactory()
        {
            Dispose(false);
        }

        public ITaskResult Begin(Action action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("TaskFactory");
            }

            var result = Create(action);
            result.Start();
            return result;
        }

        public ITaskResult Begin(Action action, int priority)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("TaskFactory");
            }

            var result = Create(action, priority);
            result.Start();
            return result;
        }

        public ITaskResult BeginImmediately(Action action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("TaskFactory");
            }

            var result = CreateImmediately(action);
            result.Start();
            return result;
        }

        public ITaskResult Create(Action action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("TaskFactory");
            }

            return new TaskResult(_threadPool, _threadPool.Create(action));
        }

        public ITaskResult Create(Action action, int priority)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("TaskFactory");
            }

            return new TaskResult(_threadPool, _threadPool.Create(action, priority));
        }

        public ITaskResult CreateImmediately(Action action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("TaskFactory");
            }

            return new TaskResult(_threadPool, _threadPool.CreateImmediately(action));
        }

        public ITaskResult Create(Func<ITaskResult> action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("TaskFactory");
            }

            ManualTaskResult ret = new ManualTaskResult(_threadPool);
            ret.Start();

            var task = Create(() =>
            {
                var next = action();

                if (next != null)
                {
                    next.Then(x =>
                    {
                        if (x.HasResult)
                        {
                            ret.SetResult(x.Result);
                        }
                        ret.SetCompleted();
                    }).Catch(error =>
                    {
                        ret.SetFailed(error);
                    });
                }
                else
                {
                    ret.SetCompleted();
                }
            }).Catch(error =>
            {
                ret.SetFailed(error);
            });

            return new BoundTaskResult(task, ret);
        }

        public ITaskResult Create(Func<ITaskResult> action, int priority)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("TaskFactory");
            }

            ManualTaskResult ret = new ManualTaskResult(_threadPool);
            ret.Start();

            var task = Create(() =>
            {
                var next = action();

                if (next != null)
                {
                    next.Then(x =>
                    {
                        if (x.HasResult)
                        {
                            ret.SetResult(x.Result);
                        }

                        ret.SetCompleted();
                    }).Catch(error =>
                    {
                        ret.SetFailed(error);
                    });
                }
                else
                {
                    ret.SetCompleted();
                }
            }, priority).Catch(error =>
            {
                ret.SetFailed(error);
            });

            return new BoundTaskResult(task, ret);
        }

        public ITaskResult CreateImmediately(Func<ITaskResult> action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("TaskFactory");
            }

            ManualTaskResult ret = new ManualTaskResult(_threadPool);
            ret.Start();

            var task = CreateImmediately(() =>
            {
                var next = action();

                if (next != null)
                {
                    next.Then(x =>
                    {
                        if (x.HasResult)
                        {
                            ret.SetResult(x.Result);
                        }

                        ret.SetCompleted();
                    }).Catch(error =>
                    {
                        ret.SetFailed(error);
                    });
                }
                else
                {
                    ret.SetCompleted();
                }
            }).Catch(error =>
            {
                ret.SetFailed(error);
            });

            return new BoundTaskResult(task, ret);
        }

        public ITaskResult Begin(Func<ITaskResult> action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("TaskFactory");
            }

            var ret = Create(action);
            ret.Start();
            return ret;
        }

        public ITaskResult Begin(Func<ITaskResult> action, int priority)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("TaskFactory");
            }

            var ret = Create(action, priority);
            ret.Start();
            return ret;
        }

        public ITaskResult BeginImmediately(Func<ITaskResult> action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("TaskFactory");
            }

            var ret = CreateImmediately(action);
            ret.Start();
            return ret;
        }

        public IThreadPool ThreadPool
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("TaskFactory");
                }

                return _threadPool;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _threadPool.Dispose();
                }

                _disposed = true;
            }
        }

        public ITaskResult Create(Func<object> action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("TaskFactory");
            }

            TaskResult ret = null;

            ret = new TaskResult(_threadPool, _threadPool.Create(() =>
            {
                ret.SetResult(action());
            }));

            return ret;
        }

        public ITaskResult Create(Func<object> action, int priority)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("TaskFactory");
            }

            TaskResult ret = null;

            ret = new TaskResult(_threadPool, _threadPool.Create(() =>
            {
                ret.SetResult(action());
            }, priority));

            return ret;
        }

        public ITaskResult CreateImmediately(Func<object> action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("TaskFactory");
            }

            TaskResult ret = null;

            ret = new TaskResult(_threadPool, _threadPool.CreateImmediately(() =>
            {
                ret.SetResult(action());
            }));

            return ret;
        }

        public bool IsDisposed
        {
            get
            {
                return _disposed;
            }
        }
    }
}
