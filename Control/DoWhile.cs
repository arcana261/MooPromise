using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Control
{
    public class DoWhileResult<T>
    {
        private DoWhileControlValue<T> _body;

        internal DoWhileResult(PromiseFactory factory, Func<IPromise<T>> body)
        {
            _body = new DoWhileControlValue<T>(factory, () =>
            {
                var next = body();

                if (next == null)
                {
                    return null;
                }

                return next.Then(result => new ControlValue<T>(result));
            });
        }

        public PromiseFactory Factory
        {
            get
            {
                return _body.Factory;
            }
        }

        public IPromise<T> While(Func<IPromise<bool>> condition)
        {
            return _body.While(condition).Then(result => result.Value);
        }

        public IPromise<T> While(Func<bool> condition)
        {
            return While(() => _body.Factory.Value(condition()));
        }
    }

    public class DoWhileVoid
    {
        private DoWhileControlState _body;

        internal DoWhileVoid(PromiseFactory factory, Func<IPromise> body)
        {
            _body = new DoWhileControlState(factory, () =>
            {
                var next = body();

                if (next == null)
                {
                    return null;
                }

                return next.Then(() => ControlState.Continue);
            });
        }

        public PromiseFactory Factory
        {
            get
            {
                return _body.Factory;
            }
        }

        public IPromise While(Func<IPromise<bool>> condition)
        {
            return _body.While(condition).Cast();
        }

        public IPromise While(Func<bool> condition)
        {
            return While(() => _body.Factory.Value(condition()));
        }
    }

    public class DoWhileControlState
    {
        private DoWhileControlValue<object> _body;

        internal DoWhileControlState(PromiseFactory factory, Func<IPromise<ControlState>> body)
        {
            _body = new DoWhileControlValue<object>(factory, () =>
            {
                var next = body();

                if (next == null)
                {
                    return null;
                }

                return next.Then(result => new ControlValue<object>(result));
            });
        }

        public PromiseFactory Factory
        {
            get
            {
                return _body.Factory;
            }
        }

        public IPromise<ControlState> While(Func<IPromise<bool>> condition)
        {
            return _body.While(condition).Then(result => result.State);
        }

        public IPromise<ControlState> While(Func<bool> condition)
        {
            return While(() => _body.Factory.Value(condition()));
        }
    }

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

        public IPromise<ControlValue<T>> While(Func<IPromise<bool>> condition)
        {
            var next = _body();

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
                    var nextCondition = condition();

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

                        return While(condition);
                    });
                }

                throw new InvalidOperationException();
            });
        }

        public IPromise<ControlValue<T>> While(Func<bool> condition)
        {
            return While(() => _factory.Value(condition()));
        }
    }
}
