using AppGlobal;
using CatalogPanel.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using Vintasoft.Twain;
using Vintasoft.Twain.ImageEncoders;
using VMInterfaces;
using VMModels.Enums;
using VMModels.Model;

namespace CatalogPanel
{
    public class Scanner : Form
    {
        private DeviceManager _deviceManager;

        private int _sessionCount;

        private int _imageCount;

        private bool _isImageAcquiring;

        private AcquiredImageCollection _images = new AcquiredImageCollection();

        private int _imageIndex = -1;

        private bool _cancelTransferBecauseFormIsClosing;

        private string DocName = string.Empty;

        private string FilePath = string.Empty;

        private string UNC = string.Empty;

        private string ScannerName = string.Empty;

        private Guid DocGuid;

        private string DataPath = string.Empty;

        private bool IsRecorded;

        private Image ThumbnailImage;

        private IContainer components;

        private Label lbl_ScanStatus;

        private FlowLayoutPanel flowPanel;

        private vProgressBar imageAcquisitionProgressBar;

        private vComboBox cboResolution;

        private Label lbl_Resolution;

        private vCheckBox vCheckBox_0;

        private vComboBox cboDevices;

        private Label lbl_ScanDevices;

        private PictureBox ScanPic;

        private vButton btn_Scan;

        private PictureBox WaitPic;

        private Label lbl_DocTitle;

        private vTextBox txtTitle;

        private vTextBox txtSubject;

        private Label lbl_Subject;

        private vTextBox txtKeywords;

        private Label lbl_Keywords;

        private Label lbl_Security;

        private vComboBox cboSecurity;

        public Guid AccountID
        {
            get;
            set;
        }

        public Scanner()
        {
            (new TwainGlobalSettings()).Register("Greg Baker", "license@hdprotech.com", "TDSbklIiUOvaEwMVnM3yheZqkfOFUrr1j6XCh8fEKqW5pcm0LUT0VnfA2UY8SaPpjXL8+TsT6y0LIQz1UdvVlYQ1ZGRDVNeh4xH67A1H6nqEQjcZGL/BLfNqWB0l06Vm2acSL5lKjQ5b2lk0Gtncf/KtY7sEEeqcAuu6Kz7ItuO8");
            this.InitializeComponent();
            this._deviceManager = new DeviceManager(this);
        }

        private void AcquireImage(bool showUI)
        {
            try
            {
                this._sessionCount = 0;
                this._imageCount = 0;
                if (this._deviceManager.ShowDefaultDeviceSelectionDialog())
                {
                    Device defaultDevice = this._deviceManager.DefaultDevice;
                    defaultDevice.ShowUI = showUI;
                    defaultDevice.DisableAfterAcquire = !showUI;
                    defaultDevice.ImageAcquired += new EventHandler<ImageAcquiredEventArgs>(this.device_ImageAcquired);
                    defaultDevice.ScanCompleted += new EventHandler(this.device_ScanCompleted);
                    defaultDevice.UserInterfaceClosed += new EventHandler(this.device_UserInterfaceClosed);
                    defaultDevice.ScanCanceled += new EventHandler(this.device_ScanCanceled);
                    defaultDevice.ScanFailed += new EventHandler<ScanFailedEventArgs>(this.device_ScanFailed);
                    defaultDevice.Acquire();
                }
                else
                {
                    MessageBox.Show("Devices is not selected.");
                }
            }
            catch (TwainException twainException)
            {
                MessageBox.Show(twainException.Message);
                this.ScanComplete();
            }
        }

        private void btn_Scan_Click(object sender, EventArgs e)
        {
            this.ScanPic.Image = null;
            this.Cursor = Cursors.WaitCursor;
            this.WaitPic.Visible = true;
            this.btn_Scan.Enabled = false;
            this.lbl_ScanStatus.Text = LangCtrl.GetString("txt_Scanning", "Scanning...");
            this.ScanDocument();
        }

        private void CloseDevice(Device device)
        {
            if (device != null)
            {
                device.ImageAcquired -= new EventHandler<ImageAcquiredEventArgs>(this.device_ImageAcquired);
                device.ScanCompleted -= new EventHandler(this.device_ScanCompleted);
                device.UserInterfaceClosed -= new EventHandler(this.device_UserInterfaceClosed);
                device.ScanCanceled -= new EventHandler(this.device_ScanCanceled);
                device.ScanFailed -= new EventHandler<ScanFailedEventArgs>(this.device_ScanFailed);
                if (device.State != DeviceState.Closed)
                {
                    device.Close();
                }
            }
            this.ScanComplete();
        }

        private void device_ImageAcquired(object sender, ImageAcquiredEventArgs e)
        {
            Device device = (Device)sender;
            if (this._cancelTransferBecauseFormIsClosing)
            {
                device.CancelTransfer();
                return;
            }
            this._images.Add(e.Image);
            this.SetCurrentImage(this._images.Count - 1);
            Application.DoEvents();
            this.ScanComplete();
            TwainPdfEncoderSettings twainPdfEncoderSetting = new TwainPdfEncoderSettings();
            twainPdfEncoderSetting.PdfDocumentInfo.Author = string.Format("{0} / {1}", Global.GlobalAccount.ToString(), Global.GlobalAccount.BadgeNumber);
            twainPdfEncoderSetting.PdfDocumentInfo.Title = this.txtTitle.Text;
            twainPdfEncoderSetting.PdfDocumentInfo.Creator = string.Concat("Documents acquired from ", device.Info.ProductName);
            this.ScannerName = device.Info.ProductName;
            twainPdfEncoderSetting.PdfDocumentInfo.Subject = this.txtSubject.Text;
            twainPdfEncoderSetting.PdfDocumentInfo.Keywords = this.txtKeywords.Text;
            twainPdfEncoderSetting.PdfDocumentInfo.ModificationDate = DateTime.Now;
            twainPdfEncoderSetting.PdfMultiPage = true;
            if (!Directory.Exists(this.FilePath))
            {
                Directory.CreateDirectory(this.FilePath);
                Network.SetAcl(this.FilePath);
            }
            string str = Path.Combine(this.FilePath, this.DocName);
            try
            {
                e.Image.Save(str, twainPdfEncoderSetting);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            if (!this.IsRecorded)
            {
                this.IsRecorded = true;
            }
        }

        private void device_ImageAcquiringProgress(object sender, ImageAcquiringProgressEventArgs e)
        {
            if (this._cancelTransferBecauseFormIsClosing)
            {
                ((Device)sender).CancelTransfer();
                return;
            }
            this.imageAcquisitionProgressBar.Value = e.Progress;
            if (this.imageAcquisitionProgressBar.Value == 100)
            {
                this.WaitPic.Visible = false;
                this.imageAcquisitionProgressBar.Value = 0;
            }
            Application.DoEvents();
        }

        private void device_ScanCanceled(object sender, EventArgs e)
        {
            if (this._cancelTransferBecauseFormIsClosing)
            {
                this._cancelTransferBecauseFormIsClosing = false;
                base.Close();
                return;
            }
            Device device = (Device)sender;
            if (device.State == DeviceState.Opened)
            {
                device.Close();
            }
            this.UnsubscribeFromDeviceEvents(device);
            this._isImageAcquiring = false;
        }

        private void device_ScanCompleted(object sender, EventArgs e)
        {
            Device device = (Device)sender;
            if (device.State == DeviceState.Enabled && !device.ShowUI)
            {
                device.Disable();
            }
            if (device.State == DeviceState.Opened)
            {
                device.Close();
            }
            if (!device.ShowUI)
            {
                this.UnsubscribeFromDeviceEvents(device);
                this._isImageAcquiring = false;
            }
            this.ScanComplete();
        }

        private void device_ScanFailed(object sender, ScanFailedEventArgs e)
        {
            Device device = (Device)sender;
            if (device.State == DeviceState.Opened)
            {
                device.Close();
            }
            this.lbl_ScanStatus.Text = e.ErrorString;
            this.UnsubscribeFromDeviceEvents(device);
            this._isImageAcquiring = false;
            this.ScanComplete();
        }

        private void device_StateChanged(object sender, DeviceStateChangedEventArgs e)
        {
            if (this._cancelTransferBecauseFormIsClosing)
            {
                ((Device)sender).CancelTransfer();
            }
        }

        private void device_UserInterfaceClosed(object sender, EventArgs e)
        {
            if (this._cancelTransferBecauseFormIsClosing)
            {
                this._cancelTransferBecauseFormIsClosing = false;
                base.Close();
                return;
            }
            Device device = (Device)sender;
            if (device.State == DeviceState.Opened)
            {
                device.Close();
            }
            this.UnsubscribeFromDeviceEvents(device);
            this._isImageAcquiring = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GetCurrentImageInfo(int index, AcquiredImage acquiredImage)
        {
            ImageInfo imageInfo = acquiredImage.ImageInfo;
            object[] objArray = new object[] { index, this._images.Count, imageInfo.Width, imageInfo.Height, imageInfo.BitCount, imageInfo.Resolution };
            return string.Format("Image {0} from {1} ({2} x {3}, {4} bpp, {5})", objArray);
        }

        private void InitializeComponent()
        {
            ListItem listItem = new ListItem();
            ListItem listItem1 = new ListItem();
            ListItem listItem2 = new ListItem();
            ListItem listItem3 = new ListItem();
            ListItem listItem4 = new ListItem();
            this.lbl_ScanStatus = new Label();
            this.flowPanel = new FlowLayoutPanel();
            this.imageAcquisitionProgressBar = new vProgressBar();
            this.cboResolution = new vComboBox();
            this.lbl_Resolution = new Label();
            this.vCheckBox_0 = new vCheckBox();
            this.cboDevices = new vComboBox();
            this.lbl_ScanDevices = new Label();
            this.WaitPic = new PictureBox();
            this.ScanPic = new PictureBox();
            this.btn_Scan = new vButton();
            this.lbl_DocTitle = new Label();
            this.txtTitle = new vTextBox();
            this.txtSubject = new vTextBox();
            this.lbl_Subject = new Label();
            this.txtKeywords = new vTextBox();
            this.lbl_Keywords = new Label();
            this.lbl_Security = new Label();
            this.cboSecurity = new vComboBox();
            ((ISupportInitialize)this.WaitPic).BeginInit();
            ((ISupportInitialize)this.ScanPic).BeginInit();
            base.SuspendLayout();
            this.lbl_ScanStatus.AutoSize = true;
            this.lbl_ScanStatus.Location = new Point(335, 348);
            this.lbl_ScanStatus.Name = "lbl_ScanStatus";
            this.lbl_ScanStatus.Size = new Size(47, 13);
            this.lbl_ScanStatus.TabIndex = 16;
            this.lbl_ScanStatus.Text = "Ready...";
            this.flowPanel.AutoScroll = true;
            this.flowPanel.BorderStyle = BorderStyle.FixedSingle;
            this.flowPanel.Location = new Point(9, 249);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Size = new Size(314, 112);
            this.flowPanel.TabIndex = 14;
            this.imageAcquisitionProgressBar.BackColor = Color.Transparent;
            this.imageAcquisitionProgressBar.Location = new Point(338, 314);
            this.imageAcquisitionProgressBar.Name = "imageAcquisitionProgressBar";
            this.imageAcquisitionProgressBar.RoundedCornersMask = 15;
            this.imageAcquisitionProgressBar.RoundedCornersRadius = 0;
            this.imageAcquisitionProgressBar.Size = new Size(231, 21);
            this.imageAcquisitionProgressBar.TabIndex = 15;
            this.imageAcquisitionProgressBar.Text = "vProgressBar1";
            this.imageAcquisitionProgressBar.Value = 0;
            this.imageAcquisitionProgressBar.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.cboResolution.BackColor = Color.White;
            this.cboResolution.DefaultText = "";
            this.cboResolution.DisplayMember = "";
            this.cboResolution.DropDownList = true;
            this.cboResolution.DropDownMaximumSize = new Size(1000, 1000);
            this.cboResolution.DropDownMinimumSize = new Size(10, 10);
            this.cboResolution.DropDownResizeDirection = SizingDirection.Both;
            this.cboResolution.DropDownWidth = 186;
            listItem.RoundedCornersMask = 15;
            listItem.Text = "100";
            listItem1.RoundedCornersMask = 15;
            listItem1.Text = "150";
            listItem2.RoundedCornersMask = 15;
            listItem2.Text = "200";
            listItem3.RoundedCornersMask = 15;
            listItem3.Text = "300";
            listItem4.RoundedCornersMask = 15;
            listItem4.Text = "600";
            this.cboResolution.Items.Add(listItem);
            this.cboResolution.Items.Add(listItem1);
            this.cboResolution.Items.Add(listItem2);
            this.cboResolution.Items.Add(listItem3);
            this.cboResolution.Items.Add(listItem4);
            this.cboResolution.Location = new Point(137, 66);
            this.cboResolution.Name = "cboResolution";
            this.cboResolution.RoundedCornersMaskListItem = 15;
            this.cboResolution.Size = new Size(186, 23);
            this.cboResolution.TabIndex = 4;
            this.cboResolution.UseThemeBackColor = false;
            this.cboResolution.UseThemeDropDownArrowColor = true;
            this.cboResolution.ValueMember = "";
            this.cboResolution.VIBlendScrollBarsTheme = VIBLEND_THEME.VISTABLUE;
            this.cboResolution.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_Resolution.AutoSize = true;
            this.lbl_Resolution.Location = new Point(9, 71);
            this.lbl_Resolution.Name = "lbl_Resolution";
            this.lbl_Resolution.Size = new Size(57, 13);
            this.lbl_Resolution.TabIndex = 3;
            this.lbl_Resolution.Text = "Resolution";
            this.vCheckBox_0.BackColor = Color.Transparent;
            this.vCheckBox_0.Location = new Point(137, 7);
            this.vCheckBox_0.Name = "chkShowUI";
            this.vCheckBox_0.Size = new Size(186, 24);
            this.vCheckBox_0.TabIndex = 0;
            this.vCheckBox_0.Text = "Show Scanner UI";
            this.vCheckBox_0.UseVisualStyleBackColor = false;
            this.vCheckBox_0.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.cboDevices.BackColor = Color.White;
            this.cboDevices.DefaultText = "";
            this.cboDevices.DisplayMember = "";
            this.cboDevices.DropDownList = true;
            this.cboDevices.DropDownMaximumSize = new Size(1000, 1000);
            this.cboDevices.DropDownMinimumSize = new Size(10, 10);
            this.cboDevices.DropDownResizeDirection = SizingDirection.Both;
            this.cboDevices.DropDownWidth = 186;
            this.cboDevices.Location = new Point(137, 37);
            this.cboDevices.Name = "cboDevices";
            this.cboDevices.RoundedCornersMaskListItem = 15;
            this.cboDevices.Size = new Size(186, 23);
            this.cboDevices.TabIndex = 2;
            this.cboDevices.UseThemeBackColor = false;
            this.cboDevices.UseThemeDropDownArrowColor = true;
            this.cboDevices.ValueMember = "";
            this.cboDevices.VIBlendScrollBarsTheme = VIBLEND_THEME.VISTABLUE;
            this.cboDevices.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_ScanDevices.AutoSize = true;
            this.lbl_ScanDevices.Location = new Point(9, 42);
            this.lbl_ScanDevices.Name = "lbl_ScanDevices";
            this.lbl_ScanDevices.Size = new Size(46, 13);
            this.lbl_ScanDevices.TabIndex = 1;
            this.lbl_ScanDevices.Text = "Devices";
            this.WaitPic.Image = Properties.Resources.loading2;
            this.WaitPic.Location = new Point(417, 119);
            this.WaitPic.Name = "WaitPic";
            this.WaitPic.Size = new Size(80, 71);
            this.WaitPic.SizeMode = PictureBoxSizeMode.CenterImage;
            this.WaitPic.TabIndex = 22;
            this.WaitPic.TabStop = false;
            this.WaitPic.Visible = false;
            this.ScanPic.BorderStyle = BorderStyle.FixedSingle;
            this.ScanPic.Location = new Point(338, 6);
            this.ScanPic.Name = "ScanPic";
            this.ScanPic.Size = new Size(231, 301);
            this.ScanPic.SizeMode = PictureBoxSizeMode.StretchImage;
            this.ScanPic.TabIndex = 13;
            this.ScanPic.TabStop = false;
            this.btn_Scan.AllowAnimations = true;
            this.btn_Scan.BackColor = Color.Transparent;
            this.btn_Scan.Image = Properties.Resources.scandoc;
            this.btn_Scan.ImageAlign = ContentAlignment.MiddleLeft;
            this.btn_Scan.Location = new Point(137, 213);
            this.btn_Scan.Name = "btn_Scan";
            this.btn_Scan.RoundedCornersMask = 15;
            this.btn_Scan.RoundedCornersRadius = 0;
            this.btn_Scan.Size = new Size(186, 30);
            this.btn_Scan.TabIndex = 13;
            this.btn_Scan.Text = "Scan";
            this.btn_Scan.UseVisualStyleBackColor = false;
            this.btn_Scan.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Scan.Click += new EventHandler(this.btn_Scan_Click);
            this.lbl_DocTitle.AutoSize = true;
            this.lbl_DocTitle.Location = new Point(9, 101);
            this.lbl_DocTitle.Name = "lbl_DocTitle";
            this.lbl_DocTitle.Size = new Size(79, 13);
            this.lbl_DocTitle.TabIndex = 5;
            this.lbl_DocTitle.Text = "Document Title";
            this.txtTitle.BackColor = Color.White;
            this.txtTitle.BoundsOffset = new Size(1, 1);
            this.txtTitle.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtTitle.DefaultText = "";
            this.txtTitle.Location = new Point(137, 96);
            this.txtTitle.MaxLength = 64;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.PasswordChar = '\0';
            this.txtTitle.ScrollBars = ScrollBars.None;
            this.txtTitle.SelectionLength = 0;
            this.txtTitle.SelectionStart = 0;
            this.txtTitle.Size = new Size(186, 23);
            this.txtTitle.TabIndex = 6;
            this.txtTitle.TextAlign = HorizontalAlignment.Left;
            this.txtTitle.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.txtSubject.BackColor = Color.White;
            this.txtSubject.BoundsOffset = new Size(1, 1);
            this.txtSubject.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtSubject.DefaultText = "";
            this.txtSubject.Location = new Point(137, 125);
            this.txtSubject.MaxLength = 64;
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.PasswordChar = '\0';
            this.txtSubject.ScrollBars = ScrollBars.None;
            this.txtSubject.SelectionLength = 0;
            this.txtSubject.SelectionStart = 0;
            this.txtSubject.Size = new Size(186, 23);
            this.txtSubject.TabIndex = 8;
            this.txtSubject.TextAlign = HorizontalAlignment.Left;
            this.txtSubject.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_Subject.AutoSize = true;
            this.lbl_Subject.Location = new Point(9, 130);
            this.lbl_Subject.Name = "lbl_Subject";
            this.lbl_Subject.Size = new Size(43, 13);
            this.lbl_Subject.TabIndex = 7;
            this.lbl_Subject.Text = "Subject";
            this.txtKeywords.BackColor = Color.White;
            this.txtKeywords.BoundsOffset = new Size(1, 1);
            this.txtKeywords.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtKeywords.DefaultText = "";
            this.txtKeywords.Location = new Point(137, 154);
            this.txtKeywords.MaxLength = 64;
            this.txtKeywords.Name = "txtKeywords";
            this.txtKeywords.PasswordChar = '\0';
            this.txtKeywords.ScrollBars = ScrollBars.None;
            this.txtKeywords.SelectionLength = 0;
            this.txtKeywords.SelectionStart = 0;
            this.txtKeywords.Size = new Size(186, 23);
            this.txtKeywords.TabIndex = 10;
            this.txtKeywords.TextAlign = HorizontalAlignment.Left;
            this.txtKeywords.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_Keywords.AutoSize = true;
            this.lbl_Keywords.Location = new Point(9, 159);
            this.lbl_Keywords.Name = "lbl_Keywords";
            this.lbl_Keywords.Size = new Size(53, 13);
            this.lbl_Keywords.TabIndex = 9;
            this.lbl_Keywords.Text = "Keywords";
            this.lbl_Security.AutoSize = true;
            this.lbl_Security.Location = new Point(9, 189);
            this.lbl_Security.Name = "lbl_Security";
            this.lbl_Security.Size = new Size(74, 13);
            this.lbl_Security.TabIndex = 11;
            this.lbl_Security.Text = "Security Level";
            this.cboSecurity.BackColor = Color.White;
            this.cboSecurity.DefaultText = "";
            this.cboSecurity.DisplayMember = "";
            this.cboSecurity.DropDownList = true;
            this.cboSecurity.DropDownMaximumSize = new Size(1000, 1000);
            this.cboSecurity.DropDownMinimumSize = new Size(10, 10);
            this.cboSecurity.DropDownResizeDirection = SizingDirection.Both;
            this.cboSecurity.DropDownWidth = 186;
            this.cboSecurity.Location = new Point(137, 184);
            this.cboSecurity.Name = "cboSecurity";
            this.cboSecurity.RoundedCornersMaskListItem = 15;
            this.cboSecurity.Size = new Size(186, 23);
            this.cboSecurity.TabIndex = 12;
            this.cboSecurity.UseThemeBackColor = false;
            this.cboSecurity.UseThemeDropDownArrowColor = true;
            this.cboSecurity.ValueMember = "";
            this.cboSecurity.VIBlendScrollBarsTheme = VIBLEND_THEME.VISTABLUE;
            this.cboSecurity.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(574, 370);
            base.Controls.Add(this.cboSecurity);
            base.Controls.Add(this.lbl_Security);
            base.Controls.Add(this.txtKeywords);
            base.Controls.Add(this.lbl_Keywords);
            base.Controls.Add(this.txtSubject);
            base.Controls.Add(this.lbl_Subject);
            base.Controls.Add(this.txtTitle);
            base.Controls.Add(this.lbl_DocTitle);
            base.Controls.Add(this.WaitPic);
            base.Controls.Add(this.lbl_ScanStatus);
            base.Controls.Add(this.flowPanel);
            base.Controls.Add(this.imageAcquisitionProgressBar);
            base.Controls.Add(this.cboResolution);
            base.Controls.Add(this.lbl_Resolution);
            base.Controls.Add(this.vCheckBox_0);
            base.Controls.Add(this.cboDevices);
            base.Controls.Add(this.lbl_ScanDevices);
            base.Controls.Add(this.ScanPic);
            base.Controls.Add(this.btn_Scan);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.Name = "Scanner";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Scanner";
            base.FormClosing += new FormClosingEventHandler(this.Scanner_FormClosing);
            base.Load += new EventHandler(this.Scanner_Load);
            ((ISupportInitialize)this.WaitPic).EndInit();
            ((ISupportInitialize)this.ScanPic).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LoadDevices()
        {
            this.cboDevices.Items.Clear();
            if (this._deviceManager.State != DeviceManagerState.Opened)
            {
                this._deviceManager.IsTwain2Compatible = true;
                if (!this._deviceManager.IsTwainAvailable)
                {
                    this._deviceManager.IsTwain2Compatible = !this._deviceManager.IsTwain2Compatible;
                    if (!this._deviceManager.IsTwainAvailable)
                    {
                        MessageBox.Show("TWAIN device manager not found");
                        return;
                    }
                }
            }
            else
            {
                this._deviceManager.Close();
            }
            try
            {
                this._deviceManager.Open();
            }
            catch (TwainDeviceManagerException twainDeviceManagerException1)
            {
                TwainDeviceManagerException twainDeviceManagerException = twainDeviceManagerException1;
                this._deviceManager.Close();
                MessageBox.Show(twainDeviceManagerException.Message);
                return;
            }
            for (int i = 0; i < this._deviceManager.Devices.Count; i++)
            {
                ListItem listItem = new ListItem();

                Text = this._deviceManager.Devices[i].Info.ProductName;
                Tag = listItem.Text;
                
                this.cboDevices.Items.Add(listItem);
            }
            this.cboDevices.SelectedIndex = 0;
        }

        private void LoadSecurity()
        {
            foreach (string str in AccountSecurity.MaxSecurityLevel(Global.GlobalAccount.Security))
            {
                ListItem listItem = new ListItem();
                listItem = new ListItem()
                {
                    Text = str
                };
                this.cboSecurity.Items.Add(listItem);
            }
            this.cboSecurity.SelectedIndex = 0;
        }

        private void LogScan()
        {
            string str = Path.Combine(this.FilePath, this.DocName);
            FileInfo fileInfo = new FileInfo(str);
            using (RPM_DataFile rPMDataFile = new RPM_DataFile())
            {
                DataFile dataFile = new DataFile()
                {
                    AccountId = this.AccountID,
                    Classification = "Unclassified",
                    FileAddedTimestamp = new DateTime?(DateTime.Now),
                    FileExtension = ".PDF",
                    FileExtension2 = ""
                };
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
                dataFile.FileHashCode = Hash.GetHashFromFile(str, sHA1);
                dataFile.FileSize = fileInfo.Length;
                dataFile.FileTimestamp = new DateTime?(fileInfo.LastWriteTime);
                dataFile.GPS = "";
                dataFile.IsEncrypted = new bool?(false);
                dataFile.IsPurged = new bool?(false);
                dataFile.OriginalFileName = this.DocGuid.ToString();
                dataFile.PurgeFileName = "";
                dataFile.Rating = 0;
                dataFile.Security = AccountSecurity.GetByString(this.cboSecurity.Text);
                dataFile.SetName = "";
                dataFile.ShortDesc = this.txtTitle.Text;
                dataFile.StoredFileName = this.DocGuid.ToString();
                dataFile.Thumbnail = Utilities.ImageToByte(this.ThumbnailImage);
                dataFile.TrackingID = Guid.Empty;
                dataFile.UNCName = this.UNC;
                dataFile.UNCPath = this.DataPath;
                string machineName = Environment.MachineName;
                string userName = Environment.UserName;
                string userDomainName = Environment.UserDomainName;
                dataFile.MachineName = machineName;
                dataFile.MachineAccount = userName;
                dataFile.UserDomain = userDomainName;
                dataFile.SourcePath = this.ScannerName;
                dataFile.LoginID = Global.LoginIDName;
                rPMDataFile.SaveUpdate(dataFile);
                rPMDataFile.Save();
            }
        }

        private bool OpenDeviceManager()
        {
            if (!this._deviceManager.IsTwainAvailable)
            {
                if (this._deviceManager.IsTwain2Compatible)
                {
                    MessageBox.Show("TWAIN device manager not found.");
                    return false;
                }
                this._deviceManager.IsTwain2Compatible = true;
                if (!this._deviceManager.IsTwainAvailable)
                {
                    MessageBox.Show("TWAIN device manager not found.");
                    return false;
                }
            }
            this._deviceManager.Open();
            if (this._deviceManager.Devices.Count != 0)
            {
                return true;
            }
            MessageBox.Show(this, "No devices found!", "Devices", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
        }

        private void ScanComplete()
        {
            this.WaitPic.Visible = false;
            this.btn_Scan.Enabled = true;
            this.Cursor = Cursors.Default;
            this.lbl_ScanStatus.Text = LangCtrl.GetString("txt_ScanReady", "Ready...");
        }

        private void ScanDoc_Shown(object sender, EventArgs e)
        {
            this.OpenDeviceManager();
        }

        private void ScanDocument()
        {
            if (this.cboDevices.SelectedItem == null)
            {
                this.Cursor = Cursors.Default;
                this.WaitPic.Visible = false;
                this.lbl_ScanStatus.Text = LangCtrl.GetString("txt_ScannerNotFound", "Scanner not found...");
                MessageBox.Show(this, LangCtrl.GetString("txt_ScannerNotFound", "Scanner not found."), "Scanner", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                this._isImageAcquiring = true;
                string str = this.cboDevices.SelectedItem.ToString();
                Device @checked = this._deviceManager.Devices.Find(str);
                try
                {
                    if (@checked != null)
                    {
                        this.SubscribeToDeviceEvents(@checked);
                        @checked.ShowUI = this.vCheckBox_0.Checked;
                        @checked.ModalUI = false;
                        @checked.ShowIndicators = false;
                        @checked.DisableAfterAcquire = false;
                        @checked.TransferMode = TransferMode.Memory;
                        try
                        {
                            @checked.Open();
                        }
                        catch (TwainException twainException1)
                        {
                            TwainException twainException = twainException1;
                            this._isImageAcquiring = false;
                            MessageBox.Show(twainException.Message, "TWAIN device", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                        @checked.PixelType = PixelType.RGB;
                    }
                    else
                    {
                        this._isImageAcquiring = false;
                        return;
                    }
                }
                catch (TwainException twainException2)
                {
                }
                try
                {
                    @checked.UnitOfMeasure = UnitOfMeasure.Inches;
                }
                catch (TwainException twainException3)
                {
                    MessageBox.Show("Unit of measure 'Inches' is not supported.", "TWAIN device");
                }
                Resolution resolution = new Resolution(100f, 100f);
                try
                {
                    if (this.cboResolution.SelectedIndex == 1)
                    {
                        resolution = new Resolution(150f, 150f);
                    }
                    else if (this.cboResolution.SelectedIndex == 2)
                    {
                        resolution = new Resolution(200f, 200f);
                    }
                    else if (this.cboResolution.SelectedIndex == 3)
                    {
                        resolution = new Resolution(300f, 300f);
                    }
                    else if (this.cboResolution.SelectedIndex == 4)
                    {
                        resolution = new Resolution(600f, 600f);
                    }
                    @checked.Resolution = resolution;
                }
                catch (TwainException twainException4)
                {
                    MessageBox.Show(string.Format("Resolution '{0}' is not supported.", resolution), "TWAIN device");
                }
                try
                {
                    @checked.Acquire();
                }
                catch (TwainException twainException6)
                {
                    TwainException twainException5 = twainException6;
                    this._isImageAcquiring = false;
                    MessageBox.Show(twainException5.Message, "TWAIN device", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void Scanner_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._deviceManager != null)
            {
                this._deviceManager.Dispose();
            }
            if (this.IsRecorded)
            {
                this.LogScan();
            }
        }

        private void Scanner_Load(object sender, EventArgs e)
        {
            this.LoadSecurity();
            this.SetLanguage();
            this.DocGuid = Guid.NewGuid();
            this.DocName = string.Format("{0}.pdf", this.DocGuid);
            this.UNC = Path.Combine(Global.UNCServer, Global.RelativePath);
            if (!this.UNC.Contains(":"))
            {
                if (!this.UNC.StartsWith("\\\\"))
                {
                    this.UNC = string.Concat("\\\\", this.UNC);
                }
            }
            else if (this.UNC.Contains(":") && !this.UNC.Substring(2, 1).Equals("\\"))
            {
                this.UNC = this.UNC.Replace(":", ":\\");
            }
            DateTime now = DateTime.Now;
            object[] str = new object[] { this.AccountID.ToString(), now.Year, now.Month, now.Day };
            this.DataPath = string.Format("{0}\\{1}\\{2:00}\\{3:00}", str);
            this.FilePath = Path.Combine(this.UNC, this.DataPath);
            this.LoadDevices();
            this.cboResolution.SelectedIndex = 3;
            this.flowPanel.VerticalScroll.Maximum = 0;
            this.flowPanel.AutoScroll = true;
        }

        private void SetCurrentImage(int index)
        {
            lock (this)
            {
                if (this.ScanPic.Image != null)
                {
                    this.ScanPic.Image.Dispose();
                    this.ScanPic.Image = null;
                }
                if (index < 0)
                {
                    this.lbl_ScanStatus.Text = LangCtrl.GetString("txt_ScanNoImages", "No images");
                    this._imageIndex = -1;
                }
                else
                {
                    AcquiredImage item = this._images[index];
                    this.ScanPic.Image = item.GetAsBitmap(true);
                    ScanImg scanImg = new ScanImg();
                    scanImg.SetImage((Image)this.ScanPic.Image.Clone());
                    this.flowPanel.Controls.Add(scanImg);
                    if (this._imageIndex == -1)
                    {
                        Image image = (Image)this.ScanPic.Image.Clone();
                        this.ThumbnailImage = image.GetThumbnailImage(220, 130, null, IntPtr.Zero);
                    }
                    this._imageIndex = index;
                }
            }
        }

        private void SetLanguage()
        {
            this.Text = LangCtrl.GetString("dlg_Scanner", "Scanner");
            LangCtrl.reText(this);
        }

        private void SubscribeToDeviceEvents(Device device)
        {
            device.StateChanged += new EventHandler<DeviceStateChangedEventArgs>(this.device_StateChanged);
            device.ImageAcquiringProgress += new EventHandler<ImageAcquiringProgressEventArgs>(this.device_ImageAcquiringProgress);
            device.ImageAcquired += new EventHandler<ImageAcquiredEventArgs>(this.device_ImageAcquired);
            device.ScanCompleted += new EventHandler(this.device_ScanCompleted);
            device.ScanCanceled += new EventHandler(this.device_ScanCanceled);
            device.ScanFailed += new EventHandler<ScanFailedEventArgs>(this.device_ScanFailed);
            device.UserInterfaceClosed += new EventHandler(this.device_UserInterfaceClosed);
        }

        private void UnsubscribeFromDeviceEvents(Device device)
        {
            device.StateChanged -= new EventHandler<DeviceStateChangedEventArgs>(this.device_StateChanged);
            device.ImageAcquiringProgress -= new EventHandler<ImageAcquiringProgressEventArgs>(this.device_ImageAcquiringProgress);
            device.ImageAcquired -= new EventHandler<ImageAcquiredEventArgs>(this.device_ImageAcquired);
            device.ScanCompleted -= new EventHandler(this.device_ScanCompleted);
            device.ScanCanceled -= new EventHandler(this.device_ScanCanceled);
            device.ScanFailed -= new EventHandler<ScanFailedEventArgs>(this.device_ScanFailed);
            device.UserInterfaceClosed -= new EventHandler(this.device_UserInterfaceClosed);
        }
    }
}