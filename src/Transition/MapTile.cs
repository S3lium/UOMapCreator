using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Xml;
namespace Transition
{
    public class MapTile
    {
        public short TileID { get; set; }
        public short AltIDMod { get; set; }
        public override string ToString()
        {
            return string.Format("{0:X4} [{1}]", this.TileID, this.AltIDMod);
        }
        public MapTile(short TileID, short AltID)
        {
            this.TileID = TileID;
            this.AltIDMod = AltID;
        }
        public MapTile()
        {
        }
        public MapTile(XmlElement xmlInfo)
        {
            try
            {
                this.TileID = XmlConvert.ToInt16(xmlInfo.GetAttribute("TileID"));
            }
            catch (Exception expr_21)
            {
                ProjectData.SetProjectError(expr_21);
                this.TileID = ShortType.FromString("&H" + xmlInfo.GetAttribute("TileID"));
                ProjectData.ClearProjectError();
            }
            this.AltIDMod = XmlConvert.ToInt16(xmlInfo.GetAttribute("AltIDMod"));
        }
        public void Save(XmlTextWriter xmlInfo)
        {
            xmlInfo.WriteStartElement("MapTile");
            xmlInfo.WriteAttributeString("TileID", StringType.FromInteger(TileID));
            xmlInfo.WriteAttributeString("AltIDMod", StringType.FromInteger(AltIDMod));
            xmlInfo.WriteEndElement();
        }
    }
}
