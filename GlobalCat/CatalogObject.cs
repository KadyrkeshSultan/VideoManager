using MemoList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Unity;
using VideoPlayer2;
using VMModels.Enums;
using VMModels.Model;

namespace GlobalCat
{
    public class CatalogObject : UserControl
    {
        private DataFile dRec;
        private int FileCountID;
        private IContainer components;
        private PictureBox picImage;
        private Label AccountName;
        private Label lblFileDate;
        private Label lblIngestDate;
        private Label lblShortDesc;
        private Label lblRMS;
        private Label lblCAD;
        private Label lblCount;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem mnu_ViewFile;
        private ToolStripMenuItem mnu_FileProperties;
        private ToolStripMenuItem reviewMemosToolStripMenuItem;
        private Label lblSetName;
        private Label lblSecurity;
        private TableLayoutPanel eTable;
        private Label lbl_Evidence;
        private PictureBox ePic;

        
        public CatalogObject()
        {
            Init();
        }

        
        public CatalogObject(int Count, DataFile df, string acctName)
        {
            Init();
            lblCount.Text = string.Format("{0}", Count);
            FileCountID = Count;
            AccountName.Text = acctName;
            dRec = df;
        }

        
        private void Init()
        {
            InitializeComponent();
            Dock = DockStyle.Top;
        }

        
        private void CatalogObject_Load(object sender, EventArgs e)
        {
            LangCtrl.reText(this);
            picImage.Image = Utilities.ByteArrayToImage(dRec.Thumbnail);
            lblFileDate.Text = string.Format(LangCtrl.GetString("lblFileDate", "File Date: {0}"), dRec.FileTimestamp);
            lblIngestDate.Text = string.Format(LangCtrl.GetString("lblIngestDate", "Ingest Date: {0}"), dRec.FileAddedTimestamp);
            lblShortDesc.Text = string.Format(LangCtrl.GetString("lblShortDesc", "Desc: {0}"), dRec.ShortDesc);
            lblRMS.Text = string.Format(LangCtrl.GetString("lblRMS", "RMS: {0}"), dRec.RMSNumber);
            lblCAD.Text = string.Format(LangCtrl.GetString("lblCAD", "CAD: {0}"), dRec.CADNumber);
            lblSetName.Text = string.Format(LangCtrl.GetString("lblSetName", "SET: {0}"), dRec.SetName);
            eTable.Visible = dRec.IsEvidence;
            lblSecurity.BackColor = Color.Transparent;
            lblSecurity.Text = LangCtrl.GetString("sec_Unclassified", "Unclassified");
            switch (dRec.Security)
            {
                case SECURITY.TOPSECRET:
                    {
                        lblSecurity.ForeColor = Color.Yellow;
                        lblSecurity.BackColor = Color.Red;
                        lblSecurity.Text = LangCtrl.GetString("sec_TopSecret", "Top Secret");
                        return;
                    }
                case SECURITY.SECRET:
                    {
                        lblSecurity.ForeColor = Color.White;
                        lblSecurity.BackColor = Color.Orange;
                        lblSecurity.Text = LangCtrl.GetString("sec_Secret", "Secret");
                        return;
                    }
                case SECURITY.OFFICIAL:
                    {
                        lblSecurity.ForeColor = Color.Black;
                        lblSecurity.BackColor = Color.Yellow;
                        lblSecurity.Text = LangCtrl.GetString("sec_Official", "Official");
                        return;
                    }
                default:
                    {
                        return;
                    }
            }
        }

        
        private void mnu_ViewFile_Click(object sender, EventArgs e)
        {
            Play();
        }

        
        private void Play()
        {
            try
            {
                BackColor = Color.Silver;
                List<Guid> files = new List<Guid>();
                files.Add(dRec.Id);
                VideoForm videoForm = new VideoForm();
                videoForm.LoadVideoList(files, dRec.AccountId);
                int num = (int)videoForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            BackColor = Color.White;
        }

        
        private void mnu_FileProperties_Click(object sender, EventArgs e)
        {
            BackColor = Color.Silver;
            DataProfile dataProfile = new DataProfile();
            dataProfile.LoadProfile(dRec, this.AccountName.Text, this.FileCountID);
            int num = (int)dataProfile.ShowDialog(this);
            BackColor = Color.White;
        }

        
        private void picImage_DoubleClick(object sender, EventArgs e)
        {
            Play();
        }

        
        private void reviewMemosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackColor = Color.Silver;
            int num = (int)new MemoForm(dRec.Id, "").ShowDialog(this);
            BackColor = Color.White;
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        
        private void InitializeComponent()
        {
            this.components = new Container();
            this.contextMenuStrip1 = new ContextMenuStrip(this.components);
            this.mnu_ViewFile = new ToolStripMenuItem();
            this.mnu_FileProperties = new ToolStripMenuItem();
            this.reviewMemosToolStripMenuItem = new ToolStripMenuItem();
            this.AccountName = new Label();
            this.lblFileDate = new Label();
            this.lblIngestDate = new Label();
            this.lblShortDesc = new Label();
            this.lblRMS = new Label();
            this.lblCAD = new Label();
            this.lblCount = new Label();
            this.lblSetName = new Label();
            this.lblSecurity = new Label();
            this.eTable = new TableLayoutPanel();
            this.lbl_Evidence = new Label();
            this.ePic = new PictureBox();
            this.picImage = new PictureBox();
            this.contextMenuStrip1.SuspendLayout();
            this.eTable.SuspendLayout();
            ((ISupportInitialize)this.ePic).BeginInit();
            ((ISupportInitialize)this.picImage).BeginInit();
            base.SuspendLayout();
            ToolStripItemCollection items = this.contextMenuStrip1.Items;
            ToolStripItem[] mnuViewFile = new ToolStripItem[] { this.mnu_ViewFile, this.mnu_FileProperties, this.reviewMemosToolStripMenuItem };
            this.contextMenuStrip1.Items.AddRange(mnuViewFile);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new Size(155, 70);
            this.mnu_ViewFile.Name = "mnu_ViewFile";
            this.mnu_ViewFile.Size = new Size(154, 22);
            this.mnu_ViewFile.Text = "View File";
            this.mnu_ViewFile.Click += new EventHandler(this.mnu_ViewFile_Click);
            this.mnu_FileProperties.Name = "mnu_FileProperties";
            this.mnu_FileProperties.Size = new Size(154, 22);
            this.mnu_FileProperties.Text = "File Properties";
            this.mnu_FileProperties.Click += new EventHandler(this.mnu_FileProperties_Click);
            this.reviewMemosToolStripMenuItem.Name = "reviewMemosToolStripMenuItem";
            this.reviewMemosToolStripMenuItem.Size = new Size(154, 22);
            this.reviewMemosToolStripMenuItem.Text = "Review Memos";
            this.reviewMemosToolStripMenuItem.Click += new EventHandler(this.reviewMemosToolStripMenuItem_Click);
            this.AccountName.AutoSize = true;
            this.AccountName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.AccountName.Location = new Point(174, 4);
            this.AccountName.Name = "AccountName";
            this.AccountName.Size = new Size(84, 13);
            this.AccountName.TabIndex = 1;
            this.AccountName.Text = "User Account";
            this.lblFileDate.AutoSize = true;
            this.lblFileDate.Location = new Point(174, 21);
            this.lblFileDate.Name = "lblFileDate";
            this.lblFileDate.Size = new Size(137, 13);
            this.lblFileDate.TabIndex = 2;
            this.lblFileDate.Text = "00/00/0000 00:00:00.0000";
            this.lblIngestDate.AutoSize = true;
            this.lblIngestDate.Location = new Point(174, 38);
            this.lblIngestDate.Name = "lblIngestDate";
            this.lblIngestDate.Size = new Size(137, 13);
            this.lblIngestDate.TabIndex = 3;
            this.lblIngestDate.Text = "00/00/0000 00:00:00.0000";
            this.lblShortDesc.AutoSize = true;
            this.lblShortDesc.Location = new Point(174, 106);
            this.lblShortDesc.Name = "lblShortDesc";
            this.lblShortDesc.Size = new Size(88, 13);
            this.lblShortDesc.TabIndex = 4;
            this.lblShortDesc.Text = "Short Description";
            this.lblRMS.AutoSize = true;
            this.lblRMS.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblRMS.Location = new Point(174, 72);
            this.lblRMS.Name = "lblRMS";
            this.lblRMS.Size = new Size(38, 13);
            this.lblRMS.TabIndex = 5;
            this.lblRMS.Text = "RMS:";
            this.lblCAD.AutoEllipsis = true;
            this.lblCAD.AutoSize = true;
            this.lblCAD.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblCAD.Location = new Point(174, 89);
            this.lblCAD.Name = "lblCAD";
            this.lblCAD.Size = new Size(36, 13);
            this.lblCAD.TabIndex = 6;
            this.lblCAD.Text = "CAD:";
            this.lblCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblCount.BackColor = Color.LightGray;
            this.lblCount.BorderStyle = BorderStyle.FixedSingle;
            this.lblCount.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblCount.Location = new Point(394, 5);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new Size(74, 23);
            this.lblCount.TabIndex = 7;
            this.lblCount.Text = "0";
            this.lblCount.TextAlign = ContentAlignment.MiddleCenter;
            this.lblSetName.AutoEllipsis = true;
            this.lblSetName.AutoSize = true;
            this.lblSetName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblSetName.Location = new Point(174, 55);
            this.lblSetName.Name = "lblSetName";
            this.lblSetName.Size = new Size(35, 13);
            this.lblSetName.TabIndex = 10;
            this.lblSetName.Text = "SET:";
            this.lblSetName.TextAlign = ContentAlignment.MiddleLeft;
            this.lblSecurity.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblSecurity.BorderStyle = BorderStyle.FixedSingle;
            this.lblSecurity.FlatStyle = FlatStyle.Flat;
            this.lblSecurity.Location = new Point(394, 32);
            this.lblSecurity.Name = "lblSecurity";
            this.lblSecurity.Size = new Size(74, 18);
            this.lblSecurity.TabIndex = 11;
            this.lblSecurity.Text = "n/a";
            this.lblSecurity.TextAlign = ContentAlignment.MiddleCenter;
            this.eTable.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.eTable.ColumnCount = 2;
            this.eTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 76.47059f));
            this.eTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.52941f));
            this.eTable.Controls.Add(this.lbl_Evidence, 0, 0);
            this.eTable.Controls.Add(this.ePic, 1, 0);
            this.eTable.Location = new Point(381, 54);
            this.eTable.Name = "eTable";
            this.eTable.RowCount = 1;
            this.eTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.eTable.Size = new Size(87, 20);
            this.eTable.TabIndex = 12;
            this.eTable.Visible = false;
            this.lbl_Evidence.AutoSize = true;
            this.lbl_Evidence.Dock = DockStyle.Fill;
            this.lbl_Evidence.Location = new Point(3, 0);
            this.lbl_Evidence.Name = "lbl_Evidence";
            this.lbl_Evidence.Size = new Size(60, 20);
            this.lbl_Evidence.TabIndex = 0;
            this.lbl_Evidence.Text = "Evidence";
            this.lbl_Evidence.TextAlign = ContentAlignment.MiddleRight;
            this.ePic.Image = Properties.Resources.check2;
            this.ePic.Location = new Point(66, 0);
            this.ePic.Margin = new Padding(0);
            this.ePic.Name = "ePic";
            this.ePic.Size = new Size(21, 20);
            this.ePic.SizeMode = PictureBoxSizeMode.CenterImage;
            this.ePic.TabIndex = 1;
            this.ePic.TabStop = false;
            this.picImage.BackColor = Color.Black;
            this.picImage.ContextMenuStrip = this.contextMenuStrip1;
            this.picImage.Cursor = Cursors.Hand;
            this.picImage.Location = new Point(5, 5);
            this.picImage.Name = "picImage";
            this.picImage.Size = new Size(163, 114);
            this.picImage.SizeMode = PictureBoxSizeMode.StretchImage;
            this.picImage.TabIndex = 0;
            this.picImage.TabStop = false;
            this.picImage.DoubleClick += new EventHandler(this.picImage_DoubleClick);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.BorderStyle = BorderStyle.FixedSingle;
            base.Controls.Add(this.eTable);
            base.Controls.Add(this.lblSecurity);
            base.Controls.Add(this.lblSetName);
            base.Controls.Add(this.lblCount);
            base.Controls.Add(this.lblCAD);
            base.Controls.Add(this.lblRMS);
            base.Controls.Add(this.lblShortDesc);
            base.Controls.Add(this.lblIngestDate);
            base.Controls.Add(this.lblFileDate);
            base.Controls.Add(this.AccountName);
            base.Controls.Add(this.picImage);
            base.Name = "CatalogObject";
            base.Size = new Size(472, 125);
            base.Load += new EventHandler(this.CatalogObject_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.eTable.ResumeLayout(false);
            this.eTable.PerformLayout();
            ((ISupportInitialize)this.ePic).EndInit();
            ((ISupportInitialize)this.picImage).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
