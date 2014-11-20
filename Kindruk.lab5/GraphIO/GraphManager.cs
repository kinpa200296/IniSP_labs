using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using WeightedGraph;
using Plugin;

[assembly: AssemblyTitle("GraphIO"), AssemblyVersion("1.0.0.1")]
[assembly: AssemblyDescription("Provides methods for saving and loading weighted graphs.")]
[assembly: CoreDll("GraphIO", "kinpa200296", "Provides methods for saving and loading weighted graphs.")]
namespace GraphIO
{
    public static class GraphManager
    {
        public static Graph Load(StreamReader streamReader)
        {
            try
            {
                var s = streamReader.ReadLine();
                var c = s.Split(ConfigurationManager.ConnectionStrings["Separators"].ConnectionString.ToCharArray());
                c = c.Where(str => !string.IsNullOrWhiteSpace(str)).ToArray();
                var n = int.Parse(c[0]);
                var m = int.Parse(c[1]);
                s = streamReader.ReadLine();
                if (s != ConfigurationManager.ConnectionStrings["OrientiedMark"].ConnectionString &&
                    s != ConfigurationManager.ConnectionStrings["NonOrientiedMark"].ConnectionString)
                    throw new Exception(ConfigurationManager.ConnectionStrings["MissingGraphType"].ConnectionString);
                var graph = new Graph(n, s == ConfigurationManager.ConnectionStrings["OrientiedMark"].ConnectionString);
                for (var i = 0; i < m; i++)
                {
                    s = streamReader.ReadLine();
                    c = s.Split(ConfigurationManager.ConnectionStrings["Separators"].ConnectionString.ToCharArray());
                    c = c.Where(str => !string.IsNullOrWhiteSpace(str)).ToArray();
                    var x = int.Parse(c[0]) - 1;
                    var y = int.Parse(c[1]) - 1;
                    var w = int.Parse(c[2]);
                    var nodeX = new Node(x);
                    var nodeY = new Node(y);
                    var edge = new Edge(nodeX, nodeY, w);
                    graph.Add(edge);
                }

                return graph;
            }
            catch (Exception e)
            {
                throw new FormatException(
                    ConfigurationManager.ConnectionStrings["FileFormatException"].ConnectionString + e.Message);
            }

        }

        public static void Write(StreamWriter streamWriter, Graph graph)
        {
            var edges = graph.GetAllEdges();
            streamWriter.WriteLine(
                ConfigurationManager.ConnectionStrings["NodesEdgesCountOutputFormat"].ConnectionString, graph.NodeCount,
                edges.Length);
            streamWriter.WriteLine(graph.IsOrientied
                ? ConfigurationManager.ConnectionStrings["OrientiedMark"].ConnectionString
                : ConfigurationManager.ConnectionStrings["NonOrientiedMark"].ConnectionString);
            foreach (var edge in edges)
            {
                if (!graph.IsOrientied)
                    if (edge.From.Mark > edge.To.Mark)
                        continue;
                streamWriter.WriteLine(ConfigurationManager.ConnectionStrings["EdgeOutputFormat"].ConnectionString,
                    edge.From.Mark + 1, edge.To.Mark + 1, edge.Weight);
            }
        }
    }
}
