using MooPromise.Backend;
using MooPromise.Enumerable;
using MooPromise.PromiseImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MooPromise
{
    public class Promise : IPromise
    {
        private static object _syncRoot = new object();
        private static PromiseFactory _factory;
        private static PromiseBackend _backendType;
        private static IBackend _backend;

        public static PromiseFactory Factory
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_factory == null)
                    {
                        _factory = new PromiseFactory();
                        _backendType = PromiseFactory.DefaultBackend;
                        _backend = null;
                    }

                    return _factory;
                }
            }
        }

        public static void SetDefaultFactory(PromiseBackend backend)
        {
            lock (_syncRoot)
            {
                if (_factory == null)
                {
                    _factory = new PromiseFactory(backend);
                    _backend = null;
                    if (backend == PromiseBackend.Default)
                    {
                        _backendType = PromiseFactory.DefaultBackend;
                    }
                    else
                    {
                        _backendType = backend;
                    }
                }
                else if (_backend != null)
                {
                    _factory.Dispose();
                    _factory = new PromiseFactory(backend);
                    _backend = null;

                    if (backend == PromiseBackend.Default)
                    {
                        _backendType = PromiseFactory.DefaultBackend;
                    }
                    else
                    {
                        _backendType = backend;
                    }
                }
                else if (_backendType != backend)
                {
                    _factory.Dispose();
                    _factory = new PromiseFactory(backend);

                    if (backend == PromiseBackend.Default)
                    {
                        _backendType = PromiseFactory.DefaultBackend;
                    }
                    else
                    {
                        _backendType = backend;
                    }
                }
            }
        }

        public static void SetDefaultFactory(IBackend backend)
        {
            lock (_syncRoot)
            {
                if (_factory == null)
                {
                    _factory = new PromiseFactory(backend);
                    _backend = backend;
                }
                else if (!Object.Equals(_backend, backend))
                {
                    _factory.Dispose();
                    _factory = new PromiseFactory(backend);
                    _backend = backend;
                }
            }
        }

        public static void SetDefaultFactory(int minThreads, int maxThreads)
        {
            SetDefaultFactory(new MooBackend(minThreads, maxThreads));
        }

        public static void SetDefaultFactory(SynchronizationContext context)
        {
            SetDefaultFactory(new SynchronizationContextBackend(context));
        }

        public static void SetDefaultFactory(System.Windows.Threading.Dispatcher dispatcher)
        {
            SetDefaultFactory(new WpfDispatcherBackend(dispatcher));
        }

        public static PromiseBackend DefaultFactory
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_factory == null)
                    {
                        return PromiseBackend.Default;
                    }
                    else if (_backend == null)
                    {
                        return _backendType;
                    }

                    return PromiseBackend.Custom;
                }
            }

            set
            {
                lock (_syncRoot)
                {
                    SetDefaultFactory(value);
                }
            }
        }

        public static IPromiseEnumerable<T> Enumerable<T>(IPromise<IEnumerable<T>> items)
        {
            return new PromiseEnumerable<T>(items);
        }

        public static IPromiseEnumerable<T> Enumerable<T>(IPromise<ICollection<T>> items)
        {
            return Enumerable(items.Cast<IEnumerable<T>>());
        }

        public static IPromiseEnumerable<T> Enumerable<T>(IPromise<IList<T>> items)
        {
            return Enumerable(items.Cast<IEnumerable<T>>());
        }

        public static IPromiseEnumerable<T> Enumerable<T>(IPromise<ISet<T>> items)
        {
            return Enumerable(items.Cast<IEnumerable<T>>());
        }

        public static IPromiseEnumerable<KeyValuePair<K, V>> Enumerable<K, V>(IPromise<IDictionary<K, V>> items)
        {
            return Enumerable(items.Cast<IEnumerable<KeyValuePair<K, V>>>());
        }

        public static IPromiseEnumerable<T> Enumerable<T>(IPromise<T[]> items)
        {
            return Enumerable(items.Cast<IEnumerable<T>>());
        }

        public static IPromiseEnumerable<T> Enumerable<T>(IPromise<List<T>> items)
        {
            return Enumerable(items.Cast<IEnumerable<T>>());
        }

        public static IPromiseEnumerable<T> Enumerable<T>(IPromise<HashSet<T>> items)
        {
            return Enumerable(items.Cast<IEnumerable<T>>());
        }

        public static IPromiseEnumerable<T> Enumerable<T>(IPromise<SortedSet<T>> items)
        {
            return Enumerable(items.Cast<IEnumerable<T>>());
        }

        public static IPromiseEnumerable<KeyValuePair<K, V>> Enumerable<K, V>(IPromise<SortedList<K, V>> items)
        {
            return Enumerable(items.Cast<IEnumerable<KeyValuePair<K, V>>>());
        }

        public static IPromiseEnumerable<T> Enumerable<T>(IPromise<LinkedList<T>> items)
        {
            return Enumerable(items.Cast<IEnumerable<T>>());
        }

        public static IPromiseEnumerable<KeyValuePair<K, V>> Enumerable<K, V>(IPromise<Dictionary<K, V>> items)
        {
            return Enumerable(items.Cast<IEnumerable<KeyValuePair<K, V>>>());
        }

        public static IPromiseEnumerable<T> Enumerable<T>(IPromise<Stack<T>> items)
        {
            return Enumerable(items.Cast<IEnumerable<T>>());
        }

        public static IPromiseEnumerable<T> Enumerable<T>(IPromise<Queue<T>> items)
        {
            return Enumerable(items.Cast<IEnumerable<T>>());
        }

        public static IPromiseEnumerable<T> Enumerable<T>(PromiseFactory factory, IEnumerable<T> items)
        {
            return new PromiseEnumerable<T>(factory, items);
        }

        public static IPromiseEnumerable<KeyValuePair<K, V>> Enumerable<K, V>(IPromise<SortedDictionary<K, V>> items)
        {
            return Enumerable(items.Cast<IEnumerable<KeyValuePair<K, V>>>());
        }

        public static IPromise JoinParallel(IEnumerable<IPromise> promises)
        {
            return Factory.JoinParallel(promises);
        }

        public static IPromise JoinParallel<T>(IEnumerable<IPromise<T>> promises)
        {
            return Factory.JoinParallel(promises);
        }

        public static IPromise JoinSerial(IEnumerable<IPromise> promises)
        {
            return Factory.JoinSerial(promises);
        }

        public static IPromise JoinSerial<T>(IEnumerable<IPromise<T>> promises)
        {
            return Factory.JoinSerial(promises);
        }

        public static PromiseFactory CreateFactory(IBackend backend)
        {
            return new PromiseFactory(backend);
        }

        public static PromiseFactory CreateFactory(PromiseBackend backend)
        {
            return new PromiseFactory(backend);
        }

        public static PromiseFactory CreateFactory(int minThreads, int maxThreads)
        {
            return new PromiseFactory(minThreads, maxThreads);
        }

        public static PromiseFactory CreateFactory(SynchronizationContext context)
        {
            return new PromiseFactory(context);
        }

        public static PromiseFactory CreateFactory(System.Windows.Threading.Dispatcher dispatcher)
        {
            return new PromiseFactory(dispatcher);
        }

        public static PromiseFactory CreateFactory()
        {
            return new PromiseFactory();
        }

        public static PromiseSynchronizationContext CreateSynchronizationContext(PromiseFactory factory)
        {
            return new PromiseSynchronizationContext(factory);
        }

        public static PromiseSynchronizationContext CreateSynchronizationContext()
        {
            return CreateSynchronizationContext(Factory);
        }

        public IPromise Immediately
        {
            get
            {
                return _promise.Immediately;
            }
        }

        public Exception Error
        {
            get
            {
                return _promise.Error;
            }
        }

        public AsyncState State
        {
            get
            {
                return _promise.State;
            }
        }

        private IPromise _promise;

        public Promise(PromiseFactory factory, Action<Action, Action<Exception>> promise, PromisePriority priority)
        {
            _promise = factory.StartNew(() =>
            {
                Exception error = null;
                bool hasError = false;

                promise(() =>
                {
                    error = null;
                    hasError = false;
                }, x =>
                { 
                    error = x;
                    hasError = true;
                });

                if (hasError)
                {
                    throw error;
                }
            }, priority);
        }

        public Promise(PromiseFactory factory, Action<Action, Action<Exception>> promise)
            : this(factory, promise, PromisePriority.Normal)
        {

        }

        public Promise(Action<Action, Action<Exception>> promise)
            : this(Factory, promise)
        {

        }

        public Promise(PromiseFactory factory, Action promise, PromisePriority priority)
        {
            _promise = factory.StartNew(promise, priority);
        }

        public Promise(PromiseFactory factory, Action promise)
            : this(factory, promise, PromisePriority.Normal)
        {

        }

        public Promise(Action promise)
            : this(Factory, promise)
        {

        }

        PromiseFactory IPromiseBase.Factory
        {
            get
            {
                return _promise.Factory;
            }
        }

        public IPromise Then(Action action)
        {
            return _promise.Then(action);
        }

        public IPromise<T> Then<T>(Func<T> action)
        {
            return _promise.Then(action);
        }

        public IPromise Then(Func<IPromise> action)
        {
            return _promise.Then(action);
        }

        public IPromise<T> Then<T>(Func<IPromise<T>> action)
        {
            return _promise.Then(action);
        }

        public IPromise Catch(Action<Exception> action)
        {
            return _promise.Catch(action);
        }

        public IPromise Catch(Action action)
        {
            return _promise.Catch(action);
        }

        public IPromise Finally(Action action)
        {
            return _promise.Finally(action);
        }

        public IPromise Priority(PromisePriority priority)
        {
            return _promise.Priority(priority);
        }

        public void Start()
        {
            _promise.Start();
        }

        public bool Cancel()
        {
            return _promise.Cancel();
        }

        public IPromise Finally(Action<Exception> action)
        {
            return _promise.Finally(action);
        }


        public System.Threading.WaitHandle AsyncWaitHandle
        {
            get
            {
                return _promise.AsyncWaitHandle;
            }
        }

        public void Join()
        {
            _promise.Join();
        }
    }

    public class Promise<T> : IPromise<T>
    {
        private IPromise<T> _promise;

        public static PromiseFactory Factory
        {
            get
            {
                return Promise.Factory;
            }
        }

        public static PromiseFactory CreateFactory(IBackend backend)
        {
            return new PromiseFactory(backend);
        }

        public static PromiseFactory CreateFactory(PromiseBackend backend)
        {
            return new PromiseFactory(backend);
        }

        public static PromiseFactory CreateFactory(int minThreads, int maxThreads)
        {
            return new PromiseFactory(minThreads, maxThreads);
        }

        public static PromiseFactory CreateFactory(SynchronizationContext context)
        {
            return new PromiseFactory(context);
        }

        public static PromiseFactory CreateFactory(System.Windows.Threading.Dispatcher dispatcher)
        {
            return new PromiseFactory(dispatcher);
        }

        public static PromiseFactory CreateFactory()
        {
            return new PromiseFactory();
        }

        public static void SetDefaultFactory(PromiseBackend backend)
        {
            Promise.SetDefaultFactory(backend);
        }

        public static void SetDefaultFactory(IBackend backend)
        {
            Promise.SetDefaultFactory(backend);
        }

        public static void SetDefaultFactory(int minThreads, int maxThreads)
        {
            Promise.SetDefaultFactory(minThreads, maxThreads);
        }

        public static void SetDefaultFactory(SynchronizationContext context)
        {
            Promise.SetDefaultFactory(context);
        }

        public static void SetDefaultFactory(System.Windows.Threading.Dispatcher dispatcher)
        {
            Promise.SetDefaultFactory(dispatcher);
        }

        public IPromise<T> Immediately
        {
            get
            {
                return _promise.Immediately;
            }
        }

        public T Result
        {
            get
            {
                return _promise.Result;
            }
        }

        public Exception Error
        {
            get
            {
                return _promise.Error;
            }
        }

        public AsyncState State
        {
            get
            {
                return _promise.State;
            }
        }

        public Promise(PromiseFactory factory, Action<Action<T>, Action<Exception>> promise, PromisePriority priority)
        {
            _promise = factory.StartNew(() =>
            {
                Exception error = null;
                T result = default(T);
                bool hasError = false;

                promise(x =>
                {
                    error = null;
                    result = x;
                    hasError = false;
                }, x =>
                {
                    error = x;
                    hasError = true;
                });

                if (hasError)
                {
                    throw error;
                }

                return result;
            }, priority);
        }

        public Promise(PromiseFactory factory, Action<Action<T>, Action<Exception>> promise)
            : this(factory, promise, PromisePriority.Normal)
        {

        }

        public Promise(Action<Action<T>, Action<Exception>> promise)
            : this(Promise.Factory, promise)
        {

        }

        public Promise(PromiseFactory factory, Func<T> promise, PromisePriority priority)
        {
            _promise = factory.StartNew(promise, priority);
        }

        public Promise(PromiseFactory factory, Func<T> promise)
            : this(factory, promise, PromisePriority.Normal)
        {

        }

        public Promise(Func<T> promise)
            : this(Promise.Factory, promise)
        {

        }

        PromiseFactory IPromiseBase.Factory
        {
            get
            {
                return _promise.Factory;
            }
        }

        public IPromise Then(Action<T> action)
        {
            return _promise.Then(action);
        }

        public IPromise<F> Then<F>(Func<T, F> action)
        {
            return _promise.Then(action);
        }

        public IPromise Then(Func<T, IPromise> action)
        {
            return _promise.Then(action);
        }

        public IPromise<F> Then<F>(Func<T, IPromise<F>> action)
        {
            return _promise.Then(action);
        }

        public IPromise Then(Action action)
        {
            return _promise.Then(action);
        }

        public IPromise<F> Then<F>(Func<F> action)
        {
            return _promise.Then(action);
        }

        public IPromise Then(Func<IPromise> action)
        {
            return _promise.Then(action);
        }

        public IPromise<F> Then<F>(Func<IPromise<F>> action)
        {
            return _promise.Then(action);
        }

        public IPromise<T> Catch(Action<Exception> action)
        {
            return _promise.Catch(action);
        }

        public IPromise<T> Catch(Action action)
        {
            return _promise.Catch(action);
        }

        public IPromise<T> Finally(Action action)
        {
            return _promise.Finally(action);
        }

        public IPromise<F> Cast<F>()
        {
            return _promise.Cast<F>();
        }

        public IPromise Cast()
        {
            return _promise.Cast();
        }

        public IPromise<T> Priority(PromisePriority priority)
        {
            return _promise.Priority(priority);
        }

        public void Start()
        {
            _promise.Start();
        }

        public bool Cancel()
        {
            return _promise.Cancel();
        }

        public IPromise<T> Finally(Action<Exception> action)
        {
            return _promise.Finally(action);
        }


        public System.Threading.WaitHandle AsyncWaitHandle
        {
            get
            {
                return _promise.AsyncWaitHandle;
            }
        }

        public T Join()
        {
            return _promise.Join();
        }
    }
}
