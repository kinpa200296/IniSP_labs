using System.IO;
using System.IO.Compression;

namespace Kindruk.lab4
{
    public static class QuizManager
    {
        public static void WriteToBinaryFile(string filename, IStreamable quiz)
        {
            using (var file = new FileStream(filename, FileMode.Create))
            {
                using (var compressedStream = new GZipStream(file, CompressionMode.Compress))
                {
                    quiz.WriteToBinaryStream(compressedStream);
                }
            }
        }

        public static Quiz ReadFromBinaryFile(string filename)
        {
            var quiz = new Quiz();
            using (var file = new FileStream(filename, FileMode.Open))
            {
                using (var compressedStream = new GZipStream(file, CompressionMode.Decompress))
                {
                    quiz.ReadFromBinaryStream(compressedStream);
                }
            }
            return quiz;
        }

        public static void WriteToFile(string filename, IStreamable quiz)
        {
            using (var file = new FileStream(filename, FileMode.Create))
            {
                using (var compressedStream = new GZipStream(file, CompressionMode.Compress))
                {
                    quiz.WriteToStream(compressedStream);
                }
            }
        }

        public static Quiz ReadFromFile(string filename)
        {
            var quiz = new Quiz();
            using (var file = new FileStream(filename, FileMode.Open))
            {
                using (var compressedStream = new GZipStream(file, CompressionMode.Decompress))
                {
                    quiz.ReadFromStream(new StreamReader(compressedStream));
                }
            }
            return quiz;
        }
    }
}
