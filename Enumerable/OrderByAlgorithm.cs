using MooPromise.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal static class OrderByAlgorithm
    {
        private class MergeEnumerator<Key, Value> : IPromiseEnumerator<Tuple<Key, Value>>
        {
            private IPromiseEnumerator<Tuple<Key, Value>> _left;
            private IPromiseEnumerator<Tuple<Key, Value>> _right;
            private NullableResult<Tuple<Key, Value>> _current;
            private IComparer<Key> _comparer;

            public MergeEnumerator(PromiseFactory factory, NullableResult<Tuple<Key, Value>> current,
                IComparer<Key> comparer, IPromiseEnumerator<Tuple<Key, Value>> left,
                IPromiseEnumerator<Tuple<Key, Value>> right)
            {
                this._left = left;
                this._right = right;
                this.Factory = factory;
                this._current = current;
                this._comparer = comparer;
            }

            public Tuple<Key, Value> Current
            {
                get
                {
                    if (!_current.HasResult)
                    {
                        throw new NullReferenceException();
                    }

                    return _current.Result;
                }
            }

            public PromiseFactory Factory
            {
                get;
                private set;
            }

            public IPromise<IPromiseEnumerator<Tuple<Key, Value>>> MoveNext()
            {
                if (_left == null)
                {
                    if (_right == null)
                    {
                        return Factory.Value<IPromiseEnumerator<Tuple<Key, Value>>>(null);
                    }

                    return _right.MoveNext().Then(nextRight => 
                    {
                        if (nextRight == null)
                        {
                            return null;
                        }

                        return (IPromiseEnumerator<Tuple<Key, Value>>)(
                            new MergeEnumerator<Key, Value>(
                                Factory, new NullableResult<Tuple<Key, Value>>(nextRight.Current), _comparer, null, nextRight));
                    });
                }

                if (_right == null)
                {
                    return _left.MoveNext().Then(nextLeft => 
                    {
                        if (nextLeft == null)
                        {
                            return null;
                        }

                        return (IPromiseEnumerator<Tuple<Key, Value>>)(
                            new MergeEnumerator<Key, Value>(
                                Factory, new NullableResult<Tuple<Key, Value>>(nextLeft.Current), _comparer, nextLeft, null));
                    });
                }

                return _right.MoveNext().Then(nextRight => _left.MoveNext().Then(nextLeft =>
                {
                    if (nextLeft == null)
                    {
                        if (nextRight == null)
                        {
                            return null;
                        }

                        return (IPromiseEnumerator<Tuple<Key, Value>>)(
                            new MergeEnumerator<Key, Value>(
                                Factory, new NullableResult<Tuple<Key, Value>>(nextRight.Current), _comparer, null, nextRight));
                    }

                    if (nextRight == null)
                    {
                        return (IPromiseEnumerator<Tuple<Key, Value>>)(
                            new MergeEnumerator<Key, Value>(
                                Factory, new NullableResult<Tuple<Key, Value>>(nextLeft.Current), _comparer, nextLeft, null));
                    }

                    if (_comparer.Compare(nextLeft.Current.Item1, nextRight.Current.Item1) <= 0)
                    {
                        return (IPromiseEnumerator<Tuple<Key, Value>>)(
                            new MergeEnumerator<Key, Value>(
                                Factory, new NullableResult<Tuple<Key, Value>>(nextLeft.Current), _comparer, nextLeft, _right));
                    }

                    return (IPromiseEnumerator<Tuple<Key, Value>>)(
                            new MergeEnumerator<Key, Value>(
                                Factory, new NullableResult<Tuple<Key, Value>>(nextRight.Current), _comparer, _left, nextRight));
                }));
            }
        }

        private static IPromise<IPromiseEnumerator<Tuple<Key, Value>>> OrderByInternal<Key, Value>
            (PromiseFactory factory, IPromiseEnumerator<Tuple<Key, Value>> items, int count, IComparer<Key> comparer)
        {
            if (count < 2)
            {
                return factory.Value(items);
            }

            int at = count / 2;

            return OrderByInternal(factory, TakeWhileEnumerator.Create(items, at), at, comparer).Then(firstPart =>
                items.Advance(at).Then(half => OrderByInternal(factory, half, count - at, comparer)).Then(secondPart =>
                    (IPromiseEnumerator<Tuple<Key, Value>>)
                        new MergeEnumerator<Key, Value>(factory, new NullableResult<Tuple<Key, Value>>(), comparer, firstPart, secondPart)));
        }

        public static IPromise<IPromiseEnumerator<Value>> OrderBy<Key, Value>(IPromiseEnumerator<Value> items, Func<Value, int, IPromise<Key>> key, IComparer<Key> comparer)
        {
            return CountAlgorithm.Count(items).Then(count =>
                OrderByInternal(items.Factory, SelectEnumerator.Create(items, (x, i) => key(x, i).Then(v => Tuple.Create(v, x))), count, comparer)
                    .Then(i => SelectEnumerator.Create(i, x => x.Item2)));
        }
    }
}
