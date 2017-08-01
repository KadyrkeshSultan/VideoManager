using MemoList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Unity;
using VideoPlayer2;
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
            lblCount.Text = string.Format("GlobalCat_CatObj_1", Count);
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
            //TODO : LangCtrl
            //LangCtrl.reText((Control)this);
            //this.picImage.Image = Utilities.ByteArrayToImage(this.dRec.Thumbnail);
            //this.lblFileDate.Text = string.Format(LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(452), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(478)), (object)this.dRec.FileTimestamp);
            //this.lblIngestDate.Text = string.Format(LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(510), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(540)), (object)this.dRec.FileAddedTimestamp);
            //this.lblShortDesc.Text = string.Format(LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(576), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(604)), (object)this.dRec.ShortDesc);
            //this.lblRMS.Text = string.Format(LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(626), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(642)), (object)this.dRec.RMSNumber);
            //this.lblCAD.Text = string.Format(LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(662), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(678)), (object)this.dRec.CADNumber);
            //this.lblSetName.Text = string.Format(LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(698), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(722)), (object)this.dRec.SetName);
            //this.eTable.Visible = this.dRec.IsEvidence;
            //this.lblSecurity.BackColor = Color.Transparent;
            //this.lblSecurity.Text = LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(742), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(778));
            //switch (this.dRec.Security)
            //{
            //    case SECURITY.TOPSECRET:
            //        this.lblSecurity.ForeColor = Color.Yellow;
            //        this.lblSecurity.BackColor = Color.Red;
            //        this.lblSecurity.Text = LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(806), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(836));
            //        break;
            //    case SECURITY.SECRET:
            //        this.lblSecurity.ForeColor = Color.White;
            //        this.lblSecurity.BackColor = Color.Orange;
            //        this.lblSecurity.Text = LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(860), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(884));
            //        break;
            //    case SECURITY.OFFICIAL:
            //        this.lblSecurity.ForeColor = Color.Black;
            //        this.lblSecurity.BackColor = Color.Yellow;
            //        this.lblSecurity.Text = LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(900), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(928));
            //        break;
            //}
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
                int num = (int)MessageBox.Show(this, ex.Message, "GlobalCat_CatObj_2", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
            this.components = (IContainer)new Container();
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
            this.SuspendLayout();
            this.contextMenuStrip1.Items.AddRange(new ToolStripItem[3]
            {
        (ToolStripItem) this.mnu_ViewFile,
        (ToolStripItem) this.mnu_FileProperties,
        (ToolStripItem) this.reviewMemosToolStripMenuItem
            });
            this.contextMenuStrip1.Name = "GlobalCat_CatObj_3";
            this.contextMenuStrip1.Size = new Size(155, 70);
            this.mnu_ViewFile.Name = "GlobalCat_CatObj_4";
            this.mnu_ViewFile.Size = new Size(154, 22);
            this.mnu_ViewFile.Text = "GlobalCat_CatObj_5";
            this.mnu_ViewFile.Click += new EventHandler(this.mnu_ViewFile_Click);
            this.mnu_FileProperties.Name = "GlobalCat_CatObj_6";
            this.mnu_FileProperties.Size = new Size(154, 22);
            this.mnu_FileProperties.Text = "GlobalCat_CatObj_7";
            this.mnu_FileProperties.Click += new EventHandler(this.mnu_FileProperties_Click);
            this.reviewMemosToolStripMenuItem.Name = "GlobalCat_CatObj_8";
            this.reviewMemosToolStripMenuItem.Size = new Size(154, 22);
            this.reviewMemosToolStripMenuItem.Text = "GlobalCat_CatObj_9";
            this.reviewMemosToolStripMenuItem.Click += new EventHandler(this.reviewMemosToolStripMenuItem_Click);
            this.AccountName.AutoSize = true;
            this.AccountName.Font = new Font("GlobalCat_CatObj_10", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.AccountName.Location = new Point(174, 4);
            this.AccountName.Name = "GlobalCat_CatObj_11";
            this.AccountName.Size = new Size(84, 13);
            this.AccountName.TabIndex = 1;
            this.AccountName.Text = "GlobalCat_CatObj_12";
            this.lblFileDate.AutoSize = true;
            this.lblFileDate.Location = new Point(174, 21);
            this.lblFileDate.Name = "GlobalCat_CatObj_13";
            this.lblFileDate.Size = new Size(137, 13);
            this.lblFileDate.TabIndex = 2;
            this.lblFileDate.Text = "GlobalCat_CatObj_14";
            this.lblIngestDate.AutoSize = true;
            this.lblIngestDate.Location = new Point(174, 38);
            this.lblIngestDate.Name = "GlobalCat_CatObj_15";
            this.lblIngestDate.Size = new Size(137, 13);
            this.lblIngestDate.TabIndex = 3;
            this.lblIngestDate.Text = "GlobalCat_CatObj_16";
            this.lblShortDesc.AutoSize = true;
            this.lblShortDesc.Location = new Point(174, 106);
            this.lblShortDesc.Name = "GlobalCat_CatObj_17";
            this.lblShortDesc.Size = new Size(88, 13);
            this.lblShortDesc.TabIndex = 4;
            this.lblShortDesc.Text = "GlobalCat_CatObj_18";
            this.lblRMS.AutoSize = true;
            this.lblRMS.Font = new Font("GlobalCat_CatObj_19", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lblRMS.Location = new Point(174, 72);
            this.lblRMS.Name = "GlobalCat_CatObj_20";
            this.lblRMS.Size = new Size(38, 13);
            this.lblRMS.TabIndex = 5;
            this.lblRMS.Text = "GlobalCat_CatObj_21";
            this.lblCAD.AutoEllipsis = true;
            this.lblCAD.AutoSize = true;
            this.lblCAD.Font = new Font("GlobalCat_CatObj_22", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lblCAD.Location = new Point(174, 89);
            this.lblCAD.Name = "GlobalCat_CatObj_23";
            this.lblCAD.Size = new Size(36, 13);
            this.lblCAD.TabIndex = 6;
            this.lblCAD.Text = "GlobalCat_CatObj_24";
            this.lblCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblCount.BackColor = Color.LightGray;
            this.lblCount.BorderStyle = BorderStyle.FixedSingle;
            this.lblCount.Font = new Font("GlobalCat_CatObj_25", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lblCount.Location = new Point(394, 5);
            this.lblCount.Name = "GlobalCat_CatObj_26";
            this.lblCount.Size = new Size(74, 23);
            this.lblCount.TabIndex = 7;
            this.lblCount.Text = "GlobalCat_CatObj_27";
            this.lblCount.TextAlign = ContentAlignment.MiddleCenter;
            this.lblSetName.AutoEllipsis = true;
            this.lblSetName.AutoSize = true;
            this.lblSetName.Font = new Font("GlobalCat_CatObj_28", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lblSetName.Location = new Point(174, 55);
            this.lblSetName.Name = "GlobalCat_CatObj_29";
            this.lblSetName.Size = new Size(35, 13);
            this.lblSetName.TabIndex = 10;
            this.lblSetName.Text = "GlobalCat_CatObj_30";
            this.lblSetName.TextAlign = ContentAlignment.MiddleLeft;
            this.lblSecurity.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblSecurity.BorderStyle = BorderStyle.FixedSingle;
            this.lblSecurity.FlatStyle = FlatStyle.Flat;
            this.lblSecurity.Location = new Point(394, 32);
            this.lblSecurity.Name = "GlobalCat_CatObj_31";
            this.lblSecurity.Size = new Size(74, 18);
            this.lblSecurity.TabIndex = 11;
            this.lblSecurity.Text = "GlobalCat_CatObj_32";
            this.lblSecurity.TextAlign = ContentAlignment.MiddleCenter;
            this.eTable.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.eTable.ColumnCount = 2;
            this.eTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 76.47059f));
            this.eTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.52941f));
            this.eTable.Controls.Add((Control)this.lbl_Evidence, 0, 0);
            this.eTable.Controls.Add((Control)this.ePic, 1, 0);
            this.eTable.Location = new Point(381, 54);
            this.eTable.Name = "GlobalCat_CatObj_33";
            this.eTable.RowCount = 1;
            this.eTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.eTable.Size = new Size(87, 20);
            this.eTable.TabIndex = 12;
            this.eTable.Visible = false;
            this.lbl_Evidence.AutoSize = true;
            this.lbl_Evidence.Dock = DockStyle.Fill;
            this.lbl_Evidence.Location = new Point(3, 0);
            this.lbl_Evidence.Name = "GlobalCat_CatObj_34";
            this.lbl_Evidence.Size = new Size(60, 20);
            this.lbl_Evidence.TabIndex = 0;
            this.lbl_Evidence.Text = "GlobalCat_CatObj_35";
            this.lbl_Evidence.TextAlign = ContentAlignment.MiddleRight;
            this.ePic.Image = (Image)Properties.Resources.check2;
            this.ePic.Location = new Point(66, 0);
            this.ePic.Margin = new Padding(0);
            this.ePic.Name = "GlobalCat_CatObj_36";
            this.ePic.Size = new Size(21, 20);
            this.ePic.SizeMode = PictureBoxSizeMode.CenterImage;
            this.ePic.TabIndex = 1;
            this.ePic.TabStop = false;
            this.picImage.BackColor = Color.Black;
            this.picImage.ContextMenuStrip = this.contextMenuStrip1;
            this.picImage.Cursor = Cursors.Hand;
            this.picImage.Location = new Point(5, 5);
            this.picImage.Name = "GlobalCat_CatObj_37";
            this.picImage.Size = new Size(163, 114);
            this.picImage.SizeMode = PictureBoxSizeMode.StretchImage;
            this.picImage.TabIndex = 0;
            this.picImage.TabStop = false;
            this.picImage.DoubleClick += new EventHandler(this.picImage_DoubleClick);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add((Control)this.eTable);
            this.Controls.Add((Control)this.lblSecurity);
            this.Controls.Add((Control)this.lblSetName);
            this.Controls.Add((Control)this.lblCount);
            this.Controls.Add((Control)this.lblCAD);
            this.Controls.Add((Control)this.lblRMS);
            this.Controls.Add((Control)this.lblShortDesc);
            this.Controls.Add((Control)this.lblIngestDate);
            this.Controls.Add((Control)this.lblFileDate);
            this.Controls.Add((Control)this.AccountName);
            this.Controls.Add((Control)this.picImage);
            this.Name = "GlobalCat_CatObj_38";
            this.Size = new Size(472, 125);
            this.Load += new EventHandler(this.CatalogObject_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.eTable.ResumeLayout(false);
            this.eTable.PerformLayout();
            ((ISupportInitialize)this.ePic).EndInit();
            ((ISupportInitialize)this.picImage).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
