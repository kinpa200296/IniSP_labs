using System.Xml.Linq;

namespace Audio
{
    public interface IStorable
    {
        XElement ToXElement();
        void ReadFromXElement(XElement element);
    }
}
