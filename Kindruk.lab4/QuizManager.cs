using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

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

        public static Quiz ReadFromXmlFile(string filename)
        {
            var quiz = new Quiz();
            var settings = new XmlReaderSettings {IgnoreComments = true, IgnoreProcessingInstructions = true};
            using (var reader = XmlReader.Create(filename, settings))
            {
                reader.Read();
                quiz.ReadFromXmlReader(reader);
            }
            return quiz;
        }

        public static void WriteToXmlFile(string filename, IXmlWritable quiz)
        {
            var settings = new XmlWriterSettings {CloseOutput = true, Encoding = Encoding.GetEncoding("utf-8"), Indent = true};
            using (var writer = XmlWriter.Create(filename, settings))
            {
                writer.WriteStartDocument(true);
                quiz.WriteToXmlWriter(writer);
                writer.WriteEndDocument();
            }
        }

        public static void WriteToXmlViaDataContract(string filename, Quiz quiz)
        {
            var ds = new DataContractSerializer(typeof(Quiz), new[] { typeof(LinkedList<Question>), typeof(LinkedList<Answer>) }.Select(a => a));
            using (Stream file = File.Create(filename))
            {
                ds.WriteObject(file, quiz);
            }
        }

        public static Quiz ReadFromXmlViaDataContract(string filename)
        {
            var ds = new DataContractSerializer(typeof(Quiz), new[] { typeof(LinkedList<Question>), typeof(LinkedList<Answer>) }.Select(a => a));
            using (Stream file = File.OpenRead(filename))
            {
                return (Quiz) ds.ReadObject(file);
            }
        }
    }
}
