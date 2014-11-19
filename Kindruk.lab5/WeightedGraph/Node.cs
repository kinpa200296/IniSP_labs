using System;

namespace WeightedGraph
{
    public class Node : INode
    {
        private bool _disposed;

        public int Mark { get; private set; }

        public Node(int mark)
        {
            Mark = mark;
        }

        ~Node()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing){}
            _disposed = true;
        }
    }
}
