using System.Collections;
using System.Collections.Generic;

namespace Kindruk.lab3
{
    class Quiz : ILinkedList<Question>
    {
        private LinkedList<Question> _questions = new LinkedList<Question>();
        private bool _disposed;

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
    }
}
