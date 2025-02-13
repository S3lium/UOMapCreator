﻿using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml;
using Terrain;
using Transition;
using Ultima;

namespace TransitionWizard
{
    public partial class TransitionWizard : Form
    {
        private readonly Art UOArt;
        private bool ViewTiles;
        private readonly Transition.Transition iTransition;
        private readonly TransitionTable iTransitionTable;

        private readonly TreeNode iMapNode;
        private readonly TreeNode iStaticNode;
        private readonly TreeNode iMapOuterTopLeft;
        private readonly TreeNode iMapOuterTopRight;
        private readonly TreeNode iMapOuterBottomLeft;
        private readonly TreeNode iMapOuterBottomRight;
        private readonly TreeNode iMapInnerTopLeft;
        private readonly TreeNode iMapInnerTop;
        private readonly TreeNode iMapInnerTopRight;
        private readonly TreeNode iMapInnerLeft;
        private readonly TreeNode iMapInnerRight;
        private readonly TreeNode iMapInnerBottomLeft;
        private readonly TreeNode iMapInnerBottom;
        private readonly TreeNode iMapInnerBottomRight;
        private readonly TreeNode iStaticOuterTopLeft;
        private readonly TreeNode iStaticOuterTopRight;
        private readonly TreeNode iStaticOuterBottomLeft;
        private readonly TreeNode iStaticOuterBottomRight;
        private readonly TreeNode iStaticInnerTopLeft;
        private readonly TreeNode iStaticInnerTop;
        private readonly TreeNode iStaticInnerTopRight;
        private readonly TreeNode iStaticInnerLeft;
        private readonly TreeNode iStaticInnerRight;
        private readonly TreeNode iStaticInnerBottomLeft;
        private readonly TreeNode iStaticInnerBottom;
        private readonly TreeNode iStaticInnerBottomRight;

        private readonly ClsTerrainTable iGroupA;
        private readonly ClsTerrainTable iGroupB;

        public TransitionWizard()
        {
            TransitionWizard twiz = this;
            base.Load += new EventHandler(twiz.Twiz_Load);
            this.ViewTiles = false;
            this.iMapNode = new TreeNode("Map Tiles");
            this.iStaticNode = new TreeNode("Static Tiles");
            this.iMapOuterTopLeft = new TreeNode("Outer Top Left");
            this.iMapOuterTopRight = new TreeNode("Outer Top Right");
            this.iMapOuterBottomLeft = new TreeNode("Outer Bottom Left");
            this.iMapOuterBottomRight = new TreeNode("Outer Bottom Right");
            this.iMapInnerTopLeft = new TreeNode("Inner Top Left");
            this.iMapInnerTop = new TreeNode("Inner Top");
            this.iMapInnerTopRight = new TreeNode("Inner Top Right");
            this.iMapInnerLeft = new TreeNode("Inner Left");
            this.iMapInnerRight = new TreeNode("Inner Right");
            this.iMapInnerBottomLeft = new TreeNode("Inner Bottom Left");
            this.iMapInnerBottom = new TreeNode("Inner Bottom");
            this.iMapInnerBottomRight = new TreeNode("Inner Bottom Right");
            this.iStaticOuterTopLeft = new TreeNode("Outer Top Left");
            this.iStaticOuterTopRight = new TreeNode("Outer Top Right");
            this.iStaticOuterBottomLeft = new TreeNode("Outer Bottom Left");
            this.iStaticOuterBottomRight = new TreeNode("Outer Bottom Right");
            this.iStaticInnerTopLeft = new TreeNode("Inner Top Left");
            this.iStaticInnerTop = new TreeNode("Inner Top");
            this.iStaticInnerTopRight = new TreeNode("Inner Top Right");
            this.iStaticInnerLeft = new TreeNode("Inner Left");
            this.iStaticInnerRight = new TreeNode("Inner Right");
            this.iStaticInnerBottomLeft = new TreeNode("Inner Bottom Left");
            this.iStaticInnerBottom = new TreeNode("Inner Bottom");
            this.iStaticInnerBottomRight = new TreeNode("Inner Bottom Right");
            this.iGroupA = new ClsTerrainTable();
            this.iGroupB = new ClsTerrainTable();
            InitializeComponent();
        }

        private void Add_Tile()
        {
            TreeNode treeNode = new(string.Format("Tile:{0}", this.TileID.Value))
            {
                Tag = this.TileID.Value
            };
            if (this.iMapOuterTopLeft.IsSelected)
            {
                this.iMapOuterTopLeft.Nodes.Add(treeNode);
            }
            if (this.iMapInnerTopLeft.IsSelected)
            {
                this.iMapInnerTopLeft.Nodes.Add(treeNode);
            }
            if (this.iMapInnerTop.IsSelected)
            {
                this.iMapInnerTop.Nodes.Add(treeNode);
            }
            if (this.iMapInnerTopRight.IsSelected)
            {
                this.iMapInnerTopRight.Nodes.Add(treeNode);
            }
            if (this.iMapOuterTopRight.IsSelected)
            {
                this.iMapOuterTopRight.Nodes.Add(treeNode);
            }
            if (this.iMapInnerLeft.IsSelected)
            {
                this.iMapInnerLeft.Nodes.Add(treeNode);
            }
            if (this.iMapInnerRight.IsSelected)
            {
                this.iMapInnerRight.Nodes.Add(treeNode);
            }
            if (this.iMapOuterBottomLeft.IsSelected)
            {
                this.iMapOuterBottomLeft.Nodes.Add(treeNode);
            }
            if (this.iMapInnerBottomLeft.IsSelected)
            {
                this.iMapInnerBottomLeft.Nodes.Add(treeNode);
            }
            if (this.iMapInnerBottom.IsSelected)
            {
                this.iMapInnerBottom.Nodes.Add(treeNode);
            }
            if (this.iMapInnerBottomRight.IsSelected)
            {
                this.iMapInnerBottomRight.Nodes.Add(treeNode);
            }
            if (this.iMapOuterBottomRight.IsSelected)
            {
                this.iMapOuterBottomRight.Nodes.Add(treeNode);
            }
            if (this.iStaticOuterTopLeft.IsSelected)
            {
                this.iStaticOuterTopLeft.Nodes.Add(treeNode);
            }
            if (this.iStaticInnerTopLeft.IsSelected)
            {
                this.iStaticInnerTopLeft.Nodes.Add(treeNode);
            }
            if (this.iStaticInnerTop.IsSelected)
            {
                this.iStaticInnerTop.Nodes.Add(treeNode);
            }
            if (this.iStaticInnerTopRight.IsSelected)
            {
                this.iStaticInnerTopRight.Nodes.Add(treeNode);
            }
            if (this.iStaticOuterTopRight.IsSelected)
            {
                this.iStaticOuterTopRight.Nodes.Add(treeNode);
            }
            if (this.iStaticInnerLeft.IsSelected)
            {
                this.iStaticInnerLeft.Nodes.Add(treeNode);
            }
            if (this.iStaticInnerRight.IsSelected)
            {
                this.iStaticInnerRight.Nodes.Add(treeNode);
            }
            if (this.iStaticOuterBottomLeft.IsSelected)
            {
                this.iStaticOuterBottomLeft.Nodes.Add(treeNode);
            }
            if (this.iStaticInnerBottomLeft.IsSelected)
            {
                this.iStaticInnerBottomLeft.Nodes.Add(treeNode);
            }
            if (this.iStaticInnerBottom.IsSelected)
            {
                this.iStaticInnerBottom.Nodes.Add(treeNode);
            }
            if (this.iStaticInnerBottomRight.IsSelected)
            {
                this.iStaticInnerBottomRight.Nodes.Add(treeNode);
            }
            if (this.iStaticOuterBottomRight.IsSelected)
            {
                this.iStaticOuterBottomRight.Nodes.Add(treeNode);
            }
            this.Panel2.Refresh();
        }

        private void Delete_Tile()
        {
            TreeNode selectedNode = this.TreeView1.SelectedNode;
            if (selectedNode.Tag != null)
            {
                this.TreeView1.Nodes.Remove(selectedNode);
                this.Panel2.Refresh();
            }
        }

        private void DrawStatic(short TileID, short X, short Y, PaintEventArgs e)
        {
            Bitmap bitmap = new(Art.GetStatic(TileID));
            X = checked((short)(checked(X + 22)));
            Y = checked((short)(checked(Y + 22)));
            Point point = new(checked((int)Math.Round(X - (double)bitmap.Width / 2)), checked(checked(Y - bitmap.Height) + 21));
            e.Graphics.DrawImage(bitmap, point);
        }

        private void Twiz_Load(object sender, EventArgs e)
        {
            this.iMapNode.Tag = "Map Tiles";
            this.iStaticNode.Tag = "Static Tiles";
            this.iGroupA.Load();
            this.iGroupB.Load();
            this.iGroupA.Display(this.Select_Group_A);
            this.iGroupB.Display(this.Select_Group_B);
            this.NodeInit();
        }

        public MapTileCollection Get_MapTile(TreeNodeCollection iTreeNode)
        {
            IEnumerator enumerator = null;
            MapTileCollection mapTileCollection = new();
            mapTileCollection.Clear();
            try
            {
                enumerator = iTreeNode.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    TreeNode current = (TreeNode)enumerator.Current;
                    mapTileCollection.Add(new MapTile(ShortType.FromObject(current.Tag), 0));
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    ((IDisposable)enumerator).Dispose();
                }
            }
            return mapTileCollection;
        }

        public MapTileCollection Get_MapTile(short iTileID)
        {
            MapTileCollection mapTileCollection = new();
            mapTileCollection.Clear();
            mapTileCollection.Add(new MapTile(iTileID, 0));
            return mapTileCollection;
        }

        public MapTileCollection Get_MapTiles(string iMapTile)
        {
            MapTileCollection mapTileCollection = new();
            string str = iMapTile;
            if (StringType.StrCmp(str, "Outer Top Left", false) == 0)
            {
                mapTileCollection = this.Get_MapTile(this.iMapOuterTopLeft.Nodes);
            }
            else if (StringType.StrCmp(str, "Outer Top Right", false) == 0)
            {
                mapTileCollection = this.Get_MapTile(this.iMapOuterTopRight.Nodes);
            }
            else if (StringType.StrCmp(str, "Outer Bottom Left", false) == 0)
            {
                mapTileCollection = this.Get_MapTile(this.iMapOuterBottomLeft.Nodes);
            }
            else if (StringType.StrCmp(str, "Outer Bottom Right", false) == 0)
            {
                mapTileCollection = this.Get_MapTile(this.iMapOuterBottomRight.Nodes);
            }
            else if (StringType.StrCmp(str, "Inner Top Left", false) == 0)
            {
                mapTileCollection = this.Get_MapTile(this.iMapInnerTopLeft.Nodes);
            }
            else if (StringType.StrCmp(str, "Inner Top", false) == 0)
            {
                mapTileCollection = this.Get_MapTile(this.iMapInnerTop.Nodes);
            }
            else if (StringType.StrCmp(str, "Inner Top Right", false) == 0)
            {
                mapTileCollection = this.Get_MapTile(this.iMapInnerTopRight.Nodes);
            }
            else if (StringType.StrCmp(str, "Inner Left", false) == 0)
            {
                mapTileCollection = this.Get_MapTile(this.iMapInnerLeft.Nodes);
            }
            else if (StringType.StrCmp(str, "Inner Right", false) == 0)
            {
                mapTileCollection = this.Get_MapTile(this.iMapInnerRight.Nodes);
            }
            else if (StringType.StrCmp(str, "Inner Bottom Left", false) == 0)
            {
                mapTileCollection = this.Get_MapTile(this.iMapInnerBottomLeft.Nodes);
            }
            else if (StringType.StrCmp(str, "Inner Bottom", false) == 0)
            {
                mapTileCollection = this.Get_MapTile(this.iMapInnerBottom.Nodes);
            }
            else if (StringType.StrCmp(str, "Inner Bottom Right", false) == 0)
            {
                mapTileCollection = this.Get_MapTile(this.iMapInnerBottomRight.Nodes);
            }
            else if (StringType.StrCmp(str, "Autocorrect", false) == 0)
            {
                mapTileCollection = this.Get_MapTile(((ClsTerrain)this.Select_Group_B.SelectedItem).TileID);
            }
            return mapTileCollection;
        }

        public StaticTileCollection Get_StaticTile(TreeNodeCollection iTreeNode)
        {
            IEnumerator enumerator = null;
            StaticTileCollection staticTileCollection = new();
            staticTileCollection.Clear();
            try
            {
                enumerator = iTreeNode.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    TreeNode current = (TreeNode)enumerator.Current;
                    staticTileCollection.Add(new Transition.StaticTile(ShortType.FromObject(current.Tag), 0));
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    ((IDisposable)enumerator).Dispose();
                }
            }
            return staticTileCollection;
        }

        public StaticTileCollection Get_StaticTiles(string iStaticTile)
        {
            StaticTileCollection staticTileCollection;
            StaticTileCollection staticTile = new();
            string str = iStaticTile;
            if (StringType.StrCmp(str, "Outer Top Left", false) == 0)
            {
                staticTile = this.Get_StaticTile(this.iStaticOuterTopLeft.Nodes);
            }
            else if (StringType.StrCmp(str, "Outer Top Right", false) == 0)
            {
                staticTile = this.Get_StaticTile(this.iStaticOuterTopRight.Nodes);
            }
            else if (StringType.StrCmp(str, "Outer Bottom Left", false) == 0)
            {
                staticTile = this.Get_StaticTile(this.iStaticOuterBottomLeft.Nodes);
            }
            else if (StringType.StrCmp(str, "Outer Bottom Right", false) == 0)
            {
                staticTile = this.Get_StaticTile(this.iStaticOuterBottomRight.Nodes);
            }
            else if (StringType.StrCmp(str, "Inner Top Left", false) == 0)
            {
                staticTile = this.Get_StaticTile(this.iStaticInnerTopLeft.Nodes);
            }
            else if (StringType.StrCmp(str, "Inner Top", false) == 0)
            {
                staticTile = this.Get_StaticTile(this.iStaticInnerTop.Nodes);
            }
            else if (StringType.StrCmp(str, "Inner Top Right", false) == 0)
            {
                staticTile = this.Get_StaticTile(this.iStaticInnerTopRight.Nodes);
            }
            else if (StringType.StrCmp(str, "Inner Left", false) == 0)
            {
                staticTile = this.Get_StaticTile(this.iStaticInnerLeft.Nodes);
            }
            else if (StringType.StrCmp(str, "Inner Right", false) == 0)
            {
                staticTile = this.Get_StaticTile(this.iStaticInnerRight.Nodes);
            }
            else if (StringType.StrCmp(str, "Inner Bottom Left", false) == 0)
            {
                staticTile = this.Get_StaticTile(this.iStaticInnerBottomLeft.Nodes);
            }
            else if (StringType.StrCmp(str, "Inner Bottom", false) == 0)
            {
                staticTile = this.Get_StaticTile(this.iStaticInnerBottom.Nodes);
            }
            else if (StringType.StrCmp(str, "Inner Bottom Right", false) != 0)
            {
                if (StringType.StrCmp(str, "Autocorrect", false) != 0)
                {
                    staticTileCollection = staticTile;
                    return staticTileCollection;
                }
                staticTileCollection = null;
                return staticTileCollection;
            }
            else
            {
                staticTile = this.Get_StaticTile(this.iStaticInnerBottomRight.Nodes);
            }
            staticTileCollection = staticTile;
            return staticTileCollection;
        }

        private Point[] GetPoints(int X, int Y)
        {
            Point point = new(checked(X + 21), Y);
            Point point1 = new(X, checked(Y + 21));
            Point point2 = new(checked(X + 21), checked(Y + 42));
            Point point3 = new(checked(X + 42), checked(Y + 21));
            Point point4 = new(checked(X + 21), Y);
            return new Point[] { point, point1, point2, point3, point4 };
        }

        private void LandTileID_Scroll(object sender, ScrollEventArgs e)
        {
            this.Panel1.Refresh();
        }

        private void MenuItem3_Click(object sender, EventArgs e)
        {
            this.ViewTiles = false;
            this.Panel2.Refresh();
            this.Panel1.Refresh();
        }

        private void MenuItem4_Click(object sender, EventArgs e)
        {
            this.ViewTiles = true;
            this.Panel2.Refresh();
            this.Panel1.Refresh();
        }

        private void MenuItem5_Click(object sender, EventArgs e)
        {
            IEnumerator enumerator = null;
            Transition.Transition transition = new();
            TransitionTable transitionTable = new();
            if (this.Select_Group_A != null)
            {
                if (this.Select_Group_B != null)
                {
                    if (ObjectType.ObjTst(LateBinding.LateGet(this.Select_Group_A.SelectedItem, null, "Name", new object[0], null, null), LateBinding.LateGet(this.Select_Group_B.SelectedItem, null, "Name", new object[0], null, null), false) != 0)
                    {
                        string str = string.Format("{0} To {1}", RuntimeHelpers.GetObjectValue(LateBinding.LateGet(this.Select_Group_A.SelectedItem, null, "Name", new object[0], null, null)), RuntimeHelpers.GetObjectValue(LateBinding.LateGet(this.Select_Group_B.SelectedItem, null, "Name", new object[0], null, null)));
                        string str1 = string.Format("Data/System/2Way_Template.xml");
                        XmlDocument xmlDocument = new();
                        transitionTable.Clear();
                        try
                        {
                            xmlDocument.Load(str1);
                            try
                            {
                                enumerator = xmlDocument.SelectNodes("//Wizard/Tile").GetEnumerator();
                                while (enumerator.MoveNext())
                                {
                                    XmlElement current = (XmlElement)enumerator.Current;
                                    string attribute = current.GetAttribute("Pattern");
                                    string attribute1 = current.GetAttribute("MapTile");
                                    string attribute2 = current.GetAttribute("StaticTile");
                                    Transition.Transition transition1 = new(str, attribute, (ClsTerrain)this.Select_Group_A.SelectedItem, (ClsTerrain)this.Select_Group_B.SelectedItem, this.Get_MapTiles(attribute1), this.Get_StaticTiles(attribute2));
                                    transitionTable.Add(transition1);
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
                        catch (Exception exception)
                        {
                            ProjectData.SetProjectError(exception);
                            Interaction.MsgBox(exception.ToString(), MsgBoxStyle.OkOnly, null);
                            ProjectData.ClearProjectError();
                        }
                        transitionTable.Save(string.Format("{0}.xml", str));
                    }
                }
            }
        }

        private void NodeInit()
        {
            this.iMapNode.Nodes.Add(this.iMapOuterTopLeft);
            this.iMapNode.Nodes.Add(this.iMapInnerTopLeft);
            this.iMapNode.Nodes.Add(this.iMapInnerTop);
            this.iMapNode.Nodes.Add(this.iMapInnerTopRight);
            this.iMapNode.Nodes.Add(this.iMapOuterTopRight);
            this.iMapNode.Nodes.Add(this.iMapInnerLeft);
            this.iMapNode.Nodes.Add(this.iMapInnerRight);
            this.iMapNode.Nodes.Add(this.iMapOuterBottomLeft);
            this.iMapNode.Nodes.Add(this.iMapInnerBottomLeft);
            this.iMapNode.Nodes.Add(this.iMapInnerBottom);
            this.iMapNode.Nodes.Add(this.iMapInnerBottomRight);
            this.iMapNode.Nodes.Add(this.iMapOuterBottomRight);
            this.TreeView1.Nodes.Add(this.iMapNode);
            this.iStaticNode.Nodes.Add(this.iStaticOuterTopLeft);
            this.iStaticNode.Nodes.Add(this.iStaticInnerTopLeft);
            this.iStaticNode.Nodes.Add(this.iStaticInnerTop);
            this.iStaticNode.Nodes.Add(this.iStaticInnerTopRight);
            this.iStaticNode.Nodes.Add(this.iStaticOuterTopRight);
            this.iStaticNode.Nodes.Add(this.iStaticInnerLeft);
            this.iStaticNode.Nodes.Add(this.iStaticInnerRight);
            this.iStaticNode.Nodes.Add(this.iStaticOuterBottomLeft);
            this.iStaticNode.Nodes.Add(this.iStaticInnerBottomLeft);
            this.iStaticNode.Nodes.Add(this.iStaticInnerBottom);
            this.iStaticNode.Nodes.Add(this.iStaticInnerBottomRight);
            this.iStaticNode.Nodes.Add(this.iStaticOuterBottomRight);
            this.TreeView1.Nodes.Add(this.iStaticNode);
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            Point point;
            System.Drawing.Font font = new("Arial", 10f, FontStyle.Regular);
            SolidBrush solidBrush = new(Color.Black);
            Graphics graphics = e.Graphics;
            graphics.DrawString(StringType.FromInteger(this.TileID.Value), font, solidBrush, 51f, 5f);
            switch (this.ViewTiles)
            {
                case false:
                    {
                        if (Art.GetLand(this.TileID.Value) != null)
                        {
                            Bitmap land = Art.GetLand(this.TileID.Value);
                            point = new Point(5, 5);
                            graphics.DrawImage(land, point);
                        }
                        break;
                    }
                case true:
                    {
                        if (Art.GetStatic(this.TileID.Value) != null)
                        {
                            Bitmap @static = Art.GetStatic(this.TileID.Value);
                            point = new Point(5, 5);
                            graphics.DrawImage(@static, point);
                        }
                        break;
                    }
            }
            graphics = null;
        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {
            string name = null;
            string str = null;
            Point point;
            Graphics graphics = e.Graphics;
            graphics.DrawPolygon(new Pen(Color.Blue), this.GetPoints(5, 125));
            graphics.DrawPolygon(new Pen(Color.Blue), this.GetPoints(197, 125));
            graphics.DrawPolygon(new Pen(Color.Blue), this.GetPoints(101, 29));
            graphics.DrawPolygon(new Pen(Color.Blue), this.GetPoints(101, 221));
            graphics.DrawPolygon(new Pen(Color.Blue), this.GetPoints(101, 79));
            graphics.DrawPolygon(new Pen(Color.Blue), this.GetPoints(78, 102));
            graphics.DrawPolygon(new Pen(Color.Blue), this.GetPoints(124, 102));
            graphics.DrawPolygon(new Pen(Color.Blue), this.GetPoints(55, 125));
            graphics.DrawPolygon(new Pen(Color.Blue), this.GetPoints(101, 125));
            graphics.DrawPolygon(new Pen(Color.Blue), this.GetPoints(147, 125));
            graphics.DrawPolygon(new Pen(Color.Blue), this.GetPoints(78, 148));
            graphics.DrawPolygon(new Pen(Color.Blue), this.GetPoints(124, 148));
            graphics.DrawPolygon(new Pen(Color.Blue), this.GetPoints(101, 171));
            if (!this.ViewTiles)
            {
                if (this.iMapOuterTopLeft.GetNodeCount(true) > 0)
                {
                    Bitmap land = Art.GetLand(IntegerType.FromObject(this.iMapOuterTopLeft.Nodes[0].Tag));
                    point = new Point(101, 29);
                    graphics.DrawImage(land, point);
                }
                if (this.iMapInnerTopLeft.GetNodeCount(true) > 0)
                {
                    Bitmap bitmap = Art.GetLand(IntegerType.FromObject(this.iMapInnerTopLeft.Nodes[0].Tag));
                    point = new Point(101, 79);
                    graphics.DrawImage(bitmap, point);
                }
                if (this.iMapInnerTop.GetNodeCount(true) > 0)
                {
                    Bitmap land1 = Art.GetLand(IntegerType.FromObject(this.iMapInnerTop.Nodes[0].Tag));
                    point = new Point(124, 102);
                    graphics.DrawImage(land1, point);
                }
                if (this.iMapInnerTopRight.GetNodeCount(true) > 0)
                {
                    Bitmap bitmap1 = Art.GetLand(IntegerType.FromObject(this.iMapInnerTopRight.Nodes[0].Tag));
                    point = new Point(147, 125);
                    graphics.DrawImage(bitmap1, point);
                }
                if (this.iMapOuterTopRight.GetNodeCount(true) > 0)
                {
                    Bitmap land2 = Art.GetLand(IntegerType.FromObject(this.iMapOuterTopRight.Nodes[0].Tag));
                    point = new Point(197, 125);
                    graphics.DrawImage(land2, point);
                }
                if (this.iMapInnerLeft.GetNodeCount(true) > 0)
                {
                    Bitmap bitmap2 = Art.GetLand(IntegerType.FromObject(this.iMapInnerLeft.Nodes[0].Tag));
                    point = new Point(78, 102);
                    graphics.DrawImage(bitmap2, point);
                }
                if (this.iMapInnerRight.GetNodeCount(true) > 0)
                {
                    Bitmap land3 = Art.GetLand(IntegerType.FromObject(this.iMapInnerRight.Nodes[0].Tag));
                    point = new Point(124, 148);
                    graphics.DrawImage(land3, point);
                }
                if (this.iMapOuterBottomLeft.GetNodeCount(true) > 0)
                {
                    Bitmap bitmap3 = Art.GetLand(IntegerType.FromObject(this.iMapOuterBottomLeft.Nodes[0].Tag));
                    point = new Point(5, 125);
                    graphics.DrawImage(bitmap3, point);
                }
                if (this.iMapInnerBottomLeft.GetNodeCount(true) > 0)
                {
                    Bitmap land4 = Art.GetLand(IntegerType.FromObject(this.iMapInnerBottomLeft.Nodes[0].Tag));
                    point = new Point(55, 125);
                    graphics.DrawImage(land4, point);
                }
                if (this.iMapInnerBottom.GetNodeCount(true) > 0)
                {
                    Bitmap bitmap4 = Art.GetLand(IntegerType.FromObject(this.iMapInnerBottom.Nodes[0].Tag));
                    point = new Point(78, 148);
                    graphics.DrawImage(bitmap4, point);
                }
                if (this.iMapInnerBottomRight.GetNodeCount(true) > 0)
                {
                    Bitmap land5 = Art.GetLand(IntegerType.FromObject(this.iMapInnerBottomRight.Nodes[0].Tag));
                    point = new Point(101, 171);
                    graphics.DrawImage(land5, point);
                }
                if (this.iMapOuterBottomRight.GetNodeCount(true) > 0)
                {
                    Bitmap bitmap5 = Art.GetLand(IntegerType.FromObject(this.iMapOuterBottomRight.Nodes[0].Tag));
                    point = new Point(101, 221);
                    graphics.DrawImage(bitmap5, point);
                }
            }
            else
            {
                if (this.iStaticOuterTopLeft.GetNodeCount(true) > 0)
                {
                    this.DrawStatic(ShortType.FromObject(this.iStaticOuterTopLeft.Nodes[0].Tag), 101, 29, e);
                }
                if (this.iStaticInnerTopLeft.GetNodeCount(true) > 0)
                {
                    this.DrawStatic(ShortType.FromObject(this.iStaticInnerTopLeft.Nodes[0].Tag), 101, 79, e);
                }
                if (this.iStaticInnerTop.GetNodeCount(true) > 0)
                {
                    this.DrawStatic(ShortType.FromObject(this.iStaticInnerTop.Nodes[0].Tag), 124, 102, e);
                }
                if (this.iStaticInnerTopRight.GetNodeCount(true) > 0)
                {
                    this.DrawStatic(ShortType.FromObject(this.iStaticInnerTopRight.Nodes[0].Tag), 147, 125, e);
                }
                if (this.iStaticOuterTopRight.GetNodeCount(true) > 0)
                {
                    this.DrawStatic(ShortType.FromObject(this.iStaticOuterTopRight.Nodes[0].Tag), 197, 125, e);
                }
                if (this.iStaticInnerLeft.GetNodeCount(true) > 0)
                {
                    this.DrawStatic(ShortType.FromObject(this.iStaticInnerLeft.Nodes[0].Tag), 78, 102, e);
                }
                if (this.iStaticInnerRight.GetNodeCount(true) > 0)
                {
                    this.DrawStatic(ShortType.FromObject(this.iStaticInnerRight.Nodes[0].Tag), 124, 148, e);
                }
                if (this.iStaticOuterBottomLeft.GetNodeCount(true) > 0)
                {
                    this.DrawStatic(ShortType.FromObject(this.iStaticOuterBottomLeft.Nodes[0].Tag), 5, 125, e);
                }
                if (this.iStaticInnerBottomLeft.GetNodeCount(true) > 0)
                {
                    this.DrawStatic(ShortType.FromObject(this.iStaticInnerBottomLeft.Nodes[0].Tag), 55, 125, e);
                }
                if (this.iStaticInnerBottom.GetNodeCount(true) > 0)
                {
                    this.DrawStatic(ShortType.FromObject(this.iStaticInnerBottom.Nodes[0].Tag), 78, 148, e);
                }
                if (this.iStaticInnerBottomRight.GetNodeCount(true) > 0)
                {
                    this.DrawStatic(ShortType.FromObject(this.iStaticInnerBottomRight.Nodes[0].Tag), 101, 171, e);
                }
                if (this.iStaticOuterBottomRight.GetNodeCount(true) > 0)
                {
                    this.DrawStatic(ShortType.FromObject(this.iStaticOuterBottomRight.Nodes[0].Tag), 101, 221, e);
                }
            }
            Pen pen = new(Color.Red);
            if (this.iMapOuterTopLeft.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(101, 29));
            }
            if (this.iMapInnerTopLeft.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(101, 79));
            }
            if (this.iMapInnerTop.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(124, 102));
            }
            if (this.iMapInnerTopRight.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(147, 125));
            }
            if (this.iMapOuterTopRight.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(197, 125));
            }
            if (this.iMapInnerLeft.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(78, 102));
            }
            if (this.iMapInnerRight.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(124, 148));
            }
            if (this.iMapOuterBottomLeft.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(5, 125));
            }
            if (this.iMapInnerBottomLeft.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(55, 125));
            }
            if (this.iMapInnerBottom.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(78, 148));
            }
            if (this.iMapInnerBottomRight.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(101, 171));
            }
            if (this.iMapOuterBottomRight.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(101, 221));
            }
            pen = new Pen(Color.Magenta);
            if (this.iStaticOuterTopLeft.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(101, 29));
            }
            if (this.iStaticInnerTopLeft.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(101, 79));
            }
            if (this.iStaticInnerTop.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(124, 102));
            }
            if (this.iStaticInnerTopRight.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(147, 125));
            }
            if (this.iStaticOuterTopRight.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(197, 125));
            }
            if (this.iStaticInnerLeft.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(78, 102));
            }
            if (this.iStaticInnerRight.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(124, 148));
            }
            if (this.iStaticOuterBottomLeft.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(5, 125));
            }
            if (this.iStaticInnerBottomLeft.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(55, 125));
            }
            if (this.iStaticInnerBottom.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(78, 148));
            }
            if (this.iStaticInnerBottomRight.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(101, 171));
            }
            if (this.iStaticOuterBottomRight.IsSelected)
            {
                graphics.DrawPolygon(pen, this.GetPoints(101, 221));
            }
            ClsTerrain selectedItem = (ClsTerrain)this.Select_Group_A.SelectedItem;
            if (selectedItem != null)
            {
                Bitmap land6 = Art.GetLand(selectedItem.TileID);
                point = new Point(5, 242);
                graphics.DrawImage(land6, point);
                Bitmap bitmap6 = Art.GetLand(selectedItem.TileID);
                point = new Point(101, 125);
                graphics.DrawImage(bitmap6, point);
                name = selectedItem.Name;
            }
            selectedItem = (ClsTerrain)this.Select_Group_B.SelectedItem;
            if (selectedItem != null)
            {
                Bitmap land7 = Art.GetLand(selectedItem.TileID);
                point = new Point(55, 242);
                graphics.DrawImage(land7, point);
                str = selectedItem.Name;
            }
            this.TextBox1.Text = string.Format("{0} To {1}", name, str);
            graphics = null;
        }

        private void Select_Group_A_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Panel2.Refresh();
        }

        private void Select_Group_B_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Panel2.Refresh();
        }

        #region private void ToolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)

        #region Original Code Doesn't Work
        /*
        private void ToolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            object tag = e.Button.Tag;
            if (ObjectType.ObjTst(tag, "Add", false) == 0)
            {
                this.Add_Tile();
            }
            else if (ObjectType.ObjTst(tag, "Delete", false) == 0)
            {
                this.Delete_Tile();
            }
        }
        */
        #endregion

        private void ToolAdd_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            object tag = button.Tag;
            if (ObjectType.ObjTst(tag, "Add", false) == 0)
            {
                this.Add_Tile();
            }
        }

        private void ToolDelete_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            object tag = button.Tag;
            if (ObjectType.ObjTst(tag, "Delete", false) == 0)
            {
                this.Delete_Tile();
            }
        }

        //MapZoom
        private void MapZoom_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            object tag = button.Tag;
            if (ObjectType.ObjTst(tag, "MapZoom", false) == 0)
            {
                MapZoom mapZoom = new();
                mapZoom.Tag = TileID;
                mapZoom.Show();
            }
        }

        //StaticZoom
        private void StaticZoom_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            object tag = button.Tag;
            if (ObjectType.ObjTst(tag, "StaticZoom", false) == 0)
            {
                StaticZoom staticZoom = new();
                staticZoom.Tag = TileID;
                staticZoom.Show();
            }
        }

        #endregion

        private void TreeView1_AfterSelec(object sender, TreeViewEventArgs e)
        {
            if (this.iMapNode.IsSelected)
            {
                this.ViewTiles = false;
            }
            if (this.iStaticNode.IsSelected)
            {
                this.ViewTiles = true;
            }
            this.Panel1.Refresh();
            this.Panel2.Refresh();
        }

        //New
        private void MenuItem6_Click(object sender, EventArgs e)
        {
            new TransitionWizard().Show();
            this.Hide();
        }

        //Save
        private void MenuItem7_Click(object sender, EventArgs e)
        {
            IEnumerator enumerator = null;
            Transition.Transition transition = new();
            TransitionTable transitionTable = new();
            if (this.Select_Group_A != null)
            {
                if (this.Select_Group_B != null)
                {
                    if (ObjectType.ObjTst(LateBinding.LateGet(this.Select_Group_A.SelectedItem, null, "Name", new object[0], null, null), LateBinding.LateGet(this.Select_Group_B.SelectedItem, null, "Name", new object[0], null, null), false) != 0)
                    {
                        string str = string.Format("{0} To {1}", RuntimeHelpers.GetObjectValue(LateBinding.LateGet(this.Select_Group_A.SelectedItem, null, "Name", new object[0], null, null)), RuntimeHelpers.GetObjectValue(LateBinding.LateGet(this.Select_Group_B.SelectedItem, null, "Name", new object[0], null, null)));
                        string str1 = string.Format("Data/System/2Way_Template.xml");
                        XmlDocument xmlDocument = new();
                        transitionTable.Clear();
                        try
                        {
                            xmlDocument.Load(str1);
                            try
                            {
                                enumerator = xmlDocument.SelectNodes("//Wizard/Tile").GetEnumerator();
                                while (enumerator.MoveNext())
                                {
                                    XmlElement current = (XmlElement)enumerator.Current;
                                    string attribute = current.GetAttribute("Pattern");
                                    string attribute1 = current.GetAttribute("MapTile");
                                    string attribute2 = current.GetAttribute("StaticTile");
                                    Transition.Transition transition1 = new(str, attribute, (ClsTerrain)this.Select_Group_A.SelectedItem, (ClsTerrain)this.Select_Group_B.SelectedItem, this.Get_MapTiles(attribute1), this.Get_StaticTiles(attribute2));
                                    transitionTable.Add(transition1);
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
                        catch (Exception exception)
                        {
                            ProjectData.SetProjectError(exception);
                            Interaction.MsgBox(exception.ToString(), MsgBoxStyle.OkOnly, null);
                            ProjectData.ClearProjectError();
                        }
                        transitionTable.Save(string.Format("{0}.xml", str));
                    }
                }
            }
        }

        //Load
        private void MenuItem8_Click(object sender, EventArgs e)
        {
            //ToDo: Load Up Transitions xml          
        }
    }
}
