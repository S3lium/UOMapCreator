using Microsoft.VisualBasic.CompilerServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

namespace Terrain
{
    public class ClsTerrain
    {
        [Category("Tile ID")]
        public byte AltID { get; set; }

        [Category("Colour")]
        public Color Colour { get; set; }

        [Category("Key")]
        public int GroupID { get; set; }

        [Category("Group ID")]
        public string GroupIDHex => string.Format("{0:X}", this.GroupID);

        [Category("Description")]
        public string Name { get; set; }

        [Category("Tile ID")]
        public bool RandAlt { get; set; }

        [Category("Tile ID")]
        public short TileID { get; set; }

        public ClsTerrain(string iName, int iGroupID, short iTileID, Color iColor, byte iBase, bool iRandAlt)
        {
            this.Name = iName;
            this.GroupID = iGroupID;
            this.TileID = iTileID;
            this.Colour = iColor;
            this.AltID = iBase;
            this.RandAlt = iRandAlt;
        }

        public ClsTerrain(XmlElement xmlInfo)
        {
            this.Name = xmlInfo.GetAttribute("Name");
            this.GroupID = XmlConvert.ToInt32(xmlInfo.GetAttribute("ID"));
            this.TileID = XmlConvert.ToInt16(xmlInfo.GetAttribute("TileID"));
            this.Colour = Color.FromArgb(XmlConvert.ToByte(xmlInfo.GetAttribute("R")), XmlConvert.ToByte(xmlInfo.GetAttribute("G")), XmlConvert.ToByte(xmlInfo.GetAttribute("B")));
            this.AltID = XmlConvert.ToByte(xmlInfo.GetAttribute("Base"));
            string attribute = xmlInfo.GetAttribute("Random");
            if (StringType.StrCmp(attribute, "False", false) == 0)
            {
                this.RandAlt = false;
            }
            else if (StringType.StrCmp(attribute, "True", false) == 0)
            {
                this.RandAlt = true;
            }
        }

        public void Save(XmlTextWriter xmlInfo)
        {
            xmlInfo.WriteStartElement("Terrain");
            xmlInfo.WriteAttributeString("Name", this.Name);
            xmlInfo.WriteAttributeString("ID", StringType.FromInteger(this.GroupID));
            xmlInfo.WriteAttributeString("TileID", StringType.FromInteger(this.TileID));
            xmlInfo.WriteAttributeString("R", StringType.FromByte(this.Colour.R));
            xmlInfo.WriteAttributeString("G", StringType.FromByte(this.Colour.G));
            xmlInfo.WriteAttributeString("B", StringType.FromByte(this.Colour.B));
            xmlInfo.WriteAttributeString("Base", StringType.FromByte(this.AltID));
            xmlInfo.WriteAttributeString("Random", StringType.FromBoolean(this.RandAlt));
            xmlInfo.WriteEndElement();
        }

        public void SaveACO(BinaryWriter iACTFile)
        {
            byte num = 0;
            iACTFile.Write(this.Colour.R);
            iACTFile.Write(this.Colour.R);
            iACTFile.Write(this.Colour.G);
            iACTFile.Write(this.Colour.G);
            iACTFile.Write(this.Colour.B);
            iACTFile.Write(this.Colour.B);
            iACTFile.Write(num);
            iACTFile.Write(num);
        }

        public void SaveACOText(BinaryWriter iACTFile)
        {
            byte num = 0;
            iACTFile.Write(this.Colour.R);
            iACTFile.Write(this.Colour.R);
            iACTFile.Write(this.Colour.G);
            iACTFile.Write(this.Colour.G);
            iACTFile.Write(this.Colour.B);
            iACTFile.Write(this.Colour.B);
            iACTFile.Write(num);
            iACTFile.Write(num);
            iACTFile.Write(num);
            iACTFile.Write(num);
            byte[] bytes = (new UnicodeEncoding(true, true)).GetBytes(this.Name);
            byte num1 = Convert.ToByte(bytes.Length);
            byte num2 = checked((byte)Math.Round((double)num1 / 2 + 1));
            iACTFile.Write(num);
            iACTFile.Write(num2);
            byte[] numArray = bytes;
            for (int i = 0; i < numArray.Length; i++)
            {
                iACTFile.Write(numArray[i]);
            }
            iACTFile.Write(num);
            iACTFile.Write(num);
        }

        public void SaveACT(BinaryWriter iACTFile)
        {
            iACTFile.Write(this.Colour.R);
            iACTFile.Write(this.Colour.G);
            iACTFile.Write(this.Colour.B);
        }

        public override string ToString()
        {
            string str;
            str = (!this.RandAlt ? string.Format("[{0:X2}] {1}", this.GroupID, this.Name) : string.Format("[{0:X2}] *{1}", this.GroupID, this.Name));
            return str;
        }
    }
}