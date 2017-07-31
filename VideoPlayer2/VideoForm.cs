using AppGlobal;
using AxAXVLC;
using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMInterfaces;
using VMModels.Enums;
using VMModels.Model;

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
        private List<Guid> FileID;
        private List<MediaFile> Media;
        private List<ImageFile> Images;
        private Guid AccountID;
        private VideoTag vTag;
        private bool IsCmdPanel;
        private bool IsPlaying;
        private int TagIndex;
        private string StartTag;
        private string EndTag;
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
                createParams.ClassStyle |= 131072;
                return createParams;
            }
        }

        
        public VideoForm()
        {
            FileID = new List<Guid>();
            Media = new List<MediaFile>();
            Images = new List<ImageFile>();
            AccountID = Guid.Empty;
            vTag = new VideoTag();
            IsCmdPanel = true;
            StartTag = string.Empty;
            EndTag = string.Empty;
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        
        private void VideoForm_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
            {
                HeaderPanel.BackgroundImage = Properties.Resources.topbar45;
                VideoPanel.BackColor = Color.FromArgb(64, 64, 64);
                btnClose.VIBlendTheme = VIBLEND_THEME.NERO;
            }
            LangCtrl.reText(this);
            LangCtrl.reText(VideoMenu);
            Global.Log("VideoForm_1", "VideoForm_2");
            VolumeBar.Value = 100;
            InitControls();
            SetToolTips();
        }

        
        private void SetToolTips()
        {
            // TODO : Придется доделовать 
            ToolTip toolTip = new ToolTip();
            //toolTip.SetToolTip((Control)this.picEvidence, LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(6722), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(6754)));
            //toolTip.SetToolTip((Control)this.btn_Map, LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(6774), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(6790)));
            //toolTip.SetToolTip((Control)this.btnFrameMinus, LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(6828), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(6858)));
            //toolTip.SetToolTip((Control)this.btnFramePlus, LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(6888), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(6916)));
            //toolTip.SetToolTip((Control)this.btnNext, LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(6944), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(6968)));
            //toolTip.SetToolTip((Control)this.btnPlay, LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(6992), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(7010)));
            //toolTip.SetToolTip((Control)this.btnPrev, LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(7038), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(7056)));
            //toolTip.SetToolTip((Control)this.btnSnapshot, LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(7088), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(7112)));
            //toolTip.SetToolTip((Control)this.btnStop, LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(7144), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(7168)));
            //toolTip.SetToolTip((Control)this.btnTags, LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(7192), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(7218)));
            //toolTip.SetToolTip((Control)this.btnThumbnails, LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(7244), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(7282)));
            //toolTip.SetToolTip((Control)this.SpeedBar, LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(7316), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(7344)));
            //toolTip.SetToolTip((Control)this.VolumeBar, LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(7376), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(7398)));
            //toolTip.SetToolTip((Control)this.btnImageFiles, LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(7414), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(7444)));
        }

        
        private void btnClose_Click(object sender, EventArgs e)
        {
            tagPanel.EVT_MergeVideo -= new TagPanel.DEL_MergeVideo(tagPanel_EVT_MergeVideo);
            mapPanel.EVT_Compass -= new MapPanel2.DEL_Compass(mapPanel_EVT_Compass);
            if (vlc != null)
            {
                try
                {
                    vlc.MediaPlayerTimeChanged -= new DVLCEvents_MediaPlayerTimeChangedEventHandler(vlc_MediaPlayerTimeChanged);
                    vlc.playlist.stop();
                }
                catch
                {
                }
            }
            Global.Log("VideoForm_3", "VideoForm_4");
            Application.DoEvents();
            Close();
        }

        
        private void VideoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        
        private void HeaderMouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            ReleaseCapture();
            SendMessage(Handle, 161, 2, 0);
        }

        
        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            HeaderMouseDown(e);
        }

        
        private void LogoPic_MouseDown(object sender, MouseEventArgs e)
        {
            HeaderMouseDown(e);
        }

        
        private void lbl_File_MouseDown(object sender, MouseEventArgs e)
        {
            HeaderMouseDown(e);
        }

        
        private void lblVideoTitle_MouseDown(object sender, MouseEventArgs e)
        {
            HeaderMouseDown(e);
        }

        
        public void LoadVideoList(List<Guid> files, Guid AccountId)
        {
            vlc.Hide();
            FileID = files;
            AccountID = AccountId;
            timer1.Start();
        }

        
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Enabled = false;
            if (vlc == null)
                return;
            vlc.MediaPlayerTimeChanged -= new DVLCEvents_MediaPlayerTimeChangedEventHandler(vlc_MediaPlayerTimeChanged);
            vlc.MediaPlayerTimeChanged += new DVLCEvents_MediaPlayerTimeChangedEventHandler(vlc_MediaPlayerTimeChanged);
            ProcessFiles();
        }

        
        private void ProcessFiles()
        {
            if (vlc != null)
                vlc.playlist.items.clear();
            fileIndex = 0;
            Media = new List<MediaFile>();
            List<string> stringList = new List<string>();
            using (RPM_DataFile rpmDataFile = new RPM_DataFile())
            {
                string str1 = "VideoForm_5";
                string str2 = "VideoForm_6";
                foreach (Guid Id in FileID)
                {
                    DataFile dataFile = rpmDataFile.GetDataFile(Id);
                    string path = Path.Combine(Path.Combine(dataFile.UNCName, dataFile.UNCPath), dataFile.StoredFileName) + dataFile.FileExtension;
                    if (File.Exists(path))
                    {
                        if (str1.Contains(dataFile.FileExtension.ToUpper()))
                        {
                            MediaFile mediaFile = new MediaFile();
                            mediaFile.Classification = dataFile.Classification;
                            mediaFile.FileDate = dataFile.FileTimestamp.Value;
                            mediaFile.FileName = path;
                            mediaFile.IsEvidence = dataFile.IsEvidence;
                            mediaFile.Security = dataFile.Security;
                            mediaFile.Ext2 = dataFile.FileExtension2;
                            mediaFile.FileID = dataFile.Id;
                            mediaFile.Set = dataFile.SetName;
                            mediaFile.UNCName = dataFile.UNCName;
                            mediaFile.UNCPath = dataFile.UNCPath;
                            this.Media.Add(mediaFile);
                            Global.Log("VideoForm_7", string.Format("VideoForm_8", mediaFile.FileName));
                            if (vlc != null)
                                vlc.playlist.add("VideoForm_9" + path, null, null);
                        }
                        else if (str2.Contains(dataFile.FileExtension.ToUpper()))
                            Images.Add(new ImageFile()
                            {
                                FileName = path,
                                Thumbnail = Utilities.resizeImage(160, 100, Utilities.ByteArrayToImage(dataFile.Thumbnail))
                            });
                    }
                    else
                        stringList.Add(Path.GetFileName(path));
                }
            }
            lbl_VideoFileCount.Text = string.Format(LangCtrl.GetString("VideoForm_10", "VideoForm_11"), Media.Count);
            lbl_ImageFileCount.Text = string.Format(LangCtrl.GetString("VideoForm_12", "VideoForm_13"), Images.Count);
            Mapper.ClearPoints();
            lbl_File.Text = string.Format(LangCtrl.GetString("VideoForm_14", "VideoForm_15"), 1, Media.Count);
            if (Images.Count > 0)
            {
                imgPanel.LoadImages(Images);
                LoadOptionControl(imgPanel);
            }
            if (Media.Count > 0)
            {
                vlc.Show();
                PlayVideo();
            }
            if (Media.Count > 0)
            {
                try
                {
                    filePanel.LoadData(Media[fileIndex].FileID);
                }
                catch (Exception ex)
                {
                    int num = (int)MessageBox.Show(this, string.Format(LangCtrl.GetString("VideoForm_16", "VideoForm_17"), ex.Message), "VideoForm_18", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            else
            {
                try
                {
                    vlc.Hide();
                    if (Images.Count != 0)
                        return;
                    string empty = string.Empty;
                    int num1 = 1;
                    foreach (string str in stringList)
                        empty += string.Format("VideoForm_19", num1++, str);
                    int num2 = (int)MessageBox.Show(this, string.Format(LangCtrl.GetString("VideoForm_20", "VideoForm_21"), empty), "VideoForm_22", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                catch
                {
                }
            }
        }

        
        private void btnCtrlPanel_Click(object sender, EventArgs e)
        {
            IsCmdPanel = !IsCmdPanel;
            ControlPanel.Visible = IsCmdPanel;
        }

        
        private void InitControls()
        {
            mapPanel = new MapPanel2();
            mapPanel.EVT_Compass -= new MapPanel2.DEL_Compass(mapPanel_EVT_Compass);
            mapPanel.EVT_Compass += new MapPanel2.DEL_Compass(mapPanel_EVT_Compass);
            Mapper.SetMapper(mapPanel.GetMapObject());
            LoadOptionControl(mapPanel);
            thumbPanel = new ThumbPanel();
            imgPanel = new ImageFilePanel();
            tagPanel = new TagPanel();
            tagPanel.EVT_MergeVideo -= new TagPanel.DEL_MergeVideo(tagPanel_EVT_MergeVideo);
            tagPanel.EVT_MergeVideo += new TagPanel.DEL_MergeVideo(tagPanel_EVT_MergeVideo);
            filePanel = new FilePanel();
        }

        
        private void btn_Map_Click(object sender, EventArgs e)
        {
            LoadOptionControl(mapPanel);
        }

        
        private void btnThumbnails_Click(object sender, EventArgs e)
        {
            LoadOptionControl(thumbPanel);
        }

        
        private void btnFiles_Click(object sender, EventArgs e)
        {
            LoadOptionControl(filePanel);
        }

        
        private void btnTags_Click(object sender, EventArgs e)
        {
            LoadOptionControl(tagPanel);
        }

        
        private void btnImageFiles_Click(object sender, EventArgs e)
        {
            LoadOptionControl(imgPanel);
        }

        
        private void mapPanel_EVT_Compass(bool b)
        {
            Mapper.ShowCompass(b);
        }

        
        private void LoadOptionControl(Control ctrl)
        {
            if (OptionPanel.Controls.Contains(ctrl))
                return;
            try
            {
                OptionPanel.Controls.Clear();
                OptionPanel.Controls.Add(ctrl);
            }
            catch
            {
            }
        }

        
        private void btnPlay_Click(object sender, EventArgs e)
        {
            PlayVideo();
        }

        
        private void PlayVideo()
        {
            try
            {
                if (Media.Count <= 0 || vlc == null)
                    return;
                vlc.Show();
                if (Media.Count > 1)
                    btnPrev.Enabled = btnNext.Enabled = true;
                if (!vlc.playlist.isPlaying && !IsPlaying)
                {
                    LoadGPS();
                    btnPlay.Image = Properties.Resources.pause;
                    try
                    {
                        vlc.playlist.playItem(fileIndex);
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                    }
                    GetFileLength();
                    FPS = vlc.input.fps;
                    if (FPS == 0.0)
                        FPS = 30.0;
                    IsPlaying = true;
                }
                else
                {
                    btnPlay.Image = Properties.Resources.play;
                    if (!vlc.playlist.isPlaying)
                        btnPlay.Image = Properties.Resources.pause;
                    vlc.playlist.togglePause();
                }
                UpdateVideoTime();
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(this, string.Format("VideoForm_23", ex.Message), "VideoForm_24", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        
        private void GetFileLength()
        {
            FileLength = (int)vlc.input.Length;
            while (FileLength == 0)
                FileLength = (int)vlc.input.Length;
            VideoBar.Maximum = FileLength;
        }

        
        private void btnStop_Click(object sender, EventArgs e)
        {
            StopPlayer();
        }

        
        private void StopPlayer()
        {
            VideoBar.Value = 0;
            TagIndex = 0;
            vlc.playlist.stop();
            IsPlaying = false;
            btnPlay.Image = Properties.Resources.play;
            vlc.Hide();
        }

        
        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (--fileIndex < 0)
                fileIndex = Media.Count - 1;
            MoveToVideo();
        }

        
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (++fileIndex > Media.Count - 1)
                fileIndex = 0;
            MoveToVideo();
        }

        
        private void MoveToVideo()
        {
            ClearTAG();
            Mapper.ClearPoints();
            vlc.Show();
            TagIndex = 0;
            LoadGPS();
            vlc.playlist.playItem(fileIndex);
            GetFileLength();
            lbl_File.Text = string.Format(LangCtrl.GetString("VideoForm_25", "VideoForm_26"), fileIndex + 1, Media.Count);
            btnPlay.Image = Properties.Resources.pause;
            UpdateVideoTime();
            filePanel.LoadData(Media[fileIndex].FileID);
        }

        
        private void vlc_MediaPlayerTimeChanged(object sender, DVLCEvents_MediaPlayerTimeChangedEvent e)
        {
            if (IsPlayTags)
                PlayTags(e.time);
            else
                Update(e.time);
        }

        
        private void PlayTags(int t)
        {
            //TODO : Надо изменить delegate
            Action p1;
            BeginInvoke((p1 = () =>
            {
                if (FileLength > 0 && TagIndex <= tagPanel.TagList.Count - 1)
                {
                    if (t <= VideoBar.Maximum)
                        VideoBar.Value = t;
                    if (t > (int)tagPanel.TagList[TagIndex].EndFrame)
                    {
                        ++TagIndex;
                        if (TagIndex > tagPanel.TagList.Count - 1 || tagPanel.TagList.Count == 0)
                        {
                            TagIndex = 0;
                            EOF();
                            filePanel.LoadData(Media[fileIndex].FileID);
                        }
                        if (tagPanel.TagList.Count > 0)
                            vlc.input.Time = tagPanel.TagList[TagIndex].StartFrame;
                        else
                            EOF();
                    }
                    UpdateLabelsAndMap(t);
                }
                else
                    EOF();
            }));
        }

        
        private void Update(int t)
        {
            try
            {
                // TODO : Delegate
                Action p2;
                BeginInvoke(( p2 = () =>
                {
                    if (FileLength <= 0)
                        return;
                    if (t <= VideoBar.Maximum)
                        VideoBar.Value = t;
                    if (IsLoop && EndMS > 0 && t > EndMS)
                    {
                        t = StartMS;
                        vlc.input.Time = t;
                    }
                    UpdateLabelsAndMap(t);
                }));
            }
            catch
            {
            }
        }

        
        private void UpdateLabelsAndMap(int t)
        {
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(t);
            lblTime.Text = string.Format("VideoForm_27", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
            lblFrame.Text = string.Format("VideoForm_28", GetFrame());
            Mapper.UpdatePosition(t / 1000);
        }

        
        private void UpdateVideoTime()
        {
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(VideoBar.Maximum);
            lbl_VideoTime.Text = string.Format("VideoForm_29", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
            lbl_SecurityLevel.BackColor = Color.White;
            lbl_SecurityLevel.ForeColor = Color.Black;
            switch (Media[fileIndex].Security)
            {
                case SECURITY.TOPSECRET:
                    lbl_SecurityLevel.ForeColor = Color.White;
                    lbl_SecurityLevel.BackColor = Color.Red;
                    break;
                case SECURITY.SECRET:
                    lbl_SecurityLevel.BackColor = Color.Orange;
                    lbl_SecurityLevel.ForeColor = Color.Black;
                    break;
                case SECURITY.OFFICIAL:
                    lbl_SecurityLevel.BackColor = Color.Yellow;
                    lbl_SecurityLevel.ForeColor = Color.Black;
                    break;
            }
            picEvidence.Visible = false;
            if (Media[fileIndex].IsEvidence)
                picEvidence.Visible = true;
            lbl_SecurityLevel.Text = AccountSecurity.GetSecurityDesc(Media[fileIndex].Security);
            lbl_Classification.Text = Media[fileIndex].Classification;
            lbl_Filedate.Text = Media[fileIndex].FileDate.ToString();
            lblSet.Text = Media[fileIndex].Set;
            thumbPanel.SetFileID(Media[fileIndex].FileID);
            tagPanel.ListTags(Media[fileIndex].FileID);
        }

        
        private void VolumeBar_Scroll(object sender, EventArgs e)
        {
            vlc.Volume = VolumeBar.Value;
            VolPic.Image = Properties.Resources.volume;
            if (VolumeBar.Value != 0)
                return;
            VolPic.Image = Properties.Resources.volume_mute;
        }

        
        private void VolPic_MouseClick(object sender, MouseEventArgs e)
        {
            vlc.Volume = VolumeBar.Value = 0;
            VolPic.Image = Properties.Resources.volume_mute;
        }

        
        private void VideoBar_Scroll(object sender, ScrollEventArgs e)
        {
            CheckScroll();
        }

        
        private void CheckScroll()
        {
            int startMs = VideoBar.Value;
            if (IsLoop && EndMS > 0 && startMs > EndMS)
                startMs = StartMS;
            vlc.input.Time = (double)startMs;
            if (vlc.playlist.isPlaying)
                return;
            Update((int)((double)startMs + FPS));
        }

        
        private void vlc_MediaPlayerEndReached(object sender, EventArgs e)
        {
            EOF();
        }

        
        private void EOF()
        {
            if (++fileIndex > Media.Count - 1)
                fileIndex = 0;
            lbl_File.Text = string.Format(LangCtrl.GetString("VideoForm_30", "VideoForm_31"), fileIndex + 1, Media.Count);
            LoadGPS();
            vlc.playlist.playItem(fileIndex);
            FPS = vlc.input.fps;
            if (FPS == 0.0)
                FPS = 30.0;
            FileLength = 0;
            while (FileLength == 0)
                FileLength = (int)vlc.input.Length;
            TagIndex = 0;
            VideoBar.Maximum = FileLength;
            UpdateVideoTime();
            filePanel.LoadData(Media[fileIndex].FileID);
        }

        
        private void SpeedBar_Scroll(object sender, EventArgs e)
        {
            double num = new double[8]
            {
        0.25,
        0.5,
        0.75,
        1.0,
        1.25,
        1.5,
        1.75,
        2.0
            }[SpeedBar.Value - 1];
            lblSpeed.Text = string.Format("VideoForm_22", num);
            vlc.input.rate = num;
        }

        
        private void btnFrameMinus_Click(object sender, EventArgs e)
        {
            double num = (double)VideoBar.Value - FPS;
            if (num < 0.0)
                return;
            vlc.input.Time = num;
            VideoBar.Value = (int)num;
            Update((int)num);
        }

        
        private void btnFramePlus_Click(object sender, EventArgs e)
        {
            double num = (double)VideoBar.Value + FPS;
            if (num > (double)FileLength)
                return;
            vlc.input.Time = num;
            VideoBar.Value = (int)num;
            Update((int)num);
        }

        
        private int GetFrame()
        {
            return (int)((double)VideoBar.Value / (1000.0 / FPS));
        }

        
        private void mnu_TagStart_Click(object sender, EventArgs e)
        {
            int num = VideoBar.Value;
            StartMS = num;
            StartFrame = num;
            StartTag = string.Format("VideoForm_33", GetFrame(), lblTime.Text);
            lbl_TagState.Text = string.Format(LangCtrl.GetString("VideoForm_34", "VideoForm_35"), StartTag);
        }

        
        private void mnu_TagEnd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(StartTag))
                return;
            int num = VideoBar.Value;
            EndMS = num;
            EndFrame = num;
            EndTag = string.Format("VideoForm_36", GetFrame(), lblTime.Text);
            lbl_TagState.Text = string.Format(LangCtrl.GetString("VideoForm_37", "VideoForm_38"), StartTag, EndTag);
        }

        
        private void mnu_TagClear_Click(object sender, EventArgs e)
        {
            ClearTAG();
        }

        
        private void ClearTAG()
        {
            StartTag = EndTag = string.Empty;
            lbl_TagState.Text = LangCtrl.GetString("VideoForm_39", "VideoForm_40");
            StartFrame = EndFrame = StartMS = EndMS = 0;
            mnu_TagLoop.Checked = IsLoop = false;
        }

        
        private void mnu_TagSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveTag saveTag = new SaveTag();
                if (StartMS < 0 || EndMS <= 1 || saveTag.ShowDialog(this) != DialogResult.OK)
                    return;
                vTag = new VideoTag();
                vTag.ShortDesc = saveTag.Desc;
                vTag.StartFrame = (long)StartFrame;
                vTag.EndFrame = (long)EndFrame;
                vTag.StartTime = new double?((double)StartMS / 1000.0);
                vTag.EndTime = new double?((double)EndMS / 1000.0);
                DataFile rec = new DataFile();
                using (RPM_DataFile rpmDataFile = new RPM_DataFile())
                {
                    rec = rpmDataFile.GetDataFile(Media[fileIndex].FileID);
                    rec.VideoTags.Add(vTag);
                    rpmDataFile.SaveUpdate(rec);
                    rpmDataFile.Save();
                }
                TimeSpan timeSpan1 = new TimeSpan(0, 0, Convert.ToInt32(vTag.StartTime));
                TimeSpan timeSpan2 = new TimeSpan(0, 0, Convert.ToInt32(vTag.EndTime));
                string str1 = string.Format("VideoForm_41", timeSpan1.Hours, timeSpan1.Minutes, timeSpan1.Seconds);
                string str2 = string.Format("VideoForm_42", timeSpan2.Hours, timeSpan2.Minutes, timeSpan2.Seconds);
                Global.Log("VideoForm_43", string.Format("VideoForm_44", rec.StoredFileName, rec.FileExtension, str1, str2, vTag.ShortDesc.ToUpper()));
                tagPanel.ListTags(Media[fileIndex].FileID);
            }
            catch
            {
            }
        }

        
        private void mnu_TagLoop_Click(object sender, EventArgs e)
        {
            IsLoop = !IsLoop;
            mnu_TagLoop.Checked = IsLoop;
        }

        
        private void LoadGPS()
        {
            try
            {
                Mapper.ClearPoints();
                Mapper.MapHide();
                if (string.IsNullOrEmpty(Media[fileIndex].Ext2))
                    return;
                Mapper.LoadDataPoints(Media[fileIndex].FileName.Substring(0, Media[fileIndex].FileName.IndexOf('.')), Media[fileIndex].Ext2);
                if (Mapper.GPSDataPoints() <= 0.0)
                    return;
                Mapper.MapShow();
            }
            catch
            {
            }
        }

        
        private void btnSnapshot_Click(object sender, EventArgs ee)
        {
            new Thread(new ThreadStart(WriteSnapshot)).Start();
        }

        
        private void WriteSnapshot()
        {
            try
            {
                string path2 = string.Format("VideoForm_45", Guid.NewGuid());
                DateTime now = DateTime.Now;
                string.Format("VideoForm_46", Global.GlobalAccount.Id, now.Year, now.Month, now.Day);
                string uncPath = Media[fileIndex].UNCPath;
                string str1 = Path.Combine(Path.Combine(Global.UNCServer, Global.RelativePath), uncPath);
                if (!str1.Contains("VideoForm_47"))
                {
                    if (!str1.StartsWith("VideoForm_48"))
                        str1 = "VideoForm_49" + str1;
                }
                else if (str1.Contains("VideoForm_50") && !str1.Contains("VideoForm_51"))
                    str1 = str1.Replace("VideoForm_52", "VideoForm_53");
                if (!Directory.Exists(str1))
                    Directory.CreateDirectory(str1);
                Network.SetAcl(str1);
                Image tNail = null;
                string str2 = Path.Combine(str1, path2);
                Network.SetAcl(str1);
                try
                {
                    float num = (float)(VideoBar.Value / 1000);
                    Global.Log("VideoForm_54", string.Format("VideoForm_55", Media[fileIndex].FileName, num));
                    new FFMpegConverter().GetVideoThumbnail(Media[fileIndex].FileName, str2, new float?(num));
                    Thread.Sleep(500);
                    if (File.Exists(str2))
                    {
                        using (FileStream fileStream = new FileStream(str2, FileMode.Open, FileAccess.Read))
                            tNail = Image.FromStream(fileStream);
                    }
                    //TODO : Доделать завтра
                }
                catch (Exception ex)
                {
                    VideoForm videoForm = this;
                    int num;
                    this.BeginInvoke((Delegate)(() => num = (int)MessageBox.Show((IWin32Window)videoForm, string.Format(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(12358), (object)ex.Message), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(12400), MessageBoxButtons.OK, MessageBoxIcon.Hand)));
                }
                if (tNail == null)
                    return;
                Snapshot rec = new Snapshot();
                using (RPM_Snapshot rpmSnapshot = new RPM_Snapshot())
                {
                    rec.DataFileId = this.Media[this.fileIndex].FileID;
                    rec.FileAddedTimestamp = new DateTime?(DateTime.Now);
                    rec.FileExtension = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9040);
                    tNail = Unity.Utilities.resizeImage(160, 100, Image.FromFile(str2));
                    rec.Thumbnail = Unity.Utilities.ImageToByte(tNail);
                    rec.StoredFileName = path2;
                    rec.FrameNumber = Convert.ToInt32(this.lblFrame.Text);
                    rec.UNCName = Path.Combine(Global.UNCServer, Global.RelativePath);
                    rec.UNCPath = uncPath;
                    rec.FileHash = string.Empty;
                    rpmSnapshot.SaveUpdate(rec);
                    rpmSnapshot.Save();
                }
                this.BeginInvoke((Delegate)(() =>
                {
                    this.btnSnapshot.Enabled = true;
                    if (this.thumbPanel == null)
                        return;
                    this.thumbPanel.AddImage(rec, tNail);
                }));
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }

        
        private void chk_TagsOnly_CheckedChanged(object sender, EventArgs e)
        {
            this.IsPlayTags = !this.IsPlayTags;
            if (this.IsPlayTags)
            {
                this.TagIndex = 0;
                this.VideoBar.Enabled = false;
                this.PlayTags(0);
                this.lbl_TagState.Text = LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9052), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9084));
            }
            else
            {
                this.VideoBar.Enabled = true;
                this.lbl_TagState.Text = LangCtrl.GetString(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9116), eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9134));
            }
        }

        
        private void tagPanel_EVT_MergeVideo(string folder)
        {
            if (this.Media.Count <= 0)
                return;
            MergeVideo mergeVideo = new MergeVideo();
            mergeVideo.EVT_StopPlayer -= new MergeVideo.DEL_StopPlayer(this.mVid_EVT_StopPlayer);
            mergeVideo.EVT_StopPlayer += new MergeVideo.DEL_StopPlayer(this.mVid_EVT_StopPlayer);
            mergeVideo.VideoPath = folder;
            mergeVideo.media = this.Media;
            int num = (int)mergeVideo.ShowDialog((IWin32Window)this);
            mergeVideo.EVT_StopPlayer -= new MergeVideo.DEL_StopPlayer(this.mVid_EVT_StopPlayer);
        }

        
        private void mVid_EVT_StopPlayer()
        {
            this.StopPlayer();
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
            this.SpeedBar.BeginInit();
            this.VolumeBar.BeginInit();
            this.VideoPanel.SuspendLayout();
            this.vlc.BeginInit();
            this.TrackbarTable.SuspendLayout();
            this.VideoMenu.SuspendLayout();
            this.ControlPanel.SuspendLayout();
            this.MenuPanel.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((ISupportInitialize)this.LogoPic).BeginInit();
            this.SuspendLayout();
            this.FormPanel.BackColor = Color.White;
            this.FormPanel.BorderStyle = BorderStyle.FixedSingle;
            this.FormPanel.Controls.Add((Control)this.VCRPanel);
            this.FormPanel.Controls.Add((Control)this.VideoPanel);
            this.FormPanel.Controls.Add((Control)this.TrackbarTable);
            this.FormPanel.Controls.Add((Control)this.ControlPanel);
            this.FormPanel.Controls.Add((Control)this.HeaderPanel);
            this.FormPanel.Dock = DockStyle.Fill;
            this.FormPanel.Font = new Font(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9146), 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.FormPanel.Location = new Point(0, 0);
            this.FormPanel.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9190);
            this.FormPanel.Size = new Size(1000, 650);
            this.FormPanel.TabIndex = 0;
            this.VCRPanel.Controls.Add((Control)this.VolPic);
            this.VCRPanel.Controls.Add((Control)this.btnStop);
            this.VCRPanel.Controls.Add((Control)this.btnPlay);
            this.VCRPanel.Controls.Add((Control)this.picEvidence);
            this.VCRPanel.Controls.Add((Control)this.lblTime);
            this.VCRPanel.Controls.Add((Control)this.lblSet);
            this.VCRPanel.Controls.Add((Control)this.lblFrame);
            this.VCRPanel.Controls.Add((Control)this.lbl_Classification);
            this.VCRPanel.Controls.Add((Control)this.btnPrev);
            this.VCRPanel.Controls.Add((Control)this.lbl_Filedate);
            this.VCRPanel.Controls.Add((Control)this.btnNext);
            this.VCRPanel.Controls.Add((Control)this.chk_TagsOnly);
            this.VCRPanel.Controls.Add((Control)this.btnFrameMinus);
            this.VCRPanel.Controls.Add((Control)this.lbl_SecurityLevel);
            this.VCRPanel.Controls.Add((Control)this.btnFramePlus);
            this.VCRPanel.Controls.Add((Control)this.lblSpeed);
            this.VCRPanel.Controls.Add((Control)this.SpeedBar);
            this.VCRPanel.Controls.Add((Control)this.btnSnapshot);
            this.VCRPanel.Controls.Add((Control)this.VolumeBar);
            this.VCRPanel.Dock = DockStyle.Bottom;
            this.VCRPanel.Location = new Point(0, 544);
            this.VCRPanel.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9212);
            this.VCRPanel.Size = new Size(647, 104);
            this.VCRPanel.TabIndex = 2;
            this.VolPic.Cursor = Cursors.Hand;
            this.VolPic.Image = (Image)Resources.volume;
            this.VolPic.Location = new Point(401, 75);
            this.VolPic.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9232);
            this.VolPic.Size = new Size(20, 20);
            this.VolPic.SizeMode = PictureBoxSizeMode.CenterImage;
            this.VolPic.TabIndex = 42;
            this.VolPic.TabStop = false;
            this.VolPic.MouseClick += new MouseEventHandler(this.VolPic_MouseClick);
            this.btnStop.AllowAnimations = true;
            this.btnStop.BackColor = Color.Transparent;
            this.btnStop.Image = (Image)Resources.stop;
            this.btnStop.Location = new Point(58, 3);
            this.btnStop.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9248);
            this.btnStop.RoundedCornersMask = (byte)15;
            this.btnStop.RoundedCornersRadius = 0;
            this.btnStop.Size = new Size(28, 28);
            this.btnStop.TabIndex = 11;
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnStop.Click += new EventHandler(this.btnStop_Click);
            this.btnPlay.AllowAnimations = true;
            this.btnPlay.BackColor = Color.Transparent;
            this.btnPlay.Image = (Image)Resources.pause;
            this.btnPlay.Location = new Point(24, 3);
            this.btnPlay.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9266);
            this.btnPlay.RoundedCornersMask = (byte)15;
            this.btnPlay.RoundedCornersRadius = 0;
            this.btnPlay.Size = new Size(28, 28);
            this.btnPlay.TabIndex = 10;
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnPlay.Click += new EventHandler(this.btnPlay_Click);
            this.picEvidence.Image = (Image)Resources.star;
            this.picEvidence.Location = new Point(462, 4);
            this.picEvidence.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9284);
            this.picEvidence.Size = new Size(20, 20);
            this.picEvidence.SizeMode = PictureBoxSizeMode.CenterImage;
            this.picEvidence.TabIndex = 41;
            this.picEvidence.TabStop = false;
            this.lblTime.BorderStyle = BorderStyle.FixedSingle;
            this.lblTime.Font = new Font(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9310), 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.lblTime.Location = new Point(24, 41);
            this.lblTime.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9354);
            this.lblTime.Size = new Size(130, 23);
            this.lblTime.TabIndex = 19;
            this.lblTime.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9372);
            this.lblTime.TextAlign = ContentAlignment.MiddleCenter;
            this.lblSet.AutoSize = true;
            this.lblSet.Location = new Point(462, 80);
            this.lblSet.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9400);
            this.lblSet.Size = new Size(23, 13);
            this.lblSet.TabIndex = 40;
            this.lblSet.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9416);
            this.lblFrame.BorderStyle = BorderStyle.FixedSingle;
            this.lblFrame.Font = new Font(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9426), 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.lblFrame.Location = new Point(24, 72);
            this.lblFrame.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9470);
            this.lblFrame.Size = new Size(130, 23);
            this.lblFrame.TabIndex = 20;
            this.lblFrame.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9490);
            this.lblFrame.TextAlign = ContentAlignment.MiddleCenter;
            this.lbl_Classification.AutoSize = true;
            this.lbl_Classification.Location = new Point(462, 63);
            this.lbl_Classification.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9496);
            this.lbl_Classification.Size = new Size(68, 13);
            this.lbl_Classification.TabIndex = 39;
            this.lbl_Classification.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9536);
            this.btnPrev.AllowAnimations = true;
            this.btnPrev.BackColor = Color.Transparent;
            this.btnPrev.Enabled = false;
            this.btnPrev.Image = (Image)Resources.file_prev;
            this.btnPrev.Location = new Point(92, 3);
            this.btnPrev.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9568);
            this.btnPrev.RoundedCornersMask = (byte)15;
            this.btnPrev.RoundedCornersRadius = 0;
            this.btnPrev.Size = new Size(28, 28);
            this.btnPrev.TabIndex = 21;
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnPrev.Click += new EventHandler(this.btnPrev_Click);
            this.lbl_Filedate.AutoSize = true;
            this.lbl_Filedate.Location = new Point(462, 46);
            this.lbl_Filedate.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9586);
            this.lbl_Filedate.Size = new Size(158, 13);
            this.lbl_Filedate.TabIndex = 37;
            this.lbl_Filedate.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9614);
            this.btnNext.AllowAnimations = true;
            this.btnNext.BackColor = Color.Transparent;
            this.btnNext.Enabled = false;
            this.btnNext.Image = (Image)Resources.filenext;
            this.btnNext.Location = new Point(126, 3);
            this.btnNext.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9678);
            this.btnNext.RoundedCornersMask = (byte)15;
            this.btnNext.RoundedCornersRadius = 0;
            this.btnNext.Size = new Size(28, 28);
            this.btnNext.TabIndex = 22;
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnNext.Click += new EventHandler(this.btnNext_Click);
            this.chk_TagsOnly.BackColor = Color.Transparent;
            this.chk_TagsOnly.Location = new Point(172, 41);
            this.chk_TagsOnly.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9696);
            this.chk_TagsOnly.Size = new Size(118, 24);
            this.chk_TagsOnly.TabIndex = 36;
            this.chk_TagsOnly.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9724);
            this.chk_TagsOnly.UseVisualStyleBackColor = false;
            this.chk_TagsOnly.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.chk_TagsOnly.CheckedChanged += new EventHandler(this.chk_TagsOnly_CheckedChanged);
            this.btnFrameMinus.AllowAnimations = true;
            this.btnFrameMinus.BackColor = Color.Transparent;
            this.btnFrameMinus.Font = new Font(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9760), 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.btnFrameMinus.Location = new Point(172, 3);
            this.btnFrameMinus.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9804);
            this.btnFrameMinus.RoundedCornersMask = (byte)15;
            this.btnFrameMinus.RoundedCornersRadius = 0;
            this.btnFrameMinus.Size = new Size(28, 28);
            this.btnFrameMinus.TabIndex = 23;
            this.btnFrameMinus.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9834);
            this.btnFrameMinus.UseVisualStyleBackColor = false;
            this.btnFrameMinus.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnFrameMinus.Click += new EventHandler(this.btnFrameMinus_Click);
            this.lbl_SecurityLevel.AutoSize = true;
            this.lbl_SecurityLevel.BorderStyle = BorderStyle.FixedSingle;
            this.lbl_SecurityLevel.Location = new Point(462, 27);
            this.lbl_SecurityLevel.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9840);
            this.lbl_SecurityLevel.Size = new Size(66, 15);
            this.lbl_SecurityLevel.TabIndex = 35;
            this.lbl_SecurityLevel.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9878);
            this.btnFramePlus.AllowAnimations = true;
            this.btnFramePlus.BackColor = Color.Transparent;
            this.btnFramePlus.Font = new Font(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9906), 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.btnFramePlus.Location = new Point(206, 3);
            this.btnFramePlus.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9950);
            this.btnFramePlus.RoundedCornersMask = (byte)15;
            this.btnFramePlus.RoundedCornersRadius = 0;
            this.btnFramePlus.Size = new Size(28, 28);
            this.btnFramePlus.TabIndex = 25;
            this.btnFramePlus.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9978);
            this.btnFramePlus.UseVisualStyleBackColor = false;
            this.btnFramePlus.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnFramePlus.Click += new EventHandler(this.btnFramePlus_Click);
            this.lblSpeed.Location = new Point((int)byte.MaxValue, 81);
            this.lblSpeed.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(9984);
            this.lblSpeed.Size = new Size(49, 14);
            this.lblSpeed.TabIndex = 33;
            this.lblSpeed.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10004);
            this.lblSpeed.TextAlign = ContentAlignment.MiddleRight;
            this.SpeedBar.AutoSize = false;
            this.SpeedBar.Location = new Point(310, 3);
            this.SpeedBar.Maximum = 8;
            this.SpeedBar.Minimum = 1;
            this.SpeedBar.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10012);
            this.SpeedBar.Orientation = Orientation.Vertical;
            this.SpeedBar.Size = new Size(40, 98);
            this.SpeedBar.TabIndex = 29;
            this.SpeedBar.Value = 4;
            this.SpeedBar.Scroll += new EventHandler(this.SpeedBar_Scroll);
            this.btnSnapshot.AllowAnimations = true;
            this.btnSnapshot.BackColor = Color.Transparent;
            this.btnSnapshot.Image = (Image)Resources.snapshot;
            this.btnSnapshot.Location = new Point(240, 3);
            this.btnSnapshot.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10032);
            this.btnSnapshot.RoundedCornersMask = (byte)15;
            this.btnSnapshot.RoundedCornersRadius = 0;
            this.btnSnapshot.Size = new Size(28, 28);
            this.btnSnapshot.TabIndex = 32;
            this.btnSnapshot.UseVisualStyleBackColor = false;
            this.btnSnapshot.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnSnapshot.Click += new EventHandler(this.btnSnapshot_Click);
            this.VolumeBar.AutoSize = false;
            this.VolumeBar.Location = new Point(357, 3);
            this.VolumeBar.Maximum = 100;
            this.VolumeBar.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10058);
            this.VolumeBar.Orientation = Orientation.Vertical;
            this.VolumeBar.Size = new Size(40, 98);
            this.VolumeBar.TabIndex = 30;
            this.VolumeBar.TickFrequency = 10;
            this.VolumeBar.TickStyle = TickStyle.TopLeft;
            this.VolumeBar.Value = 50;
            this.VolumeBar.Scroll += new EventHandler(this.VolumeBar_Scroll);
            this.VideoPanel.BackColor = Color.Black;
            this.VideoPanel.BackgroundImage = (Image)Resources.video;
            this.VideoPanel.BackgroundImageLayout = ImageLayout.Center;
            this.VideoPanel.Controls.Add((Control)this.vlc);
            this.VideoPanel.Dock = DockStyle.Top;
            this.VideoPanel.Location = new Point(0, 45);
            this.VideoPanel.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10080);
            this.VideoPanel.Padding = new Padding(0, 1, 0, 0);
            this.VideoPanel.Size = new Size(647, 451);
            this.VideoPanel.TabIndex = 0;
            this.vlc.Dock = DockStyle.Fill;
            this.vlc.Enabled = true;
            this.vlc.Location = new Point(0, 1);
            this.vlc.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10104);
            this.vlc.OcxState = (AxHost.State)componentResourceManager.GetObject(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10114));
            this.vlc.Size = new Size(647, 450);
            this.vlc.TabIndex = 0;
            this.vlc.MediaPlayerEndReached += new EventHandler(this.vlc_MediaPlayerEndReached);
            this.TrackbarTable.ColumnCount = 5;
            this.TrackbarTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
            this.TrackbarTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            this.TrackbarTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            this.TrackbarTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            this.TrackbarTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 22f));
            this.TrackbarTable.Controls.Add((Control)this.VideoBar, 1, 0);
            this.TrackbarTable.Controls.Add((Control)this.lbl_File, 2, 1);
            this.TrackbarTable.Controls.Add((Control)this.lbl_TagState, 1, 1);
            this.TrackbarTable.Controls.Add((Control)this.lbl_VideoTime, 3, 1);
            this.TrackbarTable.Location = new Point(0, 499);
            this.TrackbarTable.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10142);
            this.TrackbarTable.RowCount = 2;
            this.TrackbarTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.TrackbarTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 14f));
            this.TrackbarTable.Size = new Size(647, 42);
            this.TrackbarTable.TabIndex = 1;
            this.VideoBar.BackColor = Color.Transparent;
            this.TrackbarTable.SetColumnSpan((Control)this.VideoBar, 3);
            this.VideoBar.ContextMenuStrip = this.VideoMenu;
            this.VideoBar.Dock = DockStyle.Fill;
            this.VideoBar.Location = new Point(23, 3);
            this.VideoBar.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10172);
            this.VideoBar.RoundedCornersMask = (byte)15;
            this.VideoBar.RoundedCornersMaskThumb = (byte)15;
            this.VideoBar.RoundedCornersRadius = 0;
            this.VideoBar.RoundedCornersRadiusThumb = 0;
            this.VideoBar.Size = new Size(599, 22);
            this.VideoBar.TabIndex = 0;
            this.VideoBar.Value = 0;
            this.VideoBar.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.VideoBar.Scroll += new ScrollEventHandler(this.VideoBar_Scroll);
            this.VideoMenu.Items.AddRange(new ToolStripItem[6]
            {
        (ToolStripItem) this.mnu_TagStart,
        (ToolStripItem) this.mnu_TagEnd,
        (ToolStripItem) this.mnu_TagClear,
        (ToolStripItem) this.toolStripMenuItem1,
        (ToolStripItem) this.mnu_TagSave,
        (ToolStripItem) this.mnu_TagLoop
            });
            this.VideoMenu.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10192);
            this.VideoMenu.Size = new Size(170, 120);
            this.mnu_TagStart.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10214);
            this.mnu_TagStart.Size = new Size(169, 22);
            this.mnu_TagStart.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10242);
            this.mnu_TagStart.Click += new EventHandler(this.mnu_TagStart_Click);
            this.mnu_TagEnd.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10270);
            this.mnu_TagEnd.Size = new Size(169, 22);
            this.mnu_TagEnd.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10294);
            this.mnu_TagEnd.Click += new EventHandler(this.mnu_TagEnd_Click);
            this.mnu_TagClear.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10318);
            this.mnu_TagClear.Size = new Size(169, 22);
            this.mnu_TagClear.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10346);
            this.mnu_TagClear.Click += new EventHandler(this.mnu_TagClear_Click);
            this.toolStripMenuItem1.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10374);
            this.toolStripMenuItem1.Size = new Size(166, 6);
            this.mnu_TagSave.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10414);
            this.mnu_TagSave.Size = new Size(169, 22);
            this.mnu_TagSave.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10440);
            this.mnu_TagSave.Click += new EventHandler(this.mnu_TagSave_Click);
            this.mnu_TagLoop.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10474);
            this.mnu_TagLoop.Size = new Size(169, 22);
            this.mnu_TagLoop.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10500);
            this.mnu_TagLoop.Click += new EventHandler(this.mnu_TagLoop_Click);
            this.lbl_File.AutoSize = true;
            this.lbl_File.Dock = DockStyle.Fill;
            this.lbl_File.Location = new Point(265, 28);
            this.lbl_File.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10536);
            this.lbl_File.Size = new Size(115, 14);
            this.lbl_File.TabIndex = 34;
            this.lbl_File.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10556);
            this.lbl_File.TextAlign = ContentAlignment.MiddleCenter;
            this.lbl_TagState.AutoSize = true;
            this.lbl_TagState.Dock = DockStyle.Fill;
            this.lbl_TagState.Font = new Font(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10580), 6.75f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.lbl_TagState.ForeColor = Color.FromArgb(64, 64, 64);
            this.lbl_TagState.Location = new Point(23, 28);
            this.lbl_TagState.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10624);
            this.lbl_TagState.Size = new Size(236, 14);
            this.lbl_TagState.TabIndex = 1;
            this.lbl_TagState.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10652);
            this.lbl_VideoTime.AutoSize = true;
            this.lbl_VideoTime.Dock = DockStyle.Fill;
            this.lbl_VideoTime.Font = new Font(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10664), 6.75f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.lbl_VideoTime.ForeColor = Color.FromArgb(64, 64, 64);
            this.lbl_VideoTime.Location = new Point(386, 28);
            this.lbl_VideoTime.Margin = new Padding(3, 0, 0, 0);
            this.lbl_VideoTime.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10708);
            this.lbl_VideoTime.Size = new Size(239, 14);
            this.lbl_VideoTime.TabIndex = 2;
            this.lbl_VideoTime.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10738);
            this.lbl_VideoTime.TextAlign = ContentAlignment.MiddleRight;
            this.ControlPanel.AutoScroll = true;
            this.ControlPanel.Controls.Add((Control)this.OptionPanel);
            this.ControlPanel.Controls.Add((Control)this.MenuPanel);
            this.ControlPanel.Dock = DockStyle.Right;
            this.ControlPanel.Location = new Point(647, 45);
            this.ControlPanel.Margin = new Padding(0);
            this.ControlPanel.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10766);
            this.ControlPanel.Size = new Size(351, 603);
            this.ControlPanel.TabIndex = 2;
            this.OptionPanel.Dock = DockStyle.Fill;
            this.OptionPanel.Location = new Point(0, 0);
            this.OptionPanel.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10794);
            this.OptionPanel.Size = new Size(351, 553);
            this.OptionPanel.TabIndex = 1;
            this.MenuPanel.Controls.Add((Control)this.btnImageFiles);
            this.MenuPanel.Controls.Add((Control)this.btn_Map);
            this.MenuPanel.Controls.Add((Control)this.btnFiles);
            this.MenuPanel.Controls.Add((Control)this.btnTags);
            this.MenuPanel.Controls.Add((Control)this.btnThumbnails);
            this.MenuPanel.Dock = DockStyle.Bottom;
            this.MenuPanel.Location = new Point(0, 553);
            this.MenuPanel.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10820);
            this.MenuPanel.Size = new Size(351, 50);
            this.MenuPanel.TabIndex = 0;
            this.btnImageFiles.AllowAnimations = true;
            this.btnImageFiles.BackColor = Color.Transparent;
            this.btnImageFiles.Font = new Font(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10842), 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.btnImageFiles.Image = (Image)Resources.picture_2;
            this.btnImageFiles.Location = new Point(140, 11);
            this.btnImageFiles.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10886);
            this.btnImageFiles.RoundedCornersMask = (byte)15;
            this.btnImageFiles.RoundedCornersRadius = 0;
            this.btnImageFiles.Size = new Size(28, 28);
            this.btnImageFiles.TabIndex = 30;
            this.btnImageFiles.UseVisualStyleBackColor = false;
            this.btnImageFiles.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnImageFiles.Click += new EventHandler(this.btnImageFiles_Click);
            this.btn_Map.AllowAnimations = true;
            this.btn_Map.BackColor = Color.Transparent;
            this.btn_Map.Font = new Font(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10916), 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.btn_Map.Image = (Image)Resources.map;
            this.btn_Map.Location = new Point(4, 11);
            this.btn_Map.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10960);
            this.btn_Map.RoundedCornersMask = (byte)15;
            this.btn_Map.RoundedCornersRadius = 0;
            this.btn_Map.Size = new Size(28, 28);
            this.btn_Map.TabIndex = 26;
            this.btn_Map.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10978);
            this.btn_Map.UseVisualStyleBackColor = false;
            this.btn_Map.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btn_Map.Click += new EventHandler(this.btn_Map_Click);
            this.btnFiles.AllowAnimations = true;
            this.btnFiles.BackColor = Color.Transparent;
            this.btnFiles.Font = new Font(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(10984), 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.btnFiles.Image = (Image)Resources.folder2;
            this.btnFiles.Location = new Point(106, 11);
            this.btnFiles.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11028);
            this.btnFiles.RoundedCornersMask = (byte)15;
            this.btnFiles.RoundedCornersRadius = 0;
            this.btnFiles.Size = new Size(28, 28);
            this.btnFiles.TabIndex = 29;
            this.btnFiles.UseVisualStyleBackColor = false;
            this.btnFiles.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnFiles.Click += new EventHandler(this.btnFiles_Click);
            this.btnTags.AllowAnimations = true;
            this.btnTags.BackColor = Color.Transparent;
            this.btnTags.Font = new Font(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11048), 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.btnTags.Image = (Image)Resources.tag;
            this.btnTags.Location = new Point(72, 11);
            this.btnTags.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11092);
            this.btnTags.RoundedCornersMask = (byte)15;
            this.btnTags.RoundedCornersRadius = 0;
            this.btnTags.Size = new Size(28, 28);
            this.btnTags.TabIndex = 28;
            this.btnTags.UseVisualStyleBackColor = false;
            this.btnTags.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnTags.Click += new EventHandler(this.btnTags_Click);
            this.btnThumbnails.AllowAnimations = true;
            this.btnThumbnails.BackColor = Color.Transparent;
            this.btnThumbnails.Font = new Font(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11110), 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.btnThumbnails.Image = (Image)Resources.thumbnail;
            this.btnThumbnails.Location = new Point(38, 11);
            this.btnThumbnails.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11154);
            this.btnThumbnails.RoundedCornersMask = (byte)15;
            this.btnThumbnails.RoundedCornersRadius = 0;
            this.btnThumbnails.Size = new Size(28, 28);
            this.btnThumbnails.TabIndex = 27;
            this.btnThumbnails.UseVisualStyleBackColor = false;
            this.btnThumbnails.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnThumbnails.Click += new EventHandler(this.btnThumbnails_Click);
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add((Control)this.lbl_ImageFileCount);
            this.HeaderPanel.Controls.Add((Control)this.lbl_VideoFileCount);
            this.HeaderPanel.Controls.Add((Control)this.lblVideoTitle);
            this.HeaderPanel.Controls.Add((Control)this.btnCtrlPanel);
            this.HeaderPanel.Controls.Add((Control)this.LogoPic);
            this.HeaderPanel.Controls.Add((Control)this.btnClose);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11184);
            this.HeaderPanel.Size = new Size(998, 45);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.lbl_ImageFileCount.AutoSize = true;
            this.lbl_ImageFileCount.BackColor = Color.Transparent;
            this.lbl_ImageFileCount.ForeColor = Color.White;
            this.lbl_ImageFileCount.Location = new Point(285, 25);
            this.lbl_ImageFileCount.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11210);
            this.lbl_ImageFileCount.Size = new Size(72, 13);
            this.lbl_ImageFileCount.TabIndex = 6;
            this.lbl_ImageFileCount.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11250);
            this.lbl_VideoFileCount.AutoSize = true;
            this.lbl_VideoFileCount.BackColor = Color.Transparent;
            this.lbl_VideoFileCount.ForeColor = Color.White;
            this.lbl_VideoFileCount.Location = new Point(285, 8);
            this.lbl_VideoFileCount.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11282);
            this.lbl_VideoFileCount.Size = new Size(70, 13);
            this.lbl_VideoFileCount.TabIndex = 5;
            this.lbl_VideoFileCount.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11322);
            this.lblVideoTitle.AutoSize = true;
            this.lblVideoTitle.BackColor = Color.Transparent;
            this.lblVideoTitle.Font = new Font(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11354), 12f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lblVideoTitle.ForeColor = Color.White;
            this.lblVideoTitle.Location = new Point(53, 4);
            this.lblVideoTitle.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11398);
            this.lblVideoTitle.Size = new Size(141, 20);
            this.lblVideoTitle.TabIndex = 4;
            this.lblVideoTitle.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11428);
            this.lblVideoTitle.TextAlign = ContentAlignment.MiddleLeft;
            this.lblVideoTitle.MouseDown += new MouseEventHandler(this.lblVideoTitle_MouseDown);
            this.btnCtrlPanel.AllowAnimations = true;
            this.btnCtrlPanel.BackColor = Color.Transparent;
            this.btnCtrlPanel.Image = (Image)Resources.showhide;
            this.btnCtrlPanel.Location = new Point(650, 5);
            this.btnCtrlPanel.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11456);
            this.btnCtrlPanel.PaintBorder = false;
            this.btnCtrlPanel.PaintDefaultBorder = false;
            this.btnCtrlPanel.PaintDefaultFill = false;
            this.btnCtrlPanel.RoundedCornersMask = (byte)15;
            this.btnCtrlPanel.RoundedCornersRadius = 0;
            this.btnCtrlPanel.Size = new Size(36, 36);
            this.btnCtrlPanel.TabIndex = 2;
            this.btnCtrlPanel.UseVisualStyleBackColor = false;
            this.btnCtrlPanel.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnCtrlPanel.Click += new EventHandler(this.btnCtrlPanel_Click);
            this.LogoPic.BackColor = Color.Transparent;
            this.LogoPic.Image = (Image)Resources.camlens2;
            this.LogoPic.Location = new Point(8, 4);
            this.LogoPic.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11484);
            this.LogoPic.Size = new Size(36, 36);
            this.LogoPic.SizeMode = PictureBoxSizeMode.CenterImage;
            this.LogoPic.TabIndex = 1;
            this.LogoPic.TabStop = false;
            this.LogoPic.MouseDown += new MouseEventHandler(this.LogoPic_MouseDown);
            this.btnClose.AllowAnimations = true;
            this.btnClose.BackColor = Color.Transparent;
            this.btnClose.Dock = DockStyle.Right;
            this.btnClose.Image = (Image)Resources.close;
            this.btnClose.Location = new Point(952, 0);
            this.btnClose.Margin = new Padding(0);
            this.btnClose.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11502);
            this.btnClose.PaintBorder = false;
            this.btnClose.PaintDefaultBorder = false;
            this.btnClose.PaintDefaultFill = false;
            this.btnClose.RoundedCornersMask = (byte)15;
            this.btnClose.RoundedCornersRadius = 0;
            this.btnClose.Size = new Size(46, 45);
            this.btnClose.TabIndex = 0;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.openFileDialog1.FileName = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11522);
            this.openFileDialog1.Multiselect = true;
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1000, 650);
            this.Controls.Add((Control)this.FormPanel);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Icon = (Icon)componentResourceManager.GetObject(eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11556));
            this.Name = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11580);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = eNQ3Jf6G6vENo1KFlF.eacsfnmlb(11602);
            this.FormClosing += new FormClosingEventHandler(this.VideoForm_FormClosing);
            this.Load += new EventHandler(this.VideoForm_Load);
            this.FormPanel.ResumeLayout(false);
            this.VCRPanel.ResumeLayout(false);
            this.VCRPanel.PerformLayout();
            ((ISupportInitialize)this.VolPic).EndInit();
            ((ISupportInitialize)this.picEvidence).EndInit();
            this.SpeedBar.EndInit();
            this.VolumeBar.EndInit();
            this.VideoPanel.ResumeLayout(false);
            this.vlc.EndInit();
            this.TrackbarTable.ResumeLayout(false);
            this.TrackbarTable.PerformLayout();
            this.VideoMenu.ResumeLayout(false);
            this.ControlPanel.ResumeLayout(false);
            this.MenuPanel.ResumeLayout(false);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((ISupportInitialize)this.LogoPic).EndInit();
            this.ResumeLayout(false);
        }
    }
}
