using Elevation;
using Logger;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Terrain;

namespace CreateElevationBitmap
{
    public partial class CreateElevationBitmap : Form
    {
        private readonly ClsTerrainTable iTerrain;
        private readonly ClsElevationTable iAltitude;
        private readonly LoggerForm iLogger;

        public CreateElevationBitmap()
        {
            CreateElevationBitmap altImagePrep = this;
            base.Load += new EventHandler(altImagePrep.AltImagePrep_Load);
            this.iTerrain = new ClsTerrainTable();
            this.iAltitude = new ClsElevationTable();
            this.iLogger = new LoggerForm();
            InitializeComponent();
        }

        private void MenuPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new()
            {
                SelectedPath = this.ProjectPath.Text
            };
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.ProjectPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private async void MenuMake_Click(object sender, EventArgs e)
        {
            Progress<int> progress = new(i => { ProgressBar1.Value = i; });
            Progress<string> logger = new(i => { iLogger.LogMessage(i); });
            Task resetProgress = new(() => { Thread.Sleep(1000); ((IProgress<int>)progress).Report(0); });
            await Task.Run(() => CreateElevationBitmapHelper.MakeAltitudeImage(ProjectPath.Text, TerrainFile.Text, AltitudeFile.Text, iAltitude, iTerrain, progress, logger)).ContinueWith(c => resetProgress.Start());
        }

        private void AltImagePrep_Load(object sender, EventArgs e)
        {
            this.iLogger.Show();
            int x = checked(this.iLogger.Location.X + 100);
            Point location = this.iLogger.Location;
            Point point = new(x, checked(location.Y + 100));
            this.Location = point;
            this.ProjectPath.Text = Directory.GetCurrentDirectory();
            this.iTerrain.Load();
            this.iAltitude.Load();
        }
    }
}
