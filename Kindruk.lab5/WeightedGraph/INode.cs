using System;

namespace WeightedGraph
{
    public interface INode : IDisposable
    {
        int Mark { get; }
    }
}
