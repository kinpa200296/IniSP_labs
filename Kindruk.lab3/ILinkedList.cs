using System;
using System.Collections.Generic;

namespace Kindruk.lab3
{
    interface ILinkedList<T> : IEnumerable<T>, IDisposable where T : class, IDisposable, IEquatable<T>
    {
        LinkedListNode<T> First { get; }
        LinkedListNode<T> Last { get; }
        int Count { get; }
        T this[int index] { get; set; }
        bool IsReadOnly { get; }

        void AddAfter(LinkedListNode<T> item, T data);
        void Add(T data);
        void AddFirst(T data);
        void Delete(LinkedListNode<T> item);
        int IndexOf(T item);
        void Clear();
        bool Contains(T item);
        void CopyTo(T[] array, int arrayIndex);
        LinkedListNode<T> GetNodeByData(T data);
        LinkedListNode<T> GetNodeByIndex(int index);
    }
}
