#if DEBUG && !_NDIAGNOSTICS
using MooPromise.DataStructure.Debug;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MooPromise.DataStructure
{
#if DEBUG && !_NDIAGNOSTICS
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DebuggerTypeProxy(typeof(ListDebugView))]
#endif
    internal class Heap<T> : IQueue<T>
    {
        private Func<T, T, int> _comparer;
        private IList<T> _items;

#if DEBUG && !_NDIAGNOSTICS
        private string DebuggerDisplay
        {
            get
            {
                return "Count = " + _items.Count;
            }
        }
#endif

        public Heap(Func<T, T, int> comparer)
        {
            this._comparer = comparer;
            this._items = new ArrayList<T>();
        }

        public Heap(IComparer<T> comparer)
            : this(new Func<T, T, int>((x, y) => comparer.Compare(x, y)))
        {

        }

        public Heap() : this(Comparer<T>.Default)
        {

        }

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return _items.IsReadOnly;
            }
        }

        private void Swap(int i, int j)
        {
            T temp = _items[i];
            _items[i] = _items[j];
            _items[j] = temp;
        }

        public void Add(T item)
        {
            _items.Add(item);

            int i = _items.Count - 1;
            
            while (i > 0)
            {
                int j = (i - 1) / 2;

                if (_comparer(_items[i], _items[j]) < 0)
                {
                    Swap(i, j);
                    i = j;
                }
                else
                {
                    break;
                }
            }
        }

        private void RemoveAt(int i)
        {
            if (i < 0 || i >= _items.Count)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            if (i == (_items.Count - 1))
            {
                _items.RemoveAt(i);
            }
            else
            {
                Swap(i, _items.Count - 1);
                _items.RemoveAt(_items.Count - 1);

                int j = (i * 2) + 1;
                while (j < _items.Count)
                {
                    int k = j;

                    if (((j + 1) < _items.Count) && (_comparer(_items[j + 1], _items[j]) < 0))
                    {
                        k = j + 1;
                    }

                    if (_comparer(_items[k], _items[i]) < 0)
                    {
                        Swap(i, k);
                        i = k;
                        j = (i * 2) + 1;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public bool Remove(T item)
        {
            int index = _items.IndexOf(item);

            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T Pop()
        {
            T result = _items[0];
            RemoveAt(0);
            return result;
        }

        public T Peek()
        {
            return _items[0];
        }

        public bool TryPop(out T value)
        {
            if (_items.Count < 1)
            {
                value = default(T);
                return false;
            }

            value = Pop();
            return true;
        }
    }
}
