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

        public List<VideoTag> TagList = new List<VideoTag>();

        private List<string> tFiles = new List<string>();

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

        public List<MediaFile> media
        {
            get;
            set;
        }

        public string VideoPath
        {
            get;
            set;
        }

        public MergeVideo()
        {
            this.InitializeComponent();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btn_Merge_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtName.Text))
            {
                MessageBox.Show(this, LangCtrl.GetString("mv_EnterFileName", "Please enter a file name."), "File Name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            this.ValidateFileName();
            this.pictureBox1.Visible = true;
            this.vListBox.Items.Insert(string.Format(LangCtrl.GetString("mv_Processing", "{0} - Processing file tags..."), DateTime.Now), 0);
            vButton btnClose = this.btn_Close;
            this.btn_Merge.Enabled = false;
            btnClose.Enabled = false;
            this.Callback();
            (new Thread(new ThreadStart(this.MergeFiles))).Start();
        }

        private void Callback()
        {
            if (this.EVT_StopPlayer != null)
            {
                this.EVT_StopPlayer();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void HeaderMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MergeVideo.ReleaseCapture();
                MergeVideo.SendMessage(base.Handle, 161, 2, 0);
            }
        }

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
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
            base.SuspendLayout();
            this.FormPanel.BorderStyle = BorderStyle.FixedSingle;
            this.FormPanel.Controls.Add(this.chk_SaveSegments);
            this.FormPanel.Controls.Add(this.btnMOV);
            this.FormPanel.Controls.Add(this.btnAVI);
            this.FormPanel.Controls.Add(this.btnMP4);
            this.FormPanel.Controls.Add(this.vListBox);
            this.FormPanel.Controls.Add(this.lblPath);
            this.FormPanel.Controls.Add(this.lbl_VideoFormat);
            this.FormPanel.Controls.Add(this.btn_Close);
            this.FormPanel.Controls.Add(this.btn_Merge);
            this.FormPanel.Controls.Add(this.txtName);
            this.FormPanel.Controls.Add(this.lbl_VideoName);
            this.FormPanel.Controls.Add(this.HeaderPanel);
            this.FormPanel.Dock = DockStyle.Fill;
            this.FormPanel.Location = new Point(0, 0);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new Size(537, 376);
            this.FormPanel.TabIndex = 0;
            this.chk_SaveSegments.BackColor = Color.Transparent;
            this.chk_SaveSegments.Location = new Point(159, 135);
            this.chk_SaveSegments.Name = "chk_SaveSegments";
            this.chk_SaveSegments.Size = new Size(213, 24);
            this.chk_SaveSegments.TabIndex = 15;
            this.chk_SaveSegments.Text = "Save Video Mark Segments";
            this.chk_SaveSegments.UseVisualStyleBackColor = false;
            this.chk_SaveSegments.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnMOV.BackColor = Color.Transparent;
            this.btnMOV.Flat = true;
            this.btnMOV.Image = null;
            this.btnMOV.Location = new Point(289, 104);
            this.btnMOV.Name = "btnMOV";
            this.btnMOV.Size = new Size(59, 24);
            this.btnMOV.TabIndex = 14;
            this.btnMOV.Text = "MOV";
            this.btnMOV.UseVisualStyleBackColor = false;
            this.btnMOV.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnAVI.BackColor = Color.Transparent;
            this.btnAVI.Flat = true;
            this.btnAVI.Image = null;
            this.btnAVI.Location = new Point(224, 104);
            this.btnAVI.Name = "btnAVI";
            this.btnAVI.Size = new Size(59, 24);
            this.btnAVI.TabIndex = 13;
            this.btnAVI.Text = "AVI";
            this.btnAVI.UseVisualStyleBackColor = false;
            this.btnAVI.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnMP4.BackColor = Color.Transparent;
            this.btnMP4.Checked = true;
            this.btnMP4.Flat = true;
            this.btnMP4.Image = null;
            this.btnMP4.Location = new Point(159, 104);
            this.btnMP4.Name = "btnMP4";
            this.btnMP4.Size = new Size(59, 24);
            this.btnMP4.TabIndex = 12;
            this.btnMP4.TabStop = true;
            this.btnMP4.Text = "MP4";
            this.btnMP4.UseVisualStyleBackColor = false;
            this.btnMP4.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.vListBox.Controls.Add(this.pictureBox1);
            this.vListBox.Location = new Point(15, 166);
            this.vListBox.Name = "vListBox";
            this.vListBox.RoundedCornersMaskListItem = 15;
            this.vListBox.SelectionMode = SelectionMode.None;
            this.vListBox.Size = new Size(509, 197);
            this.vListBox.TabIndex = 11;
            this.vListBox.VIBlendScrollBarsTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.vListBox.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.pictureBox1.Image = Properties.Resources.hbscreen_e0;
            this.pictureBox1.Location = new Point(450, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(42, 30);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            this.lblPath.AutoEllipsis = true;
            this.lblPath.Location = new Point(159, 53);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new Size(213, 13);
            this.lblPath.TabIndex = 10;
            this.lblPath.Text = "Video Path";
            this.lbl_VideoFormat.AutoSize = true;
            this.lbl_VideoFormat.Location = new Point(12, 110);
            this.lbl_VideoFormat.Name = "lbl_VideoFormat";
            this.lbl_VideoFormat.Size = new Size(69, 13);
            this.lbl_VideoFormat.TabIndex = 8;
            this.lbl_VideoFormat.Text = "Video Format";
            this.btn_Close.AllowAnimations = true;
            this.btn_Close.BackColor = Color.Transparent;
            this.btn_Close.Location = new Point(394, 98);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.RoundedCornersMask = 15;
            this.btn_Close.RoundedCornersRadius = 0;
            this.btn_Close.Size = new Size(130, 30);
            this.btn_Close.TabIndex = 7;
            this.btn_Close.Text = "Close";
            this.btn_Close.UseVisualStyleBackColor = false;
            this.btn_Close.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Close.Click += new EventHandler(this.btn_Close_Click);
            this.btn_Merge.AllowAnimations = true;
            this.btn_Merge.BackColor = Color.Transparent;
            this.btn_Merge.Location = new Point(394, 62);
            this.btn_Merge.Name = "btn_Merge";
            this.btn_Merge.RoundedCornersMask = 15;
            this.btn_Merge.RoundedCornersRadius = 0;
            this.btn_Merge.Size = new Size(130, 30);
            this.btn_Merge.TabIndex = 6;
            this.btn_Merge.Text = "Merge Video";
            this.btn_Merge.UseVisualStyleBackColor = false;
            this.btn_Merge.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Merge.Click += new EventHandler(this.btn_Merge_Click);
            this.txtName.BackColor = Color.White;
            this.txtName.BoundsOffset = new Size(1, 1);
            this.txtName.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtName.DefaultText = "";
            this.txtName.Location = new Point(159, 75);
            this.txtName.MaxLength = 64;
            this.txtName.Name = "txtName";
            this.txtName.PasswordChar = '\0';
            this.txtName.ScrollBars = ScrollBars.None;
            this.txtName.SelectionLength = 0;
            this.txtName.SelectionStart = 0;
            this.txtName.Size = new Size(213, 23);
            this.txtName.TabIndex = 2;
            this.txtName.TextAlign = HorizontalAlignment.Left;
            this.txtName.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_VideoName.AutoSize = true;
            this.lbl_VideoName.Location = new Point(12, 80);
            this.lbl_VideoName.Name = "lbl_VideoName";
            this.lbl_VideoName.Size = new Size(84, 13);
            this.lbl_VideoName.TabIndex = 1;
            this.lbl_VideoName.Text = "Video File Name";
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.lbl_MergeTags);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(535, 40);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.lbl_MergeTags.AutoSize = true;
            this.lbl_MergeTags.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_MergeTags.ForeColor = Color.White;
            this.lbl_MergeTags.Location = new Point(11, 8);
            this.lbl_MergeTags.Name = "lbl_MergeTags";
            this.lbl_MergeTags.Size = new Size(202, 20);
            this.lbl_MergeTags.TabIndex = 0;
            this.lbl_MergeTags.Text = "MERGE VIDEO MARKS";
            this.lbl_MergeTags.MouseDown += new MouseEventHandler(this.lbl_MergeTags_MouseDown);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(537, 376);
            base.Controls.Add(this.FormPanel);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "MergeVideo";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "MergeVideo";
            base.Load += new EventHandler(this.MergeVideo_Load);
            this.FormPanel.ResumeLayout(false);
            this.FormPanel.PerformLayout();
            this.vListBox.ResumeLayout(false);
            ((ISupportInitialize)this.pictureBox1).EndInit();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            base.ResumeLayout(false);
        }

        private void lbl_MergeTags_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void MergeFiles()
        {
            string empty = string.Empty;
            int num = 0;
            if (this.media.Count > 0)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Network.SetAcl(this.VideoPath);
                using (RPM_VideoTag rPMVideoTag = new RPM_VideoTag())
                {
                    foreach (MediaFile medium in this.media)
                    {
                        this.TagList = rPMVideoTag.GetTagList(medium.FileID);
                        foreach (VideoTag tagList in this.TagList)
                        {
                            TimeSpan timeSpan = new TimeSpan(0, 0, Convert.ToInt32(tagList.StartTime));
                            TimeSpan timeSpan1 = new TimeSpan(0, 0, Convert.ToInt32(tagList.EndTime));
                            string str = string.Format("{0:00}:{1:00}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
                            string str1 = string.Format("{0:00}:{1:00}:{2:D2}", timeSpan1.Hours, timeSpan1.Minutes, timeSpan1.Seconds);
                            TimeSpan timeSpan2 = timeSpan1 - timeSpan;
                            string str2 = string.Format("{0:00}:{1:00}:{2:D2}", timeSpan2.Hours, timeSpan2.Minutes, timeSpan2.Seconds);
                            ListItemsCollection items = this.vListBox.Items;
                            object[] shortDesc = new object[] { tagList.ShortDesc, str, str1, str2 };
                            items.Insert(string.Format("{0} {1}-{2}  Len: {3}", shortDesc), 0);
                            string str3 = string.Format("{0}{1}", num, Path.GetExtension(medium.FileName));
                            try
                            {
                                ProcessStartInfo processStartInfo = new ProcessStartInfo()
                                {
                                    UseShellExecute = false,
                                    CreateNoWindow = true,
                                    FileName = Path.Combine(Directory.GetCurrentDirectory(), "ffmpeg.exe")
                                };
                                processStartInfo.CreateNoWindow = true;
                                object[] fileName = new object[] { str, medium.FileName, str2, this.VideoPath, str3 };
                                processStartInfo.Arguments = string.Format("-v 0 -y -ss {0} -i \"{1}\" -t {2} -c:v copy -c:a copy \"{3}\\{4}\"", fileName);
                                Process.Start(processStartInfo).WaitForExit();
                            }
                            catch (Exception exception1)
                            {
                                Exception exception = exception1;
                                this.vListBox.Items.Insert(exception.Message, 0);
                                MessageBox.Show(exception.Message);
                            }
                            string str4 = Path.Combine(this.VideoPath, str3);
                            if (!File.Exists(str4))
                            {
                                this.vListBox.Items.Insert(string.Format(LangCtrl.GetString("mv_ErrorCreating", "Error creating: {0}"), str3), 0);
                            }
                            else
                            {
                                this.tFiles.Add(string.Format("{0}", str4));
                                num++;
                            }
                        }
                    }
                    string[] strArrays = new string[this.tFiles.Count];
                    int num1 = 0;
                    foreach (string tFile in this.tFiles)
                    {
                        if (!File.Exists(tFile))
                        {
                            continue;
                        }
                        strArrays[num1] = tFile;
                        num1++;
                    }
                    if ((int)strArrays.Length > 0)
                    {
                        string str5 = "MP4";
                        if (this.btnAVI.Checked)
                        {
                            str5 = "AVI";
                        }
                        if (this.btnMOV.Checked)
                        {
                            str5 = "MOV";
                        }
                        string str6 = string.Format("{0}\\{1}.{2}", this.VideoPath, this.txtName.Text, str5);
                        empty = str6;
                        this.vListBox.Items.Insert(LangCtrl.GetString("mv_Merging", "Merging Video Files...This may take a few minutes..."), 0);
                        Thread.Sleep(3000);
                        if ((int)strArrays.Length != 1)
                        {
                            try
                            {
                                ConcatSettings concatSetting = new ConcatSettings();
                                FFMpegConverter fFMpegConverter = new FFMpegConverter();
                                Thread.Sleep(1000);
                                fFMpegConverter.ConcatMedia(strArrays, str6, str5, concatSetting);
                            }
                            catch (Exception exception2)
                            {
                                this.vListBox.Items.Insert(exception2.Message, 0);
                            }
                        }
                        else
                        {
                            File.Move(strArrays[0], str6);
                        }
                    }
                    if (!this.chk_SaveSegments.Checked)
                    {
                        try
                        {
                            foreach (string tFile1 in this.tFiles)
                            {
                                if (!File.Exists(tFile1))
                                {
                                    continue;
                                }
                                File.Delete(tFile1);
                            }
                        }
                        catch (Exception exception3)
                        {
                            this.vListBox.Items.Insert(exception3.Message, 0);
                        }
                    }
                    stopwatch.Stop();
                    TimeSpan elapsed = stopwatch.Elapsed;
                    string str7 = string.Format("{0:00}:{1:00}:{2:D2}", elapsed.Hours, elapsed.Minutes, elapsed.Seconds);
                    this.vListBox.Items.Insert(string.Format(LangCtrl.GetString("mv_MergeComplete", "{0} - File Merge completed. Elapsed Time {1}"), DateTime.Now, str7), 0);
                    if (File.Exists(empty) && Global.IsRights(Global.RightsProfile, UserRights.REDACT) && Global.IsRedact)
                    {
                        base.BeginInvoke(new MethodInvoker(() => {
                            if (MessageBox.Show(this, LangCtrl.GetString("mv_RedactVideo", "Redact video file?"), "Redact", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                try
                                {
                                    Global.Log("REDACT", string.Format("Redact Video Merge: {0}", empty));
                                    Process.Start(new ProcessStartInfo()
                                    {
                                        FileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Redact/VideoEditor.exe"),
                                        Arguments = string.Concat("\"", empty, "\"")
                                    });
                                }
                                catch
                                {
                                }
                            }
                        }));
                    }
                }
                base.BeginInvoke(new MethodInvoker(() => {
                    this.btn_Close.Enabled = true;
                    this.pictureBox1.Visible = false;
                }));
            }
        }

        private void MergeVideo_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
            {
                this.HeaderPanel.BackgroundImage = Properties.Resources.topbar45;
            }
            LangCtrl.reText(this);
            this.lblPath.Text = this.VideoPath;
            this.vListBox.Items.Add(string.Format(LangCtrl.GetString("mv_FilesCount", "Files to process: {0}"), this.media.Count));
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public void ValidateFileName()
        {
            string text = this.txtName.Text;
            this.txtName.Text = text.Replace(' ', '\u005F');
        }

        public event MergeVideo.DEL_StopPlayer EVT_StopPlayer;

        public delegate void DEL_StopPlayer();
    }
}