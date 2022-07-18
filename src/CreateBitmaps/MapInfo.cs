using Microsoft.VisualBasic.CompilerServices;
using System.Xml;

namespace CreateBitmaps
{
    public class MapInfo
    {
        public string MapName { get; }

        public byte MapNumber { get; }

        public int XSize { get; }

        public int YSize { get; }

        public MapInfo(XmlElement iXml)
        {
            this.MapName = iXml.GetAttribute("Name");
            this.MapNumber = ByteType.FromString(iXml.GetAttribute("Num"));
            this.XSize = IntegerType.FromString(iXml.GetAttribute("XSize"));
            this.YSize = IntegerType.FromString(iXml.GetAttribute("YSize"));
        }

        public override string ToString()
        {
            return string.Format("{0}", this.MapName);
        }
    }
}
