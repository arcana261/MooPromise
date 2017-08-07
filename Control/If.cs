using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Control
{
    public class If : DoAble
    {
        private Func<IPromise<ControlValue<bool>>> _condition;

        internal If(PromiseFactory factory, Func<IPromise<ControlValue<bool>>> condition)
            : base(factory)
        {
            this._condition = condition;
        }

        public override IPromise<ControlValue<T>> Do<T>(Func<IPromise<ControlValue<T>>> body)
        {
            return Factory.SafeThen(_condition, check =>
            {
                if (check == null || check.State != ControlState.Return || !check.HasValue || !check.Value)
                {
                    return Factory.Value(ControlValue<T>.Next);
                }

                return Factory.SafeThen(body, result =>
                {
                    if (result == null)
                    {
                        return ControlValue<T>.Break;
                    }

                    return result;
                });
            }); 
        }

        public If Else
        {
            get
            {
                return ElseIf(true);
            }
        }

        public If ElseIf(Func<IPromise<ControlValue<bool>>> newCondition)
        {
            return new If(Factory, () => Factory.SafeThen(_condition, check =>
            {
                if (check == null || check.State != ControlState.Return || !check.HasValue)
                {
                    return Factory.Value(ControlValue<bool>.Break);
                }

                if (check.Value)
                {
                    return Factory.Value(ControlValue<bool>.Return(false));
                }

                return newCondition();
            }));
        }

        public If ElseIf(Func<ControlValue<bool>> newCondition)
        {
            return ElseIf(Factory.Canonical(newCondition));
        }

        public If ElseIf(Func<IPromise<NullableResult<bool>>> newCondition)
        {
            return ElseIf(Factory.Canonical(newCondition));
        }

        public If ElseIf(Func<NullableResult<bool>> newCondition)
        {
            return ElseIf(Factory.Canonical(newCondition));
        }

        public If ElseIf(Func<IPromise<bool>> newCondition)
        {
            return ElseIf(Factory.Canonical(newCondition));
        }

        public If ElseIf(Func<bool> newCondition)
        {
            return ElseIf(Factory.Canonical(newCondition));
        }

        public If ElseIf(bool value)
        {
            return ElseIf(() => value);
        }

        public If ElseIf()
        {
            return ElseIf(true);
        }

        public If ElseIf(ControlValue<bool> value)
        {
            return ElseIf(() => value);
        }

        public If ElseIf(NullableResult<bool> value)
        {
            return ElseIf(() => value);
        }

        public If ElseIf(IPromise<ControlValue<bool>> value)
        {
            return ElseIf(() => value);
        }

        public If ElseIf(IPromise<NullableResult<bool>> value)
        {
            return ElseIf(() => value);
        }

        public If ElseIf(IPromise<bool> value)
        {
            return ElseIf(() => value);
        }
    }
}
