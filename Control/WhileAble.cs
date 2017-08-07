using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Control
{
    public abstract class WhileAble<TRet>
    {
        private PromiseFactory _factory;

        public WhileAble(PromiseFactory factory)
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

        public abstract TRet While(Func<IPromise<ControlValue<bool>>> condition);

        public TRet While(Func<ControlValue<bool>> condition)
        {
            return While(Factory.Canonical(condition));
        }

        public TRet While(Func<IPromise<NullableResult<bool>>> condition)
        {
            return While(Factory.Canonical(condition));
        }

        public TRet While(Func<NullableResult<bool>> condition)
        {
            return While(Factory.Canonical(condition));
        }

        public TRet While(Func<IPromise<bool>> condition)
        {
            return While(Factory.Canonical(condition));
        }

        public TRet While(Func<bool> condition)
        {
            return While(Factory.Canonical(condition));
        }

        public TRet While(bool value)
        {
            return While(() => value);
        }

        public TRet While()
        {
            return While(true);
        }

        public TRet While(ControlValue<bool> value)
        {
            return While(() => value);
        }

        public TRet While(IPromise<ControlValue<bool>> value)
        {
            return While(() => value);
        }

        public TRet While(NullableResult<bool> value)
        {
            return While(() => value);
        }

        public TRet While(IPromise<NullableResult<bool>> value)
        {
            return While(() => value);
        }

        public TRet While(IPromise<bool> value)
        {
            return While(() => value);
        }
    }
}
