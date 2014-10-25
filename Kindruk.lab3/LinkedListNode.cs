using System;

namespace Kindruk.lab3
{
    public class LinkedListNode<T> : ILinkedListNode<T> where T : IDisposable
    {
        private T _data;
        private bool _disposed;

        #region constructors

        public LinkedListNode()
        {
            _data = default(T);
        }

        public LinkedListNode(T data)
        {
            Data = data;
        }
        #endregion

        #region properties
        public T Data
        {
            get { return _data; }
            set
            {
                if (ReferenceEquals(value, null))
                    throw new ArgumentNullException();
                _data = value;
            }
        }

        public ILinkedListNode<T> Previous { get; set; }

        public ILinkedListNode<T> Next { get; set; }

        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                _data.Dispose();
            }
            _disposed = true;
        }

        ~LinkedListNode()
        {
            Dispose(false);
        }
    }
}
