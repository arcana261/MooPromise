using MooPromise.Backend;
using MooPromise.ThreadPool.Moo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.ThreadPool
{
    internal class ThreadPool : IThreadPool
    {
        private IBackend _threadPool;
        private bool _disposed;

        public ThreadPool(IBackend threadPool)
        {
            this._threadPool = threadPool;
            this._disposed = false;
        }

        public IThreadPoolResult Create(Action action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("AsyncThreadPool");
            }

            return new BackendResult(_threadPool, action);
        }

        public IThreadPoolResult Create(Action action, int priority)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("AsyncThreadPool");
            }

            return new BackendResultWithPriority(_threadPool, action, priority);
        }

        public IThreadPoolResult CreateImmediately(Action action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("AsyncThreadPool");
            }

            return new ImmediateBackendResult(_threadPool, action);
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
                _disposed = true;

                if (disposing)
                {
                    if (_threadPool != null)
                    {
                        _threadPool.Dispose();
                    }
                }
            }
        }

        public IThreadPoolResult Begin(Action action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("AsyncThreadPool");
            }

            var result = Create(action);
            result.Start();
            return result;
        }

        public IThreadPoolResult Begin(Action action, int priority)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("AsyncThreadPool");
            }

            var result = Create(action, priority);
            result.Start();
            return result;
        }

        public IThreadPoolResult BeginImmediately(Action action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("AsyncThreadPool");
            }

            var result = CreateImmediately(action);
            result.Start();
            return result;
        }

        public IThreadPoolResult CreateFuture(int dueTickCount, Action action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("AsyncThreadPool");
            }

            return new FutureBackendResult(_threadPool, dueTickCount, action);
        }

        public IThreadPoolResult CreateFuture(int dueTickCount, Action action, int priority)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("AsyncThreadPool");
            }

            return new FutureBackendResultWithPriority(_threadPool, dueTickCount, action, priority);
        }

        public IThreadPoolResult BeginFuture(int dueTickCount, Action action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("AsyncThreadPool");
            }

            var result = CreateFuture(dueTickCount, action);
            result.Start();
            return result;
        }

        public IThreadPoolResult BeginFuture(int dueTickCount, Action action, int priority)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("AsyncThreadPool");
            }

            var result = CreateFuture(dueTickCount, action, priority);
            result.Start();
            return result;
        }

        public IBackend Backend
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("AsyncThreadPool");
                }

                return _threadPool;
            }
        }
    }
}
