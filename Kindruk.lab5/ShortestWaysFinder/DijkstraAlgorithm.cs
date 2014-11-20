using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using GraphIO;
using Plugin;
using WeightedGraph;

[assembly: AssemblyVersion("1.0.0.1"), AssemblyTitle("ShortestWaysFinder")]
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
            Console.WriteLine(ConfigurationManager.ConnectionStrings["FileNameRequest"].ConnectionString);
            var filename = Console.ReadLine();
            while (!File.Exists(filename))
            {
                Console.WriteLine(ConfigurationManager.ConnectionStrings["FileNotFound"].ConnectionString);
                Console.WriteLine(ConfigurationManager.ConnectionStrings["RepeatInput"].ConnectionString);
                filename = Console.ReadLine();
            }
            Graph graph;
            using (var file = new FileStream(filename, FileMode.Open))
            {
                graph = GraphManager.Load(new StreamReader(file));
            }
            Console.WriteLine(ConfigurationManager.ConnectionStrings["NodeMarkRequest"].ConnectionString);
            var s = Console.ReadLine();
            int x;
            while (!int.TryParse(s, out x) || x < 1 || x > graph.NodeCount)
            {
                Console.WriteLine(ConfigurationManager.ConnectionStrings["NodeMarkNotFound"].ConnectionString);
                Console.WriteLine(ConfigurationManager.ConnectionStrings["RepeatInput"].ConnectionString);
                s = Console.ReadLine();
            }
            var distance = DistanceToAllNodesFrom(graph, new Node(x - 1));
            Console.WriteLine(ConfigurationManager.ConnectionStrings["ShortestWaysFromNode"].ConnectionString + " {0}:", x);
            for (var i = 0; i < graph.NodeCount; i++)
            {
                Console.WriteLine(ConfigurationManager.ConnectionStrings["ShortestWayOutputFormat"].ConnectionString, x, i,
                    distance[i]);
            }
        }
    }
}
