using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsolePlayer
{
    public class LogManager : IDisposable
    {
        private readonly FileStream _file;
        private readonly StreamWriter _logFile;
        private readonly List<string> _logStrings = new List<string>(); 

        private readonly string _logFileName;

        private bool _disposed;

        public int LogSize{get { return _logStrings.Count; }}

        public LogManager(string logfilename = "CurrentLog.txt")
        {
            _file = new FileStream(logfilename, FileMode.Create);
            _logFile = new StreamWriter(_file);
            _logFileName = logfilename;
            _logStrings.Clear();
        }

        public void ClearLog()
        {
            _logFile.Flush();
            _logStrings.Clear();
        }

        public string GetByIndex(int index)
        {
            return _logStrings[index];
        }

        public void Add(string s)
        {
            _logFile.WriteLine(s);
            var sb = new StringBuilder(s);
            for (var i = 0; i < s.Length; i += ConsoleDisplayManager.LogFrameWidth - 4)
            {
                _logStrings.Add(sb.ToString(i, Math.Min(ConsoleDisplayManager.LogFrameWidth - 4, s.Length - i)));
            }
            LogPageViewManager.ShowLastPage();
        }

        public void EndLog()
        {
            _logFile.Flush();
            _logFile.Dispose();
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
