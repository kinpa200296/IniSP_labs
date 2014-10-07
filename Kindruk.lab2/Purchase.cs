using System;
using System.Collections;
using System.Collections.Generic;

namespace Kindruk.lab2
{
    class Purchase<T> : IPurchase<T> where T : IMerchandise
    {
        private const int BaseLength = 40;

        private int _maxLength = BaseLength;
        private T[] _items = new T[BaseLength];
        private int _count;

        #region properties

        private int MaxLength
        {
            get { return _maxLength; }
            set
            {
                Array.Resize(ref _items, value);
                _maxLength = value;
            }
        }

        public int Count 
        {
            get { return _count; }
            private set
            {
                if (value > MaxLength)
                    MaxLength *= 2;
                _count = value;
            }
        }

        public bool IsReadOnly 
        {
            get { return false; }
        }

        public T this[int index]
        {
            get
            {
                if (index < Count && index > 0)
                    return _items[index];
                throw new IndexOutOfRangeException();
            }
            set
            {
                if (index < Count && index > 0)
                    _items[index] = value;
                throw new IndexOutOfRangeException();
            }
        }

        #endregion

        private class PurchaseEnumerator : IEnumerator<T>
        {
            private readonly T[] _data;
            private int _currentPosition;

            public PurchaseEnumerator(T[] values)
            {
                _data = new T[values.Length];
                values.CopyTo(_data, 0);
            }
            
            public void Dispose(){}

            public bool MoveNext()
            {
                _currentPosition++;
                return _currentPosition < _data.Length;
            }

            public void Reset()
            {
                _currentPosition = -1;
            }

            public T Current 
            {
                get { return _data[_currentPosition]; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new PurchaseEnumerator(_items);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
