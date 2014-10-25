using System.IO;

namespace Kindruk.lab3
{
    public interface IStreamable
    {
        void WriteBinaryToStream(Stream stream);
        void ReadBinaryFromStream(Stream stream);
        void WriteToStream(Stream stream);
        void ReadFromStream(StreamReader stream);
    }
}
