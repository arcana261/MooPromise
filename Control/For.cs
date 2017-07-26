using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Control
{
    public class For<T>
    {
        private PromiseFactory _factory;
        private T _current;
        private Func<T, IPromise<bool>> _condition;
        private Func<T, IPromise<T>> _iterator;

        internal For(PromiseFactory factory, T current, Func<T, IPromise<bool>> condition, Func<T, IPromise<T>> iterator)
        {
            this._factory = factory;
            this._current = current;
            this._condition = condition;
            this._iterator = iterator;
        }

        public PromiseFactory Factory
        {
            get
            {
                return _factory;
            }
        }

        public IPromise Do(Func<T, IPromise> body)
        {
            return Do(x =>
            {
                var next = body(x);

                if (next == null)
                {
                    return null;
                }

                return next.Then(() => ControlState.Continue);
            }).Cast();
        }

        public IPromise Do(Action<T> body)
        {
            return Do(x =>
            {
                body(x);

                return _factory.Value();
            });
        }

        public IPromise<ControlState> Do(Func<T, IPromise<ControlState>> body)
        {
            return Do<object>(x =>
            {
                var next = body(x);

                if (next == null)
                {
                    return null;
                }

                return next.Then(state => new ControlValue<object>(state));
            }).Then(result => result.State);
        }

        public IPromise<ControlState> Do(Func<T, ControlState> body)
        {
            return Do(x => _factory.Value(body(x)));
        }

        public IPromise<ControlValue<E>> Do<E>(Func<T, ControlValue<E>> body)
        {
            return Do(x => _factory.Value(body(x)));
        }

        public IPromise<ControlValue<E>> Do<E>(Func<T, IPromise<ControlValue<E>>> body)
        {
            var nextCondition = _condition(_current);

            if (nextCondition == null)
            {
                return _factory.Value(ControlValue<E>.Continue);
            }

            return nextCondition.Then(check =>
            {
                if (!check)
                {
                    return _factory.Value(ControlValue<E>.Continue);
                }

                var next = body(_current);

                if (next == null)
                {
                    return _factory.Value(ControlValue<E>.Continue);
                }

                return next.Then(result =>
                {
                    if (result == null || result.State == ControlState.Break)
                    {
                        return _factory.Value(ControlValue<E>.Continue);
                    }

                    if (result.State == ControlState.Return)
                    {
                        return _factory.Value(result);
                    }

                    if (result.State == ControlState.Continue)
                    {
                        var nextIterator = _iterator(_current);

                        if (nextIterator == null)
                        {
                            return _factory.Value(ControlValue<E>.Continue);
                        }

                        return nextIterator.Then(nextValue => (new For<T>(_factory, nextValue, _condition, _iterator)).Do(body));
                    }

                    throw new InvalidOperationException();
                });
            });
        }

        public IPromise Do(Action body)
        {
            return Do(x => body());
        }

        public IPromise Do(Func<IPromise> body)
        {
            return Do(x => body());
        }

        public IPromise<ControlState> Do(Func<ControlState> body)
        {
            return Do(x => body());
        }

        public IPromise<ControlState> Do(Func<IPromise<ControlState>> body)
        {
            return Do(x => body());
        }

        public IPromise<ControlValue<E>> Do<E>(Func<ControlValue<E>> body)
        {
            return Do(x => body());
        }

        public IPromise<ControlValue<E>> Do<E>(Func<IPromise<ControlValue<E>>> body)
        {
            return Do(x => body());
        }

        public IPromise<E> Do<E>(Func<T, IPromise<E>> body)
        {
            return Do(x =>
            {
                var next = body(x);

                if (next == null)
                {
                    return null;
                }

                return next.Then(result => new ControlValue<E>(result));
            }).Then(result => result.Value);
        }

        public IPromise<E> Do<E>(Func<IPromise<E>> body)
        {
            return Do(x => body());
        }

        public IPromise<E> Do<E>(Func<T, E> body)
        {
            return Do(x => _factory.Value(body(x)));
        }

        public IPromise<E> Do<E>(Func<E> body)
        {
            return Do(x => body());
        }
    }
}
