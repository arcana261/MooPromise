using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MooPromise.Control;

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
                        if (value == null || value.State != ControlState.Next)
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
                    _result.Resolve(ControlValue<T>.Next);
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
            return Run(_factory.Canonical(action));
        }

        public Scope<T> Run(Func<IPromise<ControlState>> action)
        {
            return Run(_factory.Canonical<T>(action));
        }

        public Scope<T> Run(Func<ControlState> action)
        {
            return Run(_factory.Canonical<T>(action));
        }

        public Scope<T> Run(Func<IPromise<T>> action)
        {
            return Run(_factory.Canonical(action));
        }

        public Scope<T> Run(Func<T> action)
        {
            return Run(_factory.Canonical(action));
        }

        public Scope<T> Run(Func<IPromise> action)
        {
            return Run(_factory.Canonical<T>(action));
        }

        public Scope<T> Run(Action action)
        {
            return Run(_factory.Canonical<T>(action));
        }

        public Scope<T> Run(Func<IPromise<NullableResult<T>>> action)
        {
            return Run(_factory.Canonical(action));
        }

        public Scope<T> Run(Func<NullableResult<T>> action)
        {
            return Run(_factory.Canonical(action));
        }

        public Scope<T> Return(IPromise<ControlValue<T>> result)
        {
            return Run(() => result);
        }

        public Scope<T> Return(ControlValue<T> result)
        {
            return Run(() => result);
        }

        public Scope<T> Return(IPromise<NullableResult<T>> result)
        {
            return Run(() => result);
        }

        public Scope<T> Return(NullableResult<T> result)
        {
            return Run(() => result);
        }

        public Scope<T> Return(IPromise<T> result)
        {
            return Run(() => result);
        }

        public Scope<T> Return(T result)
        {
            return Run(() => result);
        }

        public Scope<T> Return(Func<IPromise<ControlValue<T>>> result)
        {
            return Run(result);
        }

        public Scope<T> Return(Func<ControlValue<T>> result)
        {
            return Run(result);
        }

        public Scope<T> Return(Func<IPromise<NullableResult<T>>> result)
        {
            return Run(result);
        }

        public Scope<T> Return(Func<NullableResult<T>> result)
        {
            return Run(result);
        }

        public Scope<T> Return(Func<IPromise<T>> result)
        {
            return Run(result);
        }

        public Scope<T> Return(Func<T> result)
        {
            return Run(result);
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

        public While<T> While(Func<IPromise<ControlValue<bool>>> condition)
        {
            return While(scope => scope.Return(condition()));
        }

        public While<T> While(Func<ControlValue<bool>> condition)
        {
            return While(scope => scope.Return(condition()));
        }

        public While<T> While(Func<IPromise<NullableResult<bool>>> condition)
        {
            return While(scope => scope.Return(condition()));
        }

        public While<T> While(Func<NullableResult<bool>> condition)
        {
            return While(scope => scope.Return(condition()));
        }

        public While<T> While(Func<IPromise<bool>> condition)
        {
            return While(scope => scope.Return(condition()));
        }

        public While<T> While(Func<bool> condition)
        {
            return While(() => _factory.Value(condition()));
        }

        public While<T> While(bool condition)
        {
            return While(() => condition);
        }

        public While<T> While()
        {
            return While(true);
        }

        public If<T> If(Action<Scope<bool>> condition)
        {
            return new If<T>(this, condition);
        }

        public If<T> If(Func<IPromise<ControlValue<bool>>> condition)
        {
            return If(block => block.Return(condition));
        }

        public If<T> If(Func<ControlValue<bool>> condition)
        {
            return If(block => block.Return(condition));
        }

        public If<T> If(Func<IPromise<NullableResult<bool>>> condition)
        {
            return If(block => block.Return(condition));
        }

        public If<T> If(Func<NullableResult<bool>> condition)
        {
            return If(block => block.Return(condition));
        }

        public If<T> If(Func<IPromise<bool>> condition)
        {
            return If(block => block.Return(condition));
        }

        public If<T> If(Func<bool> condition)
        {
            return If(block => block.Return(condition));
        }

        public If<T> If(bool condition)
        {
            return If(() => condition);
        }

        public If<T> If()
        {
            return If(true);
        }

        public If<T> If(NullableResult<bool> condition)
        {
            return If(() => condition);
        }

        public If<T> If(ControlValue<bool> condition)
        {
            return If(() => condition);
        }

        public For<T> For(Func<IPromise<T>> initial, Action<T, Scope<bool>> condition, Action<T, Scope<T>> iterator)
        {
            return null;
        }
    }
}
