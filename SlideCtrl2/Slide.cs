using AppGlobal;
using MemoEditor;
using MemoList;
using SlideCtrl2.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMInterfaces;
using VMModels.Enums;
using VMModels.Model;

namespace SlideCtrl2
{
    public class Slide : UserControl
    {
        private const int NORMAL = 150;

        private const int MEMO = 250;

        private const int SELECT = 200;

        public const int WM_NCLBUTTONDOWN = 161;

        public const int HT_CAPTION = 2;

        private const int CS_DROPSHADOW = 131072;

        public SlideRecord sRecord = new SlideRecord();

        private Account aRecord = new Account();

        private Image imgRetention = Resources.retention;

        private Image imgShare = Resources.users;

        private Image imgMemo = Resources.notes;

        private Image imgSet = Resources.@group;

        private Image imgLock = Resources.padlock;

        private Image imgStar = Resources.star;

        private Image imgVideo = Resources.video;

        private Image imgRestore = Resources.restore;

        private bool IsVideo;

        private bool IsRestored;

        private string[] VideoFormats = new string[] { ".AVI", ".MOV", ".MP4", ".WMV", ".DIVX", ".MPG", ".MPEG", ".264", ".FLV", ".VOB", ".3GP", ".MKV", ".ASF" };

        private string[] ImageFormats = new string[] { ".JPG", ".BMP", ".PNG", ".GIF" };

        private IContainer components;

        private Label lblNumber;

        private Label lblTimestamp;

        private vRatingControl Rating;

        private PictureBox Thumbnail;

        private Label lblLine;

        private ContextMenuStrip SlideMenu;

        private ToolStripMenuItem mnu_ViewFile;

        private ToolStripSeparator toolStripMenuItem1;

        private ToolStripMenuItem mnu_RemoveSlide;

        private ToolStripMenuItem mnu_DeleteFile;

        private ToolStripMenuItem mnu_ShareFile;

        private ToolStripSeparator toolStripMenuItem2;

        private ToolStripMenuItem mnuAddMemo;

        private ToolStripMenuItem mnu_ReviewMemos;

        private ToolStripSeparator SeperatorLine;

        private ToolStripMenuItem mnuProfileData;

        private ToolStripMenuItem mnu_ExportFile;

        private ToolStripMenuItem mnu_DVD;

        private ToolStripMenuItem mnu_ExportToDevice;

        private Label label_0;

        private Label lblClassification;

        private Label lblSecurity;

        private Label lblDesc;

        private FolderBrowserDialog folderBrowserDialog;

        private ToolStripMenuItem mnu_CloudSync;

        private Label lblRetention;

        private PictureBox picDelete;

        private ToolStripMenuItem mnu_RedactVideo;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                CreateParams classStyle = createParams;
                classStyle.ClassStyle = classStyle.ClassStyle | 131072;
                return createParams;
            }
        }

        public Slide(SlideRecord sRec)
        {
            this.InitializeComponent();
            this.sRecord = sRec;
            (new ToolTip()).SetToolTip(this.lblRetention, "Retention Days Remaining");
            this.IsVideo = false;
            string[] videoFormats = this.VideoFormats;
            int num = 0;
            while (true)
            {
                if (num < (int)videoFormats.Length)
                {
                    string str = videoFormats[num];
                    if (this.sRecord.dRecord.FileExtension.ToUpper().Equals(str))
                    {
                        this.IsVideo = true;
                        break;
                    }
                    else
                    {
                        num++;
                    }
                }
                else
                {
                    break;
                }
            }
            this.SetSlideData();
            if (Global.IsRights(Global.RightsProfile, UserRights.SHARE))
            {
                this.mnu_ShareFile.Visible = true;
            }
            if ((this.VideoFormats.Contains<string>(sRec.dRecord.FileExtension.ToUpper()) || this.ImageFormats.Contains<string>(sRec.dRecord.FileExtension.ToUpper())) && Global.IsRights(Global.RightsProfile, UserRights.REDACT))
            {
                this.mnu_RedactVideo.Visible = true;
            }
            if (sRec != null)
            {
                using (RPM_Account rPMAccount = new RPM_Account())
                {
                    this.aRecord = rPMAccount.GetAccount(sRec.dRecord.AccountId);
                }
                this.sRecord.IsMemo = this.IsMemos();
            }
        }

        public Slide()
        {
            this.InitializeComponent();
        }

        private void Callback(object sender, CmdSlideEventArgs e)
        {
            if (this.EVT_SlideCallback != null)
            {
                this.EVT_SlideCallback(this, e);
            }
        }

        private void childControl_MouseClick(object sender, MouseEventArgs e)
        {
            this.Selection();
        }

        private string CleanInput(string strIn)
        {
            string str;
            try
            {
                str = Regex.Replace(strIn, "[^\\w\\d]", " ");
            }
            catch (Exception exception)
            {
                str = strIn;
            }
            return str;
        }

        public void ClearSelection()
        {
            this.sRecord.IsSelected = false;
            this.BackColor = Color.FromArgb(150, 150, 150);
            this.sRecord.Action = ACTION.SELECT;
            this.Callback(this, new CmdSlideEventArgs(this.sRecord));
        }

        private int CountTrue(params bool[] args)
        {
            return ((IEnumerable<bool>)args).Count<bool>((bool t) => t);
        }

        public bool DeleteSlideFile(int days)
        {
            this.sRecord.Action = ACTION.NONE;
            bool flag = false;
            if (days > 0 && (this.sRecord.dRecord.Classification.ToUpper().Equals("UNCLASSIFIED") || string.IsNullOrEmpty(this.sRecord.dRecord.Classification)))
            {
                DateTime value = this.sRecord.dRecord.FileAddedTimestamp.Value;
                value = value.AddDays((double)days);
                if (DateTime.Now > value)
                {
                    this.picDelete.Visible = true;
                    Application.DoEvents();
                    DateTime now = DateTime.Now;
                    object[] year = new object[] { now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond };
                    string str = string.Format("{0}{1:00}{2:00}_{3:00}{4:00}{5:00}.{6:0000}.zip", year);
                    string str1 = Path.Combine(this.sRecord.dRecord.UNCName, this.sRecord.dRecord.UNCPath);
                    string str2 = Path.Combine(Network.FormatPath(str1), string.Format("{0}{1}", this.sRecord.dRecord.StoredFileName, this.sRecord.dRecord.FileExtension));
                    if (!File.Exists(str2))
                    {
                        MessageBox.Show(this, string.Format(LangCtrl.GetString("sld_SourceFile", "Source File Not Found: {0}"), str2), "File Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    else
                    {
                        string empty = string.Empty;
                        string empty1 = string.Empty;
                        using (RPM_GlobalConfig rPMGlobalConfig = new RPM_GlobalConfig())
                        {
                            GlobalConfig configRecord = rPMGlobalConfig.GetConfigRecord("RET_UNC_ROOT");
                            if (configRecord != null)
                            {
                                empty = configRecord.Value;
                            }
                            configRecord = rPMGlobalConfig.GetConfigRecord("RET_UNC_PATH");
                            if (configRecord != null)
                            {
                                empty1 = configRecord.Value;
                            }
                        }
                        if (!string.IsNullOrEmpty(empty) && !string.IsNullOrEmpty(empty1))
                        {
                            string str3 = Network.FormatPath(Path.Combine(empty, empty1));
                            if (!Directory.Exists(str3))
                            {
                                MessageBox.Show(this, LangCtrl.GetString("sld_ArchiveFolder", "Archive Folder Not Set!\nContact your system administrator."), "Archive Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            }
                            else if (Zip.ZipToFile(str2, str3, str))
                            {
                                using (RPM_DataFile rPMDataFile = new RPM_DataFile())
                                {
                                    DataFile dataFile = rPMDataFile.GetDataFile(this.sRecord.dRecord.Id);
                                    dataFile.PurgeFileName = str;
                                    dataFile.PurgeTimestamp = new DateTime?(DateTime.Now);
                                    dataFile.IsPurged = new bool?(true);
                                    rPMDataFile.SaveUpdate(dataFile);
                                    rPMDataFile.Save();
                                    try
                                    {
                                        File.Delete(str2);
                                    }
                                    catch (Exception exception1)
                                    {
                                        Exception exception = exception1;
                                        MessageBox.Show(this, exception.Message, "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                    }
                                    if (!File.Exists(str2))
                                    {
                                        Global.Log("USER DELETE", string.Format("File: {0}", str2));
                                        flag = true;
                                    }
                                }
                            }
                        }
                    }
                    this.picDelete.Visible = false;
                    Application.DoEvents();
                }
            }
            return flag;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public Guid GetRecordID()
        {
            return this.sRecord.dRecord.Id;
        }

        private void HookupMouseEnterEvents(Control control)
        {
            foreach (Control slideMenu in control.Controls)
            {
                if (slideMenu.Name.Equals("Rating"))
                {
                    continue;
                }
                slideMenu.MouseClick += new MouseEventHandler(this.childControl_MouseClick);
                slideMenu.ContextMenuStrip = this.SlideMenu;
                this.HookupMouseEnterEvents(slideMenu);
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.lblNumber = new Label();
            this.lblTimestamp = new Label();
            this.Rating = new vRatingControl();
            this.lblLine = new Label();
            this.SlideMenu = new ContextMenuStrip(this.components);
            this.mnu_ViewFile = new ToolStripMenuItem();
            this.toolStripMenuItem1 = new ToolStripSeparator();
            this.mnu_RemoveSlide = new ToolStripMenuItem();
            this.mnu_DeleteFile = new ToolStripMenuItem();
            this.mnu_ShareFile = new ToolStripMenuItem();
            this.mnu_RedactVideo = new ToolStripMenuItem();
            this.toolStripMenuItem2 = new ToolStripSeparator();
            this.mnuAddMemo = new ToolStripMenuItem();
            this.mnu_ReviewMemos = new ToolStripMenuItem();
            this.SeperatorLine = new ToolStripSeparator();
            this.mnuProfileData = new ToolStripMenuItem();
            this.mnu_ExportFile = new ToolStripMenuItem();
            this.mnu_DVD = new ToolStripMenuItem();
            this.mnu_ExportToDevice = new ToolStripMenuItem();
            this.mnu_CloudSync = new ToolStripMenuItem();
            this.label_0 = new Label();
            this.lblClassification = new Label();
            this.lblSecurity = new Label();
            this.lblDesc = new Label();
            this.folderBrowserDialog = new FolderBrowserDialog();
            this.lblRetention = new Label();
            this.Thumbnail = new PictureBox();
            this.picDelete = new PictureBox();
            this.SlideMenu.SuspendLayout();
            ((ISupportInitialize)this.Thumbnail).BeginInit();
            ((ISupportInitialize)this.picDelete).BeginInit();
            base.SuspendLayout();
            this.lblNumber.BackColor = Color.FromArgb(224, 224, 224);
            this.lblNumber.BorderStyle = BorderStyle.FixedSingle;
            this.lblNumber.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblNumber.Location = new Point(4, 4);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new Size(73, 23);
            this.lblNumber.TabIndex = 0;
            this.lblNumber.Text = "0000000";
            this.lblNumber.TextAlign = ContentAlignment.MiddleCenter;
            this.lblTimestamp.BackColor = Color.Transparent;
            this.lblTimestamp.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblTimestamp.Location = new Point(83, 8);
            this.lblTimestamp.Name = "lblTimestamp";
            this.lblTimestamp.Size = new Size(164, 14);
            this.lblTimestamp.TabIndex = 1;
            this.lblTimestamp.Text = "00/00/0000 00:00:00";
            this.lblTimestamp.TextAlign = ContentAlignment.MiddleRight;
            this.Rating.BackColor = Color.Transparent;
            this.Rating.Enabled = false;
            this.Rating.Location = new Point(4, 31);
            this.Rating.Margin = new Padding(0);
            this.Rating.Name = "Rating";
            this.Rating.Shape = vRatingControlShapes.Star;
            this.Rating.Size = new Size(75, 14);
            this.Rating.TabIndex = 2;
            this.Rating.Text = "vRatingControl1";
            this.Rating.Value = 0f;
            this.Rating.ValueIndicatorSize = 6;
            this.Rating.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lblLine.BackColor = Color.FromArgb(64, 64, 64);
            this.lblLine.Location = new Point(1, 48);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new Size(249, 1);
            this.lblLine.TabIndex = 4;
            ToolStripItemCollection items = this.SlideMenu.Items;
            ToolStripItem[] mnuViewFile = new ToolStripItem[] { this.mnu_ViewFile, this.toolStripMenuItem1, this.mnu_RemoveSlide, this.mnu_DeleteFile, this.mnu_ShareFile, this.mnu_RedactVideo, this.toolStripMenuItem2, this.mnuAddMemo, this.mnu_ReviewMemos, this.SeperatorLine, this.mnuProfileData, this.mnu_ExportFile, this.mnu_CloudSync };
            this.SlideMenu.Items.AddRange(mnuViewFile);
            this.SlideMenu.Name = "SlideMenu";
            this.SlideMenu.Size = new Size(177, 242);
            this.mnu_ViewFile.Name = "mnu_ViewFile";
            this.mnu_ViewFile.Size = new Size(176, 22);
            this.mnu_ViewFile.Text = "View File...";
            this.mnu_ViewFile.Click += new EventHandler(this.mnu_ViewFile_Click);
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new Size(173, 6);
            this.mnu_RemoveSlide.Name = "mnu_RemoveSlide";
            this.mnu_RemoveSlide.Size = new Size(176, 22);
            this.mnu_RemoveSlide.Text = "Remove From View";
            this.mnu_RemoveSlide.Click += new EventHandler(this.mnu_RemoveSlide_Click);
            this.mnu_DeleteFile.Name = "mnu_DeleteFile";
            this.mnu_DeleteFile.Size = new Size(176, 22);
            this.mnu_DeleteFile.Text = "Delete File...";
            this.mnu_DeleteFile.Click += new EventHandler(this.mnu_DeleteFile_Click);
            this.mnu_ShareFile.Name = "mnu_ShareFile";
            this.mnu_ShareFile.Size = new Size(176, 22);
            this.mnu_ShareFile.Text = "Share File...";
            this.mnu_ShareFile.Visible = false;
            this.mnu_ShareFile.Click += new EventHandler(this.mnu_ShareFile_Click);
            this.mnu_RedactVideo.Name = "mnu_RedactVideo";
            this.mnu_RedactVideo.Size = new Size(176, 22);
            this.mnu_RedactVideo.Text = "Redact Media File";
            this.mnu_RedactVideo.Visible = false;
            this.mnu_RedactVideo.Click += new EventHandler(this.mnu_RedactVideo_Click);
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new Size(173, 6);
            this.mnuAddMemo.Name = "mnuAddMemo";
            this.mnuAddMemo.Size = new Size(176, 22);
            this.mnuAddMemo.Text = "Add Memo";
            this.mnuAddMemo.Click += new EventHandler(this.mnuAddMemo_Click);
            this.mnu_ReviewMemos.Name = "mnu_ReviewMemos";
            this.mnu_ReviewMemos.Size = new Size(176, 22);
            this.mnu_ReviewMemos.Text = "Review Memos...";
            this.mnu_ReviewMemos.Click += new EventHandler(this.mnu_ReviewMemos_Click);
            this.SeperatorLine.Name = "SeperatorLine";
            this.SeperatorLine.Size = new Size(173, 6);
            this.mnuProfileData.Name = "mnuProfileData";
            this.mnuProfileData.Size = new Size(176, 22);
            this.mnuProfileData.Text = "Profile Settings...";
            this.mnuProfileData.Click += new EventHandler(this.mnuProfileData_Click);
            ToolStripItemCollection dropDownItems = this.mnu_ExportFile.DropDownItems;
            ToolStripItem[] mnuDVD = new ToolStripItem[] { this.mnu_DVD, this.mnu_ExportToDevice };
            this.mnu_ExportFile.DropDownItems.AddRange(mnuDVD);
            this.mnu_ExportFile.Name = "mnu_ExportFile";
            this.mnu_ExportFile.Size = new Size(176, 22);
            this.mnu_ExportFile.Text = "Export File";
            this.mnu_DVD.Name = "mnu_DVD";
            this.mnu_DVD.Size = new Size(118, 22);
            this.mnu_DVD.Text = "DVD";
            this.mnu_DVD.Click += new EventHandler(this.mnu_DVD_Click);
            this.mnu_ExportToDevice.Name = "mnu_ExportToDevice";
            this.mnu_ExportToDevice.Size = new Size(118, 22);
            this.mnu_ExportToDevice.Text = "To File...";
            this.mnu_ExportToDevice.Click += new EventHandler(this.mnu_ExportToDevice_Click);
            this.mnu_CloudSync.Name = "mnu_CloudSync";
            this.mnu_CloudSync.Size = new Size(176, 22);
            this.mnu_CloudSync.Text = "Sync to Cloud";
            this.mnu_CloudSync.Visible = false;
            this.label_0.BackColor = Color.Transparent;
            this.label_0.Location = new Point(85, 27);
            this.label_0.Name = "SetIDName";
            this.label_0.Size = new Size(162, 18);
            this.label_0.TabIndex = 6;
            this.label_0.Text = "SET NAME";
            this.label_0.TextAlign = ContentAlignment.MiddleRight;
            this.lblClassification.BackColor = Color.Transparent;
            this.lblClassification.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblClassification.Location = new Point(16, 208);
            this.lblClassification.Name = "lblClassification";
            this.lblClassification.Size = new Size(218, 14);
            this.lblClassification.TabIndex = 7;
            this.lblClassification.Text = "Unclassified";
            this.lblClassification.TextAlign = ContentAlignment.MiddleLeft;
            this.lblSecurity.AutoSize = true;
            this.lblSecurity.BorderStyle = BorderStyle.FixedSingle;
            this.lblSecurity.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblSecurity.Location = new Point(16, 227);
            this.lblSecurity.Name = "lblSecurity";
            this.lblSecurity.Size = new Size(41, 14);
            this.lblSecurity.TabIndex = 8;
            this.lblSecurity.Text = "Security";
            this.lblSecurity.TextAlign = ContentAlignment.MiddleLeft;
            this.lblDesc.BackColor = Color.Transparent;
            this.lblDesc.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblDesc.Location = new Point(16, 189);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new Size(218, 14);
            this.lblDesc.TabIndex = 9;
            this.lblDesc.Text = "Short Description";
            this.lblDesc.TextAlign = ContentAlignment.MiddleLeft;
            this.lblRetention.AutoSize = true;
            this.lblRetention.BackColor = Color.White;
            this.lblRetention.BorderStyle = BorderStyle.FixedSingle;
            this.lblRetention.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblRetention.Location = new Point(17, 170);
            this.lblRetention.Name = "lblRetention";
            this.lblRetention.Size = new Size(2, 14);
            this.lblRetention.TabIndex = 10;
            this.lblRetention.TextAlign = ContentAlignment.MiddleLeft;
            this.lblRetention.Visible = false;
            this.Thumbnail.BackColor = Color.Black;
            this.Thumbnail.Location = new Point(15, 55);
            this.Thumbnail.Name = "Thumbnail";
            this.Thumbnail.Size = new Size(220, 130);
            this.Thumbnail.SizeMode = PictureBoxSizeMode.CenterImage;
            this.Thumbnail.TabIndex = 3;
            this.Thumbnail.TabStop = false;
            this.Thumbnail.Paint += new PaintEventHandler(this.Thumbnail_Paint);
            this.picDelete.BackColor = Color.White;
            this.picDelete.BorderStyle = BorderStyle.FixedSingle;
            this.picDelete.Image = Resources.trashcan;
            this.picDelete.Location = new Point(212, 219);
            this.picDelete.Name = "picDelete";
            this.picDelete.Size = new Size(22, 22);
            this.picDelete.SizeMode = PictureBoxSizeMode.CenterImage;
            this.picDelete.TabIndex = 11;
            this.picDelete.TabStop = false;
            this.picDelete.Visible = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.DarkGray;
            this.ContextMenuStrip = this.SlideMenu;
            base.Controls.Add(this.picDelete);
            base.Controls.Add(this.lblRetention);
            base.Controls.Add(this.lblDesc);
            base.Controls.Add(this.lblSecurity);
            base.Controls.Add(this.lblClassification);
            base.Controls.Add(this.label_0);
            base.Controls.Add(this.lblLine);
            base.Controls.Add(this.Thumbnail);
            base.Controls.Add(this.Rating);
            base.Controls.Add(this.lblTimestamp);
            base.Controls.Add(this.lblNumber);
            this.DoubleBuffered = true;
            base.Margin = new Padding(0);
            base.Name = "Slide";
            base.Size = new Size(250, 250);
            base.Load += new EventHandler(this.Slide_Load);
            base.Paint += new PaintEventHandler(this.Slide_Paint);
            base.MouseDown += new MouseEventHandler(this.Slide_MouseDown);
            this.SlideMenu.ResumeLayout(false);
            ((ISupportInitialize)this.Thumbnail).EndInit();
            ((ISupportInitialize)this.picDelete).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private bool IsBoundry()
        {
            bool flag = false;
            if (base.ClientRectangle.Contains(base.PointToClient(Control.MousePosition)))
            {
                flag = true;
            }
            return flag;
        }

        private bool IsMemos()
        {
            bool flag = false;
            using (RPM_DataFile rPMDataFile = new RPM_DataFile())
            {
                if (rPMDataFile.GetMemoCount(this.sRecord.dRecord.Id) > 0)
                {
                    flag = true;
                }
            }
            return flag;
        }

        public string MediaFile()
        {
            string str = Path.Combine(this.sRecord.dRecord.UNCPath, string.Concat(this.sRecord.dRecord.StoredFileName, this.sRecord.dRecord.FileExtension));
            return str;
        }

        private void mnu_DeleteFile_Click(object sender, EventArgs e)
        {
            try
            {
                this.Selected();
                this.sRecord.Action = ACTION.DELETE;
                this.Callback(this, new CmdSlideEventArgs(this.sRecord));
            }
            catch
            {
            }
        }

        private void mnu_DVD_Click(object sender, EventArgs e)
        {
            this.Selected();
            this.PackageCallback(this, new CmdPackageEventArgs(""));
        }

        private void mnu_ExportToDevice_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.Selected();
                this.PackageCallback(this, new CmdPackageEventArgs(this.folderBrowserDialog.SelectedPath));
            }
        }

        private void mnu_RedactVideo_Click(object sender, EventArgs e)
        {
            string str = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Redact\\VideoEditor.exe");
            if (File.Exists(str))
            {
                try
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo()
                    {
                        FileName = str
                    };
                    string str1 = Path.Combine(this.sRecord.dRecord.UNCName, this.sRecord.dRecord.UNCPath);
                    string str2 = string.Concat(Path.Combine(str1, this.sRecord.dRecord.StoredFileName), this.sRecord.dRecord.FileExtension);
                    if (File.Exists(str2))
                    {
                        Global.Log("REDACT", string.Format("Media File: {0}", str2));
                        processStartInfo.Arguments = string.Concat("\"", str2, "\"");
                        Process.Start(processStartInfo);
                    }
                }
                catch
                {
                }
            }
        }

        private void mnu_RemoveSlide_Click(object sender, EventArgs e)
        {
            try
            {
                this.sRecord.Action = ACTION.REMOVE;
                this.Callback(this, new CmdSlideEventArgs(this.sRecord));
                base.Parent.Controls.Remove(this);
            }
            catch
            {
            }
        }

        private void mnu_ReviewMemos_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            MemoForm memoForm = new MemoForm(this.sRecord.dRecord.Id, this.sRecord.dRecord.ShortDesc);
            memoForm.ShowDialog(this);
            this.SetSelectMode();
        }

        private void mnu_ShareFile_Click(object sender, EventArgs e)
        {
            this.Selected();
            try
            {
                this.sRecord.Action = ACTION.SHARE;
                this.Callback(this, new CmdSlideEventArgs(this.sRecord));
            }
            catch (Exception exception)
            {
            }
        }

        private void mnu_ViewFile_Click(object sender, EventArgs e)
        {
            this.Selected();
            string fileExtension = this.sRecord.dRecord.FileExtension;
            bool flag = true;
            using (RPM_FileType rPMFileType = new RPM_FileType())
            {
                flag = rPMFileType.IsInternal(fileExtension.ToUpper());
            }
            if (flag)
            {
                this.sRecord.Action = ACTION.PLAYLIST;
                this.Callback(this, new CmdSlideEventArgs(this.sRecord));
                return;
            }
            if (Global.IsRights(Global.RightsProfile, UserRights.WINDOW_APPS))
            {
                try
                {
                    string str = Path.Combine(this.sRecord.dRecord.UNCName, this.sRecord.dRecord.UNCPath);
                    string str1 = string.Concat(Path.Combine(str, this.sRecord.dRecord.StoredFileName), this.sRecord.dRecord.FileExtension);
                    if (!File.Exists(str1))
                    {
                        MessageBox.Show(this, string.Format(LangCtrl.GetString("sld_FileNotAvail", "File not available:\n{0}{1}\nPlease contact your administrator."), this.sRecord.dRecord.StoredFileName, this.sRecord.dRecord.FileExtension), "File", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    else
                    {
                        ProcessStartInfo processStartInfo = new ProcessStartInfo()
                        {
                            FileName = str1,
                            WindowStyle = ProcessWindowStyle.Normal,
                            UseShellExecute = true,
                            ErrorDialog = true
                        };
                        Process.Start(processStartInfo);
                    }
                }
                catch (Exception exception)
                {
                    string message = exception.Message;
                }
            }
        }

        private void mnuAddMemo_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            EditorForm editorForm = new EditorForm(this.sRecord.SlideNumber);
            if (editorForm.ShowDialog(this) == DialogResult.OK)
            {
                FileMemo fileMemo = new FileMemo()
                {
                    AccountName = this.aRecord.ToString(),
                    BadgeNumber = this.aRecord.BadgeNumber,
                    Timestamp = new DateTime?(DateTime.Now),
                    Memo = editorForm.RTF,
                    Text = editorForm.DocText
                };
                if (fileMemo.Text.Length >= 64)
                {
                    fileMemo.ShortDesc = this.CleanInput(fileMemo.Text.Substring(0, 64));
                }
                else
                {
                    fileMemo.ShortDesc = this.CleanInput(fileMemo.Text);
                }
                using (RPM_DataFile rPMDataFile = new RPM_DataFile())
                {
                    DataFile dataFile = rPMDataFile.GetDataFile(this.sRecord.dRecord.Id);
                    fileMemo.DataFile = dataFile;
                    dataFile.FileMemos.Add(fileMemo);
                    rPMDataFile.SaveUpdate(dataFile);
                    rPMDataFile.Save();
                }
                this.sRecord.IsMemo = true;
                this.Thumbnail.Invalidate();
                this.SetSelectMode();
            }
        }

        private void mnuProfileData_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            ProfileData profileDatum = new ProfileData(this.sRecord);
            using (RPM_DataFile rPMDataFile = new RPM_DataFile())
            {
                rPMDataFile.ViewFileData(this.sRecord.dRecord.Id);
            }
            if (profileDatum.ShowDialog(this) == DialogResult.OK)
            {
                this.sRecord = profileDatum.sRecord;
                using (RPM_DataFile rPMDataFile1 = new RPM_DataFile())
                {
                    this.sRecord.Action = ACTION.UPDATE;
                    rPMDataFile1.SaveUpdate(this.sRecord.dRecord);
                    rPMDataFile1.Save();
                    this.lblDesc.Text = this.sRecord.dRecord.ShortDesc;
                    this.Callback(this, new CmdSlideEventArgs(this.sRecord));
                }
                this.SetSlideData();
            }
            this.SetSelectMode();
        }

        private void PackageCallback(object sender, CmdPackageEventArgs e)
        {
            if (this.EVT_PackageCallback != null)
            {
                this.EVT_PackageCallback(this, e);
            }
        }

        private void PlayMedia()
        {
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool ReleaseCapture();

        public void RetentionDisplay()
        {
            this.lblRetention.Visible = false;
            if (!this.sRecord.dRecord.IsIndefinite && this.sRecord.ClassDays > -10000)
            {
                if (Global.ClassificationDays.ContainsKey(this.sRecord.dRecord.Classification))
                {
                    this.sRecord.ClassDays = Global.ClassificationDays[this.sRecord.dRecord.Classification];
                }
                this.lblRetention.BackColor = Color.White;
                DateTime now = DateTime.Now;
                DateTime? fileAddedTimestamp = this.sRecord.dRecord.FileAddedTimestamp;
                TimeSpan timeSpan = now.Subtract(fileAddedTimestamp.Value);
                int classDays = this.sRecord.ClassDays - timeSpan.Days;
                this.lblRetention.Text = string.Format("{0:00}", classDays);
                if (classDays <= 7)
                {
                    this.lblRetention.BackColor = Color.Yellow;
                }
                this.lblRetention.Visible = true;
            }
        }

        private void SecurityProfile()
        {
            if (!Global.IsRights(Global.RightsProfile, UserRights.DVD))
            {
                this.mnu_DVD.Visible = false;
            }
            if (!Global.IsRights(Global.RightsProfile, UserRights.DVD))
            {
                this.mnu_DVD.Visible = false;
            }
            if (!Global.IsRights(Global.RightsProfile, UserRights.EXPORT))
            {
                this.mnu_ExportFile.Visible = false;
            }
            if (!Global.IsRights(Global.RightsProfile, UserRights.SETS))
            {
                this.mnuProfileData.Visible = false;
            }
            if (!Global.IsRights(Global.RightsProfile, UserRights.VIDEO))
            {
                this.mnu_ViewFile.Visible = false;
            }
            if (!Global.IsRights(Global.RightsProfile, UserRights.DELETE))
            {
                this.mnu_DeleteFile.Visible = false;
            }
            if (!this.sRecord.IsDelete)
            {
                this.mnu_DeleteFile.Visible = false;
            }
            if (!Global.IsRights(Global.RightsProfile, UserRights.SHARE))
            {
                this.mnu_ShareFile.Visible = false;
            }
            if (!Global.IsRights(Global.RightsProfile, UserRights.MEMO_CREATE))
            {
                this.mnuAddMemo.Visible = false;
            }
            if (!Global.IsRights(Global.RightsProfile, UserRights.MEMO_VIEW))
            {
                this.mnu_ReviewMemos.Visible = false;
            }
        }

        private void Selected()
        {
            this.sRecord.IsSelected = true;
            this.BackColor = Color.FromArgb(200, 200, 200);
        }

        private void Selection()
        {
            if (this.IsBoundry())
            {
                this.sRecord.IsSelected = !this.sRecord.IsSelected;
                if (Control.ModifierKeys == Keys.Shift)
                {
                    this.sRecord.Action = ACTION.RANGE;
                }
                this.Callback(this, new CmdSlideEventArgs(this.sRecord));
                if (!this.sRecord.IsSelected)
                {
                    this.BackColor = Color.FromArgb(150, 150, 150);
                }
                else
                {
                    this.BackColor = Color.FromArgb(200, 200, 200);
                }
            }
            this.sRecord.Action = ACTION.SELECT;
            this.Callback(this, new CmdSlideEventArgs(this.sRecord));
        }

        public void SelectSlide()
        {
            this.sRecord.IsSelected = true;
            this.BackColor = Color.FromArgb(200, 200, 200);
            this.sRecord.Action = ACTION.SELECT;
            this.Callback(this, new CmdSlideEventArgs(this.sRecord));
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void SetLanguage()
        {
            LangCtrl.reText(this);
            this.mnu_RemoveSlide.Text = LangCtrl.GetString("mnu_RemoveSlide", "Remove Slide");
            this.mnuAddMemo.Text = LangCtrl.GetString("mnuAddMemo", "Add Memo");
            this.mnu_ViewFile.Text = LangCtrl.GetString("mnu_ViewFile", "View File");
            this.mnu_DeleteFile.Text = LangCtrl.GetString("mnu_DeleteFile", "Delete File");
            this.mnu_ReviewMemos.Text = LangCtrl.GetString("mnu_ReviewMemos", "Review Memos");
            this.mnuProfileData.Text = LangCtrl.GetString("mnuProfileData", "Profile Settings");
            this.mnu_ExportFile.Text = LangCtrl.GetString("mnu_ExportFile", "Export File");
            this.mnu_DVD.Text = LangCtrl.GetString("mnu_DVD", "DVD");
            this.mnu_ExportToDevice.Text = LangCtrl.GetString("mnu_ExportToDevice", "To Device");
            this.mnu_CloudSync.Text = LangCtrl.GetString("mnu_CloudSync", "Sync to Cloud");
            this.mnu_ShareFile.Text = LangCtrl.GetString("mnu_ShareFile", "Share File...");
        }

        private void SetSelectMode()
        {
            if (this.sRecord.IsSelected)
            {
                this.BackColor = Color.FromArgb(200, 200, 200);
                return;
            }
            this.BackColor = Color.FromArgb(150, 150, 150);
        }

        private void SetSlideData()
        {
            try
            {
                this.lblNumber.Text = Convert.ToString(this.sRecord.SlideNumber);
                this.lblTimestamp.Text = string.Format("{0}", this.sRecord.dRecord.FileTimestamp);
                this.Rating.Value = (float)this.sRecord.dRecord.Rating;
                Image image = Utilities.ByteArrayToImage(this.sRecord.dRecord.Thumbnail);
                this.Thumbnail.Image = image;
                this.label_0.Text = this.sRecord.dRecord.SetName;
                this.lblClassification.Text = this.sRecord.dRecord.Classification;
                this.lblDesc.Text = this.sRecord.dRecord.ShortDesc;
                this.sRecord.IsSet = !string.IsNullOrEmpty(this.sRecord.dRecord.SetName);
                this.lblSecurity.BackColor = Color.Transparent;
                switch (this.sRecord.dRecord.Security)
                {
                    case SECURITY.TOPSECRET:
                        {
                            this.lblSecurity.ForeColor = Color.Yellow;
                            this.lblSecurity.BackColor = Color.Red;
                            break;
                        }
                    case SECURITY.SECRET:
                        {
                            this.lblSecurity.ForeColor = Color.White;
                            this.lblSecurity.BackColor = Color.Orange;
                            break;
                        }
                    case SECURITY.OFFICIAL:
                        {
                            this.lblSecurity.ForeColor = Color.Black;
                            this.lblSecurity.BackColor = Color.Yellow;
                            break;
                        }
                }
                this.lblSecurity.Text = AccountSecurity.GetSecurityDesc(this.sRecord.dRecord.Security);
                this.RetentionDisplay();
            }
            catch
            {
            }
        }

        private void Slide_Load(object sender, EventArgs e)
        {
            this.HookupMouseEnterEvents(this);
            this.BackColor = Color.FromArgb(150, 150, 150);
            this.SecurityProfile();
            this.SetLanguage();
        }

        private void Slide_MouseDown(object sender, MouseEventArgs e)
        {
            this.Selection();
        }

        private void Slide_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder3D(e.Graphics, ((Control)sender).ClientRectangle, Border3DStyle.RaisedOuter);
        }

        private void Thumbnail_Paint(object sender, PaintEventArgs e)
        {
            int height = this.Thumbnail.Height;
            int width = this.Thumbnail.Width;
            int num = 18;
            int num1 = 18;
            bool[] isRet = new bool[] { this.sRecord.IsRet, this.sRecord.IsShare, this.sRecord.IsSet, this.sRecord.IsMemo, this.sRecord.dRecord.IsIndefinite, this.sRecord.dRecord.IsEvidence, this.IsVideo };
            if (this.CountTrue(isRet) > 0)
            {
                if (!this.IsVideo)
                {
                    num = 18;
                }
                else
                {
                    num = 16;
                    e.Graphics.DrawImage(this.imgVideo, width - 16, height - num1);
                    num = 16 + num1;
                }
                bool? isPurged = this.sRecord.dRecord.IsPurged;
                if ((isPurged.GetValueOrDefault() ? false : isPurged.HasValue) && this.sRecord.dRecord.PurgeTimestamp.HasValue)
                {
                    this.lblNumber.BackColor = Color.Yellow;
                    this.IsRestored = true;
                }
                if (this.sRecord.IsRet)
                {
                    e.Graphics.DrawImage(this.imgRetention, width - num, height - num1);
                    num += num1;
                }
                if (this.sRecord.IsShare)
                {
                    e.Graphics.DrawImage(this.imgShare, width - num, height - num1);
                    num += num1;
                }
                if (this.sRecord.IsSet)
                {
                    e.Graphics.DrawImage(this.imgSet, width - num, height - num1);
                    num += num1;
                }
                if (this.sRecord.IsMemo)
                {
                    e.Graphics.DrawImage(this.imgMemo, width - num, height - num1);
                    num += num1;
                }
                if (this.sRecord.dRecord.IsIndefinite)
                {
                    e.Graphics.DrawImage(this.imgLock, width - num, height - num1);
                    num += num1;
                }
                if (this.sRecord.dRecord.IsEvidence)
                {
                    e.Graphics.DrawImage(this.imgStar, width - num, height - num1);
                    num += num1;
                }
                if (this.IsRestored)
                {
                    e.Graphics.DrawImage(this.imgRestore, width - num, height - num1);
                    num += num1;
                }
            }
        }

        public void UpdateSlideData(DataFile dRec)
        {
            this.sRecord.dRecord.Classification = dRec.Classification;
            this.sRecord.dRecord.SetName = dRec.SetName;
            this.sRecord.dRecord.ShortDesc = dRec.ShortDesc;
            this.sRecord.dRecord.Security = dRec.Security;
            this.sRecord.dRecord.Rating = dRec.Rating;
            this.sRecord.dRecord.CADNumber = (dRec.CADNumber);
            this.sRecord.dRecord.RMSNumber = (dRec.RMSNumber);
            this.sRecord.dRecord.IsIndefinite = dRec.IsIndefinite;
            this.sRecord.dRecord.IsEvidence = dRec.IsEvidence;
            this.RetentionDisplay();
            this.SetSlideData();
        }

        public void UpdateSlideNumber(int num)
        {
            this.sRecord.SlideNumber = num;
            this.lblNumber.Text = Convert.ToString(this.sRecord.SlideNumber);
        }

        public event Slide.DEL_PackageCallback EVT_PackageCallback;

        public event Slide.DEL_SlideCallback EVT_SlideCallback;

        public delegate void DEL_PackageCallback(object sender, CmdPackageEventArgs e);

        public delegate void DEL_SlideCallback(object sender, CmdSlideEventArgs e);
    }
}