﻿using AppGlobal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMInterfaces;
using VMModels.Model;

namespace VideoPlayer2
{
    public class TagPanel : UserControl
    {
        private Guid File_ID;
        public List<VideoTag> TagList;
        private IContainer components;
        private vCheckedListBox vListBox;
        private ContextMenuStrip TagMenu;
        private ToolStripMenuItem mnu_SelectAllTags;
        private ToolStripMenuItem mnu_DelselectAllTags;
        private ToolStripMenuItem mnu_DeleteTags;
        private vButton btn_MergeVideo;
        private FolderBrowserDialog folderBrowserDialog;
        private Label lbl_VideoMarks;
        private Panel HeaderPanel;

        private string FileName {  get;  set; }

        public event DEL_MergeVideo EVT_MergeVideo;

        
        public TagPanel()
        {
            File_ID = Guid.Empty;
            TagList = new List<VideoTag>();
            InitializeComponent();
            Dock = DockStyle.Fill;
        }

        
        private void TagPanel_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
                HeaderPanel.BackgroundImage = Properties.Resources.topbar45;
            LangCtrl.reText(this);
            LangCtrl.reText(TagMenu);
        }

        
        public void ListTags(Guid FileID)
        {
            File_ID = FileID;
            using (RPM_DataFile rpmDataFile = new RPM_DataFile())
            {
                DataFile dataFile = rpmDataFile.GetDataFile(FileID);
                FileName = string.Format("VideoPlay_TagP_1", dataFile.StoredFileName, dataFile.FileExtension);
            }
            vListBox.Items.Clear();
            vListBox.SelectedIndex = -1;
            using (RPM_VideoTag rpmVideoTag = new RPM_VideoTag())
            {
                TagList = rpmVideoTag.GetTagList(FileID);
                foreach (VideoTag tag in TagList)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = string.Format("VideoPlay_TagP_2", tag.ShortDesc);
                    TimeSpan timeSpan1 = new TimeSpan(0, 0, Convert.ToInt32(tag.StartTime));
                    TimeSpan timeSpan2 = new TimeSpan(0, 0, Convert.ToInt32(tag.EndTime));
                    string str1 = string.Format("VideoPlay_TagP_3", timeSpan1.Hours, timeSpan1.Minutes, timeSpan1.Seconds);
                    string str2 = string.Format("VideoPlay_TagP_4", timeSpan2.Hours, timeSpan2.Minutes, timeSpan2.Seconds);
                    listItem.Description = string.Format("VideoPlay_TagP_5", str1, str2);
                    listItem.Tag = tag;
                    listItem.ImageIndex = 0;
                    vListBox.Items.Add(listItem);
                }
            }
        }

        
        private void DeleteTags()
        {
            if (vListBox.Items.Count <= 0 || MessageBox.Show(this, LangCtrl.GetString("VideoPlay_TagP_6", "VideoPlay_TagP_7"), "VideoPlay_TagP_8", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            foreach (ListItem listItem in vListBox.Items)
            {
                using (RPM_VideoTag rpmVideoTag = new RPM_VideoTag())
                {
                    if (listItem.IsChecked.Value)
                    {
                        VideoTag tag1 = (VideoTag)listItem.Tag;
                        VideoTag tag2 = rpmVideoTag.GetTag(tag1.Id);
                        rpmVideoTag.DeleteVideoTag(tag2);
                        rpmVideoTag.Save();
                        Global.Log("VideoPlay_TagP_9", string.Format("VideoPlay_TagP_10", listItem.Text.ToUpper(), listItem.Description, FileName));
                    }
                }
            }
            ListTags(File_ID);
        }

        
        private void mnu_SelectAllTags_Click(object sender, EventArgs e)
        {
            foreach (ListItem listItem in vListBox.Items)
                listItem.IsChecked = new bool?(true);
        }

        
        private void mnu_DelselectAllTags_Click(object sender, EventArgs e)
        {
            foreach (ListItem listItem in vListBox.Items)
                listItem.IsChecked = new bool?(false);
        }

        
        private void mnu_DeleteTags_Click(object sender, EventArgs e)
        {
            DeleteTags();
        }

        
        private void Callback(string folder)
        {
            if (EVT_MergeVideo == null)
                return;
            EVT_MergeVideo(folder);
        }

        
        private void btn_Merge_Click(object sender, EventArgs e)
        {
            if (TagList.Count <= 0 || folderBrowserDialog.ShowDialog(this) != DialogResult.OK)
                return;
            Callback(this.folderBrowserDialog.SelectedPath);
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
            this.vListBox = new vCheckedListBox();
            this.TagMenu = new ContextMenuStrip(this.components);
            this.mnu_SelectAllTags = new ToolStripMenuItem();
            this.mnu_DelselectAllTags = new ToolStripMenuItem();
            this.mnu_DeleteTags = new ToolStripMenuItem();
            this.btn_MergeVideo = new vButton();
            this.folderBrowserDialog = new FolderBrowserDialog();
            this.lbl_VideoMarks = new Label();
            this.HeaderPanel = new Panel();
            this.TagMenu.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            this.SuspendLayout();
            this.vListBox.CheckOnClick = true;
            this.vListBox.ContextMenuStrip = this.TagMenu;
            this.vListBox.Dock = DockStyle.Fill;
            this.vListBox.ItemHeight = 34;
            this.vListBox.Location = new Point(0, 44);
            this.vListBox.Name = "VideoPlay_TagP_11";
            this.vListBox.RoundedCornersMaskListItem = (byte)15;
            this.vListBox.SelectionMode = SelectionMode.One;
            this.vListBox.Size = new Size(351, 509);
            this.vListBox.TabIndex = 1;
            this.vListBox.Text = "VideoPlay_TagP_12";
            this.vListBox.VIBlendScrollBarsTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.vListBox.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.TagMenu.Items.AddRange(new ToolStripItem[3]
            {
        (ToolStripItem) this.mnu_SelectAllTags,
        (ToolStripItem) this.mnu_DelselectAllTags,
        (ToolStripItem) this.mnu_DeleteTags
            });
            this.TagMenu.Name = "VideoPlay_TagP_13";
            this.TagMenu.Size = new Size(190, 70);
            this.mnu_SelectAllTags.Name = "VideoPlay_TagP_14";
            this.mnu_SelectAllTags.Size = new Size(189, 22);
            this.mnu_SelectAllTags.Text = "VideoPlay_TagP_15";
            this.mnu_SelectAllTags.Click += new EventHandler(this.mnu_SelectAllTags_Click);
            this.mnu_DelselectAllTags.Name = "VideoPlay_TagP_16";
            this.mnu_DelselectAllTags.Size = new Size(189, 22);
            this.mnu_DelselectAllTags.Text = "VideoPlay_TagP_17";
            this.mnu_DelselectAllTags.Click += new EventHandler(this.mnu_DelselectAllTags_Click);
            this.mnu_DeleteTags.Name = "VideoPlay_TagP_18";
            this.mnu_DeleteTags.Size = new Size(189, 22);
            this.mnu_DeleteTags.Text = "VideoPlay_TagP_19";
            this.mnu_DeleteTags.Click += new EventHandler(this.mnu_DeleteTags_Click);
            this.btn_MergeVideo.AllowAnimations = true;
            this.btn_MergeVideo.BackColor = Color.Transparent;
            this.btn_MergeVideo.Image = (Image)Properties.Resources.frame;
            this.btn_MergeVideo.ImageAlign = ContentAlignment.MiddleLeft;
            this.btn_MergeVideo.Location = new Point(8, 8);
            this.btn_MergeVideo.Name = "VideoPlay_TagP_20";
            this.btn_MergeVideo.RoundedCornersMask = (byte)15;
            this.btn_MergeVideo.RoundedCornersRadius = 0;
            this.btn_MergeVideo.Size = new Size(164, 30);
            this.btn_MergeVideo.TabIndex = 2;
            this.btn_MergeVideo.Text = "VideoPlay_TagP_21";
            this.btn_MergeVideo.UseVisualStyleBackColor = false;
            this.btn_MergeVideo.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_MergeVideo.Click += new EventHandler(this.btn_Merge_Click);
            this.lbl_VideoMarks.Anchor = AnchorStyles.Right;
            this.lbl_VideoMarks.AutoSize = true;
            this.lbl_VideoMarks.BackColor = Color.Transparent;
            this.lbl_VideoMarks.Font = new Font("VideoPlay_TagP_22", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lbl_VideoMarks.ForeColor = Color.White;
            this.lbl_VideoMarks.Location = new Point(252, 17);
            this.lbl_VideoMarks.Name = "VideoPlay_TagP_23";
            this.lbl_VideoMarks.Size = new Size(96, 16);
            this.lbl_VideoMarks.TabIndex = 3;
            this.lbl_VideoMarks.Text = "VideoPlay_TagP_24";
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add((Control)this.lbl_VideoMarks);
            this.HeaderPanel.Controls.Add((Control)this.btn_MergeVideo);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "VideoPlay_TagP_25";
            this.HeaderPanel.Size = new Size(351, 44);
            this.HeaderPanel.TabIndex = 4;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add((Control)this.vListBox);
            this.Controls.Add((Control)this.HeaderPanel);
            this.Name = "VideoPlay_TagP_26";
            this.Size = new Size(351, 553);
            this.Load += new EventHandler(this.TagPanel_Load);
            this.TagMenu.ResumeLayout(false);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        public delegate void DEL_MergeVideo(string folder);
    }
}
