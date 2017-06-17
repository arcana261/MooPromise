#if DEBUG && !_NDIAGNOSTICS
using MooPromise.DataStructure.Debug;
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Collections;

namespace MooPromise.DataStructure
{
#if DEBUG && !_NDIAGNOSTICS
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DebuggerTypeProxy(typeof(ListDebugView))]
#endif
    internal class ArrayList<T> : IList<T>
    {
#if DEBUG && !_NDIAGNOSTICS
        private string DebuggerDisplay
        {
            get
            {
                return "Count = " + _length;
            }
        }
#endif

        private T[] _list;
        private int _length;
        private int _changeCounter;

        public ArrayList()
        {
            _list = null;
            _length = 0;
            _changeCounter = 0;
        }

        private void ResizeUnchecked(int newCapacity)
        {
            T[] newArray = new T[newCapacity];

            for (int i = 0; i < _length; i++)
            {
                newArray[i] = _list[i];
            }

            _list = newArray;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _length)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                return _list[index];
            }

            set
            {
                if (index < 0 || index >= _length)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                _list[index] = value;
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

        public void Add(T item)
        {
            if (_list == null)
            {
                _list = new T[1];
                _list[0] = item;
                _length = 1;
            }
            else
            {
                if (_length == _list.Length)
                {
                    ResizeUnchecked(_list.Length * 2);
                }

                _list[_length++] = item;
            }

            _changeCounter = _changeCounter + 1;
        }

        public void Clear()
        {
            _list = null;
            _length = 0;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 0; i < _length && arrayIndex < array.Length; i++)
            {
                array[arrayIndex++] = _list[i];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            int counter = _changeCounter;

            for (int i = 0; i < _length; i++)
            {
                yield return _list[i];

                if (counter != _changeCounter)
                {
                    throw new InvalidProgramException("list changed");
                }
            }
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < _length; i++)
            {
                if (Object.Equals(item, _list[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index > _length)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            Add(item);

            int i = _length - 1;

            while (i > index)
            {
                int j = i - 1;

                T temp = _list[i];
                _list[i] = _list[j];
                _list[j] = temp;

                i = j;
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

            return false;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _length)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            int i = index;
            int j = index + 1;

            while (j < _length)
            {
                T temp = _list[i];
                _list[i] = _list[j];
                _list[j] = temp;

                i = j;
                j = j + 1;
            }

            _list[i] = default(T);
            _length = _length - 1;

            if (_length <= (_list.Length / 4))
            {
                if (_length < 1)
                {
                    _list = null;
                }
                else
                {
                    ResizeUnchecked(_list.Length / 2);
                }
            }

            _changeCounter = _changeCounter + 1;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
