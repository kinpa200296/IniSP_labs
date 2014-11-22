using System;
using System.Collections.Generic;
using System.Linq;
using Plugin;

[assembly: CoreDll("WeightedGraph", "kinpa200296", "Provides simple weighted graph.")]
namespace WeightedGraph
{
    public class Graph<T> : IGraph<T>
    {
        private List<IEdge<T>>[] _nodeEdges;
        private bool _disposed;

        public int NodeCount { get; private set; }
        public bool IsOrientied { get; private set; }

        public Graph(int nodeCount, bool isOrientied = false)
        {
            NodeCount = nodeCount;
            _nodeEdges = new List<IEdge<T>>[NodeCount];
            for(var i = 0; i < NodeCount; i++)
            {
                _nodeEdges[i] = new List<IEdge<T>>();
            }
            IsOrientied = isOrientied;
        }

        public void Add(IEdge<T> edge)
        {
            _nodeEdges[edge.From.Mark].Add(edge);
            var reverseEdge = new Edge<T>(edge.To, edge.From, edge.Weight);
            if (!IsOrientied)
                _nodeEdges[reverseEdge.From.Mark].Add(reverseEdge);
        }

        public void Remove(IEdge<T> edge)
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

        public IEnumerable<IEdge<T>> GetNodeEdges(INode node)
        {
            return _nodeEdges[node.Mark];
        }

        public IEnumerable<IEdge<T>> GetAllEdges()
        {
            var size = _nodeEdges.Sum(list => list.Count);
            var allEdges = new IEdge<T>[size];
            var i = 0;
            foreach (var edge in _nodeEdges.SelectMany(list => list))
            {
                allEdges[i] = edge;
                i++;
            }
            return allEdges.Select(x => x);
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
