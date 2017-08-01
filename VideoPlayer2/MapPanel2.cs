using VMMapEngine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VideoPlayer2.Properties;

namespace VideoPlayer2
{
    public class MapPanel2 : UserControl
    {
        private IContainer components;
        private TableLayoutPanel tableLayoutPanel1;
        private vCheckBox chk_MapCompass;
        private VMMap VMMap;

        public event DEL_Compass EVT_Compass;

        public MapPanel2()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
        }

        
        private void MapPanel_Load(object sender, EventArgs e)
        {
            chk_MapCompass.Text = LangCtrl.GetString("chk_MapCompass", "Map Compass");
        }

        
        public VMMap GetMapObject()
        {
            return VMMap;
        }

        
        private void chk_MapCompass_CheckedChanged(object sender, EventArgs e)
        {
            Callback(chk_MapCompass.Checked);
        }

        
        private void Callback(bool b)
        {
            if (EVT_Compass == null)
                return;
            EVT_Compass(b);
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.chk_MapCompass = new vCheckBox();
            this.VMMap = new VMMap();
            this.tableLayoutPanel1.SuspendLayout();
            base.SuspendLayout();
            this.tableLayoutPanel1.BackgroundImage = Properties.Resources.nogps;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.Controls.Add(this.chk_MapCompass, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.VMMap, 0, 0);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Margin = new Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
            this.tableLayoutPanel1.Size = new Size(334, 460);
            this.tableLayoutPanel1.TabIndex = 0;
            this.chk_MapCompass.BackColor = Color.Transparent;
            this.chk_MapCompass.Dock = DockStyle.Fill;
            this.chk_MapCompass.Location = new Point(3, 441);
            this.chk_MapCompass.Name = "chk_MapCompass";
            this.chk_MapCompass.Size = new Size(328, 16);
            this.chk_MapCompass.TabIndex = 0;
            this.chk_MapCompass.Text = "Map Compass";
            this.chk_MapCompass.UseVisualStyleBackColor = false;
            this.chk_MapCompass.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.chk_MapCompass.CheckedChanged += new EventHandler(this.chk_MapCompass_CheckedChanged);
            this.VMMap.Dock = DockStyle.Fill;
            this.VMMap.Location = new Point(3, 3);
            this.VMMap.Name = "VMMap";
            this.VMMap.Size = new Size(328, 432);
            this.VMMap.TabIndex = 1;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.Controls.Add(this.tableLayoutPanel1);
            base.Margin = new Padding(0);
            base.Name = "MapPanel2";
            base.Size = new Size(334, 460);
            base.Load += new EventHandler(this.MapPanel_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public delegate void DEL_Compass(bool b);
    }
}
