using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Audio;

namespace ConsolePlayer
{
    public class PlayerPlayList
    {
        public readonly List<PlayerSong> PlayerSongs = new List<PlayerSong>();
        public PlayList Data { get; private set; }
        public int Id { get; private set; }
        public int CurrentSongIndex { get; private set; }
        public bool Play { get; private set; }
        public bool StayAlive { get; set; }
        public PlayerSong CurrentSong { get { return PlayerSongs[CurrentSongIndex]; } }

        public PlayerPlayList()
        {
            PlayerSongs.Clear();
            Data = new PlayList();
            Id = Player.GetNewPlayListId();
            CurrentSongIndex = -1;
            Play = false;
        }

        public PlayerPlayList(PlayList data)
        {
            PlayerSongs.Clear();
            Data = data;
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

        public void Stop()
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
                CurrentSongIndex = CurrentSongIndex == PlayerSongs.Count
                        ? 0
                        : CurrentSongIndex + 1;
            }
        }

        public void PreviousSong()
        {
            lock (this)
            {
                CurrentSongIndex = CurrentSongIndex == 0
                        ? PlayerSongs.Count
                        : CurrentSongIndex - 1;
            }
        }
    }
}
