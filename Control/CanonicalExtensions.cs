using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Control
{
    internal static class CanonicalExtensions
    {
        public static IPromise<NullableResult<T>> ToNullableResult<T>(this IPromise<ControlValue<T>> p, PromiseFactory factory)
        {
            if (p == null)
            {
                return factory.Value(new NullableResult<T>());
            }

            return p.Then(result =>
            {
                if (result == null || result.State != ControlState.Return || !result.HasValue)
                {
                    return new NullableResult<T>();
                }

                return new NullableResult<T>(result.Value);
            });
        }

        public static IPromise<ControlState> ToControlState<T>(this IPromise<ControlValue<T>> p, PromiseFactory factory)
        {
            if (p == null)
            {
                return factory.Value(ControlState.Next);
            }

            return p.Then(result =>
            {
                if (result == null)
                {
                    return ControlState.Next;
                }

                return result.State;
            });
        }

        public static IPromise<T> UnCast<T>(this IPromise p, PromiseFactory factory, T defaultValue)
        {
            if (p == null)
            {
                return factory.Value(defaultValue);
            }

            return p.Then(() => defaultValue);
        }

        public static IPromise<T> UnCast<T>(this IPromise p, PromiseFactory factory)
        {
            return p.UnCast<T>(factory, default(T));
        }

        public static IPromise<object> UnCast(this IPromise p, PromiseFactory factory)
        {
            return p.UnCast<object>(factory, null);
        }

        public static Func<T, IPromise> ReturnPromise<T>(this Action<T> fn, PromiseFactory factory)
        {
            return x =>
            {
                fn(x);
                return factory.Value();
            };
        }

        public static Func<IPromise> ReturnPromise(this Action fn, PromiseFactory factory)
        {
            return () =>
            {
                fn();
                return factory.Value();
            };
        }

        public static IPromise<E> SafeThen<T, E>(this PromiseFactory factory, IPromise<T> next, Func<T, IPromise<E>> consumer)
        {
            if (next == null)
            {
                return null;
            }

            return next.Then(result => consumer(result));
        }

        public static IPromise<E> SafeThen<T, E>(this PromiseFactory factory, IPromise<T> next, Func<IPromise<E>> consumer)
        {
            return factory.SafeThen(next, x => consumer());
        }

        public static IPromise<E> SafeThen<T, E>(this PromiseFactory factory, IPromise<T> next, Func<T, E> conusmer)
        {
            return factory.SafeThen(next, x => factory.Value(conusmer(x)));
        }

        public static IPromise<E> SafeThen<T, E>(this PromiseFactory factory, IPromise<T> next, Func<E> conusmer)
        {
            return factory.SafeThen(next, x => conusmer());
        }

        public static IPromise SafeThen<T>(this PromiseFactory factory, IPromise<T> next, Func<T, IPromise> consumer)
        {
            return factory.SafeThen(next, x => consumer(x).UnCast(factory)).Cast();
        }

        public static IPromise SafeThen<T>(this PromiseFactory factory, IPromise<T> next, Func<IPromise> consumer)
        {
            return factory.SafeThen(next, x => consumer());
        }

        public static IPromise SafeThen<T>(this PromiseFactory factory, IPromise<T> next, Action<T> consumer)
        {
            return factory.SafeThen(next, consumer.ReturnPromise(factory));
        }

        public static IPromise SafeThen<T>(this PromiseFactory factory, IPromise<T> next, Action consumer)
        {
            return factory.SafeThen(next, x => consumer());
        }

        public static IPromise<E> SafeThen<T, E>(this PromiseFactory factory, Func<IPromise<T>> next, Func<T, IPromise<E>> consumer)
        {
            return factory.SafeThen(next(), consumer);
        }

        public static IPromise<E> SafeThen<T, E>(this PromiseFactory factory, Func<IPromise<T>> next, Func<IPromise<E>> consumer)
        {
            return factory.SafeThen(next(), consumer);
        }

        public static IPromise<E> SafeThen<T, E>(this PromiseFactory factory, Func<IPromise<T>> next, Func<T, E> consumer)
        {
            return factory.SafeThen(next(), consumer);
        }

        public static IPromise<E> SafeThen<T, E>(this PromiseFactory factory, Func<IPromise<T>> next, Func<E> consumer)
        {
            return factory.SafeThen(next(), consumer);
        }

        public static IPromise SafeThen<T>(this PromiseFactory factory, Func<IPromise<T>> next, Func<T, IPromise> consumer)
        {
            return factory.SafeThen(next(), consumer);
        }

        public static IPromise SafeThen<T>(this PromiseFactory factory, Func<IPromise<T>> next, Func<IPromise> consumer)
        {
            return factory.SafeThen(next(), consumer);
        }

        public static IPromise SafeThen<T>(this PromiseFactory factory, Func<IPromise<T>> next, Action<T> consumer)
        {
            return factory.SafeThen(next(), consumer);
        }

        public static IPromise SafeThen<T>(this PromiseFactory factory, Func<IPromise<T>> next, Action consumer)
        {
            return factory.SafeThen(next(), consumer);
        }

        public static IPromise<T> SafeThen<T>(this PromiseFactory factory, IPromise next, Func<IPromise<T>> consumer)
        {
            return factory.SafeThen(next.UnCast(factory), consumer);
        }

        public static IPromise<T> SafeThen<T>(this PromiseFactory factory, IPromise next, Func<T> conusmer)
        {
            return factory.SafeThen(next, () => factory.Value(conusmer()));
        }

        public static IPromise SafeThen(this PromiseFactory factory, IPromise next, Func<IPromise> consumer)
        {
            return factory.SafeThen(next, () => consumer().UnCast(factory)).Cast();
        }

        public static IPromise SafeThen(this PromiseFactory factory, IPromise next, Action consumer)
        {
            return factory.SafeThen(next, consumer.ReturnPromise(factory));
        }

        public static IPromise<T> SafeThen<T>(this PromiseFactory factory, Func<IPromise> next, Func<IPromise<T>> consumer)
        {
            return factory.SafeThen(next(), consumer);
        }

        public static IPromise<T> SafeThen<T>(this PromiseFactory factory, Func<IPromise> next, Func<T> consumer)
        {
            return factory.SafeThen(next(), consumer);
        }

        public static IPromise SafeThen(this PromiseFactory factory, Func<IPromise> next, Func<IPromise> consumer)
        {
            return factory.SafeThen(next(), consumer);
        }

        public static IPromise SafeThen(this PromiseFactory factory, Func<IPromise> next, Action consumer)
        {
            return factory.SafeThen(next(), consumer);
        }

        public static Func<IPromise<ControlValue<T>>> Canonical<T>(this PromiseFactory factory, Func<IPromise<ControlValue<T>>> fn)
        {
            return fn;
        }

        public static Func<IPromise<ControlValue<T>>> Canonical<T>(this PromiseFactory factory, Func<ControlValue<T>> fn)
        {
            return factory.Canonical(() => factory.Value(fn()));
        }

        public static Func<IPromise<ControlValue<T>>> Canonical<T>(this PromiseFactory factory, Func<IPromise<NullableResult<T>>> fn)
        {
            return factory.Canonical(() => factory.SafeThen(fn, result =>
            {
                if (result == null || !result.HasResult)
                {
                    return ControlValue<T>.Next;
                }

                return ControlValue<T>.Return(result.Result);
            }));
        }

        public static Func<IPromise<ControlValue<T>>> Canonical<T>(this PromiseFactory factory, Func<NullableResult<T>> fn)
        {
            return factory.Canonical(() => factory.Value(fn()));
        }

        public static Func<IPromise<ControlValue<T>>> Canonical<T>(this PromiseFactory factory, Func<IPromise<T>> fn)
        {
            return factory.Canonical(() => factory.SafeThen(fn, result => ControlValue<T>.Return(result)));
        }

        public static Func<IPromise<ControlValue<T>>> Canonical<T>(this PromiseFactory factory, Func<T> fn)
        {
            return factory.Canonical(() => factory.Value(fn()));
        }

        public static Func<IPromise<ControlValue<object>>> Canonical(this PromiseFactory factory, Func<IPromise<ControlState>> fn)
        {
            return factory.Canonical(() => factory.SafeThen(fn, result => new ControlValue<object>(result)));
        }

        public static Func<IPromise<ControlValue<object>>> Canonical(this PromiseFactory factory, Func<ControlState> fn)
        {
            return factory.Canonical(() => factory.Value(fn()));
        }

        public static Func<IPromise<ControlValue<object>>> Canonical(this PromiseFactory factory, Func<IPromise> fn)
        {
            return factory.Canonical(() => factory.SafeThen(fn, () => ControlState.Next));
        }

        public static Func<IPromise<ControlValue<object>>> Canonical(this PromiseFactory factory, Action fn)
        {
            return factory.Canonical(fn.ReturnPromise(factory));
        }

        public static Func<T, IPromise<ControlValue<E>>> Canonical<T, E>(this PromiseFactory factory, Func<T, IPromise<ControlValue<E>>> fn)
        {
            return fn;
        }

        public static Func<T, IPromise<ControlValue<E>>> Canonical<T, E>(this PromiseFactory factory, Func<T, ControlValue<E>> fn)
        {
            return factory.Canonical<T, E>(x => factory.Value(fn(x)));
        }

        public static Func<T, IPromise<ControlValue<E>>> Canonical<T, E>(this PromiseFactory factory, Func<T, IPromise<NullableResult<E>>> fn)
        {
            return factory.Canonical<T, E>(x => factory.SafeThen(fn(x), result =>
            {
                if (result == null || !result.HasResult)
                {
                    return ControlValue<E>.Next;
                }

                return ControlValue<E>.Return(result.Result);
            }));
        }

        public static Func<T, IPromise<ControlValue<E>>> Canonical<T, E>(this PromiseFactory factory, Func<T, NullableResult<E>> fn)
        {
            return factory.Canonical<T, E>(x => factory.Value(fn(x)));
        }

        public static Func<T, IPromise<ControlValue<E>>> Canonical<T, E>(this PromiseFactory factory, Func<T, IPromise<E>> fn)
        {
            return factory.Canonical<T, E>(x => factory.SafeThen(fn(x), result => ControlValue<E>.Return(result)));
        }

        public static Func<T, IPromise<ControlValue<E>>> Canonical<T, E>(this PromiseFactory factory, Func<T, E> fn)
        {
            return factory.Canonical<T, E>(x => factory.Value(fn(x)));
        }

        public static Func<T, IPromise<ControlValue<object>>> Canonical<T>(this PromiseFactory factory, Func<T, IPromise<ControlState>> fn)
        {
            return factory.Canonical<T, object>(x => factory.SafeThen(fn(x), result => new ControlValue<object>(result)));
        }

        public static Func<T, IPromise<ControlValue<object>>> Canonical<T>(this PromiseFactory factory, Func<T, ControlState> fn)
        {
            return factory.Canonical<T, object>(x => factory.Value(fn(x)));
        }

        public static Func<T, IPromise<ControlValue<object>>> Canonical<T>(this PromiseFactory factory, Func<T, IPromise> fn)
        {
            return factory.Canonical<T, object>(x => factory.SafeThen(fn(x), () => ControlValue<object>.Next));
        }

        public static Func<T, IPromise<ControlValue<object>>> Canonical<T>(this PromiseFactory factory, Action<T> fn)
        {
            return factory.Canonical<T, object>(fn.ReturnPromise(factory));
        }
    }
}
