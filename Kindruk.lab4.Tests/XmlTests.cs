using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kindruk.lab4.Tests
{
    [TestClass]
    public class XmlTests
    {
        [TestMethod]
        public void LinqToXmlTests()
        {
            var quiz1 = QuizManager.ReadFromFile("C# test 2.quiz");
            quiz1.WriteToXmlDocument().Save("C# test 2.xml");
            var quiz2 = new Quiz();
            quiz2.ReadFromXmlDocumnent(XDocument.Load("C# test 2.xml"));
            Assert.AreEqual(true, quiz1.SequenceEqual(quiz2));
            quiz2 = QuizManager.ReadFromFile("C# test.quiz");
            var document = quiz2.WriteToXmlDocument();
            var questions = document.Root.Element("Questions");
            var answers = new LinkedList<Answer>
                {
                    new Answer("IEnumrable<T>"),
                    new Answer("IEquatable<T>"),
                    new Answer("IList<T>"),
                    new Answer("IDisposable")
                };
            if (questions == null)
                throw new NullReferenceException();
            questions.Add(new Question("Какой интерфейс позволяет использовать конструкцию foreach?", 0, answers).WriteToXmlElement());
            questions.Add(new Question("Какой интерфейс гарантирует, что класс сравним по равенству?", 1, answers).WriteToXmlElement());
            questions.Add(
                new Question(
                    "Какой интерфейс сообщает сборщику мусора о необходимости высвобождения неуправляемых ресурсов?",
                    3, answers).WriteToXmlElement());
            quiz2.ReadFromXmlElement(document.Root);
            Assert.AreEqual(true, quiz1.SequenceEqual(quiz2));
        }

        [TestMethod]
        public void ReadWriteXmlFileTests()
        {
            var quiz1 = new Quiz();
            quiz1.ReadFromXmlDocumnent(XDocument.Load("C# test 2.xml"));
            QuizManager.WriteToXmlFile("C# test 2(writer).xml", quiz1);
            var quiz2 = QuizManager.ReadFromXmlFile("C# test 2(writer).xml");
            Assert.AreEqual(true, quiz1.SequenceEqual(quiz2));
        }
    }
}
