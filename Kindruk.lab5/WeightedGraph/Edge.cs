﻿using System;

namespace WeightedGraph
{
    public class Edge<T> : IEdge<T>
    {
        private bool _disposed;

        public INode From { get; private set; }
        public INode To { get; private set; }
        public T Weight { get; private set; }

        public Edge(INode from, INode to, T weight)
        {
            From = from;
            To = to;
            Weight = weight;
        }

        ~Edge()
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
            if (disposing)
            {
                From.Dispose();
                To.Dispose();
            }
            _disposed = true;
        }
    }
}
