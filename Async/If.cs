using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Async
{
    public class If<T>
    {
        private Scope<T> _owner;
        private Action<Scope<bool>> _condition;

        internal If(Scope<T> owner, Action<Scope<bool>> condition)
        {
            this._owner = owner;
            this._condition = condition;
        }

        public If<T> Do(Action<Scope<T>> block)
        {
            _owner.Run(() => _owner.Factory.Control.If(() => _owner.BeginImmediately<bool>(_condition).Finish()).Do(() => _owner.BeginImmediately<T>(block).Finish()));

            return this;
        }

        public If<T> Do(Action block)
        {
            return Do(scope => block());
        }

        public If<T> ElseIf(Action<Scope<bool>> condition)
        {
            return new If<T>(_owner, block =>
            {
                block.Return(() => _owner.BeginImmediately(_condition).Finish().Then(result =>
                {
                    if (result == null || result.State != ControlState.Return || !result.HasValue)
                    {
                        return _owner.Factory.Value(result);
                    }

                    if (result.Value)
                    {
                        return _owner.Factory.Value(ControlValue<bool>.Return(false));
                    }

                    return _owner.BeginImmediately(condition).Finish();
                }));
            });
        }

        public If<T> ElseIf(Func<IPromise<ControlValue<bool>>> condition)
        {
            return ElseIf(block => block.Return(condition));
        }

        public If<T> ElseIf(Func<ControlValue<bool>> condition)
        {
            return ElseIf(block => block.Return(condition));
        }

        public If<T> ElseIf(Func<IPromise<NullableResult<bool>>> condition)
        {
            return ElseIf(block => block.Return(condition));
        }

        public If<T> ElseIf(Func<NullableResult<bool>> condition)
        {
            return ElseIf(block => block.Return(condition));
        }

        public If<T> ElseIf(Func<IPromise<bool>> condition)
        {
            return ElseIf(block => block.Return(condition));
        }

        public If<T> ElseIf(Func<bool> condition)
        {
            return ElseIf(block => block.Return(condition));
        }

        public If<T> ElseIf(bool condition)
        {
            return ElseIf(() => condition);
        }

        public If<T> ElseIf()
        {
            return ElseIf(true);
        }

        public If<T> ElseIf(NullableResult<bool> condition)
        {
            return ElseIf(() => condition);
        }

        public If<T> ElseIf(ControlValue<bool> condition)
        {
            return ElseIf(() => condition);
        }

        public If<T> Else
        {
            get
            {
                return ElseIf(block => block.Return(true));
            }
        }
    }
}
