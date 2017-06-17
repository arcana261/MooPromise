#if DEBUG && !_NDIAGNOSTICS
using MooPromise.DataStructure.Debug;
#endif
using System;
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
    internal class DoubleEndedList<T> : IList<T>
    {
        private T[] _array;
        private int _start;
        private int _length;
        private int _changeId;

#if DEBUG && !_NDIAGNOSTICS
        private string DebuggerDisplay
        {
            get
            {
                return "Count = " + _length;
            }
        }
#endif

        public DoubleEndedList()
        {
            _changeId = 0;
            Clear();
        }

        private int Capacity
        {
            get
            {
                if (_array == null)
                {
                    return 0;
                }
                else
                {
                    return _array.Length;
                }
            }
        }

        private void Resize(int newCapacity)
        {
            if (newCapacity < 1)
            {
                _array = null;
                _start = 0;
            }
            else
            {
                T[] newArray = new T[newCapacity];

                if (_array != null)
                {
                    int endIndex = (_start + _length) % _array.Length;

                    if (_start < endIndex)
                    {
                        for (int i = _start; i < endIndex; i++)
                        {
                            newArray[i - _start] = _array[i];
                        }
                    }
                    else if (_length > 0)
                    {
                        int index = 0;

                        for (int i = _start; i < _array.Length; i++)
                        {
                            newArray[index++] = _array[i];
                        }

                        for (int i = 0; i < endIndex; i++)
                        {
                            newArray[index++] = _array[i];
                        }
                    }
                }

                _array = newArray;
                _start = 0;
            }
        }

        private void EnsureExpand(int itemsToAdd)
        {
            int newLength = _length + itemsToAdd;

            if (newLength > Capacity)
            {
                int newCapacity = Capacity * 2;

                if (newCapacity < 1)
                {
                    newCapacity = 1;
                }

                while (newCapacity < newLength)
                {
                    newCapacity = newCapacity * 2;
                }

                Resize(newCapacity);
            }
        }

        private void EnsureContract()
        {
            int threshold = Capacity / 4;

            if (_length <= threshold)
            {
                int newCapacity = Capacity / 2;
                threshold = newCapacity / 4;

                while (_length <= threshold && threshold > 0)
                {
                    newCapacity = Capacity / 2;
                    threshold = newCapacity / 4;
                }

                Resize(newCapacity);
            }
        }

        public int IndexOf(T item)
        {
            if (_array != null)
            {
                int endIndex = (_start + _length) % _array.Length;

                if (_start < endIndex)
                {
                    for (int i = _start; i < endIndex; i++)
                    {
                        if (Object.Equals(_array[i], item))
                        {
                            return i - _start;
                        }
                    }

                    return -1;
                }
                else if (_length > 0)
                {
                    int index = 0;

                    for (int i = _start; i < _array.Length; i++)
                    {
                        if (Object.Equals(_array[i], item))
                        {
                            return index;
                        }

                        index = index + 1;
                    }

                    for (int i = 0; i < endIndex; i++)
                    {
                        if (Object.Equals(_array[i], item))
                        {
                            return index;
                        }

                        index = index + 1;
                    }

                    return -1;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        private void InsertBack(T item)
        {
            _array[(_start + _length) % _array.Length] = item;
            _length = _length + 1;
        }

        private void InsertFront(T item)
        {
            _start = (_start - 1 + _array.Length) % _array.Length;
            _array[_start] = item;
            _length = _length + 1;
        }

        private void InsertUsingBacksert(int index, T item)
        {
            InsertBack(item);

            int realIndex = (_start + _length - 1) % _array.Length;
            int rounds = _length - 1 - index;

            for (int i = 0; i < rounds; i++)
            {
                int next = (realIndex - 1 + _array.Length) % _array.Length;

                T temp = _array[realIndex];
                _array[realIndex] = _array[next];
                _array[next] = temp;

                realIndex = next;
            }
        }

        private void InsertUsingFrontsert(int index, T item)
        {
            InsertFront(item);

            int realIndex = _start;

            for (int i = 0; i < index; i++)
            {
                int next = (realIndex + 1) % _array.Length;

                T temp = _array[realIndex];
                _array[realIndex] = _array[next];
                _array[next] = temp;

                realIndex = next;
            }
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index > _length)
            {
                throw new IndexOutOfRangeException();
            }

            _changeId = _changeId + 1;
            EnsureExpand(1);

            if (index == 0)
            {
                InsertFront(item);
            }
            else if (index == _length)
            {
                InsertBack(item);
            }
            else
            {
                if (index <= (_length / 2))
                {
                    InsertUsingFrontsert(index, item);
                }
                else
                {
                    InsertUsingBacksert(index, item);
                }
            }
        }

        private void RemoveBack()
        {
            _length = _length - 1;
            _array[(_start + _length) % _array.Length] = default(T);
        }

        private void RemoveFront()
        {
            _array[_start] = default(T);
            _start = (_start + 1) % _array.Length;
            _length = _length - 1;
        }

        private void RemoveUsingFront(int index)
        {
            int realIndex = (_start + index) % _array.Length;

            for (int i = 0; i < index; i++)
            {
                int next = (realIndex - 1 + _array.Length) % _array.Length;

                T temp = _array[realIndex];
                _array[realIndex] = _array[next];
                _array[next] = temp;

                realIndex = next;
            }

            RemoveFront();
        }

        private void RemoveUsingBack(int index)
        {
            int realIndex = (_start + index) % _array.Length;
            int rounds = _length - index - 1;

            for (int i = 0; i < rounds; i++)
            {
                int next = (realIndex + 1) % _array.Length;

                T temp = _array[realIndex];
                _array[realIndex] = _array[next];
                _array[next] = temp;

                realIndex = next;
            }

            RemoveBack();
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _length)
            {
                throw new IndexOutOfRangeException();
            }

            _changeId = _changeId + 1;

            if (index == 0)
            {
                RemoveFront();
            }
            else if (index == _length - 1)
            {
                RemoveBack();
            }
            else
            {
                if (index <= (_length / 2))
                {
                    RemoveUsingFront(index);
                }
                else
                {
                    RemoveUsingBack(index);
                }
            }

            EnsureContract();
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _length)
                {
                    throw new IndexOutOfRangeException();
                }

                return _array[(_start + index) % _array.Length];
            }
            set
            {
                if (index < 0 || index >= _length)
                {
                    throw new IndexOutOfRangeException();
                }

                _changeId = _changeId + 1;
                _array[(_start + index) % _array.Length] = value;
            }
        }

        public void Add(T item)
        {
            Insert(_length, item);
        }

        public void Clear()
        {
            _changeId = _changeId + 1;
            _length = 0;
            Resize(0);
        }

        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (_array != null)
            {
                int endIndex = (_start + _length) % _array.Length;
                int targetIndex = arrayIndex;

                if (_start < endIndex)
                {
                    for (int i = _start; i < endIndex && targetIndex < array.Length; i++)
                    {
                        array[targetIndex++] = _array[i];
                    }
                }
                else if (_length > 0)
                {
                    for (int i = _start; i < _array.Length; i++)
                    {
                        array[targetIndex++] = _array[i];
                    }

                    for (int i = 0; i < endIndex; i++)
                    {
                        array[targetIndex++] = _array[i];
                    }
                }
            }
        }

        public int Count
        {
            get
            {
                return _length;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);

            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (_array != null)
            {
                int currentChangeId = _changeId;
                int endIndex = (_start + _length) % _array.Length;

                if (endIndex > _start)
                {
                    for (int i = _start; i < endIndex; i++)
                    {
                        if (_changeId != currentChangeId)
                        {
                            throw new InvalidOperationException("list changed while iterating");
                        }

                        yield return _array[i];
                    }
                }
                else if (_length > 0)
                {
                    for (int i = _start; i < _array.Length; i++)
                    {
                        if (_changeId != currentChangeId)
                        {
                            throw new InvalidOperationException("list changed while iterating");
                        }

                        yield return _array[i];
                    }

                    for (int i = 0; i < endIndex; i++)
                    {
                        if (_changeId != currentChangeId)
                        {
                            throw new InvalidOperationException("list changed while iterating");
                        }

                        yield return _array[i];
                    }
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
