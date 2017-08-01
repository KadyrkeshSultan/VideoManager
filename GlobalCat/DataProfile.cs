using AppGlobal;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMModels.Model;

namespace GlobalCat
{
    public class DataProfile : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;
        public const int HT_CAPTION = 2;
        private const int CS_DROPSHADOW = 131072;
        private IContainer components;
        private Panel HeaderPanel;
        private Label lbl_MediaProfile;
        private Panel BodyPanel;
        private vButton btnClose;
        private Label lblHashCode;
        private Label mp_HashCode;
        private Label lblSize;
        private Label mp_FileSize;
        private Label lblUploaded;
        private Label mp_FileUploaded;
        private Label mp_FileDate;
        private Label lblTimestamp;
        private Label lblGPS;
        private Label mp_SourcePath;
        private Label mp_GPS;
        private Label mp_FileLoginID;
        private Label mp_FileOwner;
        private Label mp_FileDomainAccount;
        private Label mp_FileDomain;
        private Label lblAccountName;
        private Label mp_FileMachineName;
        private Label lblSourcePath;
        private Label mp_AssetID;
        private Label lblLoginID;
        private Label lblAssetID;
        private Label lblMachineAccount;
        private Label mp_FileName;
        private Label mp_FileExt;
        private Label lblDomain;
        private Label lbl_DB_FileName;
        private Label lblFileExt;
        private Label lblMachineName;
        private Label mp_OriginalName;
        private Label lblOriginalName;
        private vRatingControl RatingCtrl;
        private Label mp_SetID;
        private Label txtSetID;
        private Label mp_ShortDesc;
        private Label txtShortDesc;
        private Label rmsTxt;
        private Label cadTxt;
        private Label mp_RMS;
        private Label mp_CAD;
        private vCheckBox chk_IsEvidence;
        private vCheckBox chk_IsIndefinite;
        private PictureBox picImage;
        private Label lblCount;

        protected override CreateParams CreateParams
        {
            
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ClassStyle |= 131072;
                return createParams;
            }
        }

        
        public DataProfile()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        public void LoadProfile(DataFile dRecord, string acctName, int FileCountID)
        {
            RatingCtrl.Value = (float)dRecord.Rating;
            txtSetID.Text = dRecord.SetName;
            txtShortDesc.Text = dRecord.ShortDesc;
            lbl_DB_FileName.Text = dRecord.StoredFileName;
            lblTimestamp.Text = string.Format("{0}", dRecord.FileTimestamp);
            lblUploaded.Text = string.Format("{0}", dRecord.FileAddedTimestamp);
            lblSize.Text = string.Format("{0} Bytes", dRecord.FileSize);
            lblHashCode.Text = string.Format("{0}", dRecord.FileHashCode);
            lblGPS.Text = (string.IsNullOrEmpty(dRecord.GPS) ? "0.0,0.0" : dRecord.GPS);
            lblAssetID.Text = dRecord.TrackingID == Guid.Empty ? "n/a" : dRecord.TrackingID.ToString();
            lblFileExt.Text = dRecord.FileExtension;
            lblOriginalName.Text = dRecord.OriginalFileName;
            chk_IsIndefinite.Checked = dRecord.IsIndefinite;
            lblMachineAccount.Text = dRecord.MachineAccount;
            lblDomain.Text = dRecord.UserDomain;
            lblMachineName.Text = dRecord.MachineName;
            lblLoginID.Text = dRecord.LoginID;
            lblSourcePath.Text = dRecord.SourcePath;
            rmsTxt.Text = dRecord.RMSNumber;
            cadTxt.Text = dRecord.CADNumber;
            chk_IsEvidence.Checked = dRecord.IsEvidence;
            lblAccountName.Text = acctName;
            lblCount.Text = string.Format("{0}", FileCountID);
            picImage.Image = Utilities.ByteArrayToImage(dRecord.Thumbnail);
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

        
        private void lbl_MediaProfile_MouseDown(object sender, MouseEventArgs e)
        {
            HeaderMouseDown(e);
        }

        
        private void DataProfile_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
                HeaderPanel.BackgroundImage = (Image)Properties.Resources.topbar45;
            LangCtrl.reText(this);
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        
        private void InitializeComponent()
        {
            this.BodyPanel = new Panel();
            this.lblCount = new Label();
            this.chk_IsIndefinite = new vCheckBox();
            this.chk_IsEvidence = new vCheckBox();
            this.mp_CAD = new Label();
            this.cadTxt = new Label();
            this.mp_RMS = new Label();
            this.rmsTxt = new Label();
            this.mp_ShortDesc = new Label();
            this.txtShortDesc = new Label();
            this.mp_SetID = new Label();
            this.txtSetID = new Label();
            this.RatingCtrl = new vRatingControl();
            this.lblGPS = new Label();
            this.mp_SourcePath = new Label();
            this.mp_GPS = new Label();
            this.mp_FileLoginID = new Label();
            this.mp_FileOwner = new Label();
            this.mp_FileDomainAccount = new Label();
            this.mp_FileDomain = new Label();
            this.lblAccountName = new Label();
            this.mp_FileMachineName = new Label();
            this.lblSourcePath = new Label();
            this.mp_AssetID = new Label();
            this.lblLoginID = new Label();
            this.lblAssetID = new Label();
            this.lblMachineAccount = new Label();
            this.mp_FileName = new Label();
            this.mp_FileExt = new Label();
            this.lblDomain = new Label();
            this.lbl_DB_FileName = new Label();
            this.lblFileExt = new Label();
            this.lblMachineName = new Label();
            this.mp_OriginalName = new Label();
            this.lblOriginalName = new Label();
            this.lblHashCode = new Label();
            this.mp_HashCode = new Label();
            this.lblSize = new Label();
            this.mp_FileSize = new Label();
            this.lblUploaded = new Label();
            this.mp_FileUploaded = new Label();
            this.mp_FileDate = new Label();
            this.lblTimestamp = new Label();
            this.picImage = new PictureBox();
            this.HeaderPanel = new Panel();
            this.btnClose = new vButton();
            this.lbl_MediaProfile = new Label();
            this.BodyPanel.SuspendLayout();
            ((ISupportInitialize)this.picImage).BeginInit();
            this.HeaderPanel.SuspendLayout();
            base.SuspendLayout();
            this.BodyPanel.BackColor = Color.White;
            this.BodyPanel.BorderStyle = BorderStyle.FixedSingle;
            this.BodyPanel.Controls.Add(this.lblCount);
            this.BodyPanel.Controls.Add(this.picImage);
            this.BodyPanel.Controls.Add(this.chk_IsIndefinite);
            this.BodyPanel.Controls.Add(this.chk_IsEvidence);
            this.BodyPanel.Controls.Add(this.mp_CAD);
            this.BodyPanel.Controls.Add(this.cadTxt);
            this.BodyPanel.Controls.Add(this.mp_RMS);
            this.BodyPanel.Controls.Add(this.rmsTxt);
            this.BodyPanel.Controls.Add(this.mp_ShortDesc);
            this.BodyPanel.Controls.Add(this.txtShortDesc);
            this.BodyPanel.Controls.Add(this.mp_SetID);
            this.BodyPanel.Controls.Add(this.txtSetID);
            this.BodyPanel.Controls.Add(this.RatingCtrl);
            this.BodyPanel.Controls.Add(this.lblGPS);
            this.BodyPanel.Controls.Add(this.mp_SourcePath);
            this.BodyPanel.Controls.Add(this.mp_GPS);
            this.BodyPanel.Controls.Add(this.mp_FileLoginID);
            this.BodyPanel.Controls.Add(this.mp_FileOwner);
            this.BodyPanel.Controls.Add(this.mp_FileDomainAccount);
            this.BodyPanel.Controls.Add(this.mp_FileDomain);
            this.BodyPanel.Controls.Add(this.lblAccountName);
            this.BodyPanel.Controls.Add(this.mp_FileMachineName);
            this.BodyPanel.Controls.Add(this.lblSourcePath);
            this.BodyPanel.Controls.Add(this.mp_AssetID);
            this.BodyPanel.Controls.Add(this.lblLoginID);
            this.BodyPanel.Controls.Add(this.lblAssetID);
            this.BodyPanel.Controls.Add(this.lblMachineAccount);
            this.BodyPanel.Controls.Add(this.mp_FileName);
            this.BodyPanel.Controls.Add(this.mp_FileExt);
            this.BodyPanel.Controls.Add(this.lblDomain);
            this.BodyPanel.Controls.Add(this.lbl_DB_FileName);
            this.BodyPanel.Controls.Add(this.lblFileExt);
            this.BodyPanel.Controls.Add(this.lblMachineName);
            this.BodyPanel.Controls.Add(this.mp_OriginalName);
            this.BodyPanel.Controls.Add(this.lblOriginalName);
            this.BodyPanel.Controls.Add(this.lblHashCode);
            this.BodyPanel.Controls.Add(this.mp_HashCode);
            this.BodyPanel.Controls.Add(this.lblSize);
            this.BodyPanel.Controls.Add(this.mp_FileSize);
            this.BodyPanel.Controls.Add(this.lblUploaded);
            this.BodyPanel.Controls.Add(this.mp_FileUploaded);
            this.BodyPanel.Controls.Add(this.mp_FileDate);
            this.BodyPanel.Controls.Add(this.lblTimestamp);
            this.BodyPanel.Dock = DockStyle.Fill;
            this.BodyPanel.Location = new Point(0, 45);
            this.BodyPanel.Name = "BodyPanel";
            this.BodyPanel.Size = new Size(527, 464);
            this.BodyPanel.TabIndex = 1;
            this.lblCount.Anchor = AnchorStyles.Right;
            this.lblCount.BackColor = Color.LightGray;
            this.lblCount.BorderStyle = BorderStyle.FixedSingle;
            this.lblCount.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblCount.Location = new Point(451, 1);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new Size(71, 23);
            this.lblCount.TabIndex = 76;
            this.lblCount.Text = "0";
            this.lblCount.TextAlign = ContentAlignment.MiddleCenter;
            this.chk_IsIndefinite.BackColor = Color.Transparent;
            this.chk_IsIndefinite.Enabled = false;
            this.chk_IsIndefinite.Location = new Point(191, 62);
            this.chk_IsIndefinite.Name = "chk_IsIndefinite";
            this.chk_IsIndefinite.Size = new Size(184, 24);
            this.chk_IsIndefinite.TabIndex = 74;
            this.chk_IsIndefinite.Text = "Is Retain Indefinite";
            this.chk_IsIndefinite.UseVisualStyleBackColor = false;
            this.chk_IsIndefinite.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.chk_IsEvidence.BackColor = Color.Transparent;
            this.chk_IsEvidence.Enabled = false;
            this.chk_IsEvidence.Location = new Point(191, 32);
            this.chk_IsEvidence.Name = "chk_IsEvidence";
            this.chk_IsEvidence.Size = new Size(104, 24);
            this.chk_IsEvidence.TabIndex = 73;
            this.chk_IsEvidence.Text = "Is Evidence";
            this.chk_IsEvidence.UseVisualStyleBackColor = false;
            this.chk_IsEvidence.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.mp_CAD.AutoSize = true;
            this.mp_CAD.Location = new Point(24, 160);
            this.mp_CAD.Name = "mp_CAD";
            this.mp_CAD.Size = new Size(69, 13);
            this.mp_CAD.TabIndex = 72;
            this.mp_CAD.Text = "CAD Number";
            this.cadTxt.AutoSize = true;
            this.cadTxt.Location = new Point(191, 160);
            this.cadTxt.Name = "cadTxt";
            this.cadTxt.Size = new Size(69, 13);
            this.cadTxt.TabIndex = 71;
            this.cadTxt.Text = "CAD Number";
            this.mp_RMS.AutoSize = true;
            this.mp_RMS.Location = new Point(24, 141);
            this.mp_RMS.Name = "mp_RMS";
            this.mp_RMS.Size = new Size(71, 13);
            this.mp_RMS.TabIndex = 70;
            this.mp_RMS.Text = "RMS Number";
            this.rmsTxt.AutoSize = true;
            this.rmsTxt.Location = new Point(191, 141);
            this.rmsTxt.Name = "rmsTxt";
            this.rmsTxt.Size = new Size(71, 13);
            this.rmsTxt.TabIndex = 69;
            this.rmsTxt.Text = "RMS Number";
            this.mp_ShortDesc.AutoSize = true;
            this.mp_ShortDesc.Location = new Point(24, 122);
            this.mp_ShortDesc.Name = "mp_ShortDesc";
            this.mp_ShortDesc.Size = new Size(60, 13);
            this.mp_ShortDesc.TabIndex = 68;
            this.mp_ShortDesc.Text = "Short Desc";
            this.txtShortDesc.AutoSize = true;
            this.txtShortDesc.Location = new Point(191, 122);
            this.txtShortDesc.Name = "txtShortDesc";
            this.txtShortDesc.Size = new Size(60, 13);
            this.txtShortDesc.TabIndex = 67;
            this.txtShortDesc.Text = "Short Desc";
            this.mp_SetID.AutoSize = true;
            this.mp_SetID.Location = new Point(24, 103);
            this.mp_SetID.Name = "mp_SetID";
            this.mp_SetID.Size = new Size(37, 13);
            this.mp_SetID.TabIndex = 66;
            this.mp_SetID.Text = "Set ID";
            this.txtSetID.AutoSize = true;
            this.txtSetID.Location = new Point(191, 103);
            this.txtSetID.Name = "txtSetID";
            this.txtSetID.Size = new Size(37, 13);
            this.txtSetID.TabIndex = 65;
            this.txtSetID.Text = "Set ID";
            this.RatingCtrl.Enabled = false;
            this.RatingCtrl.Location = new Point(191, 3);
            this.RatingCtrl.Name = "RatingCtrl";
            this.RatingCtrl.Shape = vRatingControlShapes.Star;
            this.RatingCtrl.Size = new Size(144, 23);
            this.RatingCtrl.TabIndex = 64;
            this.RatingCtrl.Text = "vRatingControl1";
            this.RatingCtrl.Value = 0f;
            this.RatingCtrl.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lblGPS.Location = new Point(191, 332);
            this.lblGPS.Name = "lblGPS";
            this.lblGPS.Size = new Size(300, 13);
            this.lblGPS.TabIndex = 45;
            this.lblGPS.Text = "0.0,0.0";
            this.mp_SourcePath.AutoSize = true;
            this.mp_SourcePath.Location = new Point(24, 446);
            this.mp_SourcePath.Name = "mp_SourcePath";
            this.mp_SourcePath.Size = new Size(85, 13);
            this.mp_SourcePath.TabIndex = 63;
            this.mp_SourcePath.Text = "File Source Path";
            this.mp_GPS.AutoSize = true;
            this.mp_GPS.Location = new Point(24, 332);
            this.mp_GPS.Name = "mp_GPS";
            this.mp_GPS.Size = new Size(73, 13);
            this.mp_GPS.TabIndex = 44;
            this.mp_GPS.Text = "GPS Location";
            this.mp_FileLoginID.AutoSize = true;
            this.mp_FileLoginID.Location = new Point(24, 427);
            this.mp_FileLoginID.Name = "mp_FileLoginID";
            this.mp_FileLoginID.Size = new Size(90, 13);
            this.mp_FileLoginID.TabIndex = 62;
            this.mp_FileLoginID.Text = "Account Login ID";
            this.mp_FileOwner.AutoSize = true;
            this.mp_FileOwner.Location = new Point(24, 351);
            this.mp_FileOwner.Name = "mp_FileOwner";
            this.mp_FileOwner.Size = new Size(57, 13);
            this.mp_FileOwner.TabIndex = 46;
            this.mp_FileOwner.Text = "File Owner";
            this.mp_FileDomainAccount.AutoSize = true;
            this.mp_FileDomainAccount.Location = new Point(24, 408);
            this.mp_FileDomainAccount.Name = "mp_FileDomainAccount";
            this.mp_FileDomainAccount.Size = new Size(86, 13);
            this.mp_FileDomainAccount.TabIndex = 61;
            this.mp_FileDomainAccount.Text = "Domain Account";
            this.mp_FileDomain.AutoSize = true;
            this.mp_FileDomain.Location = new Point(24, 389);
            this.mp_FileDomain.Name = "mp_FileDomain";
            this.mp_FileDomain.Size = new Size(87, 13);
            this.mp_FileDomain.TabIndex = 60;
            this.mp_FileDomain.Text = "Machine Domain";
            this.lblAccountName.Location = new Point(191, 351);
            this.lblAccountName.Name = "lblAccountName";
            this.lblAccountName.Size = new Size(300, 13);
            this.lblAccountName.TabIndex = 47;
            this.lblAccountName.Text = "Account";
            this.mp_FileMachineName.AutoSize = true;
            this.mp_FileMachineName.Location = new Point(24, 370);
            this.mp_FileMachineName.Name = "mp_FileMachineName";
            this.mp_FileMachineName.Size = new Size(79, 13);
            this.mp_FileMachineName.TabIndex = 59;
            this.mp_FileMachineName.Text = "Machine Name";
            this.lblSourcePath.Location = new Point(191, 446);
            this.lblSourcePath.Name = "lblSourcePath";
            this.lblSourcePath.Size = new Size(300, 13);
            this.lblSourcePath.TabIndex = 58;
            this.lblSourcePath.Text = "Path";
            this.mp_AssetID.AutoSize = true;
            this.mp_AssetID.Location = new Point(24, 256);
            this.mp_AssetID.Name = "mp_AssetID";
            this.mp_AssetID.Size = new Size(47, 13);
            this.mp_AssetID.TabIndex = 48;
            this.mp_AssetID.Text = "Asset ID";
            this.lblLoginID.Location = new Point(191, 427);
            this.lblLoginID.Name = "lblLoginID";
            this.lblLoginID.Size = new Size(300, 13);
            this.lblLoginID.TabIndex = 57;
            this.lblLoginID.Text = "Login ID";
            this.lblAssetID.Location = new Point(191, 256);
            this.lblAssetID.Name = "lblAssetID";
            this.lblAssetID.Size = new Size(300, 13);
            this.lblAssetID.TabIndex = 49;
            this.lblAssetID.Text = "n/a";
            this.lblMachineAccount.Location = new Point(191, 408);
            this.lblMachineAccount.Name = "lblMachineAccount";
            this.lblMachineAccount.Size = new Size(300, 13);
            this.lblMachineAccount.TabIndex = 56;
            this.lblMachineAccount.Text = "Machine Account";
            this.mp_FileName.AutoSize = true;
            this.mp_FileName.Location = new Point(24, 275);
            this.mp_FileName.Name = "mp_FileName";
            this.mp_FileName.Size = new Size(91, 13);
            this.mp_FileName.TabIndex = 42;
            this.mp_FileName.Text = "System File Name";
            this.mp_FileExt.AutoSize = true;
            this.mp_FileExt.Location = new Point(24, 313);
            this.mp_FileExt.Name = "mp_FileExt";
            this.mp_FileExt.Size = new Size(72, 13);
            this.mp_FileExt.TabIndex = 50;
            this.mp_FileExt.Text = "File Extension";
            this.lblDomain.Location = new Point(191, 389);
            this.lblDomain.Name = "lblDomain";
            this.lblDomain.Size = new Size(300, 13);
            this.lblDomain.TabIndex = 55;
            this.lblDomain.Text = "Machine Domain";
            this.lbl_DB_FileName.Location = new Point(191, 275);
            this.lbl_DB_FileName.Name = "lbl_DB_FileName";
            this.lbl_DB_FileName.Size = new Size(300, 13);
            this.lbl_DB_FileName.TabIndex = 43;
            this.lbl_DB_FileName.Text = "File Name";
            this.lblFileExt.Location = new Point(191, 313);
            this.lblFileExt.Name = "lblFileExt";
            this.lblFileExt.Size = new Size(300, 13);
            this.lblFileExt.TabIndex = 51;
            this.lblFileExt.Text = "n/a";
            this.lblMachineName.Location = new Point(191, 370);
            this.lblMachineName.Name = "lblMachineName";
            this.lblMachineName.Size = new Size(300, 13);
            this.lblMachineName.TabIndex = 54;
            this.lblMachineName.Text = "Machine Name";
            this.mp_OriginalName.AutoSize = true;
            this.mp_OriginalName.Location = new Point(24, 294);
            this.mp_OriginalName.Name = "mp_OriginalName";
            this.mp_OriginalName.Size = new Size(92, 13);
            this.mp_OriginalName.TabIndex = 52;
            this.mp_OriginalName.Text = "Original File Name";
            this.lblOriginalName.Location = new Point(191, 294);
            this.lblOriginalName.Name = "lblOriginalName";
            this.lblOriginalName.Size = new Size(300, 13);
            this.lblOriginalName.TabIndex = 53;
            this.lblOriginalName.Text = "File";
            this.lblHashCode.AutoSize = true;
            this.lblHashCode.Location = new Point(191, 237);
            this.lblHashCode.Name = "lblHashCode";
            this.lblHashCode.Size = new Size(13, 13);
            this.lblHashCode.TabIndex = 29;
            this.lblHashCode.Text = "0";
            this.mp_HashCode.AutoSize = true;
            this.mp_HashCode.Location = new Point(24, 237);
            this.mp_HashCode.Name = "mp_HashCode";
            this.mp_HashCode.Size = new Size(92, 13);
            this.mp_HashCode.TabIndex = 28;
            this.mp_HashCode.Text = "File Security Code";
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new Point(191, 218);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new Size(42, 13);
            this.lblSize.TabIndex = 27;
            this.lblSize.Text = "0 Bytes";
            this.mp_FileSize.AutoSize = true;
            this.mp_FileSize.Location = new Point(24, 218);
            this.mp_FileSize.Name = "mp_FileSize";
            this.mp_FileSize.Size = new Size(46, 13);
            this.mp_FileSize.TabIndex = 26;
            this.mp_FileSize.Text = "File Size";
            this.lblUploaded.AutoSize = true;
            this.lblUploaded.Location = new Point(191, 199);
            this.lblUploaded.Name = "lblUploaded";
            this.lblUploaded.Size = new Size(110, 13);
            this.lblUploaded.TabIndex = 25;
            this.lblUploaded.Text = "00/00/0000 00:00:00";
            this.mp_FileUploaded.AutoSize = true;
            this.mp_FileUploaded.Location = new Point(24, 199);
            this.mp_FileUploaded.Name = "mp_FileUploaded";
            this.mp_FileUploaded.Size = new Size(72, 13);
            this.mp_FileUploaded.TabIndex = 24;
            this.mp_FileUploaded.Text = "File Uploaded";
            this.mp_FileDate.AutoSize = true;
            this.mp_FileDate.Location = new Point(24, 180);
            this.mp_FileDate.Name = "mp_FileDate";
            this.mp_FileDate.Size = new Size(49, 13);
            this.mp_FileDate.TabIndex = 22;
            this.mp_FileDate.Text = "File Date";
            this.lblTimestamp.AutoSize = true;
            this.lblTimestamp.Location = new Point(191, 180);
            this.lblTimestamp.Name = "lblTimestamp";
            this.lblTimestamp.Size = new Size(110, 13);
            this.lblTimestamp.TabIndex = 23;
            this.lblTimestamp.Text = "00/00/0000 00:00:00";
            this.picImage.BackColor = Color.Black;
            this.picImage.Location = new Point(24, 3);
            this.picImage.Name = "picImage";
            this.picImage.Size = new Size(146, 93);
            this.picImage.SizeMode = PictureBoxSizeMode.StretchImage;
            this.picImage.TabIndex = 75;
            this.picImage.TabStop = false;
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.btnClose);
            this.HeaderPanel.Controls.Add(this.lbl_MediaProfile);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(527, 45);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.btnClose.AllowAnimations = true;
            this.btnClose.BackColor = Color.Transparent;
            this.btnClose.Dock = DockStyle.Right;
            this.btnClose.Image = Properties.Resources.close;
            this.btnClose.Location = new Point(482, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaintBorder = false;
            this.btnClose.PaintDefaultBorder = false;
            this.btnClose.PaintDefaultFill = false;
            this.btnClose.RoundedCornersMask = 15;
            this.btnClose.RoundedCornersRadius = 0;
            this.btnClose.Size = new Size(45, 45);
            this.btnClose.TabIndex = 1;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.lbl_MediaProfile.AutoSize = true;
            this.lbl_MediaProfile.BackColor = Color.Transparent;
            this.lbl_MediaProfile.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_MediaProfile.ForeColor = Color.White;
            this.lbl_MediaProfile.Location = new Point(4, 4);
            this.lbl_MediaProfile.Name = "lbl_MediaProfile";
            this.lbl_MediaProfile.Size = new Size(113, 20);
            this.lbl_MediaProfile.TabIndex = 0;
            this.lbl_MediaProfile.Text = "Media Profile";
            this.lbl_MediaProfile.MouseDown += new MouseEventHandler(this.lbl_MediaProfile_MouseDown);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(527, 509);
            base.Controls.Add(this.BodyPanel);
            base.Controls.Add(this.HeaderPanel);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "DataProfile";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "DataProfile";
            base.Load += new EventHandler(this.DataProfile_Load);
            this.BodyPanel.ResumeLayout(false);
            this.BodyPanel.PerformLayout();
            ((ISupportInitialize)this.picImage).EndInit();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            base.ResumeLayout(false);
        }
    }
}
