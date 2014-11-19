using System;

namespace WeightedGraph
{
    public interface IEdge : IDisposable
    {
        INode From { get; }
        INode To { get; }
        int Weight { get; }
    }
}
