using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Kindruk.lab3
{
    public class Quiz : ILinkedList<Question>, IStreamable
    {
        private readonly LinkedList<Question> _questions = new LinkedList<Question>();
        private bool _disposed;

        #region constructors
        public Quiz()
        {
            Name = "<quiz is untitled>";
        }

        public Quiz(string name)
        {
            Name = name;
        }

        public Quiz(string name, IEnumerable<Question> questions)
        {
            Name = name;
            _questions = new LinkedList<Question>(questions);
        }
        #endregion

        #region properties
        public string Name { get; set; }

        public LinkedListNode<Question> First
        {
            get { return _questions.First; }
        }

        public LinkedListNode<Question> Last
        {
            get { return _questions.Last; }
        }
        public int Count 
        {
            get { return _questions.Count; }
        }

        public Question this[int index]
        {
            get { return _questions[index]; }
            set { _questions[index] = value; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
        #endregion

        public IEnumerator<Question> GetEnumerator()
        {
            return _questions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                foreach (var question in this)
                {
                    question.Dispose();
                }
            }
            _disposed = true;
        }

        public void AddAfter(LinkedListNode<Question> item, Question data)
        {
            _questions.AddAfter(item, data);
        }

        public void Add(Question data)
        {
            _questions.Add(data);
        }

        public void AddFirst(Question data)
        {
            _questions.AddFirst(data);
        }

        public void Delete(LinkedListNode<Question> item)
        {
            _questions.Delete(item);
        }

        public int IndexOf(Question item)
        {
            return _questions.IndexOf(item);
        }

        public void Clear()
        {
            _questions.Clear();
        }

        public bool Contains(Question item)
        {
            return _questions.Contains(item);
        }

        public void CopyTo(Question[] array, int arrayIndex)
        {
            _questions.CopyTo(array, arrayIndex);
        }

        public LinkedListNode<Question> GetNodeByData(Question data)
        {
            return _questions.GetNodeByData(data);
        }

        public LinkedListNode<Question> GetNodeByIndex(int index)
        {
            return _questions.GetNodeByIndex(index);
        }

        ~Quiz()
        {
            Dispose(false);
        }

        public void WriteBinaryToStream(Stream stream)
        {
            var bw = new BinaryWriter(stream);
            bw.Write(Name);
            bw.Write(Count);
            bw.Flush();
            foreach (var question in this)
            {
                question.WriteBinaryToStream(stream);
            }
        }

        public void ReadBinaryFromStream(Stream stream)
        {
            var br = new BinaryReader(stream);
            Name = br.ReadString();
            var count = br.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var question = new Question();
                question.ReadBinaryFromStream(stream);
                _questions.Add(question);
            }
        }

        public void WriteToStream(Stream stream)
        {
            var sw = new StreamWriter(stream);
            sw.WriteLine(Name);
            sw.WriteLine(Count);
            sw.Flush();
            foreach (var question in this)
            {
                question.WriteToStream(stream);
            }
        }

        public void ReadFromStream(StreamReader stream)
        {
            Name = stream.ReadLine();
            var count = int.Parse(stream.ReadLine());
            for (var i = 0; i < count; i++)
            {
                var question = new Question();
                question.ReadFromStream(stream);
                _questions.Add(question);
            }
        }
    }
}
