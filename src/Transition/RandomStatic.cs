using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Xml;
namespace Transition
{
    public class RandomStatic
    {
        public short TileID { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }
        public short Hue { get; set; }
        public RandomStatic()
        {
        }
        public RandomStatic(short iTileID, short iXMod, short iYMod, short iZMod, short iHueMod)
        {
            this.TileID = iTileID;
            this.X = iXMod;
            this.Y = iYMod;
            this.Z = iZMod;
            this.Hue = iHueMod;
        }
        public RandomStatic(XmlElement xmlInfo)
        {
            try
            {
                try
                {
                    this.TileID = XmlConvert.ToInt16(xmlInfo.GetAttribute("TileID"));
                }
                catch (Exception expr_22)
                {
                    ProjectData.SetProjectError(expr_22);
                    this.TileID = ShortType.FromString("&H" + xmlInfo.GetAttribute("TileID"));
                    ProjectData.ClearProjectError();
                }
                this.X = XmlConvert.ToInt16(xmlInfo.GetAttribute("X"));
                this.Y = XmlConvert.ToInt16(xmlInfo.GetAttribute("Y"));
                this.Z = XmlConvert.ToInt16(xmlInfo.GetAttribute("Z"));
                this.Hue = XmlConvert.ToInt16(xmlInfo.GetAttribute("Hue"));
            }
            catch (Exception expr_AC)
            {
                ProjectData.SetProjectError(expr_AC);
                Interaction.MsgBox(string.Format("Error\r\n{0}", xmlInfo.OuterXml), MsgBoxStyle.OkOnly, null);
                ProjectData.ClearProjectError();
            }
        }
        public override string ToString()
        {
            return string.Format("Tile:{0:X4} X:{1} Y:{2} Z:{3} Hue:{4}", new object[]
            {
                this.TileID,
                this.X,
                this.Y,
                this.Z,
                this.Hue
            });
        }
        public void Save(XmlTextWriter xmlInfo)
        {
            xmlInfo.WriteStartElement("Static");
            xmlInfo.WriteAttributeString("TileID", StringType.FromInteger(TileID));
            xmlInfo.WriteAttributeString("X", StringType.FromInteger(X));
            xmlInfo.WriteAttributeString("Y", StringType.FromInteger(Y));
            xmlInfo.WriteAttributeString("Z", StringType.FromInteger(Z));
            xmlInfo.WriteAttributeString("Hue", StringType.FromInteger(Hue));
            xmlInfo.WriteEndElement();
        }
    }
}
