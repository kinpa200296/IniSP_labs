using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kindruk.lab4.Tests
{
    [TestClass]
    public class LinkedListTests
    {
        [TestMethod]
        public void AddTests()
        {
            var list = new LinkedList<Answer> 
            {
                new Answer("string"), 
                new Answer("double"), 
                new Answer("int") 
            };
            Assert.AreEqual("int", list[2].Text);
            Assert.AreEqual("double", list[1].Text);
            Assert.AreEqual("string", list[0].Text);
            list.Add(new Answer("bool"));
            list.AddAfter(list.GetNodeByData(new Answer("int")), new Answer("long"));
            list.AddAfter(list.GetNodeByData(new Answer("double")), new Answer("float"));
            list.AddAfter(list.GetNodeByData(new Answer("bool")), new Answer("BigInteger"));
            list.Add(new Answer("StringBuilder"));
            Assert.AreEqual("string", list[0].Text);
            Assert.AreEqual("double", list[1].Text);
            Assert.AreEqual("float", list[2].Text);
            Assert.AreEqual("int", list[3].Text);
            Assert.AreEqual("long", list[4].Text);
            Assert.AreEqual("bool", list[5].Text);
            Assert.AreEqual("BigInteger", list[6].Text);
            Assert.AreEqual("StringBuilder", list[7].Text);
        }

        [TestMethod]
        public void DeleteTests()
        {
            var list = new LinkedList<Answer> 
            {
                new Answer("string"), 
                new Answer("double"), 
                new Answer("int") 
            };
            list.Delete(list.First);
            Assert.AreEqual("int", list[1].Text);
            Assert.AreEqual("double", list[0].Text);
            list.Add(new Answer("bool"));
            list.AddAfter(list.GetNodeByData(new Answer("int")), new Answer("long"));
            list.AddAfter(list.GetNodeByData(new Answer("double")), new Answer("float"));
            list.AddAfter(list.GetNodeByData(new Answer("bool")), new Answer("BigInteger"));
            list.Add(new Answer("StringBuilder"));
            Assert.AreEqual("double", list[0].Text);
            Assert.AreEqual("float", list[1].Text);
            Assert.AreEqual("int", list[2].Text);
            Assert.AreEqual("long", list[3].Text);
            Assert.AreEqual("bool", list[4].Text);
            Assert.AreEqual("BigInteger", list[5].Text);
            Assert.AreEqual("StringBuilder", list[6].Text);
            list.Delete(list.Last);
            list.Delete(list.GetNodeByData(new Answer("long")));
            list.Delete(list.GetNodeByData(new Answer("float")));
            Assert.AreEqual("double", list[0].Text);
            Assert.AreEqual("int", list[1].Text);
            Assert.AreEqual("bool", list[2].Text);
            Assert.AreEqual("BigInteger", list[3].Text);
        }

        [TestMethod]
        public void ClearTests()
        {
            var list = new LinkedList<Answer> 
            {
                new Answer("string"), 
                new Answer("double"), 
                new Answer("int") 
            };
            list.Clear();
            Assert.AreEqual(0, list.Count);
            Assert.AreEqual(0, list.Count());
            list.Add(new Answer("bool"));
            list.AddAfter(list.GetNodeByData(new Answer("bool")), new Answer("BigInteger"));
            list.Add(new Answer("StringBuilder"));
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(3, list.Count());
            list.Clear();
            Assert.AreEqual(0, list.Count);
            Assert.AreEqual(0, list.Count());
        }

        [TestMethod]
        public void ExceptionsTests()
        {
            var list = new LinkedList<Answer>
            {
                new Answer("string"), 
                new Answer("double"), 
                new Answer("int") 
            };
            try
            {
                Console.WriteLine(list[-1]);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            try
            {
                list[2] = new Answer("long");
                Console.WriteLine(list[3]);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void LinqTests()
        {
            var list = new LinkedList<Answer> 
            {
                new Answer("string"), 
                new Answer("double"), 
                new Answer("int") 
            };
            foreach (var answer in list.Where(x => list.IndexOf(x) > 1))
            {
                Assert.AreEqual("int", answer.Text);
            }
            var temp = from item in list
                where list.IndexOf(item) > 0
                select item;
            var enumerable = temp as Answer[] ?? temp.ToArray();
            Assert.AreEqual(2, enumerable.Count());
            Assert.AreEqual(true, enumerable.Contains(new Answer("int")));
            Assert.AreEqual(true, enumerable.Contains(new Answer("double")));
        }
    }
}
