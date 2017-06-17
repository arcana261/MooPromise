using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise
{
    public class Synchronization
    {
        private PromiseFactory _factory;
        private IPromise _last;
        private object _syncRoot;

        public Synchronization(PromiseFactory factory)
        {
            this._factory = factory;
            this._last = null;
            this._syncRoot = new object();
        }

        public Synchronization()
            : this(Promise.Factory)
        {

        }

        public IPromise Post(Action action)
        {
            lock (_syncRoot)
            {
                if (_last != null)
                {
                    _last = _last.Finally(action);
                }
                else
                {
                    _last = _factory.StartNew(action);
                }

                return _last;
            }
        }

        public IPromise Post(Action action, PromisePriority priority)
        {
            lock (_syncRoot)
            {
                if (_last != null)
                {
                    _last = _last.Priority(priority).Finally(action);
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
            lock (_syncRoot)
            {
                if (_last != null)
                {
                    var ret = _factory.Create(action);
                    _last.Finally(() =>
                    {
                        ret.Start();
                    });
                    _last = ret;
                }
                else
                {
                    _last = _factory.StartNew(action);
                }

                return _last;
            }
        }

        public IPromise Post(Func<IPromise> action, PromisePriority priority)
        {
            lock (_syncRoot)
            {
                if (_last != null)
                {
                    var ret = _factory.Create(action, priority);
                    _last.Finally(() =>
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
            lock (_syncRoot)
            {
                if (_last != null)
                {
                    var ret = _factory.Create(action);
                    _last.Finally(() =>
                    {
                        ret.Start();
                    });
                    _last = ret.Cast();
                    return ret;
                }
                else
                {
                    var ret = _factory.StartNew(action);
                    _last = ret.Cast();
                    return ret;
                }
            }
        }

        public IPromise<T> Post<T>(Func<T> action, PromisePriority priority)
        {
            lock (_syncRoot)
            {
                if (_last != null)
                {
                    var ret = _factory.Create(action, priority);
                    _last.Finally(() =>
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
            lock (_syncRoot)
            {
                if (_last != null)
                {
                    var ret = _factory.Create(action);
                    _last.Finally(() =>
                    {
                        ret.Start();
                    });
                    _last = ret.Cast();
                    return ret;
                }
                else
                {
                    var ret = _factory.StartNew(action);
                    _last = ret.Cast();
                    return ret;
                }
            }
        }

        public IPromise<T> Post<T>(Func<IPromise<T>> action, PromisePriority priority)
        {
            lock (_syncRoot)
            {
                if (_last != null)
                {
                    var ret = _factory.Create(action, priority);
                    _last.Finally(() =>
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
