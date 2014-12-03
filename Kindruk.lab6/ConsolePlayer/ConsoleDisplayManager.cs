using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
            _consoleBufferWidth = 80;

        private static ConsoleFrame _playlistsFrame, _logFrame;

        public static string CurrentInput { get; private set; }
        public static int CursorPosition { get; private set; }
        public static int FrameTitleDisplayLength { get; private set; }
        public static int PlayListsFrameWidth { get; private set; }
        public static int PlayListsFrameHeight { get; private set; }
        public static int LogFrameWidth { get; private set; }
        public static int LogFrameHeight { get; private set; }

        public static readonly string PlayListsFrameTitle =
            ConfigurationManager.ConnectionStrings["PlayListsFrameTitle"].ConnectionString,
            LogFrameTitle = ConfigurationManager.ConnectionStrings["LogFrameTitle"].ConnectionString,
            CommandSeparators = ConfigurationManager.ConnectionStrings["CommandSeparators"].ConnectionString;

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
                if (Console.KeyAvailable)
                {
                    var keys = new List<ConsoleKeyInfo>();
                    while (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);
                        keys.Add(key);
                    }
                    ProcessKeys(keys, out work);
                }
                ReDrawScreen();
                Thread.Sleep(Player.RefreshRate);
            }
        }

        public static void ReDrawScreen()
        {
            Console.Clear();
            _playlistsFrame.CurrentPage = PlayListsPageViewManager.CurrentPage + 1;
            _playlistsFrame.PageCount = PlayListsPageViewManager.PageCount + 1;
            _playlistsFrame.Build();
            _playlistsFrame.Fill(PlayListsPageViewManager.GetPageSnapShot());
            _playlistsFrame.Title.Scroll();
            Console.Write(_playlistsFrame.Print());
            //_logFrame.CurrentPage =
            //_logFrame.PageCount = 
            _logFrame.Build();
            //_logFrame.Fill();
            _logFrame.Title.Scroll();
            Console.Write(_logFrame.Print());
            Console.Write(">>> ");
            Console.Write(CurrentInput);
            Console.CursorLeft = (CursorPosition + 4)%ConsoleBufferWidth;
            Console.CursorTop = LogFrameHeight + PlayListsFrameHeight + (CursorPosition + 4)/ConsoleBufferWidth;
        }

        public static void Init()
        {
            var keys = new[]
            {
                "ConsoleBufferWidth", "ConsoleBufferHeight", "ConsoleWindowWidth", "ConsoleWindowHeight",
                "FrameTitleDisplayLength", "PlayListsFrameWidth", "PlayListsFrameHeight", "LogFrameWidth",
                "LogFrameHeight"
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
            CurrentInput = "";
            CursorPosition = 0;
            PlayListsPageViewManager.Init();
        }

        public static void ProcessKeys(IEnumerable<ConsoleKeyInfo> keys, out bool work)
        {
            var stringBuilder = new StringBuilder(CurrentInput, CurrentInput.Length);
            foreach (var key in keys)
            {
                if (key.Key == ConsoleKey.LeftArrow && key.Modifiers == (ConsoleModifiers.Control | ConsoleModifiers.Shift))
                {
                    Console.WriteLine("Log.GetPreviousPage");
                    continue;
                }
                if (key.Key == ConsoleKey.RightArrow && key.Modifiers == (ConsoleModifiers.Control | ConsoleModifiers.Shift))
                {
                    Console.WriteLine("Log.GetNextPage");
                    continue;
                }
                if (key.Key == ConsoleKey.LeftArrow && key.Modifiers == ConsoleModifiers.Control)
                {
                    PlayListsPageViewManager.PreviousPage();
                    continue;
                }
                if (key.Key == ConsoleKey.RightArrow && key.Modifiers == ConsoleModifiers.Control)
                {
                    PlayListsPageViewManager.NextPage();
                    continue;
                }
                if (key.Key == ConsoleKey.Q && key.Modifiers == ConsoleModifiers.Alt)
                {
                    work = false;
                    return;
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
                        ProcessCommand(stringBuilder.ToString());
                        stringBuilder.Clear();
                        CursorPosition = 0;
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
            Console.WriteLine();
            CurrentInput = stringBuilder.ToString();
            work = true;
        }

        public static void ProcessCommand(string command)
        {
            var args = command.Split(CommandSeparators.ToCharArray());
            Commands commandType;
            if (!Enum.TryParse(args[0], true, out commandType))
            {
                commandType = Commands.Unknown;
            }
            int id;
            switch (commandType)
            {
                case Commands.Load:
                    if (File.Exists(args[1]))
                    {
                        Player.LoadPlayList(args[1]);
                        Thread.Sleep(Player.RefreshRate);
                    }
                    break;
                case Commands.Kill:
                    if (int.TryParse(args[1], out id))
                    {
                        var playList = Player.PlayLists.Find(x => x.Id == id);
                        playList.StayAlive = false;
                        Player.PlayLists.Remove(playList);
                    }
                    break;
                case Commands.Next:
                    if (int.TryParse(args[1], out id))
                    {
                        Player.PlayLists.Find(x => x.Id == id).NextSong();
                    }
                    break;
                case Commands.Prev:
                    if (int.TryParse(args[1], out id))
                    {
                        Player.PlayLists.Find(x => x.Id == id).PreviousSong();
                    }
                    break;
                case Commands.Stop:
                    if (int.TryParse(args[1], out id))
                    {
                        Player.PlayLists.Find(x => x.Id == id).Stop();
                    }
                    break;
                case Commands.Resume:
                    if (int.TryParse(args[1], out id))
                    {
                        Player.PlayLists.Find(x => x.Id == id).Resume();
                    }
                    break;
                case Commands.Unknown:

                    break;
            }
        }
    }
}
