using AppGlobal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Unity;
using VMInterfaces;
using VMModels.Enums;
using VMModels.Model;

namespace VideoPlayer2
{
    public class ThumbPanel : UserControl
    {
        private Guid FileID;
        private ArrayList aList;
        private IContainer components;
        private Panel HeaderPanel;
        private Label lbl_Thumbnails;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem mnu_DeleteAllSnapshots;
        private Label lbl_VideoSnapshots;
        private FlowLayoutPanel ImagePanel;

        
        public ThumbPanel()
        {
            FileID = Guid.Empty;
            aList = new ArrayList();
            InitializeComponent();
            Dock = DockStyle.Fill;
        }

        
        private void ThumbPanel_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
                HeaderPanel.BackgroundImage = (Image)Properties.Resources.topbar45;
            mnu_DeleteAllSnapshots.Text = LangCtrl.GetString("mnu_DeleteAllSnapshots", "Delete all snapshots");
        }

        
        public void SetFileID(Guid Id)
        {
            FileID = Id;
            Application.DoEvents();
            LoadSnaphots();
        }

        
        public void LoadSnaphots()
        {
            bool b = false;
            if (Global.IsRights(Global.RightsProfile, UserRights.EXPORT))
                b = true;
            ImagePanel.Controls.Clear();
            using (RPM_Snapshot rpmSnapshot = new RPM_Snapshot())
            {
                List<Snapshot> snapshots = rpmSnapshot.GetSnapshots(this.FileID);
                if (snapshots.Count <= 0)
                    return;
                foreach (Snapshot rec in snapshots)
                {
                    ImagePanel imagePanel = new ImagePanel();
                    Image image = Utilities.ByteArrayToImage(rec.Thumbnail);
                    imagePanel.CanExport(b);
                    if (Global.IsRedact && Global.IsRights(Global.RightsProfile, UserRights.REDACT))
                        imagePanel.IsRedact(true);
                    imagePanel.SetImage(rec, image);
                    ImagePanel.Controls.Add(imagePanel);
                }
            }
        }

        
        public void ClearPanel()
        {
            ImagePanel.Controls.Clear();
        }

        
        public void AddImage(Snapshot rec, Image img)
        {
            ImagePanel imagePanel = new ImagePanel();
            imagePanel.SetImage(rec, img);
            ImagePanel.Controls.Add(imagePanel);
            aList.Add(rec.FrameNumber);
            BuildList();
            if (rec.FrameNumber > (int)aList[aList.Count - 1])
                return;
            if (rec.FrameNumber < (int)aList[0])
            {
                ImagePanel.Controls.SetChildIndex(imagePanel, 0);
            }
            else
            {
                for (int newIndex = 0; newIndex < aList.Count - 1; ++newIndex)
                {
                    if ((int)aList[newIndex] == rec.FrameNumber)
                    {
                        ImagePanel.Controls.SetChildIndex(imagePanel, newIndex);
                        break;
                    }
                }
                Application.DoEvents();
            }
        }

        
        private void BuildList()
        {
            aList.Clear();
            for (int index = 0; index < ImagePanel.Controls.Count; ++index)
                aList.Add(((ImagePanel)ImagePanel.Controls[index]).FrameNumber());
            aList.Sort();
        }

        
        private void mnu_DeleteAllSnapshots_Click(object sender, EventArgs e)
        {
            if (ImagePanel.Controls.Count <= 0 || MessageBox.Show(this, LangCtrl.GetString("tn_DeleteAll", "Delete All Snapshot Images?"), "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            using (RPM_Snapshot rpmSnapshot = new RPM_Snapshot())
            {
                rpmSnapshot.DeleteAllSnapshots(FileID);
                rpmSnapshot.Save();
            }
            ImagePanel.Controls.Clear();
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
            this.HeaderPanel = new Panel();
            this.lbl_Thumbnails = new Label();
            this.contextMenuStrip1 = new ContextMenuStrip(this.components);
            this.mnu_DeleteAllSnapshots = new ToolStripMenuItem();
            this.ImagePanel = new FlowLayoutPanel();
            this.lbl_VideoSnapshots = new Label();
            this.HeaderPanel.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            base.SuspendLayout();
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.lbl_VideoSnapshots);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(325, 44);
            this.HeaderPanel.TabIndex = 2;
            this.lbl_Thumbnails.AutoSize = true;
            this.lbl_Thumbnails.BackColor = Color.Transparent;
            this.lbl_Thumbnails.Font = new Font("Verdana", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_Thumbnails.ForeColor = Color.White;
            this.lbl_Thumbnails.Location = new Point(4, 5);
            this.lbl_Thumbnails.Name = "lbl_Thumbnails";
            this.lbl_Thumbnails.Size = new Size(136, 16);
            this.lbl_Thumbnails.TabIndex = 0;
            this.lbl_Thumbnails.Text = "Video Thumbnails";
            this.contextMenuStrip1.Items.AddRange(new ToolStripItem[] { this.mnu_DeleteAllSnapshots });
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new Size(179, 26);
            this.mnu_DeleteAllSnapshots.Name = "mnu_DeleteAllSnapshots";
            this.mnu_DeleteAllSnapshots.Size = new Size(178, 22);
            this.mnu_DeleteAllSnapshots.Text = "Delete all snapshots";
            this.mnu_DeleteAllSnapshots.Click += new EventHandler(this.mnu_DeleteAllSnapshots_Click);
            this.ImagePanel.AutoScroll = true;
            this.ImagePanel.Dock = DockStyle.Fill;
            this.ImagePanel.Location = new Point(0, 44);
            this.ImagePanel.Name = "ImagePanel";
            this.ImagePanel.Size = new Size(325, 356);
            this.ImagePanel.TabIndex = 3;
            this.lbl_VideoSnapshots.AutoSize = true;
            this.lbl_VideoSnapshots.BackColor = Color.Transparent;
            this.lbl_VideoSnapshots.Font = new Font("Verdana", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_VideoSnapshots.ForeColor = Color.White;
            this.lbl_VideoSnapshots.Location = new Point(4, 12);
            this.lbl_VideoSnapshots.Name = "lbl_VideoSnapshots";
            this.lbl_VideoSnapshots.Size = new Size(153, 18);
            this.lbl_VideoSnapshots.TabIndex = 0;
            this.lbl_VideoSnapshots.Text = "Video Snapshots";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.Controls.Add(this.ImagePanel);
            base.Controls.Add(this.HeaderPanel);
            base.Name = "ThumbPanel";
            base.Size = new Size(325, 400);
            base.Load += new EventHandler(this.ThumbPanel_Load);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}
