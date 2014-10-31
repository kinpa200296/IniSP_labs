using System.Xml;
using System.Xml.Linq;

namespace Kindruk.lab4
{
    public interface IXmlWritable
    {
        void ReadFromXmlDocumnent(XDocument document);
        XDocument WriteToXmlDocument();
        void ReadFromXmlElement(XElement element);
        XElement WriteToXmlElement();
        void WriteToXmlWriter(XmlWriter writer);
        void ReadFromXmlReader(XmlReader reader);
    }
}
