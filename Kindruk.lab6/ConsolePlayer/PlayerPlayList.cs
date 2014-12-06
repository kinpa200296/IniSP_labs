using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Audio;

namespace ConsolePlayer
{
    public class PlayerPlayList
    {
        public List<PlayerSong> PlayerSongs = new List<PlayerSong>();
        public PlayList Data { get; private set; }
        public int Id { get; private set; }
        public int CurrentSongIndex { get; private set; }
        public bool Play { get; private set; }
        public bool StayAlive { get; set; }
        public PlayerSong CurrentSong { get { return PlayerSongs[CurrentSongIndex]; } }
        public ScrollingString Name;

        public PlayerPlayList()
        {
            PlayerSongs.Clear();
            Data = new PlayList();
            Name = new ScrollingString(Data.Name, PlayListsPageViewManager.PlayListNameDisplayStringLength,
                ConsoleDisplayManager.ScrollingStringSeparator);
            Id = Player.GetNewPlayListId();
            CurrentSongIndex = -1;
            Play = false;
        }

        public PlayerPlayList(PlayList data)
        {
            PlayerSongs.Clear();
            Data = data;
            Name = new ScrollingString(Data.Name, PlayListsPageViewManager.PlayListNameDisplayStringLength,
                ConsoleDisplayManager.ScrollingStringSeparator);
            PlayerSongs.AddRange(Data.Songs.Select(x => new PlayerSong(x)));
            Id = Player.GetNewPlayListId();
            CurrentSongIndex = -1;
            Play = false;
        }

        public static PlayerPlayList Load(string filename)
        {
            return new PlayerPlayList((PlayList)PlayListManager.Load(filename));
        }

        public void Launch()
        {
            CurrentSongIndex = 0;
            Play = true;
            StayAlive = true;
            Run();
        }

        public void Run()
        {
            while (StayAlive)
            {
                Thread.Sleep(Player.RefreshRate);
                if (!Play) continue;
                if (PlayerSongs[CurrentSongIndex].TimePlayed >= PlayerSongs[CurrentSongIndex].Data.Length)
                {
                    PlayerSongs[CurrentSongIndex].TimePlayed = new TimeSpan(0);
                    CurrentSongIndex = CurrentSongIndex == PlayerSongs.Count
                        ? 0
                        : CurrentSongIndex + 1;

                }
                else
                {
                    PlayerSongs[CurrentSongIndex].TimePlayed += Player.RefreshRate;
                }
            }
        }

        public void Pause()
        {
            Play = false;
        }

        public void Resume()
        {
            Play = true;
        }

        public void NextSong()
        {
            lock (this)
            {
                CurrentSongIndex = CurrentSongIndex == PlayerSongs.Count - 1
                        ? 0
                        : CurrentSongIndex + 1;
            }
        }

        public void PreviousSong()
        {
            lock (this)
            {
                CurrentSongIndex = CurrentSongIndex == 0
                        ? PlayerSongs.Count - 1
                        : CurrentSongIndex - 1;
            }
        }

        public void RestartCurrentSong()
        {
            lock (CurrentSong)
            {
                CurrentSong.TimePlayed = new TimeSpan(0);
            }
        }

        public void SwapSongsRandomly()
        {
            var random = new Random();
            var songs = PlayerSongs.OrderBy(x => random.Next());
            foreach (var song in songs)
            {
                song.ResetScrollingStrings();
                song.TimePlayed = new TimeSpan(0);
            }
            var playerSongs = new List<PlayerSong>(songs);
            PlayerSongs = playerSongs;
            CurrentSongIndex = 0;
        }

        public void SortSongsByRating()
        {
                var songs = PlayerSongs.OrderByDescending(x => x.Data.Rating);
                foreach (var song in songs)
                {
                    song.ResetScrollingStrings();
                    song.TimePlayed = new TimeSpan(0);
                }
            var playerSongs = new List<PlayerSong>(songs);
            PlayerSongs = playerSongs;
            CurrentSongIndex = 0;
        }
    }
}
