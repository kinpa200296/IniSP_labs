using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ConsolePlayer
{
    public static class PlayListsPageViewManager
    {
        public static int PageSize { get; private set; }

        public static int FirstPlayListOnPageIndex
        {
            get { return CurrentPage*PageSize; }
        }

        public static int LastPlayListOnPageIndex
        {
            get { return Math.Min((CurrentPage + 1)*PageSize, Player.PlayLists.Count); }
        }

        public static int CurrentPage { get; private set; }

        public static int PageCount
        {
            get { return Player.PlayLists.Count/PageSize + (Player.PlayLists.Count%PageSize == 0 ? 0 : 1) - 1; }
        }

        public static void Init()
        {
            var keys = new[]
            {
                "PageSize"
            };
            var values = keys.Select(x => int.Parse(ConfigurationManager.AppSettings[x])).ToArray();
            PageSize = values[0];
        }

        public static void NextPage()
        {
            CurrentPage = CurrentPage == PageCount ? PageCount : CurrentPage + 1;
        }

        public static void PreviousPage()
        {
            CurrentPage = CurrentPage == 0 ? 0 : CurrentPage - 1;
        }

        public static string[] GetPageSnapShot()
        {
            var strings = new List<string>();
            if (Player.PlayLists.Count == 0)
                return strings.ToArray();
            for (var i = FirstPlayListOnPageIndex; i < LastPlayListOnPageIndex; i++)
            {
                strings.Add(string.Format("{0} [{1}]◙♪{2} - {3}◙♪{4:hh\\:mm\\:ss}/{5:hh\\:mm\\:ss}", Player.PlayLists[i].Data.Name, Player.PlayLists[i].Play ? "Playing" : "Stopped", Player.PlayLists[i].CurrentSong.Data.Performer, Player.PlayLists[i].CurrentSong.Data.Name,
                    Player.PlayLists[i].CurrentSong.TimePlayed,
                    Player.PlayLists[i].CurrentSong.Data.Length));
            }
            return strings.ToArray();
        }
    }
}
