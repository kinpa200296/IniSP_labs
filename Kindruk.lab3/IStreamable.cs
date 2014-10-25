using System.IO;

namespace Kindruk.lab3
{
    public interface IStreamable
    {
        void WriteToBinaryStream(Stream stream);
        void ReadFromBinaryStream(Stream stream);
        void WriteToStream(Stream stream);
        void ReadFromStream(StreamReader stream);
    }
}
