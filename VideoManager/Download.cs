using AppGlobal;
using AssetMgr;
using VideoManager.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Unity;
using UploadCtrl;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMModels.Model;
using VMInterfaces;

namespace VideoManager
{
    public class Download : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;

        public const int HT_CAPTION = 2;

        private const int CS_DROPSHADOW = 131072;

        private string FolderPath = string.Empty;

        private bool IsCancel;

        private Stopwatch stopwatch;

        private DateTime StartTimestamp;

        private int FileCount;

        private string CameraDriveID = string.Empty;

        private IContainer components;

        private TableLayoutPanel tableLayoutPanel1;

        private Label lbl_DL_FileName;

        private Label lbl_DL_FileExt;

        private Label lbl_DL_Timestamp;

        private Label lbl_DL_FileSize;

        private Label lblFileName;

        private Label lblFileExt;

        private Label lblTimestamp;

        private Label lblFileSize;

        private Panel HeaderPanel;

        private PictureBox pictureBox1;

        private vButton btn_Cancel;

        private Label lblSourcePath;

        private Upload upload1;

        private Label lblTimespan;

        private System.Windows.Forms.Timer timer1;

        private Label lbl_DLTime;

        public Guid Account_ID
        {
            get;
            set;
        }

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

        public Download(string path, Guid accountid)
        {
            this.InitializeComponent();
            this.stopwatch = new Stopwatch();
            this.stopwatch.Start();
            this.StartTimestamp = DateTime.Now;
            try
            {
                Guid accountID = (new Assets()).GetAccountID(Path.GetPathRoot(path));
                if (accountID == Guid.Empty)
                {
                    this.Account_ID = accountid;
                }
                else
                {
                    this.Account_ID = accountID;
                }
                this.FolderPath = path;
            }
            catch
            {
            }
        }

        public Download()
        {
            this.InitializeComponent();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, LangCtrl.GetString("txt_CancelUpload", "Cancel Upload?"), "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    this.upload1.CancelFlag = true;
                    this.IsCancel = true;
                    Thread.Sleep(3000);
                    base.Close();
                }
                catch
                {
                }
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

        private void Download_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.upload1.EVT_UploadComplete -= new Upload.DEL_UploadComplete(this.upload1_EVT_UploadComplete);
            this.upload1.EVT_UploadCallback -= new Upload.DEL_UploadCallback(this.upload1_EVT_UploadCallback);
        }

        private void Download_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
            {
                this.btn_Cancel.VIBlendTheme = VIBLEND_THEME.NERO;
                this.HeaderPanel.BackgroundImage = Properties.Resources.topbar45;
            }
            this.SetLanguage();
            this.lblSourcePath.Text = this.FolderPath;
            this.Text = string.Format(LangCtrl.GetString("txt_Download", "Download {0}"), this.FolderPath);
            this.Text = string.Format("Download {0}", this.FolderPath);
            this.upload1.EVT_UploadComplete -= new Upload.DEL_UploadComplete(this.upload1_EVT_UploadComplete);
            this.upload1.EVT_UploadCallback -= new Upload.DEL_UploadCallback(this.upload1_EVT_UploadCallback);
            this.upload1.EVT_UploadComplete += new Upload.DEL_UploadComplete(this.upload1_EVT_UploadComplete);
            this.upload1.EVT_UploadCallback += new Upload.DEL_UploadCallback(this.upload1_EVT_UploadCallback);
            this.upload1.DeleteSource = true;
            this.upload1.ShowTimestamp = true;
            DateTime now = DateTime.Now;
            object[] accountID = new object[] { this.Account_ID, now.Year, now.Month, now.Day };
            string str = string.Format("{0}\\{1}\\{2:00}\\{3:00}", accountID);
            string str1 = Path.Combine(Global.UNCServer, Global.RelativePath);
            str1 = Path.Combine(str1, str);
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
            bool flag = false;
            try
            {
                if (!Directory.Exists(str1))
                {
                    Directory.CreateDirectory(str1);
                    Network.SetAcl(str1);
                }
            }
            catch (Exception exception)
            {
                flag = true;
                MessageBox.Show(this, string.Format(LangCtrl.GetString("txt_StorageErr", "The storage path is not available!\nPlease check your hard drive or network."), this.FolderPath), "Storage", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                base.Close();
            }
            if (!flag)
            {
                if (Directory.Exists(str1))
                {
                    this.CameraDriveID = Path.GetPathRoot(this.FolderPath);
                    Thread.Sleep(1000);
                    if (!this.upload1.StartUpload(this.FolderPath, Global.UNCServer, Global.RelativePath, str, this.Account_ID))
                    {
                        MessageBox.Show(string.Format(LangCtrl.GetString("txt_StorageErr2", "Storage Path not available at {0}"), this.FolderPath));
                    }
                    this.FileCount = this.upload1.FileCount;
                    return;
                }
                MessageBox.Show(string.Format(LangCtrl.GetString("txt_StorageErr2", "Storage Path not available at {0}"), this.FolderPath));
                base.Close();
            }
        }

        private void Download_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder3D(e.Graphics, ((Control)sender).ClientRectangle, Border3DStyle.RaisedOuter);
        }

        private void HeaderMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Download.ReleaseCapture();
                Download.SendMessage(base.Handle, 161, 2, 0);
            }
        }

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.lbl_DL_FileName = new Label();
            this.lbl_DL_FileExt = new Label();
            this.lbl_DL_Timestamp = new Label();
            this.lbl_DL_FileSize = new Label();
            this.lblFileName = new Label();
            this.lblFileExt = new Label();
            this.lblTimestamp = new Label();
            this.lblFileSize = new Label();
            this.lblTimespan = new Label();
            this.lbl_DLTime = new Label();
            this.HeaderPanel = new Panel();
            this.btn_Cancel = new vButton();
            this.lblSourcePath = new Label();
            this.pictureBox1 = new PictureBox();
            this.upload1 = new Upload();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            base.SuspendLayout();
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36.13445f));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 63.86555f));
            this.tableLayoutPanel1.Controls.Add(this.lbl_DL_FileName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_DL_FileExt, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbl_DL_Timestamp, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbl_DL_FileSize, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblFileName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblFileExt, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblTimestamp, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblFileSize, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblTimespan, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lbl_DLTime, 0, 4);
            this.tableLayoutPanel1.Location = new Point(12, 123);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
            this.tableLayoutPanel1.Size = new Size(357, 96);
            this.tableLayoutPanel1.TabIndex = 2;
            this.lbl_DL_FileName.AutoSize = true;
            this.lbl_DL_FileName.Dock = DockStyle.Right;
            this.lbl_DL_FileName.Location = new Point(71, 0);
            this.lbl_DL_FileName.Name = "lbl_DL_FileName";
            this.lbl_DL_FileName.Size = new Size(54, 19);
            this.lbl_DL_FileName.TabIndex = 0;
            this.lbl_DL_FileName.Text = "File Name";
            this.lbl_DL_FileName.TextAlign = ContentAlignment.MiddleRight;
            this.lbl_DL_FileExt.AutoSize = true;
            this.lbl_DL_FileExt.Dock = DockStyle.Right;
            this.lbl_DL_FileExt.Location = new Point(84, 19);
            this.lbl_DL_FileExt.Name = "lbl_DL_FileExt";
            this.lbl_DL_FileExt.Size = new Size(41, 19);
            this.lbl_DL_FileExt.TabIndex = 1;
            this.lbl_DL_FileExt.Text = "File Ext";
            this.lbl_DL_FileExt.TextAlign = ContentAlignment.MiddleRight;
            this.lbl_DL_Timestamp.AutoSize = true;
            this.lbl_DL_Timestamp.Dock = DockStyle.Right;
            this.lbl_DL_Timestamp.Location = new Point(67, 38);
            this.lbl_DL_Timestamp.Name = "lbl_DL_Timestamp";
            this.lbl_DL_Timestamp.Size = new Size(58, 19);
            this.lbl_DL_Timestamp.TabIndex = 2;
            this.lbl_DL_Timestamp.Text = "Timestamp";
            this.lbl_DL_Timestamp.TextAlign = ContentAlignment.MiddleRight;
            this.lbl_DL_FileSize.AutoSize = true;
            this.lbl_DL_FileSize.Dock = DockStyle.Right;
            this.lbl_DL_FileSize.Location = new Point(79, 57);
            this.lbl_DL_FileSize.Name = "lbl_DL_FileSize";
            this.lbl_DL_FileSize.Size = new Size(46, 19);
            this.lbl_DL_FileSize.TabIndex = 3;
            this.lbl_DL_FileSize.Text = "File Size";
            this.lbl_DL_FileSize.TextAlign = ContentAlignment.MiddleRight;
            this.lblFileName.Dock = DockStyle.Fill;
            this.lblFileName.Location = new Point(131, 0);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new Size(223, 19);
            this.lblFileName.TabIndex = 5;
            this.lblFileName.TextAlign = ContentAlignment.MiddleLeft;
            this.lblFileExt.AutoSize = true;
            this.lblFileExt.Dock = DockStyle.Fill;
            this.lblFileExt.Location = new Point(131, 19);
            this.lblFileExt.Name = "lblFileExt";
            this.lblFileExt.Size = new Size(223, 19);
            this.lblFileExt.TabIndex = 6;
            this.lblFileExt.TextAlign = ContentAlignment.MiddleLeft;
            this.lblTimestamp.AutoSize = true;
            this.lblTimestamp.Dock = DockStyle.Fill;
            this.lblTimestamp.Location = new Point(131, 38);
            this.lblTimestamp.Name = "lblTimestamp";
            this.lblTimestamp.Size = new Size(223, 19);
            this.lblTimestamp.TabIndex = 7;
            this.lblTimestamp.TextAlign = ContentAlignment.MiddleLeft;
            this.lblFileSize.AutoSize = true;
            this.lblFileSize.Dock = DockStyle.Fill;
            this.lblFileSize.Location = new Point(131, 57);
            this.lblFileSize.Name = "lblFileSize";
            this.lblFileSize.Size = new Size(223, 19);
            this.lblFileSize.TabIndex = 8;
            this.lblFileSize.TextAlign = ContentAlignment.MiddleLeft;
            this.lblTimespan.AutoSize = true;
            this.lblTimespan.Dock = DockStyle.Fill;
            this.lblTimespan.Location = new Point(131, 76);
            this.lblTimespan.Name = "lblTimespan";
            this.lblTimespan.Size = new Size(223, 20);
            this.lblTimespan.TabIndex = 9;
            this.lblTimespan.Text = "00:00:00.0000";
            this.lblTimespan.TextAlign = ContentAlignment.MiddleLeft;
            this.lbl_DLTime.AutoSize = true;
            this.lbl_DLTime.Dock = DockStyle.Fill;
            this.lbl_DLTime.Location = new Point(3, 76);
            this.lbl_DLTime.Name = "lbl_DLTime";
            this.lbl_DLTime.Size = new Size(122, 20);
            this.lbl_DLTime.TabIndex = 10;
            this.lbl_DLTime.Text = "Time";
            this.lbl_DLTime.TextAlign = ContentAlignment.MiddleRight;
            this.HeaderPanel.BackgroundImage = Properties.Resources.header;
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.btn_Cancel);
            this.HeaderPanel.Controls.Add(this.lblSourcePath);
            this.HeaderPanel.Controls.Add(this.pictureBox1);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(381, 40);
            this.HeaderPanel.TabIndex = 3;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.btn_Cancel.AllowAnimations = true;
            this.btn_Cancel.BackColor = Color.Transparent;
            this.btn_Cancel.Dock = DockStyle.Right;
            this.btn_Cancel.Location = new Point(262, 0);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.RoundedCornersMask = 15;
            this.btn_Cancel.RoundedCornersRadius = 0;
            this.btn_Cancel.Size = new Size(119, 40);
            this.btn_Cancel.TabIndex = 2;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Cancel.Click += new EventHandler(this.btn_Cancel_Click);
            this.lblSourcePath.AutoSize = true;
            this.lblSourcePath.BackColor = Color.Transparent;
            this.lblSourcePath.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblSourcePath.ForeColor = Color.White;
            this.lblSourcePath.Location = new Point(53, 3);
            this.lblSourcePath.Name = "lblSourcePath";
            this.lblSourcePath.Size = new Size(66, 12);
            this.lblSourcePath.TabIndex = 1;
            this.lblSourcePath.Text = "Source Path";
            this.lblSourcePath.MouseDown += new MouseEventHandler(this.lblSourcePath_MouseDown);
            this.pictureBox1.BackColor = Color.Transparent;
            this.pictureBox1.Image = Properties.Resources.usb;
            this.pictureBox1.Location = new Point(12, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(34, 34);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new MouseEventHandler(this.pictureBox1_MouseDown);
            this.upload1.BackColor = Color.Transparent;
            this.upload1.DeleteSource = false;
            this.upload1.Filter = null;
            this.upload1.Location = new Point(12, 43);
            this.upload1.Margin = new Padding(0);
            this.upload1.Name = "upload1";
            this.upload1.ShowTimestamp = false;
            this.upload1.Size = new Size(357, 77);
            this.upload1.TabIndex = 4;
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(381, 222);
            base.Controls.Add(this.upload1);
            base.Controls.Add(this.HeaderPanel);
            base.Controls.Add(this.tableLayoutPanel1);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "Download";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Download";
            base.FormClosing += new FormClosingEventHandler(this.Download_FormClosing);
            base.Load += new EventHandler(this.Download_Load);
            base.Paint += new PaintEventHandler(this.Download_Paint);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((ISupportInitialize)this.pictureBox1).EndInit();
            base.ResumeLayout(false);
        }

        private void lblSourcePath_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private CamProfile ReadAssetProfile(string DriveID)
        {
            CamProfile camProfile = new CamProfile();
            if (File.Exists(Path.Combine(DriveID, "C3Sentinel.Dat")))
            {
                camProfile = (CamProfile)FileCrypto.LoadConfig(Path.Combine(DriveID, "C3Sentinel.Dat"));
            }
            return camProfile;
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void SetLanguage()
        {
            LangCtrl.reText(this);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Label label = this.lblTimespan;
            object hours = this.stopwatch.Elapsed.Hours;
            object minutes = this.stopwatch.Elapsed.Minutes;
            TimeSpan elapsed = this.stopwatch.Elapsed;
            label.Text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, elapsed.Seconds);
        }

        private void upload1_EVT_UploadCallback(object sender, CmdEventArgs e)
        {
            base.BeginInvoke(new MethodInvoker(() => {
                this.lblFileName.Text = e.Record.FileName;
                this.lblFileExt.Text = e.Record.FileExt;
                this.lblTimestamp.Text = string.Format("{0}", e.Record.FileTimestamp);
                this.lblFileSize.Text = string.Format("{0}", e.Record.FileSize);
            }));
        }

        private void upload1_EVT_UploadComplete()
        {
            this.timer1.Stop();
            this.stopwatch.Stop();
            this.lblFileName.Text = string.Empty;
            this.lblFileExt.Text = string.Empty;
            this.lblTimestamp.Text = string.Empty;
            this.lblFileSize.Text = string.Empty;
            try
            {
                if (!this.IsCancel)
                {
                    if (!this.upload1.CancelFlag)
                    {
                        MessageBox.Show(this, LangCtrl.GetString("txt_UploadComplete", "Upload Complete."), "UPLOAD", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        MessageBox.Show(this, LangCtrl.GetString("txt_UploadStopped", "Upload Stopped."), "UPLOAD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            catch
            {
            }
            try
            {
                CameraLog cameraLog = new CameraLog()
                {
                    AssetTag = "",
                    LogTimestamp = new DateTime?(this.StartTimestamp),
                    FileCount = this.upload1.FileCount,
                    Action = "CAMERA UPLOAD"
                };
                if (string.IsNullOrEmpty(Global.Camera_SerialNum))
                {
                    Global.Camera_SerialNum = Global.VisionSN;
                }
                cameraLog.SerialNumber = Global.Camera_SerialNum.TrimEnd(new char[0]);
                cameraLog.Battery = Global.Camera_Battery;
                cameraLog.DiskSpace = (double)Global.Camera_Disk;
                object[] text = new object[] { this.lblTimespan.Text, Environment.NewLine, Environment.NewLine, Environment.NewLine };
                cameraLog.Memo = string.Format("Elapsed Download Time: {0}{1}{2}CAMERA CONFIGURATION -------------------------------------{3}", text);
                CameraLog cameraLog1 = cameraLog;
                cameraLog1.Memo = string.Concat(cameraLog1.Memo, CiteCamera.CameraProfile(this.CameraDriveID));
                CameraLog cameraLog2 = cameraLog;
                cameraLog2.Memo = string.Concat(cameraLog2.Memo, string.Format("{0}CAMERA TRANSACTIONS -------------------------------------{1}", Environment.NewLine, Environment.NewLine));
                CameraLog cameraLog3 = cameraLog;
                cameraLog3.Memo = string.Concat(cameraLog3.Memo, CiteCamera.DailyLog(this.CameraDriveID));
                cameraLog.AccountID = Global.GlobalAccount.Id;
                cameraLog.AccountName = Global.GlobalAccount.ToString();
                cameraLog.BadgeNumber = Global.GlobalAccount.BadgeNumber;
                CamProfile camProfile = this.ReadAssetProfile(this.CameraDriveID);
                if (camProfile != null && camProfile.InventoryID != Guid.Empty)
                {
                    try
                    {
                        cameraLog.AssetTag = camProfile.AssetTag;
                        if (!string.IsNullOrEmpty(camProfile.TrackingID))
                        {
                            Guid empty = Guid.Empty;
                            using (RPM_Asset rPMAsset = new RPM_Asset())
                            {
                                empty = rPMAsset.GetAccountByTrackingID(camProfile.TrackingID);
                            }
                            if (empty != Guid.Empty)
                            {
                                using (RPM_Account rPMAccount = new RPM_Account())
                                {
                                    Account account = rPMAccount.GetAccount(empty);
                                    cameraLog.AccountID = account.Id;
                                    cameraLog.AccountName = account.ToString();
                                    cameraLog.BadgeNumber = account.BadgeNumber;
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                Global.Log(cameraLog);
            }
            catch
            {
            }
            this.upload1.ClearControl();
            base.Close();
        }
    }
}