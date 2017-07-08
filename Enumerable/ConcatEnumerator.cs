using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal class ConcatEnumerator<T> : IPromiseEnumerator<T>
    {
        private IPromiseEnumerator<T> _left;
        private IPromiseEnumerator<T> _right;

        public ConcatEnumerator(IPromiseEnumerator<T> left, IPromiseEnumerator<T> right)
        {
            this._left = left;
            this._right = right;
        }

        public T Current
        {
            get
            {
                if (_left == null)
                {
                    return _right.Current;
                }

                return _left.Current;
            }
        }

        public PromiseFactory Factory
        {
            get
            {
                if (_left == null)
                {
                    return _right.Factory;
                }

                return _left.Factory;
            }
        }

        public IPromise<IPromiseEnumerator<T>> MoveNext()
        {
            if (_left == null)
            {
                return _right.MoveNext().Then(result =>
                {
                    if (result == null)
                    {
                        return null;
                    }

                    return (IPromiseEnumerator<T>)(new ConcatEnumerator<T>(null, result));
                });
            }

            return _left.MoveNext().Then(result =>
            {
                if (result == null)
                {
                    return _right.MoveNext().Then(rest =>
                    {
                        if (rest == null)
                        {
                            return null;
                        }

                        return (IPromiseEnumerator<T>)(new ConcatEnumerator<T>(null, rest));
                    });
                }

                return _left.Factory.Value((IPromiseEnumerator<T>)(new ConcatEnumerator<T>(result, _right)));
            });
        }
    }

    internal static class ConcatEnumerator
    {
        public static IPromiseEnumerator<T> Create<T>(IPromiseEnumerator<T> left, IPromiseEnumerator<T> right)
        {
            return new ConcatEnumerator<T>(left, right);
        }
    }
}
