using MooPromise.DataStructure.Debug;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MooPromise.DataStructure
{
#if DEBUG
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DebuggerTypeProxy(typeof(ListDebugView))]
#endif
    internal class Queue<T> : IQueue<T>
    {
        private DoubleEndedList<T> _items;

#if DEBUG
        private string DebuggerDisplay
        {
            get
            {
                return "Count = " + _items.Count;
            }
        }
#endif

        public Queue()
        {
            this._items = new DoubleEndedList<T>();
        }

        public void Enqueue(T item)
        {
            _items.Add(item);
        }

        public T Peek()
        {
            return _items[0];
        }

        public T Pop()
        {
            T ret = _items[0];
            _items.RemoveAt(0);
            return ret;
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
                return false;
            }
        }

        public void Add(T item)
        {
            Enqueue(item);
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
            return _items.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
