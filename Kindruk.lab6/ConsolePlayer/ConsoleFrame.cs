using System;
using System.Text;

namespace ConsolePlayer
{
    public class ConsoleFrame
    {
        private StringBuilder _data;

        public ScrollingString Title { get; private set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public ConsoleFrame()
        {
            Title = new ScrollingString("<NoTitle>", ConsoleDisplayManager.FrameTitleDisplayLength,
                PlayListsPageViewManager.ScrollingStringSeparator);
            PageCount = 0;
            CurrentPage = 0;
            Height = 24;
            Width = 80;
        }

        public ConsoleFrame(string title, int width, int height)
        {
            Title = new ScrollingString(title, ConsoleDisplayManager.FrameTitleDisplayLength,
                PlayListsPageViewManager.ScrollingStringSeparator);
            Width = width;
            Height = height;
            PageCount = 0;
            CurrentPage = 0;
        }

        public void BuildHeader()
        {
            var stringBuilder = new StringBuilder("╔═", Width);
            stringBuilder.Append(Title);
            if (PageCount <= 1)
            {
                stringBuilder.Insert(stringBuilder.Length, "═", Width - stringBuilder.Length - 1);
                stringBuilder.Append('╗');
            }
            else
            {
                var str = string.Format("Page {0}/{1}", CurrentPage, PageCount);
                stringBuilder.Insert(stringBuilder.Length, "═", Width - stringBuilder.Length - str.Length - 2);
                stringBuilder.Append(str);
                stringBuilder.Append("═╗");
            }
            _data.Append(stringBuilder);
        }

        public void Build()
        {
            _data = new StringBuilder(Height * Width);
            BuildHeader();
            var stringBuilder = new StringBuilder(Width*Height);
            for (var i = 0; i < Height - 2; i++)
            {
                stringBuilder.Append("║");
                stringBuilder.Insert(i*Width + 1, " ", Width - 2);
                stringBuilder.Append('║');
            }
            stringBuilder.Append("╚");
            stringBuilder.Insert((Height - 2)*Width + 1, "═", Width - 2);
            stringBuilder.Append('╝');
            _data.Append(stringBuilder);
        }

        public string Print()
        {
            return _data.ToString();
        }

        public void Fill(string[] content)
        {
            for (var i = 0; i < content.Length; i++)
            {
                _data.Remove((i + 1)*Width + 1, Math.Min(content[i].Length, Width - 2));
                _data.Insert((i + 1)*Width + 1, content[i]);
            }
        }
    }
}
