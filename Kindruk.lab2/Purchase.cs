using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kindruk.lab2
{
    public class Purchase<T> : IPurchase<T> where T : IMerchandise
    {
        private const int BaseLength = 40;
        public const string IndexOutOfRange = "Индекс не является допутимым.";
        public const string TooFewSpace = "Все элементы коллекции не могут помеситься в массив.";

        private int _maxLength = BaseLength;
        private T[] _items = new T[BaseLength];
        private int _count;
        private bool _disposed;

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
                if (index < Count && index >= 0)
                    return _items[index];
                throw new ArgumentOutOfRangeException("index", IndexOutOfRange);
            }
            set
            {
                if (index < Count && index >= 0)
                    _items[index] = value;
                throw new ArgumentOutOfRangeException("index", IndexOutOfRange);
            }
        }

        #endregion

        private class PurchaseEnumerator : IEnumerator<T>
        {
            private readonly T[] _data;
            private int _currentPosition = -1;
            private bool _disposed;

            public PurchaseEnumerator(T[] values, int count)
            {
                _data = new T[count];
                for (var i = 0; i < count; i++)
                    _data[i] = values[i];
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
                Dispose(true);
            }

            private void Dispose(bool disposing)
            {
                if (_disposed) return;
                if (disposing)
                {
                    foreach (var item in _data)
                    {
                        item.Dispose();
                    }
                }
                _disposed = true;
            }

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
            return new PurchaseEnumerator(_items, Count);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            if (IsReadOnly)
                throw new NotSupportedException();
            Count++;
            _items[Count - 1] = item;
        }

        public void Clear()
        {
            if (IsReadOnly)
                throw new NotSupportedException();
            _items = new T[BaseLength];
        }

        public bool Contains(T item)
        {
            for (var i = 0; i < Count; i++)
                if (_items[i].Equals(item))
                    return true;
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex >= Count || arrayIndex < 0)
                throw new ArgumentOutOfRangeException("arrayIndex", IndexOutOfRange);
            if (array == null)
                throw new ArgumentNullException("array");
            if (array.Length < Count - arrayIndex)
                throw new ArgumentException(TooFewSpace);
            for (var i = arrayIndex; i < Count; i++)
                array[i - arrayIndex] = _items[i];
        }

        public bool Remove(T item)
        {
            if (IsReadOnly)
                throw new NotSupportedException();
            for (var i = 0; i < Count; i++)
                if (_items[i].Equals(item))
                {
                    RemoveAt(i);
                    return true;
                }
            return false;
        }

        public int IndexOf(T item)
        {
            for (var i = 0; i < Count; i++)
                if (_items[i].Equals(item))
                    return i;
            return -1;
        }

        public void Insert(int index, T item)
        {
            if (IsReadOnly)
                throw new NotSupportedException();
            if (index > Count || index < 0)
                throw new ArgumentOutOfRangeException("index", IndexOutOfRange);
            Count++;
            for (var i = index; i < Count - 1; i++)
                _items[i + 1] = _items[i];
            _items[index] = item;
        }

        public void RemoveAt(int index)
        {
            if (IsReadOnly)
                throw new NotSupportedException();
            if (index >= Count || index < 0)
                throw new ArgumentOutOfRangeException("index", IndexOutOfRange);
            for (var i = index; i < Count; i++)
                _items[i] = _items[i - 1];
            Count--;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                foreach (var item in _items)
                {
                    item.Dispose();
                }
            }
            _disposed = true;
        }

        ~Purchase()
        {
            Dispose(false);
        }

        public double TotalCost()
        {
            return this.Sum(item => item.Cost());
        }
    }
}
