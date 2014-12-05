using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsolePlayer
{
    public static class ConsoleDisplayManager
    {
        private static int _consoleWindowHeight = 26,
            _consoleWindowWidth = 80,
            _consoleBufferHeight = 26,
            _consoleBufferWidth = 80,
            _refreshCount = 1;

        private static ConsoleFrame _playlistsFrame, _logFrame;

        public static string CurrentInput { get; private set; }
        public static int CursorPosition { get; private set; }
        public static int FrameTitleDisplayLength { get; private set; }
        public static int PlayListsFrameWidth { get; private set; }
        public static int PlayListsFrameHeight { get; private set; }
        public static int LogFrameWidth { get; private set; }
        public static int LogFrameHeight { get; private set; }
        public static int ScrollingStringUpdateFrequency { get; private set; }
        public static int ScreenRedrawFrequency { get; private set; }

        public static readonly string PlayListsFrameTitle =
            ConfigurationManager.ConnectionStrings["PlayListsFrameTitle"].ConnectionString,
            LogFrameTitle = ConfigurationManager.ConnectionStrings["LogFrameTitle"].ConnectionString,
            ScrollingStringSeparator =
                ConfigurationManager.ConnectionStrings["ScrollingStringSeparator"].ConnectionString;

        public static int ConsoleWindowHeight
        {
            get { return _consoleWindowHeight; }
            set
            {
                if (value < 1 || _consoleWindowHeight == value) return;
                _consoleWindowHeight = value > 58 ? 58 : value;
                ConsoleBufferHeight = Math.Max(ConsoleBufferHeight, ConsoleWindowHeight);
                Console.SetWindowSize(ConsoleWindowWidth, ConsoleWindowHeight);
            }
        }

        public static int ConsoleWindowWidth
        {
            get { return _consoleWindowWidth; }
            set
            {
                if (value < 1 || _consoleWindowWidth == value) return;
                _consoleWindowWidth = value > 170 ? 170 : value;
                ConsoleBufferWidth = Math.Max(ConsoleBufferWidth, ConsoleWindowWidth);
                Console.SetWindowSize(ConsoleWindowWidth, ConsoleWindowHeight);
            }
        }

        public static int ConsoleBufferHeight
        {
            get { return _consoleBufferHeight; }
            set
            {
                if (value < 1 || _consoleBufferHeight == value) return;
                _consoleBufferHeight = value > 58 ? 58 : value;
                ConsoleWindowHeight = Math.Min(ConsoleWindowHeight, ConsoleBufferHeight);
                Console.SetBufferSize(ConsoleBufferWidth, ConsoleBufferHeight);
            }
        }

        public static int ConsoleBufferWidth
        {
            get { return _consoleBufferWidth; }
            set
            {
                if (value < 1 || _consoleBufferWidth == value) return;
                _consoleBufferWidth = value > 170 ? 170 : value;
                ConsoleWindowWidth = Math.Min(ConsoleWindowWidth, ConsoleBufferWidth);
                Console.SetBufferSize(ConsoleBufferWidth, ConsoleBufferHeight);
            }
        }

        public static void Launch()
        {
            try
            {
                Init();
            }
            catch (Exception e)
            {
                Console.WriteLine(ConfigurationManager.ConnectionStrings["UnhandledException"].ConnectionString,
                    e.TargetSite.DeclaringType + "." + e.TargetSite.Name, e.Message);
                throw new Exception("", e);
            }
            Run();
        }

        public static void Run()
        {
            var work = true;
            _playlistsFrame = new ConsoleFrame(PlayListsFrameTitle, PlayListsFrameWidth, PlayListsFrameHeight);
            _logFrame = new ConsoleFrame(LogFrameTitle, LogFrameWidth, LogFrameHeight);
            while (work)
            {
                var refreshScreen = false;
                if (Console.KeyAvailable)
                {
                    var keys = new List<ConsoleKeyInfo>();
                    var length = CurrentInput.Length;
                    while (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);
                        keys.Add(key);
                    }
                    refreshScreen = ProcessKeys(keys, out work);
                    if (!(_refreshCount%ScreenRedrawFrequency == 0 || refreshScreen))
                        ReDrawConsoleInput(length);
                }
                if (_refreshCount%ScrollingStringUpdateFrequency == 0)
                {
                    UpdateScrollingStrings();
                }
                if (_refreshCount%ScreenRedrawFrequency == 0 || refreshScreen)
                {
                    ReDrawScreen();   
                }
                _refreshCount = _refreshCount ==
                                MathExtension.Lcm(ScreenRedrawFrequency, ScrollingStringUpdateFrequency)
                    ? _refreshCount = 1
                    : _refreshCount + 1;
                Thread.Sleep(Player.RefreshRate);
            }
        }

        public static void UpdateScrollingStrings()
        {
            _playlistsFrame.Title.Scroll();
            _logFrame.Title.Scroll();
            for (var i = PlayListsPageViewManager.FirstPlayListOnPageIndex;
                i < PlayListsPageViewManager.FirstPlayListOnNextPageIndex;
                i++)
            {
                Player.PlayLists[i].Name.Scroll();
                Player.PlayLists[i].CurrentSong.Name.Scroll();
                Player.PlayLists[i].CurrentSong.Performer.Scroll();
                Player.PlayLists[i].CurrentSong.Genre.Scroll();
            }
        }

        public static void ReDrawScreen()
        {
            Console.Clear();
            _playlistsFrame.CurrentPage = PlayListsPageViewManager.CurrentPage + 1;
            _playlistsFrame.PageCount = PlayListsPageViewManager.PageCount;
            _playlistsFrame.Build();
            _playlistsFrame.Fill(PlayListsPageViewManager.GetPageSnapShot());
            Console.Write(_playlistsFrame.Print());
            _logFrame.CurrentPage = LogPageViewManager.CurrentPage + 1;
            _logFrame.PageCount = LogPageViewManager.PageCount;
            _logFrame.Build();
            _logFrame.Fill(LogPageViewManager.GetPageSnapShot());
            Console.Write(_logFrame.Print());
            ReDrawConsoleInput(0);
        }

        public static void ReDrawConsoleInput(int previousInputLength)
        {
            if (CurrentInput.Length < previousInputLength)
            {
                Console.SetCursorPosition(0, LogFrameHeight + PlayListsFrameHeight);
                var sb = new StringBuilder();
                sb.Append(' ', previousInputLength + 4);
                Console.Write(sb);
            }
            Console.SetCursorPosition(0, LogFrameHeight + PlayListsFrameHeight);
            Console.Write(">>> ");
            Console.Write(CurrentInput);
            Console.CursorLeft = (CursorPosition + 4) % ConsoleBufferWidth;
            Console.CursorTop = LogFrameHeight + PlayListsFrameHeight + (CursorPosition + 4) / ConsoleBufferWidth;
        }

        public static void Init()
        {
            var keys = new[]
            {
                "ConsoleBufferWidth", "ConsoleBufferHeight", "ConsoleWindowWidth", "ConsoleWindowHeight",
                "FrameTitleDisplayLength", "PlayListsFrameWidth", "PlayListsFrameHeight", "LogFrameWidth",
                "LogFrameHeight", "ScrollingStringUpdateFrequency", "ScreenRedrawFrequency"
            };
            var values = keys.Select(x => int.Parse(ConfigurationManager.AppSettings[x])).ToArray();
            ConsoleBufferWidth = values[0];
            ConsoleBufferHeight = values[1];
            ConsoleWindowWidth = values[2];
            ConsoleWindowHeight = values[3];
            FrameTitleDisplayLength = values[4];
            PlayListsFrameWidth = values[5];
            PlayListsFrameHeight = values[6];
            LogFrameWidth = values[7];
            LogFrameHeight = values[8];
            ScrollingStringUpdateFrequency = values[9];
            ScreenRedrawFrequency = values[10];
            CurrentInput = "";
            CursorPosition = 0;
            PlayListsPageViewManager.Init();
            LogPageViewManager.Init();
        }

        public static bool ProcessKeys(IEnumerable<ConsoleKeyInfo> keys, out bool work)
        {
            var stringBuilder = new StringBuilder(CurrentInput, CurrentInput.Length);
            var doRefresh = false;
            foreach (var key in keys)
            {
                if (key.Key == ConsoleKey.LeftArrow && key.Modifiers == (ConsoleModifiers.Control | ConsoleModifiers.Shift))
                {
                    LogPageViewManager.PreviousPage();
                    doRefresh = true;
                    continue;
                }
                if (key.Key == ConsoleKey.RightArrow && key.Modifiers == (ConsoleModifiers.Control | ConsoleModifiers.Shift))
                {
                    LogPageViewManager.NextPage();
                    doRefresh = true;
                    continue;
                }
                if (key.Key == ConsoleKey.LeftArrow && key.Modifiers == ConsoleModifiers.Control)
                {
                    PlayListsPageViewManager.PreviousPage();
                    doRefresh = true;
                    continue;
                }
                if (key.Key == ConsoleKey.RightArrow && key.Modifiers == ConsoleModifiers.Control)
                {
                    PlayListsPageViewManager.NextPage();
                    doRefresh = true;
                    continue;
                }
                if (key.Key == ConsoleKey.Q && key.Modifiers == ConsoleModifiers.Alt)
                {
                    work = false;
                    return false;
                }
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        CursorPosition = (CursorPosition - 1) >= 0 ? CursorPosition - 1 : CursorPosition;
                        continue;
                    case ConsoleKey.RightArrow:
                        CursorPosition = (CursorPosition + 1) <= stringBuilder.Length ? CursorPosition + 1 : CursorPosition;
                        continue;
                    case ConsoleKey.UpArrow:
                        CursorPosition = CursorPosition > ConsoleBufferWidth
                            ? CursorPosition - ConsoleBufferWidth
                            : 0;
                        continue;
                    case ConsoleKey.DownArrow:
                        CursorPosition = CursorPosition + ConsoleBufferWidth < stringBuilder.Length
                            ? CursorPosition + ConsoleBufferWidth
                            : stringBuilder.Length;
                        continue;
                    case ConsoleKey.Home:
                        CursorPosition = (CursorPosition/ConsoleBufferWidth)*ConsoleBufferWidth;
                        continue;
                    case ConsoleKey.Enter:
                        CommandsManager.ManageCommand(stringBuilder.ToString());
                        stringBuilder.Clear();
                        CursorPosition = 0;
                        doRefresh = true;
                        continue;
                    case ConsoleKey.End:
                        CursorPosition =
                            Math.Min((CursorPosition/ConsoleBufferWidth + 1)*ConsoleBufferWidth - 1,
                                stringBuilder.Length);
                        continue;
                    case ConsoleKey.Delete:
                        if (CursorPosition >= 0 && CursorPosition < stringBuilder.Length)
                        {
                            stringBuilder.Remove(CursorPosition, 1);
                        }
                        continue;
                    case ConsoleKey.Backspace:
                        if (CursorPosition > 0)
                        {
                            stringBuilder.Remove(CursorPosition - 1, 1);
                            CursorPosition--;
                        }
                        continue;
                    case ConsoleKey.Escape:
                        stringBuilder.Clear();
                        CursorPosition = 0;
                        continue;
                }
                if (CursorPosition + 1 == ConsoleBufferHeight*ConsoleBufferWidth )
                    continue;
                stringBuilder.Insert(CursorPosition, key.KeyChar);
                CursorPosition++;
            }
            CurrentInput = stringBuilder.ToString();
            work = true;
            return doRefresh;
        }
    }
}
