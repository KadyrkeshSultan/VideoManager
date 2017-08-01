using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;

namespace VMMapEngine
{
    public class VMMap : UserControl
    {
        private IContainer components;
        private Panel panel1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblGps;
        private GMapControl gMap;
        private vTrackBar zoomBar;
        private vComboBox cboProvider;
        private ToolStripStatusLabel lblVersion;
        private vButton btnSave;
        private TableLayoutPanel tableLayoutPanel1;
        private bool isMouseDown;

        public GMapControl GetMapControl
        {
            
            get
            {
                return this.gMap;
            }
        }

        
        public VMMap()
        {
            InitializeComponent();
            isMouseDown = false;
            InitMap();
            lblVersion.Alignment = ToolStripItemAlignment.Right;
            lblVersion.Text = string.Format("Ver {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnSave, "Save image of map to file");
            toolTip.SetToolTip(zoomBar, "Map zoom level");
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        
        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.btnSave = new vButton();
            this.zoomBar = new vTrackBar();
            this.cboProvider = new vComboBox();
            this.statusStrip1 = new StatusStrip();
            this.lblGps = new ToolStripStatusLabel();
            this.lblVersion = new ToolStripStatusLabel();
            this.gMap = new GMapControl();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            base.SuspendLayout();
            this.panel1.BackColor = Color.Silver;
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(591, 34);
            this.panel1.TabIndex = 0;
            this.btnSave.AllowAnimations = true;
            this.btnSave.BackColor = Color.Transparent;
            this.btnSave.Dock = DockStyle.Fill;
            this.btnSave.Location = new Point(492, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.RoundedCornersMask = 15;
            this.btnSave.Size = new Size(74, 26);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Snapshot";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.VIBlendTheme = VIBLEND_THEME.OFFICE2010BLACK;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            this.zoomBar.BackColor = Color.Transparent;
            this.zoomBar.Dock = DockStyle.Fill;
            this.zoomBar.Location = new Point(133, 3);
            this.zoomBar.Maximum = 17;
            this.zoomBar.Minimum = 1;
            this.zoomBar.Name = "zoomBar";
            this.zoomBar.RoundedCornersMask = 15;
            this.zoomBar.RoundedCornersMaskThumb = 15;
            this.zoomBar.Size = new Size(353, 26);
            this.zoomBar.TabIndex = 1;
            this.zoomBar.Value = 9;
            this.zoomBar.VIBlendTheme = VIBLEND_THEME.OFFICE2010BLACK;
            this.zoomBar.Scroll += new ScrollEventHandler(this.zoomBar_Scroll);
            this.cboProvider.BackColor = Color.White;
            this.cboProvider.DefaultText = "Select Map...";
            this.cboProvider.DisplayMember = "";
            this.cboProvider.Dock = DockStyle.Fill;
            this.cboProvider.DropDownList = true;
            this.cboProvider.DropDownMaximumSize = new Size(1000, 1000);
            this.cboProvider.DropDownMinimumSize = new Size(10, 10);
            this.cboProvider.DropDownResizeDirection = SizingDirection.Both;
            this.cboProvider.DropDownWidth = 124;
            this.cboProvider.Location = new Point(3, 3);
            this.cboProvider.Name = "cboProvider";
            this.cboProvider.RoundedCornersMaskListItem = 15;
            this.cboProvider.Size = new Size(124, 26);
            this.cboProvider.TabIndex = 0;
            this.cboProvider.UseThemeBackColor = false;
            this.cboProvider.UseThemeDropDownArrowColor = true;
            this.cboProvider.ValueMember = "";
            this.cboProvider.VIBlendScrollBarsTheme = VIBLEND_THEME.OFFICE2010BLACK;
            this.cboProvider.VIBlendTheme = VIBLEND_THEME.OFFICE2010BLACK;
            this.cboProvider.DropDownClose += new EventHandler(this.cboProvider_DropDownClose);
            this.statusStrip1.BackColor = Color.White;
            ToolStripItemCollection items = this.statusStrip1.Items;
            ToolStripItem[] toolStripItemArray = new ToolStripItem[] { this.lblGps, this.lblVersion };
            this.statusStrip1.Items.AddRange(toolStripItemArray);
            this.statusStrip1.Location = new Point(0, 561);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(591, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.lblGps.AutoSize = false;
            this.lblGps.Name = "lblGps";
            this.lblGps.Size = new Size(120, 17);
            this.lblGps.Text = "0.0,0.0";
            this.lblGps.TextAlign = ContentAlignment.MiddleLeft;
            this.lblVersion.AutoSize = false;
            this.lblVersion.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.lblVersion.Font = new Font("Segoe UI", 9f, FontStyle.Italic, GraphicsUnit.Point, 0);
            this.lblVersion.ForeColor = Color.Silver;
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new Size(100, 17);
            this.lblVersion.Text = "Version";
            this.lblVersion.TextAlign = ContentAlignment.MiddleLeft;
            this.gMap.Bearing = 0f;
            this.gMap.CanDragMap = true;
            this.gMap.Dock = DockStyle.Fill;
            this.gMap.GrayScaleMode = false;
            this.gMap.LevelsKeepInMemmory = 5;
            this.gMap.Location = new Point(0, 34);
            this.gMap.MarkersEnabled = true;
            this.gMap.MaxZoom = 2;
            this.gMap.MinZoom = 2;
            this.gMap.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            this.gMap.Name = "gMap";
            this.gMap.NegativeMode = false;
            this.gMap.PolygonsEnabled = true;
            this.gMap.RetryLoadTile = 0;
            this.gMap.RoutesEnabled = true;
            this.gMap.ShowTileGridLines = false;
            this.gMap.Size = new Size(591, 527);
            this.gMap.TabIndex = 2;
            this.gMap.Zoom = 0;
            this.gMap.Paint += new PaintEventHandler(this.gMap_Paint);
            this.gMap.MouseClick += new MouseEventHandler(this.gMap_MouseClick);
            this.gMap.MouseDown += new MouseEventHandler(this.gMap_MouseDown);
            this.gMap.MouseMove += new MouseEventHandler(this.gMap_MouseMove);
            this.gMap.MouseUp += new MouseEventHandler(this.gMap_MouseUp);
            this.tableLayoutPanel1.BackColor = Color.Gray;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130f));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80f));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
            this.tableLayoutPanel1.Controls.Add(this.cboProvider, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.zoomBar, 1, 0);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.Size = new Size(589, 32);
            this.tableLayoutPanel1.TabIndex = 3;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.gMap);
            base.Controls.Add(this.statusStrip1);
            base.Controls.Add(this.panel1);
            base.Name = "C3Map";
            base.Size = new Size(591, 583);
            this.panel1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        
        private void InitMap()
        {
            gMap.MapProvider = GMapProviders.OpenStreetMap;
            gMap.SetCurrentPositionByKeywords("USA");
            gMap.MinZoom = 0;
            gMap.MaxZoom = 17;
            gMap.Zoom = 4.0;
            zoomBar.Minimum = 0;
            zoomBar.Maximum = this.gMap.MaxZoom;
            zoomBar.Value = (int)gMap.Zoom;
            cboProvider.DataSource = GMapProviders.List;
            cboProvider.SelectedItem = new ListItem()
            {
                Text = GMapProviders.OpenStreetMap.ToString(),
                Tag = GMapProviders.OpenStreetMap
            };
        }

        
        private void gMap_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        
        private void gMap_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            int button1 = (int)e.Button;
            int button2 = (int)e.Button;
        }

        
        private void gMap_MouseClick(object sender, MouseEventArgs e)
        {
            int button1 = (int)e.Button;
            int button2 = (int)e.Button;
        }

        
        private void gMap_Paint(object sender, PaintEventArgs e)
        {
            zoomBar.Value = (int)gMap.Zoom;
        }

        
        private void gMap_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Point location = e.Location;
                PointLatLng latLng = gMap.FromLocalToLatLng(location.X, location.Y);
                lblGps.Text = string.Format("{0:0.000000},{1:0.000000}", latLng.Lat, latLng.Lng);
            }
            catch (Exception ex)
            {
            }
        }

        
        private void zoomBar_Scroll(object sender, ScrollEventArgs e)
        {
            this.gMap.Zoom = (double)this.zoomBar.Value;
        }

        
        private void cboProvider_DropDownClose(object sender, EventArgs e)
        {
            gMap.MapProvider = (GMapProvider)cboProvider.SelectedValue;
        }

        
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    DateTime now = DateTime.Now;
                    saveFileDialog.Filter = "PNG (*.png)|*.png";
                    saveFileDialog.FileName = string.Format("Map{0}{1:00}{2:00}.{3:00}{4:00}{5:00}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
                    Image image = gMap.ToImage();
                    if (image == null)
                        return;
                    using (image)
                    {
                        if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
                            return;
                        image.Save(saveFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        
        public void HideSnapshot(bool b)
        {
            btnSave.Visible = b;
        }
    }
}
