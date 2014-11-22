using System;
using System.Collections.Generic;

namespace WeightedGraph
{
    public interface IGraph<T> : IDisposable
    {
        int NodeCount { get; }
        bool IsOrientied { get; }
        void Add(IEdge<T> edge);
        void Remove(IEdge<T> edge);
        void RemoveAllEdges();
        IEnumerable<IEdge<T>> GetNodeEdges(INode node);
        IEnumerable<IEdge<T>> GetAllEdges();
    }
}
