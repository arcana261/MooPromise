using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.PromiseImpl
{
    internal class BackboneIntervalHandleImpl<Backbone> : IntervalHandleBase where Backbone : BackboneIntervalImpl
    {
        private Backbone _backbone;
        private Func<IPromise<object>> _action;
        private volatile bool _canceled;
        private volatile bool _started;
        private PromiseFactory _factory;
        private int _timeout;
        private PromisePriority _priority;

        public BackboneIntervalHandleImpl(Backbone backbone, PromiseFactory factory, int timeout, Func<IPromise<object>> action, PromisePriority priority)
        {
            this._backbone = backbone;
            this._factory = factory;
            this._timeout = timeout;
            this._action = action;
            this._priority = priority;
            this._canceled = false;
            this._started = false;
        }

        private void Schedule()
        {
            Factory.SetTimeout(() =>
            {
                if (!_canceled)
                {
                    Schedule();

                    try
                    {
                        _action().Then(value =>
                        {
                            _backbone.RaiseCompleted(value);
                        }).Catch(err =>
                        {
                            _backbone.RaiseFailed(err);
                        });
                    }
                    catch (Exception err)
                    {
                        _backbone.RaiseFailed(err);
                    }
                }
            }, Timeout, Priority);
        }

        public void Start()
        {
            lock (this)
            {
                if (!_started && !_canceled)
                {
                    _started = true;
                    Schedule();
                }
            }
        }

        public PromiseFactory Factory
        {
            get
            {
                return _factory;
            }
        }

        public bool IsCanceled
        {
            get
            {
                return _canceled;
            }
        }

        public PromisePriority Priority
        {
            get
            {
                return _priority;
            }
        }

        public int Timeout
        {
            get
            {
                return _timeout;
            }
        }

        public void Cancel()
        {
            _canceled = true;
        }
    }

    internal class IntervalHandleImpl : BaseIntervalImpl
    {
        private IntervalHandleBase _base;

        public IntervalHandleImpl(PromiseFactory factory, int timeout, Func<IPromise> action, PromisePriority priority)
        {
            var x = new BackboneIntervalHandleImpl<IntervalHandleImpl>(this, factory, timeout, () => action().Then(() => (object)null), priority);
            x.Start();

            _base = x;
        }

        public IntervalHandleImpl(PromiseFactory factory, int timeout, Action action, PromisePriority priority)
        {
            var x = new BackboneIntervalHandleImpl<IntervalHandleImpl>(this, factory, timeout, () =>
            {
                action();
                return factory.Value<object>(null);
            }, priority);
            x.Start();

            _base = x;
        }

        public override PromiseFactory Factory
        {
            get
            {
                return _base.Factory;
            }
        }

        public override bool IsCanceled
        {
            get
            {
                return _base.IsCanceled;
            }
        }

        public override PromisePriority Priority
        {
            get
            {
                return _base.Priority;
            }
        }

        public override int Timeout
        {
            get
            {
                return _base.Timeout;
            }
        }

        public override void Cancel()
        {
            _base.Cancel();
        }
    }

    internal class IntervalHandleImpl<T> : BaseIntervalImpl<T>
    {
        private IntervalHandleBase _base;

        public IntervalHandleImpl(PromiseFactory factory, int timeout, Func<IPromise<T>> action, PromisePriority priority)
        {
            var x = new BackboneIntervalHandleImpl<IntervalHandleImpl<T>>(this, factory, timeout, () => action().Then(result => (object)result), priority);
            x.Start();

            _base = x;
        }

        public IntervalHandleImpl(PromiseFactory factory, int timeout, Func<T> action, PromisePriority priority)
        {
            var x = new BackboneIntervalHandleImpl<IntervalHandleImpl<T>>(this, factory, timeout, () => factory.Value((object)action()), priority);
            x.Start();

            _base = x;
        }

        public override PromiseFactory Factory
        {
            get
            {
                return _base.Factory;
            }
        }

        public override bool IsCanceled
        {
            get
            {
                return _base.IsCanceled;
            }
        }

        public override PromisePriority Priority
        {
            get
            {
                return _base.Priority;
            }
        }

        public override int Timeout
        {
            get
            {
                return _base.Timeout;
            }
        }

        public override void Cancel()
        {
            _base.Cancel();
        }
    }
}
