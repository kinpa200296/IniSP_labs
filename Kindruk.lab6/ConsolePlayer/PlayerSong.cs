using System;
using Audio;

namespace ConsolePlayer
{
    public class PlayerSong
    {
        public Song Data { get; private set; }
        public TimeSpan TimePlayed { get; set; }
        public int Id { get; private set; }

        public PlayerSong()
        {
            Data = new Song();
            TimePlayed = new TimeSpan(0);
            Id = Player.GetNewSongId();
        }

        public PlayerSong(Song data)
        {
            Data = data;
            TimePlayed = new TimeSpan(0);
            Id = Player.GetNewSongId();
        }
    }
}
