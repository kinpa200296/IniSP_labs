using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace ConsolePlayer
{
    public static class PlayListsPageViewManager
    {
        public static readonly string ScrollingStringSeparator =
            ConfigurationManager.ConnectionStrings["ScrollingStringSeparator"].ConnectionString,
            PlayListsSeparator = ConfigurationManager.ConnectionStrings["PlayListsSeparator"].ConnectionString,
            PlayListStateStringPlay = ConfigurationManager.ConnectionStrings["PlayListStateStringPlay"].ConnectionString,
            PlayListStateStringStop = ConfigurationManager.ConnectionStrings["PlayListStateStringStop"].ConnectionString;

        public static int ScrollingStringUpdateFrequency,
            NameDisplayStringLength,
            PlayListNameDisplayStringLength,
            PerformerDisplayStringLength,
            GenreDisplayStringLength;

        public static int PageSize { get; private set; }

        public static int FirstPlayListOnPageIndex
        {
            get { return CurrentPage*PageSize; }
        }

        public static int FirstPlayListOnNextPageIndex
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
                "PageSize", "ScrollingStringUpdateFrequency", "NameDisplayStringLength",
                "PlayListNameDisplayStringLength", "PerformerDisplayStringLength",
                "GenreDisplayStringLength"
            };
            var values = keys.Select(x => int.Parse(ConfigurationManager.AppSettings[x])).ToArray();
            PageSize = values[0];
            ScrollingStringUpdateFrequency = values[1];
            NameDisplayStringLength = values[2];
            PlayListNameDisplayStringLength = values[3];
            PerformerDisplayStringLength = values[4];
            GenreDisplayStringLength = values[5];
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
            for (var i = FirstPlayListOnPageIndex; i < FirstPlayListOnNextPageIndex; i++)
            {
                strings.Add(
                    string.Format(
                        ConfigurationManager.ConnectionStrings["PlayListDisplayFormatString1"].ConnectionString,
                        Player.PlayLists[i].Name, Player.PlayLists[i].Data.Rating.ToString(CultureInfo.InvariantCulture),
                        Player.PlayLists[i].Id,
                        Player.PlayLists[i].Play ? PlayListStateStringPlay : PlayListStateStringStop));
                strings.Add(
                    string.Format(
                        ConfigurationManager.ConnectionStrings["PlayListDisplayFormatString2"].ConnectionString,
                    Player.PlayLists[i].CurrentSong.Name, Player.PlayLists[i].CurrentSong.Performer,
                    Player.PlayLists[i].CurrentSong.TimePlayed, Player.PlayLists[i].CurrentSong.Data.Length));
                strings.Add(
                    string.Format(
                        ConfigurationManager.ConnectionStrings["PlayListDisplayFormatString3"].ConnectionString,
                        Player.PlayLists[i].CurrentSong.Genre, Player.PlayLists[i].CurrentSong.Data.Rating,
                        Player.PlayLists[i].CurrentSong.Id));
                if (i != FirstPlayListOnNextPageIndex - 1)
                {
                    strings.Add(PlayListsSeparator);
                }
            }
            return strings.ToArray();
        }
    }
}
