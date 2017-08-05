using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Control
{
    public class Control
    {
        private PromiseFactory _factory;

        internal Control(PromiseFactory factory)
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

        public DoWhileControlValue<T> Do<T>(Func<IPromise<ControlValue<T>>> body)
        {
            return new DoWhileControlValue<T>(_factory, body);
        }

        public DoWhileControlValue<T> Do<T>(Func<ControlValue<T>> body)
        {
            return Do(_factory.Canonical(body));
        }

        public DoWhileNullableResult<T> Do<T>(Func<IPromise<NullableResult<T>>> body)
        {
            return new DoWhileNullableResult<T>(_factory, _factory.Canonical(body));
        }

        public DoWhileNullableResult<T> Do<T>(Func<NullableResult<T>> body)
        {
            return new DoWhileNullableResult<T>(_factory, _factory.Canonical(body));
        }

        public DoWhileNullableResult<T> Do<T>(Func<IPromise<T>> body)
        {
            return new DoWhileNullableResult<T>(_factory, _factory.Canonical(body));
        }

        public DoWhileNullableResult<T> Do<T>(Func<T> body)
        {
            return new DoWhileNullableResult<T>(_factory, _factory.Canonical(body));
        }

        public DoWhileVoid Do(Func<IPromise> body)
        {
            return new DoWhileVoid(_factory, _factory.Canonical(body));
        }

        public DoWhileVoid Do(Action body)
        {
            return new DoWhileVoid(_factory, _factory.Canonical(body));
        }

        public DoWhileVoid Do(Func<IPromise<ControlState>> body)
        {
            return new DoWhileVoid(_factory, _factory.Canonical(body));
        }

        public DoWhileVoid Do(Func<ControlState> body)
        {
            return new DoWhileVoid(_factory, _factory.Canonical(body));
        }

        public While While(Func<IPromise<bool>> condition)
        {
            return While(_factory.Canonical(condition));
        }

        public While While(Func<bool> condition)
        {
            return While(() => _factory.Value(condition()));
        }

        public While While(Func<IPromise<ControlValue<bool>>> condition)
        {
            return new While(_factory, condition);
        }

        public While While(Func<ControlValue<bool>> condition)
        {
            return While(_factory.Canonical(condition));
        }

        public While While(Func<IPromise<NullableResult<bool>>> condition)
        {
            return While(_factory.Canonical(condition));
        }

        public While While(Func<NullableResult<bool>> condition)
        {
            return While(_factory.Canonical(condition));
        }

        public While While(bool value)
        {
            return While(() => value);
        }

        public While While()
        {
            return While(true);
        }

        public While While(IPromise<bool> condition)
        {
            return While(() => condition);
        }

        public While While(NullableResult<bool> condition)
        {
            return While(() => condition);
        }

        public While While(IPromise<NullableResult<bool>> condition)
        {
            return While(() => condition);
        }

        public While While(ControlValue<bool> condition)
        {
            return While(() => condition);
        }

        public While While(IPromise<ControlValue<bool>> condition)
        {
            return While(() => condition);
        }

        public ForWithSeed<T> For<T>(Func<IPromise<ControlValue<T>>> seed)
        {
            return new ForWithSeed<T>(_factory, seed);
        }

        public ForWithSeed<T> For<T>(Func<ControlValue<T>> seed)
        {
            return For(_factory.Canonical(seed));
        }

        public ForWithSeed<T> For<T>(Func<IPromise<NullableResult<T>>> seed)
        {
            return For(_factory.Canonical(seed));
        }

        public ForWithSeed<T> For<T>(Func<NullableResult<T>> seed)
        {
            return For(_factory.Canonical(seed));
        }

        public ForWithSeed<T> For<T>(Func<IPromise<T>> seed)
        {
            return For(_factory.Canonical(seed));
        }

        public ForWithSeed<T> For<T>(Func<T> seed)
        {
            return For(_factory.Canonical(seed));
        }

        public ForWithSeed<T> For<T>(T seed)
        {
            return For(() => seed);
        }

        public ForWithSeed<T> For<T>(IPromise<T> seed)
        {
            return For(() => seed);
        }

        public ForWithSeed<T> For<T>(NullableResult<T> seed)
        {
            return For(() => seed);
        }

        public ForWithSeed<T> For<T>(IPromise<NullableResult<T>> seed)
        {
            return For(() => seed);
        }

        public ForWithSeed<T> For<T>(ControlValue<T> seed)
        {
            return For(() => seed);
        }

        public ForWithSeed<T> For<T>(IPromise<ControlValue<T>> seed)
        {
            return For(() => seed);
        }

        public If If(Func<IPromise<ControlValue<bool>>> condition)
        {
            return new If(_factory, condition);
        }

        public If If(Func<ControlValue<bool>> condition)
        {
            return If(_factory.Canonical(condition));
        }

        public If If(Func<IPromise<NullableResult<bool>>> condition)
        {
            return If(_factory.Canonical(condition));
        }

        public If If(Func<NullableResult<bool>> condition)
        {
            return If(_factory.Canonical(condition));
        }

        public If If(Func<IPromise<bool>> condition)
        {
            return If(_factory.Canonical(condition));
        }

        public If If(Func<bool> condition)
        {
            return If(_factory.Canonical(condition));
        }

        public If If(bool value)
        {
            return If(() => value);
        }

        public If If()
        {
            return If(true);
        }

        public If If(IPromise<bool> value)
        {
            return If(() => value);
        }

        public If If(NullableResult<bool> value)
        {
            return If(() => value);
        }

        public If If(IPromise<NullableResult<bool>> value)
        {
            return If(() => value);
        }

        public If If(ControlValue<bool> value)
        {
            return If(() => value);
        }

        public If If(IPromise<ControlValue<bool>> value)
        {
            return If(() => value);
        }
    }
}
