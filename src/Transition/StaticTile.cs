using Microsoft.VisualBasic.CompilerServices;
using System.Xml;
namespace Transition
{
    public class StaticTile
    {
        public short TileID { get; set; }
        public short AltIDMod { get; set; }
        public override string ToString()
        {
            return string.Format("{0:X4} [{1}]", this.TileID, this.AltIDMod);
        }
        public StaticTile()
        {
        }
        public StaticTile(short TileID, short AltIDMod)
        {
            this.TileID = TileID;
            this.AltIDMod = AltIDMod;
        }
        public StaticTile(XmlElement xmlInfo)
        {
            this.TileID = XmlConvert.ToInt16(xmlInfo.GetAttribute("TileID"));
            this.AltIDMod = XmlConvert.ToInt16(xmlInfo.GetAttribute("AltIDMod"));
        }
        public void Save(XmlTextWriter xmlInfo)
        {
            xmlInfo.WriteStartElement("StaticTile");
            xmlInfo.WriteAttributeString("TileID", StringType.FromInteger(TileID));
            xmlInfo.WriteAttributeString("AltIDMod", StringType.FromInteger(AltIDMod));
            xmlInfo.WriteEndElement();
        }
    }
}
