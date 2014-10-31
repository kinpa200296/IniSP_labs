using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Kindruk.lab4
{
    public class Question : ILinkedList<Answer>, IEquatable<Question>, IStreamable, IXmlWritable
    {
        private readonly ILinkedList<Answer> _answers = new LinkedList<Answer>();
        private bool _disposed;

        #region constructors
        public Question()
        {
            Text = "<no question text>";
            CorrectAnswerPos = 0;
        }

        public Question(string text, int correctAnswerPos = 0)
        {
            Text = text;
            CorrectAnswerPos = correctAnswerPos;
        }

        public Question(string text, int correctAnswerPos, IEnumerable<Answer> answers)
        {
            Text = text;
            CorrectAnswerPos = correctAnswerPos;
            _answers = new LinkedList<Answer>(answers);
        }
        #endregion

        #region properties
        public string Text { get; set; }

        public int CorrectAnswerPos { get; private set; }

        public Answer CorrectAnswer
        {
            get
            {
                return CorrectAnswerPos == -1 ? new Answer("<no correct answer>") : _answers[CorrectAnswerPos];
            }
        }

        public ILinkedListNode<Answer> First
        {
            get { return _answers.First; }
        }

        public ILinkedListNode<Answer> Last
        {
            get { return _answers.Last; } 
        }

        public int Count
        {
            get { return _answers.Count; }

        }

        public Answer this[int index]
        {
            get { return _answers[index]; }
            set { _answers[index] = value; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
        #endregion

        public IEnumerator<Answer> GetEnumerator()
        {
            return _answers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
                foreach (var answer in this)
                {
                    answer.Dispose();
                }
            }
            _disposed = true;
        }

        public void AddAfter(ILinkedListNode<Answer> item, Answer data)
        {
            _answers.AddAfter(item, data);
        }

        public void Add(Answer data)
        {
            _answers.Add(data);
        }

        public void AddFirst(Answer data)
        {
            _answers.AddFirst(data);
        }

        public void Delete(ILinkedListNode<Answer> item)
        {
            _answers.Delete(item);
        }

        public int IndexOf(Answer item)
        {
            return _answers.IndexOf(item);
        }

        public void Clear()
        {
            _answers.Clear();
        }

        public bool Contains(Answer item)
        {
            return _answers.Contains(item);
        }

        public void CopyTo(Answer[] array, int arrayIndex)
        {
            _answers.CopyTo(array, arrayIndex);
        }

        public ILinkedListNode<Answer> GetNodeByData(Answer data)
        {
            return _answers.GetNodeByData(data);
        }

        public ILinkedListNode<Answer> GetNodeByIndex(int index)
        {
            return _answers.GetNodeByIndex(index);
        }

        public bool Equals(Question other)
        {
            if (Text != other.Text) return false;
            var first = this.OrderBy(item => item.Text);
            var second = other.OrderBy(item => item.Text);
            return first.SequenceEqual(second);
        }

        ~Question()
        {
            Dispose(false);
        }

        public void WriteToBinaryStream(Stream stream)
        {
            var bw = new BinaryWriter(stream);
            bw.Write(Text);
            bw.Write(Count);
            bw.Write(CorrectAnswerPos);
            bw.Flush();
            foreach (var answer in this)
            {
                answer.WriteToBinaryStream(stream);
            }
        }

        public void ReadFromBinaryStream(Stream stream)
        {
            var br = new BinaryReader(stream);
            Text = br.ReadString();
            var count = br.ReadInt32();
            CorrectAnswerPos = br.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var answer = new Answer();
                answer.ReadFromBinaryStream(stream);
                _answers.Add(answer);
            }
        }

        public void WriteToStream(Stream stream)
        {
            var sw = new StreamWriter(stream);
            sw.WriteLine(Text);
            sw.WriteLine(Count);
            sw.WriteLine(CorrectAnswerPos);
            sw.Flush();
            foreach (var answer in this)
            {
                answer.WriteToStream(stream);
            }

        }

        public void ReadFromStream(StreamReader stream)
        {
            Text = stream.ReadLine();
            var count = int.Parse(stream.ReadLine());
            CorrectAnswerPos = int.Parse(stream.ReadLine());
            for (var i = 0; i < count; i++)
            {
                var answer = new Answer();
                answer.ReadFromStream(stream);
                _answers.Add(answer);
            }
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
            if (element.Name != "Question")
                throw new XmlException();
            CorrectAnswerPos = int.Parse(element.Attribute("Correct_Answer").Value);
            Text = element.Element("Text").Value;
            _answers.Clear();
            var xAnswers = element.Element("Answers").Elements();
            foreach (var xAnswer in xAnswers)
            {
                var answer = new Answer();
                answer.ReadFromXmlElement(xAnswer);
                _answers.Add(answer);
            }
        }

        public XElement WriteToXmlElement()
        {
            return new XElement("Question", new XAttribute("Answers_Count", _answers.Count), 
                new XAttribute("Correct_Answer", CorrectAnswerPos), new XElement("Text", Text),
                    new XElement("Answers",
                        from answer in _answers
                            select answer.WriteToXmlElement()));
        }

        public void WriteToXmlWriter(XmlWriter writer)
        {
            writer.WriteStartElement("Question");
            writer.WriteAttributeString("Answers_Count", _answers.Count.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("Correct_Answer", CorrectAnswerPos.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("Text", Text);
            writer.WriteStartElement("Answers");
            foreach (var answer in _answers)
            {
                answer.WriteToXmlWriter(writer);
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        public void ReadFromXmlReader(XmlReader reader)
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Attribute:
                        switch (reader.Name)
                        {
                            case "Answers_Count":
                                break;
                            case "Correct_Answer":
                                CorrectAnswerPos = int.Parse(reader.Value);
                                break;
                            default:
                                throw new XmlException();
                        }
                        break;
                    case XmlNodeType.Element:
                        switch (reader.Name)
                        {
                            case "Question":
                            case "Text":
                                break;
                            case "Answers":
                                _answers.Clear();
                                reader.Read();
                                while (reader.Name != "Answers")
                                {
                                    var ans = new Answer();
                                    ans.ReadFromXmlReader(reader);
                                    _answers.Add(ans);
                                    reader.Read();
                                    if (reader.NodeType == XmlNodeType.Whitespace)
                                        reader.Read();
                                }
                                break;
                            default:
                                throw new XmlException();
                        }
                        break;
                    case XmlNodeType.EndElement:
                        switch (reader.Name)
                        {
                            case "Question":
                                return;
                            case "Text":
                            case "Answers":
                                break;
                            default:
                                throw new XmlException();
                        }
                        break;
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
