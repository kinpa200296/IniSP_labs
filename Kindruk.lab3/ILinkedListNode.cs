using System;

namespace Kindruk.lab3
{
    public interface ILinkedListNode<T> : IDisposable where T : IDisposable
    {
        ILinkedListNode<T> Previous { get; set; }
        ILinkedListNode<T> Next { get; set; }
        T Data { get; set; }
    }
}
