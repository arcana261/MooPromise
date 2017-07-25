using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Control
{
    public class While
    {
        private PromiseFactory _factory;
        private Func<IPromise<bool>> _condition;

        internal While(PromiseFactory factory, Func<IPromise<bool>> condition)
        {
            this._factory = factory;
            this._condition = condition;
        }

        internal While(PromiseFactory factory, Func<bool> condition)
            : this(factory, () => factory.Value(condition()))
        {

        }

        public IPromise Do(Func<IPromise> body)
        {
            return Do(() =>
            {
                var next = body();

                if (next == null)
                {
                    return null;
                }

                return next.Then(() => ControlState.Continue);
            }).Cast();
        }

        public IPromise Do(Action body)
        {
            return Do(() =>
            {
                body();
                return _factory.Value();
            });
        }

        public IPromise<ControlState> Do(Func<IPromise<ControlState>> body)
        {
            return Do<object>(() =>
            {
                var next = body();

                if (next == null)
                {
                    return null;
                }

                return next.Then(result =>
                {
                    return new ControlValue<object>(result);
                });
            }).Then(result => result.State);
        }

        public IPromise<ControlState> Do(Func<ControlState> body)
        {
            return Do(() => _factory.Value(body()));
        }

        public IPromise<ControlValue<T>> Do<T>(Func<IPromise<ControlValue<T>>> body)
        {
            var nextCondition = _condition();

            if (nextCondition == null)
            {
                return _factory.Value(ControlValue<T>.Continue);
            }

            return nextCondition.Then(check =>
            {
                if (!check)
                {
                    return _factory.Value(ControlValue<T>.Continue);
                }

                var next = body();

                if (next == null)
                {
                    return _factory.Value(ControlValue<T>.Continue);
                }

                return next.Then(result =>
                {
                    if (result == null || result.State == ControlState.Break)
                    {
                        return _factory.Value(ControlValue<T>.Continue);

                    }

                    if (result.State == ControlState.Continue)
                    {
                        return Do(body);
                    }

                    if (result.State == ControlState.Return)
                    {
                        return _factory.Value(result);
                    }

                    throw new InvalidOperationException();
                });
            });
        }

        public IPromise<ControlValue<T>> Do<T>(Func<ControlValue<T>> body)
        {
            return Do(() => _factory.Value(body()));
        }
    }
}
