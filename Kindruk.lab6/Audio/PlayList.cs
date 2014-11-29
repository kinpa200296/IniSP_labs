using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Audio
{
    public class PlayList : IStorable
    {
        public readonly List<Song> Songs = new List<Song>();  
        public string Name { get; set; }
        public TimeSpan TotalLength { get; private set; }
        public double Rating { get; private set; }

        public PlayList()
        {
            Songs.Clear();
            Name = "<unknown>";
            TotalLength = new TimeSpan(0);
            Rating = 0;
        }

        public void CalculateRating()
        {
            Rating = Songs.Sum(x => x.Rating) / ((double)Songs.Count);
        }

        public void CalculateTotalLength()
        {
            TotalLength = new TimeSpan(Songs.Sum(x => x.Length.Ticks));
        }

        public XElement ToXElement()
        {
            return new XElement("PlayList", new XElement("Name", Name),
                new XElement("Songs", from song in Songs select song.ToXElement()));
        }

        public void ReadFromXElement(XElement element)
        {
            if (element.Name != "PlayList")
                throw new XmlException();
            Name = element.Element("Name").Value;
            var xSongs = element.Element("Songs").Elements();
            foreach (var xSong in xSongs)
            {
                var song = new Song();
                song.ReadFromXElement(xSong);
                Songs.Add(song);
            }
            CalculateTotalLength();
            CalculateRating();
        }
    }
}
