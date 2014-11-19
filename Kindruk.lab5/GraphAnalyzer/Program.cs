using System;
using System.IO;
using System.Reflection;
using Plugin;

namespace GraphAnalyzer
{
    class Program
    {
        static void Main()
        {
            var path = Directory.GetCurrentDirectory();
            Console.WriteLine(path);
            var weightedGraph = Assembly.LoadFile(path + @"\CoreDlls\WeightedGraph.dll");
            var io = Assembly.LoadFile(path + @"\CoreDlls\GraphIO.dll");
            var frameTree = Assembly.LoadFile(path + @"\Plugins\FrameTreeBuilder.dll");
            var shortestWays = Assembly.LoadFile(path + @"\Plugins\ShortestWaysFinder.dll");
        }
    }
}
