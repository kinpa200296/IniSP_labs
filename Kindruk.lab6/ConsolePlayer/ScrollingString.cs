using System;
using System.Text;

namespace ConsolePlayer
{
    public class ScrollingString
    {
        private readonly StringBuilder _processedLine;

        public string Data { get; private set; }
        public string Separator { get; private set; }
        public int DisplayLength { get; private set; }
        public int StartingDisplayPosition { get; set; }

        public ScrollingString()
        {
            Data = "<NoString>";
            DisplayLength = 10;
            Separator = "     ";
            StartingDisplayPosition = 0;
            _processedLine = new StringBuilder(Data);
            _processedLine.Append(Separator);
            _processedLine.Append(Data);

        }

        public ScrollingString(string data, int displayLength)
        {
            Data = data;
            DisplayLength = displayLength;
            Separator = "     ";
            StartingDisplayPosition = 0;
            _processedLine = new StringBuilder(Data);
            _processedLine.Append(Separator);
            _processedLine.Append(Data);
        }

        public ScrollingString(string data, int displayLength, string separator)
        {
            Data = data;
            DisplayLength = displayLength;
            Separator = separator;
            StartingDisplayPosition = 0;
            _processedLine = new StringBuilder(Data);
            _processedLine.Append(Separator);
            _processedLine.Append(Data);
        }

        public override string ToString()
        {
            return _processedLine.ToString(StartingDisplayPosition, Math.Min(DisplayLength, Data.Length));
        }

        public void Scroll()
        {
            StartingDisplayPosition = (StartingDisplayPosition < Data.Length + Separator.Length) &&
                                      (DisplayLength < Data.Length)
                ? StartingDisplayPosition + 1
                : 0;
        }
    }
}
