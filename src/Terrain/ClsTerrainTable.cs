using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Terrain
{
    public class ClsTerrainTable
    {
        public Hashtable TerrainHash { get; }

        public ClsTerrain TerrianGroup(int iKey)
        {
            return (ClsTerrain)this.TerrainHash[iKey];
        }

        public ClsTerrainTable()
        {
            this.TerrainHash = new Hashtable();
        }

        public void Display(ListBox iList)
        {
            IEnumerator enumerator = null;
            iList.Items.Clear();
            try
            {
                enumerator = this.TerrainHash.Values.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    ClsTerrain current = (ClsTerrain)enumerator.Current;
                    iList.Items.Add(current);
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    ((IDisposable)enumerator).Dispose();
                }
            }
        }

        public void Display(ComboBox iCombo)
        {
            IEnumerator enumerator = null;
            iCombo.Items.Clear();
            try
            {
                enumerator = this.TerrainHash.Values.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    ClsTerrain current = (ClsTerrain)enumerator.Current;
                    iCombo.Items.Add(current);
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    ((IDisposable)enumerator).Dispose();
                }
            }
        }

        public ColorPalette GetPalette()
        {
            ColorPalette palette = (new Bitmap(2, 2, PixelFormat.Format8bppIndexed)).Palette;
            byte num = 0;
            do
            {
                if (this.TerrianGroup(num) == null)
                {
                    palette.Entries[num] = Color.Black;
                }
                else
                {
                    palette.Entries[num] = this.TerrianGroup(num).Colour;
                }
                num = checked((byte)(num + 1));
            }
            while (num <= 254);
            palette.Entries[255] = this.TerrianGroup(255).Colour;
            return palette;
        }

        public void Load()
        {
            IEnumerator enumerator = null;
            IEnumerator enumerator1 = null;
            string str = string.Format("Data/System/Terrain.xml", Directory.GetCurrentDirectory());
            XmlDocument xmlDocument = new();
            try
            {
                xmlDocument.Load(str);
                this.TerrainHash.Clear();
                try
                {
                    enumerator1 = xmlDocument.SelectNodes("Terrains").GetEnumerator();
                    while (enumerator1.MoveNext())
                    {
                        XmlElement current = (XmlElement)enumerator1.Current;
                        try
                        {
                            enumerator = current.SelectNodes("Terrain").GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                ClsTerrain clsTerrain = new((XmlElement)enumerator.Current);
                                this.TerrainHash.Add(clsTerrain.GroupID, clsTerrain);
                            }
                        }
                        finally
                        {
                            if (enumerator is IDisposable)
                            {
                                ((IDisposable)enumerator).Dispose();
                            }
                        }
                    }
                }
                finally
                {
                    if (enumerator1 is IDisposable)
                    {
                        ((IDisposable)enumerator1).Dispose();
                    }
                }
            }
            catch (Exception exception)
            {
                ProjectData.SetProjectError(exception);
                Interaction.MsgBox(exception.Message, MsgBoxStyle.OkOnly, null);
                Interaction.MsgBox(string.Format("XMLFile:{0}", str), MsgBoxStyle.OkOnly, null);
                ProjectData.ClearProjectError();
            }
        }

        public void Save()
        {
            IEnumerator enumerator = null;
            string str = string.Format("Data/System/Terrain.xml", Directory.GetCurrentDirectory());
            XmlTextWriter xmlTextWriter = new(str, Encoding.UTF8)
            {
                Indentation = 2,
                Formatting = Formatting.Indented
            };
            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement("Terrains");
            try
            {
                enumerator = this.TerrainHash.Values.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    ((ClsTerrain)enumerator.Current).Save(xmlTextWriter);
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    ((IDisposable)enumerator).Dispose();
                }
            }
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndDocument();
            xmlTextWriter.Close();
        }

        public void SaveACO()
        {
            byte num = Convert.ToByte(this.TerrainHash.Count);
            string str = string.Format("Data/Photoshop/Terrain.ACO", Directory.GetCurrentDirectory());
            FileStream fileStream = new(str, FileMode.Create);
            BinaryWriter binaryWriter = new(fileStream);
            binaryWriter.Write((byte)0);
            binaryWriter.Write((byte)1);
            binaryWriter.Write((byte)0);
            binaryWriter.Write(num);
            int num1 = 0;
            do
            {
                if (this.TerrainHash[num1] != null)
                {
                    binaryWriter.Write((byte)0);
                    binaryWriter.Write((byte)0);
                    ((ClsTerrain)this.TerrainHash[num1]).SaveACO(binaryWriter);
                }
                num1++;
            }
            while (num1 <= 255);
            binaryWriter.Write((byte)0);
            binaryWriter.Write((byte)2);
            binaryWriter.Write((byte)0);
            binaryWriter.Write(num);
            int num2 = 0;
            do
            {
                if (this.TerrainHash[num2] != null)
                {
                    binaryWriter.Write((byte)0);
                    binaryWriter.Write((byte)0);
                    ((ClsTerrain)this.TerrainHash[num2]).SaveACOText(binaryWriter);
                }
                num2++;
            }
            while (num2 <= 255);
            binaryWriter.Close();
            fileStream.Close();
            Interaction.MsgBox("Terrain.ACO Saved", MsgBoxStyle.OkOnly, null);
        }

        public void SaveACT()
        {
            string str = string.Format("Data/Photoshop/Terrain.ACT", Directory.GetCurrentDirectory());
            FileStream fileStream = new(str, FileMode.Create);
            BinaryWriter binaryWriter = new(fileStream);
            byte num = 0;
            int num1 = 0;
            do
            {
                if (this.TerrainHash[num1] != null)
                {
                    ((ClsTerrain)this.TerrainHash[num1]).SaveACT(binaryWriter);
                }
                else
                {
                    binaryWriter.Write(num);
                    binaryWriter.Write(num);
                    binaryWriter.Write(num);
                }
                num1++;
            }
            while (num1 <= 255);
            binaryWriter.Close();
            fileStream.Close();
            Interaction.MsgBox("Terrain.ACT Saved", MsgBoxStyle.OkOnly, null);
        }
    }
}