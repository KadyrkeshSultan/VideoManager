using AppGlobal;
using AxAXVLC;
using AXVLC;
using VMMapEngine;
using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VideoPlayer2.Properties;
using VMInterfaces;
using VMModels.Model;
using VMModels.Enums;

namespace VideoPlayer2
{
    public class VideoForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;

        public const int HT_CAPTION = 2;

        private const int CS_DROPSHADOW = 131072;

        private MapPanel2 mapPanel;

        private ThumbPanel thumbPanel;

        private TagPanel tagPanel;

        private FilePanel filePanel;

        private ImageFilePanel imgPanel;

        private int FileLength;

        private double FPS;

        private int fileIndex;

        private bool IsLoop;

        private int StartMS;

        private int EndMS;

        private int StartFrame;

        private int EndFrame;

        private List<Guid> FileID = new List<Guid>();

        private List<MediaFile> Media = new List<MediaFile>();

        private List<ImageFile> Images = new List<ImageFile>();

        private Guid AccountID = Guid.Empty;

        private VideoTag vTag = new VideoTag();

        private bool IsCmdPanel = true;

        private bool IsPlaying;

        private int TagIndex;

        private string StartTag = string.Empty;

        private string EndTag = string.Empty;

        private bool IsPlayTags;

        private IContainer components;

        private Panel FormPanel;

        private Panel HeaderPanel;

        private Panel ControlPanel;

        private vButton btnClose;

        private PictureBox LogoPic;

        private vButton btnStop;

        private vButton btnPlay;

        private Label lblFrame;

        private Label lblTime;

        private vButton btnCtrlPanel;

        private vButton btnNext;

        private vButton btnPrev;

        private vButton btnFramePlus;

        private vButton btnFrameMinus;

        private TrackBar VolumeBar;

        private TrackBar SpeedBar;

        private vButton btnThumbnails;

        private vButton btn_Map;

        private TableLayoutPanel TrackbarTable;

        private vTrackBar VideoBar;

        private vButton btnSnapshot;

        private Label lblSpeed;

        private vButton btnFiles;

        private vButton btnTags;

        private OpenFileDialog openFileDialog1;

        private Label lbl_File;

        private Label lbl_TagState;

        private ContextMenuStrip VideoMenu;

        private ToolStripMenuItem mnu_TagStart;

        private ToolStripMenuItem mnu_TagEnd;

        private ToolStripMenuItem mnu_TagClear;

        private ToolStripSeparator toolStripMenuItem1;

        private ToolStripMenuItem mnu_TagSave;

        private ToolStripMenuItem mnu_TagLoop;

        private Panel OptionPanel;

        private Panel MenuPanel;

        private Label lbl_SecurityLevel;

        private System.Windows.Forms.Timer timer1;

        private Label lbl_VideoTime;

        private Label lblVideoTitle;

        private vCheckBox chk_TagsOnly;

        private Label lbl_Filedate;

        private Label lbl_Classification;

        private Label lblSet;

        private PictureBox picEvidence;

        private Panel VideoPanel;

        private AxVLCPlugin2 vlc;

        private Panel VCRPanel;

        private PictureBox VolPic;

        private vButton btnImageFiles;

        private Label lbl_ImageFileCount;

        private Label lbl_VideoFileCount;

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

        public VideoForm()
        {
            try
            {
                this.InitializeComponent();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void btn_Map_Click(object sender, EventArgs e)
        {
            this.LoadOptionControl(this.mapPanel);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.tagPanel.EVT_MergeVideo -= new TagPanel.DEL_MergeVideo(this.tagPanel_EVT_MergeVideo);
            this.mapPanel.EVT_Compass -= new MapPanel2.DEL_Compass(this.mapPanel_EVT_Compass);
            if (this.vlc != null)
            {
                try
                {
                    this.vlc.MediaPlayerTimeChanged -= new AxAXVLC.DVLCEvents_MediaPlayerTimeChangedEventHandler(this.vlc_MediaPlayerTimeChanged);
                    this.vlc.playlist.stop();
                }
                catch
                {
                }
            }
            Global.Log("CLOSE", "Media Player");
            Application.DoEvents();
            base.Close();
        }

        private void btnCtrlPanel_Click(object sender, EventArgs e)
        {
            this.IsCmdPanel = !this.IsCmdPanel;
            this.ControlPanel.Visible = this.IsCmdPanel;
        }

        private void btnFiles_Click(object sender, EventArgs e)
        {
            this.LoadOptionControl(this.filePanel);
        }

        private void btnFrameMinus_Click(object sender, EventArgs e)
        {
            double value = (double)((double)this.VideoBar.Value - this.FPS);
            if (value >= 0)
            {
                this.vlc.input.Time = value;
                this.VideoBar.Value = (int)value;
                this.Update((int)value);
            }
        }

        private void btnFramePlus_Click(object sender, EventArgs e)
        {
            double value = (double)((double)this.VideoBar.Value + this.FPS);
            if (value <= (double)this.FileLength)
            {
                this.vlc.input.Time = value;
                this.VideoBar.Value = (int)value;
                this.Update((int)value);
            }
        }

        private void btnImageFiles_Click(object sender, EventArgs e)
        {
            this.LoadOptionControl(this.imgPanel);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            VideoForm videoForm = this;
            int num = videoForm.fileIndex + 1;
            int num1 = num;
            videoForm.fileIndex = num;
            if (num1 > this.Media.Count - 1)
            {
                this.fileIndex = 0;
            }
            this.MoveToVideo();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            this.PlayVideo();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            VideoForm videoForm = this;
            int num = videoForm.fileIndex - 1;
            int num1 = num;
            videoForm.fileIndex = num;
            if (num1 < 0)
            {
                this.fileIndex = this.Media.Count - 1;
            }
            this.MoveToVideo();
        }

        private void btnSnapshot_Click(object sender, EventArgs e)
        {
            (new Thread(new ThreadStart(this.WriteSnapshot))).Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.StopPlayer();
        }

        private void btnTags_Click(object sender, EventArgs e)
        {
            this.LoadOptionControl(this.tagPanel);
        }

        private void btnThumbnails_Click(object sender, EventArgs e)
        {
            this.LoadOptionControl(this.thumbPanel);
        }

        private void CheckScroll()
        {
            int value = this.VideoBar.Value;
            if (this.IsLoop && this.EndMS > 0 && value > this.EndMS)
            {
                value = this.StartMS;
            }
            this.vlc.input.Time = (double)value;
            if (!this.vlc.playlist.isPlaying)
            {
                this.Update((int)((double)((double)value + this.FPS)));
            }
        }

        private void chk_TagsOnly_CheckedChanged(object sender, EventArgs e)
        {
            this.IsPlayTags = !this.IsPlayTags;
            if (!this.IsPlayTags)
            {
                this.VideoBar.Enabled = true;
                this.lbl_TagState.Text = LangCtrl.GetString("vf_Mark", "MARK");
                return;
            }
            this.TagIndex = 0;
            this.VideoBar.Enabled = false;
            this.PlayTags(0);
            this.lbl_TagState.Text = LangCtrl.GetString("vf_MarkPlaying", "MARK - Playing");
        }

        private void ClearTAG()
        {
            string empty = string.Empty;
            string str = empty;
            this.EndTag = empty;
            this.StartTag = str;
            this.lbl_TagState.Text = LangCtrl.GetString("vf_Mark", "MARK");
            this.EndMS = 0;
            this.StartMS = 0;
            this.EndFrame = 0;
            this.StartFrame = 0;
            this.IsLoop = false;
            this.mnu_TagLoop.Checked = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EOF()
        {
            VideoForm videoForm = this;
            int num = videoForm.fileIndex + 1;
            int num1 = num;
            videoForm.fileIndex = num;
            if (num1 > this.Media.Count - 1)
            {
                this.fileIndex = 0;
            }
            this.lbl_File.Text = string.Format(LangCtrl.GetString("vf_File", "File {0} : {1}"), this.fileIndex + 1, this.Media.Count);
            this.LoadGPS();
            this.vlc.playlist.playItem(this.fileIndex);
            this.FPS = this.vlc.input.fps;
            if (this.FPS == 0)
            {
                this.FPS = 30;
            }
            this.FileLength = 0;
            while (this.FileLength == 0)
            {
                this.FileLength = (int)this.vlc.input.Length;
            }
            this.TagIndex = 0;
            this.VideoBar.Maximum = this.FileLength;
            this.UpdateVideoTime();
            this.filePanel.LoadData(this.Media[this.fileIndex].FileID);
        }

        private void GetFileLength()
        {
            this.FileLength = (int)this.vlc.input.Length;
            while (this.FileLength == 0)
            {
                this.FileLength = (int)this.vlc.input.Length;
            }
            this.VideoBar.Maximum = this.FileLength;
        }

        private int GetFrame()
        {
            int value = this.VideoBar.Value;
            return (int)((double)value / (1000 / this.FPS));
        }

        private void HeaderMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                VideoForm.ReleaseCapture();
                VideoForm.SendMessage(base.Handle, 161, 2, 0);
            }
        }

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void InitControls()
        {
            this.mapPanel = new MapPanel2();
            this.mapPanel.EVT_Compass -= new MapPanel2.DEL_Compass(this.mapPanel_EVT_Compass);
            this.mapPanel.EVT_Compass += new MapPanel2.DEL_Compass(this.mapPanel_EVT_Compass);
            Mapper.SetMapper(this.mapPanel.GetMapObject());
            this.LoadOptionControl(this.mapPanel);
            this.thumbPanel = new ThumbPanel();
            this.imgPanel = new ImageFilePanel();
            this.tagPanel = new TagPanel();
            this.tagPanel.EVT_MergeVideo -= new TagPanel.DEL_MergeVideo(this.tagPanel_EVT_MergeVideo);
            this.tagPanel.EVT_MergeVideo += new TagPanel.DEL_MergeVideo(this.tagPanel_EVT_MergeVideo);
            this.filePanel = new FilePanel();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(VideoForm));
            this.FormPanel = new Panel();
            this.VCRPanel = new Panel();
            this.VolPic = new PictureBox();
            this.btnStop = new vButton();
            this.btnPlay = new vButton();
            this.picEvidence = new PictureBox();
            this.lblTime = new Label();
            this.lblSet = new Label();
            this.lblFrame = new Label();
            this.lbl_Classification = new Label();
            this.btnPrev = new vButton();
            this.lbl_Filedate = new Label();
            this.btnNext = new vButton();
            this.chk_TagsOnly = new vCheckBox();
            this.btnFrameMinus = new vButton();
            this.lbl_SecurityLevel = new Label();
            this.btnFramePlus = new vButton();
            this.lblSpeed = new Label();
            this.SpeedBar = new TrackBar();
            this.btnSnapshot = new vButton();
            this.VolumeBar = new TrackBar();
            this.VideoPanel = new Panel();
            this.vlc = new AxVLCPlugin2();
            this.TrackbarTable = new TableLayoutPanel();
            this.VideoBar = new vTrackBar();
            this.VideoMenu = new ContextMenuStrip(this.components);
            this.mnu_TagStart = new ToolStripMenuItem();
            this.mnu_TagEnd = new ToolStripMenuItem();
            this.mnu_TagClear = new ToolStripMenuItem();
            this.toolStripMenuItem1 = new ToolStripSeparator();
            this.mnu_TagSave = new ToolStripMenuItem();
            this.mnu_TagLoop = new ToolStripMenuItem();
            this.lbl_File = new Label();
            this.lbl_TagState = new Label();
            this.lbl_VideoTime = new Label();
            this.ControlPanel = new Panel();
            this.OptionPanel = new Panel();
            this.MenuPanel = new Panel();
            this.btnImageFiles = new vButton();
            this.btn_Map = new vButton();
            this.btnFiles = new vButton();
            this.btnTags = new vButton();
            this.btnThumbnails = new vButton();
            this.HeaderPanel = new Panel();
            this.lbl_ImageFileCount = new Label();
            this.lbl_VideoFileCount = new Label();
            this.lblVideoTitle = new Label();
            this.btnCtrlPanel = new vButton();
            this.LogoPic = new PictureBox();
            this.btnClose = new vButton();
            this.openFileDialog1 = new OpenFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.FormPanel.SuspendLayout();
            this.VCRPanel.SuspendLayout();
            ((ISupportInitialize)this.VolPic).BeginInit();
            ((ISupportInitialize)this.picEvidence).BeginInit();
            ((ISupportInitialize)this.SpeedBar).BeginInit();
            ((ISupportInitialize)this.VolumeBar).BeginInit();
            this.VideoPanel.SuspendLayout();
            ((ISupportInitialize)this.vlc).BeginInit();
            this.TrackbarTable.SuspendLayout();
            this.VideoMenu.SuspendLayout();
            this.ControlPanel.SuspendLayout();
            this.MenuPanel.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((ISupportInitialize)this.LogoPic).BeginInit();
            base.SuspendLayout();
            this.FormPanel.BackColor = Color.White;
            this.FormPanel.BorderStyle = BorderStyle.FixedSingle;
            this.FormPanel.Controls.Add(this.VCRPanel);
            this.FormPanel.Controls.Add(this.VideoPanel);
            this.FormPanel.Controls.Add(this.TrackbarTable);
            this.FormPanel.Controls.Add(this.ControlPanel);
            this.FormPanel.Controls.Add(this.HeaderPanel);
            this.FormPanel.Dock = DockStyle.Fill;
            this.FormPanel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.FormPanel.Location = new Point(0, 0);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new Size(1000, 650);
            this.FormPanel.TabIndex = 0;
            this.VCRPanel.Controls.Add(this.VolPic);
            this.VCRPanel.Controls.Add(this.btnStop);
            this.VCRPanel.Controls.Add(this.btnPlay);
            this.VCRPanel.Controls.Add(this.picEvidence);
            this.VCRPanel.Controls.Add(this.lblTime);
            this.VCRPanel.Controls.Add(this.lblSet);
            this.VCRPanel.Controls.Add(this.lblFrame);
            this.VCRPanel.Controls.Add(this.lbl_Classification);
            this.VCRPanel.Controls.Add(this.btnPrev);
            this.VCRPanel.Controls.Add(this.lbl_Filedate);
            this.VCRPanel.Controls.Add(this.btnNext);
            this.VCRPanel.Controls.Add(this.chk_TagsOnly);
            this.VCRPanel.Controls.Add(this.btnFrameMinus);
            this.VCRPanel.Controls.Add(this.lbl_SecurityLevel);
            this.VCRPanel.Controls.Add(this.btnFramePlus);
            this.VCRPanel.Controls.Add(this.lblSpeed);
            this.VCRPanel.Controls.Add(this.SpeedBar);
            this.VCRPanel.Controls.Add(this.btnSnapshot);
            this.VCRPanel.Controls.Add(this.VolumeBar);
            this.VCRPanel.Dock = DockStyle.Bottom;
            this.VCRPanel.Location = new Point(0, 544);
            this.VCRPanel.Name = "VCRPanel";
            this.VCRPanel.Size = new Size(647, 104);
            this.VCRPanel.TabIndex = 2;
            this.VolPic.Cursor = Cursors.Hand;
            this.VolPic.Image = Properties.Resources.volume;
            this.VolPic.Location = new Point(401, 75);
            this.VolPic.Name = "VolPic";
            this.VolPic.Size = new Size(20, 20);
            this.VolPic.SizeMode = PictureBoxSizeMode.CenterImage;
            this.VolPic.TabIndex = 42;
            this.VolPic.TabStop = false;
            this.VolPic.MouseClick += new MouseEventHandler(this.VolPic_MouseClick);
            this.btnStop.AllowAnimations = true;
            this.btnStop.BackColor = Color.Transparent;
            this.btnStop.Image = Properties.Resources.stop;
            this.btnStop.Location = new Point(58, 3);
            this.btnStop.Name = "btnStop";
            this.btnStop.RoundedCornersMask = 15;
            this.btnStop.RoundedCornersRadius = 0;
            this.btnStop.Size = new Size(28, 28);
            this.btnStop.TabIndex = 11;
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnStop.Click += new EventHandler(this.btnStop_Click);
            this.btnPlay.AllowAnimations = true;
            this.btnPlay.BackColor = Color.Transparent;
            this.btnPlay.Image = Properties.Resources.pause;
            this.btnPlay.Location = new Point(24, 3);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.RoundedCornersMask = 15;
            this.btnPlay.RoundedCornersRadius = 0;
            this.btnPlay.Size = new Size(28, 28);
            this.btnPlay.TabIndex = 10;
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnPlay.Click += new EventHandler(this.btnPlay_Click);
            this.picEvidence.Image = Properties.Resources.star;
            this.picEvidence.Location = new Point(462, 4);
            this.picEvidence.Name = "picEvidence";
            this.picEvidence.Size = new Size(20, 20);
            this.picEvidence.SizeMode = PictureBoxSizeMode.CenterImage;
            this.picEvidence.TabIndex = 41;
            this.picEvidence.TabStop = false;
            this.lblTime.BorderStyle = BorderStyle.FixedSingle;
            this.lblTime.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblTime.Location = new Point(24, 41);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new Size(130, 23);
            this.lblTime.TabIndex = 19;
            this.lblTime.Text = "00:00:00.000";
            this.lblTime.TextAlign = ContentAlignment.MiddleCenter;
            this.lblSet.AutoSize = true;
            this.lblSet.Location = new Point(462, 80);
            this.lblSet.Name = "lblSet";
            this.lblSet.Size = new Size(23, 13);
            this.lblSet.TabIndex = 40;
            this.lblSet.Text = "Set";
            this.lblFrame.BorderStyle = BorderStyle.FixedSingle;
            this.lblFrame.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblFrame.Location = new Point(24, 72);
            this.lblFrame.Name = "lblFrame";
            this.lblFrame.Size = new Size(130, 23);
            this.lblFrame.TabIndex = 20;
            this.lblFrame.Text = "0";
            this.lblFrame.TextAlign = ContentAlignment.MiddleCenter;
            this.lbl_Classification.AutoSize = true;
            this.lbl_Classification.Location = new Point(462, 63);
            this.lbl_Classification.Name = "lbl_Classification";
            this.lbl_Classification.Size = new Size(68, 13);
            this.lbl_Classification.TabIndex = 39;
            this.lbl_Classification.Text = "Classification";
            this.btnPrev.AllowAnimations = true;
            this.btnPrev.BackColor = Color.Transparent;
            this.btnPrev.Enabled = false;
            this.btnPrev.Image = Properties.Resources.file_prev;
            this.btnPrev.Location = new Point(92, 3);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.RoundedCornersMask = 15;
            this.btnPrev.RoundedCornersRadius = 0;
            this.btnPrev.Size = new Size(28, 28);
            this.btnPrev.TabIndex = 21;
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnPrev.Click += new EventHandler(this.btnPrev_Click);
            this.lbl_Filedate.AutoSize = true;
            this.lbl_Filedate.Location = new Point(462, 46);
            this.lbl_Filedate.Name = "lbl_Filedate";
            this.lbl_Filedate.Size = new Size(158, 13);
            this.lbl_Filedate.TabIndex = 37;
            this.lbl_Filedate.Text = "File Date: 00/00/0000 00:00:00";
            this.btnNext.AllowAnimations = true;
            this.btnNext.BackColor = Color.Transparent;
            this.btnNext.Enabled = false;
            this.btnNext.Image = Properties.Resources.filenext;
            this.btnNext.Location = new Point(126, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.RoundedCornersMask = 15;
            this.btnNext.RoundedCornersRadius = 0;
            this.btnNext.Size = new Size(28, 28);
            this.btnNext.TabIndex = 22;
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnNext.Click += new EventHandler(this.btnNext_Click);
            this.chk_TagsOnly.BackColor = Color.Transparent;
            this.chk_TagsOnly.Location = new Point(172, 41);
            this.chk_TagsOnly.Name = "chk_TagsOnly";
            this.chk_TagsOnly.Size = new Size(118, 24);
            this.chk_TagsOnly.TabIndex = 36;
            this.chk_TagsOnly.Text = "Play Video Marks";
            this.chk_TagsOnly.UseVisualStyleBackColor = false;
            this.chk_TagsOnly.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.chk_TagsOnly.CheckedChanged += new EventHandler(this.chk_TagsOnly_CheckedChanged);
            this.btnFrameMinus.AllowAnimations = true;
            this.btnFrameMinus.BackColor = Color.Transparent;
            this.btnFrameMinus.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnFrameMinus.Location = new Point(172, 3);
            this.btnFrameMinus.Name = "btnFrameMinus";
            this.btnFrameMinus.RoundedCornersMask = 15;
            this.btnFrameMinus.RoundedCornersRadius = 0;
            this.btnFrameMinus.Size = new Size(28, 28);
            this.btnFrameMinus.TabIndex = 23;
            this.btnFrameMinus.Text = "-";
            this.btnFrameMinus.UseVisualStyleBackColor = false;
            this.btnFrameMinus.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnFrameMinus.Click += new EventHandler(this.btnFrameMinus_Click);
            this.lbl_SecurityLevel.AutoSize = true;
            this.lbl_SecurityLevel.BorderStyle = BorderStyle.FixedSingle;
            this.lbl_SecurityLevel.Location = new Point(462, 27);
            this.lbl_SecurityLevel.Name = "lbl_SecurityLevel";
            this.lbl_SecurityLevel.Size = new Size(66, 15);
            this.lbl_SecurityLevel.TabIndex = 35;
            this.lbl_SecurityLevel.Text = "Unclassified";
            this.btnFramePlus.AllowAnimations = true;
            this.btnFramePlus.BackColor = Color.Transparent;
            this.btnFramePlus.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnFramePlus.Location = new Point(206, 3);
            this.btnFramePlus.Name = "btnFramePlus";
            this.btnFramePlus.RoundedCornersMask = 15;
            this.btnFramePlus.RoundedCornersRadius = 0;
            this.btnFramePlus.Size = new Size(28, 28);
            this.btnFramePlus.TabIndex = 25;
            this.btnFramePlus.Text = "+";
            this.btnFramePlus.UseVisualStyleBackColor = false;
            this.btnFramePlus.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnFramePlus.Click += new EventHandler(this.btnFramePlus_Click);
            this.lblSpeed.Location = new Point(255, 81);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new Size(49, 14);
            this.lblSpeed.TabIndex = 33;
            this.lblSpeed.Text = "1x";
            this.lblSpeed.TextAlign = ContentAlignment.MiddleRight;
            this.SpeedBar.AutoSize = false;
            this.SpeedBar.Location = new Point(310, 3);
            this.SpeedBar.Maximum = 8;
            this.SpeedBar.Minimum = 1;
            this.SpeedBar.Name = "SpeedBar";
            this.SpeedBar.Orientation = Orientation.Vertical;
            this.SpeedBar.Size = new Size(40, 98);
            this.SpeedBar.TabIndex = 29;
            this.SpeedBar.Value = 4;
            this.SpeedBar.Scroll += new EventHandler(this.SpeedBar_Scroll);
            this.btnSnapshot.AllowAnimations = true;
            this.btnSnapshot.BackColor = Color.Transparent;
            this.btnSnapshot.Image = Properties.Resources.snapshot;
            this.btnSnapshot.Location = new Point(240, 3);
            this.btnSnapshot.Name = "btnSnapshot";
            this.btnSnapshot.RoundedCornersMask = 15;
            this.btnSnapshot.RoundedCornersRadius = 0;
            this.btnSnapshot.Size = new Size(28, 28);
            this.btnSnapshot.TabIndex = 32;
            this.btnSnapshot.UseVisualStyleBackColor = false;
            this.btnSnapshot.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnSnapshot.Click += new EventHandler(this.btnSnapshot_Click);
            this.VolumeBar.AutoSize = false;
            this.VolumeBar.Location = new Point(357, 3);
            this.VolumeBar.Maximum = 100;
            this.VolumeBar.Name = "VolumeBar";
            this.VolumeBar.Orientation = Orientation.Vertical;
            this.VolumeBar.Size = new Size(40, 98);
            this.VolumeBar.TabIndex = 30;
            this.VolumeBar.TickFrequency = 10;
            this.VolumeBar.TickStyle = TickStyle.TopLeft;
            this.VolumeBar.Value = 50;
            this.VolumeBar.Scroll += new EventHandler(this.VolumeBar_Scroll);
            this.VideoPanel.BackColor = Color.Black;
            this.VideoPanel.BackgroundImage = Properties.Resources.video;
            this.VideoPanel.BackgroundImageLayout = ImageLayout.Center;
            this.VideoPanel.Controls.Add(this.vlc);
            this.VideoPanel.Dock = DockStyle.Top;
            this.VideoPanel.Location = new Point(0, 45);
            this.VideoPanel.Name = "VideoPanel";
            this.VideoPanel.Padding = new Padding(0, 1, 0, 0);
            this.VideoPanel.Size = new Size(647, 451);
            this.VideoPanel.TabIndex = 0;
            this.vlc.Dock = DockStyle.Fill;
            this.vlc.Enabled = true;
            this.vlc.Location = new Point(0, 1);
            this.vlc.Name = "vlc";
            this.vlc.OcxState = (AxHost.State)Resources.VideoForm.vlc_OcxState;
            this.vlc.Size = new Size(647, 450);
            this.vlc.TabIndex = 0;
            this.vlc.MediaPlayerEndReached += new EventHandler(this.vlc_MediaPlayerEndReached);
            this.TrackbarTable.ColumnCount = 5;
            this.TrackbarTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
            this.TrackbarTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            this.TrackbarTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            this.TrackbarTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            this.TrackbarTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 22f));
            this.TrackbarTable.Controls.Add(this.VideoBar, 1, 0);
            this.TrackbarTable.Controls.Add(this.lbl_File, 2, 1);
            this.TrackbarTable.Controls.Add(this.lbl_TagState, 1, 1);
            this.TrackbarTable.Controls.Add(this.lbl_VideoTime, 3, 1);
            this.TrackbarTable.Location = new Point(0, 499);
            this.TrackbarTable.Name = "TrackbarTable";
            this.TrackbarTable.RowCount = 2;
            this.TrackbarTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.TrackbarTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 14f));
            this.TrackbarTable.Size = new Size(647, 42);
            this.TrackbarTable.TabIndex = 1;
            this.VideoBar.BackColor = Color.Transparent;
            this.TrackbarTable.SetColumnSpan(this.VideoBar, 3);
            this.VideoBar.ContextMenuStrip = this.VideoMenu;
            this.VideoBar.Dock = DockStyle.Fill;
            this.VideoBar.Location = new Point(23, 3);
            this.VideoBar.Name = "VideoBar";
            this.VideoBar.RoundedCornersMask = 15;
            this.VideoBar.RoundedCornersMaskThumb = 15;
            this.VideoBar.RoundedCornersRadius = 0;
            this.VideoBar.RoundedCornersRadiusThumb = 0;
            this.VideoBar.Size = new Size(599, 22);
            this.VideoBar.TabIndex = 0;
            this.VideoBar.Value = 0;
            this.VideoBar.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.VideoBar.Scroll += new ScrollEventHandler(this.VideoBar_Scroll);
            ToolStripItemCollection items = this.VideoMenu.Items;
            ToolStripItem[] mnuTagStart = new ToolStripItem[] { this.mnu_TagStart, this.mnu_TagEnd, this.mnu_TagClear, this.toolStripMenuItem1, this.mnu_TagSave, this.mnu_TagLoop };
            this.VideoMenu.Items.AddRange(mnuTagStart);
            this.VideoMenu.Name = "VideoMenu";
            this.VideoMenu.Size = new Size(170, 120);
            this.mnu_TagStart.Name = "mnu_TagStart";
            this.mnu_TagStart.Size = new Size(169, 22);
            this.mnu_TagStart.Text = "Mark - Start";
            this.mnu_TagStart.Click += new EventHandler(this.mnu_TagStart_Click);
            this.mnu_TagEnd.Name = "mnu_TagEnd";
            this.mnu_TagEnd.Size = new Size(169, 22);
            this.mnu_TagEnd.Text = "Mark - End";
            this.mnu_TagEnd.Click += new EventHandler(this.mnu_TagEnd_Click);
            this.mnu_TagClear.Name = "mnu_TagClear";
            this.mnu_TagClear.Size = new Size(169, 22);
            this.mnu_TagClear.Text = "Mark - Clear";
            this.mnu_TagClear.Click += new EventHandler(this.mnu_TagClear_Click);
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new Size(166, 6);
            this.mnu_TagSave.Name = "mnu_TagSave";
            this.mnu_TagSave.Size = new Size(169, 22);
            this.mnu_TagSave.Text = "Save Video Mark";
            this.mnu_TagSave.Click += new EventHandler(this.mnu_TagSave_Click);
            this.mnu_TagLoop.Name = "mnu_TagLoop";
            this.mnu_TagLoop.Size = new Size(169, 22);
            this.mnu_TagLoop.Text = "Loop Video Marks";
            this.mnu_TagLoop.Click += new EventHandler(this.mnu_TagLoop_Click);
            this.lbl_File.AutoSize = true;
            this.lbl_File.Dock = DockStyle.Fill;
            this.lbl_File.Location = new Point(265, 28);
            this.lbl_File.Name = "lbl_File";
            this.lbl_File.Size = new Size(115, 14);
            this.lbl_File.TabIndex = 34;
            this.lbl_File.Text = "File 0 : 0";
            this.lbl_File.TextAlign = ContentAlignment.MiddleCenter;
            this.lbl_TagState.AutoSize = true;
            this.lbl_TagState.Dock = DockStyle.Fill;
            this.lbl_TagState.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lbl_TagState.ForeColor = Color.FromArgb(64, 64, 64);
            this.lbl_TagState.Location = new Point(23, 28);
            this.lbl_TagState.Name = "lbl_TagState";
            this.lbl_TagState.Size = new Size(236, 14);
            this.lbl_TagState.TabIndex = 1;
            this.lbl_TagState.Text = "MARK";
            this.lbl_VideoTime.AutoSize = true;
            this.lbl_VideoTime.Dock = DockStyle.Fill;
            this.lbl_VideoTime.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lbl_VideoTime.ForeColor = Color.FromArgb(64, 64, 64);
            this.lbl_VideoTime.Location = new Point(386, 28);
            this.lbl_VideoTime.Margin = new Padding(3, 0, 0, 0);
            this.lbl_VideoTime.Name = "lbl_VideoTime";
            this.lbl_VideoTime.Size = new Size(239, 14);
            this.lbl_VideoTime.TabIndex = 2;
            this.lbl_VideoTime.Text = "00:00:00.000";
            this.lbl_VideoTime.TextAlign = ContentAlignment.MiddleRight;
            this.ControlPanel.AutoScroll = true;
            this.ControlPanel.Controls.Add(this.OptionPanel);
            this.ControlPanel.Controls.Add(this.MenuPanel);
            this.ControlPanel.Dock = DockStyle.Right;
            this.ControlPanel.Location = new Point(647, 45);
            this.ControlPanel.Margin = new Padding(0);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new Size(351, 603);
            this.ControlPanel.TabIndex = 2;
            this.OptionPanel.Dock = DockStyle.Fill;
            this.OptionPanel.Location = new Point(0, 0);
            this.OptionPanel.Name = "OptionPanel";
            this.OptionPanel.Size = new Size(351, 553);
            this.OptionPanel.TabIndex = 1;
            this.MenuPanel.Controls.Add(this.btnImageFiles);
            this.MenuPanel.Controls.Add(this.btn_Map);
            this.MenuPanel.Controls.Add(this.btnFiles);
            this.MenuPanel.Controls.Add(this.btnTags);
            this.MenuPanel.Controls.Add(this.btnThumbnails);
            this.MenuPanel.Dock = DockStyle.Bottom;
            this.MenuPanel.Location = new Point(0, 553);
            this.MenuPanel.Name = "MenuPanel";
            this.MenuPanel.Size = new Size(351, 50);
            this.MenuPanel.TabIndex = 0;
            this.btnImageFiles.AllowAnimations = true;
            this.btnImageFiles.BackColor = Color.Transparent;
            this.btnImageFiles.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnImageFiles.Image = Properties.Resources.picture_2;
            this.btnImageFiles.Location = new Point(140, 11);
            this.btnImageFiles.Name = "btnImageFiles";
            this.btnImageFiles.RoundedCornersMask = 15;
            this.btnImageFiles.RoundedCornersRadius = 0;
            this.btnImageFiles.Size = new Size(28, 28);
            this.btnImageFiles.TabIndex = 30;
            this.btnImageFiles.UseVisualStyleBackColor = false;
            this.btnImageFiles.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnImageFiles.Click += new EventHandler(this.btnImageFiles_Click);
            this.btn_Map.AllowAnimations = true;
            this.btn_Map.BackColor = Color.Transparent;
            this.btn_Map.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btn_Map.Image = Properties.Resources.map;
            this.btn_Map.Location = new Point(4, 11);
            this.btn_Map.Name = "btn_Map";
            this.btn_Map.RoundedCornersMask = 15;
            this.btn_Map.RoundedCornersRadius = 0;
            this.btn_Map.Size = new Size(28, 28);
            this.btn_Map.TabIndex = 26;
            this.btn_Map.Text = "-";
            this.btn_Map.UseVisualStyleBackColor = false;
            this.btn_Map.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btn_Map.Click += new EventHandler(this.btn_Map_Click);
            this.btnFiles.AllowAnimations = true;
            this.btnFiles.BackColor = Color.Transparent;
            this.btnFiles.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnFiles.Image = Properties.Resources.folder2;
            this.btnFiles.Location = new Point(106, 11);
            this.btnFiles.Name = "btnFiles";
            this.btnFiles.RoundedCornersMask = 15;
            this.btnFiles.RoundedCornersRadius = 0;
            this.btnFiles.Size = new Size(28, 28);
            this.btnFiles.TabIndex = 29;
            this.btnFiles.UseVisualStyleBackColor = false;
            this.btnFiles.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnFiles.Click += new EventHandler(this.btnFiles_Click);
            this.btnTags.AllowAnimations = true;
            this.btnTags.BackColor = Color.Transparent;
            this.btnTags.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnTags.Image = Properties.Resources.tag;
            this.btnTags.Location = new Point(72, 11);
            this.btnTags.Name = "btnTags";
            this.btnTags.RoundedCornersMask = 15;
            this.btnTags.RoundedCornersRadius = 0;
            this.btnTags.Size = new Size(28, 28);
            this.btnTags.TabIndex = 28;
            this.btnTags.UseVisualStyleBackColor = false;
            this.btnTags.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnTags.Click += new EventHandler(this.btnTags_Click);
            this.btnThumbnails.AllowAnimations = true;
            this.btnThumbnails.BackColor = Color.Transparent;
            this.btnThumbnails.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnThumbnails.Image = Properties.Resources.thumbnail;
            this.btnThumbnails.Location = new Point(38, 11);
            this.btnThumbnails.Name = "btnThumbnails";
            this.btnThumbnails.RoundedCornersMask = 15;
            this.btnThumbnails.RoundedCornersRadius = 0;
            this.btnThumbnails.Size = new Size(28, 28);
            this.btnThumbnails.TabIndex = 27;
            this.btnThumbnails.UseVisualStyleBackColor = false;
            this.btnThumbnails.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnThumbnails.Click += new EventHandler(this.btnThumbnails_Click);
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.lbl_ImageFileCount);
            this.HeaderPanel.Controls.Add(this.lbl_VideoFileCount);
            this.HeaderPanel.Controls.Add(this.lblVideoTitle);
            this.HeaderPanel.Controls.Add(this.btnCtrlPanel);
            this.HeaderPanel.Controls.Add(this.LogoPic);
            this.HeaderPanel.Controls.Add(this.btnClose);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(998, 45);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.lbl_ImageFileCount.AutoSize = true;
            this.lbl_ImageFileCount.BackColor = Color.Transparent;
            this.lbl_ImageFileCount.ForeColor = Color.White;
            this.lbl_ImageFileCount.Location = new Point(285, 25);
            this.lbl_ImageFileCount.Name = "lbl_ImageFileCount";
            this.lbl_ImageFileCount.Size = new Size(72, 13);
            this.lbl_ImageFileCount.TabIndex = 6;
            this.lbl_ImageFileCount.Text = "Image Files: 0";
            this.lbl_VideoFileCount.AutoSize = true;
            this.lbl_VideoFileCount.BackColor = Color.Transparent;
            this.lbl_VideoFileCount.ForeColor = Color.White;
            this.lbl_VideoFileCount.Location = new Point(285, 8);
            this.lbl_VideoFileCount.Name = "lbl_VideoFileCount";
            this.lbl_VideoFileCount.Size = new Size(70, 13);
            this.lbl_VideoFileCount.TabIndex = 5;
            this.lbl_VideoFileCount.Text = "Video Files: 0";
            this.lblVideoTitle.AutoSize = true;
            this.lblVideoTitle.BackColor = Color.Transparent;
            this.lblVideoTitle.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblVideoTitle.ForeColor = Color.White;
            this.lblVideoTitle.Location = new Point(53, 4);
            this.lblVideoTitle.Name = "lblVideoTitle";
            this.lblVideoTitle.Size = new Size(141, 20);
            this.lblVideoTitle.TabIndex = 4;
            this.lblVideoTitle.Text = "VIDEO REVIEW";
            this.lblVideoTitle.TextAlign = ContentAlignment.MiddleLeft;
            this.lblVideoTitle.MouseDown += new MouseEventHandler(this.lblVideoTitle_MouseDown);
            this.btnCtrlPanel.AllowAnimations = true;
            this.btnCtrlPanel.BackColor = Color.Transparent;
            this.btnCtrlPanel.Image = Properties.Resources.showhide;
            this.btnCtrlPanel.Location = new Point(650, 5);
            this.btnCtrlPanel.Name = "btnCtrlPanel";
            this.btnCtrlPanel.PaintBorder = false;
            this.btnCtrlPanel.PaintDefaultBorder = false;
            this.btnCtrlPanel.PaintDefaultFill = false;
            this.btnCtrlPanel.RoundedCornersMask = 15;
            this.btnCtrlPanel.RoundedCornersRadius = 0;
            this.btnCtrlPanel.Size = new Size(36, 36);
            this.btnCtrlPanel.TabIndex = 2;
            this.btnCtrlPanel.UseVisualStyleBackColor = false;
            this.btnCtrlPanel.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnCtrlPanel.Click += new EventHandler(this.btnCtrlPanel_Click);
            this.LogoPic.BackColor = Color.Transparent;
            this.LogoPic.Image = Properties.Resources.camlens2;
            this.LogoPic.Location = new Point(8, 4);
            this.LogoPic.Name = "LogoPic";
            this.LogoPic.Size = new Size(36, 36);
            this.LogoPic.SizeMode = PictureBoxSizeMode.CenterImage;
            this.LogoPic.TabIndex = 1;
            this.LogoPic.TabStop = false;
            this.LogoPic.MouseDown += new MouseEventHandler(this.LogoPic_MouseDown);
            this.btnClose.AllowAnimations = true;
            this.btnClose.BackColor = Color.Transparent;
            this.btnClose.Dock = DockStyle.Right;
            this.btnClose.Image = Properties.Resources.close;
            this.btnClose.Location = new Point(952, 0);
            this.btnClose.Margin = new Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaintBorder = false;
            this.btnClose.PaintDefaultBorder = false;
            this.btnClose.PaintDefaultFill = false;
            this.btnClose.RoundedCornersMask = 15;
            this.btnClose.RoundedCornersRadius = 0;
            this.btnClose.Size = new Size(46, 45);
            this.btnClose.TabIndex = 0;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(1000, 650);
            base.Controls.Add(this.FormPanel);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Icon = (Icon)Resources.VideoForm.VideoFormIcon;
            base.Name = "VideoForm";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Video Review";
            base.FormClosing += new FormClosingEventHandler(this.VideoForm_FormClosing);
            base.Load += new EventHandler(this.VideoForm_Load);
            this.FormPanel.ResumeLayout(false);
            this.VCRPanel.ResumeLayout(false);
            this.VCRPanel.PerformLayout();
            ((ISupportInitialize)this.VolPic).EndInit();
            ((ISupportInitialize)this.picEvidence).EndInit();
            ((ISupportInitialize)this.SpeedBar).EndInit();
            ((ISupportInitialize)this.VolumeBar).EndInit();
            this.VideoPanel.ResumeLayout(false);
            ((ISupportInitialize)this.vlc).EndInit();
            this.TrackbarTable.ResumeLayout(false);
            this.TrackbarTable.PerformLayout();
            this.VideoMenu.ResumeLayout(false);
            this.ControlPanel.ResumeLayout(false);
            this.MenuPanel.ResumeLayout(false);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((ISupportInitialize)this.LogoPic).EndInit();
            base.ResumeLayout(false);
        }

        private void lbl_File_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void lblVideoTitle_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void LoadGPS()
        {
            try
            {
                Mapper.ClearPoints();
                Mapper.MapHide();
                if (!string.IsNullOrEmpty(this.Media[this.fileIndex].Ext2))
                {
                    int num = this.Media[this.fileIndex].FileName.IndexOf('.');
                    Mapper.LoadDataPoints(this.Media[this.fileIndex].FileName.Substring(0, num), this.Media[this.fileIndex].Ext2);
                    if (Mapper.GPSDataPoints() > 0)
                    {
                        Mapper.MapShow();
                    }
                }
            }
            catch
            {
            }
        }

        private void LoadOptionControl(Control ctrl)
        {
            if (!this.OptionPanel.Controls.Contains(ctrl))
            {
                try
                {
                    this.OptionPanel.Controls.Clear();
                    this.OptionPanel.Controls.Add(ctrl);
                }
                catch
                {
                }
            }
        }

        public void LoadVideoList(List<Guid> files, Guid AccountId)
        {
            this.vlc.Hide();
            this.FileID = files;
            this.AccountID = AccountId;
            this.timer1.Start();
        }

        private void LogoPic_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void mapPanel_EVT_Compass(bool b)
        {
            Mapper.ShowCompass(b);
        }

        private void mnu_TagClear_Click(object sender, EventArgs e)
        {
            this.ClearTAG();
        }

        private void mnu_TagEnd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.StartTag))
            {
                int value = this.VideoBar.Value;
                this.EndMS = value;
                this.EndFrame = value;
                this.EndTag = string.Format("{0} ({1})", this.GetFrame(), this.lblTime.Text);
                this.lbl_TagState.Text = string.Format(LangCtrl.GetString("vf_Mark2", "MARK {0} : {1}"), this.StartTag, this.EndTag);
            }
        }

        private void mnu_TagLoop_Click(object sender, EventArgs e)
        {
            this.IsLoop = !this.IsLoop;
            this.mnu_TagLoop.Checked = this.IsLoop;
        }

        private void mnu_TagSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveTag saveTag = new SaveTag();
                if (this.StartMS >= 0 && this.EndMS > 1 && saveTag.ShowDialog(this) == DialogResult.OK)
                {
                    this.vTag = new VideoTag()
                    {
                        ShortDesc = saveTag.Desc,
                        StartFrame = (long)this.StartFrame,
                        EndFrame = (long)this.EndFrame,
                        StartTime = new double?((double)this.StartMS / 1000),
                        EndTime = new double?((double)this.EndMS / 1000)
                    };
                    DataFile dataFile = new DataFile();
                    using (RPM_DataFile rPMDataFile = new RPM_DataFile())
                    {
                        dataFile = rPMDataFile.GetDataFile(this.Media[this.fileIndex].FileID);
                        dataFile.VideoTags.Add(this.vTag);
                        rPMDataFile.SaveUpdate(dataFile);
                        rPMDataFile.Save();
                    }
                    TimeSpan timeSpan = new TimeSpan(0, 0, Convert.ToInt32(this.vTag.StartTime));
                    TimeSpan timeSpan1 = new TimeSpan(0, 0, Convert.ToInt32(this.vTag.EndTime));
                    string str = string.Format("{0:00}:{1:00}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
                    string str1 = string.Format("{0:00}:{1:00}:{2:D2}", timeSpan1.Hours, timeSpan1.Minutes, timeSpan1.Seconds);
                    object[] storedFileName = new object[] { dataFile.StoredFileName, dataFile.FileExtension, str, str1, this.vTag.ShortDesc.ToUpper() };
                    Global.Log("MARK-SAVE", string.Format("File: {0}{1} > {2} to {3} as {4}", storedFileName));
                    this.tagPanel.ListTags(this.Media[this.fileIndex].FileID);
                }
            }
            catch
            {
            }
        }

        private void mnu_TagStart_Click(object sender, EventArgs e)
        {
            int value = this.VideoBar.Value;
            this.StartMS = value;
            this.StartFrame = value;
            this.StartTag = string.Format("{0} ({1})", this.GetFrame(), this.lblTime.Text);
            this.lbl_TagState.Text = string.Format(LangCtrl.GetString("vf_Mark1", "MARK {0} : ?"), this.StartTag);
        }

        private void MoveToVideo()
        {
            this.ClearTAG();
            Mapper.ClearPoints();
            this.vlc.Show();
            this.TagIndex = 0;
            this.LoadGPS();
            this.vlc.playlist.playItem(this.fileIndex);
            this.GetFileLength();
            this.lbl_File.Text = string.Format(LangCtrl.GetString("vf_File", "File {0} : {1}"), this.fileIndex + 1, this.Media.Count);
            this.btnPlay.Image = Properties.Resources.pause;
            this.UpdateVideoTime();
            this.filePanel.LoadData(this.Media[this.fileIndex].FileID);
        }

        private void mVid_EVT_StopPlayer()
        {
            this.StopPlayer();
        }

        private void PlayTags(int t)
        {
            base.BeginInvoke(new MethodInvoker(() => {
                if (this.FileLength <= 0 || this.TagIndex > this.tagPanel.TagList.Count - 1)
                {
                    this.EOF();
                    return;
                }
                if (t <= this.VideoBar.Maximum)
                {
                    this.VideoBar.Value = t;
                }
                int endFrame = (int)this.tagPanel.TagList[this.TagIndex].EndFrame;
                if (t > endFrame)
                {
                    this.TagIndex++;
                    if (this.TagIndex > this.tagPanel.TagList.Count - 1 || this.tagPanel.TagList.Count == 0)
                    {
                        this.TagIndex = 0;
                        this.EOF();
                        this.filePanel.LoadData(this.Media[this.fileIndex].FileID);
                    }
                    if (this.tagPanel.TagList.Count <= 0)
                    {
                        this.EOF();
                    }
                    else
                    {
                        this.vlc.input.Time = (double)this.tagPanel.TagList[this.TagIndex].StartFrame;
                    }
                }
                this.UpdateLabelsAndMap(t);
            }));
        }

        private void PlayVideo()
        {
            try
            {
                if (this.Media.Count > 0 && this.vlc != null)
                {
                    this.vlc.Show();
                    if (this.Media.Count > 1)
                    {
                        vButton _vButton = this.btnPrev;
                        this.btnNext.Enabled = true;
                        _vButton.Enabled = true;
                    }
                    if (this.vlc.playlist.isPlaying || this.IsPlaying)
                    {
                        this.btnPlay.Image = Properties.Resources.play;
                        if (!this.vlc.playlist.isPlaying)
                        {
                            this.btnPlay.Image = Properties.Resources.pause;
                        }
                        this.vlc.playlist.togglePause();
                    }
                    else
                    {
                        this.LoadGPS();
                        this.btnPlay.Image = Properties.Resources.pause;
                        try
                        {
                            this.vlc.playlist.playItem(this.fileIndex);
                        }
                        catch (Exception exception)
                        {
                            string message = exception.Message;
                        }
                        this.GetFileLength();
                        this.FPS = this.vlc.input.fps;
                        if (this.FPS == 0)
                        {
                            this.FPS = 30;
                        }
                        this.IsPlaying = true;
                    }
                    this.UpdateVideoTime();
                }
            }
            catch (Exception exception2)
            {
                Exception exception1 = exception2;
                MessageBox.Show(this, string.Format("Video Error: {0}", exception1.Message), "Video", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ProcessFiles()
        {
            if (this.vlc != null)
            {
                this.vlc.playlist.items.clear();
            }
            this.fileIndex = 0;
            this.Media = new List<MediaFile>();
            List<string> strs = new List<string>();
            using (RPM_DataFile rPMDataFile = new RPM_DataFile())
            {
                string str = ".MPG.MP4.MPEG.MP2.MV4.3GP.AVI.MOV.WMV.DVX.MKV.FLV.OGV.OGG.RM.ASF.WAV";
                string str1 = ".JPG.BMP.PNG.TIF";
                foreach (Guid fileID in this.FileID)
                {
                    DataFile dataFile = rPMDataFile.GetDataFile(fileID);
                    string str2 = Path.Combine(dataFile.UNCName, dataFile.UNCPath);
                    string str3 = string.Concat(Path.Combine(str2, dataFile.StoredFileName), dataFile.FileExtension);
                    if (!File.Exists(str3))
                    {
                        strs.Add(Path.GetFileName(str3));
                    }
                    else if (!str.Contains(dataFile.FileExtension.ToUpper()))
                    {
                        if (!str1.Contains(dataFile.FileExtension.ToUpper()))
                        {
                            continue;
                        }
                        ImageFile imageFile = new ImageFile()
                        {
                            FileName = str3,
                            Thumbnail = Utilities.resizeImage(160, 100, Utilities.ByteArrayToImage(dataFile.Thumbnail))
                        };
                        this.Images.Add(imageFile);
                    }
                    else
                    {
                        MediaFile mediaFile = new MediaFile()
                        {
                            Classification = dataFile.Classification,
                            FileDate = dataFile.FileTimestamp.Value,
                            FileName = str3,
                            IsEvidence = dataFile.IsEvidence,
                            Security = dataFile.Security,
                            Ext2 = dataFile.FileExtension2,
                            FileID = dataFile.Id,
                            Set = dataFile.SetName,
                            UNCName = dataFile.UNCName,
                            UNCPath = dataFile.UNCPath
                        };
                        this.Media.Add(mediaFile);
                        Global.Log("VIDEO", string.Format("File: {0}", mediaFile.FileName));
                        if (this.vlc == null)
                        {
                            continue;
                        }
                        this.vlc.playlist.@add(string.Concat("file:///", str3), null, null);
                    }
                }
            }
            this.lbl_VideoFileCount.Text = string.Format(LangCtrl.GetString("vf_VideoFiles", "Video Files: {0}"), this.Media.Count);
            this.lbl_ImageFileCount.Text = string.Format(LangCtrl.GetString("vf_ImageFiles", "Image Files: {0}"), this.Images.Count);
            Mapper.ClearPoints();
            this.lbl_File.Text = string.Format(LangCtrl.GetString("vf_File", "File {0} : {1}"), 1, this.Media.Count);
            if (this.Images.Count > 0)
            {
                this.imgPanel.LoadImages(this.Images);
                this.LoadOptionControl(this.imgPanel);
            }
            if (this.Media.Count > 0)
            {
                this.vlc.Show();
                this.PlayVideo();
            }
            if (this.Media.Count <= 0)
            {
                try
                {
                    this.vlc.Hide();
                    if (this.Images.Count == 0)
                    {
                        string empty = string.Empty;
                        int num = 1;
                        foreach (string str4 in strs)
                        {
                            int num1 = num;
                            num = num1 + 1;
                            empty = string.Concat(empty, string.Format("{0:00} • {1}\n", num1, str4));
                        }
                        MessageBox.Show(this, string.Format(LangCtrl.GetString("vf_MissingFiles", "Missing Files:\n{0}\n\nPlease contact your system administrator."), empty), "Video", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
                catch
                {
                }
            }
            else
            {
                try
                {
                    this.filePanel.LoadData(this.Media[this.fileIndex].FileID);
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    MessageBox.Show(this, string.Format(LangCtrl.GetString("vf_VidError", "Video File Error: {0}"), exception.Message), "Video", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void SetToolTips()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(this.picEvidence, LangCtrl.GetString("tt_picEvidence", "Evidence"));
            toolTip.SetToolTip(this.btn_Map, LangCtrl.GetString("tt_Map", "Display Map Panel"));
            toolTip.SetToolTip(this.btnFrameMinus, LangCtrl.GetString("tt_MinusFrame", "Minus 1 frame"));
            toolTip.SetToolTip(this.btnFramePlus, LangCtrl.GetString("tt_PlusFrame", "Plus 1 frame"));
            toolTip.SetToolTip(this.btnNext, LangCtrl.GetString("tt_NextVid", "Next video"));
            toolTip.SetToolTip(this.btnPlay, LangCtrl.GetString("tt_Play", "Play / Pause"));
            toolTip.SetToolTip(this.btnPrev, LangCtrl.GetString("tt_Prev", "Previous video"));
            toolTip.SetToolTip(this.btnSnapshot, LangCtrl.GetString("tt_VidSnap", "Video Snapshot"));
            toolTip.SetToolTip(this.btnStop, LangCtrl.GetString("tt_StopVid", "Stop video"));
            toolTip.SetToolTip(this.btnTags, LangCtrl.GetString("tt_VidMarks", "Video Marks"));
            toolTip.SetToolTip(this.btnThumbnails, LangCtrl.GetString("tt_VideoSnapshots", "Video Snapshots"));
            toolTip.SetToolTip(this.SpeedBar, LangCtrl.GetString("tt_PlaySpeed", "Playback speed"));
            toolTip.SetToolTip(this.VolumeBar, LangCtrl.GetString("tt_Volume", "Volume"));
            toolTip.SetToolTip(this.btnImageFiles, LangCtrl.GetString("tt_ImageFiles", "Image Files"));
        }

        private void SpeedBar_Scroll(object sender, EventArgs e)
        {
            double num = (new double[] { 0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2 })[this.SpeedBar.Value - 1];
            this.lblSpeed.Text = string.Format("{0}x", num);
            this.vlc.input.rate = num;
        }

        private void StopPlayer()
        {
            this.VideoBar.Value = 0;
            this.TagIndex = 0;
            this.vlc.playlist.stop();
            this.IsPlaying = false;
            this.btnPlay.Image = Properties.Resources.play;
            this.vlc.Hide();
        }

        private void tagPanel_EVT_MergeVideo(string folder)
        {
            if (this.Media.Count > 0)
            {
                MergeVideo mergeVideo = new MergeVideo();
                mergeVideo.EVT_StopPlayer -= new MergeVideo.DEL_StopPlayer(this.mVid_EVT_StopPlayer);
                mergeVideo.EVT_StopPlayer += new MergeVideo.DEL_StopPlayer(this.mVid_EVT_StopPlayer);
                mergeVideo.VideoPath = folder;
                mergeVideo.media = this.Media;
                mergeVideo.ShowDialog(this);
                mergeVideo.EVT_StopPlayer -= new MergeVideo.DEL_StopPlayer(this.mVid_EVT_StopPlayer);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Stop();
            this.timer1.Enabled = false;
            if (this.vlc != null)
            {
                this.vlc.MediaPlayerTimeChanged -= new AxAXVLC.DVLCEvents_MediaPlayerTimeChangedEventHandler(this.vlc_MediaPlayerTimeChanged);
                this.vlc.MediaPlayerTimeChanged += new AxAXVLC.DVLCEvents_MediaPlayerTimeChangedEventHandler(this.vlc_MediaPlayerTimeChanged);
                this.ProcessFiles();
            }
        }

        private void Update(int t)
        {
            try
            {
                base.BeginInvoke(new MethodInvoker(() => {
                    if (this.FileLength > 0)
                    {
                        if (t <= this.VideoBar.Maximum)
                        {
                            this.VideoBar.Value = t;
                        }
                        if (this.IsLoop && this.EndMS > 0 && t > this.EndMS)
                        {
                            t = this.StartMS;
                            this.vlc.input.Time = (double)t;
                        }
                        this.UpdateLabelsAndMap(t);
                    }
                }));
            }
            catch
            {
            }
        }

        private void UpdateLabelsAndMap(int t)
        {
            TimeSpan timeSpan = TimeSpan.FromMilliseconds((double)t);
            Label label = this.lblTime;
            object[] hours = new object[] { timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds };
            label.Text = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", hours);
            this.lblFrame.Text = string.Format("{0}", this.GetFrame());
            Mapper.UpdatePosition(t / 1000);
        }

        private void UpdateVideoTime()
        {
            TimeSpan timeSpan = TimeSpan.FromMilliseconds((double)this.VideoBar.Maximum);
            Label lblVideoTime = this.lbl_VideoTime;
            object[] hours = new object[] { timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds };
            lblVideoTime.Text = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", hours);
            this.lbl_SecurityLevel.BackColor = Color.White;
            this.lbl_SecurityLevel.ForeColor = Color.Black;
            switch (this.Media[this.fileIndex].Security)
            {
                case SECURITY.TOPSECRET:
                    {
                        this.lbl_SecurityLevel.ForeColor = Color.White;
                        this.lbl_SecurityLevel.BackColor = Color.Red;
                        break;
                    }
                case SECURITY.SECRET:
                    {
                        this.lbl_SecurityLevel.BackColor = Color.Orange;
                        this.lbl_SecurityLevel.ForeColor = Color.Black;
                        break;
                    }
                case SECURITY.OFFICIAL:
                    {
                        this.lbl_SecurityLevel.BackColor = Color.Yellow;
                        this.lbl_SecurityLevel.ForeColor = Color.Black;
                        break;
                    }
            }
            this.picEvidence.Visible = false;
            if (this.Media[this.fileIndex].IsEvidence)
            {
                this.picEvidence.Visible = true;
            }
            this.lbl_SecurityLevel.Text = AccountSecurity.GetSecurityDesc(this.Media[this.fileIndex].Security);
            this.lbl_Classification.Text = this.Media[this.fileIndex].Classification;
            Label lblFiledate = this.lbl_Filedate;
            DateTime fileDate = this.Media[this.fileIndex].FileDate;
            lblFiledate.Text = fileDate.ToString();
            this.lblSet.Text = this.Media[this.fileIndex].Set;
            this.thumbPanel.SetFileID(this.Media[this.fileIndex].FileID);
            this.tagPanel.ListTags(this.Media[this.fileIndex].FileID);
        }

        private void VideoBar_Scroll(object sender, ScrollEventArgs e)
        {
            this.CheckScroll();
        }

        private void VideoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void VideoForm_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
            {
                this.HeaderPanel.BackgroundImage = Properties.Resources.topbar45;
                this.VideoPanel.BackColor = Color.FromArgb(64, 64, 64);
                this.btnClose.VIBlendTheme = VIBLEND_THEME.NERO;
            }
            LangCtrl.reText(this);
            LangCtrl.reText(this.VideoMenu);
            Global.Log("OPEN", "Media Player");
            this.VolumeBar.Value = 100;
            this.InitControls();
            this.SetToolTips();
        }

        private void vlc_MediaPlayerEndReached(object sender, EventArgs e)
        {
            this.EOF();
        }

        private void vlc_MediaPlayerTimeChanged(object sender, DVLCEvents_MediaPlayerTimeChangedEvent e)
        {
            if (this.IsPlayTags)
            {
                this.PlayTags(e.time);
                return;
            }
            this.Update(e.time);
        }

        private void VolPic_MouseClick(object sender, MouseEventArgs e)
        {
            AxVLCPlugin2 axVLCPlugin2 = this.vlc;
            this.VolumeBar.Value = 0;
            axVLCPlugin2.Volume = 0;
            this.VolPic.Image = Properties.Resources.volume_mute;
        }

        private void VolumeBar_Scroll(object sender, EventArgs e)
        {
            this.vlc.Volume = this.VolumeBar.Value;
            this.VolPic.Image = Properties.Resources.volume;
            if (this.VolumeBar.Value == 0)
            {
                this.VolPic.Image = Properties.Resources.volume_mute;
            }
        }

        private void WriteSnapshot()
        {
            try
            {
                string str = string.Format("{0}.jpg", Guid.NewGuid());
                DateTime now = DateTime.Now;
                object[] id = new object[] { Global.GlobalAccount.Id, now.Year, now.Month, now.Day };
                string uNCPath = string.Format("{0}\\{1}\\{2:00}\\{3:00}", id);
                uNCPath = this.Media[this.fileIndex].UNCPath;
                string str1 = Path.Combine(Global.UNCServer, Global.RelativePath);
                str1 = Path.Combine(str1, uNCPath);
                if (!str1.Contains(":"))
                {
                    if (!str1.StartsWith("\\\\"))
                    {
                        str1 = string.Concat("\\\\", str1);
                    }
                }
                else if (str1.Contains(":") && !str1.Contains(":\\"))
                {
                    str1 = str1.Replace(":", ":\\");
                }
                if (!Directory.Exists(str1))
                {
                    Directory.CreateDirectory(str1);
                }
                Network.SetAcl(str1);
                Image image = null;
                string str2 = Path.Combine(str1, str);
                Network.SetAcl(str1);
                try
                {
                    float value = (float)(this.VideoBar.Value / 1000);
                    Global.Log("SNAPSHOT", string.Format("Video: {0} - Frame: {1}", this.Media[this.fileIndex].FileName, value));
                    FFMpegConverter fFMpegConverter = new FFMpegConverter();
                    fFMpegConverter.GetVideoThumbnail(this.Media[this.fileIndex].FileName, str2, new float?(value));
                    Thread.Sleep(500);
                    if (File.Exists(str2))
                    {
                        using (FileStream fileStream = new FileStream(str2, FileMode.Open, FileAccess.Read))
                        {
                            image = Image.FromStream(fileStream);
                        }
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    base.BeginInvoke(new MethodInvoker(() => MessageBox.Show(this, string.Format("Snapshot Error: {0}", exception.Message), "Media", MessageBoxButtons.OK, MessageBoxIcon.Hand)));
                }
                if (image != null)
                {
                    Snapshot snapshot = new Snapshot();
                    using (RPM_Snapshot rPMSnapshot = new RPM_Snapshot())
                    {
                        snapshot.DataFileId = this.Media[this.fileIndex].FileID;
                        snapshot.FileAddedTimestamp = new DateTime?(DateTime.Now);
                        snapshot.FileExtension = ".jpg";
                        image = Utilities.resizeImage(160, 100, Image.FromFile(str2));
                        snapshot.Thumbnail = Utilities.ImageToByte(image);
                        snapshot.StoredFileName = str;
                        snapshot.FrameNumber = Convert.ToInt32(this.lblFrame.Text);
                        snapshot.UNCName = Path.Combine(Global.UNCServer, Global.RelativePath);
                        snapshot.UNCPath = uNCPath;
                        snapshot.FileHash = string.Empty;
                        rPMSnapshot.SaveUpdate(snapshot);
                        rPMSnapshot.Save();
                    }
                    base.BeginInvoke(new MethodInvoker(() => {
                        this.btnSnapshot.Enabled = true;
                        if (this.thumbPanel != null)
                        {
                            this.thumbPanel.AddImage(snapshot, image);
                        }
                    }));
                }
            }
            catch (Exception exception2)
            {
                string message = exception2.Message;
            }
        }
    }
}