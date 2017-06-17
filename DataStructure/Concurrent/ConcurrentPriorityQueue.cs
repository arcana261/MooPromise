#if DEBUG && !_NDIAGNOSTICS
using MooPromise.DataStructure.Debug;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace MooPromise.DataStructure.Concurrent
{
#if DEBUG && !_NDIAGNOSTICS
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DebuggerTypeProxy(typeof(ListDebugView))]
#endif
    internal class ConcurrentPriorityQueue<T> : IPriorityQueue<T>
    {
        private IPriorityQueue<T> _items;

#if DEBUG && !_NDIAGNOSTICS
        private string DebuggerDisplay
        {
            get
            {
                return "Count = " + _items.Count;
            }
        }
#endif

        public ConcurrentPriorityQueue()
        {
            _items = new PriorityQueue<T>();
        }

        public int Count
        {
            get
            {
                lock (this)
                {
                    return _items.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                lock (this)
                {
                    return _items.IsReadOnly;
                }
            }
        }

        public void Add(T item)
        {
            lock (this)
            {
                _items.Add(item);
            }
        }

        public void Add(T item, int priority)
        {
            lock (this)
            {
                _items.Add(item, priority);
            }
        }

        public void Clear()
        {
            lock (this)
            {
                _items.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (this)
            {
                return _items.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (this)
            {
                _items.CopyTo(array, arrayIndex);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotSupportedException();
        }

        public bool Remove(T item)
        {
            lock (this)
            {
                return _items.Remove(item);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotSupportedException();
        }

        public T Peek()
        {
            lock (this)
            {
                return _items.Peek();
            }
        }

        public T Pop()
        {
            lock (this)
            {
                return _items.Pop();
            }
        }

        public bool TryPop(out T value)
        {
            lock (this)
            {
                return _items.TryPop(out value);
            }
        }
    }
}
