using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kindruk.lab3
{
    class Question : ILinkedList<Answer>, IEquatable<Question>
    {
        private LinkedList<Answer> _answers = new LinkedList<Answer>();
        private bool _disposed;

        public string Text { get; set; }

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
    }
}
