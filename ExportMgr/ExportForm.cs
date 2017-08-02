using AppGlobal;
using ExportMgr.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMInterfaces;
using VMModels.Enums;
using VMModels.Model;

namespace ExportMgr
{
    public class ExportForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;

        public const int HT_CAPTION = 2;

        private const int CS_DROPSHADOW = 131072;

        private WebClient webClient = new WebClient();

        private bool IsRunning;

        public bool CancelFlag;

        private ExportForm.ExportFileData EFD;

        private int FileCount = 1;

        private int FolderCount = 1;

        private string FolderName = string.Empty;

        private List<string> FileFolders = new List<string>();

        private string SourceFileName = string.Empty;

        private string TargetFileName = string.Empty;

        private IContainer components;

        private Panel HeaderPanel;

        private Panel ExportPanel;

        private PictureBox pic;

        private Label lbl_ExportFiles;

        private vButton btnClose;

        private Label lbl_HashCode;

        private Label lbl_FileName;

        private PictureBox picThumbnail;

        private vProgressBar progBar;

        private System.Windows.Forms.Timer timer1;

        private Label lblExportFileName;

        private Label lblExportHash;

        private vProgressBar progFile;

        private vListBox vListBox;

        private ImageList imageList1;

        private Label lblExportMessage;

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

        public List<Guid> FileID
        {
            get;
            set;
        }

        public string FolderPath
        {
            get;
            set;
        }

        private bool IsDVD
        {
            get;
            set;
        }

        private string TEMP_FOLDER
        {
            get;
            set;
        }

        public ExportForm()
        {
            this.InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void BurnCompleteCallback()
        {
            if (this.EVT_BurnComplete != null)
            {
                this.EVT_BurnComplete();
            }
        }

        private void CopyFile(DataFile dRec)
        {
            try
            {
                string str = Path.Combine(dRec.UNCName, dRec.UNCPath);
                this.SourceFileName = string.Concat(Path.Combine(str, dRec.StoredFileName), dRec.FileExtension);
                string str1 = string.Concat(dRec.OriginalFileName, dRec.FileExtension);
                this.TargetFileName = Path.Combine(this.FolderName, str1);
                base.BeginInvoke(new MethodInvoker(() => this.progFile.Visible = true));
                this.CopyFileWithProgress(this.SourceFileName, this.TargetFileName);
                string str2 = string.Concat(Path.Combine(str, dRec.StoredFileName), dRec.FileExtension2);
                if (File.Exists(str2))
                {
                    str1 = string.Concat(dRec.OriginalFileName, dRec.FileExtension2);
                    this.CopyFileWithProgress(str2, Path.Combine(this.FolderName, str1));
                }
            }
            catch
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
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                this.CancelFlag = true;
            }
            base.BeginInvoke(new MethodInvoker(() => this.progFile.Visible = false));
        }

        private void CopyMemos(DataFile dRecord)
        {
            try
            {
                if (dRecord != null)
                {
                    using (RPM_DataFile rPMDataFile = new RPM_DataFile())
                    {
                        foreach (FileMemo fileMemo in rPMDataFile.GetDataFile(dRecord.Id).FileMemos)
                        {
                            string shortDesc = fileMemo.ShortDesc;
                            string str = Path.Combine(this.FolderName, string.Format("{0}.rtf", shortDesc));
                            using (StreamWriter streamWriter = new StreamWriter(str))
                            {
                                streamWriter.Write(fileMemo.Memo);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void CopyThumbnails(DataFile df)
        {
            try
            {
                string originalFileName = df.OriginalFileName;
                using (RPM_Snapshot rPMSnapshot = new RPM_Snapshot())
                {
                    List<Snapshot> snapshots = rPMSnapshot.GetSnapshots(df.Id);
                    if (snapshots.Count > 0)
                    {
                        foreach (Snapshot snapshot in snapshots)
                        {
                            string str = Path.Combine(snapshot.UNCName, snapshot.UNCPath);
                            string str1 = Path.Combine(str, snapshot.StoredFileName);
                            string str2 = Path.Combine(this.FolderName, string.Concat(snapshot.StoredFileName, snapshot.FileExtension));
                            File.Copy(str1, str2, true);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void DeleteTempFolder()
        {
            try
            {
                if (Directory.Exists(this.TEMP_FOLDER))
                {
                    Directory.Delete(this.TEMP_FOLDER);
                }
            }
            catch
            {
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

        private void Export()
        {
            this.FileFolders.Clear();
            Stopwatch stopwatch = new Stopwatch();
            int num = 0;
            if (FileID.Count > 0)
            {
                stopwatch.Start();
                progBar.Maximum = FileID.Count;
                using (RPM_DataFile rPMDataFile = new RPM_DataFile())
                {
                    foreach (Guid fileID in FileID)
                    {
                        progFile.Value = 0;
                        DataFile dataFile = rPMDataFile.GetDataFile(fileID);
                        string folderPath = FolderPath;
                        ExportForm exportForm = this;
                        int folderCount = exportForm.FolderCount;
                        int num1 = folderCount;
                        exportForm.FolderCount = folderCount + 1;
                        FolderName = Path.Combine(folderPath, string.Format("FILE_{0}", num1));
                        if (!Directory.Exists(FolderName))
                        {
                            Directory.CreateDirectory(FolderName);
                            Network.SetAcl(FolderName);
                        }
                        FileFolders.Add(FolderName);
                        ListItem listItem1 = new ListItem();
                        vProgressBar _vProgressBar = progBar;
                        ExportForm exportForm1 = this;
                        int fileCount = exportForm1.FileCount;
                        int num2 = fileCount;
                        exportForm1.FileCount = fileCount + 1;
                        _vProgressBar.Value = num2;
                        progBar.Invalidate();
                        Application.DoEvents();
                        picThumbnail.Image = Utilities.ByteArrayToImage(dataFile.Thumbnail);
                        picThumbnail.Invalidate();
                        Application.DoEvents();
                        WriteFileInfo(dataFile);
                        CopyFile(dataFile);
                        listItem1.Text = string.Format("Export: {0}", string.Concat(dataFile.OriginalFileName, dataFile.FileExtension));
                        HashAlgorithm sHA1 = HashAlgorithms.SHA1;
                        switch (dataFile.HashAlgorithm)
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
                        string hashFromFile = Hash.GetHashFromFile(TargetFileName, sHA1);
                        BeginInvoke(new MethodInvoker(() => {
                            lblExportFileName.Text = string.Concat(dataFile.OriginalFileName, dataFile.FileExtension);
                            lblExportHash.Text = Convert.ToString(hashFromFile);
                        }));
                        if (!hashFromFile.Equals(dataFile.FileHashCode))
                        {
                            num++;
                            listItem1.Description = string.Format(LangCtrl.GetString("txt_FileHashFail", "File validate Failed {0} x {1}"), dataFile.FileHashCode, hashFromFile);
                            listItem1.ImageIndex = 2;
                            vListBox.Items.Insert(listItem1, 0);
                        }
                        else
                        {
                            listItem1.ImageIndex = 1;
                            listItem1.Description = string.Format(LangCtrl.GetString("txt_HashVerified", "File Hash code Verified: {0}"), hashFromFile);
                        }
                        vListBox.Items.Insert(listItem1, 0);
                        CopyMemos(dataFile);
                        CopyThumbnails(dataFile);
                        Global.Log("EXPORT", string.Format(LangCtrl.GetString("txt_DiscExport", "Export: {0}{1} - {2}"), dataFile.StoredFileName, dataFile.FileExtension, dataFile.ShortDesc));
                        Application.DoEvents();
                        int num3 = 0;
                        try
                        {
                            using (RPM_Snapshot rPMSnapshot = new RPM_Snapshot())
                            {
                                List<Snapshot> snapshots = rPMSnapshot.GetSnapshots(fileID);
                                if (snapshots.Count > 0)
                                {
                                    foreach (Snapshot snapshot in snapshots)
                                    {
                                        string str1 = Path.Combine(snapshot.UNCName, snapshot.UNCPath);
                                        SourceFileName = Network.FormatPath(Path.Combine(str1, snapshot.StoredFileName));
                                        string str2 = string.Concat("Snapshot_", num3, snapshot.FileExtension);
                                        TargetFileName = Path.Combine(FolderName, str2);
                                        if (!File.Exists(SourceFileName))
                                        {
                                            continue;
                                        }
                                        File.Copy(SourceFileName, TargetFileName, true);
                                        num3++;
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    if (EFD.IsZipFile)
                    {
                        BeginInvoke(new MethodInvoker(() => {
                            ListItem listItem = new ListItem()
                            {
                                ImageIndex = 5,
                                Text = LangCtrl.GetString("txt_SecuringFiles", "Securing Selected Files...Please wait...")
                            };
                            vListBox.Items.Insert(listItem, 0);
                        }));
                        if (EFD.ZipName.Contains("."))
                        {
                            EFD.ZipName = this.EFD.ZipName.Substring(0, this.EFD.ZipName.IndexOf('.'));
                        }
                        //TODO : ref
                        ExportFileData eFD = EFD;
                        eFD.ZipName = string.Concat(eFD.ZipName, ".zip");
                        if (Zip.ZipFolders(this.FileFolders, Path.Combine(this.FolderPath, this.EFD.ZipName), this.EFD.ZipPwd))
                        {
                            Global.Log("EXPORT_ZIP", string.Format(LangCtrl.GetString("txt_ExportSecure", "Export secure file {0}"), Path.Combine(this.FolderPath, this.EFD.ZipName)));
                            foreach (string fileFolder in this.FileFolders)
                            {
                                Directory.Delete(fileFolder, true);
                            }
                        }
                    }
                    stopwatch.Stop();
                    base.BeginInvoke(new MethodInvoker(() => {
                        this.picThumbnail.Image = null;
                        this.lblExportFileName.Text = string.Empty;
                        this.lblExportHash.Text = string.Empty;
                        TimeSpan timeSpan = TimeSpan.FromMilliseconds((double)stopwatch.ElapsedMilliseconds);
                        string str = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D2}", new object[] { timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds });
                        ListItem listItem = new ListItem();
                        if (this.EFD.IsZipFile)
                        {
                            listItem.ImageIndex = 5;
                            listItem.Text = string.Format(LangCtrl.GetString("txt_SecureFileCreated", "Secure File Created > {0}"), this.EFD.ZipName);
                            this.vListBox.Items.Insert(listItem, 0);
                        }
                        listItem = new ListItem();
                        if (!this.IsDVD)
                        {
                            listItem.ImageIndex = 3;
                            listItem.Text = string.Format(LangCtrl.GetString("txt_ExportCompleted", "Export Completed > {0} Files in {1}"), this.FileID.Count, str);
                            Global.Log("EXPORT-COMPLETE", listItem.Text);
                        }
                        else
                        {
                            listItem.ImageIndex = 4;
                            listItem.Text = string.Format(LangCtrl.GetString("txt_DiscPrep", "Disc Prep Completed > {0} Files in {1}"), this.FileID.Count, str);
                        }
                        listItem.Description = string.Format(LangCtrl.GetString("txt_ExportFileErrors", "File Errors: {0}"), num);
                        this.vListBox.Items.Insert(listItem, 0);
                        base.BeginInvoke(new MethodInvoker(() => {
                            this.lblExportFileName.Text = string.Empty;
                            this.lblExportHash.Text = string.Empty;
                        }));
                    }));
                }
            }
            base.BeginInvoke(new MethodInvoker(() => {
                try
                {
                    if (!this.IsDVD)
                    {
                        this.picThumbnail.SizeMode = PictureBoxSizeMode.CenterImage;
                        this.picThumbnail.Image = Properties.Resources.export;
                        this.lbl_FileName.Text = LangCtrl.GetString("txt_FileExportCompleted", "File Export Completed...");
                        this.lbl_HashCode.Text = string.Empty;
                    }
                    else
                    {
                        this.picThumbnail.SizeMode = PictureBoxSizeMode.CenterImage;
                        this.picThumbnail.Image = Properties.Resources.burndisc;
                        this.lbl_FileName.Text = LangCtrl.GetString("txt_RecToDisc", "Record Files to Disc...");
                        this.lbl_HashCode.Text = string.Empty;
                        (new ExportCD()
                        {
                            RootFolder = this.TEMP_FOLDER
                        }).ShowDialog(this);
                        DirectoryInfo directoryInfo = new DirectoryInfo(this.TEMP_FOLDER);
                        FileInfo[] files = directoryInfo.GetFiles();
                        for (int i = 0; i < (int)files.Length; i++)
                        {
                            files[i].Delete();
                        }
                        DirectoryInfo[] directories = directoryInfo.GetDirectories();
                        for (int j = 0; j < (int)directories.Length; j++)
                        {
                            directories[j].Delete(true);
                        }
                        Directory.Delete(this.TEMP_FOLDER);
                        this.BurnCompleteCallback();
                    }
                }
                catch
                {
                }
            }));
        }

        private void ExportForm_EVT_BurnComplete()
        {
            this.EVT_BurnComplete -= new ExportForm.DEL_BurnComplete(this.ExportForm_EVT_BurnComplete);
            base.Close();
        }

        private void ExportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Global.DeviceDetection_Callback(true);
            try
            {
                this.EVT_BurnComplete -= new ExportForm.DEL_BurnComplete(this.ExportForm_EVT_BurnComplete);
            }
            catch
            {
            }
        }

        private void ExportForm_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
            {
                this.btnClose.VIBlendTheme = VIBLEND_THEME.NERO;
                this.HeaderPanel.BackgroundImage = Properties.Resources.topbar45;
            }
            LangCtrl.reText(this);
            Global.DeviceDetection_Callback(false);
            if (string.IsNullOrEmpty(this.FolderPath))
            {
                this.timer1.Enabled = true;
                this.timer1.Start();
                return;
            }
            ZipDlg zipDlg = new ZipDlg()
            {
                FilePath = this.FolderPath
            };
            if (zipDlg.ShowDialog(this) != DialogResult.OK)
            {
                base.Close();
                return;
            }
            this.EFD.IsPwd = zipDlg.IsPwd;
            this.EFD.IsZipFile = zipDlg.IsZipFile;
            this.EFD.ZipName = zipDlg.ZipName;
            this.EFD.ZipPwd = zipDlg.ZipPwd;
            this.timer1.Enabled = true;
            this.timer1.Start();
        }

        private void ExportForm_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder3D(e.Graphics, ((Control)sender).ClientRectangle, Border3DStyle.RaisedOuter);
        }

        private string GetTemporaryDirectory()
        {
            this.TEMP_FOLDER = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            if (!Directory.Exists(this.TEMP_FOLDER))
            {
                Directory.CreateDirectory(this.TEMP_FOLDER);
                Network.SetAcl(this.TEMP_FOLDER);
            }
            return this.TEMP_FOLDER;
        }

        private void HeaderMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ExportForm.ReleaseCapture();
                ExportForm.SendMessage(base.Handle, 161, 2, 0);
            }
        }

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ExportForm));
            this.ExportPanel = new Panel();
            this.vListBox = new vListBox();
            this.imageList1 = new ImageList(this.components);
            this.progFile = new vProgressBar();
            this.lblExportHash = new Label();
            this.lblExportFileName = new Label();
            this.lbl_HashCode = new Label();
            this.lbl_FileName = new Label();
            this.picThumbnail = new PictureBox();
            this.progBar = new vProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.HeaderPanel = new Panel();
            this.lblExportMessage = new Label();
            this.btnClose = new vButton();
            this.pic = new PictureBox();
            this.lbl_ExportFiles = new Label();
            this.ExportPanel.SuspendLayout();
            ((ISupportInitialize)this.picThumbnail).BeginInit();
            this.HeaderPanel.SuspendLayout();
            ((ISupportInitialize)this.pic).BeginInit();
            base.SuspendLayout();
            this.ExportPanel.BorderStyle = BorderStyle.FixedSingle;
            this.ExportPanel.Controls.Add(this.vListBox);
            this.ExportPanel.Controls.Add(this.progFile);
            this.ExportPanel.Controls.Add(this.lblExportHash);
            this.ExportPanel.Controls.Add(this.lblExportFileName);
            this.ExportPanel.Controls.Add(this.lbl_HashCode);
            this.ExportPanel.Controls.Add(this.lbl_FileName);
            this.ExportPanel.Controls.Add(this.picThumbnail);
            this.ExportPanel.Controls.Add(this.progBar);
            this.ExportPanel.Dock = DockStyle.Fill;
            this.ExportPanel.Location = new Point(0, 45);
            this.ExportPanel.Name = "ExportPanel";
            this.ExportPanel.Size = new Size(560, 345);
            this.ExportPanel.TabIndex = 1;
            this.vListBox.Dock = DockStyle.Bottom;
            this.vListBox.ImageList = this.imageList1;
            this.vListBox.ItemHeight = 34;
            this.vListBox.Location = new Point(0, 122);
            this.vListBox.Name = "vListBox";
            this.vListBox.RoundedCornersMaskListItem = 15;
            this.vListBox.Size = new Size(558, 221);
            this.vListBox.TabIndex = 6;
            this.vListBox.VIBlendScrollBarsTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.vListBox.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.imageList1.ImageStream = (ImageListStreamer)Resources.ExportForm.imageList1_ImageStream;
            this.imageList1.TransparentColor = Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "copy.png");
            this.imageList1.Images.SetKeyName(1, "check.png");
            this.imageList1.Images.SetKeyName(2, "stop.png");
            this.imageList1.Images.SetKeyName(3, "completed.png");
            this.imageList1.Images.SetKeyName(4, "dvd.png");
            this.imageList1.Images.SetKeyName(5, "zip.png");
            this.progFile.BackColor = Color.Transparent;
            this.progFile.Location = new Point(178, 83);
            this.progFile.Name = "progFile";
            this.progFile.RoundedCornersMask = 15;
            this.progFile.Size = new Size(368, 26);
            this.progFile.TabIndex = 1;
            this.progFile.Text = "vProgressBar1";
            this.progFile.Value = 0;
            this.progFile.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.progFile.Visible = false;
            this.lblExportHash.AutoSize = true;
            this.lblExportHash.Location = new Point(286, 29);
            this.lblExportHash.Name = "lblExportHash";
            this.lblExportHash.Size = new Size(57, 13);
            this.lblExportHash.TabIndex = 5;
            this.lblExportHash.Text = "hash code";
            this.lblExportFileName.AutoSize = true;
            this.lblExportFileName.Location = new Point(286, 5);
            this.lblExportFileName.Name = "lblExportFileName";
            this.lblExportFileName.Size = new Size(49, 13);
            this.lblExportFileName.TabIndex = 4;
            this.lblExportFileName.Text = "file name";
            this.lbl_HashCode.AutoSize = true;
            this.lbl_HashCode.Location = new Point(178, 29);
            this.lbl_HashCode.Name = "lbl_HashCode";
            this.lbl_HashCode.Size = new Size(79, 13);
            this.lbl_HashCode.TabIndex = 3;
            this.lbl_HashCode.Text = "File Hash Code";
            this.lbl_FileName.AutoSize = true;
            this.lbl_FileName.Location = new Point(178, 5);
            this.lbl_FileName.Name = "lbl_FileName";
            this.lbl_FileName.Size = new Size(54, 13);
            this.lbl_FileName.TabIndex = 2;
            this.lbl_FileName.Text = "File Name";
            this.picThumbnail.BackColor = Color.Black;
            this.picThumbnail.Location = new Point(11, 5);
            this.picThumbnail.Name = "picThumbnail";
            this.picThumbnail.Size = new Size(161, 105);
            this.picThumbnail.SizeMode = PictureBoxSizeMode.StretchImage;
            this.picThumbnail.TabIndex = 1;
            this.picThumbnail.TabStop = false;
            this.progBar.BackColor = Color.Transparent;
            this.progBar.Location = new Point(178, 53);
            this.progBar.Name = "progBar";
            this.progBar.RoundedCornersMask = 15;
            this.progBar.Size = new Size(368, 26);
            this.progBar.TabIndex = 0;
            this.progBar.Text = "vProgressBar1";
            this.progBar.Value = 0;
            this.progBar.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.timer1.Interval = 500;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.Controls.Add(this.lblExportMessage);
            this.HeaderPanel.Controls.Add(this.btnClose);
            this.HeaderPanel.Controls.Add(this.pic);
            this.HeaderPanel.Controls.Add(this.lbl_ExportFiles);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(560, 45);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.lblExportMessage.AutoSize = true;
            this.lblExportMessage.BackColor = Color.Transparent;
            this.lblExportMessage.ForeColor = Color.White;
            this.lblExportMessage.Location = new Point(66, 26);
            this.lblExportMessage.Name = "lblExportMessage";
            this.lblExportMessage.Size = new Size(93, 13);
            this.lblExportMessage.TabIndex = 3;
            this.lblExportMessage.Text = "Export to device...";
            this.lblExportMessage.MouseDown += new MouseEventHandler(this.lblMessage_MouseDown);
            this.btnClose.AllowAnimations = true;
            this.btnClose.BackColor = Color.Transparent;
            this.btnClose.Dock = DockStyle.Right;
            this.btnClose.Image = Properties.Resources.close;
            this.btnClose.Location = new Point(515, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaintBorder = false;
            this.btnClose.PaintDefaultBorder = false;
            this.btnClose.PaintDefaultFill = false;
            this.btnClose.RoundedCornersMask = 15;
            this.btnClose.RoundedCornersRadius = 0;
            this.btnClose.Size = new Size(45, 45);
            this.btnClose.TabIndex = 2;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.pic.BackColor = Color.Transparent;
            this.pic.Image = Properties.Resources.sentinel;
            this.pic.Location = new Point(5, 5);
            this.pic.Name = "pic";
            this.pic.Size = new Size(36, 36);
            this.pic.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pic.TabIndex = 1;
            this.pic.TabStop = false;
            this.pic.MouseDown += new MouseEventHandler(this.pic_MouseDown);
            this.lbl_ExportFiles.AutoSize = true;
            this.lbl_ExportFiles.BackColor = Color.Transparent;
            this.lbl_ExportFiles.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_ExportFiles.ForeColor = Color.White;
            this.lbl_ExportFiles.Location = new Point(66, 5);
            this.lbl_ExportFiles.Name = "lbl_ExportFiles";
            this.lbl_ExportFiles.Size = new Size(114, 16);
            this.lbl_ExportFiles.TabIndex = 0;
            this.lbl_ExportFiles.Text = "EXPORT FILES";
            this.lbl_ExportFiles.MouseDown += new MouseEventHandler(this.lbl_ExportFiles_MouseDown);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(560, 390);
            base.Controls.Add(this.ExportPanel);
            base.Controls.Add(this.HeaderPanel);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "ExportForm";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Form1";
            base.FormClosing += new FormClosingEventHandler(this.ExportForm_FormClosing);
            base.Load += new EventHandler(this.ExportForm_Load);
            base.Paint += new PaintEventHandler(this.ExportForm_Paint);
            this.ExportPanel.ResumeLayout(false);
            this.ExportPanel.PerformLayout();
            ((ISupportInitialize)this.picThumbnail).EndInit();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((ISupportInitialize)this.pic).EndInit();
            base.ResumeLayout(false);
        }

        private void lbl_ExportFiles_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void lblMessage_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void StartProcess()
        {
            this.IsDVD = false;
            if (string.IsNullOrEmpty(this.FolderPath))
            {
                this.IsDVD = true;
                base.BeginInvoke(new MethodInvoker(() => this.lblExportMessage.Text = LangCtrl.GetString("txt_ExportToDisc", "Export to Disc...")));
                Global.Log("EXPORT-CD", LangCtrl.GetString("txt_ExportToDisc", "Export to Disc"));
                this.GetTemporaryDirectory();
                this.FolderPath = this.TEMP_FOLDER;
                this.EVT_BurnComplete -= new ExportForm.DEL_BurnComplete(this.ExportForm_EVT_BurnComplete);
                this.EVT_BurnComplete += new ExportForm.DEL_BurnComplete(this.ExportForm_EVT_BurnComplete);
            }
            this.Export();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            this.timer1.Stop();
            (new Thread(new ThreadStart(this.StartProcess))).Start();
        }

        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.IsRunning = false;
        }

        [DllImport("mpr.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
        public static extern int WNetGetConnection(string localName, StringBuilder remoteName, ref int length);

        private string WriteFileInfo(DataFile dRec)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_1", "File Is Evidence: {0}"), Environment.NewLine), dRec.IsEvidence));
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_2", "File Name: {0}{1}"), Environment.NewLine), dRec.OriginalFileName, dRec.FileExtension));
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_3", "File Timestamp: {0}"), Environment.NewLine), dRec.FileTimestamp));
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_4", "File Uploaded: {0}"), Environment.NewLine), dRec.FileAddedTimestamp));
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_5", "File Size: {0}"), Environment.NewLine), dRec.FileSize));
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_6", "File Hash Code: {0}"), Environment.NewLine), dRec.FileHashCode));
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_7", "Short Desc: {0}"), Environment.NewLine), dRec.ShortDesc));
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_8", "Stored File Name: {0}"), Environment.NewLine), dRec.StoredFileName));
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_9", "SET: {0}"), Environment.NewLine), dRec.SetName));
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_10", "Security Level: {0}"), Environment.NewLine), dRec.Security));
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_11", "Classification: {0}"), Environment.NewLine), dRec.Classification));
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_12", "Rating: {0}"), Environment.NewLine), dRec.Rating));
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_13", "RMS Number: {0}"), Environment.NewLine), dRec.RMSNumber));
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_14", "CAD Number: {0}"), Environment.NewLine), dRec.CADNumber));
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_15", "Retain Indefinite: {0}"), Environment.NewLine), dRec.IsIndefinite));
            stringBuilder.Append(string.Format(string.Concat(Environment.NewLine, Environment.NewLine, LangCtrl.GetString("wrt_16", "File Uploaded from this machine environment: "), Environment.NewLine), new object[0]));
            stringBuilder.Append(string.Format(string.Concat("---------------------------------------------------------------------", Environment.NewLine), new object[0]));
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_17", "Machine Name: {0}"), Environment.NewLine), dRec.MachineName));
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_18", "Machine Account: {0}"), Environment.NewLine), dRec.MachineAccount));
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_19", "Machine Login ID: {0}"), Environment.NewLine), dRec.LoginID));
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_20", "Machine Domain: {0}"), Environment.NewLine), dRec.UserDomain));
            stringBuilder.Append(string.Format(string.Concat(LangCtrl.GetString("wrt_21", "File Source Path: {0}"), Environment.NewLine), dRec.SourcePath));
            string str = Path.Combine(this.FolderName, string.Format("{0}.txt", dRec.OriginalFileName));
            using (StreamWriter streamWriter = new StreamWriter(str))
            {
                streamWriter.Write(stringBuilder.ToString());
            }
            return str;
        }

        public event ExportForm.DEL_BurnComplete EVT_BurnComplete;

        public event ExportForm.IntDelegate FileCopyProgress;

        public delegate void DEL_BurnComplete();

        private struct ExportFileData
        {
            public bool IsPwd
            {
                get;
                set;
            }

            public bool IsZipFile
            {
                get;
                set;
            }

            public string ZipName
            {
                get;
                set;
            }

            public string ZipPwd
            {
                get;
                set;
            }
        }

        public delegate void IntDelegate(int Int);
    }
}