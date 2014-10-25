using System;
using System.Collections.Generic;

namespace Kindruk.lab3
{
    interface ILinkedList<T> : IEnumerable<T>, IDisposable where T : class, IDisposable, IEquatable<T>
    {
        ILinkedListNode<T> First { get; }
        ILinkedListNode<T> Last { get; }
        int Count { get; }
        T this[int index] { get; set; }
        bool IsReadOnly { get; }

        void AddAfter(ILinkedListNode<T> item, T data);
        void Add(T data);
        void AddFirst(T data);
        void Delete(ILinkedListNode<T> item);
        int IndexOf(T item);
        void Clear();
        bool Contains(T item);
        void CopyTo(T[] array, int arrayIndex);
        ILinkedListNode<T> GetNodeByData(T data);
        ILinkedListNode<T> GetNodeByIndex(int index);
    }
}
