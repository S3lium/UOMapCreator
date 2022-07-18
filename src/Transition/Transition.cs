using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml;
using Terrain;
using Ultima;

namespace Transition
{
    public class Transition
    {
        private readonly RandomStatics m_RandomTiles;

        public string File { get; set; }
        public string Description { get; set; }
        public string HashKey
        {
            get => this.GetHaskKeyTable.ToString();
            set
            {
                byte b = 0;
                do
                {
                    this.GetHaskKeyTable.Add(new HashKey(Strings.Mid(value, checked(b * 2 + 1), 2)));
                    b += 1;
                }
                while (b <= 8);
            }
        }
        public MapTileCollection GetMapTiles { get; set; }
        public StaticTileCollection GetStaticTiles { get; set; }
        public HashKeyCollection GetHaskKeyTable { get; }
        public byte GetKey(int Index)
        {
            return this.GetHaskKeyTable[Index].Key;
        }
        public virtual MapTile GetRandomMapTile()
        {
            MapTile randomTile = null;
            if (this.GetMapTiles.Count > 0)
            {
                randomTile = this.GetMapTiles.RandomTile;
            }
            return randomTile;
        }
        public virtual void GetRandomStaticTiles(short X, short Y, short Z, Collection[,] StaticMap, bool iRandom)
        {
            if (this.GetStaticTiles.Count > 0)
            {
                StaticTile randomTile = this.GetStaticTiles.RandomTile;
                StaticMap[(short)(X >> 3), (short)(Y >> 3)].Add(new StaticCell(randomTile.TileID, checked((byte)(X % 8)), checked((byte)(Y % 8)), (short)(Z + randomTile.AltIDMod)), null, null, null);
            }
            if (iRandom)
            {
                if (this.m_RandomTiles != null)
                {
                    this.m_RandomTiles.GetRandomStatic(X, Y, Z, StaticMap);
                }
            }
        }
        public void Clone(ClsTerrain iGroupA, ClsTerrain iGroupB)
        {
            this.Description = this.Description.Replace(iGroupA.Name, iGroupB.Name);
            int num = 0;
            checked
            {
                do
                {
                    if (GetHaskKeyTable[num].Key == iGroupA.GroupID)
                    {
                        this.GetHaskKeyTable[num].Key = (byte)iGroupB.GroupID;
                    }
                    num++;
                }
                while (num <= 8);
            }
        }
        public void SetHashKey(int iKey, byte iKeyHash)
        {
            this.GetHaskKeyTable[iKey].Key = iKeyHash;
        }
        public void AddMapTile(short TileID, short AltIDMod)
        {
            this.GetMapTiles.Add(new MapTile(TileID, AltIDMod));
        }
        public void RemoveMapTile(MapTile iMapTile)
        {
            this.GetMapTiles.Remove(iMapTile);
        }
        public void AddStaticTile(short TileID, short AltIDMod)
        {
            this.GetStaticTiles.Add(new StaticTile(TileID, AltIDMod));
        }
        public void RemoveStaticTile(StaticTile iStaticTile)
        {
            this.GetStaticTiles.Remove(iStaticTile);
        }
        public override string ToString()
        {
            return string.Format("[{0}] {1}", this.GetHaskKeyTable.ToString(), this.Description);
        }
        public Bitmap TransitionImage(ClsTerrainTable iTerrain)
        {
            Bitmap bitmap = new(400, 168, PixelFormat.Format16bppRgb555);
            Graphics graphics = Graphics.FromImage(bitmap);
            Font font = new("Arial", 10f);
            Graphics graphics2 = graphics;
            graphics2.Clear(Color.White);
            Graphics arg_5E_0 = graphics2;
            Image arg_5E_1 = Art.GetLand(iTerrain.TerrianGroup(0).TileID);
            Point point = new(61, 15);
            arg_5E_0.DrawImage(arg_5E_1, point);
            Graphics arg_85_0 = graphics2;
            Image arg_85_1 = Art.GetLand(iTerrain.TerrianGroup(1).TileID);
            point = new Point(84, 38);
            arg_85_0.DrawImage(arg_85_1, point);
            Graphics arg_AC_0 = graphics2;
            Image arg_AC_1 = Art.GetLand(iTerrain.TerrianGroup(2).TileID);
            point = new Point(107, 61);
            arg_AC_0.DrawImage(arg_AC_1, point);
            Graphics arg_D3_0 = graphics2;
            Image arg_D3_1 = Art.GetLand(iTerrain.TerrianGroup(3).TileID);
            point = new Point(38, 38);
            arg_D3_0.DrawImage(arg_D3_1, point);
            Graphics arg_FA_0 = graphics2;
            Image arg_FA_1 = Art.GetLand(iTerrain.TerrianGroup(4).TileID);
            point = new Point(61, 61);
            arg_FA_0.DrawImage(arg_FA_1, point);
            Graphics arg_121_0 = graphics2;
            Image arg_121_1 = Art.GetLand(iTerrain.TerrianGroup(5).TileID);
            point = new Point(84, 84);
            arg_121_0.DrawImage(arg_121_1, point);
            Graphics arg_148_0 = graphics2;
            Image arg_148_1 = Art.GetLand(iTerrain.TerrianGroup(6).TileID);
            point = new Point(15, 61);
            arg_148_0.DrawImage(arg_148_1, point);
            Graphics arg_16F_0 = graphics2;
            Image arg_16F_1 = Art.GetLand(iTerrain.TerrianGroup(7).TileID);
            point = new Point(38, 84);
            arg_16F_0.DrawImage(arg_16F_1, point);
            Graphics arg_196_0 = graphics2;
            Image arg_196_1 = Art.GetLand(iTerrain.TerrianGroup(8).TileID);
            point = new Point(61, 107);
            arg_196_0.DrawImage(arg_196_1, point);
            graphics2.DrawString(this.ToString(), font, Brushes.Black, 151f, 2f);
            graphics.Dispose();
            return bitmap;
        }
        public void Save(XmlTextWriter xmlInfo)
        {
            xmlInfo.WriteStartElement("TransInfo");
            xmlInfo.WriteAttributeString("Description", this.Description);
            xmlInfo.WriteAttributeString("HashKey", this.GetHaskKeyTable.ToString());
            if (this.File != null)
            {
                xmlInfo.WriteAttributeString("File", this.File);
            }
            this.GetMapTiles.Save(xmlInfo);
            this.GetStaticTiles.Save(xmlInfo);
            xmlInfo.WriteEndElement();
        }
        public Transition(XmlElement xmlInfo)
        {
            this.GetHaskKeyTable = new HashKeyCollection();
            this.GetStaticTiles = new StaticTileCollection();
            this.GetMapTiles = new MapTileCollection();
            this.m_RandomTiles = null;
            this.File = null;
            this.Description = xmlInfo.GetAttribute("Description");
            this.GetHaskKeyTable.AddHashKey(xmlInfo.GetAttribute("HashKey"));
            if (StringType.StrCmp(xmlInfo.GetAttribute("File"), string.Empty, false) != 0)
            {
                this.m_RandomTiles = new RandomStatics(xmlInfo.GetAttribute("File"));
                this.File = xmlInfo.GetAttribute("File");
            }
            this.GetMapTiles.Load(xmlInfo);
            this.GetStaticTiles.Load(xmlInfo);
        }
        public Transition()
        {
            this.GetHaskKeyTable = new HashKeyCollection();
            this.GetStaticTiles = new StaticTileCollection();
            this.GetMapTiles = new MapTileCollection();
            this.m_RandomTiles = null;
            this.File = null;
            this.Description = "<New Transition>";
            this.GetHaskKeyTable.Clear();
            byte b = 0;
            do
            {
                this.GetHaskKeyTable.Add(new HashKey());
                b += 1;
            }
            while (b <= 8);
        }
        public Transition(string iDescription, string iHashKey, MapTileCollection iMapTiles, StaticTileCollection iStaticTiles)
        {
            this.GetHaskKeyTable = new HashKeyCollection();
            this.GetStaticTiles = new StaticTileCollection();
            this.GetMapTiles = new MapTileCollection();
            this.m_RandomTiles = null;
            this.File = null;
            this.Description = iDescription;
            this.GetHaskKeyTable.AddHashKey(iHashKey);

            IEnumerator enumerator = iMapTiles.GetEnumerator();

            try
            {
                while (enumerator.MoveNext())
                {
                    MapTile value = (MapTile)enumerator.Current;
                    this.GetMapTiles.Add(value);
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    ((IDisposable)enumerator).Dispose();
                }
            }

            IEnumerator enumerator2 = iStaticTiles.GetEnumerator();

            try
            {
                while (enumerator2.MoveNext())
                {
                    StaticTile value2 = (StaticTile)enumerator2.Current;
                    this.GetStaticTiles.Add(value2);
                }
            }
            finally
            {
                if (enumerator2 is IDisposable)
                {
                    ((IDisposable)enumerator2).Dispose();
                }
            }
        }
        public Transition(string iDescription, ClsTerrain iGroupA, ClsTerrain iGroupB, string iHashKey)
        {
            this.GetHaskKeyTable = new HashKeyCollection();
            this.GetStaticTiles = new StaticTileCollection();
            this.GetMapTiles = new MapTileCollection();
            this.m_RandomTiles = null;
            this.File = null;
            this.Description = iDescription;
            byte b = 0;
            do
            {
                string sLeft = Strings.Mid(iHashKey, checked(b + 1), 1);
                if (StringType.StrCmp(sLeft, "A", false) == 0)
                {
                    this.GetHaskKeyTable.Add(new HashKey(iGroupA.GroupID));
                }
                else
                {
                    if (StringType.StrCmp(sLeft, "B", false) == 0)
                    {
                        this.GetHaskKeyTable.Add(new HashKey(iGroupB.GroupID));
                    }
                }
                b += 1;
            }
            while (b <= 8);
        }
        public Transition(string iDescription, string iHashKey, ClsTerrain iGroupA, ClsTerrain iGroupB, MapTileCollection iMapTiles, StaticTileCollection iStaticTiles)
        {
            this.GetHaskKeyTable = new HashKeyCollection();
            this.GetStaticTiles = new StaticTileCollection();
            this.GetMapTiles = new MapTileCollection();
            this.m_RandomTiles = null;
            this.File = null;
            this.Description = iDescription;
            byte b = 0;
            do
            {
                string sLeft = Strings.Mid(iHashKey, checked(b + 1), 1);
                if (StringType.StrCmp(sLeft, "A", false) == 0)
                {
                    this.GetHaskKeyTable.Add(new HashKey(iGroupA.GroupID));
                }
                else
                {
                    if (StringType.StrCmp(sLeft, "B", false) == 0)
                    {
                        this.GetHaskKeyTable.Add(new HashKey(iGroupB.GroupID));
                    }
                }
                b += 1;
            }
            while (b <= 8);
            if (iMapTiles != null)
            {
                IEnumerator enumerator = iMapTiles.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        MapTile value = (MapTile)enumerator.Current;
                        this.GetMapTiles.Add(value);
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
            if (iStaticTiles != null)
            {
                IEnumerator enumerator2 = iStaticTiles.GetEnumerator();

                try
                {
                    while (enumerator2.MoveNext())
                    {
                        StaticTile value2 = (StaticTile)enumerator2.Current;
                        this.GetStaticTiles.Add(value2);
                    }
                }
                finally
                {
                    if (enumerator2 is IDisposable)
                    {
                        ((IDisposable)enumerator2).Dispose();
                    }
                }
            }
        }
        public Transition(string iDescription, ClsTerrain iGroupA, ClsTerrain iGroupB, ClsTerrain iGroupC, string iHashKey)
        {
            this.GetHaskKeyTable = new HashKeyCollection();
            this.GetStaticTiles = new StaticTileCollection();
            this.GetMapTiles = new MapTileCollection();
            this.m_RandomTiles = null;
            this.File = null;
            this.Description = iDescription;
            byte b = 0;
            do
            {
                string sLeft = Strings.Mid(iHashKey, checked(b + 1), 1);
                if (StringType.StrCmp(sLeft, "A", false) == 0)
                {
                    this.GetHaskKeyTable.Add(new HashKey(iGroupA.GroupID));
                }
                else
                {
                    if (StringType.StrCmp(sLeft, "B", false) == 0)
                    {
                        this.GetHaskKeyTable.Add(new HashKey(iGroupB.GroupID));
                    }
                    else
                    {
                        if (StringType.StrCmp(sLeft, "C", false) == 0)
                        {
                            this.GetHaskKeyTable.Add(new HashKey(iGroupC.GroupID));
                        }
                    }
                }
                b += 1;
            }
            while (b <= 8);
        }
        public Transition(string iDescription, string iHashKey)
        {
            this.GetHaskKeyTable = new HashKeyCollection();
            this.GetStaticTiles = new StaticTileCollection();
            this.GetMapTiles = new MapTileCollection();
            this.m_RandomTiles = null;
            this.File = null;
            this.Description = iDescription;
            byte b = 0;
            do
            {
                this.GetHaskKeyTable.Add(new HashKey(Strings.Mid(iHashKey, checked(b * 2 + 1), 2)));
                b += 1;
            }
            while (b <= 8);
        }
        public Transition(string iDescription, ClsTerrain iGroupA, ClsTerrain iGroupB, string iHashKey, MapTile iMapTile)
        {
            this.GetHaskKeyTable = new HashKeyCollection();
            this.GetStaticTiles = new StaticTileCollection();
            this.GetMapTiles = new MapTileCollection();
            this.m_RandomTiles = null;
            this.File = null;
            this.Description = iDescription;
            byte b = 0;
            do
            {
                string sLeft = Strings.Mid(iHashKey, checked(b + 1), 1);
                if (StringType.StrCmp(sLeft, "A", false) == 0)
                {
                    this.GetHaskKeyTable.Add(new HashKey(iGroupA.GroupID));
                }
                else
                {
                    if (StringType.StrCmp(sLeft, "B", false) == 0)
                    {
                        this.GetHaskKeyTable.Add(new HashKey(iGroupB.GroupID));
                    }
                }
                b += 1;
            }
            while (b <= 8);
            this.GetMapTiles.Add(iMapTile);
        }
    }
}
