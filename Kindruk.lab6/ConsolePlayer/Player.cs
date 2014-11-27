using System;
using System.Configuration;

namespace ConsolePlayer
{
    public static class Player
    {
        public static void Launch(string[] playlists)
        {
            try
            {
                Init();
            }
            catch (Exception e)
            {
                Console.WriteLine(ConfigurationManager.ConnectionStrings["UnhandledException"].ConnectionString,
                    e.TargetSite.DeclaringType + "." + e.TargetSite.Name, e.Message);
                throw new Exception("", e);
            }
            Run();
        }

        public static void Run()
        {
            ConsoleDisplayManager.Launch();
        }

        public static void Init()
        {
            
        }
    }
}
