using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Control
{
    public class If
    {
        private PromiseFactory _factory;
        private Func<IPromise<bool>> _condition;

        internal If(PromiseFactory factory, Func<IPromise<bool>> condition)
        {
            this._factory = factory;
            this._condition = condition;
        }

        internal If(PromiseFactory factory, Func<bool> condition)
            : this(factory, () => factory.Value(condition()))
        {

        }

        public IPromise Do(Action body)
        {
            return Do(() =>
            {
                body();

                return _factory.Value();
            });
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

        public IPromise<ControlState> Do(Func<ControlState> body)
        {
            return Do(() => _factory.Value(body()));
        }

        public IPromise<ControlState> Do(Func<IPromise<ControlState>> body)
        {
            return Do(() =>
            {
                var next = body();

                if (next == null)
                {
                    return null;
                }

                return next.Then(result => new ControlValue<object>(result));
            }).Then(result => result.State);
        }

        public IPromise<ControlValue<T>> Do<T>(Func<ControlValue<T>> body)
        {
            return Do(() => _factory.Value(body()));
        }

        public IPromise<ControlValue<T>> Do<T>(Func<IPromise<ControlValue<T>>> body)
        {
            var nextCondition = _condition();

            if (_condition == null)
            {
                return _factory.Value(ControlValue<T>.Continue);
            }

            return nextCondition.Then(check =>
            {
                if (!check)
                {
                    return _factory.Value(ControlValue<T>.Continue);
                }

                return body();
            });
        }

        public If Else
        {
            get
            {
                return ElseIf(() => true);
            }
        }

        public If ElseIf(Func<bool> newCondition)
        {
            return ElseIf(() => _factory.Value(newCondition()));
        }

        public If ElseIf(Func<IPromise<bool>> newCondition)
        {
            return new If(_factory, () =>
            {
                var next = _condition();

                if (next == null)
                {
                    return null;
                }

                return next.Then(result =>
                {
                    if (result)
                    {
                        return _factory.Value(false);
                    }

                    return newCondition();
                });
            });
        }
    }
}
