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

        public static LogManager Log;

        public static TimeSpan RefreshRate { get; private set; }

        public static void Launch(string[] playlists)
        {
            using (Log = new LogManager())
            {
                try
                {
                    Init();
                }
                catch (Exception e)
                {
                    Log.Add(string.Format(ConfigurationManager.ConnectionStrings["UnhandledException"].ConnectionString,
                        e.TargetSite.DeclaringType + "." + e.TargetSite.Name, e.Message));
                    throw new Exception(string.Format("{0} ◙♪ {1}", e.TargetSite.DeclaringType + "." + e.TargetSite.Name,
                        e.Message));
                }
                foreach (var playlist in playlists)
                {
                    CommandsManager.LoadPlayList("", new[] { Commands.Load.ToString(), playlist});
                }
                Run();
            }
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
            ConsoleDisplayManager.Init();
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
