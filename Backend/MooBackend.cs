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
        private bool _disposed;

        public MooBackend(int minThreads, int maxThreads)
        {
            if (minThreads <= 0)
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

                    if (freeThreads.Count >= ((_runners.Count - _minThreads) / 2))
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

        private MooThreadPoolTask CreateTask(Action action)
        {
            return new MooThreadPoolTask(() =>
            {
                action();
                ContractThreadsIfNeeded();
            });
        }

        private bool TryImmediately(MooThreadPoolTask task)
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
            var task = CreateTask(action);

            lock (_syncRoot)
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("MooThreadPool");
                }

                bool shouldExpand = _runners.Count < _maxThreads && _runners.All(x => x.IsBusy);
                _context.Queue.Add(task, priority);

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
            lock (_syncRoot)
            {
                if (!_disposed)
                {
                    _disposed = true;

                    while (_runners.Any(x => x.IsBusy))
                    {
                        Thread.Sleep(1);
                    }

                    foreach (var runner in _runners)
                    {
                        runner.Dispose();
                    }
                }
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
    }
}
