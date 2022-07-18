using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace Ultima
{
    public sealed class Multis
    {
        private static MultiComponentList[] m_Components = new MultiComponentList[0x2000];
        private static FileIndex m_FileIndex = new("Multi.idx", "Multi.mul", 0x2000, 14);

        public enum ImportType
        {
            TXT,
            UOA,
            UOAB,
            WSC,
            MULTICACHE,
            UOADESIGN
        }

        /// <summary>
        /// ReReads multi.mul
        /// </summary>
        public static void Reload()
        {
            m_FileIndex = new FileIndex("Multi.idx", "Multi.mul", 0x2000, 14);
            m_Components = new MultiComponentList[0x2000];
        }

        /// <summary>
        /// Gets <see cref="MultiComponentList"/> of multi
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static MultiComponentList GetComponents(int index)
        {
            MultiComponentList mcl;

            index &= 0x1FFF;

            if (index >= 0 && index < m_Components.Length)
            {
                mcl = m_Components[index];

                if (mcl == null)
                    m_Components[index] = mcl = Load(index);
            }
            else
                mcl = MultiComponentList.Empty;

            return mcl;
        }

        public static MultiComponentList Load(int index)
        {
            try
            {
                int length, extra;
                bool patched;
                Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);

                if (stream == null)
                    return MultiComponentList.Empty;

                if (Art.IsUOAHS())
                    return new MultiComponentList(new BinaryReader(stream), length / 16);
                else
                    return new MultiComponentList(new BinaryReader(stream), length / 12);
            }
            catch
            {
                return MultiComponentList.Empty;
            }
        }

        public static void Remove(int index)
        {
            m_Components[index] = MultiComponentList.Empty;
        }

        public static void Add(int index, MultiComponentList comp)
        {
            m_Components[index] = comp;
        }

        public static MultiComponentList ImportFromFile(int index, string FileName, Multis.ImportType type)
        {
            try
            {
                return m_Components[index] = new MultiComponentList(FileName, type);
            }
            catch
            {
                return m_Components[index] = MultiComponentList.Empty;
            }
        }

        public static MultiComponentList LoadFromFile(string FileName, Multis.ImportType type)
        {
            try
            {
                return new MultiComponentList(FileName, type);
            }
            catch
            {
                return MultiComponentList.Empty;
            }
        }

        public static List<MultiComponentList> LoadFromCache(string FileName)
        {
            List<MultiComponentList> multilist = new();
            using (StreamReader ip = new(FileName))
            {
                string line;
                while ((line = ip.ReadLine()) != null)
                {
                    string[] split = Regex.Split(line, @"\s+");
                    if (split.Length == 7)
                    {
                        int count = Convert.ToInt32(split[2]);
                        multilist.Add(new MultiComponentList(ip, count));
                    }
                }
            }
            return multilist;
        }

        public static string ReadUOAString(BinaryReader bin)
        {
            byte flag = bin.ReadByte();

            if (flag == 0)
                return null;
            else
                return bin.ReadString();
        }
        public static List<Object[]> LoadFromDesigner(string FileName)
        {
            List<Object[]> multilist = new();
            string root = Path.GetFileNameWithoutExtension(FileName);
            string idx = String.Format("{0}.idx", root);
            string bin = String.Format("{0}.bin", root);
            if ((!File.Exists(idx)) || (!File.Exists(bin)))
                return multilist;
            using FileStream idxfs = new(idx, FileMode.Open, FileAccess.Read, FileShare.Read),
                              binfs = new(bin, FileMode.Open, FileAccess.Read, FileShare.Read);
            using (BinaryReader idxbin = new(idxfs),
                                binbin = new(binfs))
            {
                int count = idxbin.ReadInt32();
                int version = idxbin.ReadInt32();

                for (int i = 0; i < count; ++i)
                {
                    Object[] data = new Object[2];
                    switch (version)
                    {
                        case 0:
                            data[0] = ReadUOAString(idxbin);
                            List<MultiComponentList.MultiTileEntry> arr = new();
                            data[0] += "-" + ReadUOAString(idxbin);
                            data[0] += "-" + ReadUOAString(idxbin);
                            int width = idxbin.ReadInt32();
                            int height = idxbin.ReadInt32();
                            int uwidth = idxbin.ReadInt32();
                            int uheight = idxbin.ReadInt32();
                            long filepos = idxbin.ReadInt64();
                            int reccount = idxbin.ReadInt32();

                            binbin.BaseStream.Seek(filepos, SeekOrigin.Begin);
                            int index, x, y, z, level, hue;
                            for (int j = 0; j < reccount; ++j)
                            {
                                index = x = y = z = level = hue = 0;
                                int compVersion = binbin.ReadInt32();
                                switch (compVersion)
                                {
                                    case 0:
                                        index = binbin.ReadInt32();
                                        x = binbin.ReadInt32();
                                        y = binbin.ReadInt32();
                                        z = binbin.ReadInt32();
                                        level = binbin.ReadInt32();
                                        break;

                                    case 1:
                                        index = binbin.ReadInt32();
                                        x = binbin.ReadInt32();
                                        y = binbin.ReadInt32();
                                        z = binbin.ReadInt32();
                                        level = binbin.ReadInt32();
                                        hue = binbin.ReadInt32();
                                        break;
                                }
                                MultiComponentList.MultiTileEntry tempitem = new();
                                tempitem.m_ItemID = (ushort)index;
                                tempitem.m_Flags = 1;
                                tempitem.m_OffsetX = (short)x;
                                tempitem.m_OffsetY = (short)y;
                                tempitem.m_OffsetZ = (short)z;
                                tempitem.m_Unk1 = 0;
                                arr.Add(tempitem);

                            }
                            data[1] = new MultiComponentList(arr);
                            break;

                    }
                    multilist.Add(data);
                }
            }
            return multilist;
        }

        public static List<MultiComponentList.MultiTileEntry> RebuildTiles(MultiComponentList.MultiTileEntry[] tiles)
        {
            List<MultiComponentList.MultiTileEntry> newtiles = new();
            newtiles.AddRange(tiles);

            if (newtiles[0].m_OffsetX == 0 && newtiles[0].m_OffsetY == 0 && newtiles[0].m_OffsetZ == 0) // found a centeritem
            {
                if (newtiles[0].m_ItemID != 0x1) // its a "good" one
                {
                    for (int j = newtiles.Count - 1; j >= 0; --j) // remove all invis items
                    {
                        if (newtiles[j].m_ItemID == 0x1)
                            newtiles.RemoveAt(j);
                    }
                    return newtiles;
                }
                else // a bad one
                {
                    for (int i = 1; i < newtiles.Count; ++i) // do we have a better one?
                    {
                        if (newtiles[i].m_OffsetX == 0 && newtiles[i].m_OffsetY == 0
                            && newtiles[i].m_ItemID != 0x1 && newtiles[i].m_OffsetZ == 0)
                        {
                            MultiComponentList.MultiTileEntry centeritem = newtiles[i];
                            newtiles.RemoveAt(i); // jep so save it
                            for (int j = newtiles.Count - 1; j >= 0; --j) // and remove all invis
                            {
                                if (newtiles[j].m_ItemID == 0x1)
                                    newtiles.RemoveAt(j);
                            }
                            newtiles.Insert(0, centeritem);
                            return newtiles;
                        }
                    }
                    for (int j = newtiles.Count - 1; j >= 1; --j) // nothing found so remove all invis exept the first
                    {
                        if (newtiles[j].m_ItemID == 0x1)
                            newtiles.RemoveAt(j);
                    }
                    return newtiles;
                }
            }
            for (int i = 0; i < newtiles.Count; ++i) // is there a good one
            {
                if (newtiles[i].m_OffsetX == 0 && newtiles[i].m_OffsetY == 0
                    && newtiles[i].m_ItemID != 0x1 && newtiles[i].m_OffsetZ == 0)
                {
                    MultiComponentList.MultiTileEntry centeritem = newtiles[i];
                    newtiles.RemoveAt(i); // store it
                    for (int j = newtiles.Count - 1; j >= 0; --j) // remove all invis
                    {
                        if (newtiles[j].m_ItemID == 0x1)
                            newtiles.RemoveAt(j);
                    }
                    newtiles.Insert(0, centeritem);
                    return newtiles;
                }
            }
            for (int j = newtiles.Count - 1; j >= 0; --j) // nothing found so remove all invis
            {
                if (newtiles[j].m_ItemID == 0x1)
                    newtiles.RemoveAt(j);
            }
            MultiComponentList.MultiTileEntry invisitem = new();
            invisitem.m_ItemID = 0x1; // and create a new invis
            invisitem.m_OffsetX = 0;
            invisitem.m_OffsetY = 0;
            invisitem.m_OffsetZ = 0;
            invisitem.m_Flags = 0;
            invisitem.m_Unk1 = 0;
            newtiles.Insert(0, invisitem);
            return newtiles;
        }

        public static void Save(string path)
        {
            bool isUOAHS = Art.IsUOAHS();
            string idx = Path.Combine(path, "multi.idx");
            string mul = Path.Combine(path, "multi.mul");
            using FileStream fsidx = new(idx, FileMode.Create, FileAccess.Write, FileShare.Write),
                              fsmul = new(mul, FileMode.Create, FileAccess.Write, FileShare.Write);
            using BinaryWriter binidx = new(fsidx),
                                binmul = new(fsmul);
            for (int index = 0; index < 0x2000; ++index)
            {
                MultiComponentList comp = GetComponents(index);

                if (comp == MultiComponentList.Empty)
                {
                    binidx.Write(-1); // lookup
                    binidx.Write(-1); // length
                    binidx.Write(-1); // extra
                }
                else
                {
                    List<MultiComponentList.MultiTileEntry> tiles = RebuildTiles(comp.SortedTiles);
                    binidx.Write((int)fsmul.Position); //lookup
                    if (isUOAHS)
                        binidx.Write(tiles.Count * 16); //length
                    else
                        binidx.Write(tiles.Count * 12); //length
                    binidx.Write(-1); //extra
                    for (int i = 0; i < tiles.Count; ++i)
                    {
                        binmul.Write(tiles[i].m_ItemID);
                        binmul.Write(tiles[i].m_OffsetX);
                        binmul.Write(tiles[i].m_OffsetY);
                        binmul.Write(tiles[i].m_OffsetZ);
                        binmul.Write(tiles[i].m_Flags);
                        if (isUOAHS)
                            binmul.Write(tiles[i].m_Unk1);

                    }
                }
            }
        }
    }

    public sealed class MultiComponentList
    {
        private Point m_Min, m_Max, m_Center;
        public static readonly MultiComponentList Empty = new();

        public Point Min => m_Min;
        public Point Max => m_Max;
        public Point Center => m_Center;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public MTile[][][] Tiles { get; private set; }
        public int maxHeight { get; }
        public MultiTileEntry[] SortedTiles { get; }
        public int Surface { get; private set; }


        public struct MultiTileEntry
        {
            public ushort m_ItemID;
            public short m_OffsetX, m_OffsetY, m_OffsetZ;
            public int m_Flags;
            public int m_Unk1;
        }

        /// <summary>
        /// Returns Bitmap of Multi
        /// </summary>
        /// <returns></returns>
        public Bitmap GetImage()
        {
            return GetImage(300);
        }

        /// <summary>
        /// Returns Bitmap of Multi to maxheight
        /// </summary>
        /// <param name="maxheight"></param>
        /// <returns></returns>
        public Bitmap GetImage(int maxheight)
        {
            if (Width == 0 || Height == 0)
                return null;

            int xMin = 1000, yMin = 1000;
            int xMax = -1000, yMax = -1000;

            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    MTile[] tiles = Tiles[x][y];

                    for (int i = 0; i < tiles.Length; ++i)
                    {
                        Bitmap bmp = Art.GetStatic(tiles[i].ID);

                        if (bmp == null)
                            continue;

                        int px = (x - y) * 22;
                        int py = (x + y) * 22;

                        px -= (bmp.Width / 2);
                        py -= tiles[i].Z << 2;
                        py -= bmp.Height;

                        if (px < xMin)
                            xMin = px;

                        if (py < yMin)
                            yMin = py;

                        px += bmp.Width;
                        py += bmp.Height;

                        if (px > xMax)
                            xMax = px;

                        if (py > yMax)
                            yMax = py;
                    }
                }
            }

            Bitmap canvas = new(xMax - xMin, yMax - yMin);
            Graphics gfx = Graphics.FromImage(canvas);
            gfx.Clear(Color.White);
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    MTile[] tiles = Tiles[x][y];

                    for (int i = 0; i < tiles.Length; ++i)
                    {

                        Bitmap bmp = Art.GetStatic(tiles[i].ID);

                        if (bmp == null)
                            continue;
                        if ((tiles[i].Z) > maxheight)
                            continue;
                        int px = (x - y) * 22;
                        int py = (x + y) * 22;

                        px -= (bmp.Width / 2);
                        py -= tiles[i].Z << 2;
                        py -= bmp.Height;
                        px -= xMin;
                        py -= yMin;

                        gfx.DrawImageUnscaled(bmp, px, py, bmp.Width, bmp.Height);
                    }

                    int tx = (x - y) * 22;
                    int ty = (x + y) * 22;
                    tx -= xMin;
                    ty -= yMin;
                }
            }

            gfx.Dispose();

            return canvas;
        }

        public MultiComponentList(BinaryReader reader, int count)
        {
            bool useNewMultiFormat = Art.IsUOAHS();
            m_Min = m_Max = Point.Empty;
            SortedTiles = new MultiTileEntry[count];
            for (int i = 0; i < count; ++i)
            {
                SortedTiles[i].m_ItemID = Art.GetLegalItemID(reader.ReadUInt16());
                SortedTiles[i].m_OffsetX = reader.ReadInt16();
                SortedTiles[i].m_OffsetY = reader.ReadInt16();
                SortedTiles[i].m_OffsetZ = reader.ReadInt16();
                SortedTiles[i].m_Flags = reader.ReadInt32();
                if (useNewMultiFormat)
                    SortedTiles[i].m_Unk1 = reader.ReadInt32();
                else
                    SortedTiles[i].m_Unk1 = 0;

                MultiTileEntry e = SortedTiles[i];

                if (e.m_OffsetX < m_Min.X)
                    m_Min.X = e.m_OffsetX;

                if (e.m_OffsetY < m_Min.Y)
                    m_Min.Y = e.m_OffsetY;

                if (e.m_OffsetX > m_Max.X)
                    m_Max.X = e.m_OffsetX;

                if (e.m_OffsetY > m_Max.Y)
                    m_Max.Y = e.m_OffsetY;

                if (e.m_OffsetZ > maxHeight)
                    maxHeight = e.m_OffsetZ;
            }
            ConvertList();
            reader.Close();
        }

        public MultiComponentList(string FileName, Multis.ImportType Type)
        {
            m_Min = m_Max = Point.Empty;
            int itemcount;
            switch (Type)
            {
                case Multis.ImportType.TXT:
                    itemcount = 0;
                    using (StreamReader ip = new(FileName))
                    {
                        string line;
                        while ((line = ip.ReadLine()) != null)
                        {
                            itemcount++;
                        }
                    }
                    SortedTiles = new MultiTileEntry[itemcount];
                    itemcount = 0;
                    m_Min.X = 10000;
                    m_Min.Y = 10000;
                    using (StreamReader ip = new(FileName))
                    {
                        string line;
                        while ((line = ip.ReadLine()) != null)
                        {
                            string[] split = line.Split(' ');

                            string tmp = split[0];
                            tmp = tmp.Replace("0x", "");

                            SortedTiles[itemcount].m_ItemID = ushort.Parse(tmp, System.Globalization.NumberStyles.HexNumber);
                            SortedTiles[itemcount].m_OffsetX = Convert.ToInt16(split[1]);
                            SortedTiles[itemcount].m_OffsetY = Convert.ToInt16(split[2]);
                            SortedTiles[itemcount].m_OffsetZ = Convert.ToInt16(split[3]);
                            SortedTiles[itemcount].m_Flags = Convert.ToInt32(split[4]);
                            SortedTiles[itemcount].m_Unk1 = 0;

                            MultiTileEntry e = SortedTiles[itemcount];

                            if (e.m_OffsetX < m_Min.X)
                                m_Min.X = e.m_OffsetX;

                            if (e.m_OffsetY < m_Min.Y)
                                m_Min.Y = e.m_OffsetY;

                            if (e.m_OffsetX > m_Max.X)
                                m_Max.X = e.m_OffsetX;

                            if (e.m_OffsetY > m_Max.Y)
                                m_Max.Y = e.m_OffsetY;

                            if (e.m_OffsetZ > maxHeight)
                                maxHeight = e.m_OffsetZ;

                            itemcount++;
                        }
                        int centerx = m_Max.X - (int)(Math.Round((m_Max.X - m_Min.X) / 2.0));
                        int centery = m_Max.Y - (int)(Math.Round((m_Max.Y - m_Min.Y) / 2.0));

                        m_Min = m_Max = Point.Empty;
                        int i = 0;
                        for (; i < SortedTiles.Length; i++)
                        {
                            SortedTiles[i].m_OffsetX -= (short)centerx;
                            SortedTiles[i].m_OffsetY -= (short)centery;
                            if (SortedTiles[i].m_OffsetX < m_Min.X)
                                m_Min.X = SortedTiles[i].m_OffsetX;
                            if (SortedTiles[i].m_OffsetX > m_Max.X)
                                m_Max.X = SortedTiles[i].m_OffsetX;

                            if (SortedTiles[i].m_OffsetY < m_Min.Y)
                                m_Min.Y = SortedTiles[i].m_OffsetY;
                            if (SortedTiles[i].m_OffsetY > m_Max.Y)
                                m_Max.Y = SortedTiles[i].m_OffsetY;
                        }
                    }
                    break;
                case Multis.ImportType.UOA:
                    itemcount = 0;

                    using (StreamReader ip = new(FileName))
                    {
                        string line;
                        while ((line = ip.ReadLine()) != null)
                        {
                            ++itemcount;
                            if (itemcount == 4)
                            {
                                string[] split = line.Split(' ');
                                itemcount = Convert.ToInt32(split[0]);
                                break;
                            }
                        }
                    }
                    SortedTiles = new MultiTileEntry[itemcount];
                    itemcount = 0;
                    m_Min.X = 10000;
                    m_Min.Y = 10000;
                    using (StreamReader ip = new(FileName))
                    {
                        string line;
                        int i = -1;
                        while ((line = ip.ReadLine()) != null)
                        {
                            ++i;
                            if (i < 4)
                                continue;
                            string[] split = line.Split(' ');

                            SortedTiles[itemcount].m_ItemID = Convert.ToUInt16(split[0]);
                            SortedTiles[itemcount].m_OffsetX = Convert.ToInt16(split[1]);
                            SortedTiles[itemcount].m_OffsetY = Convert.ToInt16(split[2]);
                            SortedTiles[itemcount].m_OffsetZ = Convert.ToInt16(split[3]);
                            SortedTiles[itemcount].m_Flags = Convert.ToInt32(split[4]);
                            SortedTiles[itemcount].m_Unk1 = 0;

                            MultiTileEntry e = SortedTiles[itemcount];

                            if (e.m_OffsetX < m_Min.X)
                                m_Min.X = e.m_OffsetX;

                            if (e.m_OffsetY < m_Min.Y)
                                m_Min.Y = e.m_OffsetY;

                            if (e.m_OffsetX > m_Max.X)
                                m_Max.X = e.m_OffsetX;

                            if (e.m_OffsetY > m_Max.Y)
                                m_Max.Y = e.m_OffsetY;

                            if (e.m_OffsetZ > maxHeight)
                                maxHeight = e.m_OffsetZ;

                            ++itemcount;
                        }
                        int centerx = m_Max.X - (int)(Math.Round((m_Max.X - m_Min.X) / 2.0));
                        int centery = m_Max.Y - (int)(Math.Round((m_Max.Y - m_Min.Y) / 2.0));

                        m_Min = m_Max = Point.Empty;
                        i = 0;
                        for (; i < SortedTiles.Length; ++i)
                        {
                            SortedTiles[i].m_OffsetX -= (short)centerx;
                            SortedTiles[i].m_OffsetY -= (short)centery;
                            if (SortedTiles[i].m_OffsetX < m_Min.X)
                                m_Min.X = SortedTiles[i].m_OffsetX;
                            if (SortedTiles[i].m_OffsetX > m_Max.X)
                                m_Max.X = SortedTiles[i].m_OffsetX;

                            if (SortedTiles[i].m_OffsetY < m_Min.Y)
                                m_Min.Y = SortedTiles[i].m_OffsetY;
                            if (SortedTiles[i].m_OffsetY > m_Max.Y)
                                m_Max.Y = SortedTiles[i].m_OffsetY;
                        }
                    }

                    break;
                case Multis.ImportType.UOAB:
                    using (FileStream fs = new(FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (BinaryReader reader = new(fs))
                    {
                        if (reader.ReadInt16() != 1) //Version check
                            return;
                        string tmp;
                        tmp = Multis.ReadUOAString(reader); //Name
                        tmp = Multis.ReadUOAString(reader); //Category
                        tmp = Multis.ReadUOAString(reader); //Subsection
                        int width = reader.ReadInt32();
                        int height = reader.ReadInt32();
                        int uwidth = reader.ReadInt32();
                        int uheight = reader.ReadInt32();

                        int count = reader.ReadInt32();
                        itemcount = count;
                        SortedTiles = new MultiTileEntry[itemcount];
                        itemcount = 0;
                        m_Min.X = 10000;
                        m_Min.Y = 10000;
                        for (; itemcount < count; ++itemcount)
                        {
                            SortedTiles[itemcount].m_ItemID = (ushort)reader.ReadInt16();
                            SortedTiles[itemcount].m_OffsetX = reader.ReadInt16();
                            SortedTiles[itemcount].m_OffsetY = reader.ReadInt16();
                            SortedTiles[itemcount].m_OffsetZ = reader.ReadInt16();
                            reader.ReadInt16(); // level
                            SortedTiles[itemcount].m_Flags = 1;
                            reader.ReadInt16(); // hue
                            SortedTiles[itemcount].m_Unk1 = 0;

                            MultiTileEntry e = SortedTiles[itemcount];

                            if (e.m_OffsetX < m_Min.X)
                                m_Min.X = e.m_OffsetX;

                            if (e.m_OffsetY < m_Min.Y)
                                m_Min.Y = e.m_OffsetY;

                            if (e.m_OffsetX > m_Max.X)
                                m_Max.X = e.m_OffsetX;

                            if (e.m_OffsetY > m_Max.Y)
                                m_Max.Y = e.m_OffsetY;

                            if (e.m_OffsetZ > maxHeight)
                                maxHeight = e.m_OffsetZ;
                        }
                        int centerx = m_Max.X - (int)(Math.Round((m_Max.X - m_Min.X) / 2.0));
                        int centery = m_Max.Y - (int)(Math.Round((m_Max.Y - m_Min.Y) / 2.0));

                        m_Min = m_Max = Point.Empty;
                        itemcount = 0;
                        for (; itemcount < SortedTiles.Length; ++itemcount)
                        {
                            SortedTiles[itemcount].m_OffsetX -= (short)centerx;
                            SortedTiles[itemcount].m_OffsetY -= (short)centery;
                            if (SortedTiles[itemcount].m_OffsetX < m_Min.X)
                                m_Min.X = SortedTiles[itemcount].m_OffsetX;
                            if (SortedTiles[itemcount].m_OffsetX > m_Max.X)
                                m_Max.X = SortedTiles[itemcount].m_OffsetX;

                            if (SortedTiles[itemcount].m_OffsetY < m_Min.Y)
                                m_Min.Y = SortedTiles[itemcount].m_OffsetY;
                            if (SortedTiles[itemcount].m_OffsetY > m_Max.Y)
                                m_Max.Y = SortedTiles[itemcount].m_OffsetY;
                        }
                    }
                    break;

                case Multis.ImportType.WSC:
                    itemcount = 0;
                    using (StreamReader ip = new(FileName))
                    {
                        string line;
                        while ((line = ip.ReadLine()) != null)
                        {
                            line = line.Trim();
                            if (line.StartsWith("SECTION WORLDITEM"))
                                ++itemcount;
                        }
                    }
                    SortedTiles = new MultiTileEntry[itemcount];
                    itemcount = 0;
                    m_Min.X = 10000;
                    m_Min.Y = 10000;
                    using (StreamReader ip = new(FileName))
                    {
                        string line;
                        MultiTileEntry tempitem = new();
                        tempitem.m_ItemID = 0xFFFF;
                        tempitem.m_Flags = 1;
                        tempitem.m_Unk1 = 0;
                        while ((line = ip.ReadLine()) != null)
                        {
                            line = line.Trim();
                            if (line.StartsWith("SECTION WORLDITEM"))
                            {
                                if (tempitem.m_ItemID != 0xFFFF)
                                {
                                    SortedTiles[itemcount] = tempitem;
                                    ++itemcount;
                                }
                                tempitem.m_ItemID = 0xFFFF;
                            }
                            else if (line.StartsWith("ID"))
                            {
                                line = line.Remove(0, 2);
                                line = line.Trim();
                                tempitem.m_ItemID = Convert.ToUInt16(line);
                            }
                            else if (line.StartsWith("X"))
                            {
                                line = line.Remove(0, 1);
                                line = line.Trim();
                                tempitem.m_OffsetX = Convert.ToInt16(line);
                                if (tempitem.m_OffsetX < m_Min.X)
                                    m_Min.X = tempitem.m_OffsetX;
                                if (tempitem.m_OffsetX > m_Max.X)
                                    m_Max.X = tempitem.m_OffsetX;
                            }
                            else if (line.StartsWith("Y"))
                            {
                                line = line.Remove(0, 1);
                                line = line.Trim();
                                tempitem.m_OffsetY = Convert.ToInt16(line);
                                if (tempitem.m_OffsetY < m_Min.Y)
                                    m_Min.Y = tempitem.m_OffsetY;
                                if (tempitem.m_OffsetY > m_Max.Y)
                                    m_Max.Y = tempitem.m_OffsetY;
                            }
                            else if (line.StartsWith("Z"))
                            {
                                line = line.Remove(0, 1);
                                line = line.Trim();
                                tempitem.m_OffsetZ = Convert.ToInt16(line);
                                if (tempitem.m_OffsetZ > maxHeight)
                                    maxHeight = tempitem.m_OffsetZ;

                            }
                        }
                        if (tempitem.m_ItemID != 0xFFFF)
                            SortedTiles[itemcount] = tempitem;

                        int centerx = m_Max.X - (int)(Math.Round((m_Max.X - m_Min.X) / 2.0));
                        int centery = m_Max.Y - (int)(Math.Round((m_Max.Y - m_Min.Y) / 2.0));

                        m_Min = m_Max = Point.Empty;
                        int i = 0;
                        for (; i < SortedTiles.Length; i++)
                        {
                            SortedTiles[i].m_OffsetX -= (short)centerx;
                            SortedTiles[i].m_OffsetY -= (short)centery;
                            if (SortedTiles[i].m_OffsetX < m_Min.X)
                                m_Min.X = SortedTiles[i].m_OffsetX;
                            if (SortedTiles[i].m_OffsetX > m_Max.X)
                                m_Max.X = SortedTiles[i].m_OffsetX;

                            if (SortedTiles[i].m_OffsetY < m_Min.Y)
                                m_Min.Y = SortedTiles[i].m_OffsetY;
                            if (SortedTiles[i].m_OffsetY > m_Max.Y)
                                m_Max.Y = SortedTiles[i].m_OffsetY;
                        }
                    }
                    break;
            }
            ConvertList();
        }

        public MultiComponentList(List<MultiTileEntry> arr)
        {
            m_Min = m_Max = Point.Empty;
            int itemcount = arr.Count;
            SortedTiles = new MultiTileEntry[itemcount];
            m_Min.X = 10000;
            m_Min.Y = 10000;
            int i = 0;
            foreach (MultiTileEntry entry in arr)
            {
                if (entry.m_OffsetX < m_Min.X)
                    m_Min.X = entry.m_OffsetX;

                if (entry.m_OffsetY < m_Min.Y)
                    m_Min.Y = entry.m_OffsetY;

                if (entry.m_OffsetX > m_Max.X)
                    m_Max.X = entry.m_OffsetX;

                if (entry.m_OffsetY > m_Max.Y)
                    m_Max.Y = entry.m_OffsetY;

                if (entry.m_OffsetZ > maxHeight)
                    maxHeight = entry.m_OffsetZ;
                SortedTiles[i] = entry;

                ++i;
            }
            arr.Clear();
            int centerx = m_Max.X - (int)(Math.Round((m_Max.X - m_Min.X) / 2.0));
            int centery = m_Max.Y - (int)(Math.Round((m_Max.Y - m_Min.Y) / 2.0));

            m_Min = m_Max = Point.Empty;
            for (i = 0; i < SortedTiles.Length; ++i)
            {
                SortedTiles[i].m_OffsetX -= (short)centerx;
                SortedTiles[i].m_OffsetY -= (short)centery;
                if (SortedTiles[i].m_OffsetX < m_Min.X)
                    m_Min.X = SortedTiles[i].m_OffsetX;
                if (SortedTiles[i].m_OffsetX > m_Max.X)
                    m_Max.X = SortedTiles[i].m_OffsetX;

                if (SortedTiles[i].m_OffsetY < m_Min.Y)
                    m_Min.Y = SortedTiles[i].m_OffsetY;
                if (SortedTiles[i].m_OffsetY > m_Max.Y)
                    m_Max.Y = SortedTiles[i].m_OffsetY;
            }
            ConvertList();
        }

        public MultiComponentList(StreamReader stream, int count)
        {
            string line;
            int itemcount = 0;
            m_Min = m_Max = Point.Empty;
            SortedTiles = new MultiTileEntry[count];
            m_Min.X = 10000;
            m_Min.Y = 10000;

            while ((line = stream.ReadLine()) != null)
            {
                string[] split = Regex.Split(line, @"\s+");
                SortedTiles[itemcount].m_ItemID = Convert.ToUInt16(split[0]);
                SortedTiles[itemcount].m_Flags = Convert.ToInt32(split[1]);
                SortedTiles[itemcount].m_OffsetX = Convert.ToInt16(split[2]);
                SortedTiles[itemcount].m_OffsetY = Convert.ToInt16(split[3]);
                SortedTiles[itemcount].m_OffsetZ = Convert.ToInt16(split[4]);
                SortedTiles[itemcount].m_Unk1 = 0;

                MultiTileEntry e = SortedTiles[itemcount];

                if (e.m_OffsetX < m_Min.X)
                    m_Min.X = e.m_OffsetX;
                if (e.m_OffsetY < m_Min.Y)
                    m_Min.Y = e.m_OffsetY;
                if (e.m_OffsetX > m_Max.X)
                    m_Max.X = e.m_OffsetX;
                if (e.m_OffsetY > m_Max.Y)
                    m_Max.Y = e.m_OffsetY;
                if (e.m_OffsetZ > maxHeight)
                    maxHeight = e.m_OffsetZ;

                ++itemcount;
                if (itemcount == count)
                    break;

            }
            int centerx = m_Max.X - (int)(Math.Round((m_Max.X - m_Min.X) / 2.0));
            int centery = m_Max.Y - (int)(Math.Round((m_Max.Y - m_Min.Y) / 2.0));

            m_Min = m_Max = Point.Empty;
            int i = 0;
            for (; i < SortedTiles.Length; i++)
            {
                SortedTiles[i].m_OffsetX -= (short)centerx;
                SortedTiles[i].m_OffsetY -= (short)centery;
                if (SortedTiles[i].m_OffsetX < m_Min.X)
                    m_Min.X = SortedTiles[i].m_OffsetX;
                if (SortedTiles[i].m_OffsetX > m_Max.X)
                    m_Max.X = SortedTiles[i].m_OffsetX;

                if (SortedTiles[i].m_OffsetY < m_Min.Y)
                    m_Min.Y = SortedTiles[i].m_OffsetY;
                if (SortedTiles[i].m_OffsetY > m_Max.Y)
                    m_Max.Y = SortedTiles[i].m_OffsetY;
            }
            ConvertList();
        }

        private void ConvertList()
        {
            m_Center = new Point(-m_Min.X, -m_Min.Y);
            Width = (m_Max.X - m_Min.X) + 1;
            Height = (m_Max.Y - m_Min.Y) + 1;

            MTileList[][] tiles = new MTileList[Width][];
            Tiles = new MTile[Width][][];

            for (int x = 0; x < Width; ++x)
            {
                tiles[x] = new MTileList[Height];
                Tiles[x] = new MTile[Height][];

                for (int y = 0; y < Height; ++y)
                    tiles[x][y] = new MTileList();
            }

            for (int i = 0; i < SortedTiles.Length; ++i)
            {
                int xOffset = SortedTiles[i].m_OffsetX + m_Center.X;
                int yOffset = SortedTiles[i].m_OffsetY + m_Center.Y;

                tiles[xOffset][yOffset].Add(SortedTiles[i].m_ItemID, (sbyte)SortedTiles[i].m_OffsetZ, (sbyte)SortedTiles[i].m_Flags, SortedTiles[i].m_Unk1);
            }

            Surface = 0;

            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    Tiles[x][y] = tiles[x][y].ToArray();
                    for (int i = 0; i < Tiles[x][y].Length; ++i)
                        Tiles[x][y][i].Solver = i;
                    if (Tiles[x][y].Length > 1)
                        Array.Sort(Tiles[x][y]);
                    if (Tiles[x][y].Length > 0)
                        ++Surface;
                }
            }
        }

        public MultiComponentList(MTileList[][] newtiles, int count, int width, int height)
        {
            m_Min = m_Max = Point.Empty;
            SortedTiles = new MultiTileEntry[count];
            m_Center = new Point((int)(Math.Round((width / 2.0))) - 1, (int)(Math.Round((height / 2.0))) - 1);
            if (m_Center.X < 0)
                m_Center.X = width / 2;
            if (m_Center.Y < 0)
                m_Center.Y = height / 2;
            maxHeight = -128;

            int counter = 0;
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    MTile[] tiles = newtiles[x][y].ToArray();
                    for (int i = 0; i < tiles.Length; ++i)
                    {
                        SortedTiles[counter].m_ItemID = tiles[i].ID;
                        SortedTiles[counter].m_OffsetX = (short)(x - m_Center.X);
                        SortedTiles[counter].m_OffsetY = (short)(y - m_Center.Y);
                        SortedTiles[counter].m_OffsetZ = (short)(tiles[i].Z);
                        SortedTiles[counter].m_Flags = tiles[i].Flag;
                        SortedTiles[counter].m_Unk1 = 0;

                        if (SortedTiles[counter].m_OffsetX < m_Min.X)
                            m_Min.X = SortedTiles[counter].m_OffsetX;
                        if (SortedTiles[counter].m_OffsetX > m_Max.X)
                            m_Max.X = SortedTiles[counter].m_OffsetX;
                        if (SortedTiles[counter].m_OffsetY < m_Min.Y)
                            m_Min.Y = SortedTiles[counter].m_OffsetY;
                        if (SortedTiles[counter].m_OffsetY > m_Max.Y)
                            m_Max.Y = SortedTiles[counter].m_OffsetY;
                        if (SortedTiles[counter].m_OffsetZ > maxHeight)
                            maxHeight = SortedTiles[counter].m_OffsetZ;
                        ++counter;
                    }
                }
            }
            ConvertList();
        }

        private MultiComponentList()
        {
            Tiles = new MTile[0][][];
        }

        public void ExportToTextFile(string FileName)
        {
            using StreamWriter Tex = new(new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite), System.Text.Encoding.GetEncoding(1252));
            for (int i = 0; i < SortedTiles.Length; ++i)
            {
                Tex.WriteLine(String.Format("0x{0:X} {1} {2} {3} {4}",
                            SortedTiles[i].m_ItemID,
                            SortedTiles[i].m_OffsetX,
                            SortedTiles[i].m_OffsetY,
                            SortedTiles[i].m_OffsetZ,
                            SortedTiles[i].m_Flags));
            }
        }

        public void ExportToWscFile(string FileName)
        {
            using StreamWriter Tex = new(new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite), System.Text.Encoding.GetEncoding(1252));
            for (int i = 0; i < SortedTiles.Length; ++i)
            {
                Tex.WriteLine(String.Format("SECTION WORLDITEM {0}", i));
                Tex.WriteLine("{");
                Tex.WriteLine(String.Format("\tID\t{0}", SortedTiles[i].m_ItemID));
                Tex.WriteLine(String.Format("\tX\t{0}", SortedTiles[i].m_OffsetX));
                Tex.WriteLine(String.Format("\tY\t{0}", SortedTiles[i].m_OffsetY));
                Tex.WriteLine(String.Format("\tZ\t{0}", SortedTiles[i].m_OffsetZ));
                Tex.WriteLine("\tColor\t0");
                Tex.WriteLine("}");

            }
        }

        public void ExportToUOAFile(string FileName)
        {
            using StreamWriter Tex = new(new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite), System.Text.Encoding.GetEncoding(1252));
            Tex.WriteLine("6 version");
            Tex.WriteLine("1 template id");
            Tex.WriteLine("-1 item version");
            Tex.WriteLine(String.Format("{0} num components", SortedTiles.Length));
            for (int i = 0; i < SortedTiles.Length; ++i)
            {
                Tex.WriteLine(String.Format("{0} {1} {2} {3} {4}",
                            SortedTiles[i].m_ItemID,
                            SortedTiles[i].m_OffsetX,
                            SortedTiles[i].m_OffsetY,
                            SortedTiles[i].m_OffsetZ,
                            SortedTiles[i].m_Flags));
            }
        }
    }
}