using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Control
{
    public class Control : WhileAble<While>
    {
        internal Control(PromiseFactory factory)
            : base(factory)
        {

        }

        public DoWhileControlValue<T> Do<T>(Func<IPromise<ControlValue<T>>> body)
        {
            return new DoWhileControlValue<T>(Factory, body);
        }

        public DoWhileControlValue<T> Do<T>(Func<ControlValue<T>> body)
        {
            return Do(Factory.Canonical(body));
        }

        public DoWhileControlValue<T> Do<T>(IPromise<ControlValue<T>> body)
        {
            return Do(() => body);
        }

        public DoWhileControlValue<T> Do<T>(ControlValue<T> body)
        {
            return Do(() => body);
        }

        public DoWhileNullableResult<T> Do<T>(Func<IPromise<NullableResult<T>>> body)
        {
            return new DoWhileNullableResult<T>(Factory, Factory.Canonical(body));
        }

        public DoWhileNullableResult<T> Do<T>(Func<NullableResult<T>> body)
        {
            return new DoWhileNullableResult<T>(Factory, Factory.Canonical(body));
        }

        public DoWhileNullableResult<T> Do<T>(Func<IPromise<T>> body)
        {
            return new DoWhileNullableResult<T>(Factory, Factory.Canonical(body));
        }

        public DoWhileNullableResult<T> Do<T>(Func<T> body)
        {
            return new DoWhileNullableResult<T>(Factory, Factory.Canonical(body));
        }

        public DoWhileNullableResult<T> Do<T>(IPromise<NullableResult<T>> body)
        {
            return Do(() => body);
        }

        public DoWhileNullableResult<T> Do<T>(NullableResult<T> body)
        {
            return Do(() => body);
        }

        public DoWhileNullableResult<T> Do<T>(IPromise<T> body)
        {
            return Do(() => body);
        }

        public DoWhileNullableResult<T> Do<T>(T body)
        {
            return Do(() => body);
        }

        public DoWhileVoid Do(Func<IPromise> body)
        {
            return new DoWhileVoid(Factory, Factory.Canonical(body));
        }

        public DoWhileVoid Do(Action body)
        {
            return new DoWhileVoid(Factory, Factory.Canonical(body));
        }

        public DoWhileVoid Do(IPromise body)
        {
            return Do(() => body);
        }

        public DoWhileVoid Do()
        {
            return Do(() => { });
        }

        public DoWhileControlState Do(Func<IPromise<ControlState>> body)
        {
            return new DoWhileControlState(Factory, Factory.Canonical(body));
        }

        public DoWhileControlState Do(Func<ControlState> body)
        {
            return new DoWhileControlState(Factory, Factory.Canonical(body));
        }

        public DoWhileControlState Do(IPromise<ControlState> value)
        {
            return Do(() => value);
        }

        public DoWhileControlState Do(ControlState value)
        {
            return Do(() => value);
        }

        public override While While(Func<IPromise<ControlValue<bool>>> condition)
        {
            return new While(Factory, condition);
        }

        public ForWithSeed<T> For<T>(Func<IPromise<ControlValue<T>>> seed)
        {
            return new ForWithSeed<T>(Factory, seed);
        }

        public ForWithSeed<T> For<T>(Func<ControlValue<T>> seed)
        {
            return For(Factory.Canonical(seed));
        }

        public ForWithSeed<T> For<T>(Func<IPromise<NullableResult<T>>> seed)
        {
            return For(Factory.Canonical(seed));
        }

        public ForWithSeed<T> For<T>(Func<NullableResult<T>> seed)
        {
            return For(Factory.Canonical(seed));
        }

        public ForWithSeed<T> For<T>(Func<IPromise<T>> seed)
        {
            return For(Factory.Canonical(seed));
        }

        public ForWithSeed<T> For<T>(Func<T> seed)
        {
            return For(Factory.Canonical(seed));
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
            return new If(Factory, condition);
        }

        public If If(Func<ControlValue<bool>> condition)
        {
            return If(Factory.Canonical(condition));
        }

        public If If(Func<IPromise<NullableResult<bool>>> condition)
        {
            return If(Factory.Canonical(condition));
        }

        public If If(Func<NullableResult<bool>> condition)
        {
            return If(Factory.Canonical(condition));
        }

        public If If(Func<IPromise<bool>> condition)
        {
            return If(Factory.Canonical(condition));
        }

        public If If(Func<bool> condition)
        {
            return If(Factory.Canonical(condition));
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
