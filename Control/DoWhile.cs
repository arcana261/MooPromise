using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Control
{
    public class DoWhileControlValue<T> : WhileAble<IPromise<ControlValue<T>>>
    {
        private Func<IPromise<ControlValue<T>>> _body;

        internal DoWhileControlValue(PromiseFactory factory, Func<IPromise<ControlValue<T>>> body)
            : base(factory)
        {
            this._body = body;
        }

        public override IPromise<ControlValue<T>> While(Func<IPromise<ControlValue<bool>>> condition)
        {
            var f = new For<bool>(Factory, Factory.Canonical(() => true), Factory.Canonical<bool, bool>(x => x), Factory.Canonical<bool, bool>(x => condition()));
            return f.Do(_body);
        }
    }

    public class DoWhileNullableResult<T> : WhileAble<IPromise<NullableResult<T>>>
    {
        private DoWhileControlValue<T> _while;

        internal DoWhileNullableResult(PromiseFactory factory, Func<IPromise<ControlValue<T>>> body)
            : base(factory)
        {
            _while = new DoWhileControlValue<T>(factory, body);
        }

        public override IPromise<NullableResult<T>> While(Func<IPromise<ControlValue<bool>>> condition)
        {
            return _while.While(condition).ToNullableResult(_while.Factory);
        }
    }

    public class DoWhileVoid : WhileAble<IPromise>
    {
        private DoWhileControlValue<object> _while;

        internal DoWhileVoid(PromiseFactory factory, Func<IPromise<ControlValue<object>>> body)
            : base(factory)
        {
            _while = new DoWhileControlValue<object>(factory, body);
        }

        public override IPromise While(Func<IPromise<ControlValue<bool>>> condition)
        {
            return _while.While(condition).Cast();
        }
    }

    public class DoWhileControlState : WhileAble<IPromise<ControlState>>
    {
        private DoWhileControlValue<object> _while;

        internal DoWhileControlState(PromiseFactory factory, Func<IPromise<ControlValue<object>>> body)
            : base(factory)
        {
            _while = new DoWhileControlValue<object>(factory, body);
        }

        public override IPromise<ControlState> While(Func<IPromise<ControlValue<bool>>> condition)
        {
            return _while.While(condition).ToControlState(_while.Factory);
        }
    }
}
