using AppGlobal;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Unity;
using VideoPlayer2.Properties;
using VMInterfaces;
using VMModels.Model;

namespace VideoPlayer2
{
    public class FilePanel : UserControl
    {
        private IContainer components;
        private Label fp_OriginalFileName;
        private Label fp_FileExt;
        private Label txtFileName;
        private Label txtFileExt;
        private Label txtFileTimestamp;
        private Label fp_FileTimestamp;
        private Label txtAdded;
        private Label fp_FileAdded;
        private Label txtFileSize;
        private Label fp_FileSize;
        private Label txtCAD;
        private Label fp_CAD;
        private Label txtRMS;
        private Label fp_RMS;
        private Label txtSet;
        private Label fp_SetName;
        private Label txtClassification;
        private Label fp_Classification;
        private Panel HeaderPanel;
        private Label lbl_VideoFileInfo;

        public FilePanel()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
        }

        
        private void FilePanel_Load(object sender, EventArgs e)
        {
            LangCtrl.reText(this);
        }

        
        public void LoadData(Guid Id)
        {
            if (Global.IS_WOLFCOM)
                HeaderPanel.BackgroundImage = (Image)Resources.topbar45;
            using (RPM_DataFile rpmDataFile = new RPM_DataFile())
            {
                DataFile dataFile = rpmDataFile.GetDataFile(Id);
                txtFileTimestamp.Text = dataFile.FileTimestamp.ToString();
                txtFileSize.Text = string.Format("VideoPlay_FilePanel_1", dataFile.FileSize);
                txtFileExt.Text = dataFile.FileExtension;
                txtFileName.Text = dataFile.OriginalFileName;
                txtAdded.Text = dataFile.FileAddedTimestamp.ToString();
                txtCAD.Text = dataFile.CADNumber;
                txtRMS.Text = dataFile.RMSNumber;
                txtSet.Text = dataFile.SetName;
                txtClassification.Text = dataFile.Classification;
            }
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        
        private void InitializeComponent()
        {
            this.fp_OriginalFileName = new Label();
            this.fp_FileExt = new Label();
            this.txtFileName = new Label();
            this.txtFileExt = new Label();
            this.txtFileTimestamp = new Label();
            this.fp_FileTimestamp = new Label();
            this.txtAdded = new Label();
            this.fp_FileAdded = new Label();
            this.txtFileSize = new Label();
            this.fp_FileSize = new Label();
            this.txtCAD = new Label();
            this.fp_CAD = new Label();
            this.txtRMS = new Label();
            this.fp_RMS = new Label();
            this.txtSet = new Label();
            this.fp_SetName = new Label();
            this.txtClassification = new Label();
            this.fp_Classification = new Label();
            this.HeaderPanel = new Panel();
            this.lbl_VideoFileInfo = new Label();
            this.HeaderPanel.SuspendLayout();
            this.SuspendLayout();
            this.fp_OriginalFileName.AutoSize = true;
            this.fp_OriginalFileName.Location = new Point(14, 55);
            this.fp_OriginalFileName.Name = "VideoPlay_FilePanel_2";
            this.fp_OriginalFileName.Size = new Size(92, 13);
            this.fp_OriginalFileName.TabIndex = 0;
            this.fp_OriginalFileName.Text = "VideoPlay_FilePanel_3";
            this.fp_FileExt.AutoSize = true;
            this.fp_FileExt.Location = new Point(14, 75);
            this.fp_FileExt.Name = "VideoPlay_FilePanel_4";
            this.fp_FileExt.Size = new Size(72, 13);
            this.fp_FileExt.TabIndex = 1;
            this.fp_FileExt.Text = "VideoPlay_FilePanel_5";
            this.txtFileName.BorderStyle = BorderStyle.FixedSingle;
            this.txtFileName.Location = new Point(134, 52);
            this.txtFileName.Name = "VideoPlay_FilePanel_6";
            this.txtFileName.Size = new Size(200, 18);
            this.txtFileName.TabIndex = 2;
            this.txtFileName.TextAlign = ContentAlignment.MiddleLeft;
            this.txtFileExt.BorderStyle = BorderStyle.FixedSingle;
            this.txtFileExt.Location = new Point(134, 72);
            this.txtFileExt.Name = "VideoPlay_FilePanel_7";
            this.txtFileExt.Size = new Size(200, 18);
            this.txtFileExt.TabIndex = 3;
            this.txtFileExt.TextAlign = ContentAlignment.MiddleLeft;
            this.txtFileTimestamp.BorderStyle = BorderStyle.FixedSingle;
            this.txtFileTimestamp.Location = new Point(134, 92);
            this.txtFileTimestamp.Name = "VideoPlay_FilePanel_8";
            this.txtFileTimestamp.Size = new Size(200, 18);
            this.txtFileTimestamp.TabIndex = 5;
            this.txtFileTimestamp.TextAlign = ContentAlignment.MiddleLeft;
            this.fp_FileTimestamp.AutoSize = true;
            this.fp_FileTimestamp.Location = new Point(14, 95);
            this.fp_FileTimestamp.Name = "VideoPlay_FilePanel_9";
            this.fp_FileTimestamp.Size = new Size(77, 13);
            this.fp_FileTimestamp.TabIndex = 4;
            this.fp_FileTimestamp.Text = "VideoPlay_FilePanel_10";
            this.txtAdded.BorderStyle = BorderStyle.FixedSingle;
            this.txtAdded.Location = new Point(134, 112);
            this.txtAdded.Name = "VideoPlay_FilePanel_11";
            this.txtAdded.Size = new Size(200, 18);
            this.txtAdded.TabIndex = 7;
            this.txtAdded.TextAlign = ContentAlignment.MiddleLeft;
            this.fp_FileAdded.AutoSize = true;
            this.fp_FileAdded.Location = new Point(14, 115);
            this.fp_FileAdded.Name = "VideoPlay_FilePanel_12";
            this.fp_FileAdded.Size = new Size(57, 13);
            this.fp_FileAdded.TabIndex = 6;
            this.fp_FileAdded.Text = "VideoPlay_FilePanel_13";
            this.txtFileSize.BorderStyle = BorderStyle.FixedSingle;
            this.txtFileSize.Location = new Point(134, 132);
            this.txtFileSize.Name = "VideoPlay_FilePanel_14";
            this.txtFileSize.Size = new Size(200, 18);
            this.txtFileSize.TabIndex = 9;
            this.txtFileSize.TextAlign = ContentAlignment.MiddleLeft;
            this.fp_FileSize.AutoSize = true;
            this.fp_FileSize.Location = new Point(14, 135);
            this.fp_FileSize.Name = "VideoPlay_FilePanel_15";
            this.fp_FileSize.Size = new Size(46, 13);
            this.fp_FileSize.TabIndex = 8;
            this.fp_FileSize.Text = "VideoPlay_FilePanel_16";
            this.txtCAD.BorderStyle = BorderStyle.FixedSingle;
            this.txtCAD.Location = new Point(134, 152);
            this.txtCAD.Name = "VideoPlay_FilePanel_17";
            this.txtCAD.Size = new Size(200, 18);
            this.txtCAD.TabIndex = 11;
            this.txtCAD.TextAlign = ContentAlignment.MiddleLeft;
            this.fp_CAD.AutoSize = true;
            this.fp_CAD.Location = new Point(14, 155);
            this.fp_CAD.Name = "VideoPlay_FilePanel_18";
            this.fp_CAD.Size = new Size(69, 13);
            this.fp_CAD.TabIndex = 10;
            this.fp_CAD.Text = "VideoPlay_FilePanel_19";
            this.txtRMS.BorderStyle = BorderStyle.FixedSingle;
            this.txtRMS.Location = new Point(134, 172);
            this.txtRMS.Name = "VideoPlay_FilePanel_20";
            this.txtRMS.Size = new Size(200, 18);
            this.txtRMS.TabIndex = 13;
            this.txtRMS.TextAlign = ContentAlignment.MiddleLeft;
            this.fp_RMS.AutoSize = true;
            this.fp_RMS.Location = new Point(14, 175);
            this.fp_RMS.Name = "VideoPlay_FilePanel_21";
            this.fp_RMS.Size = new Size(71, 13);
            this.fp_RMS.TabIndex = 12;
            this.fp_RMS.Text = "VideoPlay_FilePanel_22";
            this.txtSet.BorderStyle = BorderStyle.FixedSingle;
            this.txtSet.Location = new Point(134, 192);
            this.txtSet.Name = "VideoPlay_FilePanel_23";
            this.txtSet.Size = new Size(200, 18);
            this.txtSet.TabIndex = 15;
            this.txtSet.TextAlign = ContentAlignment.MiddleLeft;
            this.fp_SetName.AutoSize = true;
            this.fp_SetName.Location = new Point(14, 195);
            this.fp_SetName.Name = "VideoPlay_FilePanel_24";
            this.fp_SetName.Size = new Size(59, 13);
            this.fp_SetName.TabIndex = 14;
            this.fp_SetName.Text = "VideoPlay_FilePanel_25";
            this.txtClassification.BorderStyle = BorderStyle.FixedSingle;
            this.txtClassification.Location = new Point(134, 212);
            this.txtClassification.Name = "VideoPlay_FilePanel_26";
            this.txtClassification.Size = new Size(200, 18);
            this.txtClassification.TabIndex = 17;
            this.txtClassification.TextAlign = ContentAlignment.MiddleLeft;
            this.fp_Classification.AutoSize = true;
            this.fp_Classification.Location = new Point(14, 215);
            this.fp_Classification.Name = "VideoPlay_FilePanel_27";
            this.fp_Classification.Size = new Size(68, 13);
            this.fp_Classification.TabIndex = 16;
            this.fp_Classification.Text = "VideoPlay_FilePanel_28";
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add((Control)this.lbl_VideoFileInfo);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "VideoPlay_FilePanel_29";
            this.HeaderPanel.Size = new Size(351, 44);
            this.HeaderPanel.TabIndex = 18;
            this.lbl_VideoFileInfo.AutoSize = true;
            this.lbl_VideoFileInfo.BackColor = Color.Transparent;
            this.lbl_VideoFileInfo.Font = new Font("VideoPlay_FilePanel_30", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lbl_VideoFileInfo.ForeColor = Color.White;
            this.lbl_VideoFileInfo.Location = new Point(3, 9);
            this.lbl_VideoFileInfo.Name = "VideoPlay_FilePanel_31";
            this.lbl_VideoFileInfo.Size = new Size(201, 18);
            this.lbl_VideoFileInfo.TabIndex = 0;
            this.lbl_VideoFileInfo.Text = "VideoPlay_FilePanel_32";
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add((Control)this.HeaderPanel);
            this.Controls.Add((Control)this.txtClassification);
            this.Controls.Add((Control)this.fp_Classification);
            this.Controls.Add((Control)this.txtSet);
            this.Controls.Add((Control)this.fp_SetName);
            this.Controls.Add((Control)this.txtRMS);
            this.Controls.Add((Control)this.fp_RMS);
            this.Controls.Add((Control)this.txtCAD);
            this.Controls.Add((Control)this.fp_CAD);
            this.Controls.Add((Control)this.txtFileSize);
            this.Controls.Add((Control)this.fp_FileSize);
            this.Controls.Add((Control)this.txtAdded);
            this.Controls.Add((Control)this.fp_FileAdded);
            this.Controls.Add((Control)this.txtFileTimestamp);
            this.Controls.Add((Control)this.fp_FileTimestamp);
            this.Controls.Add((Control)this.txtFileExt);
            this.Controls.Add((Control)this.txtFileName);
            this.Controls.Add((Control)this.fp_FileExt);
            this.Controls.Add((Control)this.fp_OriginalFileName);
            this.Name = "VideoPlay_FilePanel_33";
            this.Size = new Size(351, 553);
            this.Load += new EventHandler(this.FilePanel_Load);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
