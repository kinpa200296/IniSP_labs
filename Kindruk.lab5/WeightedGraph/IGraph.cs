using System;
using System.Collections.Generic;

namespace WeightedGraph
{
    public interface IGraph : IDisposable
    {
        int NodeCount { get; }
        bool IsOrientied { get; }
        void Add(IEdge edge);
        void Remove(IEdge edge);
        void RemoveAllEdges();
        IEnumerable<IEdge> GetNodeEdges(INode node);
        IEdge[] GetAllEdges();
    }
}
