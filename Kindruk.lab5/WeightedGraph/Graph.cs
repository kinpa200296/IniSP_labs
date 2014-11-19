using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plugin;

[assembly : AssemblyTitle("WeightedGraph"), AssemblyVersion("1.0.0.0")]
[assembly : AssemblyDescription("Provides simple weighted graph.")]
[assembly: CoreDll("WeightedGraph", "kinpa200296", "Provides simple weighted graph.")]
namespace WeightedGraph
{
    public class Graph : IGraph
    {
        private List<IEdge>[] _nodeEdges;
        private bool _disposed;

        public int NodeCount { get; private set; }
        public bool IsOrientied { get; private set; }

        public Graph(int nodeCount, bool isOrientied = false)
        {
            NodeCount = nodeCount;
            _nodeEdges = new List<IEdge>[NodeCount];
            for(var i = 0; i < NodeCount; i++)
            {
                _nodeEdges[i] = new List<IEdge>();
            }
            IsOrientied = isOrientied;
        }

        public void Add(IEdge edge)
        {
            _nodeEdges[edge.From.Mark].Add(edge);
            var reverseEdge = new Edge(edge.To, edge.From, edge.Weight);
            if (!IsOrientied)
                _nodeEdges[reverseEdge.From.Mark].Add(reverseEdge);
        }

        public void Remove(IEdge edge)
        {
            _nodeEdges[edge.From.Mark].Remove(edge);
            if (!IsOrientied)
                _nodeEdges[edge.To.Mark].Remove(edge);
        }

        public void RemoveAllEdges()
        {
            foreach (var list in _nodeEdges)
            {
                list.Clear();
            }
        }

        public IEnumerable<IEdge> GetNodeEdges(INode node)
        {
            return _nodeEdges[node.Mark];
        }

        public IEdge[] GetAllEdges()
        {
            var size = _nodeEdges.Sum(list => list.Count);
            var allEdges = new IEdge[size];
            var i = 0;
            foreach (var edge in _nodeEdges.SelectMany(list => list))
            {
                allEdges[i] = edge;
                i++;
            }
            return allEdges;
        }

        ~Graph()
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
                foreach (var edge in _nodeEdges.SelectMany(list => list))
                {
                    edge.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
