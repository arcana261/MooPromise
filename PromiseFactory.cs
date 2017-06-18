using MooPromise.Backend;
using MooPromise.PromiseImpl;
using MooPromise.TaskRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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

        internal static PromiseBackend DefaultBackend
        {
            get
            {
                return PromiseBackend.DotNet;
            }
        }

        private static IBackend CreateBackend(PromiseBackend backend)
        {
            switch (backend)
            {
                case PromiseBackend.Default:
                    return CreateBackend(DefaultBackend);
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

        public PromiseFactory(SynchronizationContext context)
            : this (new SynchronizationContextBackend(context))
        {

        }

        public PromiseFactory(System.Windows.Threading.Dispatcher dispatcher)
            : this(new WpfDispatcherBackend(dispatcher))
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

#if DEBUG
        public IEnumerable<int> ManagedThreadIds
        {
            get
            {
                return Backend.ManagedThreadIds;
            }
        }
#endif

        public bool IsCurrentThreadManagedByBackend()
        {
            return Backend.IsCurrentThreadManagedByBackend();
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

        public IPromise StartNew(Func<IPromise> action)
        {
            var ret = Create(action);
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

        public IPromise Create()
        {
            return StartNew();
        }

        public IPromise StartNew()
        {
            var ret = new ManualPromise(_taskFactory);
            ret.Start();
            ret.SetCompleted();

            return ret;
        }

        public IPromise<T> StartNew<T>(T value)
        {
            var ret = new ManualPromise<T>(_taskFactory);
            ret.Start();
            ret.SetResult(value);
            ret.SetCompleted();

            return ret;
        }

        public IPromise<T> Create<T>(T value)
        {
            return StartNew(value);
        }

        public IPromise StartFailed(Exception error)
        {
            var ret = new ManualPromise(_taskFactory);
            ret.Start();
            ret.SetFailed(error);

            return ret;
        }

        public IPromise<T> StartFailed<T>(Exception error)
        {
            var ret = new ManualPromise<T>(_taskFactory);
            ret.Start();
            ret.SetFailed(error);

            return ret;
        }

        public IPromise Parallelize(IEnumerable<IPromise> promises)
        {
            if (promises == null)
            {
                return StartNew();
            }

            var result = new ManualPromise(_taskFactory);
            result.Start();

            foreach (var promise in promises)
            {
                promise.Then(() =>
                {
                    if (promises.All(x => x.State == AsyncState.Completed))
                    {
                        result.SetCompleted();
                    }
                }).Finally(error =>
                {
                    if (promise.State == AsyncState.Failed)
                    {
                        result.SetFailed(error);
                    }
                });
            }

            return result;
        }

        public IPromise Sleep(int ms)
        {
            return StartNew(() =>
            {
                Thread.Sleep(ms);
            });
        }

        private IPromise Serialize(IEnumerator<IPromise> promises)
        {
            if (promises.MoveNext())
            {
                return promises.Current.Then(() => Serialize(promises));
            }
            else
            {
                return StartNew();
            }
        }

        public IPromise Serialize(IEnumerable<IPromise> promises)
        {
            if (promises == null)
            {
                return StartNew();
            }

            return Serialize(promises.GetEnumerator());
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<IPromise<T>> items, Func<E, T, IPromise<E>> action, E seed)
        {
            return Aggregate(seed, items.GetEnumerator(), action);
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<IPromise<T>> items, Func<E, T, IPromise<E>> action)
        {
            return Aggregate(items, action, default(E));
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<IPromise<T>> items, Func<E, T, E> action, E seed)
        {
            return Aggregate(items, (x, y) => StartNew(action(x, y)), seed);
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<IPromise<T>> items, Func<E, T, E> action)
        {
            return Aggregate(items, action, default(E));
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<T> items, Func<E, T, IPromise<E>> action, E seed)
        {
            return Aggregate(items.Select(x => StartNew(x)), action, seed);
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<T> items, Func<E, T, IPromise<E>> action)
        {
            return Aggregate(items, action, default(E));
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<T> items, Func<E, T, E> action, E seed)
        {
            return Aggregate(items, (x, y) => StartNew(action(x, y)), seed);
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<T> items, Func<E, T, E> action)
        {
            return Aggregate(items, action, default(E));
        }

        private IPromise<E> Aggregate<T, E>(E prev, IEnumerator<IPromise<T>> items, Func<E, T, IPromise<E>> action)
        {
            if (items.MoveNext())
            {
                return items.Current.Then(x => action(prev, x)).Then(next => Aggregate(next, items, action));
            }
            else
            {
                return StartNew(prev);
            }
        }

        private IPromise<IEnumerable<E>> Map<T, E>(IEnumerator<IPromise<T>> items, IList<E> list, Func<T, IPromise<E>> action)
        {
            if (items.MoveNext())
            {
                return items.Current.Then(x => action(x)).Then(y =>
                {
                    list.Add(y);
                    return Map(items, list, action);
                });
            }
            else
            {
                return StartNew((IEnumerable<E>)list);
            }
        }

        public IPromise<IEnumerable<E>> Map<T, E>(IEnumerable<IPromise<T>> items, Func<T, IPromise<E>> action)
        {
            return Map(items.GetEnumerator(), new List<E>(), action);
        }

        public IPromise<IEnumerable<E>> Map<T, E>(IEnumerable<T> items, Func<T, IPromise<E>> action)
        {
            return Map(items.Select(x => StartNew(x)), action);
        }

        public IPromise<IEnumerable<E>> Map<T, E>(IEnumerable<IPromise<T>> items, Func<T, E> action)
        {
            return Map(items, x => StartNew(action(x)));
        }

        public IPromise<IEnumerable<E>> Map<T, E>(IEnumerable<T> items, Func<T, E> action)
        {
            return Map(items, x => StartNew(action(x)));
        }

        private IPromise Each<T>(IEnumerator<T> items, Func<T, IPromise> action)
        {
            if (items.MoveNext())
            {
                return action(items.Current).Then(() => Each(items, action));
            }
            else
            {
                return StartNew();
            }
        }

        public IPromise Each<T>(IEnumerable<T> items, Func<T, IPromise> action)
        {
            if (items == null)
            {
                return StartNew();
            }

            return Each(items.GetEnumerator(), action);
        }

        public IPromise Each<T>(IEnumerable<T> items, Action<T> action)
        {
            if (items == null)
            {
                return StartNew();
            }

            return Each(items, x =>
            {
                action(x);
                return StartNew();
            });
        }

        public IPromise Each<T>(IPromise<IEnumerable<T>> items, Func<T, IPromise> action)
        {
            return items.Then(result => Each(result, action));
        }

        public IPromise Each<T>(IPromise<IEnumerable<T>> items, Action<T> action)
        {
            return items.Then(result => Each(result, action));
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
