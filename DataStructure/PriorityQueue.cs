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
    internal class PriorityQueue<T> : IPriorityQueue<T>
    {
        private Heap<Node> _items;

#if DEBUG && !_NDIAGNOSTICS
        private string DebuggerDisplay
        {
            get
            {
                return "Count = " + _items.Count;
            }
        }
#endif

        public PriorityQueue()
        {
            this._items = new Heap<Node>();
        }

        private class Node : IComparable<Node>, IComparable, IEquatable<Node>
        {
            public T Item
            {
                get;
                private set;
            }

            public int Priority
            {
                get;
                private set;
            }

            public Node(T item, int priority)
            {
                this.Item = item;
                this.Priority = priority;
            }

            public int CompareTo(object obj)
            {
                if (obj == null)
                {
                    return -1;
                }

                if (!(obj is Node))
                {
                    return -1;
                }

                return CompareTo(obj as Node);
            }

            public int CompareTo(Node other)
            {
                if (other == null)
                {
                    return -1;
                }

                if (Priority < other.Priority)
                {
                    return 1;
                }

                if (other.Priority < Priority)
                {
                    return -1;
                }

                return 0;
            }

            public bool Equals(Node other)
            {
                if (other == null)
                {
                    return false;
                }

                return Object.Equals(Item, other.Item);
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }

                if (!(obj is Node))
                {
                    return false;
                }

                return Equals(obj as Node);
            }

            public override int GetHashCode()
            {
                if (Item == null)
                {
                    return 0;
                }

                return Item.GetHashCode();
            }

            public override string ToString()
            {
                if (Item == null)
                {
                    return null;
                }

                return Item.ToString();
            }
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

        public void Add(T item, int priority)
        {
            _items.Add(new Node(item, priority));
        }

        public void Add(T item)
        {
            Add(item, int.MinValue);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(T item)
        {
            return _items.Contains(new Node(item, -1));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var x in _items)
            {
                if (arrayIndex >= array.Length)
                {
                    break;
                }

                array[arrayIndex++] = x.Item;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.Select(x => x.Item).GetEnumerator();
        }

        public bool Remove(T item)
        {
            return _items.Remove(new Node(item, -1));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T Peek()
        {
            return _items.Peek().Item;
        }

        public T Pop()
        {
            return _items.Pop().Item;
        }

        public bool TryPop(out T value, out int priority)
        {
            Node x;

            if (_items.TryPop(out x))
            {
                value = x.Item;
                priority = x.Priority;
                return true;
            }

            value = default(T);
            priority = 0;
            return false;
        }

        public bool TryPop(out T value)
        {
            int priority;

            return TryPop(out value, out priority);
        }
    }
}
