using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise
{
    public class PromiseSynchronizationContext
    {
        private PromiseFactory _factory;
        private IPromise _last;
        private object _syncRoot;

        public PromiseSynchronizationContext(PromiseFactory factory)
        {
            this._factory = factory;
            this._last = null;
            this._syncRoot = new object();
        }

        public PromiseSynchronizationContext()
            : this(Promise.Factory)
        {

        }

        public IPromise Post(Action action)
        {
            return Post(action, PromisePriority.Immediate);
        }

        public IPromise Post(Action action, PromisePriority priority)
        {
            lock (_syncRoot)
            {
                if (_last != null)
                {
                    var ret = _factory.Create(action, priority);
                    _last.Immediately.Finally(() =>
                    {
                        ret.Start();
                    });
                    _last = ret;
                }
                else
                {
                    _last = _factory.StartNew(action, priority);
                }

                return _last;
            }
        }

        public IPromise Post(Func<IPromise> action)
        {
            return Post(action, PromisePriority.Immediate);
        }

        public IPromise Post(Func<IPromise> action, PromisePriority priority)
        {
            lock (_syncRoot)
            {
                if (_last != null)
                {
                    var ret = _factory.Create(action, priority);
                    _last.Immediately.Finally(() =>
                    {
                        ret.Start();
                    });
                    _last = ret;
                }
                else
                {
                    _last = _factory.StartNew(action, priority);
                }

                return _last;
            }
        }

        public IPromise<T> Post<T>(Func<T> action)
        {
            return Post(action, PromisePriority.Immediate);
        }

        public IPromise<T> Post<T>(Func<T> action, PromisePriority priority)
        {
            lock (_syncRoot)
            {
                if (_last != null)
                {
                    var ret = _factory.Create(action, priority);
                    _last.Immediately.Finally(() =>
                    {
                        ret.Start();
                    });
                    _last = ret.Cast();
                    return ret;
                }
                else
                {
                    var ret = _factory.StartNew(action, priority);
                    _last = ret.Cast();
                    return ret;
                }
            }
        }

        public IPromise<T> Post<T>(Func<IPromise<T>> action)
        {
            return Post(action, PromisePriority.Immediate);
        }

        public IPromise<T> Post<T>(Func<IPromise<T>> action, PromisePriority priority)
        {
            lock (_syncRoot)
            {
                if (_last != null)
                {
                    var ret = _factory.Create(action, priority);
                    _last.Immediately.Finally(() =>
                    {
                        ret.Start();
                    });
                    _last = ret.Cast();
                    return ret;
                }
                else
                {
                    var ret = _factory.StartNew(action, priority);
                    _last = ret.Cast();
                    return ret;
                }
            }
        }
    }
}
