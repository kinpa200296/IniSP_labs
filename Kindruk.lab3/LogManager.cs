using System;
using System.IO;
using System.Linq;

namespace Kindruk.lab3
{
    public class LogManager : IDisposable
    {
        private readonly FileStream _file;
        public readonly StreamWriter LogFile;

        private FileSystemWatcher _watcher;
        private readonly string _logFileName;

        private bool _disposed;

        public LogManager(string logfilename = "CurrentLog.txt")
        {
            _file = new FileStream(logfilename, FileMode.Create);
            LogFile = new StreamWriter(_file);
            _logFileName = logfilename;
            BeginLog();
        }

        public void BeginLog()
        {
            var dir = new DirectoryInfo(".");
            _watcher = new FileSystemWatcher
            {
                Path = dir.FullName,
                Filter = @"*.*",
                IncludeSubdirectories = true
            };
            FileSystemEventHandler handler = (o, e) => LogFile.WriteLine("File {0} was {1}", e.FullPath, e.ChangeType);
            _watcher.Created += handler;
            _watcher.Deleted += handler;
            _watcher.Changed += handler;
            _watcher.Renamed += (o, e) => LogFile.WriteLine("Renamed {0} -> {1}", e.OldFullPath, e.FullPath);
            _watcher.Error += (o, e) => LogFile.WriteLine("Error: {0}", e.GetException().Message);
            _watcher.EnableRaisingEvents = true;
        }

        public void EndLog()
        {
            _watcher.Dispose();
            LogFile.Flush();
            LogFile.Dispose();
            _file.Dispose();
            var dir = new DirectoryInfo(".");
            if (!dir.GetDirectories().Contains(new DirectoryInfo(dir.FullName + @"\logs")))
                dir.CreateSubdirectory("logs");
            try
            {
                File.Move(dir.FullName + "\\" + _logFileName, dir.FullName + @"\logs\log_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".txt");
            }
            catch (IOException)
            {
                System.Threading.Thread.Sleep(1000);
                File.Move(dir.FullName + "\\" + _logFileName, dir.FullName + @"\logs\log_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".txt");
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing) { }
            EndLog();
            _disposed = true;
        }

        ~LogManager()
        {
            Dispose(false);
        }
    }
}
