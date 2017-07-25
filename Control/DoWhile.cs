using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Control
{
    public class DoWhile
    {
        private PromiseFactory _factory;
        private Func<IPromise<bool>> _condition;

        public IPromise While(Func<IPromise> body)
        {
            return While(() =>
            {
                var next = body();

                if (next == null)
                {
                    return null;
                }

                return next.Then(() => ControlState.Continue);
            }).Cast();
        }

        public IPromise While(Action body)
        {
            return While(() =>
            {
                body();

                return _factory.Value();
            });
        }

        public IPromise<ControlState> While(Func<IPromise<ControlState>> body)
        {
            return While(() =>
            {
                var next = body();

                if (next == null)
                {
                    return null;
                }

                return next.Then(result => new ControlValue<object>(result));
            }).Then(x => x.State);
        }

        public IPromise<ControlState> While(Func<ControlState> body)
        {
            return While(() => _factory.Value(body()));
        }

        public IPromise<ControlValue<T>> While<T>(Func<ControlValue<T>> body)
        {
            return While(() => _factory.Value(body()));
        }

        public IPromise<ControlValue<T>> While<T>(Func<IPromise<ControlValue<T>>> body)
        {
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

                if (result.State == ControlState.Return)
                {
                    return _factory.Value(result);
                }

                if (result.State == ControlState.Continue)
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

                        return While(body);
                    });
                }

                throw new InvalidOperationException();
            });
        }
    }
}
