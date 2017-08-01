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

        public PromiseFactory Factory
        {
            get
            {
                return _factory;
            }
        }

        public IPromise<ControlValue<T>> Do<T>(Func<IPromise<ControlValue<T>>> body)
        {
            return _factory.SafeThen(_condition, check =>
            {
                if (check == null || check.State != ControlState.Return || !check.HasValue || !check.Value)
                {
                    return _factory.Value(ControlValue<T>.Next);
                }

                return _factory.SafeThen(body, result =>
                {
                    if (result == null || result.State == ControlState.Break)
                    {
                        return _factory.Value(ControlValue<T>.Next);
                    }

                    if (result.State == ControlState.Return)
                    {
                        return _factory.Value(result);
                    }

                    return Do(body);
                });
            });
        }

        public IPromise<ControlValue<T>> Do<T>(Func<ControlValue<T>> body)
        {
            return Do(_factory.Canonical(body));
        }

        public IPromise<NullableResult<T>> Do<T>(Func<IPromise<NullableResult<T>>> body)
        {
            return Do(_factory.Canonical(body)).ToNullableResult(_factory);
        }

        public IPromise<NullableResult<T>> Do<T>(Func<NullableResult<T>> body)
        {
            return Do(_factory.Canonical(body)).ToNullableResult(_factory);
        }

        public IPromise<NullableResult<T>> Do<T>(Func<IPromise<T>> body)
        {
            return Do(_factory.Canonical(body)).ToNullableResult(_factory);
        }

        public IPromise<NullableResult<T>> Do<T>(Func<T> body)
        {
            return Do(_factory.Canonical(body)).ToNullableResult(_factory);
        }

        public IPromise<ControlState> Do(Func<IPromise<ControlState>> body)
        {
            return Do(_factory.Canonical(body)).ToControlState(_factory);
        }

        public IPromise<ControlState> Do(Func<ControlState> body)
        {
            return Do(_factory.Canonical(body)).ToControlState(_factory);
        }

        public IPromise Do(Func<IPromise> body)
        {
            return Do(_factory.Canonical(body)).Cast();
        }

        public IPromise Do(Action body)
        {
            return Do(_factory.Canonical(body)).Cast();
        }
    }
}
