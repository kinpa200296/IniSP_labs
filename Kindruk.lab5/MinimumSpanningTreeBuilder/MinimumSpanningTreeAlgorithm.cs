using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using GraphIO;
using Plugin;
using WeightedGraph;

namespace MinimumSpanningTreeBuilder
{
    [PluginClass("MinimumSpanningTreeBuilder", "MinimumSpanningTreeAlgorithm", "kinpa200296",
        "Contains methods to build Minimum Spanning Tree in unorientied weighted graph")]
    class MinimumSpanningTreeAlgorithm : IPlugin
    {
        private bool _disposed;

        public IEdge<int>[] BuildMinimumSpanningTree(Graph<int> graph)
        {
            var edges = new List<IEdge<int>>();
            var sets = new int[graph.NodeCount];
            for (var i = 0; i < graph.NodeCount; i++)
            {
                sets[i] = i;
            }
            var allEdges = graph.GetAllEdges();
            allEdges = allEdges.OrderBy(edge => edge.Weight).ToArray();
            foreach (var edge in allEdges.Where(edge => sets[edge.From.Mark] != sets[edge.To.Mark]))
            {
                edges.Add(edge);
                var oldSet = sets[edge.To.Mark];
                for (var i = 0; i < graph.NodeCount; i++)
                    sets[i] = sets[i] == oldSet ? sets[edge.From.Mark] : sets[i];
            }
            return edges.ToArray();
        }

        ~MinimumSpanningTreeAlgorithm()
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

        [PluginMethod("MinimumSpanningTreeAlgorithm", "kinpa200296", "Loads graph from requested file and print into Console it's Minimum Spanning Tree.")]
        public void DoSomeAction()
        {
            var strings = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).ConnectionStrings;
            Console.WriteLine(strings.ConnectionStrings["FileNameRequest"].ConnectionString);
            var filename = Console.ReadLine();
            while (!File.Exists(filename))
            {
                Console.WriteLine(strings.ConnectionStrings["FileNotFound"].ConnectionString);
                Console.WriteLine(strings.ConnectionStrings["RepeatInput"].ConnectionString);
                filename = Console.ReadLine();
            }
            Graph<int> graph;
            using (var file = new FileStream(filename, FileMode.Open))
            {
                graph = GraphManager.Load(new StreamReader(file));
            }
            if (graph.IsOrientied)
                throw new FormatException(
                    strings.ConnectionStrings["UnOrientiedGraphExpected"].ConnectionString);
            var edges = BuildMinimumSpanningTree(graph);
            Console.WriteLine(strings.ConnectionStrings["FrameTreeWeight"].ConnectionString + "{0}",
                edges.Sum(edge => edge.Weight));
            Console.WriteLine(strings.ConnectionStrings["UsedEdges"].ConnectionString);
            foreach (var edge in edges)
            {
                Console.WriteLine(strings.ConnectionStrings["EdgeOutputFormat"].ConnectionString,
                    edge.From.Mark, edge.To.Mark, edge.Weight);
            }
        }
    }
}
