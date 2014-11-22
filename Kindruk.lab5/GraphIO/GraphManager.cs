using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using WeightedGraph;

namespace GraphIO
{
    public static class GraphManager
    {
        public static Graph<int> Load(StreamReader streamReader)
        {
            var strings = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).ConnectionStrings;
            try
            {
                var s = streamReader.ReadLine();
                var c = s.Split(strings.ConnectionStrings["Separators"].ConnectionString.ToCharArray());
                c = c.Where(str => !string.IsNullOrWhiteSpace(str)).ToArray();
                var n = int.Parse(c[0]);
                var m = int.Parse(c[1]);
                s = streamReader.ReadLine();
                if (s != strings.ConnectionStrings["OrientiedMark"].ConnectionString &&
                    s != strings.ConnectionStrings["NonOrientiedMark"].ConnectionString)
                    throw new Exception(strings.ConnectionStrings["MissingGraphType"].ConnectionString);
                var graph = new Graph<int>(n, s == strings.ConnectionStrings["OrientiedMark"].ConnectionString);
                for (var i = 0; i < m; i++)
                {
                    s = streamReader.ReadLine();
                    c = s.Split(strings.ConnectionStrings["Separators"].ConnectionString.ToCharArray());
                    c = c.Where(str => !string.IsNullOrWhiteSpace(str)).ToArray();
                    var x = int.Parse(c[0]) - 1;
                    var y = int.Parse(c[1]) - 1;
                    var w = int.Parse(c[2]);
                    var nodeX = new Node(x);
                    var nodeY = new Node(y);
                    var edge = new Edge<int>(nodeX, nodeY, w);
                    graph.Add(edge);
                }

                return graph;
            }
            catch (Exception e)
            {
                throw new FormatException(
                    strings.ConnectionStrings["FileFormatException"].ConnectionString + e.Message);
            }

        }

        public static void Write(StreamWriter streamWriter, Graph<int> graph)
        {
            var strings = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).ConnectionStrings;
            var edges = graph.GetAllEdges().ToArray();
            streamWriter.WriteLine(
                strings.ConnectionStrings["NodesEdgesCountOutputFormat"].ConnectionString, graph.NodeCount,
                edges.Length);
            streamWriter.WriteLine(graph.IsOrientied
                ? strings.ConnectionStrings["OrientiedMark"].ConnectionString
                : strings.ConnectionStrings["NonOrientiedMark"].ConnectionString);
            foreach (var edge in edges)
            {
                if (!graph.IsOrientied)
                    if (edge.From.Mark > edge.To.Mark)
                        continue;
                streamWriter.WriteLine(strings.ConnectionStrings["EdgeOutputFormat"].ConnectionString,
                    edge.From.Mark + 1, edge.To.Mark + 1, edge.Weight);
            }
        }
    }
}
