using AppGlobal;
using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VideoPlayer2.Properties;
using VMInterfaces;
using VMModels.Enums;
using VMModels.Model;

namespace VideoPlayer2
{
    public class MergeVideo : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;
        public const int HT_CAPTION = 2;
        private const int CS_DROPSHADOW = 131072;
        public List<VideoTag> TagList;
        private List<string> tFiles;
        private IContainer components;
        private Panel FormPanel;
        private vTextBox txtName;
        private Label lbl_VideoName;
        private Panel HeaderPanel;
        private vButton btn_Close;
        private vButton btn_Merge;
        private Label lbl_MergeTags;
        private Label lbl_VideoFormat;
        private Label lblPath;
        private vListBox vListBox;
        private vRadioButton btnMOV;
        private vRadioButton btnAVI;
        private vRadioButton btnMP4;
        private PictureBox pictureBox1;
        private vCheckBox chk_SaveSegments;

        public string VideoPath {  get;  set; }

        public List<MediaFile> media {  get;  set; }

        protected override CreateParams CreateParams
        {
            
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ClassStyle |= 131072;
                return createParams;
            }
        }

        public event DEL_StopPlayer EVT_StopPlayer;

        
        public MergeVideo()
        {
            TagList = new List<VideoTag>();
            tFiles = new List<string>();
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        
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

        
        private void lbl_MergeTags_MouseDown(object sender, MouseEventArgs e)
        {
            HeaderMouseDown(e);
        }

        
        private void MergeVideo_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
                HeaderPanel.BackgroundImage = (Image)Properties.Resources.topbar45;
            LangCtrl.reText(this);
            lblPath.Text = VideoPath;
            vListBox.Items.Add(string.Format(LangCtrl.GetString("VideoPlay_MergeV_1", "VideoPlay_MergeV_2"), media.Count));
        }

        
        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        
        private void btn_Merge_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtName.Text))
            {
                ValidateFileName();
                pictureBox1.Visible = true;
                vListBox.Items.Insert(string.Format(LangCtrl.GetString("VideoPlay_MergeV_3", "VideoPlay_MergeV_4"), DateTime.Now), 0);
                btn_Close.Enabled = btn_Merge.Enabled = false;
                Callback();
                new Thread(new ThreadStart(MergeFiles)).Start();
            }
            else
            {
                int num = (int)MessageBox.Show(this, LangCtrl.GetString("VideoPlay_MergeV_5", "VideoPlay_MergeV_6"), "VideoPlay_MergeV_7", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        
        public void ValidateFileName()
        {
            txtName.Text = txtName.Text.Replace(' ', '_');
        }

        
        private void MergeFiles()
        {
            string RedactFile = string.Empty;
            int num1 = 0;
            if (media.Count <= 0)
                return;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Network.SetAcl(this.VideoPath);
            using (RPM_VideoTag rpmVideoTag = new RPM_VideoTag())
            {
                foreach (MediaFile mediaFile in media)
                {
                    TagList = rpmVideoTag.GetTagList(mediaFile.FileID);
                    foreach (VideoTag tag in TagList)
                    {
                        TimeSpan timeSpan1 = new TimeSpan(0, 0, Convert.ToInt32(tag.StartTime));
                        TimeSpan timeSpan2 = new TimeSpan(0, 0, Convert.ToInt32(tag.EndTime));
                        string str1 = string.Format("VideoPlay_MergeV_8", timeSpan1.Hours, timeSpan1.Minutes, timeSpan1.Seconds);
                        string str2 = string.Format("VideoPlay_MergeV_9", timeSpan2.Hours, timeSpan2.Minutes, timeSpan2.Seconds);
                        TimeSpan timeSpan3 = timeSpan2 - timeSpan1;
                        string str3 = string.Format("VideoPlay_MergeV_10", timeSpan3.Hours, timeSpan3.Minutes, timeSpan3.Seconds);
                        vListBox.Items.Insert(string.Format("VideoPlay_MergeV_11", tag.ShortDesc, str1, str2, str3), 0);
                        string path2 = string.Format("VideoPlay_MergeV_12", num1, Path.GetExtension(mediaFile.FileName));
                        try
                        {
                            Process.Start(new ProcessStartInfo()
                            {
                                UseShellExecute = false,
                                CreateNoWindow = true,
                                FileName = Path.Combine(Directory.GetCurrentDirectory(), "VideoPlay_MergeV_13"),
                                Arguments = string.Format("VideoPlay_MergeV_14", str1, mediaFile.FileName, str3, VideoPath, path2)
                            }).WaitForExit();
                        }
                        catch (Exception ex)
                        {
                            vListBox.Items.Insert(ex.Message, 0);
                            int num2 = (int)MessageBox.Show(ex.Message);
                        }
                        string path = Path.Combine(VideoPath, path2);
                        if (File.Exists(path))
                        {
                            tFiles.Add(string.Format("VideoPlay_MergeV_15", path));
                            ++num1;
                        }
                        else
                            vListBox.Items.Insert(string.Format(LangCtrl.GetString("VideoPlay_MergeV_16", "VideoPlay_MergeV_17"), path2), 0);
                    }
                }
                string[] inputFiles = new string[tFiles.Count];
                int index = 0;
                foreach (string tFile in tFiles)
                {
                    if (File.Exists(tFile))
                    {
                        inputFiles[index] = tFile;
                        ++index;
                    }
                }
                if (inputFiles.Length > 0)
                {
                    string outputFormat = "VideoPlay_MergeV_18";
                    if (btnAVI.Checked)
                        outputFormat = "VideoPlay_MergeV_19";
                    if (btnMOV.Checked)
                        outputFormat = "VideoPlay_MergeV_20";
                    string str = string.Format("VideoPlay_MergeV_21", VideoPath, txtName.Text, outputFormat);
                    RedactFile = str;
                    vListBox.Items.Insert(LangCtrl.GetString("VideoPlay_MergeV_22", "VideoPlay_MergeV_23"), 0);
                    Thread.Sleep(3000);
                    if (inputFiles.Length == 1)
                    {
                        File.Move(inputFiles[0], str);
                    }
                    else
                    {
                        try
                        {
                            ConcatSettings settings = new ConcatSettings();
                            FFMpegConverter ffMpegConverter = new FFMpegConverter();
                            Thread.Sleep(1000);
                            ffMpegConverter.ConcatMedia(inputFiles, str, outputFormat, settings);
                        }
                        catch (Exception ex)
                        {
                            vListBox.Items.Insert(ex.Message, 0);
                        }
                    }
                }
                if (!chk_SaveSegments.Checked)
                {
                    try
                    {
                        foreach (string tFile in tFiles)
                        {
                            if (File.Exists(tFile))
                                File.Delete(tFile);
                        }
                    }
                    catch (Exception ex)
                    {
                        vListBox.Items.Insert(ex.Message, 0);
                    }
                }
                stopwatch.Stop();
                TimeSpan elapsed = stopwatch.Elapsed;
                vListBox.Items.Insert(string.Format(LangCtrl.GetString("VideoPlay_MergeV_24", "VideoPlay_MergeV_25"), DateTime.Now, string.Format("VideoPlay_MergeV_26", elapsed.Hours, elapsed.Minutes, elapsed.Seconds)), 0);
                if (File.Exists(RedactFile))
                {
                    if (Global.IsRights(Global.RightsProfile, UserRights.REDACT))
                    {
                        // TODO : Здесь кое-что поменял, надо будет посмотреть исходники
                        Action p1;
                        if (Global.IsRedact)
                            BeginInvoke((p1 = () =>
                            {
                                if (MessageBox.Show(this, LangCtrl.GetString("VideoPlay_MergeV_27", "VideoPlay_MergeV_28"), "VideoPlay_MergeV_29", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                                    return;
                                try
                                {
                                    Global.Log("VideoPlay_MergeV_30", string.Format("VideoPlay_MergeV_31", RedactFile));
                                    string str = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "VideoPlay_MergeV_32");
                                    Process.Start(new ProcessStartInfo()
                                    {
                                        FileName = str,
                                        Arguments = "VideoPlay_MergeV_33" + RedactFile + "VideoPlay_MergeV_34"
                                    });
                                }
                                catch
                                {
                                }
                            }));
                    }
                }
            }
            // TODO : Здесь кое-что поменял, надо будет посмотреть исходники(2)
            Action p2;
            BeginInvoke((p2 = () =>
            {
                btn_Close.Enabled = true;
                pictureBox1.Visible = false;
            }));
        }
        

        private void Callback()
        {
            if (EVT_StopPlayer == null)
                return;
            EVT_StopPlayer();
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        
        private void InitializeComponent()
        {
            this.FormPanel = new Panel();
            this.chk_SaveSegments = new vCheckBox();
            this.btnMOV = new vRadioButton();
            this.btnAVI = new vRadioButton();
            this.btnMP4 = new vRadioButton();
            this.vListBox = new vListBox();
            this.pictureBox1 = new PictureBox();
            this.lblPath = new Label();
            this.lbl_VideoFormat = new Label();
            this.btn_Close = new vButton();
            this.btn_Merge = new vButton();
            this.txtName = new vTextBox();
            this.lbl_VideoName = new Label();
            this.HeaderPanel = new Panel();
            this.lbl_MergeTags = new Label();
            this.FormPanel.SuspendLayout();
            this.vListBox.SuspendLayout();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            this.HeaderPanel.SuspendLayout();
            this.SuspendLayout();
            this.FormPanel.BorderStyle = BorderStyle.FixedSingle;
            this.FormPanel.Controls.Add((Control)this.chk_SaveSegments);
            this.FormPanel.Controls.Add((Control)this.btnMOV);
            this.FormPanel.Controls.Add((Control)this.btnAVI);
            this.FormPanel.Controls.Add((Control)this.btnMP4);
            this.FormPanel.Controls.Add((Control)this.vListBox);
            this.FormPanel.Controls.Add((Control)this.lblPath);
            this.FormPanel.Controls.Add((Control)this.lbl_VideoFormat);
            this.FormPanel.Controls.Add((Control)this.btn_Close);
            this.FormPanel.Controls.Add((Control)this.btn_Merge);
            this.FormPanel.Controls.Add((Control)this.txtName);
            this.FormPanel.Controls.Add((Control)this.lbl_VideoName);
            this.FormPanel.Controls.Add((Control)this.HeaderPanel);
            this.FormPanel.Dock = DockStyle.Fill;
            this.FormPanel.Location = new Point(0, 0);
            this.FormPanel.Name = "VideoPlay_MergeV_35";
            this.FormPanel.Size = new Size(537, 376);
            this.FormPanel.TabIndex = 0;
            this.chk_SaveSegments.BackColor = Color.Transparent;
            this.chk_SaveSegments.Location = new Point(159, 135);
            this.chk_SaveSegments.Name = "VideoPlay_MergeV_36";
            this.chk_SaveSegments.Size = new Size(213, 24);
            this.chk_SaveSegments.TabIndex = 15;
            this.chk_SaveSegments.Text = "VideoPlay_MergeV_37";
            this.chk_SaveSegments.UseVisualStyleBackColor = false;
            this.chk_SaveSegments.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnMOV.BackColor = Color.Transparent;
            this.btnMOV.Flat = true;
            this.btnMOV.Image = (Image)null;
            this.btnMOV.Location = new Point(289, 104);
            this.btnMOV.Name = "VideoPlay_MergeV_38";
            this.btnMOV.Size = new Size(59, 24);
            this.btnMOV.TabIndex = 14;
            this.btnMOV.Text = "VideoPlay_MergeV_39";
            this.btnMOV.UseVisualStyleBackColor = false;
            this.btnMOV.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnAVI.BackColor = Color.Transparent;
            this.btnAVI.Flat = true;
            this.btnAVI.Image = (Image)null;
            this.btnAVI.Location = new Point(224, 104);
            this.btnAVI.Name = "VideoPlay_MergeV_40";
            this.btnAVI.Size = new Size(59, 24);
            this.btnAVI.TabIndex = 13;
            this.btnAVI.Text = "VideoPlay_MergeV_41";
            this.btnAVI.UseVisualStyleBackColor = false;
            this.btnAVI.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnMP4.BackColor = Color.Transparent;
            this.btnMP4.Checked = true;
            this.btnMP4.Flat = true;
            this.btnMP4.Image = (Image)null;
            this.btnMP4.Location = new Point(159, 104);
            this.btnMP4.Name = "VideoPlay_MergeV_42";
            this.btnMP4.Size = new Size(59, 24);
            this.btnMP4.TabIndex = 12;
            this.btnMP4.TabStop = true;
            this.btnMP4.Text = "VideoPlay_MergeV_43";
            this.btnMP4.UseVisualStyleBackColor = false;
            this.btnMP4.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.vListBox.Controls.Add((Control)this.pictureBox1);
            this.vListBox.Location = new Point(15, 166);
            this.vListBox.Name = "VideoPlay_MergeV_44";
            this.vListBox.RoundedCornersMaskListItem = (byte)15;
            this.vListBox.SelectionMode = SelectionMode.None;
            this.vListBox.Size = new Size(509, 197);
            this.vListBox.TabIndex = 11;
            this.vListBox.VIBlendScrollBarsTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.vListBox.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.pictureBox1.Image = (Image)Properties.Resources.hbscreen_e0;
            this.pictureBox1.Location = new Point(450, 2);
            this.pictureBox1.Name = "VideoPlay_MergeV_45";
            this.pictureBox1.Size = new Size(42, 30);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            this.lblPath.AutoEllipsis = true;
            this.lblPath.Location = new Point(159, 53);
            this.lblPath.Name = "VideoPlay_MergeV_46";
            this.lblPath.Size = new Size(213, 13);
            this.lblPath.TabIndex = 10;
            this.lblPath.Text = "VideoPlay_MergeV_47";
            this.lbl_VideoFormat.AutoSize = true;
            this.lbl_VideoFormat.Location = new Point(12, 110);
            this.lbl_VideoFormat.Name = "VideoPlay_MergeV_48";
            this.lbl_VideoFormat.Size = new Size(69, 13);
            this.lbl_VideoFormat.TabIndex = 8;
            this.lbl_VideoFormat.Text = "VideoPlay_MergeV_49";
            this.btn_Close.AllowAnimations = true;
            this.btn_Close.BackColor = Color.Transparent;
            this.btn_Close.Location = new Point(394, 98);
            this.btn_Close.Name = "VideoPlay_MergeV_50";
            this.btn_Close.RoundedCornersMask = (byte)15;
            this.btn_Close.RoundedCornersRadius = 0;
            this.btn_Close.Size = new Size(130, 30);
            this.btn_Close.TabIndex = 7;
            this.btn_Close.Text = "VideoPlay_MergeV_51";
            this.btn_Close.UseVisualStyleBackColor = false;
            this.btn_Close.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Close.Click += new EventHandler(this.btn_Close_Click);
            this.btn_Merge.AllowAnimations = true;
            this.btn_Merge.BackColor = Color.Transparent;
            this.btn_Merge.Location = new Point(394, 62);
            this.btn_Merge.Name = "VideoPlay_MergeV_52";
            this.btn_Merge.RoundedCornersMask = (byte)15;
            this.btn_Merge.RoundedCornersRadius = 0;
            this.btn_Merge.Size = new Size(130, 30);
            this.btn_Merge.TabIndex = 6;
            this.btn_Merge.Text = "VideoPlay_MergeV_53";
            this.btn_Merge.UseVisualStyleBackColor = false;
            this.btn_Merge.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Merge.Click += new EventHandler(this.btn_Merge_Click);
            this.txtName.BackColor = Color.White;
            this.txtName.BoundsOffset = new Size(1, 1);
            this.txtName.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtName.DefaultText = "";
            this.txtName.Location = new Point(159, 75);
            this.txtName.MaxLength = 64;
            this.txtName.Name = "VideoPlay_MergeV_54";
            this.txtName.PasswordChar = char.MinValue;
            this.txtName.ScrollBars = ScrollBars.None;
            this.txtName.SelectionLength = 0;
            this.txtName.SelectionStart = 0;
            this.txtName.Size = new Size(213, 23);
            this.txtName.TabIndex = 2;
            this.txtName.TextAlign = HorizontalAlignment.Left;
            this.txtName.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_VideoName.AutoSize = true;
            this.lbl_VideoName.Location = new Point(12, 80);
            this.lbl_VideoName.Name = "VideoPlay_MergeV_55";
            this.lbl_VideoName.Size = new Size(84, 13);
            this.lbl_VideoName.TabIndex = 1;
            this.lbl_VideoName.Text = "VideoPlay_MergeV_56";
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add((Control)this.lbl_MergeTags);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "VideoPlay_MergeV_57";
            this.HeaderPanel.Size = new Size(535, 40);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.lbl_MergeTags.AutoSize = true;
            this.lbl_MergeTags.Font = new Font("VideoPlay_MergeV_58", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lbl_MergeTags.ForeColor = Color.White;
            this.lbl_MergeTags.Location = new Point(11, 8);
            this.lbl_MergeTags.Name = "VideoPlay_MergeV_59";
            this.lbl_MergeTags.Size = new Size(202, 20);
            this.lbl_MergeTags.TabIndex = 0;
            this.lbl_MergeTags.Text = "VideoPlay_MergeV_60";
            this.lbl_MergeTags.MouseDown += new MouseEventHandler(this.lbl_MergeTags_MouseDown);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(537, 376);
            this.Controls.Add((Control)this.FormPanel);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Name = "VideoPlay_MergeV_61";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "VideoPlay_MergeV_62";
            this.Load += new EventHandler(this.MergeVideo_Load);
            this.FormPanel.ResumeLayout(false);
            this.FormPanel.PerformLayout();
            this.vListBox.ResumeLayout(false);
            ((ISupportInitialize)this.pictureBox1).EndInit();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        public delegate void DEL_StopPlayer();
    }
}
