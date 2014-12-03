using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;

namespace ConsolePlayer
{
    public static class Player
    {
        private static int _freeSongId, _freePlayListId;

        public static readonly List<PlayerPlayList> PlayLists = new List<PlayerPlayList>();

        public static TimeSpan RefreshRate { get; private set; }

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
            PlayLists.Clear();
            var keys = new[]
            {
                "RefreshRate"
            };
            var values = keys.Select(x => int.Parse(ConfigurationManager.AppSettings[x])).ToArray();
            RefreshRate = new TimeSpan(0, 0, 0, 0, values[0]);
        }

        public static int GetNewSongId()
        {
            return _freeSongId++;
        }

        public static int GetNewPlayListId()
        {
            return _freePlayListId++;
        }

        public static void LoadPlayList(string filename)
        {
            var playList = PlayerPlayList.Load(filename);
            var t = new Thread(playList.Launch)
            {
                IsBackground = true,
                Priority = ThreadPriority.AboveNormal
            };
            t.Start();
            PlayLists.Add(playList);
        }
    }
}
