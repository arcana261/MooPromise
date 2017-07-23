using MooPromise.Backend.Moo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MooPromise.Backend
{
    internal class MooBackend : IBackend
    {
        private int _minThreads;
        private int _maxThreads;
        private object _syncRoot;
        private IList<MooBackendRunner> _runners;
        private MooBackendContext _context;
        private volatile bool _disposed;

        public MooBackend(int minThreads, int maxThreads)
        {
            if (minThreads < 0)
            {
                throw new ArgumentException("minThreads");
            }

            if (maxThreads < minThreads)
            {
                throw new ArgumentException("maxThreads");
            }

            this._minThreads = minThreads;
            this._maxThreads = maxThreads;
            this._syncRoot = new object();
            this._runners = new List<MooBackendRunner>();
            this._context = new MooBackendContext();
            this._disposed = false;
        }

        public MooBackend(int maxThreads)
            : this(0, maxThreads)
        {

        }

        public MooBackend()
            : this(Environment.ProcessorCount / 2, Environment.ProcessorCount * 2)
        {

        }

        ~MooBackend()
        {
            Dispose(false);
        }

#if DEBUG
        public IEnumerable<int> ManagedThreadIds
        {
            get
            {
                lock (_syncRoot)
                {
                    return _runners.Select(x => x.ManagedThreadId).ToList();
                }
            }
        }
#endif

        private void ContractThreadsIfNeeded()
        {
            IList<MooBackendRunner> freeThreads = null;

            lock (_syncRoot)
            {
                if (_disposed)
                {
                    return;
                }

                if (_runners.Count > _minThreads)
                {
                    freeThreads = _runners.Where(x => !x.IsBusy).Take(_runners.Count - _minThreads).ToList();

                    if (freeThreads.Count >= ((_runners.Count - _minThreads) / 2) && freeThreads.Count >= 4)
                    {
                        foreach (var thread in freeThreads)
                        {
                            if (!_runners.Remove(thread))
                            {
                                throw new InvalidProgramException("could not safely dispose un-needed threadpool threads");
                            }
                        }
                    }
                    else
                    {
                        freeThreads = null;
                    }
                }
            }

            if (freeThreads != null)
            {
                Add(() =>
                {
                    try
                    {
                        foreach (var thread in freeThreads)
                        {
                            thread.Dispose();
                        }
                    }
                    catch (Exception error)
                    {
                        Environment.FailFast("could not dispose un-needed threadpool threads", error);
                    }
                }, int.MinValue);
            }
        }

        private MooBackendTask CreateTask(Action action)
        {
            return new MooBackendTask(() =>
            {
                action();
                ContractThreadsIfNeeded();
            });
        }

        private MooBackendTask CreateFutureTask(int dueTickCount, Action action)
        {
            return new MooBackendFutureTask(dueTickCount, () =>
            {
                action();
                ContractThreadsIfNeeded();
            });
        }

        private bool TryImmediately(MooBackendTask task)
        {
            bool canRun = false;

            lock (_syncRoot)
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("MooThreadPool");
                }

                canRun = _runners.Any(x => x.IsInsideRunnerThread());
            }

            if (canRun)
            {
                try
                {
                    task.Action();
                }
                catch (Exception error)
                {
                    Environment.FailFast("Unhandled exception while triggering backend task", error);
                }

                return true;
            }

            return false;
        }

        public void Add(Action action)
        {
            Add(action, 0);
        }

        public void Add(Action action, int priority)
        {
            Add(CreateTask(action), priority);
        }

        private void Add(MooBackendTask task, int priority)
        {
            lock (_syncRoot)
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("MooThreadPool");
                }

                bool shouldExpand = _runners.Count < _maxThreads && _runners.All(x => x.IsBusy);
                _context.Queue.Add(task, priority);
                _context.TaskAddedSignal.Set();

                if (shouldExpand)
                {
                    var newRunner = new MooBackendRunner(_context);
                    newRunner.Start();
                    _runners.Add(newRunner);
                }
            }
        }

        public void AddImmediately(Action action)
        {
            if (!TryImmediately(CreateTask(action)))
            {
                Add(action, int.MaxValue);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (_syncRoot)
                {
                    foreach (var runner in _runners)
                    {
                        runner.Dispose();
                    }

                    _disposed = true;
                }
            }
            else
            {
                _disposed = true;
            }
        }

        public bool IsCurrentThreadManagedByBackend()
        {
            lock (_syncRoot)
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("MooThreadPool");
                }

                return _runners.Any(x => x.IsInsideRunnerThread());
            }
        }

        public void AddFuture(int dueTickTime, Action action)
        {
            AddFuture(dueTickTime, action, 0);
        }

        public void AddFuture(int dueTickTime, Action action, int priority)
        {
            Add(CreateFutureTask(dueTickTime, action), priority);
        }

        public void WaitUntilDisposed()
        {
            while (!_disposed)
            {
                Thread.Sleep(1);
            }

            foreach (var runner in _runners)
            {
                runner.WaitUntilDisposed();
            }
        }

        public bool WaitUntilDisposed(int waitMs)
        {
            if (waitMs < 0)
            {
                WaitUntilDisposed();
                return true;
            }

            while (!_disposed && waitMs > 0)
            {
                Thread.Sleep(1);
                waitMs--;
            }

            if (waitMs < 1)
            {
                return false;
            }

            foreach (var runner in _runners)
            {
                if (waitMs < 1)
                {
                    return false;
                }

                int start = Environment.TickCount;
                if (!runner.WaitUntilDisposed(waitMs))
                {
                    return false;
                }

                waitMs = waitMs - (Environment.TickCount - start);
            }

            return true;
        }
    }
}
