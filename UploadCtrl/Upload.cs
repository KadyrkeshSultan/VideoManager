using AppGlobal;
using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Unity;
using UploadCtrl.Properties;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMInterfaces;
using VMModels.Enums;
using VMModels.Model;

namespace UploadCtrl
{
    public class Upload : UserControl, IDisposable
    {
        private bool IsRawName;

        private Guid AccountID = Guid.Empty;

        private string MachineName = Environment.MachineName;

        private string MachineAccount = Environment.UserName;

        private string MachineDomain = Environment.UserDomainName;

        public int FileCount;

        private WebClient webClient = new WebClient();

        private string SourcePath = string.Empty;

        private string Server = string.Empty;

        private string TargetPath = string.Empty;

        private string RelativePath = string.Empty;

        private bool IsRunning;

        private string[] fileEntries;

        private int i;

        private string TimeStamp = string.Empty;

        private Dictionary<string, bool> FileList = new Dictionary<string, bool>();

        public bool CancelFlag;

        private string[] FileNames;

        private string DataPath = string.Empty;

        private string UNCRoot = string.Empty;

        private string SourceFileName = string.Empty;

        private string TargetFileName = string.Empty;

        private IContainer components;

        private TableLayoutPanel tableLayoutPanel1;

        private PictureBox picBox;

        private vProgressBar progFile;

        private Label lblStatus;

        private vProgressBar progBar;

        private Label lblFileName;

        private Label lblFileCount;

        public bool DeleteSource
        {
            get;
            set;
        }

        public string Filter
        {
            get;
            set;
        }

        private string SETID
        {
            get;
            set;
        }

        public bool ShowTimestamp
        {
            get;
            set;
        }

        public Upload()
        {
            this.InitializeComponent();
        }

        private void Callback(object sender, CmdEventArgs e)
        {
            if (this.EVT_UploadCallback != null)
            {
                this.EVT_UploadCallback(this, e);
            }
        }

        public void ClearControl()
        {
            this.ShowTimestamp = false;
            this.progFile.Value = 0;
            this.progBar.Value = 0;
            this.picBox.Image = null;
            this.lblFileCount.Text = "0";
            this.lblFileName.Text = string.Empty;
            this.lblStatus.Text = "Ready...";
        }

        public void CopyFiles(string[] sourceFiles, string server, string relativePath, Guid AcctId, string SetID, bool RawName)
        {
            this.IsRawName = RawName;
            this.SETID = SetID;
            this.AccountID = AcctId;
            this.FileNames = sourceFiles;
            this.Server = server;
            try
            {
                string str = Path.Combine(server, relativePath);
                str = Network.FormatPath(Path.Combine(server, relativePath));
                this.UNCRoot = str;
                this.TargetPath = str;
                if (!this.TargetPath.Contains(":") && !this.TargetPath.Substring(0, 2).Equals("\\\\"))
                {
                    this.TargetPath = string.Concat("\\\\", this.TargetPath);
                }
                this.TargetPath = this.method_0(this.TargetPath);
                DateTime now = DateTime.Now;
                string str1 = this.AccountID.ToString();
                object[] year = new object[] { str1, now.Year, now.Month, now.Day };
                this.DataPath = string.Format("{0}\\{1}\\{2:00}\\{3:00}", year);
                this.TargetPath = Path.Combine(this.TargetPath, this.DataPath);
                if (!Directory.Exists(this.TargetPath))
                {
                    Directory.CreateDirectory(this.TargetPath);
                    Network.SetAcl(this.TargetPath);
                }
                this.RelativePath = relativePath;
                if (!Directory.Exists(this.TargetPath))
                {
                    this.CancelFlag = true;
                }
                else
                {
                    (new Thread(new ThreadStart(this.RunUpload))).Start();
                }
            }
            catch (Exception exception)
            {
            }
        }

        public void CopyFileWithProgress(string source, string destination)
        {
            this.SourceFileName = source;
            this.TargetFileName = destination;
            try
            {
                this.webClient = new WebClient();
                this.webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(this.webClient_DownloadFileCompleted);
                this.webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.DownloadProgress);
                this.IsRunning = true;
                this.webClient.DownloadFileAsync(new Uri(source), destination);
                do
                {
                    if (!this.IsRunning)
                    {
                        break;
                    }
                    Thread.Yield();
                }
                while (!this.CancelFlag);
                this.webClient.DownloadFileCompleted -= new AsyncCompletedEventHandler(this.webClient_DownloadFileCompleted);
                this.webClient.DownloadProgressChanged -= new DownloadProgressChangedEventHandler(this.DownloadProgress);
                this.webClient.Dispose();
                if (this.DeleteSource && !string.IsNullOrEmpty(this.TargetFileName) && File.Exists(this.TargetFileName))
                {
                    File.GetAttributes(this.SourceFileName);
                    File.SetAttributes(this.SourceFileName, FileAttributes.Normal);
                    File.Delete(this.SourceFileName);
                    this.SourceFileName = string.Empty;
                    this.TargetFileName = string.Empty;
                }
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                this.CancelFlag = true;
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

        private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            try
            {
                base.BeginInvoke(new MethodInvoker(() => {
                    this.progFile.Value = e.ProgressPercentage;
                    this.progFile.Invalidate();
                }));
                if (this.FileCopyProgress != null)
                {
                    this.FileCopyProgress(e.ProgressPercentage);
                }
            }
            catch (Exception exception)
            {
            }
        }

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.progBar = new vProgressBar();
            this.lblFileCount = new Label();
            this.picBox = new PictureBox();
            this.lblStatus = new Label();
            this.progFile = new vProgressBar();
            this.lblFileName = new Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.progBar.SuspendLayout();
            ((ISupportInitialize)this.picBox).BeginInit();
            this.progFile.SuspendLayout();
            base.SuspendLayout();
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100f));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.Controls.Add(this.progBar, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.picBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblStatus, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.progFile, 1, 2);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
            this.tableLayoutPanel1.Size = new Size(304, 77);
            this.tableLayoutPanel1.TabIndex = 0;
            this.progBar.BackColor = Color.Transparent;
            this.progBar.Controls.Add(this.lblFileCount);
            this.progBar.Dock = DockStyle.Fill;
            this.progBar.Location = new Point(101, 26);
            this.progBar.Margin = new Padding(1);
            this.progBar.Name = "progBar";
            this.progBar.RoundedCornersMask = 15;
            this.progBar.RoundedCornersRadius = 0;
            this.progBar.Size = new Size(202, 23);
            this.progBar.TabIndex = 3;
            this.progBar.Value = 0;
            this.progBar.VIBlendTheme = VIBLEND_THEME.OFFICESILVER;
            this.lblFileCount.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.lblFileCount.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblFileCount.Location = new Point(0, 0);
            this.lblFileCount.Name = "lblFileCount";
            this.lblFileCount.Size = new Size(203, 23);
            this.lblFileCount.TabIndex = 0;
            this.lblFileCount.Text = "0";
            this.lblFileCount.TextAlign = ContentAlignment.MiddleCenter;
            this.picBox.BackColor = Color.Transparent;
            this.picBox.BorderStyle = BorderStyle.FixedSingle;
            this.picBox.Dock = DockStyle.Fill;
            this.picBox.Location = new Point(0, 0);
            this.picBox.Margin = new Padding(0);
            this.picBox.Name = "picBox";
            this.tableLayoutPanel1.SetRowSpan(this.picBox, 3);
            this.picBox.Size = new Size(100, 77);
            this.picBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.picBox.TabIndex = 0;
            this.picBox.TabStop = false;
            this.picBox.Paint += new PaintEventHandler(this.picBox_Paint);
            this.lblStatus.AutoEllipsis = true;
            this.lblStatus.AutoSize = true;
            this.lblStatus.Dock = DockStyle.Fill;
            this.lblStatus.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblStatus.Location = new Point(103, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(198, 25);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Ready...";
            this.lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            this.progFile.BackColor = Color.Transparent;
            this.progFile.Controls.Add(this.lblFileName);
            this.progFile.Dock = DockStyle.Fill;
            this.progFile.Location = new Point(101, 51);
            this.progFile.Margin = new Padding(1);
            this.progFile.Name = "progFile";
            this.progFile.RoundedCornersMask = 15;
            this.progFile.RoundedCornersRadius = 0;
            this.progFile.Size = new Size(202, 25);
            this.progFile.TabIndex = 2;
            this.progFile.Value = 0;
            this.progFile.VIBlendTheme = VIBLEND_THEME.OFFICESILVER;
            this.lblFileName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.lblFileName.AutoEllipsis = true;
            this.lblFileName.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblFileName.Location = new Point(1, -1);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new Size(202, 26);
            this.lblFileName.TabIndex = 0;
            this.lblFileName.Text = "File";
            this.lblFileName.TextAlign = ContentAlignment.MiddleCenter;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Transparent;
            base.Controls.Add(this.tableLayoutPanel1);
            base.Margin = new Padding(0);
            base.Name = "Upload";
            base.Size = new Size(304, 77);
            base.Load += new EventHandler(this.Upload_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.progBar.ResumeLayout(false);
            ((ISupportInitialize)this.picBox).EndInit();
            this.progFile.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public string method_0(string originalPath)
        {
            StringBuilder stringBuilder = new StringBuilder(512);
            int capacity = stringBuilder.Capacity;
            if (originalPath.Length > 2 && originalPath[1] == ':')
            {
                char chr = originalPath[0];
                if ((chr >= 'a' && chr <= 'z' || chr >= 'A' && chr <= 'Z') && Upload.WNetGetConnection(originalPath.Substring(0, 2), stringBuilder, ref capacity) == 0)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(originalPath);
                    string str = Path.GetFullPath(originalPath).Substring(Path.GetPathRoot(originalPath).Length);
                    return Path.Combine(stringBuilder.ToString().TrimEnd(new char[0]), str);
                }
            }
            return originalPath;
        }

        private void picBox_Paint(object sender, PaintEventArgs e)
        {
            if (this.ShowTimestamp)
            {
                e.Graphics.DrawString(this.TimeStamp, new Font("Verdana", 5f), Brushes.Yellow, 4f, (float)(this.picBox.Height - 10));
            }
        }

        private void RunUpload()
        {
            this.Filter = string.Empty;
            using (RPM_FileExt rPMFileExt = new RPM_FileExt())
            {
                List<FileExt> fileExtensions = rPMFileExt.GetFileExtensions();
                int num2 = 0;
                if (fileExtensions.Count <= 0)
                {
                    this.Filter = ".DAT,.GPS,.EXIF";
                }
                else
                {
                    foreach (FileExt fileExtension in fileExtensions)
                    {
                        if (num2 <= 0)
                        {
                            Upload upload1 = this;
                            upload1.Filter = string.Concat(upload1.Filter, fileExtension.Ext);
                        }
                        else
                        {
                            Upload upload2 = this;
                            upload2.Filter = string.Concat(upload2.Filter, ",", fileExtension.Ext);
                        }
                        num2++;
                    }
                }
                if (!this.Filter.Contains(".GPS"))
                {
                    Upload upload3 = this;
                    upload3.Filter = string.Concat(upload3.Filter, ",.GPS");
                }
                if (!this.Filter.Contains(".DAT"))
                {
                    Upload upload4 = this;
                    upload4.Filter = string.Concat(upload4.Filter, ",.DAT");
                }
                if (!this.Filter.Contains(".EXIF"))
                {
                    Upload upload5 = this;
                    upload5.Filter = string.Concat(upload5.Filter, ",.EXIF");
                }
            }
            if (this.FileNames == null)
            {
                if (string.IsNullOrEmpty(this.SourcePath))
                {
                    return;
                }
                this.fileEntries = Directory.GetFiles(this.SourcePath);
            }
            else if ((int)this.FileNames.Length > 0)
            {
                this.fileEntries = this.FileNames;
            }
            this.FileList.Clear();
            string[] strArrays = this.Filter.Split(new char[] { ',' });
            string[] strArrays1 = this.fileEntries;
            for (int i = 0; i < (int)strArrays1.Length; i++)
            {
                string str = strArrays1[i];
                bool flag = false;
                string upper = Path.GetExtension(str).ToUpper();
                if ((int)strArrays.Length > 0)
                {
                    string[] strArrays2 = strArrays;
                    int num3 = 0;
                    while (num3 < (int)strArrays2.Length)
                    {
                        string str1 = strArrays2[num3];
                        if (string.IsNullOrEmpty(str1) || !upper.Equals(str1.ToUpper()))
                        {
                            num3++;
                        }
                        else
                        {
                            flag = true;
                            goto Label0;
                        }
                    }
                }
            Label0:
                if (!flag)
                {
                    this.FileList.Add(str, false);
                }
            }
            HashAlgorithm sHA1 = HashAlgorithms.SHA1;
            switch (Global.DefaultHashAlgorithm)
            {
                case HASH_ALGORITHM.MD5:
                    {
                        sHA1 = HashAlgorithms.MD5;
                        break;
                    }
                case HASH_ALGORITHM.SHA1:
                    {
                        sHA1 = HashAlgorithms.SHA1;
                        break;
                    }
                case HASH_ALGORITHM.SHA256:
                    {
                        sHA1 = HashAlgorithms.SHA256;
                        break;
                    }
                case HASH_ALGORITHM.SHA384:
                    {
                        sHA1 = HashAlgorithms.SHA384;
                        break;
                    }
                case HASH_ALGORITHM.SHA512:
                    {
                        sHA1 = HashAlgorithms.SHA512;
                        break;
                    }
                case HASH_ALGORITHM.RIPEMD160:
                    {
                        sHA1 = HashAlgorithms.RIPEMD160;
                        break;
                    }
            }
            this.progBar.Maximum = this.FileList.Count;
            Image thumbnailImage = Resources.folder;
            this.i = 0;
            foreach (KeyValuePair<string, bool> fileList in this.FileList)
            {
                string key = fileList.Key;
                FileInfo fileInfo = new FileInfo(key);
                base.BeginInvoke(new MethodInvoker(() => {
                    try
                    {
                        vProgressBar u003cu003e4_this = this.progBar;
                        Upload upload = this;
                        int num = upload.i + 1;
                        int num1 = num;
                        upload.i = num;
                        u003cu003e4_this.Value = num1;
                        this.lblStatus.Text = "Uploading...";
                        this.lblFileCount.Text = string.Format("{0} of {1}", this.i, this.FileList.Count);
                        this.lblFileName.Text = string.Format("{0} • {1}", Path.GetFileName(fileList.Key), Utilities.BytesToString(fileInfo.Length));
                    }
                    catch (Exception exception)
                    {
                        this.CancelFlag = true;
                    }
                }));
                if (!File.Exists(key))
                {
                    continue;
                }
                FileData fileDatum = new FileData();
                try
                {
                    string extension = Path.GetExtension(key);
                    if (!this.IsRawName)
                    {
                        fileDatum.FileName = Guid.NewGuid().ToString();
                    }
                    else
                    {
                        fileDatum.FileName = Path.GetFileNameWithoutExtension(key);
                    }
                    fileDatum.OriginalFileName = Path.GetFileNameWithoutExtension(key);
                    fileDatum.FileExt = extension;
                    fileDatum.FileHashCode = "Pending...";
                    fileDatum.FileSize = fileInfo.Length;
                    fileDatum.FileTimestamp = fileInfo.LastWriteTime;
                    this.TimeStamp = fileDatum.FileTimestamp.ToString();
                    fileDatum.Server = this.Server;
                    fileDatum.RelativePath = this.RelativePath;
                    fileDatum.IsDat = fileList.Value;
                    MemoryStream memoryStream = new MemoryStream();
                    (new FFMpegConverter()).GetVideoThumbnail(key, memoryStream);
                    Image image = Image.FromStream(memoryStream);
                    thumbnailImage = image.GetThumbnailImage(220, 130, null, IntPtr.Zero);
                    this.picBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.picBox.Image = (Image)thumbnailImage.Clone();
                    fileDatum.Thumbnail = (Image)thumbnailImage.Clone();
                }
                catch (Exception exception1)
                {
                    string upper1 = Path.GetExtension(key).ToUpper();
                    string str2 = upper1;
                    if (upper1 != null)
                    {
                        switch (str2)
                        {
                            case ".WAV":
                                {
                                    fileDatum.Thumbnail = Utilities.resizeImage(48, 48, Resources.audio);
                                    this.picBox.SizeMode = PictureBoxSizeMode.CenterImage;
                                    this.picBox.Image = fileDatum.Thumbnail;
                                    goto Label1;
                                }
                            case ".JPG":
                            case ".BMP":
                            case ".GIF":
                            case ".TIF":
                            case ".PNG":
                                {
                                    Image image1 = Image.FromFile(key);
                                    fileDatum.Thumbnail = image1.GetThumbnailImage(220, 130, null, IntPtr.Zero);
                                    this.picBox.SizeMode = PictureBoxSizeMode.StretchImage;
                                    this.picBox.Image = fileDatum.Thumbnail;
                                    goto Label1;
                                }
                        }
                    }
                    Icon winLogo = SystemIcons.WinLogo;
                    fileDatum.Thumbnail = Utilities.resizeImage(48, 48, Icon.ExtractAssociatedIcon(key).ToBitmap());
                    this.picBox.SizeMode = PictureBoxSizeMode.CenterImage;
                    this.picBox.Image = fileDatum.Thumbnail;
                //TODO : goto
                Label1:;
                }
                this.picBox.Invalidate();
                string str3 = Path.Combine(this.TargetPath, string.Concat(fileDatum.FileName, fileDatum.FileExt));
                this.Callback(this, new CmdEventArgs(fileDatum));
                string str4 = str3;
                try
                {
                    if (!File.Exists(str3))
                    {
                        this.progFile.Value = 0;
                        this.progFile.Text = string.Empty;
                        this.FileCount++;
                        this.CopyFileWithProgress(key, str3);
                        string empty = string.Empty;
                        string[] strArrays3 = strArrays;
                        for (int j = 0; j < (int)strArrays3.Length; j++)
                        {
                            string str5 = strArrays3[j];
                            string str6 = Path.Combine(Path.GetDirectoryName(key), string.Concat(Path.GetFileNameWithoutExtension(key), str5));
                            if (File.Exists(str6))
                            {
                                this.progFile.Value = 0;
                                this.progFile.Text = string.Empty;
                                base.BeginInvoke(new MethodInvoker(() => this.lblFileName.Text = Path.GetFileName(Path.GetFileName(str6))));
                                str3 = Path.Combine(this.TargetPath, string.Concat(fileDatum.FileName, str5));
                                empty = str5;
                                this.SourceFileName = str6;
                                this.TargetFileName = str3;
                                this.CopyFileWithProgress(str6, str3);
                            }
                        }
                        FileInfo fileInfo1 = new FileInfo(str4);
                        if (!(fileDatum.FileSize != fileInfo1.Length | this.CancelFlag))
                        {
                            using (RPM_DataFile rPMDataFile = new RPM_DataFile())
                            {
                                DataFile dataFile = new DataFile()
                                {
                                    AccountId = this.AccountID,
                                    Classification = "Unclassified",
                                    FileAddedTimestamp = new DateTime?(DateTime.Now),
                                    FileExtension = fileDatum.FileExt,
                                    FileExtension2 = empty,
                                    HashAlgorithm = Global.DefaultHashAlgorithm,
                                    FileHashCode = Hash.GetHashFromFile(str3, sHA1),
                                    FileSize = fileDatum.FileSize,
                                    FileTimestamp = new DateTime?(fileDatum.FileTimestamp),
                                    GPS = "",
                                    IsEncrypted = new bool?(false),
                                    IsPurged = new bool?(false),
                                    OriginalFileName = fileDatum.OriginalFileName,
                                    PurgeFileName = "",
                                    Rating = 0,
                                    Security = SECURITY.UNCLASSIFIED,
                                    SetName = ""
                                };
                                if (!string.IsNullOrEmpty(this.SETID))
                                {
                                    dataFile.SetName = this.SETID;
                                    object[] originalFileName = new object[] { dataFile.OriginalFileName, fileDatum.FileName, dataFile.FileExtension, this.SETID };
                                    Global.Log("SET COPY", string.Format("Copy File: {0} -> {1}{2} into SET ID {3}", originalFileName));
                                }
                                dataFile.ShortDesc = string.Concat(fileDatum.OriginalFileName, fileDatum.FileExt);
                                dataFile.StoredFileName = fileDatum.FileName;
                                dataFile.Thumbnail = Utilities.ImageToByte(fileDatum.Thumbnail);
                                dataFile.TrackingID = Guid.Empty;
                                dataFile.UNCName = this.UNCRoot;
                                dataFile.UNCPath = this.DataPath;
                                dataFile.MachineName = this.MachineName;
                                dataFile.MachineAccount = this.MachineAccount;
                                dataFile.UserDomain = this.MachineDomain;
                                dataFile.SourcePath = Path.GetDirectoryName(key);
                                //TODO: Метод был изменен на свойство
                                dataFile.LoginID = Global.LoginIDName;
                                rPMDataFile.SaveUpdate(dataFile);
                                rPMDataFile.Save();
                            }
                        }
                        else
                        {
                            Global.Log("UPLOAD_FAIL", string.Format("Upload failed for {0} / Cancel upload.", this.SourceFileName));
                            if (File.Exists(str3))
                            {
                                File.Delete(str3);
                            }
                        }
                    }
                }
                catch
                {
                }
                Thread.Sleep(250);
                if (this.CancelFlag)
                {
                    break;
                }
            }
            base.BeginInvoke(new MethodInvoker(() => {
                try
                {
                    this.ShowTimestamp = false;
                    this.lblFileName.Text = string.Empty;
                    this.picBox.Image = Resources.check64;
                    this.lblStatus.Text = "Upload Complete";
                    this.lblFileName.Text = string.Empty;
                    this.progFile.Value = 0;
                    this.progBar.Value = 0;
                    this.progFile.Invalidate();
                    this.progBar.Invalidate();
                    this.UploadCompleteCallback();
                }
                catch (Exception exception)
                {
                    this.UploadCompleteCallback();
                }
            }));
        }

        public bool StartUpload(string sourcePath, string server, string relativePath, string AccountPath, Guid account_ID)
        {
            this.FileCount = 0;
            this.AccountID = account_ID;
            bool flag = false;
            this.SourcePath = sourcePath;
            this.Server = server;
            try
            {
                string str = Path.Combine(server, relativePath);
                str = Network.FormatPath(Path.Combine(server, relativePath));
                this.UNCRoot = str;
                this.DataPath = AccountPath;
                this.TargetPath = Path.Combine(str, AccountPath);
                this.RelativePath = relativePath;
                if (!Directory.Exists(this.TargetPath))
                {
                    this.CancelFlag = true;
                }
                else
                {
                    (new Thread(new ThreadStart(this.RunUpload))).Start();
                    flag = true;
                }
            }
            catch (Exception exception)
            {
            }
            return flag;
        }

        void System.IDisposable.Dispose()
        {
            base.Dispose();
        }

        private void Upload_Load(object sender, EventArgs e)
        {
        }

        private void UploadCompleteCallback()
        {
            if (this.EVT_UploadComplete != null)
            {
                this.EVT_UploadComplete();
            }
        }

        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.IsRunning = false;
        }

        [DllImport("mpr.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
        public static extern int WNetGetConnection(string localName, StringBuilder remoteName, ref int length);

        public event Upload.DEL_UploadCallback EVT_UploadCallback;

        public event Upload.DEL_UploadComplete EVT_UploadComplete;

        public event Upload.IntDelegate FileCopyProgress;

        public delegate void DEL_UploadCallback(object sender, CmdEventArgs e);

        public delegate void DEL_UploadComplete();

        public delegate void IntDelegate(int Int);
    }
}