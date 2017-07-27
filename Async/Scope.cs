using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Async
{
    public class Scope<T>
    {
        private PromiseFactory _factory;
        private IPromise<ControlValue<T>> _last;
        private DefinitionBag _defs;
        private IManualPromise<ControlValue<T>> _result;

        internal Scope(PromiseFactory factory)
            : this(factory, null)
        {
            
        }

        internal Scope(PromiseFactory factory, DefinitionBag defs)
        {
            this._factory = factory;
            this._last = null;
            this._defs = new DefinitionBag(defs);
            this._result = factory.CreateManual<ControlValue<T>>();
        }

        public PromiseFactory Factory
        {
            get
            {
                return _factory;
            }
        }

        public DefinitionBag Variables
        {
            get
            {
                return _defs;
            }
        }

        internal void Next(Func<IPromise<ControlValue<T>>> action)
        {
            lock (this)
            {
                if (_last == null)
                {
                    _last = action();
                }
                else
                {
                    _last = _last.Then(value =>
                    {
                        if (value == null || value.State != ControlState.Continue)
                        {
                            return _factory.Value(value);
                        }

                        return action();
                    });
                }
            }
        }

        internal IPromise<ControlValue<T>> Finish()
        {
            lock (this)
            {
                if (_last == null)
                {
                    _result.Resolve(ControlValue<T>.Continue);
                }
                else
                {
                    _last.Then(value =>
                    {
                        _result.Resolve(value);
                    }).Catch(err =>
                    {
                        _result.Reject(err);
                    });
                }

                return _result;
            }
        }

        public Scope<T> Run(Func<IPromise<ControlValue<T>>> action)
        {
            Next(action);
            return this;
        }

        public Scope<T> Run(Func<ControlValue<T>> action)
        {
            return Run(() => _factory.Value(action()));
        }

        public Scope<T> Run(Func<IPromise<ControlState>> action)
        {
            return Run(() =>
            {
                var next = action();

                if (next == null)
                {
                    return null;
                }

                return next.Then(result => new ControlValue<T>(result));
            });
        }

        public Scope<T> Run(Func<ControlState> action)
        {
            return Run(() => _factory.Value(action()));
        }

        public Scope<T> Run(Func<IPromise<T>> action)
        {
            return Run(() =>
            {
                var next = action();

                if (next == null)
                {
                    return null;
                }

                return next.Then(result => new ControlValue<T>(result));
            });
        }

        public Scope<T> Run(Func<T> action)
        {
            return Run(() => _factory.Value(action()));
        }

        public Scope<T> Run(Func<IPromise> action)
        {
            return Run(() =>
            {
                var next = action();

                if (next == null)
                {
                    return null;
                }

                return next.Then(() => ControlState.Continue);
            });
        }

        public Scope<T> Run(Action action)
        {
            return Run(() =>
            {
                action();

                return _factory.Value();
            });
        }

        public Scope<T> Return(IPromise<T> result)
        {
            return Run(() => result);
        }

        public Scope<T> Return(T result)
        {
            return Run(() => result);
        }

        public Scope<T> Return(Func<T> result)
        {
            return Run(() => result());
        }

        public Scope<T> Return(Func<IPromise<T>> result)
        {
            return Run(() => result());
        }

        public Scope<T> Begin(Action<Scope<T>> block)
        {
            return Run(() =>
            {
                Scope<T> newScope = new Scope<T>(_factory, _defs);
                block(newScope);

                return newScope.Finish();
            });
        }

        public Scope<T> Begin(Action block)
        {
            return Begin(scope => block());
        }

        internal Scope<E> BeginImmediately<E>(Action<Scope<E>> block)
        {
            Scope<E> newScope = new Scope<E>(_factory, _defs);
            block(newScope);

            return newScope;
        }

        public While<T> While(Action<Scope<bool>> condition)
        {
            return new While<T>(this, condition);
        }

        public While<T> While(Func<IPromise<bool>> condition)
        {
            return While(scope => scope.Return(condition));
        }

        public While<T> While(Func<bool> condition)
        {
            return While(() => _factory.Value(condition()));
        }

        public For<T> For(Func<IPromise<T>> initial, Action<T, Scope<bool>> condition, Action<T, Scope<T>> iterator)
        {
            return null;
        }
    }
}
