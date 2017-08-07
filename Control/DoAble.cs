using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Control
{
    public abstract class DoAble
    {
        private PromiseFactory _factory;

        protected DoAble(PromiseFactory factory)
        {
            this._factory = factory;
        }

        public PromiseFactory Factory
        {
            get
            {
                return _factory;
            }
        }

        public abstract IPromise<ControlValue<T>> Do<T>(Func<IPromise<ControlValue<T>>> body);

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

        public IPromise<ControlValue<T>> Do<T>(ControlValue<T> value)
        {
            return Do(() => value);
        }

        public IPromise<NullableResult<T>> Do<T>(NullableResult<T> value)
        {
            return Do(() => value);
        }

        public IPromise<NullableResult<T>> Do<T>(T value)
        {
            return Do(() => value);
        }

        public IPromise<ControlState> Do(ControlState value)
        {
            return Do(() => value);
        }

        public IPromise Do()
        {
            return Do(() => { });
        }

        public IPromise<ControlValue<T>> Do<T>(IPromise<ControlValue<T>> value)
        {
            return Do(() => value);
        }

        public IPromise<NullableResult<T>> Do<T>(IPromise<NullableResult<T>> value)
        {
            return Do(() => value);
        }

        public IPromise<NullableResult<T>> Do<T>(IPromise<T> value)
        {
            return Do(() => value);
        }

        public IPromise<ControlState> Do(IPromise<ControlState> value)
        {
            return Do(() => value);
        }

        public IPromise Do(IPromise value)
        {
            return Do(() => value);
        }
    }

    public abstract class DoAble<T>
    {
        private PromiseFactory _factory;

        protected DoAble(PromiseFactory factory)
        {
            this._factory = factory;
        }

        public PromiseFactory Factory
        {
            get
            {
                return _factory;
            }
        }

        public abstract IPromise<ControlValue<T>> Do(Func<IPromise<ControlValue<T>>> body);

        public IPromise<ControlValue<T>> Do(Func<ControlValue<T>> body)
        {
            return Do(_factory.Canonical(body));
        }

        public IPromise<NullableResult<T>> Do(Func<IPromise<NullableResult<T>>> body)
        {
            return Do(_factory.Canonical(body)).ToNullableResult(_factory);
        }

        public IPromise<NullableResult<T>> Do(Func<NullableResult<T>> body)
        {
            return Do(_factory.Canonical(body)).ToNullableResult(_factory);
        }

        public IPromise<NullableResult<T>> Do(Func<IPromise<T>> body)
        {
            return Do(_factory.Canonical(body)).ToNullableResult(_factory);
        }

        public IPromise<NullableResult<T>> Do(Func<T> body)
        {
            return Do(_factory.Canonical(body)).ToNullableResult(_factory);
        }

        public IPromise<ControlState> Do(Func<IPromise<ControlState>> body)
        {
            return Do(_factory.Canonical<T>(body)).ToControlState(_factory);
        }

        public IPromise<ControlState> Do(Func<ControlState> body)
        {
            return Do(_factory.Canonical<T>(body)).ToControlState(_factory);
        }

        public IPromise Do(Func<IPromise> body)
        {
            return Do(_factory.Canonical<T>(body)).Cast();
        }

        public IPromise Do(Action body)
        {
            return Do(_factory.Canonical<T>(body)).Cast();
        }

        public IPromise<ControlValue<T>> Do(ControlValue<T> value)
        {
            return Do(() => value);
        }

        public IPromise<NullableResult<T>> Do(NullableResult<T> value)
        {
            return Do(() => value);
        }

        public IPromise<NullableResult<T>> Do(T value)
        {
            return Do(() => value);
        }

        public IPromise<ControlState> Do(ControlState value)
        {
            return Do(() => value);
        }

        public IPromise Do()
        {
            return Do(() => { });
        }

        public IPromise<ControlValue<T>> Do(IPromise<ControlValue<T>> value)
        {
            return Do(() => value);
        }

        public IPromise<NullableResult<T>> Do(IPromise<NullableResult<T>> value)
        {
            return Do(() => value);
        }

        public IPromise<NullableResult<T>> Do(IPromise<T> value)
        {
            return Do(() => value);
        }

        public IPromise<ControlState> Do(IPromise<ControlState> value)
        {
            return Do(() => value);
        }

        public IPromise Do(IPromise value)
        {
            return Do(() => value);
        }
    }
}
