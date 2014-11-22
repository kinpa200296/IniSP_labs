using System;

namespace WeightedGraph
{
    public interface IEdge<out T> : IDisposable
    {
        INode From { get; }
        INode To { get; }
        T Weight { get; }
    }
}
