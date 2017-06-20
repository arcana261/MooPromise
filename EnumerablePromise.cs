using MooPromise.PromiseImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MooPromise.TaskRunner;
using System.Threading;
using MooPromise.DataStructure;

namespace MooPromise
{
    internal class EnumerablePromise<T> : IEnumerablePromise<T>
    {
        private IPromise<IEnumerable<IPromise<T>>> _items;

        public EnumerablePromise(IPromise<IEnumerable<IPromise<T>>> items)
        {
            this._items = items;
        }

        public EnumerablePromise(IPromise<IEnumerable<T>> items)
            : this(items.Then(x => x.Select(y => Promise.Factory.StartNew(y))))
        {

        }

        public EnumerablePromise(IEnumerable<IPromise<T>> items)
            : this(Promise.Factory.StartNew(items))
        {

        }

        public EnumerablePromise(IEnumerable<T> items)
            : this(Promise.Factory.StartNew(items))
        {

        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                return _items.AsyncWaitHandle;
            }
        }

        public Exception Error
        {
            get
            {
                return _items.Error;
            }
        }

        public IPromise<IEnumerable<IPromise<T>>> Immediately
        {
            get
            {
                return _items.Immediately;
            }
        }

        public IEnumerable<IPromise<T>> Result
        {
            get
            {
                return _items.Result;
            }
        }

        public AsyncState State
        {
            get
            {
                return _items.State;
            }
        }

        public bool Cancel()
        {
            return _items.Cancel();
        }

        public IPromise Cast()
        {
            return _items.Cast();
        }

        public IPromise<F> Cast<F>()
        {
            return _items.Cast<F>();
        }

        public IPromise<IEnumerable<IPromise<T>>> Catch(Action action)
        {
            return _items.Catch(action);
        }

        public IPromise<IEnumerable<IPromise<T>>> Catch(Action<Exception> action)
        {
            return _items.Catch(action);
        }

        public IPromise<IEnumerable<IPromise<T>>> Finally(Action<Exception> action)
        {
            return _items.Finally(action);
        }

        public IPromise<IEnumerable<IPromise<T>>> Finally(Action action)
        {
            return _items.Finally(action);
        }

        public IEnumerable<IPromise<T>> Join()
        {
            return _items.Join();
        }

        public IPromise<IEnumerable<IPromise<T>>> Priority(PromisePriority priority)
        {
            return _items.Priority(priority);
        }

        public void Start()
        {
            _items.Start();
        }

        public IPromise Then(Action action)
        {
            return _items.Then(action);
        }

        public IPromise Then(Func<IPromise> action)
        {
            return _items.Then(action);
        }

        public IPromise Then(Func<IEnumerable<IPromise<T>>, IPromise> action)
        {
            return _items.Then(action);
        }

        public IPromise Then(Action<IEnumerable<IPromise<T>>> action)
        {
            return _items.Then(action);
        }

        public IPromise<F> Then<F>(Func<IPromise<F>> action)
        {
            return _items.Then(action);
        }

        public IPromise<F> Then<F>(Func<F> action)
        {
            return _items.Then(action);
        }

        public IPromise<F> Then<F>(Func<IEnumerable<IPromise<T>>, IPromise<F>> action)
        {
            return _items.Then(action);
        }

        public IPromise<F> Then<F>(Func<IEnumerable<IPromise<T>>, F> action)
        {
            return _items.Then(action);
        }

        

        public IPromise<IPromiseEnumerator<T>> GetEnumerator()
        {
            return _items.Then(x => (IPromiseEnumerator<T>)(new Enumerator(x.GetEnumerator())));
        }
    }
}
