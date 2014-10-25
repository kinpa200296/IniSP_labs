using System.IO;
using System.IO.Compression;

namespace Kindruk.lab3
{
    public static class QuizManager
    {
        public static void WriteBinaryToFile(string filename, Quiz quiz)
        {
            using (var file = new FileStream(filename, FileMode.Create))
            {
                using (var compressedStream = new GZipStream(file, CompressionMode.Compress))
                {
                    quiz.WriteBinaryToStream(compressedStream);
                }
            }
        }

        public static Quiz ReadBinaryFromFile(string filename)
        {
            var quiz = new Quiz();
            using (var file = new FileStream(filename, FileMode.Open))
            {
                using (var compressedStream = new GZipStream(file, CompressionMode.Decompress))
                {
                    quiz.ReadBinaryFromStream(compressedStream);
                }
            }
            return quiz;
        }

        public static void WriteToFile(string filename, Quiz quiz)
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
