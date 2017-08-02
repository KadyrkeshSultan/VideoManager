using AppGlobal;
using SlideCtrl2.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMInterfaces;
using VMModels.Enums;
using VMModels.Model;

namespace SlideCtrl2
{
    public class ProfileData : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;

        public const int HT_CAPTION = 2;

        private const int CS_DROPSHADOW = 131072;

        public SlideRecord sRecord = new SlideRecord();

        private IContainer components;

        private Panel HeaderPanel;

        private vButton btnClose;

        private vComboBox cboClass;

        private Label lbl_Classification;

        private Label lbl_FileSecurity;

        private vComboBox cboSecurity;

        private Label lbl_SetName;

        private vTextBox txtSetID;

        private Label lbl_ShortDesc;

        private vTextBox txtShortDesc;

        private Label lbl_Rating;

        private vRatingControl RatingCtrl;

        private Label lbl_FileProfile;

        private PictureBox Thumbnail;

        private vButton btn_Save;

        private Label lblLine;

        private Label ps_FileName;

        private Label lbl_DB_FileName;

        private Label ps_FileDate;

        private Label lblTimestamp;

        private Label ps_FileUploaded;

        private Label lblUploaded;

        private Label ps_FileSize;

        private Label lblSize;

        private Label ps_HashCode;

        private Label lblHashCode;

        private Label ps_GPS;

        private Label lblGPS;

        private Label ps_FileOwner;

        private Label lblAccountName;

        private Label ps_AssetID;

        private Label label_0;

        private Label lbl_FileInfo;

        private Label ps_FileExt;

        private Label lblFileExt;

        private Label ps_OriginalName;

        private Label lblOriginalName;

        private Label lblMachineName;

        private Label lblDomain;

        private Label lblMachineAccount;

        private Label label_1;

        private Label lblSourcePath;

        private Label ps_FileMachineName;

        private Label ps_FileDomain;

        private Label ps_FileDomainAccount;

        private Label ps_FileLoginID;

        private Label ps_SourcePath;

        private vTextBox txtRMS;

        private Label ps_RMS;

        private vTextBox txtCAD;

        private Label ps_CAD;

        private vCheckBox chk_PSIsIndefinite;

        private Panel MainPanel;

        private vCheckBox chk_PSEvidence;

        private Panel BottomPanel;

        private Panel TopPanel;

        private Panel RestorePanel;

        private Label lblRestoreDate;

        private Label lbl_FileRestored;

        private PictureBox pictureBox1;

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

        public ProfileData(SlideRecord sRec)
        {
            this.InitializeComponent();
            this.sRecord = sRec;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            this.sRecord.dRecord.Rating = (int)this.RatingCtrl.Value;
            this.sRecord.dRecord.Classification = this.cboClass.Text;
            this.sRecord.dRecord.Security = AccountSecurity.GetByString(this.cboSecurity.Text);
            this.sRecord.IsDescUpdate = false;
            if (!this.sRecord.dRecord.ShortDesc.Equals(this.txtShortDesc.Text))
            {
                this.sRecord.IsDescUpdate = true;
            }
            this.sRecord.dRecord.ShortDesc = this.txtShortDesc.Text;
            this.sRecord.dRecord.SetName = this.txtSetID.Text;
            this.sRecord.dRecord.CADNumber = (this.txtCAD.Text);
            this.sRecord.dRecord.RMSNumber = (this.txtRMS.Text);
            this.sRecord.dRecord.IsIndefinite = this.chk_PSIsIndefinite.Checked;
            this.sRecord.dRecord.IsEvidence = this.chk_PSEvidence.Checked;
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
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
                ProfileData.ReleaseCapture();
                ProfileData.SendMessage(base.Handle, 161, 2, 0);
            }
        }

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void InitializeComponent()
        {
            this.cboClass = new vComboBox();
            this.lbl_Classification = new Label();
            this.lbl_FileSecurity = new Label();
            this.cboSecurity = new vComboBox();
            this.lbl_SetName = new Label();
            this.txtSetID = new vTextBox();
            this.lbl_ShortDesc = new Label();
            this.txtShortDesc = new vTextBox();
            this.lbl_Rating = new Label();
            this.RatingCtrl = new vRatingControl();
            this.btn_Save = new vButton();
            this.lblLine = new Label();
            this.ps_FileName = new Label();
            this.lbl_DB_FileName = new Label();
            this.ps_FileDate = new Label();
            this.lblTimestamp = new Label();
            this.ps_FileUploaded = new Label();
            this.lblUploaded = new Label();
            this.ps_FileSize = new Label();
            this.lblSize = new Label();
            this.ps_HashCode = new Label();
            this.lblHashCode = new Label();
            this.ps_GPS = new Label();
            this.lblGPS = new Label();
            this.ps_FileOwner = new Label();
            this.lblAccountName = new Label();
            this.ps_AssetID = new Label();
            this.label_0 = new Label();
            this.ps_FileExt = new Label();
            this.lblFileExt = new Label();
            this.ps_OriginalName = new Label();
            this.lblOriginalName = new Label();
            this.lblMachineName = new Label();
            this.lblDomain = new Label();
            this.lblMachineAccount = new Label();
            this.label_1 = new Label();
            this.lblSourcePath = new Label();
            this.ps_FileMachineName = new Label();
            this.ps_FileDomain = new Label();
            this.ps_FileDomainAccount = new Label();
            this.ps_FileLoginID = new Label();
            this.ps_SourcePath = new Label();
            this.txtRMS = new vTextBox();
            this.ps_RMS = new Label();
            this.txtCAD = new vTextBox();
            this.ps_CAD = new Label();
            this.chk_PSIsIndefinite = new vCheckBox();
            this.MainPanel = new Panel();
            this.BottomPanel = new Panel();
            this.Thumbnail = new PictureBox();
            this.TopPanel = new Panel();
            this.RestorePanel = new Panel();
            this.lblRestoreDate = new Label();
            this.lbl_FileRestored = new Label();
            this.pictureBox1 = new PictureBox();
            this.chk_PSEvidence = new vCheckBox();
            this.HeaderPanel = new Panel();
            this.lbl_FileInfo = new Label();
            this.lbl_FileProfile = new Label();
            this.btnClose = new vButton();
            this.MainPanel.SuspendLayout();
            this.BottomPanel.SuspendLayout();
            ((ISupportInitialize)this.Thumbnail).BeginInit();
            this.TopPanel.SuspendLayout();
            this.RestorePanel.SuspendLayout();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            this.HeaderPanel.SuspendLayout();
            base.SuspendLayout();
            this.cboClass.BackColor = Color.White;
            this.cboClass.DefaultText = "";
            this.cboClass.DisplayMember = "";
            this.cboClass.DropDownList = true;
            this.cboClass.DropDownMaximumSize = new Size(1000, 1000);
            this.cboClass.DropDownMinimumSize = new Size(10, 10);
            this.cboClass.DropDownResizeDirection = SizingDirection.Both;
            this.cboClass.DropDownWidth = 244;
            this.cboClass.Location = new Point(311, 70);
            this.cboClass.Name = "cboClass";
            this.cboClass.RoundedCornersMaskListItem = 15;
            this.cboClass.Size = new Size(244, 26);
            this.cboClass.TabIndex = 5;
            this.cboClass.UseThemeBackColor = false;
            this.cboClass.UseThemeDropDownArrowColor = true;
            this.cboClass.ValueMember = "";
            this.cboClass.VIBlendScrollBarsTheme = VIBLEND_THEME.VISTABLUE;
            this.cboClass.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_Classification.AutoSize = true;
            this.lbl_Classification.Location = new Point(11, 77);
            this.lbl_Classification.Name = "lbl_Classification";
            this.lbl_Classification.Size = new Size(87, 13);
            this.lbl_Classification.TabIndex = 4;
            this.lbl_Classification.Text = "File Classification";
            this.lbl_FileSecurity.AutoSize = true;
            this.lbl_FileSecurity.Location = new Point(11, 106);
            this.lbl_FileSecurity.Name = "lbl_FileSecurity";
            this.lbl_FileSecurity.Size = new Size(93, 13);
            this.lbl_FileSecurity.TabIndex = 6;
            this.lbl_FileSecurity.Text = "File Security Level";
            this.cboSecurity.BackColor = Color.White;
            this.cboSecurity.DefaultText = "";
            this.cboSecurity.DisplayMember = "";
            this.cboSecurity.DropDownList = true;
            this.cboSecurity.DropDownMaximumSize = new Size(1000, 1000);
            this.cboSecurity.DropDownMinimumSize = new Size(10, 10);
            this.cboSecurity.DropDownResizeDirection = SizingDirection.Both;
            this.cboSecurity.DropDownWidth = 244;
            this.cboSecurity.Location = new Point(311, 99);
            this.cboSecurity.Name = "cboSecurity";
            this.cboSecurity.RoundedCornersMaskListItem = 15;
            this.cboSecurity.Size = new Size(244, 26);
            this.cboSecurity.TabIndex = 7;
            this.cboSecurity.UseThemeBackColor = false;
            this.cboSecurity.UseThemeDropDownArrowColor = true;
            this.cboSecurity.ValueMember = "";
            this.cboSecurity.VIBlendScrollBarsTheme = VIBLEND_THEME.VISTABLUE;
            this.cboSecurity.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_SetName.AutoSize = true;
            this.lbl_SetName.Location = new Point(11, 48);
            this.lbl_SetName.Name = "lbl_SetName";
            this.lbl_SetName.Size = new Size(37, 13);
            this.lbl_SetName.TabIndex = 2;
            this.lbl_SetName.Text = "Set ID";
            this.txtSetID.BackColor = Color.White;
            this.txtSetID.BoundsOffset = new Size(1, 1);
            this.txtSetID.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtSetID.DefaultText = "";
            this.txtSetID.Location = new Point(311, 41);
            this.txtSetID.MaxLength = 32;
            this.txtSetID.Name = "txtSetID";
            this.txtSetID.PasswordChar = '\0';
            this.txtSetID.ScrollBars = ScrollBars.None;
            this.txtSetID.SelectionLength = 0;
            this.txtSetID.SelectionStart = 0;
            this.txtSetID.Size = new Size(244, 26);
            this.txtSetID.TabIndex = 3;
            this.txtSetID.TextAlign = HorizontalAlignment.Left;
            this.txtSetID.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_ShortDesc.AutoSize = true;
            this.lbl_ShortDesc.Location = new Point(11, 135);
            this.lbl_ShortDesc.Name = "lbl_ShortDesc";
            this.lbl_ShortDesc.Size = new Size(88, 13);
            this.lbl_ShortDesc.TabIndex = 8;
            this.lbl_ShortDesc.Text = "Short Description";
            this.txtShortDesc.BackColor = Color.White;
            this.txtShortDesc.BoundsOffset = new Size(1, 1);
            this.txtShortDesc.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtShortDesc.DefaultText = "";
            this.txtShortDesc.Location = new Point(311, 128);
            this.txtShortDesc.MaxLength = 32;
            this.txtShortDesc.Name = "txtShortDesc";
            this.txtShortDesc.PasswordChar = '\0';
            this.txtShortDesc.ScrollBars = ScrollBars.None;
            this.txtShortDesc.SelectionLength = 0;
            this.txtShortDesc.SelectionStart = 0;
            this.txtShortDesc.Size = new Size(244, 26);
            this.txtShortDesc.TabIndex = 9;
            this.txtShortDesc.TextAlign = HorizontalAlignment.Left;
            this.txtShortDesc.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_Rating.AutoSize = true;
            this.lbl_Rating.Location = new Point(11, 17);
            this.lbl_Rating.Name = "lbl_Rating";
            this.lbl_Rating.Size = new Size(38, 13);
            this.lbl_Rating.TabIndex = 0;
            this.lbl_Rating.Text = "Rating";
            this.RatingCtrl.Location = new Point(311, 12);
            this.RatingCtrl.Name = "RatingCtrl";
            this.RatingCtrl.Shape = vRatingControlShapes.Star;
            this.RatingCtrl.Size = new Size(118, 23);
            this.RatingCtrl.TabIndex = 1;
            this.RatingCtrl.Value = 0f;
            this.RatingCtrl.ValueIndicatorSize = 10;
            this.RatingCtrl.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Save.AllowAnimations = true;
            this.btn_Save.BackColor = Color.Transparent;
            this.btn_Save.Location = new Point(456, 3);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.RoundedCornersMask = 15;
            this.btn_Save.Size = new Size(100, 30);
            this.btn_Save.TabIndex = 10;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = false;
            this.btn_Save.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Save.Click += new EventHandler(this.btn_Save_Click);
            this.lblLine.BackColor = Color.FromArgb(64, 64, 64);
            this.lblLine.Location = new Point(12, 267);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new Size(580, 1);
            this.lblLine.TabIndex = 11;
            this.ps_FileName.AutoSize = true;
            this.ps_FileName.Location = new Point(14, 115);
            this.ps_FileName.Name = "ps_FileName";
            this.ps_FileName.Size = new Size(91, 13);
            this.ps_FileName.TabIndex = 12;
            this.ps_FileName.Text = "System File Name";
            this.lbl_DB_FileName.Location = new Point(166, 115);
            this.lbl_DB_FileName.Name = "lbl_DB_FileName";
            this.lbl_DB_FileName.Size = new Size(300, 13);
            this.lbl_DB_FileName.TabIndex = 13;
            this.lbl_DB_FileName.Text = "File Name";
            this.ps_FileDate.AutoSize = true;
            this.ps_FileDate.Location = new Point(166, 7);
            this.ps_FileDate.Name = "ps_FileDate";
            this.ps_FileDate.Size = new Size(49, 13);
            this.ps_FileDate.TabIndex = 14;
            this.ps_FileDate.Text = "File Date";
            this.lblTimestamp.AutoSize = true;
            this.lblTimestamp.Location = new Point(311, 7);
            this.lblTimestamp.Name = "lblTimestamp";
            this.lblTimestamp.Size = new Size(110, 13);
            this.lblTimestamp.TabIndex = 15;
            this.lblTimestamp.Text = "00/00/0000 00:00:00";
            this.ps_FileUploaded.AutoSize = true;
            this.ps_FileUploaded.Location = new Point(166, 28);
            this.ps_FileUploaded.Name = "ps_FileUploaded";
            this.ps_FileUploaded.Size = new Size(72, 13);
            this.ps_FileUploaded.TabIndex = 16;
            this.ps_FileUploaded.Text = "File Uploaded";
            this.lblUploaded.AutoSize = true;
            this.lblUploaded.Location = new Point(311, 28);
            this.lblUploaded.Name = "lblUploaded";
            this.lblUploaded.Size = new Size(110, 13);
            this.lblUploaded.TabIndex = 17;
            this.lblUploaded.Text = "00/00/0000 00:00:00";
            this.ps_FileSize.AutoSize = true;
            this.ps_FileSize.Location = new Point(166, 49);
            this.ps_FileSize.Name = "ps_FileSize";
            this.ps_FileSize.Size = new Size(46, 13);
            this.ps_FileSize.TabIndex = 18;
            this.ps_FileSize.Text = "File Size";
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new Point(311, 49);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new Size(42, 13);
            this.lblSize.TabIndex = 19;
            this.lblSize.Text = "0 Bytes";
            this.ps_HashCode.AutoSize = true;
            this.ps_HashCode.Location = new Point(166, 70);
            this.ps_HashCode.Name = "ps_HashCode";
            this.ps_HashCode.Size = new Size(92, 13);
            this.ps_HashCode.TabIndex = 20;
            this.ps_HashCode.Text = "File Security Code";
            this.lblHashCode.AutoSize = true;
            this.lblHashCode.Location = new Point(311, 70);
            this.lblHashCode.Name = "lblHashCode";
            this.lblHashCode.Size = new Size(13, 13);
            this.lblHashCode.TabIndex = 21;
            this.lblHashCode.Text = "0";
            this.ps_GPS.AutoSize = true;
            this.ps_GPS.Location = new Point(14, 181);
            this.ps_GPS.Name = "ps_GPS";
            this.ps_GPS.Size = new Size(73, 13);
            this.ps_GPS.TabIndex = 22;
            this.ps_GPS.Text = "GPS Location";
            this.lblGPS.Location = new Point(166, 181);
            this.lblGPS.Name = "lblGPS";
            this.lblGPS.Size = new Size(300, 13);
            this.lblGPS.TabIndex = 23;
            this.lblGPS.Text = "0.0,0.0";
            this.ps_FileOwner.AutoSize = true;
            this.ps_FileOwner.Location = new Point(14, 203);
            this.ps_FileOwner.Name = "ps_FileOwner";
            this.ps_FileOwner.Size = new Size(57, 13);
            this.ps_FileOwner.TabIndex = 24;
            this.ps_FileOwner.Text = "File Owner";
            this.lblAccountName.Location = new Point(166, 203);
            this.lblAccountName.Name = "lblAccountName";
            this.lblAccountName.Size = new Size(300, 13);
            this.lblAccountName.TabIndex = 25;
            this.lblAccountName.Text = "Account";
            this.ps_AssetID.AutoSize = true;
            this.ps_AssetID.Location = new Point(14, 93);
            this.ps_AssetID.Name = "ps_AssetID";
            this.ps_AssetID.Size = new Size(47, 13);
            this.ps_AssetID.TabIndex = 26;
            this.ps_AssetID.Text = "Asset ID";
            this.label_0.Location = new Point(166, 93);
            this.label_0.Name = "lblAssetID";
            this.label_0.Size = new Size(300, 13);
            this.label_0.TabIndex = 27;
            this.label_0.Text = "n/a";
            this.ps_FileExt.AutoSize = true;
            this.ps_FileExt.Location = new Point(14, 159);
            this.ps_FileExt.Name = "ps_FileExt";
            this.ps_FileExt.Size = new Size(72, 13);
            this.ps_FileExt.TabIndex = 28;
            this.ps_FileExt.Text = "File Extension";
            this.lblFileExt.Location = new Point(166, 159);
            this.lblFileExt.Name = "lblFileExt";
            this.lblFileExt.Size = new Size(300, 13);
            this.lblFileExt.TabIndex = 29;
            this.lblFileExt.Text = "n/a";
            this.ps_OriginalName.AutoSize = true;
            this.ps_OriginalName.Location = new Point(14, 137);
            this.ps_OriginalName.Name = "ps_OriginalName";
            this.ps_OriginalName.Size = new Size(92, 13);
            this.ps_OriginalName.TabIndex = 30;
            this.ps_OriginalName.Text = "Original File Name";
            this.lblOriginalName.Location = new Point(166, 137);
            this.lblOriginalName.Name = "lblOriginalName";
            this.lblOriginalName.Size = new Size(300, 13);
            this.lblOriginalName.TabIndex = 31;
            this.lblOriginalName.Text = "File";
            this.lblMachineName.Location = new Point(166, 225);
            this.lblMachineName.Name = "lblMachineName";
            this.lblMachineName.Size = new Size(300, 13);
            this.lblMachineName.TabIndex = 32;
            this.lblMachineName.Text = "Machine Name";
            this.lblDomain.Location = new Point(166, 247);
            this.lblDomain.Name = "lblDomain";
            this.lblDomain.Size = new Size(300, 13);
            this.lblDomain.TabIndex = 33;
            this.lblDomain.Text = "Machine Domain";
            this.lblMachineAccount.Location = new Point(166, 269);
            this.lblMachineAccount.Name = "lblMachineAccount";
            this.lblMachineAccount.Size = new Size(300, 13);
            this.lblMachineAccount.TabIndex = 34;
            this.lblMachineAccount.Text = "Machine Account";
            this.label_1.Location = new Point(166, 291);
            this.label_1.Name = "lblLoginID";
            this.label_1.Size = new Size(300, 13);
            this.label_1.TabIndex = 35;
            this.label_1.Text = "Login ID";
            this.lblSourcePath.Location = new Point(166, 313);
            this.lblSourcePath.Name = "lblSourcePath";
            this.lblSourcePath.Size = new Size(300, 13);
            this.lblSourcePath.TabIndex = 36;
            this.lblSourcePath.Text = "Path";
            this.ps_FileMachineName.AutoSize = true;
            this.ps_FileMachineName.Location = new Point(14, 225);
            this.ps_FileMachineName.Name = "ps_FileMachineName";
            this.ps_FileMachineName.Size = new Size(79, 13);
            this.ps_FileMachineName.TabIndex = 37;
            this.ps_FileMachineName.Text = "Machine Name";
            this.ps_FileDomain.AutoSize = true;
            this.ps_FileDomain.Location = new Point(14, 247);
            this.ps_FileDomain.Name = "ps_FileDomain";
            this.ps_FileDomain.Size = new Size(87, 13);
            this.ps_FileDomain.TabIndex = 38;
            this.ps_FileDomain.Text = "Machine Domain";
            this.ps_FileDomainAccount.AutoSize = true;
            this.ps_FileDomainAccount.Location = new Point(14, 269);
            this.ps_FileDomainAccount.Name = "ps_FileDomainAccount";
            this.ps_FileDomainAccount.Size = new Size(86, 13);
            this.ps_FileDomainAccount.TabIndex = 39;
            this.ps_FileDomainAccount.Text = "Domain Account";
            this.ps_FileLoginID.AutoSize = true;
            this.ps_FileLoginID.Location = new Point(14, 291);
            this.ps_FileLoginID.Name = "ps_FileLoginID";
            this.ps_FileLoginID.Size = new Size(90, 13);
            this.ps_FileLoginID.TabIndex = 40;
            this.ps_FileLoginID.Text = "Account Login ID";
            this.ps_FileLoginID.Click += new EventHandler(this.lbl_FileLoginID_Click);
            this.ps_SourcePath.AutoSize = true;
            this.ps_SourcePath.Location = new Point(14, 313);
            this.ps_SourcePath.Name = "ps_SourcePath";
            this.ps_SourcePath.Size = new Size(85, 13);
            this.ps_SourcePath.TabIndex = 41;
            this.ps_SourcePath.Text = "File Source Path";
            this.txtRMS.BackColor = Color.White;
            this.txtRMS.BoundsOffset = new Size(1, 1);
            this.txtRMS.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtRMS.DefaultText = "";
            this.txtRMS.Location = new Point(311, 157);
            this.txtRMS.MaxLength = 32;
            this.txtRMS.Name = "txtRMS";
            this.txtRMS.PasswordChar = '\0';
            this.txtRMS.ScrollBars = ScrollBars.None;
            this.txtRMS.SelectionLength = 0;
            this.txtRMS.SelectionStart = 0;
            this.txtRMS.Size = new Size(244, 26);
            this.txtRMS.TabIndex = 43;
            this.txtRMS.TextAlign = HorizontalAlignment.Left;
            this.txtRMS.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.ps_RMS.AutoSize = true;
            this.ps_RMS.Location = new Point(11, 164);
            this.ps_RMS.Name = "ps_RMS";
            this.ps_RMS.Size = new Size(158, 13);
            this.ps_RMS.TabIndex = 42;
            this.ps_RMS.Text = "Record Management System ID";
            this.txtCAD.BackColor = Color.White;
            this.txtCAD.BoundsOffset = new Size(1, 1);
            this.txtCAD.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtCAD.DefaultText = "";
            this.txtCAD.Location = new Point(311, 186);
            this.txtCAD.MaxLength = 32;
            this.txtCAD.Name = "txtCAD";
            this.txtCAD.PasswordChar = '\0';
            this.txtCAD.ScrollBars = ScrollBars.None;
            this.txtCAD.SelectionLength = 0;
            this.txtCAD.SelectionStart = 0;
            this.txtCAD.Size = new Size(244, 26);
            this.txtCAD.TabIndex = 45;
            this.txtCAD.TextAlign = HorizontalAlignment.Left;
            this.txtCAD.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.ps_CAD.AutoSize = true;
            this.ps_CAD.Location = new Point(11, 193);
            this.ps_CAD.Name = "ps_CAD";
            this.ps_CAD.Size = new Size(141, 13);
            this.ps_CAD.TabIndex = 44;
            this.ps_CAD.Text = "Computer Aided Dispatch ID";
            this.chk_PSIsIndefinite.BackColor = Color.Transparent;
            this.chk_PSIsIndefinite.Location = new Point(311, 215);
            this.chk_PSIsIndefinite.Name = "chk_PSIsIndefinite";
            this.chk_PSIsIndefinite.Size = new Size(244, 24);
            this.chk_PSIsIndefinite.TabIndex = 46;
            this.chk_PSIsIndefinite.Text = "Retain File Indefinite";
            this.chk_PSIsIndefinite.UseVisualStyleBackColor = false;
            this.chk_PSIsIndefinite.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.MainPanel.BorderStyle = BorderStyle.FixedSingle;
            this.MainPanel.Controls.Add(this.BottomPanel);
            this.MainPanel.Controls.Add(this.TopPanel);
            this.MainPanel.Dock = DockStyle.Fill;
            this.MainPanel.Location = new Point(0, 45);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new Size(600, 605);
            this.MainPanel.TabIndex = 47;
            this.BottomPanel.Controls.Add(this.Thumbnail);
            this.BottomPanel.Controls.Add(this.lblGPS);
            this.BottomPanel.Controls.Add(this.ps_SourcePath);
            this.BottomPanel.Controls.Add(this.ps_GPS);
            this.BottomPanel.Controls.Add(this.ps_FileLoginID);
            this.BottomPanel.Controls.Add(this.ps_FileOwner);
            this.BottomPanel.Controls.Add(this.ps_FileDomainAccount);
            this.BottomPanel.Controls.Add(this.lblHashCode);
            this.BottomPanel.Controls.Add(this.ps_FileDomain);
            this.BottomPanel.Controls.Add(this.lblAccountName);
            this.BottomPanel.Controls.Add(this.ps_FileMachineName);
            this.BottomPanel.Controls.Add(this.ps_HashCode);
            this.BottomPanel.Controls.Add(this.lblSourcePath);
            this.BottomPanel.Controls.Add(this.ps_AssetID);
            this.BottomPanel.Controls.Add(this.lblSize);
            this.BottomPanel.Controls.Add(this.label_1);
            this.BottomPanel.Controls.Add(this.label_0);
            this.BottomPanel.Controls.Add(this.lblMachineAccount);
            this.BottomPanel.Controls.Add(this.ps_FileSize);
            this.BottomPanel.Controls.Add(this.ps_FileName);
            this.BottomPanel.Controls.Add(this.ps_FileExt);
            this.BottomPanel.Controls.Add(this.lblDomain);
            this.BottomPanel.Controls.Add(this.lblUploaded);
            this.BottomPanel.Controls.Add(this.lbl_DB_FileName);
            this.BottomPanel.Controls.Add(this.lblFileExt);
            this.BottomPanel.Controls.Add(this.lblMachineName);
            this.BottomPanel.Controls.Add(this.ps_FileUploaded);
            this.BottomPanel.Controls.Add(this.ps_FileDate);
            this.BottomPanel.Controls.Add(this.ps_OriginalName);
            this.BottomPanel.Controls.Add(this.lblOriginalName);
            this.BottomPanel.Controls.Add(this.lblTimestamp);
            this.BottomPanel.Dock = DockStyle.Top;
            this.BottomPanel.Location = new Point(0, 272);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new Size(598, 332);
            this.BottomPanel.TabIndex = 49;
            this.Thumbnail.BackColor = Color.Black;
            this.Thumbnail.Location = new Point(14, 5);
            this.Thumbnail.Margin = new Padding(0);
            this.Thumbnail.Name = "Thumbnail";
            this.Thumbnail.Size = new Size(137, 80);
            this.Thumbnail.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Thumbnail.TabIndex = 9;
            this.Thumbnail.TabStop = false;
            this.TopPanel.Controls.Add(this.RestorePanel);
            this.TopPanel.Controls.Add(this.btn_Save);
            this.TopPanel.Controls.Add(this.chk_PSEvidence);
            this.TopPanel.Controls.Add(this.lblLine);
            this.TopPanel.Controls.Add(this.RatingCtrl);
            this.TopPanel.Controls.Add(this.chk_PSIsIndefinite);
            this.TopPanel.Controls.Add(this.lbl_Rating);
            this.TopPanel.Controls.Add(this.cboClass);
            this.TopPanel.Controls.Add(this.txtShortDesc);
            this.TopPanel.Controls.Add(this.txtCAD);
            this.TopPanel.Controls.Add(this.lbl_ShortDesc);
            this.TopPanel.Controls.Add(this.lbl_Classification);
            this.TopPanel.Controls.Add(this.txtSetID);
            this.TopPanel.Controls.Add(this.ps_CAD);
            this.TopPanel.Controls.Add(this.lbl_SetName);
            this.TopPanel.Controls.Add(this.lbl_FileSecurity);
            this.TopPanel.Controls.Add(this.ps_RMS);
            this.TopPanel.Controls.Add(this.txtRMS);
            this.TopPanel.Controls.Add(this.cboSecurity);
            this.TopPanel.Dock = DockStyle.Top;
            this.TopPanel.Location = new Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new Size(598, 272);
            this.TopPanel.TabIndex = 48;
            this.RestorePanel.Controls.Add(this.lblRestoreDate);
            this.RestorePanel.Controls.Add(this.lbl_FileRestored);
            this.RestorePanel.Controls.Add(this.pictureBox1);
            this.RestorePanel.Location = new Point(4, 218);
            this.RestorePanel.Name = "RestorePanel";
            this.RestorePanel.Size = new Size(301, 44);
            this.RestorePanel.TabIndex = 48;
            this.RestorePanel.Visible = false;
            this.lblRestoreDate.AutoSize = true;
            this.lblRestoreDate.Location = new Point(32, 25);
            this.lblRestoreDate.Name = "lblRestoreDate";
            this.lblRestoreDate.Size = new Size(110, 13);
            this.lblRestoreDate.TabIndex = 2;
            this.lblRestoreDate.Text = "00/00/0000 00:00:00";
            this.lbl_FileRestored.AutoSize = true;
            this.lbl_FileRestored.Location = new Point(32, 8);
            this.lbl_FileRestored.Name = "lbl_FileRestored";
            this.lbl_FileRestored.Size = new Size(69, 13);
            this.lbl_FileRestored.TabIndex = 1;
            this.lbl_FileRestored.Text = "File Restored";
            this.pictureBox1.Image = Resources.restore;
            this.pictureBox1.Location = new Point(8, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(18, 18);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.chk_PSEvidence.BackColor = Color.Transparent;
            this.chk_PSEvidence.Location = new Point(311, 240);
            this.chk_PSEvidence.Name = "chk_PSEvidence";
            this.chk_PSEvidence.Size = new Size(244, 24);
            this.chk_PSEvidence.TabIndex = 47;
            this.chk_PSEvidence.Text = "Evidence";
            this.chk_PSEvidence.UseVisualStyleBackColor = false;
            this.chk_PSEvidence.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.lbl_FileInfo);
            this.HeaderPanel.Controls.Add(this.lbl_FileProfile);
            this.HeaderPanel.Controls.Add(this.btnClose);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(600, 45);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.lbl_FileInfo.AutoSize = true;
            this.lbl_FileInfo.BackColor = Color.Transparent;
            this.lbl_FileInfo.ForeColor = Color.White;
            this.lbl_FileInfo.Location = new Point(17, 25);
            this.lbl_FileInfo.Name = "lbl_FileInfo";
            this.lbl_FileInfo.Size = new Size(78, 13);
            this.lbl_FileInfo.TabIndex = 3;
            this.lbl_FileInfo.Text = "File Information";
            this.lbl_FileInfo.TextAlign = ContentAlignment.MiddleLeft;
            this.lbl_FileProfile.AutoSize = true;
            this.lbl_FileProfile.BackColor = Color.Transparent;
            this.lbl_FileProfile.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_FileProfile.ForeColor = Color.White;
            this.lbl_FileProfile.Location = new Point(17, 5);
            this.lbl_FileProfile.Name = "lbl_FileProfile";
            this.lbl_FileProfile.Size = new Size(113, 16);
            this.lbl_FileProfile.TabIndex = 2;
            this.lbl_FileProfile.Text = "Profile Settings";
            this.lbl_FileProfile.TextAlign = ContentAlignment.MiddleLeft;
            this.lbl_FileProfile.MouseDown += new MouseEventHandler(this.lbl_FileProfile_MouseDown);
            this.btnClose.AllowAnimations = true;
            this.btnClose.BackColor = Color.Transparent;
            this.btnClose.Dock = DockStyle.Right;
            this.btnClose.Image = Resources.close;
            this.btnClose.Location = new Point(555, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaintBorder = false;
            this.btnClose.PaintDefaultBorder = false;
            this.btnClose.PaintDefaultFill = false;
            this.btnClose.RoundedCornersMask = 15;
            this.btnClose.RoundedCornersRadius = 0;
            this.btnClose.Size = new Size(45, 45);
            this.btnClose.TabIndex = 0;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(600, 650);
            base.Controls.Add(this.MainPanel);
            base.Controls.Add(this.HeaderPanel);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "ProfileData";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "ProfileData";
            base.Load += new EventHandler(this.ProfileData_Load);
            base.Paint += new PaintEventHandler(this.ProfileData_Paint);
            base.Enter += new EventHandler(this.ProfileData_Enter);
            this.MainPanel.ResumeLayout(false);
            this.BottomPanel.ResumeLayout(false);
            this.BottomPanel.PerformLayout();
            ((ISupportInitialize)this.Thumbnail).EndInit();
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            this.RestorePanel.ResumeLayout(false);
            this.RestorePanel.PerformLayout();
            ((ISupportInitialize)this.pictureBox1).EndInit();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            base.ResumeLayout(false);
        }

        private void lbl_FileLoginID_Click(object sender, EventArgs e)
        {
        }

        private void lbl_FileProfile_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void LoadClassList()
        {
            this.cboClass.Items.Clear();
            using (RPM_GeneralData rPMGeneralDatum = new RPM_GeneralData())
            {
                foreach (Classification classificationList in rPMGeneralDatum.GetClassificationList())
                {
                    ListItem listItem = new ListItem()
                    {
                        Text = classificationList.Name,
                        Tag = classificationList
                    };
                    this.cboClass.Items.Add(listItem);
                }
            }
        }

        private void LoadSecurity()
        {
            this.cboSecurity.Items.Clear();
            foreach (string str in AccountSecurity.MaxSecurityLevel(Global.GlobalAccount.Security))
            {
                ListItem listItem = new ListItem();
                listItem = new ListItem()
                {
                    Text = str
                };
                this.cboSecurity.Items.Add(listItem);
            }
        }

        private void ProfileData_Enter(object sender, EventArgs e)
        {
        }

        private void ProfileData_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
            {
                this.HeaderPanel.BackgroundImage = Resources.topbar45;
            }
            this.LoadClassList();
            this.LoadSecurity();
            this.SetLanguage();
            this.SetData();
        }

        private void ProfileData_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder3D(e.Graphics, ((Control)sender).ClientRectangle, Border3DStyle.RaisedOuter);
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void SetData()
        {
            Image image = Utilities.ByteArrayToImage(this.sRecord.dRecord.Thumbnail);
            this.Thumbnail.SizeMode = PictureBoxSizeMode.CenterImage;
            if (image.Height >= 137 || image.Width >= 80)
            {
                this.Thumbnail.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            this.Thumbnail.Image = image;
            this.RatingCtrl.Value = (float)this.sRecord.dRecord.Rating;
            this.txtSetID.Text = this.sRecord.dRecord.SetName;
            this.txtShortDesc.Text = this.sRecord.dRecord.ShortDesc;
            this.lbl_DB_FileName.Text = this.sRecord.dRecord.StoredFileName;
            this.lblTimestamp.Text = string.Format("{0}", this.sRecord.dRecord.FileTimestamp);
            this.lblUploaded.Text = string.Format("{0}", this.sRecord.dRecord.FileAddedTimestamp);
            this.lblSize.Text = string.Format("{0} Bytes", this.sRecord.dRecord.FileSize);
            this.lblHashCode.Text = string.Format("{0}", this.sRecord.dRecord.FileHashCode);
            this.lblGPS.Text = (string.IsNullOrEmpty(this.sRecord.dRecord.GPS) ? "0.0,0.0" : this.sRecord.dRecord.GPS);
            this.label_0.Text = (this.sRecord.dRecord.TrackingID == Guid.Empty ? "n/a" : this.sRecord.dRecord.TrackingID.ToString());
            this.lblFileExt.Text = this.sRecord.dRecord.FileExtension;
            this.lblOriginalName.Text = this.sRecord.dRecord.OriginalFileName;
            this.chk_PSIsIndefinite.Checked = this.sRecord.dRecord.IsIndefinite;
            this.lblMachineAccount.Text = this.sRecord.dRecord.MachineAccount;
            this.lblDomain.Text = this.sRecord.dRecord.UserDomain;
            this.lblMachineName.Text = this.sRecord.dRecord.MachineName;
            this.label_1.Text = this.sRecord.dRecord.LoginID;
            this.lblSourcePath.Text = this.sRecord.dRecord.SourcePath;
            this.txtRMS.Text = this.sRecord.dRecord.RMSNumber;
            this.txtCAD.Text = this.sRecord.dRecord.CADNumber;
            this.chk_PSEvidence.Checked = this.sRecord.dRecord.IsEvidence;
            using (RPM_Account rPMAccount = new RPM_Account())
            {
                Account account = rPMAccount.GetAccount(this.sRecord.dRecord.AccountId);
                this.lblAccountName.Text = string.Format("{0} • {1}", account.ToString(), account.BadgeNumber);
            }
            FormCtrl.SetComboItem(this.cboClass, this.sRecord.dRecord.Classification);
            FormCtrl.SetComboItem(this.cboSecurity, AccountSecurity.GetSecurityDesc(this.sRecord.dRecord.Security));
            bool? isPurged = this.sRecord.dRecord.IsPurged;
            if ((isPurged.GetValueOrDefault() ? false : isPurged.HasValue) && this.sRecord.dRecord.PurgeTimestamp.HasValue)
            {
                this.lblRestoreDate.Text = this.sRecord.dRecord.PurgeTimestamp.ToString();
                this.RestorePanel.Visible = true;
            }
        }

        private void SetLanguage()
        {
            LangCtrl.reText(this);
        }

        private void Thumbnail_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }
    }
}