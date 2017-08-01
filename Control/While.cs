using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Control
{
    public class While
    {
        private PromiseFactory _factory;
        private Func<IPromise<ControlValue<bool>>> _condition;

        internal While(PromiseFactory factory, Func<IPromise<ControlValue<bool>>> condition)
        {
            this._factory = factory;
            this._condition = condition;
        }

        internal While(PromiseFactory factory, Func<ControlValue<bool>> condition)
            : this(factory, () => factory.Value(condition()))
        {

        }

        internal While(PromiseFactory factory, Func<IPromise<bool>> condition)
            : this(factory, () =>
            {
                var next = condition();

                if (next == null)
                {
                    return null;
                }

                return next.Then(result => new ControlValue<bool>(result));
            })
        {

        }

        internal While(PromiseFactory factory, Func<bool> condition)
            : this(factory, () => factory.Value(condition()))
        {

        }

        public PromiseFactory Factory
        {
            get
            {
                return _factory;
            }
        }

        public IPromise Do(Action body)
        {
            return Do(() =>
            {
                body();

                return _factory.Value();
            });
        }

        public IPromise Do(Func<IPromise> body)
        {
            return Do(() =>
            {
                var next = body();

                if (next == null)
                {
                    return null;
                }

                return next.Then(() => ControlState.Next);
            }).Cast();
        }

        public IPromise<ControlState> Do(Func<ControlState> body)
        {
            return Do(() => _factory.Value(body()));
        }

        public IPromise<ControlState> Do(Func<IPromise<ControlState>> body)
        {
            return Do(() =>
            {
                var next = body();

                if (next == null)
                {
                    return null;
                }

                return next.Then(result => new ControlValue<object>(result));
            }).Then(result => result.State);
        }

        public IPromise<ControlValue<E>> Do<E>(Func<ControlValue<E>> body)
        {
            return Do(() => _factory.Value(body()));
        }

        public IPromise<NullableResult<T>> Do<T>(Func<IPromise<NullableResult<T>>> body)
        {
            return Do(() =>
            {
                var next = body();

                if (next == null)
                {
                    return null;
                }

                return next.Then(result =>
                {
                    if (result == null || !result.HasResult)
                    {
                        return ControlValue<NullableResult<T>>.Next;
                    }

                    return ControlValue<NullableResult<T>>.Return(result);
                });
            }).Then(result =>
            {
                if (result == null || !result.HasValue)
                {
                    return new NullableResult<T>();
                }

                return result.Value;
            });
        }

        public IPromise<NullableResult<T>> Do<T>(Func<NullableResult<T>> body)
        {
            return Do(() => _factory.Value(body()));
        }

        public IPromise<NullableResult<T>> Do<T>(Func<IPromise<T>> body)
        {
            return Do(() =>
            {
                var next = body();

                if (next == null)
                {
                    return null;
                }

                return next.Then(result => new NullableResult<T>(result));
            });
        }

        public IPromise<NullableResult<T>> Do<T>(Func<T> body)
        {
            return Do(() => _factory.Value(body()));
        }

        public IPromise<ControlValue<E>> Do<E>(Func<IPromise<ControlValue<E>>> body)
        {
            var nextCondition = _condition();

            if (nextCondition == null)
            {
                return _factory.Value(ControlValue<E>.Next);
            }

            return nextCondition.Then(conditionResult =>
            {
                if (conditionResult == null || conditionResult.State != ControlState.Return || !conditionResult.HasValue || !conditionResult.Value)
                {
                    return _factory.Value(ControlValue<E>.Next);
                }

                var nextBody = body();

                if (nextBody == null)
                {
                    return _factory.Value(ControlValue<E>.Next);
                }

                return nextBody.Then(bodyResult =>
                {
                    if (bodyResult == null || bodyResult.State == ControlState.Break)
                    {
                        return _factory.Value(ControlValue<E>.Next);
                    }

                    if (bodyResult.State == ControlState.Return)
                    {
                        return _factory.Value(bodyResult);
                    }

                    return Do(body);
                });
            });
        }
    }
}
