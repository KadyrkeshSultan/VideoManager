using HDP_Progressbar;
using IMAPI2.Interop;
using IMAPI2.MediaItem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;

namespace CDBurnCtrl
{
    public class CDCtrl : UserControl, IDisposable
    {
        private const string ClientName = "C3BurnMedia";

        private bool _isBurning;

        private IMAPI_BURN_VERIFICATION_LEVEL _verificationLevel = IMAPI_BURN_VERIFICATION_LEVEL.IMAPI_BURN_VERIFICATION_QUICK;

        private bool _closeMedia;

        private bool _ejectMedia;

        private CDUtils cdUtil = new CDUtils();

        private List<DirectoryItem> DirItems = new List<DirectoryItem>();

        private long _totalDiscSize;

        private BurnData _burnData = new BurnData();

        private IContainer components;

        private vCheckBox chkEject;

        private Label lbl_DiscDrive;

        private Label lbl_VolumeLabel;

        private Label lblMedia;

        private vButton btn_DetectMedia;

        private vButton btn_BurnDisc;

        private TextProgressBar progBar;

        private TextProgressBar progSize;

        private vTextBox txtVolumeLabel;

        private vComboBox cboDevice;

        private BackgroundWorker backgroundWorker;

        private Label lbl_DiscStatus;

        public CDCtrl()
        {
            this.InitializeComponent();
        }

        public void AddFolders(string Folder)
        {
            lbl_DiscStatus.Text = "Calculating Storage Requirements...";
            lbl_DiscStatus.Invalidate();
            Application.DoEvents();
            DirItems.Clear();
            if (Directory.Exists(Folder))
            {
                string[] directories = Directory.GetDirectories(Folder);
                for (int i = 0; i < (int)directories.Length; i++)
                {
                    DirectoryItem directoryItem = new DirectoryItem(directories[i]);
                    DirItems.Add(directoryItem);
                    UpdateCapacity();
                }
                lbl_DiscStatus.Text = "Ready...";
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            IStream stream;
            MsftDiscRecorder2 msftDiscRecorder2Class = null;
            GInterface6 gInterface6 = null;
            try
            {
                try
                {
                    msftDiscRecorder2Class = (MsftDiscRecorder2)(new MsftDiscRecorder2Class());
                    msftDiscRecorder2Class.InitializeDiscRecorder(((BurnData)e.Argument).uniqueRecorderId);
                    GInterface6 msftDiscFormat2DataClass = (GInterface6)(new MsftDiscFormat2DataClass());
                    msftDiscFormat2DataClass.Recorder = msftDiscRecorder2Class;
                    msftDiscFormat2DataClass.ClientName = "C3BurnMedia";
                    msftDiscFormat2DataClass.ForceMediaToBeClosed = this._closeMedia;
                    gInterface6 = msftDiscFormat2DataClass;
                    ((IBurnVerification)gInterface6).BurnVerificationLevel = this._verificationLevel;
                    object[] multisessionInterfaces = null;
                    if (!gInterface6.MediaHeuristicallyBlank)
                    {
                        multisessionInterfaces = gInterface6.MultisessionInterfaces;
                    }
                    if (this.CreateMediaFileSystem(msftDiscRecorder2Class, multisessionInterfaces, out stream))
                    {
                        gInterface6.Update += new DiscFormat2Data_EventHandler(this.discFormatData_Update);
                        this.backgroundWorker.ReportProgress(0, this._burnData);
                        try
                        {
                            try
                            {
                                gInterface6.Write(stream);
                                e.Result = 0;
                            }
                            catch (COMException cOMException1)
                            {
                                COMException cOMException = cOMException1;
                                e.Result = cOMException.ErrorCode;
                                MessageBox.Show(this, cOMException.Message, "Write failed", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            }
                        }
                        finally
                        {
                            if (stream != null)
                            {
                                Marshal.FinalReleaseComObject(stream);
                            }
                        }
                        gInterface6.Update -= new DiscFormat2Data_EventHandler(this.discFormatData_Update);
                        this.backgroundWorker.ReportProgress(0, this._burnData);
                        if (this._ejectMedia)
                        {
                            msftDiscRecorder2Class.EjectMedia();
                        }
                    }
                    else
                    {
                        e.Result = -1;
                        return;
                    }
                }
                catch (COMException cOMException3)
                {
                    COMException cOMException2 = cOMException3;
                    MessageBox.Show(cOMException2.Message);
                    e.Result = cOMException2.ErrorCode;
                }
            }
            finally
            {
                if (msftDiscRecorder2Class != null)
                {
                    Marshal.ReleaseComObject(msftDiscRecorder2Class);
                }
                if (gInterface6 != null)
                {
                    Marshal.ReleaseComObject(gInterface6);
                }
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BurnData userState = (BurnData)e.UserState;
            if (userState.task == BURN_MEDIA_TASK.BURN_MEDIA_TASK_FILE_SYSTEM)
            {
                this.lbl_DiscStatus.Text = userState.statusMessage;
            }
            else if (userState.task == BURN_MEDIA_TASK.BURN_MEDIA_TASK_WRITING)
            {
                switch (userState.currentAction)
                {
                    case IMAPI_FORMAT2_DATA_WRITE_ACTION.IMAPI_FORMAT2_DATA_WRITE_ACTION_VALIDATING_MEDIA:
                        {
                            this.lbl_DiscStatus.Text = "Validating current media...";
                            break;
                        }
                    case IMAPI_FORMAT2_DATA_WRITE_ACTION.IMAPI_FORMAT2_DATA_WRITE_ACTION_FORMATTING_MEDIA:
                        {
                            this.lbl_DiscStatus.Text = "Formatting media...";
                            break;
                        }
                    case IMAPI_FORMAT2_DATA_WRITE_ACTION.IMAPI_FORMAT2_DATA_WRITE_ACTION_INITIALIZING_HARDWARE:
                        {
                            this.lbl_DiscStatus.Text = "Initializing hardware...";
                            break;
                        }
                    case IMAPI_FORMAT2_DATA_WRITE_ACTION.IMAPI_FORMAT2_DATA_WRITE_ACTION_CALIBRATING_POWER:
                        {
                            this.lbl_DiscStatus.Text = "Optimizing laser intensity...";
                            break;
                        }
                    case IMAPI_FORMAT2_DATA_WRITE_ACTION.IMAPI_FORMAT2_DATA_WRITE_ACTION_WRITING_DATA:
                        {
                            long num = userState.lastWrittenLba - userState.startLba;
                            if (num <= 0L || userState.sectorCount <= 0L)
                            {
                                this.lbl_DiscStatus.Text = "Progress 0%";
                                this.progBar.Value = 0;
                                break;
                            }
                            else
                            {
                                int num1 = (int)(100L * num / userState.sectorCount);
                                this.progBar.Text = string.Format("{0}%", num1);
                                this.progBar.Value = num1;
                                break;
                            }
                        }
                    case IMAPI_FORMAT2_DATA_WRITE_ACTION.IMAPI_FORMAT2_DATA_WRITE_ACTION_FINALIZATION:
                        {
                            this.lbl_DiscStatus.Text = "Finalizing writing...";
                            break;
                        }
                    case IMAPI_FORMAT2_DATA_WRITE_ACTION.IMAPI_FORMAT2_DATA_WRITE_ACTION_COMPLETED:
                        {
                            this.lbl_DiscStatus.Text = "Completed!";
                            break;
                        }
                    case IMAPI_FORMAT2_DATA_WRITE_ACTION.IMAPI_FORMAT2_DATA_WRITE_ACTION_VERIFYING:
                        {
                            this.lbl_DiscStatus.Text = "Verifying";
                            break;
                        }
                }
            }
            Application.DoEvents();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.lbl_DiscStatus.Text = ((int)e.Result == 0 ? "Disc Export Completed!" : "Error Burning Disc!");
            this.progBar.Value = 0;
            this._isBurning = false;
            this.btn_BurnDisc.Text = "Burn Disc";
            this.btn_BurnDisc.Enabled = true;
            this.Cursor = Cursors.Default;
            this.Callback(false);
        }

        private void btn_BurnDisc_Click(object sender, EventArgs e)
        {
            if (this.cboDevice.SelectedIndex > -1)
            {
                if (this._isBurning)
                {
                    this.btn_BurnDisc.Text = LangCtrl.GetString("lbl_BurnDisc", "Burn Disc");
                    this.btn_BurnDisc.Enabled = true;
                    this.backgroundWorker.CancelAsync();
                    return;
                }
                this.DetectMedia();
                this.progBar.Value = 0;
                this.progBar.Text = "0%";
                this.lbl_DiscStatus.Text = LangCtrl.GetString("Disc_1", "Recording Disc...");
                this.btn_BurnDisc.Text = LangCtrl.GetString("DiscCancel", "Cancel");
                this._isBurning = true;
                this._closeMedia = true;
                this._ejectMedia = this.chkEject.Checked;
                ListItem selectedItem = this.cboDevice.SelectedItem;
                this.Callback(true);
                IDiscRecorder2 tag = (IDiscRecorder2)selectedItem.Tag;
                this._burnData.uniqueRecorderId = tag.ActiveDiscRecorder;
                this.backgroundWorker.RunWorkerAsync(this._burnData);
                this.Cursor = Cursors.WaitCursor;
            }
        }

        private void btn_DetectMedia_Click(object sender, EventArgs e)
        {
            this.DetectMedia();
        }

        private void Callback(bool IsRecording)
        {
            if (this.EVT_CDAction != null)
            {
                this.EVT_CDAction(IsRecording);
            }
        }

        private void CDCtrl_Load(object sender, EventArgs e)
        {
            LangCtrl.reText(this);
            DateTime now = DateTime.Now;
            this.txtVolumeLabel.Text = string.Format("{0}_{1:00}_{2:00}", now.Year, now.Month, now.Day);
            this.cdUtil.DetectCDRoms(ref this.cboDevice);
            this.cboDevice.SelectedIndex = -1;
        }

        private bool CreateMediaFileSystem(IDiscRecorder2 discRecorder, object[] multisessionInterfaces, out IStream dataStream)
        {
            bool flag;
            MsftFileSystemImage msftFileSystemImageClass = null;
            try
            {
                try
                {
                    msftFileSystemImageClass = (MsftFileSystemImage)(new MsftFileSystemImageClass());
                    msftFileSystemImageClass.ChooseImageDefaults(discRecorder);
                    msftFileSystemImageClass.FileSystemsToCreate = FsiFileSystems.FsiFileSystemISO9660 | FsiFileSystems.FsiFileSystemJoliet;
                    msftFileSystemImageClass.VolumeName = this.txtVolumeLabel.Text;
                    msftFileSystemImageClass.Update += new DFileSystemImage_EventHandler(this.fileSystemImage_Update);
                    if (multisessionInterfaces != null)
                    {
                        msftFileSystemImageClass.MultisessionInterfaces = multisessionInterfaces;
                        msftFileSystemImageClass.ImportFileSystem();
                    }
                    IFsiDirectoryItem root = msftFileSystemImageClass.Root;
                    foreach (IMediaItem dirItem in this.DirItems)
                    {
                        if (this.backgroundWorker.CancellationPending)
                        {
                            break;
                        }
                        dirItem.AddToFileSystem(root);
                    }
                    msftFileSystemImageClass.Update -= new DFileSystemImage_EventHandler(this.fileSystemImage_Update);
                    if (!this.backgroundWorker.CancellationPending)
                    {
                        dataStream = msftFileSystemImageClass.CreateResultImage().ImageStream;
                    }
                    else
                    {
                        dataStream = null;
                        flag = false;
                        return flag;
                    }
                }
                catch (COMException cOMException1)
                {
                    COMException cOMException = cOMException1;
                    MessageBox.Show(this, cOMException.Message, "Create File System Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    dataStream = null;
                    flag = false;
                    return flag;
                }
                return true;
            }
            finally
            {
                if (msftFileSystemImageClass != null)
                {
                    Marshal.ReleaseComObject(msftFileSystemImageClass);
                }
            }
            return flag;
        }

        private void DetectMedia()
        {
            if (this.cboDevice.SelectedIndex == -1)
            {
                return;
            }
            IDiscRecorder2 tag = (IDiscRecorder2)this.cboDevice.SelectedItem.Tag;
            MsftFileSystemImage msftFileSystemImageClass = null;
            GInterface6 msftDiscFormat2DataClass = null;
            try
            {
                try
                {
                    msftDiscFormat2DataClass = (GInterface6)(new MsftDiscFormat2DataClass());
                    if (msftDiscFormat2DataClass.IsCurrentMediaSupported(tag))
                    {
                        msftDiscFormat2DataClass.Recorder = tag;
                        IMAPI_MEDIA_PHYSICAL_TYPE currentPhysicalMediaType = msftDiscFormat2DataClass.CurrentPhysicalMediaType;
                        this.lblMedia.Text = this.cdUtil.GetMediaTypeString(currentPhysicalMediaType);
                        msftFileSystemImageClass = (MsftFileSystemImage)(new MsftFileSystemImageClass());
                        msftFileSystemImageClass.ChooseImageDefaultsForMediaType(currentPhysicalMediaType);
                        if (!msftDiscFormat2DataClass.MediaHeuristicallyBlank)
                        {
                            msftFileSystemImageClass.MultisessionInterfaces = msftDiscFormat2DataClass.MultisessionInterfaces;
                            msftFileSystemImageClass.ImportFileSystem();
                        }
                        this._totalDiscSize = 2048L * (long)msftFileSystemImageClass.FreeMediaBlocks;
                        this.btn_BurnDisc.Enabled = true;
                    }
                    else
                    {
                        this.lblMedia.Text = LangCtrl.GetString("lblMedia", "NO MEDIA");
                        this._totalDiscSize = 0L;
                        return;
                    }
                }
                catch (COMException cOMException1)
                {
                    COMException cOMException = cOMException1;
                    MessageBox.Show(this, cOMException.Message, "Detect Media Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            finally
            {
                if (msftDiscFormat2DataClass != null)
                {
                    Marshal.ReleaseComObject(msftDiscFormat2DataClass);
                }
                if (msftFileSystemImageClass != null)
                {
                    Marshal.ReleaseComObject(msftFileSystemImageClass);
                }
            }
            this.UpdateCapacity();
        }

        private void discFormatData_Update([In] object sender, [In] object progress)
        {
            if (this.backgroundWorker.CancellationPending)
            {
                ((IDiscFormat2Data)sender).CancelWrite();
                return;
            }
            IDiscFormat2DataEventArgs discFormat2DataEventArg = (IDiscFormat2DataEventArgs)progress;
            this._burnData.task = BURN_MEDIA_TASK.BURN_MEDIA_TASK_WRITING;
            this._burnData.elapsedTime = (long)discFormat2DataEventArg.ElapsedTime;
            this._burnData.remainingTime = (long)discFormat2DataEventArg.RemainingTime;
            this._burnData.totalTime = (long)discFormat2DataEventArg.TotalTime;
            this._burnData.currentAction = discFormat2DataEventArg.CurrentAction;
            this._burnData.startLba = (long)discFormat2DataEventArg.StartLba;
            this._burnData.sectorCount = (long)discFormat2DataEventArg.SectorCount;
            this._burnData.lastReadLba = (long)discFormat2DataEventArg.LastReadLba;
            this._burnData.lastWrittenLba = (long)discFormat2DataEventArg.LastWrittenLba;
            this._burnData.totalSystemBuffer = (long)discFormat2DataEventArg.TotalSystemBuffer;
            this._burnData.usedSystemBuffer = (long)discFormat2DataEventArg.UsedSystemBuffer;
            this._burnData.freeSystemBuffer = (long)discFormat2DataEventArg.FreeSystemBuffer;
            this.backgroundWorker.ReportProgress(0, this._burnData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void fileSystemImage_Update([In] object sender, [In] string currentFile, [In] int copiedSectors, [In] int totalSectors)
        {
            int num = 0;
            if (copiedSectors > 0 && totalSectors > 0)
            {
                num = copiedSectors * 100 / totalSectors;
            }
            if (!string.IsNullOrEmpty(currentFile))
            {
                FileInfo fileInfo = new FileInfo(currentFile);
                this.lbl_DiscStatus.Text = string.Concat("Adding \"", fileInfo.Name, "\" to image...");
                this._burnData.task = BURN_MEDIA_TASK.BURN_MEDIA_TASK_FILE_SYSTEM;
                this.backgroundWorker.ReportProgress(num, this._burnData);
            }
        }

        private void InitializeComponent()
        {
            this.chkEject = new vCheckBox();
            this.lbl_DiscDrive = new Label();
            this.lbl_VolumeLabel = new Label();
            this.lblMedia = new Label();
            this.btn_DetectMedia = new vButton();
            this.btn_BurnDisc = new vButton();
            this.progBar = new TextProgressBar();
            this.progSize = new TextProgressBar();
            this.txtVolumeLabel = new vTextBox();
            this.cboDevice = new vComboBox();
            this.backgroundWorker = new BackgroundWorker();
            this.lbl_DiscStatus = new Label();
            base.SuspendLayout();
            this.chkEject.BackColor = Color.Transparent;
            this.chkEject.Checked = true;
            this.chkEject.CheckState = CheckState.Checked;
            this.chkEject.Location = new Point(130, 5);
            this.chkEject.Name = "chkEject";
            this.chkEject.Size = new Size(271, 24);
            this.chkEject.TabIndex = 0;
            this.chkEject.Text = "Eject Disc When Completed";
            this.chkEject.UseVisualStyleBackColor = false;
            this.chkEject.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_DiscDrive.AutoSize = true;
            this.lbl_DiscDrive.Location = new Point(4, 40);
            this.lbl_DiscDrive.Name = "lbl_DiscDrive";
            this.lbl_DiscDrive.Size = new Size(56, 13);
            this.lbl_DiscDrive.TabIndex = 1;
            this.lbl_DiscDrive.Text = "Disc Drive";
            this.lbl_VolumeLabel.AutoSize = true;
            this.lbl_VolumeLabel.Location = new Point(4, 69);
            this.lbl_VolumeLabel.Name = "lbl_VolumeLabel";
            this.lbl_VolumeLabel.Size = new Size(95, 13);
            this.lbl_VolumeLabel.TabIndex = 3;
            this.lbl_VolumeLabel.Text = "Disc Volume Label";
            this.lblMedia.BorderStyle = BorderStyle.FixedSingle;
            this.lblMedia.Location = new Point(308, 35);
            this.lblMedia.Name = "lblMedia";
            this.lblMedia.Size = new Size(121, 23);
            this.lblMedia.TabIndex = 5;
            this.lblMedia.Text = "No Media";
            this.lblMedia.TextAlign = ContentAlignment.MiddleCenter;
            this.btn_DetectMedia.AllowAnimations = true;
            this.btn_DetectMedia.BackColor = Color.Transparent;
            this.btn_DetectMedia.Location = new Point(308, 64);
            this.btn_DetectMedia.Name = "btn_DetectMedia";
            this.btn_DetectMedia.RoundedCornersMask = 15;
            this.btn_DetectMedia.RoundedCornersRadius = 0;
            this.btn_DetectMedia.Size = new Size(121, 46);
            this.btn_DetectMedia.TabIndex = 6;
            this.btn_DetectMedia.Text = "Detect Media";
            this.btn_DetectMedia.UseVisualStyleBackColor = false;
            this.btn_DetectMedia.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_DetectMedia.Click += new EventHandler(this.btn_DetectMedia_Click);
            this.btn_BurnDisc.AllowAnimations = true;
            this.btn_BurnDisc.BackColor = Color.Transparent;
            this.btn_BurnDisc.Enabled = false;
            this.btn_BurnDisc.Location = new Point(308, 116);
            this.btn_BurnDisc.Name = "btn_BurnDisc";
            this.btn_BurnDisc.RoundedCornersMask = 15;
            this.btn_BurnDisc.RoundedCornersRadius = 0;
            this.btn_BurnDisc.Size = new Size(121, 46);
            this.btn_BurnDisc.TabIndex = 7;
            this.btn_BurnDisc.Text = "Burn Disc";
            this.btn_BurnDisc.UseVisualStyleBackColor = false;
            this.btn_BurnDisc.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_BurnDisc.Click += new EventHandler(this.btn_BurnDisc_Click);
            this.progBar.Location = new Point(7, 98);
            this.progBar.Name = "progBar";
            this.progBar.Size = new Size(280, 12);
            this.progBar.TabIndex = 8;
            this.progBar.Text = "0%";
            this.progBar.TextForeColor = Color.Black;
            this.progBar.Visible = false;
            this.progSize.Location = new Point(7, 116);
            this.progSize.Name = "progSize";
            this.progSize.Size = new Size(280, 23);
            this.progSize.TabIndex = 9;
            this.progSize.Text = "0%";
            this.progSize.TextForeColor = Color.Black;
            this.txtVolumeLabel.BackColor = Color.White;
            this.txtVolumeLabel.BoundsOffset = new Size(1, 1);
            this.txtVolumeLabel.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtVolumeLabel.DefaultText = "";
            this.txtVolumeLabel.Location = new Point(130, 64);
            this.txtVolumeLabel.MaxLength = 16;
            this.txtVolumeLabel.Name = "txtVolumeLabel";
            this.txtVolumeLabel.PasswordChar = '\0';
            this.txtVolumeLabel.ScrollBars = ScrollBars.None;
            this.txtVolumeLabel.SelectionLength = 0;
            this.txtVolumeLabel.SelectionStart = 0;
            this.txtVolumeLabel.Size = new Size(157, 23);
            this.txtVolumeLabel.TabIndex = 4;
            this.txtVolumeLabel.TextAlign = HorizontalAlignment.Left;
            this.txtVolumeLabel.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.cboDevice.BackColor = Color.White;
            this.cboDevice.DefaultText = "Select Disc...";
            this.cboDevice.DisplayMember = "";
            this.cboDevice.DropDownList = true;
            this.cboDevice.DropDownMaximumSize = new Size(1000, 1000);
            this.cboDevice.DropDownMinimumSize = new Size(10, 10);
            this.cboDevice.DropDownResizeDirection = SizingDirection.Both;
            this.cboDevice.DropDownWidth = 157;
            this.cboDevice.Location = new Point(130, 35);
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.RoundedCornersMaskListItem = 15;
            this.cboDevice.Size = new Size(157, 23);
            this.cboDevice.TabIndex = 11;
            this.cboDevice.UseThemeBackColor = false;
            this.cboDevice.UseThemeDropDownArrowColor = true;
            this.cboDevice.ValueMember = "";
            this.cboDevice.VIBlendScrollBarsTheme = VIBLEND_THEME.VISTABLUE;
            this.cboDevice.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            this.lbl_DiscStatus.Location = new Point(7, 146);
            this.lbl_DiscStatus.Name = "lbl_DiscStatus";
            this.lbl_DiscStatus.Size = new Size(280, 16);
            this.lbl_DiscStatus.TabIndex = 12;
            this.lbl_DiscStatus.Text = "Ready...";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.Controls.Add(this.lbl_DiscStatus);
            base.Controls.Add(this.cboDevice);
            base.Controls.Add(this.progSize);
            base.Controls.Add(this.progBar);
            base.Controls.Add(this.btn_BurnDisc);
            base.Controls.Add(this.btn_DetectMedia);
            base.Controls.Add(this.lblMedia);
            base.Controls.Add(this.txtVolumeLabel);
            base.Controls.Add(this.lbl_VolumeLabel);
            base.Controls.Add(this.lbl_DiscDrive);
            base.Controls.Add(this.chkEject);
            base.Name = "CDCtrl";
            base.Size = new Size(442, 174);
            base.Load += new EventHandler(this.CDCtrl_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            base.ParentForm.FormClosing += new FormClosingEventHandler(this.ParentForm_FormClosing);
        }

        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        void System.IDisposable.Dispose()
        {
        }

        private void UpdateCapacity()
        {
            if (this._totalDiscSize == 0L)
            {
                this.progSize.Text = "0MB";
                return;
            }
            this.progSize.Text = (this._totalDiscSize < 1000000000L ? string.Format("{0}MB", this._totalDiscSize / 1000000L) : string.Format("{0:F2}GB", (double)((float)this._totalDiscSize) / 1000000000));
            long sizeOnDisc = 0L;
            foreach (IMediaItem dirItem in this.DirItems)
            {
                sizeOnDisc += dirItem.SizeOnDisc;
            }
            if (sizeOnDisc == 0L)
            {
                this.progSize.Value = 0;
                this.btn_BurnDisc.Text = LangCtrl.GetString("btn_BurnDisc", "Burn Disc");
                this.btn_BurnDisc.Enabled = true;
                return;
            }
            int num = (int)(sizeOnDisc * 100L / this._totalDiscSize);
            if (num > 100)
            {
                this.progSize.Value = 100;
                this.progSize.Text = LangCtrl.GetString("DiscSize", "Disc Capacity Exceeded!");
                this.btn_BurnDisc.Enabled = false;
                return;
            }
            this.progSize.Value = num;
            TextProgressBar textProgressBar = this.progSize;
            textProgressBar.Text = string.Concat(textProgressBar.Text, string.Format(" - {0}% Free", 100 - num));
            this.btn_BurnDisc.Enabled = true;
        }

        public event CDCtrl.DEL_CDAction EVT_CDAction;

        public delegate void DEL_CDAction(bool IsRecording);
    }
}