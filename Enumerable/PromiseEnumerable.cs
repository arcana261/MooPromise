using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Enumerable
{
    internal class PromiseEnumerable<T> : IPromiseEnumerable<T>
    {
        private IPromise<IPromiseEnumerator<T>> _items;
        private IPromise _then;

        public PromiseEnumerable(IPromise<IPromiseEnumerator<T>> items)
        {
            _items = items;
            _then = null;
        }

        public PromiseEnumerable(IPromise<IEnumerable<T>> items)
            : this(PromiseEnumerator.Create(items))
        {

        }

        public PromiseEnumerable(PromiseFactory factory, IEnumerable<T> items)
            : this(PromiseEnumerator.Create(factory, items))
        {

        }

        public IPromise<List<T>> ToList()
        {
            return _items.Then(list => ToListAlgorithm.ToList(list));
        }

        public IPromiseEnumerable<T> Where(Func<T, IPromise<bool>> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => WhereEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<T> Where(Func<T, bool> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => WhereEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<T> Where(Func<T, int, bool> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => WhereEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<T> Where(Func<T, int, IPromise<bool>> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => WhereEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<E> Select<E>(Func<T, int, IPromise<E>> action)
        {
            return new PromiseEnumerable<E>(_items.Then(list => SelectEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<E> Select<E>(Func<T, int, E> action)
        {
            return new PromiseEnumerable<E>(_items.Then(list => SelectEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<E> Select<E>(Func<T, IPromise<E>> action)
        {
            return new PromiseEnumerable<E>(_items.Then(list => SelectEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<E> Select<E>(Func<T, E> action)
        {
            return new PromiseEnumerable<E>(_items.Then(list => SelectEnumerator.Create(list, action)));
        }

        public IPromise<E> Aggregate<E>(Func<E, T, int, IPromise<E>> action, E seed)
        {
            return _items.Then(list => ReduceAlgorithm.Reduce(list, action, seed));
        }

        public IPromise<E> Aggregate<E>(Func<E, T, int, E> action, E seed)
        {
            return _items.Then(list => ReduceAlgorithm.Reduce(list, action, seed));
        }

        public IPromise<E> Aggregate<E>(Func<E, T, IPromise<E>> action, E seed)
        {
            return _items.Then(list => ReduceAlgorithm.Reduce(list, action, seed));
        }

        public IPromise<E> Aggregate<E>(Func<E, T, E> action, E seed)
        {
            return _items.Then(list => ReduceAlgorithm.Reduce(list, action, seed));
        }

        public IPromise<E> Aggregate<E>(Func<E, T, int, IPromise<E>> action)
        {
            return _items.Then(list => ReduceAlgorithm.Reduce(list, action));
        }

        public IPromise<E> Aggregate<E>(Func<E, T, int, E> action)
        {
            return _items.Then(list => ReduceAlgorithm.Reduce(list, action));
        }

        public IPromise<E> Aggregate<E>(Func<E, T, IPromise<E>> action)
        {
            return _items.Then(list => ReduceAlgorithm.Reduce(list, action));
        }

        public IPromise<E> Aggregate<E>(Func<E, T, E> action)
        {
            return _items.Then(list => ReduceAlgorithm.Reduce(list, action));
        }

        public IPromise Each(Func<T, int, IPromise> action)
        {
            return _items.Then(list => EachAlgorithm.Each(list, action)).Cast();
        }

        public IPromise Each(Action<T, int> action)
        {
            return _items.Then(list => EachAlgorithm.Each(list, action)).Cast();
        }

        public IPromise Each(Func<T, IPromise> action)
        {
            return _items.Then(list => EachAlgorithm.Each(list, action)).Cast();
        }

        public IPromise Each(Action<T> action)
        {
            return _items.Then(list => EachAlgorithm.Each(list, action)).Cast();
        }

        public IPromise<long> LongCount(Func<T, int, IPromise<bool>> filter)
        {
            return _items.Then(list => LongCountAlgorithm.LongCount(list, filter));
        }

        public IPromise<long> LongCount(Func<T, int, bool> filter)
        {
            return _items.Then(list => LongCountAlgorithm.LongCount(list, filter));
        }

        public IPromise<long> LongCount(Func<T, IPromise<bool>> filter)
        {
            return _items.Then(list => LongCountAlgorithm.LongCount(list, filter));
        }

        public IPromise<long> LongCount(Func<T, bool> filter)
        {
            return _items.Then(list => LongCountAlgorithm.LongCount(list, filter));
        }

        public IPromise<long> LongCount()
        {
            return _items.Then(list => LongCountAlgorithm.LongCount(list));
        }

        public IPromise<int> Count(Func<T, int, IPromise<bool>> filter)
        {
            return _items.Then(list => CountAlgorithm.Count(list, filter));
        }

        public IPromise<int> Count(Func<T, int, bool> filter)
        {
            return _items.Then(list => CountAlgorithm.Count(list, filter));
        }

        public IPromise<int> Count(Func<T, IPromise<bool>> filter)
        {
            return _items.Then(list => CountAlgorithm.Count(list, filter));
        }

        public IPromise<int> Count(Func<T, bool> filter)
        {
            return _items.Then(list => CountAlgorithm.Count(list, filter));
        }

        public IPromise<int> Count()
        {
            return _items.Then(list => CountAlgorithm.Count(list));
        }

        private IPromise Then()
        {
            if (_then == null)
            {
                _then = this.Count().Cast();
            }

            return _then;
        }

        public IPromise Then(Action action)
        {
            return Then().Then(action);
        }

        public IPromise<F> Then<F>(Func<F> action)
        {
            return Then().Then(action);
        }

        public IPromise Then(Func<IPromise> action)
        {
            return Then().Then(action);
        }

        public IPromise<F> Then<F>(Func<IPromise<F>> action)
        {
            return Then().Then(action);
        }

        public IPromise Catch(Action<Exception> action)
        {
            return Then().Catch(action);
        }

        public IPromise Catch(Action action)
        {
            return Then().Catch(action);
        }

        public IPromise Finally(Action<Exception> action)
        {
            return Then().Finally(action);
        }

        public IPromise Finally(Action action)
        {
            return Then().Finally(action);
        }

        public IPromiseEnumerable<T> TakeWhile(Func<T, int, IPromise<bool>> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => TakeWhileEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<T> TakeWhile(Func<T, int, bool> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => TakeWhileEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<T> TakeWhile(Func<T, IPromise<bool>> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => TakeWhileEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<T> TakeWhile(Func<T, bool> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => TakeWhileEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<T> SkipWhile(Func<T, int, IPromise<bool>> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => SkipWhileEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<T> SkipWhile(Func<T, int, bool> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => SkipWhileEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<T> SkipWhile(Func<T, IPromise<bool>> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => SkipWhileEnumerator.Create(list, action)));
        }

        public IPromiseEnumerable<T> SkipWhile(Func<T, bool> action)
        {
            return new PromiseEnumerable<T>(_items.Then(list => SkipWhileEnumerator.Create(list, action)));
        }

        public IPromise<bool> Empty()
        {
            return _items.Then(list => EmptyAlgorithm.Empty(list));
        }

        public IPromise<bool> Empty(Func<T, int, IPromise<bool>> action)
        {
            return _items.Then(list => EmptyAlgorithm.Empty(list, action));
        }

        public IPromise<bool> Empty(Func<T, int, bool> action)
        {
            return _items.Then(list => EmptyAlgorithm.Empty(list, action));
        }

        public IPromise<bool> Empty(Func<T, IPromise<bool>> action)
        {
            return _items.Then(list => EmptyAlgorithm.Empty(list, action));
        }

        public IPromise<bool> Empty(Func<T, bool> action)
        {
            return _items.Then(list => EmptyAlgorithm.Empty(list, action));
        }

        public IPromise<bool> Any(Func<T, int, IPromise<bool>> action)
        {
            return _items.Then(list => AnyAlgorithm.Any(list, action));
        }

        public IPromise<bool> Any(Func<T, int, bool> action)
        {
            return _items.Then(list => AnyAlgorithm.Any(list, action));
        }

        public IPromise<bool> Any(Func<T, IPromise<bool>> action)
        {
            return _items.Then(list => AnyAlgorithm.Any(list, action));
        }

        public IPromise<bool> Any(Func<T, bool> action)
        {
            return _items.Then(list => AnyAlgorithm.Any(list, action));
        }

        public IPromise<bool> Exists(Func<T, int, IPromise<bool>> action)
        {
            return Any(action);
        }

        public IPromise<bool> Exists(Func<T, int, bool> action)
        {
            return Any(action);
        }

        public IPromise<bool> Exists(Func<T, IPromise<bool>> action)
        {
            return Any(action);
        }

        public IPromise<bool> Exists(Func<T, bool> action)
        {
            return Any(action);
        }

        public IPromise<bool> Any()
        {
            return _items.Then(list => AnyAlgorithm.Any(list));
        }

        public IPromise<bool> Exists()
        {
            return Any();
        }

        public IPromiseEnumerable<T> Take(int count)
        {
            return new PromiseEnumerable<T>(_items.Then(list => TakeWhileEnumerator.Create(list, count)));
        }

        public IPromiseEnumerable<T> Skip(int count)
        {
            return new PromiseEnumerable<T>(_items.Then(list => SkipWhileEnumerator.Create(list, count)));
        }

        public IPromise<T> FirstOrDefault()
        {
            return _items.Then(list => FirstOrDefaultAlgorithm.FirstOrDefault(list));
        }

        public IPromise<T> First()
        {
            return _items.Then(list => FirstAlgorithm.First(list));
        }

        public IPromise<T> SingleOrDefault()
        {
            return _items.Then(list => SingleOrDefaultAlgorithm.SingleOrDefault(list));
        }

        public IPromise<T> Single()
        {
            return _items.Then(list => SingleAlgorithm.Single(list));
        }

        public IPromise<T> FirstOrDefault(T defaultValue)
        {
            return _items.Then(list => FirstOrDefaultAlgorithm.FirstOrDefault(list, defaultValue));
        }

        public IPromise<T> FirstOrDefault(Func<T, int, IPromise<bool>> predicate, T defaultValue)
        {
            return _items.Then(list => FirstOrDefaultAlgorithm.FirstOrDefault(list, predicate, defaultValue));
        }

        public IPromise<T> FirstOrDefault(Func<T, int, bool> predicate, T defaultValue)
        {
            return _items.Then(list => FirstOrDefaultAlgorithm.FirstOrDefault(list, predicate, defaultValue));
        }

        public IPromise<T> FirstOrDefault(Func<T, IPromise<bool>> predicate, T defaultValue)
        {
            return _items.Then(list => FirstOrDefaultAlgorithm.FirstOrDefault(list, predicate, defaultValue));
        }

        public IPromise<T> FirstOrDefault(Func<T, bool> predicate, T defaultValue)
        {
            return _items.Then(list => FirstOrDefaultAlgorithm.FirstOrDefault(list, predicate, defaultValue));
        }

        public IPromise<T> FirstOrDefault(Func<T, int, IPromise<bool>> predicate)
        {
            return _items.Then(list => FirstOrDefaultAlgorithm.FirstOrDefault(list, predicate));
        }

        public IPromise<T> FirstOrDefault(Func<T, int, bool> predicate)
        {
            return _items.Then(list => FirstOrDefaultAlgorithm.FirstOrDefault(list, predicate));
        }

        public IPromise<T> FirstOrDefault(Func<T, IPromise<bool>> predicate)
        {
            return _items.Then(list => FirstOrDefaultAlgorithm.FirstOrDefault(list, predicate));
        }

        public IPromise<T> FirstOrDefault(Func<T, bool> predicate)
        {
            return _items.Then(list => FirstOrDefaultAlgorithm.FirstOrDefault(list, predicate));
        }

        public IPromise<T> First(Func<T, int, IPromise<bool>> predicate)
        {
            return _items.Then(list => FirstAlgorithm.First(list, predicate));
        }

        public IPromise<T> First(Func<T, int, bool> predicate)
        {
            return _items.Then(list => FirstAlgorithm.First(list, predicate));
        }

        public IPromise<T> First(Func<T, IPromise<bool>> predicate)
        {
            return _items.Then(list => FirstAlgorithm.First(list, predicate));
        }

        public IPromise<T> First(Func<T, bool> predicate)
        {
            return _items.Then(list => FirstAlgorithm.First(list, predicate));
        }

        public IPromise<T> SingleOrDefault(T defaultValue)
        {
            return _items.Then(list => SingleOrDefaultAlgorithm.SingleOrDefault(list, defaultValue));
        }

        public IPromise<T> SingleOrDefault(Func<T, int, IPromise<bool>> predicate, T defaultValue)
        {
            return _items.Then(list => SingleOrDefaultAlgorithm.SingleOrDefault(list, predicate, defaultValue));
        }

        public IPromise<T> SingleOrDefault(Func<T, int, bool> predicate, T defaultValue)
        {
            return _items.Then(list => SingleOrDefaultAlgorithm.SingleOrDefault(list, predicate, defaultValue));
        }

        public IPromise<T> SingleOrDefault(Func<T, IPromise<bool>> predicate, T defaultValue)
        {
            return _items.Then(list => SingleOrDefaultAlgorithm.SingleOrDefault(list, predicate, defaultValue));
        }

        public IPromise<T> SingleOrDefault(Func<T, bool> predicate, T defaultValue)
        {
            return _items.Then(list => SingleOrDefaultAlgorithm.SingleOrDefault(list, predicate, defaultValue));
        }

        public IPromise<T> SingleOrDefault(Func<T, int, IPromise<bool>> predicate)
        {
            return _items.Then(list => SingleOrDefaultAlgorithm.SingleOrDefault(list, predicate));
        }

        public IPromise<T> SingleOrDefault(Func<T, int, bool> predicate)
        {
            return _items.Then(list => SingleOrDefaultAlgorithm.SingleOrDefault(list, predicate));
        }

        public IPromise<T> SingleOrDefault(Func<T, IPromise<bool>> predicate)
        {
            return _items.Then(list => SingleOrDefaultAlgorithm.SingleOrDefault(list, predicate));
        }

        public IPromise<T> SingleOrDefault(Func<T, bool> predicate)
        {
            return _items.Then(list => SingleOrDefaultAlgorithm.SingleOrDefault(list, predicate));
        }

        public IPromise<T> Single(Func<T, int, IPromise<bool>> predicate)
        {
            return _items.Then(list => SingleAlgorithm.Single(list, predicate));
        }

        public IPromise<T> Single(Func<T, int, bool> predicate)
        {
            return _items.Then(list => SingleAlgorithm.Single(list, predicate));
        }

        public IPromise<T> Single(Func<T, IPromise<bool>> predicate)
        {
            return _items.Then(list => SingleAlgorithm.Single(list, predicate));
        }

        public IPromise<T> Single(Func<T, bool> predicate)
        {
            return _items.Then(list => SingleAlgorithm.Single(list, predicate));
        }

        public IPromiseEnumerable<T> DefaultIfEmpty(T defaultValue)
        {
            return new PromiseEnumerable<T>(_items.Then(list => DefaultIfEmptyAlgorithm.DefaultIfEmpty(list, defaultValue)));
        }

        public IPromiseEnumerable<T> DefaultIfEmpty()
        {
            return new PromiseEnumerable<T>(_items.Then(list => DefaultIfEmptyAlgorithm.DefaultIfEmpty(list)));
        }

        public IPromise<T> LastOrDefault()
        {
            return _items.Then(list => LastOrDefaultAlgoritm.LastOrDefault(list));
        }

        public IPromise<T> LastOrDefault(T defaultValue)
        {
            return _items.Then(list => LastOrDefaultAlgoritm.LastOrDefault(list, defaultValue));
        }

        public IPromise<T> LastOrDefault(Func<T, int, IPromise<bool>> predicate, T defaultValue)
        {
            return _items.Then(list => LastOrDefaultAlgoritm.LastOrDefault(list, predicate, defaultValue));
        }

        public IPromise<T> LastOrDefault(Func<T, int, bool> predicate, T defaultValue)
        {
            return _items.Then(list => LastOrDefaultAlgoritm.LastOrDefault(list, predicate, defaultValue));
        }

        public IPromise<T> LastOrDefault(Func<T, IPromise<bool>> predicate, T defaultValue)
        {
            return _items.Then(list => LastOrDefaultAlgoritm.LastOrDefault(list, predicate, defaultValue));
        }

        public IPromise<T> LastOrDefault(Func<T, bool> predicate, T defaultValue)
        {
            return _items.Then(list => LastOrDefaultAlgoritm.LastOrDefault(list, predicate, defaultValue));
        }

        public IPromise<T> LastOrDefault(Func<T, int, IPromise<bool>> predicate)
        {
            return _items.Then(list => LastOrDefaultAlgoritm.LastOrDefault(list, predicate));
        }

        public IPromise<T> LastOrDefault(Func<T, int, bool> predicate)
        {
            return _items.Then(list => LastOrDefaultAlgoritm.LastOrDefault(list, predicate));
        }

        public IPromise<T> LastOrDefault(Func<T, IPromise<bool>> predicate)
        {
            return _items.Then(list => LastOrDefaultAlgoritm.LastOrDefault(list, predicate));
        }

        public IPromise<T> LastOrDefault(Func<T, bool> predicate)
        {
            return _items.Then(list => LastOrDefaultAlgoritm.LastOrDefault(list, predicate));
        }

        public IPromise<T> Last()
        {
            return _items.Then(list => LastAlgorithm.Last(list));
        }

        public IPromise<T> Last(Func<T, int, IPromise<bool>> predicate)
        {
            return _items.Then(list => LastAlgorithm.Last(list, predicate));
        }

        public IPromise<T> Last(Func<T, int, bool> predicate)
        {
            return _items.Then(list => LastAlgorithm.Last(list, predicate));
        }

        public IPromise<T> Last(Func<T, IPromise<bool>> predicate)
        {
            return _items.Then(list => LastAlgorithm.Last(list, predicate));
        }

        public IPromise<T> Last(Func<T, bool> predicate)
        {
            return _items.Then(list => LastAlgorithm.Last(list, predicate));
        }

        public IPromiseEnumerable<T> Concat(IPromiseEnumerable<T> other)
        {
            if (!(other is PromiseEnumerable<T>))
            {
                throw new InvalidOperationException();
            }

            return new PromiseEnumerable<T>(_items.Then(left => ((PromiseEnumerable<T>)other)._items.Then(right => ConcatEnumerator.Create(left, right))));
        }

        public IPromiseEnumerable<T> Concat(IEnumerable<T> other)
        {
            return Concat(new PromiseEnumerable<T>(this.Factory, other));
        }

        public IPromiseEnumerable<T> Concat(IPromise<IEnumerable<T>> other)
        {
            return Concat(new PromiseEnumerable<T>(other));
        }

        public IPromiseEnumerable<T> Concat(IPromise<ICollection<T>> other)
        {
            return Concat(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Concat(IPromise<IList<T>> other)
        {
            return Concat(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Concat(IPromise<ISet<T>> other)
        {
            return Concat(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Concat(IPromise<T[]> other)
        {
            return Concat(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Concat(IPromise<List<T>> other)
        {
            return Concat(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Concat(IPromise<HashSet<T>> other)
        {
            return Concat(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Concat(IPromise<SortedSet<T>> other)
        {
            return Concat(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Concat(IPromise<LinkedList<T>> other)
        {
            return Concat(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Concat(IPromise<Stack<T>> other)
        {
            return Concat(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Concat(IPromise<Queue<T>> other)
        {
            return Concat(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Reverse()
        {
            return new PromiseEnumerable<T>(_items.Then(list => ReverseAlgorithm.Reverse(list)));
        }

        public IPromise<int> Sum(Func<T, int, IPromise<int>> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<int> Sum(Func<T, int, int> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<int> Sum(Func<T, IPromise<int>> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<int> Sum(Func<T, int> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<long> Sum(Func<T, int, IPromise<long>> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<long> Sum(Func<T, int, long> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<long> Sum(Func<T, IPromise<long>> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<long> Sum(Func<T, long> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<float> Sum(Func<T, int, IPromise<float>> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<float> Sum(Func<T, int, float> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<float> Sum(Func<T, IPromise<float>> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<float> Sum(Func<T, float> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<double> Sum(Func<T, int, IPromise<double>> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<double> Sum(Func<T, int, double> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<double> Sum(Func<T, IPromise<double>> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<double> Sum(Func<T, double> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<decimal> Sum(Func<T, int, IPromise<decimal>> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<decimal> Sum(Func<T, int, decimal> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<decimal> Sum(Func<T, IPromise<decimal>> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<decimal> Sum(Func<T, decimal> action)
        {
            return this.Select(action).Sum();
        }

        public IPromise<double> Average(Func<T, int, IPromise<int>> action)
        {
            return this.Select(action).Average();
        }

        public IPromise<double> Average(Func<T, int, int> action)
        {
            return this.Select(action).Average();
        }

        public IPromise<double> Average(Func<T, IPromise<int>> action)
        {
            return this.Select(action).Average();
        }

        public IPromise<double> Average(Func<T, int> action)
        {
            return this.Select(action).Average();
        }

        public IPromise<double> Average(Func<T, int, IPromise<long>> action)
        {
            return this.Select(action).Average();
        }

        public IPromise<double> Average(Func<T, int, long> action)
        {
            return this.Select(action).Average();
        }

        public IPromise<double> Average(Func<T, IPromise<long>> action)
        {
            return this.Select(action).Average();
        }

        public IPromise<double> Average(Func<T, long> action)
        {
            return this.Select(action).Average();
        }

        public IPromise<double> Average(Func<T, int, IPromise<float>> action)
        {
            return this.Select(action).Average();
        }

        public IPromise<double> Average(Func<T, int, float> action)
        {
            return this.Select(action).Average();
        }

        public IPromise<double> Average(Func<T, IPromise<float>> action)
        {
            return this.Select(action).Average();
        }

        public IPromise<double> Average(Func<T, float> action)
        {
            return this.Select(action).Average();
        }

        public IPromise<double> Average(Func<T, int, IPromise<double>> action)
        {
            return this.Select(action).Average();
        }

        public IPromise<double> Average(Func<T, int, double> action)
        {
            return this.Select(action).Average();
        }

        public IPromise<double> Average(Func<T, IPromise<double>> action)
        {
            return this.Select(action).Average();
        }

        public IPromise<double> Average(Func<T, double> action)
        {
            return this.Select(action).Average();
        }

        public IPromise<double> Average(Func<T, int, IPromise<decimal>> action)
        {
            return this.Select(action).Average();
        }

        public IPromise<double> Average(Func<T, int, decimal> action)
        {
            return this.Select(action).Average();
        }

        public IPromise<double> Average(Func<T, IPromise<decimal>> action)
        {
            return this.Select(action).Average();
        }

        public IPromise<double> Average(Func<T, decimal> action)
        {
            return this.Select(action).Average();
        }

        public IPromiseEnumerable<T> Distinct(IEqualityComparer<T> comparer)
        {
            return new PromiseEnumerable<T>(_items.Then(list => DistinctEnumerator.Create(list, comparer)));
        }

        public IPromiseEnumerable<T> Distinct()
        {
            return new PromiseEnumerable<T>(_items.Then(list => DistinctEnumerator.Create(list)));
        }

        public IPromiseEnumerable<T> Union(IPromiseEnumerable<T> items, IEqualityComparer<T> comparer)
        {
            if (!(items is PromiseEnumerable<T>))
            {
                throw new ArgumentException();
            }

            return new PromiseEnumerable<T>(_items.Then(left => ((PromiseEnumerable<T>)items)._items.Then(right => UnionAlgorithm.Union(left, right, comparer))));
        }

        public IPromiseEnumerable<T> Union(IPromiseEnumerable<T> items)
        {
            if (!(items is PromiseEnumerable<T>))
            {
                throw new ArgumentException();
            }

            return new PromiseEnumerable<T>(_items.Then(left => ((PromiseEnumerable<T>)items)._items.Then(right => UnionAlgorithm.Union(left, right))));
        }

        public IPromiseEnumerable<T> Union(IEnumerable<T> other)
        {
            return Union(new PromiseEnumerable<T>(this.Factory, other));
        }

        public IPromiseEnumerable<T> Union(IPromise<IEnumerable<T>> other)
        {
            return Union(new PromiseEnumerable<T>(other));
        }

        public IPromiseEnumerable<T> Union(IPromise<ICollection<T>> other)
        {
            return Union(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Union(IPromise<IList<T>> other)
        {
            return Union(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Union(IPromise<ISet<T>> other)
        {
            return Union(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Union(IPromise<T[]> other)
        {
            return Union(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Union(IPromise<List<T>> other)
        {
            return Union(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Union(IPromise<HashSet<T>> other)
        {
            return Union(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Union(IPromise<SortedSet<T>> other)
        {
            return Union(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Union(IPromise<LinkedList<T>> other)
        {
            return Union(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Union(IPromise<Stack<T>> other)
        {
            return Union(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Union(IPromise<Queue<T>> other)
        {
            return Union(other.Cast<IEnumerable<T>>());
        }

        public IPromiseEnumerable<T> Union(IEnumerable<T> other, IEqualityComparer<T> comparer)
        {
            return Union(new PromiseEnumerable<T>(this.Factory, other), comparer);
        }

        public IPromiseEnumerable<T> Union(IPromise<IEnumerable<T>> other, IEqualityComparer<T> comparer)
        {
            return Union(new PromiseEnumerable<T>(other), comparer);
        }

        public IPromiseEnumerable<T> Union(IPromise<ICollection<T>> other, IEqualityComparer<T> comparer)
        {
            return Union(other.Cast<IEnumerable<T>>(), comparer);
        }

        public IPromiseEnumerable<T> Union(IPromise<IList<T>> other, IEqualityComparer<T> comparer)
        {
            return Union(other.Cast<IEnumerable<T>>(), comparer);
        }

        public IPromiseEnumerable<T> Union(IPromise<ISet<T>> other, IEqualityComparer<T> comparer)
        {
            return Union(other.Cast<IEnumerable<T>>(), comparer);
        }

        public IPromiseEnumerable<T> Union(IPromise<T[]> other, IEqualityComparer<T> comparer)
        {
            return Union(other.Cast<IEnumerable<T>>(), comparer);
        }

        public IPromiseEnumerable<T> Union(IPromise<List<T>> other, IEqualityComparer<T> comparer)
        {
            return Union(other.Cast<IEnumerable<T>>(), comparer);
        }

        public IPromiseEnumerable<T> Union(IPromise<HashSet<T>> other, IEqualityComparer<T> comparer)
        {
            return Union(other.Cast<IEnumerable<T>>(), comparer);
        }

        public IPromiseEnumerable<T> Union(IPromise<SortedSet<T>> other, IEqualityComparer<T> comparer)
        {
            return Union(other.Cast<IEnumerable<T>>(), comparer);
        }

        public IPromiseEnumerable<T> Union(IPromise<LinkedList<T>> other, IEqualityComparer<T> comparer)
        {
            return Union(other.Cast<IEnumerable<T>>(), comparer);
        }

        public IPromiseEnumerable<T> Union(IPromise<Stack<T>> other, IEqualityComparer<T> comparer)
        {
            return Union(other.Cast<IEnumerable<T>>(), comparer);
        }

        public IPromiseEnumerable<T> Union(IPromise<Queue<T>> other, IEqualityComparer<T> comparer)
        {
            return Union(other.Cast<IEnumerable<T>>(), comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, int, IPromise<TKey>> key, Func<T, int, IPromise<TValue>> value, IEqualityComparer<TKey> comparer)
        {
            return new PromiseEnumerable<IPromiseGrouping<TKey, TValue>>(_items.Then(list =>
            {
                var tuples = SelectEnumerator.Create(list, (item, index) => key(item, index).Then(k => value(item, index).Then(v => Tuple.Create(k, v))));
                var keys = DistinctEnumerator.Create(SelectEnumerator.Create(tuples, x => x.Item1));

                return SelectEnumerator.Create(keys, k => (IPromiseGrouping<TKey, TValue>)(new PromiseGrouping<T, TKey, TValue>(k, tuples, comparer)));
            }));
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, int, IPromise<TKey>> key, Func<T, int, TValue> value, IEqualityComparer<TKey> comparer)
        {
            return GroupBy(key, (x, i) => Factory.Value(value(x, i)), comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, int, TKey> key, Func<T, int, IPromise<TValue>> value, IEqualityComparer<TKey> comparer)
        {
            return GroupBy((x, i) => Factory.Value(key(x, i)), value, comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, int, TKey> key, Func<T, int, TValue> value, IEqualityComparer<TKey> comparer)
        {
            return GroupBy((x, i) => Factory.Value(key(x, i)), (x, i) => Factory.Value(value(x, i)), comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, int, IPromise<TKey>> key, Func<T, IPromise<TValue>> value, IEqualityComparer<TKey> comparer)
        {
            return GroupBy(key, (x, i) => value(x), comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, int, IPromise<TKey>> key, Func<T, TValue> value, IEqualityComparer<TKey> comparer)
        {
            return GroupBy(key, (x, i) => value(x), comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, int, TKey> key, Func<T, IPromise<TValue>> value, IEqualityComparer<TKey> comparer)
        {
            return GroupBy(key, (x, i) => value(x), comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, int, TKey> key, Func<T, TValue> value, IEqualityComparer<TKey> comparer)
        {
            return GroupBy(key, (x, i) => value(x), comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, IPromise<TKey>> key, Func<T, int, IPromise<TValue>> value, IEqualityComparer<TKey> comparer)
        {
            return GroupBy((x, i) => key(x), value, comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, IPromise<TKey>> key, Func<T, int, TValue> value, IEqualityComparer<TKey> comparer)
        {
            return GroupBy((x, i) => key(x), value, comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, TKey> key, Func<T, int, IPromise<TValue>> value, IEqualityComparer<TKey> comparer)
        {
            return GroupBy((x, i) => key(x), value, comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, TKey> key, Func<T, int, TValue> value, IEqualityComparer<TKey> comparer)
        {
            return GroupBy((x, i) => key(x), value, comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, IPromise<TKey>> key, Func<T, IPromise<TValue>> value, IEqualityComparer<TKey> comparer)
        {
            return GroupBy((x, i) => key(x), (x, i) => value(x), comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, IPromise<TKey>> key, Func<T, TValue> value, IEqualityComparer<TKey> comparer)
        {
            return GroupBy((x, i) => key(x), (x, i) => value(x), comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, TKey> key, Func<T, IPromise<TValue>> value, IEqualityComparer<TKey> comparer)
        {
            return GroupBy((x, i) => key(x), (x, i) => value(x), comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, TKey> key, Func<T, TValue> value, IEqualityComparer<TKey> comparer)
        {
            return GroupBy((x, i) => key(x), (x, i) => value(x), comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, int, IPromise<TKey>> key, Func<T, int, IPromise<TValue>> value)
        {
            return GroupBy(key, value, EqualityComparer<TKey>.Default);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, int, IPromise<TKey>> key, Func<T, int, TValue> value)
        {
            return GroupBy(key, value, EqualityComparer<TKey>.Default);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, int, TKey> key, Func<T, int, IPromise<TValue>> value)
        {
            return GroupBy(key, value, EqualityComparer<TKey>.Default);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, int, TKey> key, Func<T, int, TValue> value)
        {
            return GroupBy(key, value, EqualityComparer<TKey>.Default);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, int, IPromise<TKey>> key, Func<T, IPromise<TValue>> value)
        {
            return GroupBy(key, value, EqualityComparer<TKey>.Default);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, int, IPromise<TKey>> key, Func<T, TValue> value)
        {
            return GroupBy(key, value, EqualityComparer<TKey>.Default);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, int, TKey> key, Func<T, IPromise<TValue>> value)
        {
            return GroupBy(key, value, EqualityComparer<TKey>.Default);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, int, TKey> key, Func<T, TValue> value)
        {
            return GroupBy(key, value, EqualityComparer<TKey>.Default);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, IPromise<TKey>> key, Func<T, int, IPromise<TValue>> value)
        {
            return GroupBy(key, value, EqualityComparer<TKey>.Default);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, IPromise<TKey>> key, Func<T, int, TValue> value)
        {
            return GroupBy(key, value, EqualityComparer<TKey>.Default);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, TKey> key, Func<T, int, IPromise<TValue>> value)
        {
            return GroupBy(key, value, EqualityComparer<TKey>.Default);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, TKey> key, Func<T, int, TValue> value)
        {
            return GroupBy(key, value, EqualityComparer<TKey>.Default);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, IPromise<TKey>> key, Func<T, IPromise<TValue>> value)
        {
            return GroupBy(key, value, EqualityComparer<TKey>.Default);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, IPromise<TKey>> key, Func<T, TValue> value)
        {
            return GroupBy(key, value, EqualityComparer<TKey>.Default);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, TKey> key, Func<T, IPromise<TValue>> value)
        {
            return GroupBy(key, value, EqualityComparer<TKey>.Default);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, TValue>> GroupBy<TKey, TValue>(Func<T, TKey> key, Func<T, TValue> value)
        {
            return GroupBy(key, value, EqualityComparer<TKey>.Default);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, T>> GroupBy<TKey>(Func<T, int, IPromise<TKey>> key, IEqualityComparer<TKey> comparer)
        {
            return GroupBy(key, x => x, comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, T>> GroupBy<TKey>(Func<T, int, TKey> key, IEqualityComparer<TKey> comparer)
        {
            return GroupBy(key, x => x, comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, T>> GroupBy<TKey>(Func<T, IPromise<TKey>> key, IEqualityComparer<TKey> comparer)
        {
            return GroupBy(key, x => x, comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, T>> GroupBy<TKey>(Func<T, TKey> key, IEqualityComparer<TKey> comparer)
        {
            return GroupBy(key, x => x, comparer);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, T>> GroupBy<TKey>(Func<T, int, IPromise<TKey>> key)
        {
            return GroupBy(key, x => x);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, T>> GroupBy<TKey>(Func<T, int, TKey> key)
        {
            return GroupBy(key, x => x);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, T>> GroupBy<TKey>(Func<T, IPromise<TKey>> key)
        {
            return GroupBy(key, x => x);
        }

        public IPromiseEnumerable<IPromiseGrouping<TKey, T>> GroupBy<TKey>(Func<T, TKey> key)
        {
            return GroupBy(key, x => x);
        }

        public PromiseFactory Factory
        {
            get
            {
                return _items.Factory;
            }
        }
    }
}
