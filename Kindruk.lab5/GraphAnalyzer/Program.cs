using System;
using System.Configuration;
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
            var coreDllsFolder =
                new DirectoryInfo(path + "\\" +
                                  ConfigurationManager.ConnectionStrings["CoreDllsFolder"].ConnectionString);
            var pluginsFolder =
                new DirectoryInfo(path + "\\" + ConfigurationManager.ConnectionStrings["PluginsFolder"].ConnectionString);
            Console.WriteLine(coreDllsFolder.FullName);
            Console.WriteLine(pluginsFolder.FullName);
            //var weightedGraph = Assembly.LoadFile(coreDllsFolder.FullName + @"\WeightedGraph.dll");
            //var io = Assembly.LoadFile(coreDllsFolder.FullName + @"\GraphIO.dll");
            //var frameTree = Assembly.LoadFile(pluginsFolder.FullName + @"\FrameTreeBuilder.dll");
            //var shortestWays = Assembly.LoadFile(pluginsFolder.FullName + @"\ShortestWaysFinder.dll");
            Console.ReadKey();
        }
    }
}
