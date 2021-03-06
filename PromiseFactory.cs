﻿using MooPromise.Backend;
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
        private IBackend _backend;
        private bool _disposed;
        private Control.Control _controlObject;
        private Async.Async _asyncObject;

        public PromiseFactory(IBackend backend)
        {
            if (backend == null)
            {
                throw new ArgumentNullException("backend");
            }

            _backend = backend;
            _taskFactory = new TaskFactory(backend);
            _controlObject = null;
        }

        public Control.Control Control
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("PromiseFactory");
                }

                lock (this)
                {
                    if (_controlObject == null)
                    {
                        _controlObject = new Control.Control(this);
                    }

                    return _controlObject;
                }
            }
        }

        public Async.Async Async
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("PromiseFactory");
                }

                lock (this)
                {
                    if (_asyncObject == null)
                    {
                        _asyncObject = new Async.Async(this);
                    }

                    return _asyncObject;
                }
            }
        }

        public bool IsDisposed
        {
            get
            {
                return _disposed;
            }
        }

        public void WaitUntilDisposed()
        {
            _backend.WaitUntilDisposed();
        }

        public bool WaitUntilDisposed(int waitMs)
        {
            return _backend.WaitUntilDisposed(waitMs);
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

        public IManualPromise CreateManual()
        {
            return new ManualPromise(this, _taskFactory);
        }

        public IManualPromise<T> CreateManual<T>()
        {
            return new ManualPromise<T>(this, _taskFactory);
        }

        public IPromise Create(Action action)
        {
            return Create(action, PromisePriority.Normal);
        }

        public IPromise SetTimeout(int delay, Action action, PromisePriority priority)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("PromiseFactory");
            }

            switch (priority)
            {
                case PromisePriority.Normal:
                    return new NormalPromise(this, _taskFactory, _taskFactory.BeginFuture(Environment.TickCount + delay, action));
                default:
                    return new NormalPromise(this, _taskFactory, _taskFactory.BeginFuture(Environment.TickCount + delay, action, (int)priority));
            }
        }

        public IPromise<T> SetTimeout<T>(int delay, Func<T> action, PromisePriority priority)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("PromiseFactory");
            }

            switch (priority)
            {
                case PromisePriority.Normal:
                    return new NormalPromise<T>(this, _taskFactory, _taskFactory.BeginFuture(Environment.TickCount + delay, () => (object)action()));
                default:
                    return new NormalPromise<T>(this, _taskFactory, _taskFactory.BeginFuture(Environment.TickCount + delay, () => (object)action(), (int)priority));
            }
        }

        public IPromise SetTimeout(int delay, Func<IPromise> action, PromisePriority priority)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("PromiseFactory");
            }

            switch (priority)
            {
                case PromisePriority.Normal:
                    return new NormalPromise(this, _taskFactory, _taskFactory.BeginFuture(Environment.TickCount + delay, () => PromiseHelpers.ConvertPromiseToTaskResult(_taskFactory, action())));
                default:
                    return new NormalPromise(this, _taskFactory, _taskFactory.BeginFuture(Environment.TickCount + delay, () => PromiseHelpers.ConvertPromiseToTaskResult(_taskFactory, action()), (int)priority));
            }
        }

        public IPromise<T> SetTimeout<T>(int delay, Func<IPromise<T>> action, PromisePriority priority)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("PromiseFactory");
            }

            switch (priority)
            {
                case PromisePriority.Normal:
                    return new NormalPromise<T>(this, _taskFactory, _taskFactory.BeginFuture(Environment.TickCount + delay, () => PromiseHelpers.ConvertPromiseToTaskResult(_taskFactory, action())));
                default:
                    return new NormalPromise<T>(this, _taskFactory, _taskFactory.BeginFuture(Environment.TickCount + delay, () => PromiseHelpers.ConvertPromiseToTaskResult(_taskFactory, action()), (int)priority));
            }
        }

        public IPromise SetTimeout(Action action, int delay, PromisePriority priority)
        {
            return SetTimeout(delay, action, priority);
        }

        public IPromise<T> SetTimeout<T>(Func<T> action, int delay, PromisePriority priority)
        {
            return SetTimeout(delay, action, priority);
        }

        public IPromise SetTimeout(Func<IPromise> action, int delay, PromisePriority priority)
        {
            return SetTimeout(delay, action, priority);
        }

        public IPromise<T> SetTimeout<T>(Func<IPromise<T>> action, int delay, PromisePriority priority)
        {
            return SetTimeout(delay, action, priority);
        }

        public IPromise SetTimeout(int delay, Action action)
        {
            return SetTimeout(delay, action, PromisePriority.Normal);
        }

        public IPromise<T> SetTimeout<T>(int delay, Func<T> action)
        {
            return SetTimeout(delay, action, PromisePriority.Normal);
        }

        public IPromise SetTimeout(int delay, Func<IPromise> action)
        {
            return SetTimeout(delay, action, PromisePriority.Normal);
        }

        public IPromise<T> SetTimeout<T>(int delay, Func<IPromise<T>> action)
        {
            return SetTimeout(delay, action, PromisePriority.Normal);
        }

        public IPromise SetTimeout(Action action, int delay)
        {
            return SetTimeout(delay, action);
        }

        public IPromise<T> SetTimeout<T>(Func<T> action, int delay)
        {
            return SetTimeout(delay, action);
        }

        public IPromise SetTimeout(Func<IPromise> action, int delay)
        {
            return SetTimeout(delay, action);
        }

        public IPromise<T> SetTimeout<T>(Func<IPromise<T>> action, int delay)
        {
            return SetTimeout(delay, action);
        }

        public IPromiseInterval SetInterval(Action action, int delay, PromisePriority priority)
        {
            return new IntervalHandleImpl(this, delay, action, priority);
        }

        public IPromiseInterval SetInterval(Func<IPromise> action, int delay, PromisePriority priority)
        {
            return new IntervalHandleImpl(this, delay, action, priority);
        }

        public IPromiseInterval<T> SetInterval<T>(Func<T> action, int delay, PromisePriority priority)
        {
            return new IntervalHandleImpl<T>(this, delay, action, priority);
        }

        public IPromiseInterval<T> SetInterval<T>(Func<IPromise<T>> action, int delay, PromisePriority priority)
        {
            return new IntervalHandleImpl<T>(this, delay, action, priority);
        }

        public IPromiseInterval SetInterval(int delay, Action action, PromisePriority priority)
        {
            return SetInterval(action, delay, priority);
        }

        public IPromiseInterval SetInterval(int delay, Func<IPromise> action, PromisePriority priority)
        {
            return SetInterval(action, delay, priority);
        }

        public IPromiseInterval<T> SetInterval<T>(int delay, Func<T> action, PromisePriority priority)
        {
            return SetInterval(action, delay, priority);
        }

        public IPromiseInterval<T> SetInterval<T>(int delay, Func<IPromise<T>> action, PromisePriority priority)
        {
            return SetInterval(action, delay, priority);
        }

        public IPromiseInterval SetInterval(Action action, int delay)
        {
            return SetInterval(action, delay, PromisePriority.Normal);
        }

        public IPromiseInterval SetInterval(Func<IPromise> action, int delay)
        {
            return SetInterval(action, delay, PromisePriority.Normal);
        }

        public IPromiseInterval<T> SetInterval<T>(Func<T> action, int delay)
        {
            return SetInterval(action, delay, PromisePriority.Normal);
        }

        public IPromiseInterval<T> SetInterval<T>(Func<IPromise<T>> action, int delay)
        {
            return SetInterval(action, delay, PromisePriority.Normal);
        }

        public IPromiseInterval SetInterval(int delay, Action action)
        {
            return SetInterval(action, delay);
        }

        public IPromiseInterval SetInterval(int delay, Func<IPromise> action)
        {
            return SetInterval(action, delay);
        }

        public IPromiseInterval<T> SetInterval<T>(int delay, Func<T> action)
        {
            return SetInterval(action, delay);
        }

        public IPromiseInterval<T> SetInterval<T>(int delay, Func<IPromise<T>> action)
        {
            return SetInterval(action, delay);
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
                    return new NormalPromise(this, _taskFactory, _taskFactory.Create(action));
                case PromisePriority.Immediate:
                    return new NormalPromise(this, _taskFactory, _taskFactory.CreateImmediately(action));
                default:
                    return new NormalPromise(this, _taskFactory, _taskFactory.Create(action, (int)priority));
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
                    return new NormalPromise<T>(this, _taskFactory, _taskFactory.Create(() => (object)action()));
                case PromisePriority.Immediate:
                    return new NormalPromise<T>(this, _taskFactory, _taskFactory.CreateImmediately(() => (object)action()));
                default:
                    return new NormalPromise<T>(this, _taskFactory, _taskFactory.Create(() => (object)action(), (int)priority));
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
                    return new NormalPromise(this, _taskFactory, _taskFactory.Create(() => PromiseHelpers.ConvertPromiseToTaskResult(_taskFactory, action())));
                case PromisePriority.Immediate:
                    return new NormalPromise(this, _taskFactory, _taskFactory.CreateImmediately(() => PromiseHelpers.ConvertPromiseToTaskResult(_taskFactory, action())));
                default:
                    return new NormalPromise(this, _taskFactory, _taskFactory.Create(() => PromiseHelpers.ConvertPromiseToTaskResult(_taskFactory, action()), (int)priority));
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
                    return new NormalPromise<T>(this, _taskFactory, _taskFactory.Create(() => PromiseHelpers.ConvertPromiseToTaskResult<T>(_taskFactory, action())));
                case PromisePriority.Immediate:
                    return new NormalPromise<T>(this, _taskFactory, _taskFactory.CreateImmediately(() => PromiseHelpers.ConvertPromiseToTaskResult<T>(_taskFactory, action())));
                default:
                    return new NormalPromise<T>(this, _taskFactory, _taskFactory.Create(() => PromiseHelpers.ConvertPromiseToTaskResult<T>(_taskFactory, action()), (int)priority));
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
            var ret = new ManualPromise(this, _taskFactory);
            ret.Start();
            ret.SetCompleted();

            return ret.Immediately;
        }

        public IPromise<T> StartNew<T>(T value)
        {
            var ret = new ManualPromise<T>(this, _taskFactory);
            ret.Start();
            ret.SetResult(value);
            ret.SetCompleted();

            return ret.Immediately;
        }

        public IPromise<T> Create<T>(T value)
        {
            return StartNew(value);
        }

        public IPromise Value()
        {
            return StartNew();
        }

        public IPromise<T> Value<T>(T value)
        {
            return StartNew(value);
        }

        public IPromise StartFailed(Exception error)
        {
            var ret = new ManualPromise(this, _taskFactory);
            ret.Start();
            ret.SetFailed(error);

            return ret;
        }

        public IPromise<T> StartFailed<T>(Exception error)
        {
            var ret = new ManualPromise<T>(this, _taskFactory);
            ret.Start();
            ret.SetFailed(error);

            return ret;
        }

        public IPromise Fail(Exception error)
        {
            return StartFailed(error);
        }

        public IPromise<T> Fail<T>(Exception error)
        {
            return StartFailed<T>(error);
        }

        public IPromise Sleep(int ms)
        {
            var ret = new ManualPromise(this, _taskFactory);

            SetTimeout(() =>
            {
                ret.Resolve();
            }, ms).Catch(err =>
            {
                ret.Reject(err);
            });

            return ret;
        }

        public IPromise JoinParallel(IEnumerable<IPromise> promisesAsEum)
        {
            var promises = promisesAsEum.ToList();
            var result = new ManualPromise(this, _taskFactory);
            result.Start();

            foreach (var promise in promises)
            {
                promise.Then(() =>
                {
#if DEBUG
                    var states = promises.Select(x => x.State).ToList();
#endif

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

        public IPromise JoinParallel<T>(IEnumerable<IPromise<T>> promises)
        {
            return JoinParallel(promises.Select(x => x.Cast()));
        }

        private IPromise JoinSerial(IEnumerator<IPromise> promises)
        {
            if (promises.MoveNext())
            {
                return promises.Current.Then(() => JoinSerial(promises));
            }
            else
            {
                return StartNew();
            }
        }

        public IPromise JoinSerial(IEnumerable<IPromise> promises)
        {
            return JoinSerial(promises.GetEnumerator());
        }

        public IPromise JoinSerial<T>(IEnumerable<IPromise<T>> promises)
        {
            return JoinSerial(promises.Select(x => x.Cast()));
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<IPromise<T>> items, Func<E, T, int, IPromise<E>> action, E seed)
        {
            return Aggregate(seed, 0, items.GetEnumerator(), action);
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<IPromise<T>> items, Func<E, T, int, IPromise<E>> action)
        {
            return Aggregate(items, action, default(E));
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<IPromise<T>> items, Func<E, T, int, E> action, E seed)
        {
            return Aggregate(items, (x, y, i) => StartNew(action(x, y, i)), seed);
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<IPromise<T>> items, Func<E, T, int, E> action)
        {
            return Aggregate(items, action, default(E));
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<T> items, Func<E, T, int, IPromise<E>> action, E seed)
        {
            return Aggregate(items.Select(x => StartNew(x)), action, seed);
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<T> items, Func<E, T, int, IPromise<E>> action)
        {
            return Aggregate(items, action, default(E));
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<T> items, Func<E, T, int, E> action, E seed)
        {
            return Aggregate(items, (x, y, i) => StartNew(action(x, y, i)), seed);
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<T> items, Func<E, T, int, E> action)
        {
            return Aggregate(items, action, default(E));
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<IPromise<T>> items, Func<E, T, IPromise<E>> action, E seed)
        {
            return Aggregate(items, (x, y, i) => action(x, y), seed);
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<IPromise<T>> items, Func<E, T, IPromise<E>> action)
        {
            return Aggregate(items, action, default(E));
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<IPromise<T>> items, Func<E, T, E> action, E seed)
        {
            return Aggregate(items, (x, y, i) => action(x, y), seed);
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<IPromise<T>> items, Func<E, T, E> action)
        {
            return Aggregate(items, action, default(E));
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<T> items, Func<E, T, IPromise<E>> action, E seed)
        {
            return Aggregate(items, (x, y, i) => action(x, y), seed);
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<T> items, Func<E, T, IPromise<E>> action)
        {
            return Aggregate(items, action, default(E));
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<T> items, Func<E, T, E> action, E seed)
        {
            return Aggregate(items, (x, y, i) => action(x, y), seed);
        }

        public IPromise<E> Aggregate<T, E>(IEnumerable<T> items, Func<E, T, E> action)
        {
            return Aggregate(items, action, default(E));
        }

        private IPromise<E> Aggregate<T, E>(E prev, int i, IEnumerator<IPromise<T>> items, Func<E, T, int, IPromise<E>> action)
        {
            if (items.MoveNext())
            {
                return items.Current.Then(x => action(prev, x, i)).Then(next => Aggregate(next, i + 1, items, action));
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
            if (!_disposed)
            {
                _disposed = true;

                if (disposing)
                {
                    if (_taskFactory != null)
                    {
                        _taskFactory.Dispose();
                    }
                }
            }
        }
    }
}
