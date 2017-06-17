using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise
{
    public class Promise : IPromise
    {
        private static object _syncRoot = new object();
        private static PromiseFactory _factory;

        public static PromiseFactory Factory
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_factory == null)
                    {
                        _factory = new PromiseFactory();
                    }

                    return _factory;
                }
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

        public static PromiseFactory CreateFactory()
        {
            return new PromiseFactory();
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
                promise(() => { error = null; }, x => { error = x; });

                if (error != null)
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

        public static PromiseFactory CreateFactory()
        {
            return new PromiseFactory();
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

                promise(x => { error = null; result = x; }, x => { error = x; });

                if (error != null)
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
    }
}
