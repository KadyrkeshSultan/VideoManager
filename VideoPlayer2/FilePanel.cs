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
                HeaderPanel.BackgroundImage = (Image)Properties.Resources.topbar45;
            using (RPM_DataFile rpmDataFile = new RPM_DataFile())
            {
                DataFile dataFile = rpmDataFile.GetDataFile(Id);
                txtFileTimestamp.Text = dataFile.FileTimestamp.ToString();
                txtFileSize.Text = string.Format("{0}", dataFile.FileSize);
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
            base.SuspendLayout();
            this.fp_OriginalFileName.AutoSize = true;
            this.fp_OriginalFileName.Location = new Point(14, 55);
            this.fp_OriginalFileName.Name = "fp_OriginalFileName";
            this.fp_OriginalFileName.Size = new Size(92, 13);
            this.fp_OriginalFileName.TabIndex = 0;
            this.fp_OriginalFileName.Text = "Original File Name";
            this.fp_FileExt.AutoSize = true;
            this.fp_FileExt.Location = new Point(14, 75);
            this.fp_FileExt.Name = "fp_FileExt";
            this.fp_FileExt.Size = new Size(72, 13);
            this.fp_FileExt.TabIndex = 1;
            this.fp_FileExt.Text = "File Extension";
            this.txtFileName.BorderStyle = BorderStyle.FixedSingle;
            this.txtFileName.Location = new Point(134, 52);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new Size(200, 18);
            this.txtFileName.TabIndex = 2;
            this.txtFileName.TextAlign = ContentAlignment.MiddleLeft;
            this.txtFileExt.BorderStyle = BorderStyle.FixedSingle;
            this.txtFileExt.Location = new Point(134, 72);
            this.txtFileExt.Name = "txtFileExt";
            this.txtFileExt.Size = new Size(200, 18);
            this.txtFileExt.TabIndex = 3;
            this.txtFileExt.TextAlign = ContentAlignment.MiddleLeft;
            this.txtFileTimestamp.BorderStyle = BorderStyle.FixedSingle;
            this.txtFileTimestamp.Location = new Point(134, 92);
            this.txtFileTimestamp.Name = "txtFileTimestamp";
            this.txtFileTimestamp.Size = new Size(200, 18);
            this.txtFileTimestamp.TabIndex = 5;
            this.txtFileTimestamp.TextAlign = ContentAlignment.MiddleLeft;
            this.fp_FileTimestamp.AutoSize = true;
            this.fp_FileTimestamp.Location = new Point(14, 95);
            this.fp_FileTimestamp.Name = "fp_FileTimestamp";
            this.fp_FileTimestamp.Size = new Size(77, 13);
            this.fp_FileTimestamp.TabIndex = 4;
            this.fp_FileTimestamp.Text = "File Timestamp";
            this.txtAdded.BorderStyle = BorderStyle.FixedSingle;
            this.txtAdded.Location = new Point(134, 112);
            this.txtAdded.Name = "txtAdded";
            this.txtAdded.Size = new Size(200, 18);
            this.txtAdded.TabIndex = 7;
            this.txtAdded.TextAlign = ContentAlignment.MiddleLeft;
            this.fp_FileAdded.AutoSize = true;
            this.fp_FileAdded.Location = new Point(14, 115);
            this.fp_FileAdded.Name = "fp_FileAdded";
            this.fp_FileAdded.Size = new Size(57, 13);
            this.fp_FileAdded.TabIndex = 6;
            this.fp_FileAdded.Text = "File Added";
            this.txtFileSize.BorderStyle = BorderStyle.FixedSingle;
            this.txtFileSize.Location = new Point(134, 132);
            this.txtFileSize.Name = "txtFileSize";
            this.txtFileSize.Size = new Size(200, 18);
            this.txtFileSize.TabIndex = 9;
            this.txtFileSize.TextAlign = ContentAlignment.MiddleLeft;
            this.fp_FileSize.AutoSize = true;
            this.fp_FileSize.Location = new Point(14, 135);
            this.fp_FileSize.Name = "fp_FileSize";
            this.fp_FileSize.Size = new Size(46, 13);
            this.fp_FileSize.TabIndex = 8;
            this.fp_FileSize.Text = "File Size";
            this.txtCAD.BorderStyle = BorderStyle.FixedSingle;
            this.txtCAD.Location = new Point(134, 152);
            this.txtCAD.Name = "txtCAD";
            this.txtCAD.Size = new Size(200, 18);
            this.txtCAD.TabIndex = 11;
            this.txtCAD.TextAlign = ContentAlignment.MiddleLeft;
            this.fp_CAD.AutoSize = true;
            this.fp_CAD.Location = new Point(14, 155);
            this.fp_CAD.Name = "fp_CAD";
            this.fp_CAD.Size = new Size(69, 13);
            this.fp_CAD.TabIndex = 10;
            this.fp_CAD.Text = "CAD Number";
            this.txtRMS.BorderStyle = BorderStyle.FixedSingle;
            this.txtRMS.Location = new Point(134, 172);
            this.txtRMS.Name = "txtRMS";
            this.txtRMS.Size = new Size(200, 18);
            this.txtRMS.TabIndex = 13;
            this.txtRMS.TextAlign = ContentAlignment.MiddleLeft;
            this.fp_RMS.AutoSize = true;
            this.fp_RMS.Location = new Point(14, 175);
            this.fp_RMS.Name = "fp_RMS";
            this.fp_RMS.Size = new Size(71, 13);
            this.fp_RMS.TabIndex = 12;
            this.fp_RMS.Text = "RMS Number";
            this.txtSet.BorderStyle = BorderStyle.FixedSingle;
            this.txtSet.Location = new Point(134, 192);
            this.txtSet.Name = "txtSet";
            this.txtSet.Size = new Size(200, 18);
            this.txtSet.TabIndex = 15;
            this.txtSet.TextAlign = ContentAlignment.MiddleLeft;
            this.fp_SetName.AutoSize = true;
            this.fp_SetName.Location = new Point(14, 195);
            this.fp_SetName.Name = "fp_SetName";
            this.fp_SetName.Size = new Size(59, 13);
            this.fp_SetName.TabIndex = 14;
            this.fp_SetName.Text = "SET Name";
            this.txtClassification.BorderStyle = BorderStyle.FixedSingle;
            this.txtClassification.Location = new Point(134, 212);
            this.txtClassification.Name = "txtClassification";
            this.txtClassification.Size = new Size(200, 18);
            this.txtClassification.TabIndex = 17;
            this.txtClassification.TextAlign = ContentAlignment.MiddleLeft;
            this.fp_Classification.AutoSize = true;
            this.fp_Classification.Location = new Point(14, 215);
            this.fp_Classification.Name = "fp_Classification";
            this.fp_Classification.Size = new Size(68, 13);
            this.fp_Classification.TabIndex = 16;
            this.fp_Classification.Text = "Classification";
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.lbl_VideoFileInfo);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(351, 44);
            this.HeaderPanel.TabIndex = 18;
            this.lbl_VideoFileInfo.AutoSize = true;
            this.lbl_VideoFileInfo.BackColor = Color.Transparent;
            this.lbl_VideoFileInfo.Font = new Font("Verdana", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_VideoFileInfo.ForeColor = Color.White;
            this.lbl_VideoFileInfo.Location = new Point(3, 9);
            this.lbl_VideoFileInfo.Name = "lbl_VideoFileInfo";
            this.lbl_VideoFileInfo.Size = new Size(201, 18);
            this.lbl_VideoFileInfo.TabIndex = 0;
            this.lbl_VideoFileInfo.Text = "Video File Information";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.Controls.Add(this.HeaderPanel);
            base.Controls.Add(this.txtClassification);
            base.Controls.Add(this.fp_Classification);
            base.Controls.Add(this.txtSet);
            base.Controls.Add(this.fp_SetName);
            base.Controls.Add(this.txtRMS);
            base.Controls.Add(this.fp_RMS);
            base.Controls.Add(this.txtCAD);
            base.Controls.Add(this.fp_CAD);
            base.Controls.Add(this.txtFileSize);
            base.Controls.Add(this.fp_FileSize);
            base.Controls.Add(this.txtAdded);
            base.Controls.Add(this.fp_FileAdded);
            base.Controls.Add(this.txtFileTimestamp);
            base.Controls.Add(this.fp_FileTimestamp);
            base.Controls.Add(this.txtFileExt);
            base.Controls.Add(this.txtFileName);
            base.Controls.Add(this.fp_FileExt);
            base.Controls.Add(this.fp_OriginalFileName);
            base.Name = "FilePanel";
            base.Size = new Size(351, 553);
            base.Load += new EventHandler(this.FilePanel_Load);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
        
    }
}
