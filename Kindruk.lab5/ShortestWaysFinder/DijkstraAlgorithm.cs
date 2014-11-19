using System;
using System.IO;
using System.Reflection;
using GraphIO;
using Plugin;
using WeightedGraph;

[assembly: AssemblyVersion("1.0.0.0"), AssemblyTitle("ShortestWaysFinder")]
[assembly: AssemblyDescription("Provides methods to find the length of the shortest ways from specified node to other nodes.")]
namespace ShortestWaysFinder
{
    [PluginClass("ShorestWaysFinder", "DijkstraAlgorithm", "kinpa200296",
        "Contains methods to find the length of the shortest ways from specified node to other nodes")]
    public class DijkstraAlgorithm : IPlugin
    {
        private bool _disposed;

        public int[] DistanceToAllNodesFrom(Graph graph, INode node)
        {
            var distance = new int[graph.NodeCount];
            var used = new bool[graph.NodeCount];
            for (var i = 0; i < graph.NodeCount; i++)
            {
                distance[i] = int.MaxValue / 3;
                used[i] = false;
            }
            var currentNode = node;
            distance[currentNode.Mark] = 0;
            for (var j = 0; j < graph.NodeCount; j++)
            {
                var minDistance = int.MaxValue;
                var nextNode = currentNode;
                for (var i = 0; i < graph.NodeCount; i++)
                {
                    if (used[i] || distance[i] >= minDistance) continue;
                    minDistance = distance[i];
                    nextNode = new Node(i);
                }
                currentNode = nextNode;
                used[currentNode.Mark] = true;
                foreach (var edge in graph.GetNodeEdges(currentNode))
                {
                    distance[edge.To.Mark] = Math.Min(distance[edge.To.Mark], distance[edge.From.Mark] + edge.Weight);
                }
            }
            return distance;
        }

        ~DijkstraAlgorithm()
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

        public void Init()
        {
        }

        public void DoSomeAction()
        {
            Console.WriteLine("Filename Please.");
            var filename = Console.ReadLine();
            Graph graph;
            using (var file = new FileStream(filename, FileMode.Open))
            {
                graph = GraphManager.Load(new StreamReader(file));
            }
            Console.WriteLine("Node Please.");
            var s = Console.ReadLine();
            var x = int.Parse(s);
            var distance = DistanceToAllNodesFrom(graph, new Node(x));
            Console.WriteLine("Shortest ways from node {0}:", x);
            for (var i = 0; i < graph.NodeCount; i++)
            {
                Console.WriteLine("{0} → {1} - {2}", x, i, distance[i]);
            }
        }
    }
}
