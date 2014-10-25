using System;
using System.Collections;
using System.Collections.Generic;

namespace Kindruk.lab3
{
    public class LinkedList<T> : ILinkedList<T> where T : class, IDisposable, IEquatable<T>
    {
        private readonly ILinkedListNode<T> _emptyElement = new LinkedListNode<T>();
        private int _count;
        private bool _disposed;
        

        #region constructors
        public LinkedList()
        {
            Last = _emptyElement;
        }

        public LinkedList(IEnumerable<T> data)
        {
            Last = _emptyElement;
            foreach (var item in data)
            {
                AddAfter(Last, item);
            }
        }
        #endregion

        #region properties
        public ILinkedListNode<T> First
        {
            get { return _emptyElement.Next; }
        }

        public ILinkedListNode<T> Last { get; private set; }

        public int Count
        {
            get { return _count; }
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");
                _count = value;
            }
        }

        public bool IsReadOnly { get { return false; } }

        public T this[int index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index");
                if (index >= Count)
                    throw new ArgumentOutOfRangeException("index");
                foreach (var item in this)
                {
                    if (index == 0)
                        return item;
                    index--;
                }
                return null;
            }
            set
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index");
                if (index >= Count)
                    throw new ArgumentOutOfRangeException("index");
                for (var node = First; node != null; node = node.Next)
                {
                    if (index == 0)
                        node.Data = value;
                    index--;
                }
            }
        }
        #endregion

        private class LinkedListEnumerator : IEnumerator<T>
        {
            private readonly LinkedList<T> _list;
            private ILinkedListNode<T> _currentelement;

            public T Current { get { return _currentelement.Data; } }

            public LinkedListEnumerator(LinkedList<T> list)
            {
                _list = list;
                _currentelement = _list._emptyElement;
            }

            public void Dispose() { }

            public bool MoveNext()
            {
                _currentelement = _currentelement.Next;
                return _currentelement != null;
            }

            public void Reset()
            {
                _currentelement = _list._emptyElement;
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new LinkedListEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddAfter(ILinkedListNode<T> item, T data)
        {
            if (IndexOf(item.Data) == -1 && item != _emptyElement)
                throw new ArgumentException();
            var element = new LinkedListNode<T>(data) {Next = item.Next, Previous = item};
            item.Next = element;
            if (ReferenceEquals(item, Last))
                Last = element;
            else element.Next.Previous = element;
            Count++;
        }

        public void Add(T data)
        {
            AddAfter(Last, data);
        }

        public void AddFirst(T data)
        {
            AddAfter(_emptyElement, data);
        }

        public void Delete(ILinkedListNode<T> item)
        {
            if (IndexOf(item.Data) == -1)
                throw new ArgumentException();
            if (ReferenceEquals(Last, item))
                Last = item.Previous;
            else item.Next.Previous = item.Previous;
            item.Previous.Next = item.Next;
            item.Dispose();
            Count--;
        }

        public int IndexOf(T item)
        {
            var i = 0;
            foreach (var node in this)
            {
                if (node.Equals(item))
                    return i;
                i++;
            }
            return -1;
        }

        public void Clear()
        {
            foreach (var item in this)
            {
                item.Dispose();
            }
            Last = _emptyElement;
            _emptyElement.Next = _emptyElement.Previous = null;
            Count = 0;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (ReferenceEquals(array, null))
                throw new ArgumentNullException("array");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("arrayIndex");
            if (array.Length - arrayIndex < Count)
                throw new ArgumentException();
            foreach (var item in this)
            {
                array[arrayIndex] = item;
                arrayIndex++;
            }
        }

        public ILinkedListNode<T> GetNodeByData(T data)
        {
            if (IndexOf(data) == -1)
                throw new ArgumentException();
            for (var node = First; node != null; node = node.Next)
            {
                if (data.Equals(node.Data))
                    return node;
            }
            return null;
        }

        public ILinkedListNode<T> GetNodeByIndex(int index)
        {
            return GetNodeByData(this[index]);
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
                foreach (var item in this)
                {
                    item.Dispose();
                }
            }
            _disposed = true;
        }

        ~LinkedList()
        {
            Dispose(false);
        }
    }
}
