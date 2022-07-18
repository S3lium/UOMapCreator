using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Terrain;
using Transition;
using Ultima;

namespace CreateTransitions
{
    public partial class CreateTransitions : Form
    {
        private readonly VScrollBar _LandItems;
        private readonly Art UOArt;
        private bool ImageTest;
        private readonly ClsTerrainTable iGroups;
        private ClsTerrain iSelectedGroup;
        private readonly bool iSelected;
        private Transition.Transition iTransition;
        private readonly TransitionTable iTransitionTable;

        public ClsTerrain Selected_Terrain_A { get; set; }

        public ClsTerrain Selected_Terrain_B { get; set; }

        public ClsTerrain Selected_Terrain_C { get; set; }

        public CreateTransitions()
        {
            this.Load += new EventHandler(this.TEdit_Load);
            this.ImageTest = false;
            this.iGroups = new ClsTerrainTable();
            this.iSelected = false;
            this.iTransition = new Transition.Transition();
            this.iTransitionTable = new TransitionTable();
            InitializeComponent();
        }

        private void TEdit_Load(object sender, EventArgs e)
        {
            this.iGroups.Load();
            this.iGroups.Display(this.GroupSelect);
        }

        private void GroupSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.iSelectedGroup = (ClsTerrain)this.GroupSelect.SelectedItem;
            this.PictureBox3.Image = Art.GetLand(iSelectedGroup.TileID);
            this.Box_TileID.Text = StringType.FromInteger(iSelectedGroup.TileID);
            this.Box_TileID_Hex.Text = string.Format("{0:X4}", iSelectedGroup.TileID);
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics1 = e.Graphics;
            this.LandImage.Image = null;
            this.StaticImage.Image = null;
            this.Box_Description.Text = this.iTransition.Description;
            this.Lbl_HashKey.Text = this.iTransition.HashKey;
            this.BoxFileName.Text = this.iTransition.File;
            this.iTransition.GetStaticTiles.Display(this.StaticTileList);
            this.iTransition.GetMapTiles.Display(this.MapTileList);
            graphics1.Clear(Color.LightGray);
            Graphics graphics2 = graphics1;
            Bitmap land1 = Art.GetLand(iGroups.TerrianGroup(iTransition.GetKey(0)).TileID);
            Point point1 = new(61, 15);
            Point point2 = point1;
            graphics2.DrawImage(land1, point2);
            Graphics graphics3 = graphics1;
            Bitmap land2 = Art.GetLand(iGroups.TerrianGroup(iTransition.GetKey(1)).TileID);
            point1 = new Point(84, 38);
            Point point3 = point1;
            graphics3.DrawImage(land2, point3);
            Graphics graphics4 = graphics1;
            Bitmap land3 = Art.GetLand(iGroups.TerrianGroup(iTransition.GetKey(2)).TileID);
            point1 = new Point(107, 61);
            Point point4 = point1;
            graphics4.DrawImage(land3, point4);
            Graphics graphics5 = graphics1;
            Bitmap land4 = Art.GetLand(iGroups.TerrianGroup(iTransition.GetKey(3)).TileID);
            point1 = new Point(38, 38);
            Point point5 = point1;
            graphics5.DrawImage(land4, point5);
            if (this.ImageTest)
            {
                Graphics graphics6 = graphics1;
                Bitmap land5 = Art.GetLand(IntegerType.FromString(this.Map_TileID.Text));
                point1 = new Point(61, 61);
                Point point6 = point1;
                graphics6.DrawImage(land5, point6);
            }
            else
            {
                Graphics graphics6 = graphics1;
                Bitmap land5 = Art.GetLand(iGroups.TerrianGroup(iTransition.GetKey(4)).TileID);
                point1 = new Point(61, 61);
                Point point6 = point1;
                graphics6.DrawImage(land5, point6);
            }
            Graphics graphics7 = graphics1;
            Bitmap land6 = Art.GetLand(iGroups.TerrianGroup(iTransition.GetKey(5)).TileID);
            point1 = new Point(84, 84);
            Point point7 = point1;
            graphics7.DrawImage(land6, point7);
            Graphics graphics8 = graphics1;
            Bitmap land7 = Art.GetLand(iGroups.TerrianGroup(iTransition.GetKey(6)).TileID);
            point1 = new Point(15, 61);
            Point point8 = point1;
            graphics8.DrawImage(land7, point8);
            Graphics graphics9 = graphics1;
            Bitmap land8 = Art.GetLand(iGroups.TerrianGroup(iTransition.GetKey(7)).TileID);
            point1 = new Point(38, 84);
            Point point9 = point1;
            graphics9.DrawImage(land8, point9);
            Graphics graphics10 = graphics1;
            Bitmap land9 = Art.GetLand(iGroups.TerrianGroup(iTransition.GetKey(8)).TileID);
            point1 = new Point(61, 107);
            Point point10 = point1;
            graphics10.DrawImage(land9, point10);
            Graphics graphics11 = graphics1;
            Pen pen1 = new(Color.Red, 1f);
            point1 = new Point(82, 60);
            Point pt1_1 = point1;
            Point point11 = new(60, 82);
            Point pt2_1 = point11;
            graphics11.DrawLine(pen1, pt1_1, pt2_1);
            Graphics graphics12 = graphics1;
            Pen pen2 = new(Color.Red, 1f);
            point11 = new Point(60, 83);
            Point pt1_2 = point11;
            point1 = new Point(82, 105);
            Point pt2_2 = point1;
            graphics12.DrawLine(pen2, pt1_2, pt2_2);
            Graphics graphics13 = graphics1;
            Pen pen3 = new(Color.Red, 1f);
            point11 = new Point(83, 105);
            Point pt1_3 = point11;
            point1 = new Point(105, 83);
            Point pt2_3 = point1;
            graphics13.DrawLine(pen3, pt1_3, pt2_3);
            Graphics graphics14 = graphics1;
            Pen pen4 = new(Color.Red, 1f);
            point11 = new Point(105, 82);
            Point pt1_4 = point11;
            point1 = new Point(83, 60);
            Point pt2_4 = point1;
            graphics14.DrawLine(pen4, pt1_4, pt2_4);
        }

        #region private void ToolBar1_ButtonClick(object sender, EventArgs e)

        #region Original Code Doesn't Work
        /*
        private void ToolBar1_ButtonClick(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            ClsTerrain clsTerrain = (ClsTerrain)this.GroupSelect.SelectedItem;
            if (clsTerrain == null)
                return;
            this.iTransition.SetHashKey(IntegerType.FromObject(button.Tag), checked((byte)clsTerrain.GroupID));
            this.PictureBox1.Refresh();
        }
        */
        #endregion

        private void ToolBarButton1_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            ClsTerrain clsTerrain = (ClsTerrain)this.GroupSelect.SelectedItem;
            if (clsTerrain == null)
                return;
            this.iTransition.SetHashKey(IntegerType.FromObject(button.Tag), checked((byte)clsTerrain.GroupID));
            this.PictureBox1.Refresh();
        }

        private void ToolBarButton2_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            ClsTerrain clsTerrain = (ClsTerrain)this.GroupSelect.SelectedItem;
            if (clsTerrain == null)
                return;
            this.iTransition.SetHashKey(IntegerType.FromObject(button.Tag), checked((byte)clsTerrain.GroupID));
            this.PictureBox1.Refresh();
        }

        private void ToolBarButton3_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            ClsTerrain clsTerrain = (ClsTerrain)this.GroupSelect.SelectedItem;
            if (clsTerrain == null)
                return;
            this.iTransition.SetHashKey(IntegerType.FromObject(button.Tag), checked((byte)clsTerrain.GroupID));
            this.PictureBox1.Refresh();
        }

        private void ToolBarButton4_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            ClsTerrain clsTerrain = (ClsTerrain)this.GroupSelect.SelectedItem;
            if (clsTerrain == null)
                return;
            this.iTransition.SetHashKey(IntegerType.FromObject(button.Tag), checked((byte)clsTerrain.GroupID));
            this.PictureBox1.Refresh();
        }

        private void ToolBarButton5_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            ClsTerrain clsTerrain = (ClsTerrain)this.GroupSelect.SelectedItem;
            if (clsTerrain == null)
                return;
            this.iTransition.SetHashKey(IntegerType.FromObject(button.Tag), checked((byte)clsTerrain.GroupID));
            this.PictureBox1.Refresh();
        }

        private void ToolBarButton6_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            ClsTerrain clsTerrain = (ClsTerrain)this.GroupSelect.SelectedItem;
            if (clsTerrain == null)
                return;
            this.iTransition.SetHashKey(IntegerType.FromObject(button.Tag), checked((byte)clsTerrain.GroupID));
            this.PictureBox1.Refresh();
        }

        private void ToolBarButton7_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            ClsTerrain clsTerrain = (ClsTerrain)this.GroupSelect.SelectedItem;
            if (clsTerrain == null)
                return;
            this.iTransition.SetHashKey(IntegerType.FromObject(button.Tag), checked((byte)clsTerrain.GroupID));
            this.PictureBox1.Refresh();
        }

        private void ToolBarButton8_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            ClsTerrain clsTerrain = (ClsTerrain)this.GroupSelect.SelectedItem;
            if (clsTerrain == null)
                return;
            this.iTransition.SetHashKey(IntegerType.FromObject(button.Tag), checked((byte)clsTerrain.GroupID));
            this.PictureBox1.Refresh();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            ClsTerrain clsTerrain = (ClsTerrain)this.GroupSelect.SelectedItem;
            if (clsTerrain == null)
                return;
            this.iTransition.SetHashKey(IntegerType.FromObject(button.Tag), checked((byte)clsTerrain.GroupID));
            this.PictureBox1.Refresh();
        }

        #endregion

        private void Btn_Land_Click(object sender, EventArgs e)
        {
            try
            {
                int index = IntegerType.FromString(this.Map_TileID.Text);
                this.LandItems.Value = index;
                if (Art.GetLand(index) == null)
                    this.LandImage.Image = null;
                else
                    this.LandImage.Image = Art.GetLand(index);
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                int num = (int)Interaction.MsgBox(this.ErrorMessage(this.Map_TileID.Text, "1", "16535"), MsgBoxStyle.OkOnly, null);
                ProjectData.ClearProjectError();
            }
        }

        private void Btn_Land_Hex_Click(object sender, EventArgs e)
        {
            try
            {
                int index = IntegerType.FromString(this.Map_TileID_Hex.Text);
                this.LandItems.Value = index;
                this.Map_TileID.Text = index.ToString();
                if (Art.GetLand(index) == null)
                    this.LandImage.Image = null;
                else
                    this.LandImage.Image = Art.GetLand(index);
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                int num = (int)Interaction.MsgBox(this.ErrorMessage(this.Map_TileID_Hex.Text, "&H0000", "&H3FFF"), MsgBoxStyle.OkOnly, null);
                ProjectData.ClearProjectError();
            }
        }

        private void Btn_Static_Click(object sender, EventArgs e)
        {
            try
            {
                int index = IntegerType.FromString(this.Static_TileID.Text);
                this.StaticItems.Value = index;
                if (Art.GetStatic(index) == null)
                    this.StaticImage.Image = null;
                else
                    this.StaticImage.Image = Art.GetStatic(index);
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                int num = (int)Interaction.MsgBox(this.ErrorMessage(this.Static_TileID.Text, "1", "16535"), MsgBoxStyle.OkOnly, null);
                ProjectData.ClearProjectError();
            }
        }

        private void Btn_Static_Hex_Click(object sender, EventArgs e)
        {
            try
            {
                int index = IntegerType.FromString(this.Static_TileID_Hex.Text);
                this.StaticItems.Value = index;
                this.Static_TileID.Text = index.ToString();
                if (Art.GetStatic(index) == null)
                    this.StaticImage.Image = null;
                else
                    this.StaticImage.Image = Art.GetStatic(index);
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                int num = (int)Interaction.MsgBox(this.ErrorMessage(this.Static_TileID_Hex.Text, "&H0000", "&H3FFF"), MsgBoxStyle.OkOnly, null);
                ProjectData.ClearProjectError();
            }
        }

        private string ErrorMessage(string iValue, string iMin, string iMax)
        {
            return string.Format("{0} is outside the range\r\nPlease enter a value between {1} and {2}", iValue, iMin, iMax);
        }

        private void LandItems_Scroll(object sender, ScrollEventArgs e)
        {
            this.Map_TileID.Text = this.LandItems.Value.ToString();
            this.Map_TileID_Hex.Text = "&H" + Conversion.Hex(this.LandItems.Value);
            if (Art.GetLand(this.LandItems.Value) == null)
                this.LandImage.Image = null;
            else
                this.LandImage.Image = Art.GetLand(this.LandItems.Value);
        }

        private void LandItems_ValueChanged(object sender, EventArgs e)
        {
            this.Map_TileID.Text = this.LandItems.Value.ToString();
            this.Map_TileID_Hex.Text = "&H" + Conversion.Hex(this.LandItems.Value);
            if (Art.GetLand(this.LandItems.Value) == null)
                this.LandImage.Image = null;
            else
                this.LandImage.Image = Art.GetLand(this.LandItems.Value);
        }

        private void StaticItems_Scroll(object sender, ScrollEventArgs e)
        {
            this.Static_TileID.Text = this.StaticItems.Value.ToString();
            this.Static_TileID_Hex.Text = "&H" + Conversion.Hex(this.StaticItems.Value);
            if (Art.GetStatic(this.StaticItems.Value) == null)
                this.StaticImage.Image = null;
            else
                this.StaticImage.Image = Art.GetStatic(this.StaticItems.Value);
        }

        private void StaticItems_ValueChanged(object sender, EventArgs e)
        {
            this.Static_TileID.Text = this.StaticItems.Value.ToString();
            this.Static_TileID_Hex.Text = "&H" + Conversion.Hex(this.StaticItems.Value);
            if (Art.GetStatic(this.StaticItems.Value) == null)
                this.StaticImage.Image = null;
            else
                this.StaticImage.Image = Art.GetStatic(this.StaticItems.Value);
        }

        #region private void MapToolBar_ButtonClick(object sender, EventArgs e)

        #region Original Code Doesn't Work
        /*
        private void MapToolBar_ButtonClick(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            object tag = button.Tag;
            if (ObjectType.ObjTst(tag, (object)"Add", false) == 0)
            {
                if (StringType.StrCmp(this.Map_TileID.Text, string.Empty, false) == 0)
                    return;
                this.iTransition.AddMapTile(ShortType.FromString(this.Map_TileID.Text), Convert.ToInt16(this.Map_AltIDMod.Value));
                this.iTransition.GetMapTiles.Display(this.MapTileList);
            }
            else if (ObjectType.ObjTst(tag, (object)"Delete", false) == 0)
            {
                MapTile iMapTile = (MapTile)this.MapTileList.SelectedItem;
                if (iMapTile == null)
                    return;
                this.LandImage.Image = (Image)null;
                this.iTransition.RemoveMapTile(iMapTile);
                this.iTransition.GetMapTiles.Display(this.MapTileList);
            }
            else if (ObjectType.ObjTst(tag, (object)"Test", false) == 0)
            {
                if (StringType.StrCmp(this.Map_TileID.Text, string.Empty, false) == 0)
                    return;
                this.ImageTest = !this.ImageTest;
                this.PictureBox1.Refresh();
                this.LandImage.Image = Art.GetLand(IntegerType.FromString(this.Map_TileID.Text)) != null ? (Image)Art.GetLand(IntegerType.FromString(this.Map_TileID.Text)) : (Image)null;
            }
            else if (ObjectType.ObjTst(tag, (object)"Select", false) == 0)
            {
                MapZoom mapZoom = new MapZoom();
                mapZoom.Tag = (object)this.LandItems;
                mapZoom.Show();
            }
        }
        */
        #endregion

        private void ToolBarButton10_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            object tag = button.Tag;
            if (ObjectType.ObjTst(tag, "Add", false) == 0)
            {
                if (StringType.StrCmp(this.Map_TileID.Text, string.Empty, false) == 0)
                    return;
                this.iTransition.AddMapTile(ShortType.FromString(this.Map_TileID.Text), Convert.ToInt16(this.Map_AltIDMod.Value));
                this.iTransition.GetMapTiles.Display(this.MapTileList);
            }
        }

        private void ToolBarButton11_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            object tag = button.Tag;
            if (ObjectType.ObjTst(tag, "Delete", false) == 0)
            {
                MapTile iMapTile = (MapTile)this.MapTileList.SelectedItem;
                if (iMapTile == null)
                    return;
                this.LandImage.Image = null;
                this.iTransition.RemoveMapTile(iMapTile);
                this.iTransition.GetMapTiles.Display(this.MapTileList);
            }
        }

        private void ToolBarButton12_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            object tag = button.Tag;
            if (ObjectType.ObjTst(tag, "Test", false) == 0)
            {
                if (StringType.StrCmp(this.Map_TileID.Text, string.Empty, false) == 0)
                    return;
                this.ImageTest = !this.ImageTest;
                this.PictureBox1.Refresh();
                this.LandImage.Image = Art.GetLand(IntegerType.FromString(this.Map_TileID.Text)) != null ? (Image)Art.GetLand(IntegerType.FromString(this.Map_TileID.Text)) : null;
            }
        }

        private void ToolBarButton13_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            object tag = button.Tag;
            if (ObjectType.ObjTst(tag, "Select", false) == 0)
            {
                MapZoom mapZoom = new();
                mapZoom.Tag = LandItems;
                mapZoom.Show();
            }
        }

        #endregion


        #region private void StaticToolBar_ButtonClick(object sender, EventArgs e)

        #region Original Code Doesn't Work
        /*
        private void StaticToolBar_ButtonClick(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            object tag = button.Tag;
            if (ObjectType.ObjTst(tag, (object)"Add", false) == 0)
            {
                if (StringType.StrCmp(this.Static_TileID.Text, string.Empty, false) == 0)
                    return;
                this.iTransition.AddStaticTile(ShortType.FromString(this.Static_TileID.Text), Convert.ToInt16(this.Static_AltIDMod.Value));
                this.iTransition.GetStaticTiles.Display(this.StaticTileList);
            }
            else if (ObjectType.ObjTst(tag, (object)"Delete", false) == 0)
            {
                Transition.StaticTile iStaticTile = (Transition.StaticTile)this.StaticTileList.SelectedItem;
                if (iStaticTile == null)
                    return;
                this.StaticImage.Image = (Image)null;
                this.iTransition.RemoveStaticTile(iStaticTile);
                this.iTransition.GetStaticTiles.Display(this.StaticTileList);
            }
            else if (ObjectType.ObjTst(tag, (object)"Select", false) == 0)
            {
                StaticZoom staticZoom = new StaticZoom();
                staticZoom.Tag = (object)this.StaticItems;
                staticZoom.Show();
            }
        }
        */
        #endregion

        private void ToolBarButton14_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            object tag = button.Tag;
            if (ObjectType.ObjTst(tag, "Add", false) == 0)
            {
                if (StringType.StrCmp(this.Static_TileID.Text, string.Empty, false) == 0)
                    return;
                this.iTransition.AddStaticTile(ShortType.FromString(this.Static_TileID.Text), Convert.ToInt16(this.Static_AltIDMod.Value));
                this.iTransition.GetStaticTiles.Display(this.StaticTileList);
            }
        }

        private void ToolBarButton15_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            object tag = button.Tag;
            if (ObjectType.ObjTst(tag, "Delete", false) == 0)
            {
                Transition.StaticTile iStaticTile = (Transition.StaticTile)this.StaticTileList.SelectedItem;
                if (iStaticTile == null)
                    return;
                this.StaticImage.Image = null;
                this.iTransition.RemoveStaticTile(iStaticTile);
                this.iTransition.GetStaticTiles.Display(this.StaticTileList);
            }
        }

        private void ToolBarButton16_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            object tag = button.Tag;
            if (ObjectType.ObjTst(tag, "Select", false) == 0)
            {
                StaticZoom staticZoom = new();
                staticZoom.Tag = StaticItems;
                staticZoom.Show();
            }
        }

        #endregion

        private void MenuTerrainA_Click(object sender, EventArgs e)
        {
            GroupSelect groupSelect = new();
            groupSelect.SelectGroupName.Text = "Select Group A";
            groupSelect.Tag = this;
            groupSelect.Show();
        }

        private void MenuTerrainB_Click(object sender, EventArgs e)
        {
            GroupSelect groupSelect = new();
            groupSelect.SelectGroupName.Text = "Select Group B";
            groupSelect.Tag = this;
            groupSelect.Show();
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Transition.Transition transition = (Transition.Transition)this.ListBox1.SelectedItem;
            if (transition == null)
                return;
            this.iTransition = transition;
            this.PictureBox1.Refresh();
        }

        private void MenuNew_Click(object sender, EventArgs e)
        {
            this.iTransitionTable.Clear();
            this.iTransitionTable.Display(this.ListBox1);
            this.iTransition = new Transition.Transition();
            this.PictureBox1.Refresh();
        }

        private void MenuLoad_Click(object sender, EventArgs e)
        {
            this.iTransitionTable.Clear();
            OpenFileDialog openFileDialog = new();
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            this.iTransitionTable.Load(openFileDialog.FileName);
            this.iTransitionTable.Display(this.ListBox1);
        }

        private void MenuSave_Click(object sender, EventArgs e)
        {
            this.iTransitionTable.Save(this.Box_Description.Text);
        }

        private void MapTileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            MapTile mapTile = (MapTile)this.MapTileList.SelectedItem;
            if (mapTile == null)
                return;
            this.Map_TileID.Text = mapTile.TileID.ToString();
            this.Map_TileID_Hex.Text = "&H" + Conversion.Hex(mapTile.TileID);
            this.LandItems.Value = mapTile.TileID;
            this.LandImage.Image = Art.GetLand(mapTile.TileID) != null ? (Image)Art.GetLand(mapTile.TileID) : null;
        }

        private void StaticTileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Transition.StaticTile staticTile = (Transition.StaticTile)this.StaticTileList.SelectedItem;
            if (staticTile == null)
                return;
            this.Static_TileID.Text = staticTile.TileID.ToString();
            this.Static_TileID_Hex.Text = "&H" + Conversion.Hex(staticTile.TileID);
            this.StaticItems.Value = staticTile.TileID;
            this.StaticImage.Image = Art.GetStatic(staticTile.TileID) != null ? (Image)Art.GetStatic(staticTile.TileID) : null;
        }

        private void MenuAddKey_Click(object sender, EventArgs e)
        {
            this.iTransitionTable.Add(this.iTransition);
            this.iTransition = new Transition.Transition();
            this.iTransitionTable.Display(this.ListBox1);
            this.PictureBox1.Refresh();
        }

        private void MenuDelKey_Click(object sender, EventArgs e)
        {
            Transition.Transition iValue = (Transition.Transition)this.ListBox1.SelectedItem;
            if (iValue == null)
                return;
            this.iTransitionTable.Remove(iValue);
            this.iTransitionTable.Display(this.ListBox1);
        }

        private void MenuCopyKey_Click(object sender, EventArgs e)
        {
            this.iTransitionTable.Add(this.iTransition);
            this.iTransitionTable.Display(this.ListBox1);
            this.PictureBox1.Refresh();
        }

        private void MenuItem1_Click(object sender, EventArgs e)
        {
            this.iTransition = new Transition.Transition();
            this.PictureBox1.Refresh();
        }

        private void BoxFileName_TextChanged(object sender, EventArgs e)
        {
            this.iTransition.File = this.BoxFileName.Text;
            this.iTransitionTable.Display(this.ListBox1);
        }

        private void Btn_StaticFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Data/Statics";
            openFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            this.BoxFileName.Text = new FileInfo(openFileDialog.FileName).Name;
        }

        private void Box_Description_Leave(object sender, EventArgs e)
        {
            this.iTransition.Description = this.Box_Description.Text;
            this.iTransitionTable.Display(this.ListBox1);
        }

        private void Menu2Way_Click(object sender, EventArgs e)
        {
            if (this.Selected_Terrain_A == null || this.Selected_Terrain_B == null || StringType.StrCmp(this.Selected_Terrain_A.Name, this.Selected_Terrain_B.Name, false) == 0)
                return;
            string iDescription = string.Format("{0} To {1}", Selected_Terrain_A.Name, Selected_Terrain_B.Name);
            string filename = string.Format("{0}Data/System/2Way_Template.xml", AppDomain.CurrentDomain.BaseDirectory);
            XmlDocument xmlDocument = new();
            this.iTransitionTable.Clear();
            try
            {
                xmlDocument.Load(filename);
                try
                {
                    foreach (XmlElement xmlElement in xmlDocument.SelectNodes("//Wizard/Tile"))
                    {
                        string attribute = xmlElement.GetAttribute("Pattern");
                        this.iTransitionTable.Add(new Transition.Transition(iDescription, this.Selected_Terrain_A, this.Selected_Terrain_B, attribute));
                    }
                }
                finally
                {
                    IEnumerator enumerator = null;
                    if (enumerator is IDisposable)
                        ((IDisposable)enumerator).Dispose();
                }
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                int num = (int)Interaction.MsgBox(string.Format("XMLFile:{0}", filename), MsgBoxStyle.OkOnly, null);
                ProjectData.ClearProjectError();
            }
            this.iTransitionTable.Display(this.ListBox1);
        }

        private void Menu3Way_Click(object sender, EventArgs e)
        {
            if (this.Selected_Terrain_A == null || this.Selected_Terrain_B == null || (this.Selected_Terrain_C == null || StringType.StrCmp(this.Selected_Terrain_A.Name, this.Selected_Terrain_B.Name, false) == 0))
                return;
            string iDescription = string.Format("{0}-{1}-{2}", Selected_Terrain_A.Name, Selected_Terrain_B.Name, Selected_Terrain_C.Name);
            string filename = string.Format("{0}Data/System/3Way_Template.xml", AppDomain.CurrentDomain.BaseDirectory);
            XmlDocument xmlDocument = new();
            this.iTransitionTable.Clear();
            try
            {
                xmlDocument.Load(filename);
                try
                {
                    foreach (XmlElement xmlElement in xmlDocument.SelectNodes("//Wizard/Tile"))
                    {
                        string attribute = xmlElement.GetAttribute("Pattern");
                        this.iTransitionTable.Add(new Transition.Transition(iDescription, this.Selected_Terrain_A, this.Selected_Terrain_B, this.Selected_Terrain_C, attribute));
                    }
                }
                finally
                {
                    IEnumerator enumerator = null;
                    if (enumerator is IDisposable)
                        ((IDisposable)enumerator).Dispose();
                }
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                int num = (int)Interaction.MsgBox(string.Format("XMLFile:{0}", filename), MsgBoxStyle.OkOnly, null);
                ProjectData.ClearProjectError();
            }
            this.iTransitionTable.Display(this.ListBox1);
        }

        private void MenuTerrainC_Click(object sender, EventArgs e)
        {
            GroupSelect groupSelect = new();
            groupSelect.SelectGroupName.Text = "Select Group C";
            groupSelect.Tag = this;
            groupSelect.Show();
        }

        private void MenuItem3_Click(object sender, EventArgs e)
        {
            PrintTransition printTransition = new();
        }

        private void Menu_CloneGroupA_Click(object sender, EventArgs e)
        {
            GroupSelect groupSelect = new();
            groupSelect.SelectGroupName.Text = "Clone Group A";
            groupSelect.Tag = this;
            groupSelect.Show();
        }

        private void Menu_CloneGroupB_Click(object sender, EventArgs e)
        {
            GroupSelect groupSelect = new();
            groupSelect.SelectGroupName.Text = "Clone Group B";
            groupSelect.Tag = this;
            groupSelect.Show();
        }

        private void MenuItem10_Click(object sender, EventArgs e)
        {
            if (this.Selected_Terrain_A == null || this.Selected_Terrain_B == null)
                return;
            this.iTransitionTable.Clear();
            OpenFileDialog openFileDialog = new();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                this.iTransitionTable.Load(openFileDialog.FileName);
            try
            {
                foreach (Transition.Transition transition in (IEnumerable)this.iTransitionTable.GetTransitionTable.Values)
                    transition.Clone(this.Selected_Terrain_A, this.Selected_Terrain_B);
            }
            finally
            {
                IEnumerator enumerator = null;
                if (enumerator is IDisposable)
                    ((IDisposable)enumerator).Dispose();
            }
            this.iTransitionTable.Save(openFileDialog.FileName.Replace(this.Selected_Terrain_A.Name, this.Selected_Terrain_B.Name));
        }

        private void launchTransitionWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form TransitionWizard = new TransitionWizard.TransitionWizard();
            TransitionWizard.Show();
        }
    }
}


