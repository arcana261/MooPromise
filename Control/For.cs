using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Control
{
    public class For<T> : DoAble
    {
        private Func<IPromise<ControlValue<T>>> _seed;
        private T _current;
        private Func<T, IPromise<ControlValue<bool>>> _condition;
        private Func<T, IPromise<ControlValue<T>>> _iterator;

        internal For(PromiseFactory factory, Func<IPromise<ControlValue<T>>> seed, Func<T, IPromise<ControlValue<bool>>> condition, Func<T, IPromise<ControlValue<T>>> iterator)
            : base(factory)
        {
            this._current = default(T);
            this._seed = seed;
            this._condition = condition;
            this._iterator = iterator;
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
            return Factory.SafeThen(_seed, seedResult =>
            {
                if (seedResult == null || seedResult.State != ControlState.Return || !seedResult.HasValue)
                {
                    return Factory.Value(ControlValue<E>.Next);
                }

                SetCurrent(seedResult.Value);

                var w = new While(Factory, () => _condition(GetCurrent()));

                return w.Do(() => Factory.SafeThen(body(GetCurrent()), result =>
                {
                    if (result == null || result.State == ControlState.Break)
                    {
                        return Factory.Value(ControlValue<E>.Next);
                    }

                    if (result.State == ControlState.Return)
                    {
                        return Factory.Value(result);
                    }

                    return Factory.SafeThen(_iterator(GetCurrent()), next =>
                    {
                        if (next == null || next.State != ControlState.Return || !next.HasValue)
                        {
                            return ControlValue<E>.Break;
                        }

                        SetCurrent(next.Value);
                        return ControlValue<E>.Continue;
                    });
                }));
            });
        }

        public override IPromise<ControlValue<E>> Do<E>(Func<IPromise<ControlValue<E>>> body)
        {
            return Do(x => body());
        }

        public IPromise<ControlValue<E>> Do<E>(Func<T, ControlValue<E>> body)
        {
            return Do(Factory.Canonical(body));
        }

        public IPromise<NullableResult<E>> Do<E>(Func<T, IPromise<NullableResult<E>>> body)
        {
            return Do(Factory.Canonical(body)).ToNullableResult(Factory);
        }

        public IPromise<NullableResult<E>> Do<E>(Func<T, NullableResult<E>> body)
        {
            return Do(Factory.Canonical(body)).ToNullableResult(Factory);
        }

        public IPromise<NullableResult<E>> Do<E>(Func<T, IPromise<E>> body)
        {
            return Do(Factory.Canonical(body)).ToNullableResult(Factory);
        }

        public IPromise<NullableResult<E>> Do<E>(Func<T, E> body)
        {
            return Do(Factory.Canonical(body)).ToNullableResult(Factory);
        }

        public IPromise Do(Func<T, IPromise> body)
        {
            return Do(Factory.Canonical(body)).Cast();
        }

        public IPromise Do(Action<T> body)
        {
            return Do(Factory.Canonical(body)).Cast();
        }

        public IPromise<ControlState> Do(Func<T, IPromise<ControlState>> body)
        {
            return Do(Factory.Canonical(body)).ToControlState(Factory);
        }

        public IPromise<ControlState> Do(Func<T, ControlState> body)
        {
            return Do(Factory.Canonical(body)).ToControlState(Factory);
        }
    }

    public class ForWithSeedAndCondition<T>
    {
        private PromiseFactory _factory;
        private Func<IPromise<ControlValue<T>>> _seed;
        private Func<T, IPromise<ControlValue<bool>>> _condition;

        internal ForWithSeedAndCondition(PromiseFactory factory, Func<IPromise<ControlValue<T>>> seed, Func<T, IPromise<ControlValue<bool>>> condition)
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

        public For<T> Iterate(ControlValue<T> value)
        {
            return Iterate(() => value);
        }

        public For<T> Iterate(IPromise<ControlValue<T>> value)
        {
            return Iterate(() => value);
        }

        public For<T> Iterate(NullableResult<T> value)
        {
            return Iterate(() => value);
        }

        public For<T> Iterate(IPromise<NullableResult<T>> value)
        {
            return Iterate(() => value);
        }

        public For<T> Iterate(IPromise<T> value)
        {
            return Iterate(() => value);
        }
    }

    public class ForWithSeed<T> : WhileAble<ForWithSeedAndCondition<T>>
    {
        private Func<IPromise<ControlValue<T>>> _seed;

        internal ForWithSeed(PromiseFactory factory, Func<IPromise<ControlValue<T>>> seed)
            : base(factory)
        {
            this._seed = seed;
        }

        public ForWithSeedAndCondition<T> While(Func<T, IPromise<ControlValue<bool>>> condition)
        {
            return new ForWithSeedAndCondition<T>(Factory, _seed, condition);
        }

        public override ForWithSeedAndCondition<T> While(Func<IPromise<ControlValue<bool>>> condition)
        {
            return While(x => condition());
        }

        public ForWithSeedAndCondition<T> While(Func<T, ControlValue<bool>> condition)
        {
            return While(Factory.Canonical(condition));
        }

        public ForWithSeedAndCondition<T> While(Func<T, IPromise<NullableResult<bool>>> condition)
        {
            return While(Factory.Canonical(condition));
        }

        public ForWithSeedAndCondition<T> While(Func<T, NullableResult<bool>> condition)
        {
            return While(Factory.Canonical(condition));
        }

        public ForWithSeedAndCondition<T> While(Func<T, IPromise<bool>> condition)
        {
            return While(Factory.Canonical(condition));
        }

        public ForWithSeedAndCondition<T> While(Func<T, bool> condition)
        {
            return While(Factory.Canonical(condition));
        }
    }
}
