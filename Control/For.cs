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
        private Func<T, IPromise<ControlValue<bool>>> _condition;
        private Func<T, IPromise<ControlValue<T>>> _iterator;

        internal For(PromiseFactory factory, T current, Func<T, IPromise<ControlValue<bool>>> condition, Func<T, IPromise<ControlValue<T>>> iterator)
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

                return next.Then(() => ControlState.Next);
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
            return Do(x =>
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
                return _factory.Value(ControlValue<E>.Next);
            }

            return nextCondition.Then(check =>
            {
                if (check == null || check.State != ControlState.Return || !check.HasValue || !check.Value)
                {
                    return _factory.Value(ControlValue<E>.Next);
                }

                var next = body(_current);

                if (next == null)
                {
                    return _factory.Value(ControlValue<E>.Next);
                }

                return next.Then(result =>
                {
                    if (result == null || result.State == ControlState.Break)
                    {
                        return _factory.Value(ControlValue<E>.Next);
                    }

                    if (result.State == ControlState.Return)
                    {
                        return _factory.Value(result);
                    }

                    var nextIterator = _iterator(_current);

                    if (nextIterator == null)
                    {
                        return _factory.Value(ControlValue<E>.Next);
                    }

                    return nextIterator.Then(nextValue =>
                    {
                        if (nextValue == null || nextValue.State != ControlState.Return || !nextValue.HasValue)
                        {
                            return _factory.Value(ControlValue<E>.Next);
                        }

                        return (new For<T>(_factory, nextValue.Value, _condition, _iterator)).Do(body);
                    });
                });
            });
        }

        public IPromise<NullableResult<E>> Do<E>(Func<T, IPromise<NullableResult<E>>> body)
        {
            return Do(x =>
            {
                var next = body(x);

                if (next == null)
                {
                    return null;
                }

                return next.Then(result =>
                {
                    if (result == null || !result.HasResult)
                    {
                        return new ControlValue<NullableResult<E>>(ControlState.Next);
                    }

                    return new ControlValue<NullableResult<E>>(result);
                });
            }).Then(result =>
            {
                if (result == null || !result.HasValue)
                {
                    return new NullableResult<E>();
                }

                return result.Value;
            });
        }

        public IPromise<NullableResult<E>> Do<E>(Func<T, NullableResult<E>> body)
        {
            return Do(x => _factory.Value(body(x)));
        }

        public IPromise<NullableResult<E>> Do<E>(Func<T, IPromise<E>> body)
        {
            return Do(x =>
            {
                var next = body(x);

                if (next == null)
                {
                    return null;
                }

                return next.Then(result => new NullableResult<E>(result));
            });
        }

        public IPromise<NullableResult<E>> Do<E>(Func<T, E> body)
        {
            return Do(x => _factory.Value(body(x)));
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
        
        public IPromise<NullableResult<E>> Do<E>(Func<IPromise<NullableResult<E>>> body)
        {
            return Do(x => body());
        }

        public IPromise<NullableResult<E>> Do<E>(Func<NullableResult<E>> body)
        {
            return Do(x => body());
        }

        public IPromise<NullableResult<E>> Do<E>(Func<IPromise<E>> body)
        {
            return Do(x => body());
        }

        public IPromise<NullableResult<E>> Do<E>(Func<E> body)
        {
            return Do(x => body());
        }
    }
}
