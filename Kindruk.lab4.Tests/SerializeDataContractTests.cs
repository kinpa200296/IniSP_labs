using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kindruk.lab4.Tests
{
    [TestClass]
    public class SerializeDataContractTests
    {
        [TestMethod]
        public void RuntimeSerializationTests()
        {
            var quiz1 = new Quiz();
            quiz1.ReadFromXmlDocumnent(XDocument.Load("C# test 2.xml"));
            var formatter = new BinaryFormatter();
            using (Stream file = File.Create("C# test 2(serilized).dat"))
            {
                formatter.Serialize(file, quiz1);
            }
            Quiz quiz2;
            using (Stream file = File.OpenRead("C# test 2(serilized).dat"))
            {
                quiz2 = (Quiz) formatter.Deserialize(file);
            }
            Assert.AreEqual(true, quiz1.SequenceEqual(quiz2));
        }

        [TestMethod]
        public void DataContractTests()
        {
            var quiz1 = new Quiz();
            quiz1.ReadFromXmlDocumnent(XDocument.Load("C# test 2.xml"));
            QuizManager.WriteToXmlViaDataContract("C# test 2(DataContract).xml", quiz1);
            var quiz2 = QuizManager.ReadFromXmlViaDataContract("C# test 2(DataContract).xml");
            Assert.AreEqual(true, quiz1.SequenceEqual(quiz2));
        }

        [TestMethod]
        public void XmlSerializerTests()
        {
            var quiz1 = QuizManager.ReadFromXmlViaDataContract("C# test 2(DataContract).xml");
            QuizManager.WriteToXmlViaXmlSerializer("C# test 2(XmlSerializer).xml", quiz1);
            var quiz2 = QuizManager.ReadFromXmlViaXmlSerializer("C# test 2(XmlSerializer).xml");
            Assert.AreEqual(true, quiz1.SequenceEqual(quiz2));
        }
    }
}
