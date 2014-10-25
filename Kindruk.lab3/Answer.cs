using System;
using System.IO;

namespace Kindruk.lab3
{
    public class Answer : IDisposable, IEquatable<Answer>, IStreamable
    {
        private bool _disposed;

        public string Text { get; set; }

        public Answer()
        {
            Text = "<empty answer>";
        }

        public Answer(string answer)
        {
            Text = answer;
        }

        public bool Equals(Answer other)
        {
            if (ReferenceEquals(other, null))
                return false;
            return Text == other.Text;
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == GetType() && Equals((Answer)obj);
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }

        public static bool operator ==(Answer p1, Answer p2)
        {
            return !ReferenceEquals(p1, null) ? p1.Equals(p2) : ReferenceEquals(p2, null);
        }

        public static bool operator !=(Answer p1, Answer p2)
        {
            return !(p1 == p2);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
            }
            _disposed = true;
        }

        ~Answer()
        {
            Dispose(false);
        }

        public void WriteToBinaryStream(Stream stream)
        {
            var bw = new BinaryWriter(stream);
            bw.Write(Text);
            bw.Flush();
        }

        public void ReadFromBinaryStream(Stream stream)
        {
            var br = new BinaryReader(stream);
            Text = br.ReadString();
        }

        public void WriteToStream(Stream stream)
        {
            var sw = new StreamWriter(stream);
            sw.WriteLine(Text);
            sw.Flush();
        }

        public void ReadFromStream(StreamReader stream)
        {
            Text = stream.ReadLine();
        }
    }
}
