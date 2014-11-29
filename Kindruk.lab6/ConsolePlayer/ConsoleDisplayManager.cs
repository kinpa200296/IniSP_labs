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
            _consoleBufferWidth = 80;

        public static TimeSpan RefreshRate { get; private set; }
        public static string CurrentInput { get; private set; }
        public static int CursorPosition { get; private set; }

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
                    Console.Clear();
                    ProcessKeys(keys, out work);
                    Console.Write(CurrentInput);
                    Console.CursorLeft = CursorPosition%ConsoleBufferWidth;
                    Console.CursorTop = CursorPosition/ConsoleBufferWidth + 1;
                }
                Thread.Sleep(RefreshRate);
            }
        }

        public static void Init()
        {
            var keys = new[]
            {
                "RefreshRate", "ConsoleBufferWidth", "ConsoleBufferHeight", "ConsoleWindowWidth", "ConsoleWindowHeight"
            };
            var values = keys.Select(x => int.Parse(ConfigurationManager.AppSettings[x])).ToArray();
            RefreshRate = new TimeSpan(0, 0, 0, 0, values[0]);
            ConsoleBufferWidth = values[1];
            ConsoleBufferHeight = values[2];
            ConsoleWindowWidth = values[3];
            ConsoleWindowHeight = values[4];
            CurrentInput = "";
            CursorPosition = 0;
        }

        static void ProcessKeys(IEnumerable<ConsoleKeyInfo> keys, out bool work)
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
                    Console.WriteLine("PlayListManager.GetPreviousPage");
                    continue;
                }
                if (key.Key == ConsoleKey.RightArrow && key.Modifiers == ConsoleModifiers.Control)
                {
                    Console.WriteLine("PlayListManager.GetNextPage");
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
                            : CursorPosition;
                        continue;
                    case ConsoleKey.DownArrow:
                        CursorPosition = CursorPosition + ConsoleBufferWidth < stringBuilder.Length
                            ? CursorPosition + ConsoleBufferWidth
                            : CursorPosition;
                        continue;
                    case ConsoleKey.Home:
                        CursorPosition = (CursorPosition/ConsoleBufferWidth)*ConsoleBufferWidth;
                        continue;
                    case ConsoleKey.Enter:
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
    }
}
