using System;
using System.Xml;
using System.Xml.Linq;

namespace Audio
{
    public class Song : IStorable
    {
        private int _rating;

        public string Name { get; set; }
        public TimeSpan Length { get; set; }
        public string Performer { get; set; }
        public Genres Genre { get; set; }

        public int Rating
        {
            get { return _rating; }
            set { _rating = value > 0 ? value < 10 ? value : 10 : 0; }
        }

        public Song()
        {
            Name = "<unknown>";
            Length = new TimeSpan(0);
            Performer = "unknown";
            Genre = Genres.Unknown;
            Rating = 0;
        }

        public XElement ToXElement()
        {
            return new XElement("Song", new XElement("Name", Name), new XElement("Length", Length.Ticks),
                new XElement("Performer", Performer), new XElement("Genre", (int)Genre), new XElement("Rating", Rating));
        }

        public void ReadFromXElement(XElement element)
        {
            if (element.Name != "Song") 
                throw new XmlException();
            Name = element.Element("Name").Value;
            Length = new TimeSpan(long.Parse(element.Element("Length").Value));
            Performer = element.Element("Performer").Value;
            Genre = (Genres)int.Parse(element.Element("Genre").Value);
            Rating = int.Parse(element.Element("Rating").Value);
        }
    }
}
