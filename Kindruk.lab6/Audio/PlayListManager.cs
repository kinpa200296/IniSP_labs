using System.IO;
using System.IO.Compression;
using System.Xml.Linq;

namespace Audio
{
    public static class PlayListManager
    {
        public static void Save(IStorable data, string filename)
        {
            using (var file = new FileStream(filename, FileMode.Create))
            {
                using (var compressedStream = new GZipStream(file, CompressionMode.Compress))
                {
                    var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), data.ToXElement());
                    doc.Save(compressedStream);
                }
            }
        }

        public static IStorable Load(string filename)
        {
            PlayList playList;
            using (var file = new FileStream(filename, FileMode.Open))
            {
                using (var compressedStream = new GZipStream(file, CompressionMode.Decompress))
                {
                    var doc = XDocument.Load(compressedStream);
                    playList = new PlayList();
                    playList.ReadFromXElement(doc.Root);
                }
            }
            return playList;
        }
    }
}
