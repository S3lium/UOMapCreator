using Microsoft.VisualBasic.CompilerServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

namespace Elevation
{
    public class ClsElevation
    {
        [Category("ID")]
        public Color AltitudeColor { get; set; }

        [Category("Method")]
        public short GetAltitude { get; set; }

        [Category("Key")]
        public int Key { get; set; }

        [Category("ID")]
        public string Type { get; set; }

        public ClsElevation(int iKey, string iType, short iAlt, Color iAltColor)
        {
            this.Key = iKey;
            this.Type = iType;
            this.GetAltitude = iAlt;
            this.AltitudeColor = iAltColor;
        }

        public ClsElevation(XmlElement xmlInfo)
        {
            this.Key = XmlConvert.ToInt32(xmlInfo.GetAttribute("Key"));
            this.Type = xmlInfo.GetAttribute("Type");
            this.GetAltitude = XmlConvert.ToInt16(xmlInfo.GetAttribute("Altitude"));
            this.AltitudeColor = Color.FromArgb(XmlConvert.ToByte(xmlInfo.GetAttribute("R")), XmlConvert.ToByte(xmlInfo.GetAttribute("G")), XmlConvert.ToByte(xmlInfo.GetAttribute("B")));
        }

        public void Save(XmlTextWriter xmlInfo)
        {
            xmlInfo.WriteStartElement("Altitude");
            xmlInfo.WriteAttributeString("Key", StringType.FromInteger(this.Key));
            xmlInfo.WriteAttributeString("Type", this.Type);
            xmlInfo.WriteAttributeString("Altitude", StringType.FromInteger(this.GetAltitude));
            xmlInfo.WriteAttributeString("R", StringType.FromByte(this.AltitudeColor.R));
            xmlInfo.WriteAttributeString("G", StringType.FromByte(this.AltitudeColor.G));
            xmlInfo.WriteAttributeString("B", StringType.FromByte(this.AltitudeColor.B));
            xmlInfo.WriteEndElement();
        }

        public void SaveACO(BinaryWriter iACTFile)
        {
            byte num = 0;
            iACTFile.Write(this.AltitudeColor.R);
            iACTFile.Write(this.AltitudeColor.R);
            iACTFile.Write(this.AltitudeColor.G);
            iACTFile.Write(this.AltitudeColor.G);
            iACTFile.Write(this.AltitudeColor.B);
            iACTFile.Write(this.AltitudeColor.B);
            iACTFile.Write(num);
            iACTFile.Write(num);
        }

        public void SaveACOText(BinaryWriter iACTFile)
        {
            byte num = 0;
            iACTFile.Write(this.AltitudeColor.R);
            iACTFile.Write(this.AltitudeColor.R);
            iACTFile.Write(this.AltitudeColor.G);
            iACTFile.Write(this.AltitudeColor.G);
            iACTFile.Write(this.AltitudeColor.B);
            iACTFile.Write(this.AltitudeColor.B);
            iACTFile.Write(num);
            iACTFile.Write(num);
            iACTFile.Write(num);
            iACTFile.Write(num);
            UnicodeEncoding unicodeEncoding = new(true, true);
            string str = string.Format("{0} {1}", this.Type, this.GetAltitude);
            byte[] bytes = unicodeEncoding.GetBytes(str);
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
            iACTFile.Write(this.AltitudeColor.R);
            iACTFile.Write(this.AltitudeColor.G);
            iACTFile.Write(this.AltitudeColor.B);
        }

        public override string ToString()
        {
            string str = string.Format("[{0:X3}] {1} {2}", this.Key, this.Type, this.GetAltitude);
            return str;
        }
    }
}