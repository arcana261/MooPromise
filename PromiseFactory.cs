using MooPromise.Backend;
using MooPromise.PromiseImpl;
using MooPromise.TaskRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise
{
    public class PromiseFactory : IDisposable
    {
        private ITaskFactory _taskFactory;
        private bool _disposed;

        public PromiseFactory(IBackend backend)
        {
            _taskFactory = new TaskFactory(backend);
        }

        private static IBackend CreateBackend(PromiseBackend backend)
        {
            switch (backend)
            {
                case PromiseBackend.Default:
                case PromiseBackend.DotNet:
                    return new TplBackend();
                case PromiseBackend.MooThreadPool:
                    return new MooBackend();
                default:
                    throw new InvalidProgramException("Unknown backend type: " + backend.ToString());
            }
        }

        public PromiseFactory(PromiseBackend backend)
            : this(CreateBackend(backend))
        {

        }

        public PromiseFactory(int minThreads, int maxThreads)
            : this(new MooBackend(minThreads, maxThreads))
        {

        }

        public PromiseFactory()
            : this(PromiseBackend.Default)
        {

        }
        
        ~PromiseFactory()
        {
            Dispose(false);
        }

        public IBackend Backend
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("PromiseFactory");
                }

                return _taskFactory.ThreadPool.Backend;
            }
        }

        public IPromise Create(Action action)
        {
            return Create(action, PromisePriority.Normal);
        }

        public IPromise Create(Action action, PromisePriority priority)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("PromiseFactory");
            }

            switch (priority)
            {
                case PromisePriority.Normal:
                    return new NormalPromise(_taskFactory, _taskFactory.Create(action));
                case PromisePriority.Immediate:
                    return new NormalPromise(_taskFactory, _taskFactory.CreateImmediately(action));
                default:
                    return new NormalPromise(_taskFactory, _taskFactory.Create(action, (int)priority));
            }
        }

        public IPromise StartNew(Action action)
        {
            var ret = Create(action);
            ret.Start();
            return ret;
        }

        public IPromise StartNew(Action action, PromisePriority priority)
        {
            var ret = Create(action, priority);
            ret.Start();
            return ret;
        }

        public IPromise<T> Create<T>(Func<T> action, PromisePriority priority)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("PromiseFactory");
            }

            switch (priority)
            {
                case PromisePriority.Normal:
                    return new NormalPromise<T>(_taskFactory, _taskFactory.Create(() => (object)action()));
                case PromisePriority.Immediate:
                    return new NormalPromise<T>(_taskFactory, _taskFactory.CreateImmediately(() => (object)action()));
                default:
                    return new NormalPromise<T>(_taskFactory, _taskFactory.Create(() => (object)action(), (int)priority));
            }
        }

        public IPromise<T> Create<T>(Func<T> action)
        {
            return Create(action, PromisePriority.Normal);
        }

        public IPromise<T> StartNew<T>(Func<T> action)
        {
            var ret = Create(action);
            ret.Start();
            return ret;
        }

        public IPromise<T> StartNew<T>(Func<T> action, PromisePriority priority)
        {
            var ret = Create(action, priority);
            ret.Start();
            return ret;
        }

        public IPromise Create(Func<IPromise> action, PromisePriority priority)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("PromiseFactory");
            }

            switch (priority)
            {
                case PromisePriority.Normal:
                    return new NormalPromise(_taskFactory, _taskFactory.Create(() => PromiseHelpers.ConvertPromiseToTaskResult(_taskFactory, action())));
                case PromisePriority.Immediate:
                    return new NormalPromise(_taskFactory, _taskFactory.CreateImmediately(() => PromiseHelpers.ConvertPromiseToTaskResult(_taskFactory, action())));
                default:
                    return new NormalPromise(_taskFactory, _taskFactory.Create(() => PromiseHelpers.ConvertPromiseToTaskResult(_taskFactory, action()), (int)priority));
            }
        }

        public IPromise Create(Func<IPromise> action)
        {
            return Create(action, PromisePriority.Normal);
        }

        public IPromise StartNew(Func<IPromise> action, PromisePriority priority)
        {
            var ret = Create(action, priority);
            ret.Start();
            return ret;
        }

        public IPromise<T> Create<T>(Func<IPromise<T>> action, PromisePriority priority)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("PromiseFactory");
            }

            switch (priority)
            {
                case PromisePriority.Normal:
                    return new NormalPromise<T>(_taskFactory, _taskFactory.Create(() => PromiseHelpers.ConvertPromiseToTaskResult<T>(_taskFactory, action())));
                case PromisePriority.Immediate:
                    return new NormalPromise<T>(_taskFactory, _taskFactory.CreateImmediately(() => PromiseHelpers.ConvertPromiseToTaskResult<T>(_taskFactory, action())));
                default:
                    return new NormalPromise<T>(_taskFactory, _taskFactory.Create(() => PromiseHelpers.ConvertPromiseToTaskResult<T>(_taskFactory, action()), (int)priority));
            }
        }

        public IPromise<T> Create<T>(Func<IPromise<T>> action)
        {
            return Create(action, PromisePriority.Normal);
        }

        public IPromise<T> StartNew<T>(Func<IPromise<T>> action, PromisePriority priority)
        {
            var ret = Create(action, priority);
            ret.Start();
            return ret;
        }

        public IPromise<T> StartNew<T>(Func<IPromise<T>> action)
        {
            var ret = Create(action);
            ret.Start();
            return ret;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            lock (this)
            {
                if (!_disposed)
                {
                    _disposed = true;
                    _taskFactory.Dispose();
                }
            }
        }
    }
}
