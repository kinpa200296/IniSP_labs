using System;
using System.Configuration;

namespace ConsolePlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Player.Launch(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(ConfigurationManager.ConnectionStrings["UnhandledException"].ConnectionString,
                        e.TargetSite.DeclaringType + "." + e.TargetSite.Name, e.Message);
                Console.WriteLine(ConfigurationManager.ConnectionStrings["FatalError"]);
                Console.ReadKey();
            }
        }
    }
}
