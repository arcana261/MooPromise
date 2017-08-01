using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Control
{
    public class DoWhileControlValue<T>
    {
        private PromiseFactory _factory;
        private Func<IPromise<ControlValue<T>>> _body;

        internal DoWhileControlValue(PromiseFactory factory, Func<IPromise<ControlValue<T>>> body)
        {
            this._factory = factory;
            this._body = body;
        }

        public PromiseFactory Factory
        {
            get
            {
                return _factory;
            }
        }

        public IPromise<ControlValue<T>> While(Func<IPromise<ControlValue<bool>>> condition)
        {
            var f = new For<bool>(_factory, true, _factory.Canonical<bool, bool>(x => x), _factory.Canonical<bool, bool>(x => condition()));
            return f.Do(_body);
        }

        public IPromise<ControlValue<T>> While(Func<ControlValue<bool>> condition)
        {
            return While(_factory.Canonical(condition));
        }

        public IPromise<ControlValue<T>> While(Func<IPromise<NullableResult<bool>>> condition)
        {
            return While(_factory.Canonical(condition));
        }

        public IPromise<ControlValue<T>> While(Func<NullableResult<bool>> condition)
        {
            return While(_factory.Canonical(condition));
        }

        public IPromise<ControlValue<T>> While(Func<IPromise<bool>> condition)
        {
            return While(_factory.Canonical(condition));
        }

        public IPromise<ControlValue<T>> While(Func<bool> condition)
        {
            return While(_factory.Canonical(condition));
        }

        public IPromise<ControlValue<T>> While(bool condition)
        {
            return While(() => condition);
        }

        public IPromise<ControlValue<T>> While()
        {
            return While(true);
        }
    }

    public class DoWhileNullableResult<T>
    {
        private DoWhileControlValue<T> _while;

        internal DoWhileNullableResult(PromiseFactory factory, Func<IPromise<ControlValue<T>>> body)
        {
            _while = new DoWhileControlValue<T>(factory, body);
        }

        public PromiseFactory Factory
        {
            get
            {
                return _while.Factory;
            }
        }

        public IPromise<NullableResult<T>> While(Func<IPromise<ControlValue<bool>>> condition)
        {
            return _while.While(condition).ToNullableResult(_while.Factory);
        }

        public IPromise<NullableResult<T>> While(Func<ControlValue<bool>> condition)
        {
            return _while.While(condition).ToNullableResult(_while.Factory);
        }

        public IPromise<NullableResult<T>> While(Func<IPromise<NullableResult<bool>>> condition)
        {
            return _while.While(condition).ToNullableResult(_while.Factory);
        }

        public IPromise<NullableResult<T>> While(Func<NullableResult<bool>> condition)
        {
            return _while.While(condition).ToNullableResult(_while.Factory);
        }

        public IPromise<NullableResult<T>> While(Func<IPromise<bool>> condition)
        {
            return _while.While(condition).ToNullableResult(_while.Factory);
        }

        public IPromise<NullableResult<T>> While(Func<bool> condition)
        {
            return _while.While(condition).ToNullableResult(_while.Factory);
        }

        public IPromise<NullableResult<T>> While(bool condition)
        {
            return _while.While(condition).ToNullableResult(_while.Factory);
        }

        public IPromise<NullableResult<T>> While()
        {
            return _while.While().ToNullableResult(_while.Factory);
        }
    }

    public class DoWhileVoid
    {
        private DoWhileControlValue<object> _while;

        internal DoWhileVoid(PromiseFactory factory, Func<IPromise<ControlValue<object>>> body)
        {
            _while = new DoWhileControlValue<object>(factory, body);
        }

        public PromiseFactory Factory
        {
            get
            {
                return _while.Factory;
            }
        }

        public IPromise While(Func<IPromise<ControlValue<bool>>> condition)
        {
            return _while.While(condition).Cast();
        }

        public IPromise While(Func<ControlValue<bool>> condition)
        {
            return _while.While(condition).Cast();
        }

        public IPromise While(Func<IPromise<NullableResult<bool>>> condition)
        {
            return _while.While(condition).Cast();
        }

        public IPromise While(Func<NullableResult<bool>> condition)
        {
            return _while.While(condition).Cast();
        }

        public IPromise While(Func<IPromise<bool>> condition)
        {
            return _while.While(condition).Cast();
        }

        public IPromise While(Func<bool> condition)
        {
            return _while.While(condition).Cast();
        }

        public IPromise While(bool condition)
        {
            return _while.While(condition).Cast();
        }

        public IPromise While()
        {
            return _while.While().Cast();
        }
    }
}
