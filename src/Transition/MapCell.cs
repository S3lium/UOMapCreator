using System;
using System.IO;
namespace Transition
{
    public class MapCell
    {
        public byte GroupID { get; }
        public short TileID { get; set; }
        public short AltID { get; set; }
        public void ChangeAltID(short AltMOD)
        {
            this.AltID += AltMOD;
        }
        public MapCell(byte i_Terrian, short i_Alt)
        {
            this.GroupID = i_Terrian;
            this.TileID = 0;
            this.AltID = i_Alt;
        }
        public void WriteMapMul(BinaryWriter i_MapFile)
        {
            i_MapFile.Write(this.TileID);
            if (this.AltID <= -127)
            {
                this.AltID = -127;
            }
            if (this.AltID >= 127)
            {
                this.AltID = 127;
            }
            sbyte b = Convert.ToSByte(this.AltID);
            i_MapFile.Write(b);
        }
    }
}
