using System;
using Audio;

namespace ConsolePlayer
{
    public class PlayerSong
    {
        public Song Data { get; private set; }
        public TimeSpan TimePlayed { get; set; }
        public int Id { get; private set; }

        public ScrollingString Name;
        public ScrollingString Performer;
        public ScrollingString Genre;

        public PlayerSong()
        {
            Data = new Song();
            TimePlayed = new TimeSpan(0);
            Id = Player.GetNewSongId();
            ResetScrollingStrings();
        }

        public PlayerSong(Song data)
        {
            Data = data;
            TimePlayed = new TimeSpan(0);
            Id = Player.GetNewSongId();
            ResetScrollingStrings();
        }

        public void ResetScrollingStrings()
        {
            Name = new ScrollingString(Data.Name, PlayListsPageViewManager.NameDisplayStringLength,
                PlayListsPageViewManager.ScrollingStringSeparator);
            Performer = new ScrollingString(Data.Performer, PlayListsPageViewManager.PerformerDisplayStringLength,
                PlayListsPageViewManager.ScrollingStringSeparator);
            Genre = new ScrollingString(Data.Genre.ToString(), PlayListsPageViewManager.GenreDisplayStringLength,
                PlayListsPageViewManager.ScrollingStringSeparator);
        }
    }
}
