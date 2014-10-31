using System.IO;

namespace Kindruk.lab4
{
    public interface IStreamable
    {
        void WriteToBinaryStream(Stream stream);
        void ReadFromBinaryStream(Stream stream);
        void WriteToStream(Stream stream);
        void ReadFromStream(StreamReader stream);
    }
}
