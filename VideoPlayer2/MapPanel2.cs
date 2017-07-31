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
            chk_MapCompass.Text = LangCtrl.GetString("VideoPlay_MapPanel_1", "VideoPlay_MapPanel_2");
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
            this.SuspendLayout();
            this.tableLayoutPanel1.BackgroundImage = (Image)Properties.Resources.nogps;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.Controls.Add((Control)this.chk_MapCompass, 0, 1);
            this.tableLayoutPanel1.Controls.Add((Control)this.VMMap, 0, 0);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Margin = new Padding(0);
            this.tableLayoutPanel1.Name = "VideoPlay_MapPanel_3";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
            this.tableLayoutPanel1.Size = new Size(334, 460);
            this.tableLayoutPanel1.TabIndex = 0;
            this.chk_MapCompass.BackColor = Color.Transparent;
            this.chk_MapCompass.Dock = DockStyle.Fill;
            this.chk_MapCompass.Location = new Point(3, 441);
            this.chk_MapCompass.Name = "VideoPlay_MapPanel_4";
            this.chk_MapCompass.Size = new Size(328, 16);
            this.chk_MapCompass.TabIndex = 0;
            this.chk_MapCompass.Text = "VideoPlay_MapPanel_5";
            this.chk_MapCompass.UseVisualStyleBackColor = false;
            this.chk_MapCompass.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.chk_MapCompass.CheckedChanged += new EventHandler(this.chk_MapCompass_CheckedChanged);
            this.VMMap.Dock = DockStyle.Fill;
            this.VMMap.Location = new Point(3, 3);
            this.VMMap.Name = "VideoPlay_MapPanel_6";
            this.VMMap.Size = new Size(328, 432);
            this.VMMap.TabIndex = 1;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add((Control)this.tableLayoutPanel1);
            this.Margin = new Padding(0);
            this.Name = "VideoPlay_MapPanel_7";
            this.Size = new Size(334, 460);
            this.Load += new EventHandler(this.MapPanel_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        public delegate void DEL_Compass(bool b);
    }
}
