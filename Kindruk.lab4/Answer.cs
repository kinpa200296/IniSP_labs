using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Kindruk.lab4
{
    public class Answer : IDisposable, IEquatable<Answer>, IStreamable, IXmlWritable
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

        public void ReadFromXmlDocumnent(XDocument document)
        {
            ReadFromXmlElement(document.Root);
        }

        public XDocument WriteToXmlDocument()
        {
            return new XDocument(new XDeclaration("1.0", "utf-8", "yes"), WriteToXmlElement());
        }

        public void ReadFromXmlElement(XElement element)
        {
            if (element.Name != "Answer")
                throw new XmlException();
            Text = element.Value;
        }

        public XElement WriteToXmlElement()
        {
            return new XElement("Answer", Text);
        }

        public void WriteToXmlWriter(XmlWriter writer)
        {
            writer.WriteElementString("Answer", Text);
        }

        public void ReadFromXmlReader(XmlReader reader)
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name != "Answer")
                            throw new XmlException();
                        break;
                    case XmlNodeType.EndElement:
                        if (reader.Name != "Answer")
                            throw new XmlException();
                        return;
                    case XmlNodeType.Text:
                        Text = reader.Value;
                        break;
                    case XmlNodeType.Whitespace:
                        break;
                    default:
                        throw new XmlException();
                }
            }
        }
    }
}
