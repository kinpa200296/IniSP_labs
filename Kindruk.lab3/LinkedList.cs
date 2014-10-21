using System;
using System.Collections;
using System.Collections.Generic;

namespace Kindruk.lab3
{
    class LinkedList<T> : ILinkedList<T> where T : class, IDisposable, IEquatable<T>
    {
        private readonly LinkedListNode<T> _emptyElement = new LinkedListNode<T>(null);
        private int _count;
        private bool _disposed;
        public const string ThereIsNoSuchArgument = "Такого элемента нет в списке.";
        public const string CountOutOfRangeMessage = "Количество элементов в списке не может быть отрицательным.";
        public const string ArrayTooSmall = "Все элементы коллекции не могут поместиться в массив.";
        public const string IndexOutOfRange = "Индекс должен быть целым числом, большим нуля.";
        public const string IndexTooBig = "Индекс не должен превышать размер списка.";
        public const string ArgumentNull = "Аргумент является пустой ссылкой.";

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

        public LinkedListNode<T> First
        {
            get { return _emptyElement.Next; }
        }

        public LinkedListNode<T> Last { get; private set; }

        public int Count
        {
            get { return _count; }
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", CountOutOfRangeMessage);
                _count = value;
            }
        }

        public bool IsReadOnly { get { return false; } }

        public T this[int index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index", IndexOutOfRange);
                if (index >= Count)
                    throw new ArgumentOutOfRangeException("index", IndexTooBig);
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
                    throw new ArgumentOutOfRangeException("index", IndexOutOfRange);
                if (index >= Count)
                    throw new ArgumentOutOfRangeException("index", IndexTooBig);
                for (var node = First.Next; node != null; node = node.Next)
                {
                    if (index == 0)
                        node.Data = value;
                    index--;
                }
            }
        }

        private class LinkedListEnumerator : IEnumerator<T>
        {
            private readonly LinkedList<T> _list;
            private LinkedListNode<T> _currentelement;

            public T Current { get { return _currentelement.Data; } }

            public LinkedListEnumerator(LinkedList<T> list)
            {
                _list = list;
                _currentelement = _list.First;
            }

            public void Dispose() { }

            public bool MoveNext()
            {
                _currentelement = _currentelement.Next;
                return _currentelement != null;
            }

            public void Reset()
            {
                _currentelement = _list.First;
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

        public void AddAfter(LinkedListNode<T> item, T data)
        {
            var element = new LinkedListNode<T>(data) {Next = item.Next, Previous = item};
            item.Next = element;
            element.Next.Previous = element;
            Count++;
            if (ReferenceEquals(item, Last))
                Last = element;
        }

        public void AddFirst(T data)
        {
            AddAfter(_emptyElement, data);
        }

        public void Delete(LinkedListNode<T> item)
        {
            if (IndexOf(item.Data) == -1)
                throw new ArgumentException(ThereIsNoSuchArgument);
            if (ReferenceEquals(Last, item))
                Last = item.Previous;
            item.Previous.Next = item.Next;
            item.Next.Previous = item.Previous;
            item.Dispose();
            Count--;
        }

        public int IndexOf(T item)
        {
            var i = 0;
            foreach (var node in this)
            {
                if (node == item)
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
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", ArgumentNull);
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("arrayIndex", IndexOutOfRange);
            if (array.Length - arrayIndex < Count)
                throw new ArgumentException(ArrayTooSmall);
            foreach (var item in this)
            {
                array[arrayIndex] = item;
                arrayIndex++;
            }
        }

        public LinkedListNode<T> GetNodeByData(T data)
        {
            if (IndexOf(data) == -1)
                throw new ArgumentException(ThereIsNoSuchArgument);
            for (var node = First.Next; node != null; node = node.Next)
            {
                if (data == node.Data)
                    return node;
            }
            return null;
        }

        public LinkedListNode<T> GetNodeByIndex(int index)
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
