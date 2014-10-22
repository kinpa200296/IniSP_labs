using System;

namespace Kindruk.lab3
{
    public class LinkedListNode<T> : IDisposable where T : IDisposable
    {
        public const string NullData = "Получена пустая ссылка на данные для элемента списка.";
        private T _data;
        private LinkedListNode<T> _previous, _next;
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
                    throw new ArgumentNullException("Da" + "ta", NullData);
                _data = value;
            }
        }

        public LinkedListNode<T> Previous
        {
            get { return _previous; }
            set { _previous = value; }
        }

        public LinkedListNode<T> Next
        {
            get { return _next; }
            set { _next = value; } 
        }
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
