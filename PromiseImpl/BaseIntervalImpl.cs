using MooPromise.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.PromiseImpl
{
    internal abstract class BackboneIntervalImpl : IPromiseIntervalBase
    {
        private IList<Action<Exception>> _errorHandlers;
        private IList<Action<object>> _completedHandlers;

        public BackboneIntervalImpl()
        {
            this._errorHandlers = new List<Action<Exception>>();
            this._completedHandlers = new List<Action<object>>();
        }

        public abstract PromiseFactory Factory { get; }
        public abstract bool IsCanceled { get; }
        public abstract PromisePriority Priority { get; }
        public abstract int Timeout { get; }
        public abstract void Cancel();

        public void RaiseFailed(Exception err)
        {
            lock (this)
            {
                foreach (var handler in _errorHandlers)
                {
                    try
                    {
                        handler(err);
                    }
                    catch (Exception inner)
                    {
                        Environment.FailFast("Error occured while executing interval exception handler", inner);
                    }
                }
            }
        }

        public void RaiseCompleted(object value)
        {
            lock (this)
            {
                foreach (var handler in _completedHandlers)
                {
                    try
                    {
                        handler(value);
                    }
                    catch (Exception inner)
                    {
                        Environment.FailFast("Error occured while executing interval exception handler", inner);
                    }
                }
            }
        }

        protected Backbone BaseFinally<Backbone>(Action<Exception> handler, Backbone ret) where Backbone : BackboneIntervalImpl
        {
            lock (this)
            {
                Action<Exception> errorHandler = (err) =>
                {
                    Factory.StartNew(() =>
                    {
                        try
                        {
                            handler(err);
                        }
                        catch (Exception inner)
                        {
                            ret.RaiseFailed(ExceptionUtility.AggregateExceptions("failed to execute error handler", err, inner));
                        }
                    }, Priority);
                };
                _errorHandlers.Add(errorHandler);

                Action<object> completedHandler = value =>
                {
                    Factory.StartNew(() =>
                    {
                        try
                        {
                            handler(null);
                            ret.RaiseCompleted(value);
                        }
                        catch (Exception inner)
                        {
                            ret.RaiseFailed(inner);
                        }
                    }, Priority);
                };
                _completedHandlers.Add(completedHandler);

                return ret;
            }
        }

        protected Backbone BaseFinally<Backbone>(Action handler, Backbone ret) where Backbone : BackboneIntervalImpl
        {
            return BaseFinally(err => handler(), ret);
        }

        protected Backbone BaseCatch<Backbone>(Action<Exception> handler, Backbone ret) where Backbone : BackboneIntervalImpl
        {
            return BaseFinally(err =>
            {
                if (err != null)
                {
                    handler(err);
                }
            }, ret);
        }

        protected Backbone BaseCatch<Backbone>(Action handler, Backbone ret) where Backbone : BackboneIntervalImpl
        {
            return BaseCatch(err => handler(), ret);
        }

        protected Backbone BaseThen<Backbone>(Action handler, Backbone ret) where Backbone : BackboneIntervalImpl
        {
            return BaseThenImpl(value =>
            {
                handler();
                return (object)null;
            }, ret);
        }

        protected Backbone BaseThen<T, Backbone>(Action<T> handler, Backbone ret) where Backbone : BackboneIntervalImpl
        {
            return BaseThenImpl(value =>
            {
                handler((T)value);
                return (object)null;
            }, ret);
        }

        protected Backbone BaseThen<T, Backbone>(Func<T> handler, Backbone ret) where Backbone : BackboneIntervalImpl
        {
            return BaseThenImpl(value =>
            {
                var result = handler();
                return (object)result;
            }, ret);
        }

        protected Backbone BaseThen<T, E, Backbone>(Func<T, E> handler, Backbone ret) where Backbone : BackboneIntervalImpl
        {
            return BaseThenImpl(value =>
            {
                var result = handler((T)value);
                return (object)result;
            }, ret);
        }

        protected Backbone BaseThen<Backbone>(Func<IPromise> handler, Backbone ret) where Backbone : BackboneIntervalImpl
        {
            return BaseThenImpl(value => handler().Then(() => (object)null), ret);
        }

        protected Backbone BaseThen<T, Backbone>(Func<T, IPromise> handler, Backbone ret) where Backbone : BackboneIntervalImpl
        {
            return BaseThenImpl(value => handler((T)value).Then(() => (object)null), ret);
        }

        protected Backbone BaseThen<T, Backbone>(Func<IPromise<T>> handler, Backbone ret) where Backbone : BackboneIntervalImpl
        {
            return BaseThenImpl(value => handler().Then(result => (object)result), ret);
        }

        protected Backbone BaseThen<T, E, Backbone>(Func<T, IPromise<E>> handler, Backbone ret) where Backbone : BackboneIntervalImpl
        {
            return BaseThenImpl(value => handler((T)value).Then(result => (object)result), ret);
        }

        private Backbone BaseThenImpl<Backbone>(Func<object, object> handler, Backbone ret) where Backbone : BackboneIntervalImpl
        {
            lock(this)
            {
                Action<Exception> errorHandler = (err) =>
                {
                    ret.RaiseFailed(err);
                };
                _errorHandlers.Add(errorHandler);

                Action<object> completedHandler = value =>
                {
                    Factory.StartNew(() =>
                    {
                        try
                        {
                            var result = handler(value);
                            ret.RaiseCompleted(result);
                        }
                        catch (Exception inner)
                        {
                            ret.RaiseFailed(inner);
                        }
                    }, Priority);
                };
                _completedHandlers.Add(completedHandler);

                return ret;
            }
        }

        private Backbone BaseThenImpl<Backbone>(Func<object, IPromise<object>> handler, Backbone ret) where Backbone : BackboneIntervalImpl
        {
            lock (this)
            {
                Action<Exception> errorHandler = (err) =>
                {
                    ret.RaiseFailed(err);
                };
                _errorHandlers.Add(errorHandler);

                Action<object> completedHandler = value =>
                {
                    Factory.StartNew(() =>
                    {
                        try
                        {
                            var next = handler(value);

                            next.Then(result =>
                            {
                                ret.RaiseCompleted(result);
                            }).Catch(err =>
                            {
                                ret.RaiseFailed(err);
                            });
                        }
                        catch (Exception inner)
                        {
                            ret.RaiseFailed(inner);
                        }
                    }, Priority);
                };
                _completedHandlers.Add(completedHandler);

                return ret;
            }
        }
    }

    internal abstract class BaseIntervalImpl : BackboneIntervalImpl, IPromiseInterval
    {
        public IPromiseInterval Catch(Action handler)
        {
            return BaseCatch(handler, new BoundIntervalHandle(this));
        }

        public IPromiseInterval Catch(Action<Exception> handler)
        {
            return BaseCatch(handler, new BoundIntervalHandle(this));
        }

        public IPromiseInterval Finally(Action handler)
        {
            return BaseFinally(handler, new BoundIntervalHandle(this));
        }

        public IPromiseInterval Finally(Action<Exception> handler)
        {
            return BaseFinally(handler, new BoundIntervalHandle(this));
        }

        public IPromiseInterval Then(Func<IPromise> handler)
        {
            return BaseThen(handler, new BoundIntervalHandle(this));
        }

        public IPromiseInterval Then(Action handler)
        {
            return BaseThen(handler, new BoundIntervalHandle(this));
        }

        public IPromiseInterval<T> Then<T>(Func<T> handler)
        {
            return BaseThen(handler, new BoundIntervalHandle<T>(this));
        }

        public IPromiseInterval<T> Then<T>(Func<IPromise<T>> handler)
        {
            return BaseThen(handler, new BoundIntervalHandle<T>(this));
        }
    }

    internal abstract class BaseIntervalImpl<T> : BackboneIntervalImpl, IPromiseInterval<T>
    {
        public IPromiseInterval Cast()
        {
            return Then(() => { });
        }

        public IPromiseInterval<E> Cast<E>()
        {
            return Then(value => (E)((object)value));
        }

        public IPromiseInterval<T> Catch(Action handler)
        {
            return BaseCatch(handler, new BoundIntervalHandle<T>(this));
        }

        public IPromiseInterval<T> Catch(Action<Exception> handler)
        {
            return BaseCatch(handler, new BoundIntervalHandle<T>(this));
        }

        public IPromiseInterval<T> Finally(Action handler)
        {
            return BaseFinally(handler, new BoundIntervalHandle<T>(this));
        }

        public IPromiseInterval<T> Finally(Action<Exception> handler)
        {
            return BaseFinally(handler, new BoundIntervalHandle<T>(this));
        }

        public IPromiseInterval Then(Func<IPromise> handler)
        {
            return Then(value => handler());
        }

        public IPromiseInterval Then(Func<T, IPromise> handler)
        {
            return BaseThen(handler, new BoundIntervalHandle(this));
        }

        public IPromiseInterval Then(Action handler)
        {
            return Then(value => handler());
        }

        public IPromiseInterval Then(Action<T> handler)
        {
            return BaseThen(handler, new BoundIntervalHandle(this));
        }

        public IPromiseInterval<E> Then<E>(Func<IPromise<E>> handler)
        {
            return Then(value => handler());
        }

        public IPromiseInterval<E> Then<E>(Func<E> handler)
        {
            return Then(value => handler());
        }

        public IPromiseInterval<E> Then<E>(Func<T, IPromise<E>> handler)
        {
            return BaseThen(handler, new BoundIntervalHandle<E>(this));
        }

        public IPromiseInterval<E> Then<E>(Func<T, E> handler)
        {
            return BaseThen(handler, new BoundIntervalHandle<E>(this));
        }
    }
}
