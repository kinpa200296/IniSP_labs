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
