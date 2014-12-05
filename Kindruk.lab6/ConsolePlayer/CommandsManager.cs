using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;

namespace ConsolePlayer
{
    enum Commands
    {
        Unknown, Load, Kill, Next, Prev, Pause, Resume
    }

    public static class CommandsManager
    {
        public static readonly string CommandSeparators =
            ConfigurationManager.ConnectionStrings["CommandSeparators"].ConnectionString;

        public static void ManageCommand(string command)
        {
            var args = command.Split(CommandSeparators.ToCharArray()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            Commands commandType;
            if (!Enum.TryParse(args[0], true, out commandType))
            {
                commandType = Commands.Unknown;
            }
            switch (commandType)
            {
                case Commands.Load:
                    LoadPlayList(command, args);
                    break;
                case Commands.Kill:
                    KillPlayList(command, args);
                    break;
                case Commands.Next:
                    NextSongInPlayList(command, args);
                    break;
                case Commands.Prev:
                    PreviousSongInPlayList(command, args);
                    break;
                case Commands.Pause:
                    PausePlayList(command, args);
                    break;
                case Commands.Resume:
                    ResumePlayList(command, args);
                    break;
                case Commands.Unknown:
                    UnknownCommand(command);
                    break;
            }
        }

        public static void LoadPlayList(string command, string[] args)
        {
            if (args.Length > 2)
            {
                Player.Log.Add(
                    string.Format(ConfigurationManager.ConnectionStrings["UnknownCommand"].ConnectionString,
                        command));
                return;
            }
            if (File.Exists(args[1]))
            {
                try
                {
                    Player.LoadPlayList(args[1]);
                    Thread.Sleep(Player.RefreshRate);
                }
                catch (Exception)
                {
                    Player.Log.Add(
                        string.Format(ConfigurationManager.ConnectionStrings["WrongFileFormat"].ConnectionString,
                            args[1]));
                    Player.Log.Add(ConfigurationManager.ConnectionStrings["AbortingOperation"].ConnectionString);
                    return;
                }
                var playList = Player.PlayLists.Last();
                Player.Log.Add(string.Format(
                    ConfigurationManager.ConnectionStrings["PlayListLoaded"].ConnectionString,
                    playList.Data.Name, playList.Id, args[1]));
            }
            else
                Player.Log.Add(string.Format(
                    ConfigurationManager.ConnectionStrings["NoFileFound"].ConnectionString, args[1]));
        }

        public static void KillPlayList(string command, string[] args)
        {
            int id;
            if (args.Length > 2)
            {
                Player.Log.Add(
                    string.Format(ConfigurationManager.ConnectionStrings["UnknownCommand"].ConnectionString,
                        command));
                return;
            }
            if (int.TryParse(args[1], out id))
            {
                var playList = Player.PlayLists.Find(x => x.Id == id);
                if (playList == null)
                {
                    Player.Log.Add(string.Format(
                        ConfigurationManager.ConnectionStrings["InvalidId"].ConnectionString, args[1]));
                    return;
                }
                playList.StayAlive = false;
                Player.PlayLists.Remove(playList);
                Player.Log.Add(string.Format(
                    ConfigurationManager.ConnectionStrings["PlayListKilled"].ConnectionString,
                    playList.Data.Name, playList.Id));
            }
            else
                Player.Log.Add(string.Format(
                    ConfigurationManager.ConnectionStrings["InvalidId"].ConnectionString, args[1]));
        }

        public static void NextSongInPlayList(string command, string[] args)
        {
            int id;
            if (args.Length > 2)
            {
                Player.Log.Add(
                    string.Format(ConfigurationManager.ConnectionStrings["UnknownCommand"].ConnectionString,
                        command));
                return;
            }
            if (int.TryParse(args[1], out id))
            {
                var playList = Player.PlayLists.Find(x => x.Id == id);
                if (playList == null)
                {
                    Player.Log.Add(string.Format(
                        ConfigurationManager.ConnectionStrings["InvalidId"].ConnectionString, args[1]));
                    return;
                }
                playList.NextSong();
                Player.Log.Add(string.Format(
                    ConfigurationManager.ConnectionStrings["PlayListCurrentSongSwitched"].ConnectionString,
                    playList.Data.Name, playList.Id, playList.CurrentSong.Data.Name));
            }
            else
                Player.Log.Add(string.Format(
                    ConfigurationManager.ConnectionStrings["InvalidId"].ConnectionString, args[1]));
        }

        public static void PreviousSongInPlayList(string command, string[] args)
        {
            int id;
            if (args.Length > 2)
            {
                Player.Log.Add(
                    string.Format(ConfigurationManager.ConnectionStrings["UnknownCommand"].ConnectionString,
                        command));
                return;
            }
            if (int.TryParse(args[1], out id))
            {
                var playList = Player.PlayLists.Find(x => x.Id == id);
                if (playList == null)
                {
                    Player.Log.Add(string.Format(
                        ConfigurationManager.ConnectionStrings["InvalidId"].ConnectionString, args[1]));
                    return;
                }
                playList.PreviousSong();
                Player.Log.Add(string.Format(
                    ConfigurationManager.ConnectionStrings["PlayListCurrentSongSwitched"].ConnectionString,
                    playList.Data.Name, playList.Id, playList.CurrentSong.Data.Name));
            }
            else
                Player.Log.Add(string.Format(
                    ConfigurationManager.ConnectionStrings["InvalidId"].ConnectionString, args[1]));
        }

        public static void PausePlayList(string command, string[] args)
        {
            int id;
            if (args.Length > 2)
            {
                Player.Log.Add(
                    string.Format(ConfigurationManager.ConnectionStrings["UnknownCommand"].ConnectionString,
                        command));
                return;
            }
            if (int.TryParse(args[1], out id))
            {
                var playList = Player.PlayLists.Find(x => x.Id == id);
                if (playList == null)
                {
                    Player.Log.Add(string.Format(
                        ConfigurationManager.ConnectionStrings["InvalidId"].ConnectionString, args[1]));
                    return;
                }
                playList.Pause();
                Player.Log.Add(string.Format(
                    ConfigurationManager.ConnectionStrings["PlayListPaused"].ConnectionString,
                    playList.Data.Name, playList.Id));
            }
            else
                Player.Log.Add(string.Format(
                    ConfigurationManager.ConnectionStrings["InvalidId"].ConnectionString, args[1]));
        }

        public static void ResumePlayList(string command, string[] args)
        {
            int id;
            if (args.Length > 2)
            {
                Player.Log.Add(
                    string.Format(ConfigurationManager.ConnectionStrings["UnknownCommand"].ConnectionString,
                        command));
                return;
            }
            if (int.TryParse(args[1], out id))
            {
                var playList = Player.PlayLists.Find(x => x.Id == id);
                if (playList == null)
                {
                    Player.Log.Add(string.Format(
                        ConfigurationManager.ConnectionStrings["InvalidId"].ConnectionString, args[1]));
                    return;
                }
                playList.Resume();
                Player.Log.Add(string.Format(
                    ConfigurationManager.ConnectionStrings["PlayListResumed"].ConnectionString,
                    playList.Data.Name, playList.Id));
            }
            else
                Player.Log.Add(string.Format(
                    ConfigurationManager.ConnectionStrings["InvalidId"].ConnectionString, args[1]));
        }

        public static void UnknownCommand(string command)
        {
            Player.Log.Add(string.Format(
                ConfigurationManager.ConnectionStrings["UnknownCommand"].ConnectionString, command));
        }
    }
}
