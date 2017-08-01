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

        private T GetCurrent()
        {
            lock (this)
            {
                return _current;
            }
        }

        private void SetCurrent(T value)
        {
            lock (this)
            {
                _current = value;
            }
        }

        public IPromise<ControlValue<E>> Do<E>(Func<T, IPromise<ControlValue<E>>> body)
        {
            var w = new While(_factory, () => _condition(GetCurrent()));

            return w.Do(() => _factory.SafeThen(body(GetCurrent()), result =>
            {
                if (result == null || result.State == ControlState.Break)
                {
                    return _factory.Value(ControlValue<E>.Next);
                }

                if (result.State == ControlState.Return)
                {
                    return _factory.Value(result);
                }

                return _factory.SafeThen(_iterator(GetCurrent()), next =>
                {
                    if (next == null || next.State != ControlState.Return || !next.HasValue)
                    {
                        return ControlValue<E>.Break;
                    }

                    SetCurrent(next.Value);
                    return ControlValue<E>.Continue;
                });
            }));
        }

        public IPromise<ControlValue<E>> Do<E>(Func<T, ControlValue<E>> body)
        {
            return Do(_factory.Canonical(body));
        }

        public IPromise<NullableResult<E>> Do<E>(Func<T, IPromise<NullableResult<E>>> body)
        {
            return Do(_factory.Canonical(body)).ToNullableResult(_factory);
        }

        public IPromise<NullableResult<E>> Do<E>(Func<T, NullableResult<E>> body)
        {
            return Do(_factory.Canonical(body)).ToNullableResult(_factory);
        }

        public IPromise<NullableResult<E>> Do<E>(Func<T, IPromise<E>> body)
        {
            return Do(_factory.Canonical(body)).ToNullableResult(_factory);
        }

        public IPromise<NullableResult<E>> Do<E>(Func<T, E> body)
        {
            return Do(_factory.Canonical(body)).ToNullableResult(_factory);
        }

        public IPromise Do(Func<T, IPromise> body)
        {
            return Do(_factory.Canonical(body)).Cast();
        }

        public IPromise Do(Action<T> body)
        {
            return Do(_factory.Canonical(body)).Cast();
        }

        public IPromise Do(Func<T, IPromise<ControlState>> body)
        {
            return Do(_factory.Canonical(body)).Cast();
        }

        public IPromise Do(Func<T, ControlState> body)
        {
            return Do(_factory.Canonical(body)).Cast();
        }

        public IPromise<ControlValue<E>> Do<E>(Func<IPromise<ControlValue<E>>> body)
        {
            return Do(x => body());
        }

        public IPromise<ControlValue<E>> Do<E>(Func<ControlValue<E>> body)
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

        public IPromise Do(Func<IPromise> body)
        {
            return Do(x => body());
        }

        public IPromise Do(Action body)
        {
            return Do(x => body());
        }

        public IPromise Do(Func<IPromise<ControlState>> body)
        {
            return Do(x => body());
        }

        public IPromise Do(Func<ControlState> body)
        {
            return Do(x => body());
        }
    }

    public class ForWithSeedAndCondition<T>
    {
        private PromiseFactory _factory;
        private T _seed;
        private Func<T, IPromise<ControlValue<bool>>> _condition;

        internal ForWithSeedAndCondition(PromiseFactory factory, T seed, Func<T, IPromise<ControlValue<bool>>> condition)
        {
            this._factory = factory;
            this._seed = seed;
            this._condition = condition;
        }

        public For<T> Iterate(Func<T, IPromise<ControlValue<T>>> iterator)
        {
            return new For<T>(_factory, _seed, _condition, iterator);
        }

        public For<T> Iterate(Func<T, ControlValue<T>> iterator)
        {
            return Iterate(_factory.Canonical(iterator));
        }

        public For<T> Iterate(Func<T, IPromise<NullableResult<T>>> iterator)
        {
            return Iterate(_factory.Canonical(iterator));
        }

        public For<T> Iterate(Func<T, NullableResult<T>> iterator)
        {
            return Iterate(_factory.Canonical(iterator));
        }

        public For<T> Iterate(Func<T, IPromise<T>> iterator)
        {
            return Iterate(_factory.Canonical(iterator));
        }

        public For<T> Iterate(Func<T, T> iterator)
        {
            return Iterate(_factory.Canonical(iterator));
        }

        public For<T> Iterate(Func<IPromise<ControlValue<T>>> iterator)
        {
            return Iterate(x => iterator());
        }

        public For<T> Iterate(Func<ControlValue<T>> iterator)
        {
            return Iterate(x => iterator());
        }

        public For<T> Iterate(Func<IPromise<NullableResult<T>>> iterator)
        {
            return Iterate(x => iterator());
        }

        public For<T> Iterate(Func<NullableResult<T>> iterator)
        {
            return Iterate(x => iterator());
        }

        public For<T> Iterate(Func<IPromise<T>> iterator)
        {
            return Iterate(x => iterator());
        }

        public For<T> Iterate(Func<T> iterator)
        {
            return Iterate(x => iterator());
        }

        public For<T> Iterate(T value)
        {
            return Iterate(() => value);
        }
    }

    public class ForWithSeed<T>
    {
        private PromiseFactory _factory;
        private T _seed;

        internal ForWithSeed(PromiseFactory factory, T seed)
        {
            this._factory = factory;
            this._seed = seed;
        }

        public PromiseFactory Factory
        {
            get
            {
                return _factory;
            }
        }

        public ForWithSeedAndCondition<T> While(Func<T, IPromise<ControlValue<bool>>> condition)
        {
            return new ForWithSeedAndCondition<T>(_factory, _seed, condition);
        }

        public ForWithSeedAndCondition<T> While(Func<T, ControlValue<bool>> condition)
        {
            return While(_factory.Canonical(condition));
        }

        public ForWithSeedAndCondition<T> While(Func<T, IPromise<NullableResult<bool>>> condition)
        {
            return While(_factory.Canonical(condition));
        }

        public ForWithSeedAndCondition<T> While(Func<T, NullableResult<bool>> condition)
        {
            return While(_factory.Canonical(condition));
        }

        public ForWithSeedAndCondition<T> While(Func<T, IPromise<bool>> condition)
        {
            return While(_factory.Canonical(condition));
        }

        public ForWithSeedAndCondition<T> While(Func<T, bool> condition)
        {
            return While(_factory.Canonical(condition));
        }

        public ForWithSeedAndCondition<T> While(Func<IPromise<ControlValue<bool>>> condition)
        {
            return While(x => condition());
        }

        public ForWithSeedAndCondition<T> While(Func<ControlValue<bool>> condition)
        {
            return While(x => condition());
        }

        public ForWithSeedAndCondition<T> While(Func<IPromise<NullableResult<bool>>> condition)
        {
            return While(x => condition());
        }

        public ForWithSeedAndCondition<T> While(Func<NullableResult<bool>> condition)
        {
            return While(x => condition());
        }

        public ForWithSeedAndCondition<T> While(Func<IPromise<bool>> condition)
        {
            return While(x => condition());
        }

        public ForWithSeedAndCondition<T> While(Func<bool> condition)
        {
            return While(x => condition());
        }

        public ForWithSeedAndCondition<T> While(bool value)
        {
            return While(() => value);
        }

        public ForWithSeedAndCondition<T> While()
        {
            return While(true);
        }
    }
}
