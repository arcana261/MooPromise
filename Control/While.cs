using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Control
{
    public class While : DoAble
    {
        private Func<IPromise<ControlValue<bool>>> _condition;

        internal While(PromiseFactory factory, Func<IPromise<ControlValue<bool>>> condition)
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
                    if (result == null || result.State == ControlState.Break)
                    {
                        return Factory.Value(ControlValue<T>.Next);
                    }

                    if (result.State == ControlState.Return)
                    {
                        return Factory.Value(result);
                    }

                    return Do(body);
                });
            });
        }
    }
}
