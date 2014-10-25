using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Kindruk.lab3
{
    public class Question : ILinkedList<Answer>, IEquatable<Question>, IStreamable
    {
        private readonly LinkedList<Answer> _answers = new LinkedList<Answer>();
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

        public LinkedListNode<Answer> First
        {
            get { return _answers.First; }
        }

        public LinkedListNode<Answer> Last
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

        public void AddAfter(LinkedListNode<Answer> item, Answer data)
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

        public void Delete(LinkedListNode<Answer> item)
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

        public LinkedListNode<Answer> GetNodeByData(Answer data)
        {
            return _answers.GetNodeByData(data);
        }

        public LinkedListNode<Answer> GetNodeByIndex(int index)
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

        public void WriteBinaryToStream(Stream stream)
        {
            var bw = new BinaryWriter(stream);
            bw.Write(Text);
            bw.Write(Count);
            bw.Write(CorrectAnswerPos);
            bw.Flush();
            foreach (var answer in this)
            {
                answer.WriteBinaryToStream(stream);
            }
        }

        public void ReadBinaryFromStream(Stream stream)
        {
            var br = new BinaryReader(stream);
            Text = br.ReadString();
            var count = br.ReadInt32();
            CorrectAnswerPos = br.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var answer = new Answer();
                answer.ReadBinaryFromStream(stream);
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
    }
}
