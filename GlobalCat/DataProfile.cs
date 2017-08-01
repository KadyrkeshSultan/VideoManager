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
            lblTimestamp.Text = string.Format("GlobalCat_DatProf_1", dRecord.FileTimestamp);
            lblUploaded.Text = string.Format("GlobalCat_DatProf_2", dRecord.FileAddedTimestamp);
            lblSize.Text = string.Format("GlobalCat_DatProf_3", dRecord.FileSize);
            lblHashCode.Text = string.Format("GlobalCat_DatProf_4", dRecord.FileHashCode);
            lblGPS.Text = string.IsNullOrEmpty(dRecord.GPS) ? "GlobalCat_DatProf_5" : dRecord.GPS;
            lblAssetID.Text = dRecord.TrackingID == Guid.Empty ? "GlobalCat_DatProf_6" : dRecord.TrackingID.ToString();
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
            lblCount.Text = string.Format("GlobalCat_DatProf_7", FileCountID);
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
            this.SuspendLayout();
            this.BodyPanel.BackColor = Color.White;
            this.BodyPanel.BorderStyle = BorderStyle.FixedSingle;
            this.BodyPanel.Controls.Add((Control)this.lblCount);
            this.BodyPanel.Controls.Add((Control)this.picImage);
            this.BodyPanel.Controls.Add((Control)this.chk_IsIndefinite);
            this.BodyPanel.Controls.Add((Control)this.chk_IsEvidence);
            this.BodyPanel.Controls.Add((Control)this.mp_CAD);
            this.BodyPanel.Controls.Add((Control)this.cadTxt);
            this.BodyPanel.Controls.Add((Control)this.mp_RMS);
            this.BodyPanel.Controls.Add((Control)this.rmsTxt);
            this.BodyPanel.Controls.Add((Control)this.mp_ShortDesc);
            this.BodyPanel.Controls.Add((Control)this.txtShortDesc);
            this.BodyPanel.Controls.Add((Control)this.mp_SetID);
            this.BodyPanel.Controls.Add((Control)this.txtSetID);
            this.BodyPanel.Controls.Add((Control)this.RatingCtrl);
            this.BodyPanel.Controls.Add((Control)this.lblGPS);
            this.BodyPanel.Controls.Add((Control)this.mp_SourcePath);
            this.BodyPanel.Controls.Add((Control)this.mp_GPS);
            this.BodyPanel.Controls.Add((Control)this.mp_FileLoginID);
            this.BodyPanel.Controls.Add((Control)this.mp_FileOwner);
            this.BodyPanel.Controls.Add((Control)this.mp_FileDomainAccount);
            this.BodyPanel.Controls.Add((Control)this.mp_FileDomain);
            this.BodyPanel.Controls.Add((Control)this.lblAccountName);
            this.BodyPanel.Controls.Add((Control)this.mp_FileMachineName);
            this.BodyPanel.Controls.Add((Control)this.lblSourcePath);
            this.BodyPanel.Controls.Add((Control)this.mp_AssetID);
            this.BodyPanel.Controls.Add((Control)this.lblLoginID);
            this.BodyPanel.Controls.Add((Control)this.lblAssetID);
            this.BodyPanel.Controls.Add((Control)this.lblMachineAccount);
            this.BodyPanel.Controls.Add((Control)this.mp_FileName);
            this.BodyPanel.Controls.Add((Control)this.mp_FileExt);
            this.BodyPanel.Controls.Add((Control)this.lblDomain);
            this.BodyPanel.Controls.Add((Control)this.lbl_DB_FileName);
            this.BodyPanel.Controls.Add((Control)this.lblFileExt);
            this.BodyPanel.Controls.Add((Control)this.lblMachineName);
            this.BodyPanel.Controls.Add((Control)this.mp_OriginalName);
            this.BodyPanel.Controls.Add((Control)this.lblOriginalName);
            this.BodyPanel.Controls.Add((Control)this.lblHashCode);
            this.BodyPanel.Controls.Add((Control)this.mp_HashCode);
            this.BodyPanel.Controls.Add((Control)this.lblSize);
            this.BodyPanel.Controls.Add((Control)this.mp_FileSize);
            this.BodyPanel.Controls.Add((Control)this.lblUploaded);
            this.BodyPanel.Controls.Add((Control)this.mp_FileUploaded);
            this.BodyPanel.Controls.Add((Control)this.mp_FileDate);
            this.BodyPanel.Controls.Add((Control)this.lblTimestamp);
            this.BodyPanel.Dock = DockStyle.Fill;
            this.BodyPanel.Location = new Point(0, 45);
            this.BodyPanel.Name = "GlobalCat_DatProf_7";
            this.BodyPanel.Size = new Size(527, 464);
            this.BodyPanel.TabIndex = 1;
            this.lblCount.Anchor = AnchorStyles.Right;
            this.lblCount.BackColor = Color.LightGray;
            this.lblCount.BorderStyle = BorderStyle.FixedSingle;
            this.lblCount.Font = new Font("GlobalCat_DatProf_8", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lblCount.Location = new Point(451, 1);
            this.lblCount.Name = "GlobalCat_DatProf_9";
            this.lblCount.Size = new Size(71, 23);
            this.lblCount.TabIndex = 76;
            this.lblCount.Text = "GlobalCat_DatProf_10";
            this.lblCount.TextAlign = ContentAlignment.MiddleCenter;
            this.chk_IsIndefinite.BackColor = Color.Transparent;
            this.chk_IsIndefinite.Enabled = false;
            this.chk_IsIndefinite.Location = new Point(191, 62);
            this.chk_IsIndefinite.Name = "GlobalCat_DatProf_11";
            this.chk_IsIndefinite.Size = new Size(184, 24);
            this.chk_IsIndefinite.TabIndex = 74;
            this.chk_IsIndefinite.Text = "GlobalCat_DatProf_12";
            this.chk_IsIndefinite.UseVisualStyleBackColor = false;
            this.chk_IsIndefinite.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.chk_IsEvidence.BackColor = Color.Transparent;
            this.chk_IsEvidence.Enabled = false;
            this.chk_IsEvidence.Location = new Point(191, 32);
            this.chk_IsEvidence.Name = "GlobalCat_DatProf_13";
            this.chk_IsEvidence.Size = new Size(104, 24);
            this.chk_IsEvidence.TabIndex = 73;
            this.chk_IsEvidence.Text = "GlobalCat_DatProf_14";
            this.chk_IsEvidence.UseVisualStyleBackColor = false;
            this.chk_IsEvidence.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.mp_CAD.AutoSize = true;
            this.mp_CAD.Location = new Point(24, 160);
            this.mp_CAD.Name = "GlobalCat_DatProf_15";
            this.mp_CAD.Size = new Size(69, 13);
            this.mp_CAD.TabIndex = 72;
            this.mp_CAD.Text = "GlobalCat_DatProf_16";
            this.cadTxt.AutoSize = true;
            this.cadTxt.Location = new Point(191, 160);
            this.cadTxt.Name = "GlobalCat_DatProf_17";
            this.cadTxt.Size = new Size(69, 13);
            this.cadTxt.TabIndex = 71;
            this.cadTxt.Text = "GlobalCat_DatProf_18";
            this.mp_RMS.AutoSize = true;
            this.mp_RMS.Location = new Point(24, 141);
            this.mp_RMS.Name = "GlobalCat_DatProf_19";
            this.mp_RMS.Size = new Size(71, 13);
            this.mp_RMS.TabIndex = 70;
            this.mp_RMS.Text = "GlobalCat_DatProf_20";
            this.rmsTxt.AutoSize = true;
            this.rmsTxt.Location = new Point(191, 141);
            this.rmsTxt.Name = "GlobalCat_DatProf_21";
            this.rmsTxt.Size = new Size(71, 13);
            this.rmsTxt.TabIndex = 69;
            this.rmsTxt.Text = "GlobalCat_DatProf_22";
            this.mp_ShortDesc.AutoSize = true;
            this.mp_ShortDesc.Location = new Point(24, 122);
            this.mp_ShortDesc.Name = "GlobalCat_DatProf_23";
            this.mp_ShortDesc.Size = new Size(60, 13);
            this.mp_ShortDesc.TabIndex = 68;
            this.mp_ShortDesc.Text = "GlobalCat_DatProf_24";
            this.txtShortDesc.AutoSize = true;
            this.txtShortDesc.Location = new Point(191, 122);
            this.txtShortDesc.Name = "GlobalCat_DatProf_25";
            this.txtShortDesc.Size = new Size(60, 13);
            this.txtShortDesc.TabIndex = 67;
            this.txtShortDesc.Text = "GlobalCat_DatProf_26";
            this.mp_SetID.AutoSize = true;
            this.mp_SetID.Location = new Point(24, 103);
            this.mp_SetID.Name = "GlobalCat_DatProf_27";
            this.mp_SetID.Size = new Size(37, 13);
            this.mp_SetID.TabIndex = 66;
            this.mp_SetID.Text = "GlobalCat_DatProf_28";
            this.txtSetID.AutoSize = true;
            this.txtSetID.Location = new Point(191, 103);
            this.txtSetID.Name = "GlobalCat_DatProf_29";
            this.txtSetID.Size = new Size(37, 13);
            this.txtSetID.TabIndex = 65;
            this.txtSetID.Text = "GlobalCat_DatProf_30";
            this.RatingCtrl.Enabled = false;
            this.RatingCtrl.Location = new Point(191, 3);
            this.RatingCtrl.Name = "GlobalCat_DatProf_31";
            this.RatingCtrl.Shape = vRatingControlShapes.Star;
            this.RatingCtrl.Size = new Size(144, 23);
            this.RatingCtrl.TabIndex = 64;
            this.RatingCtrl.Text = "GlobalCat_DatProf_32";
            this.RatingCtrl.Value = 0.0f;
            this.RatingCtrl.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lblGPS.Location = new Point(191, 332);
            this.lblGPS.Name = "GlobalCat_DatProf_33";
            this.lblGPS.Size = new Size(300, 13);
            this.lblGPS.TabIndex = 45;
            this.lblGPS.Text = "GlobalCat_DatProf_34";
            this.mp_SourcePath.AutoSize = true;
            this.mp_SourcePath.Location = new Point(24, 446);
            this.mp_SourcePath.Name = "GlobalCat_DatProf_35";
            this.mp_SourcePath.Size = new Size(85, 13);
            this.mp_SourcePath.TabIndex = 63;
            this.mp_SourcePath.Text = "GlobalCat_DatProf_36";
            this.mp_GPS.AutoSize = true;
            this.mp_GPS.Location = new Point(24, 332);
            this.mp_GPS.Name = "GlobalCat_DatProf_37";
            this.mp_GPS.Size = new Size(73, 13);
            this.mp_GPS.TabIndex = 44;
            this.mp_GPS.Text = "GlobalCat_DatProf_38";
            this.mp_FileLoginID.AutoSize = true;
            this.mp_FileLoginID.Location = new Point(24, 427);
            this.mp_FileLoginID.Name = "GlobalCat_DatProf_39";
            this.mp_FileLoginID.Size = new Size(90, 13);
            this.mp_FileLoginID.TabIndex = 62;
            this.mp_FileLoginID.Text = "GlobalCat_DatProf_40";
            this.mp_FileOwner.AutoSize = true;
            this.mp_FileOwner.Location = new Point(24, 351);
            this.mp_FileOwner.Name = "GlobalCat_DatProf_41";
            this.mp_FileOwner.Size = new Size(57, 13);
            this.mp_FileOwner.TabIndex = 46;
            this.mp_FileOwner.Text = "GlobalCat_DatProf_42";
            this.mp_FileDomainAccount.AutoSize = true;
            this.mp_FileDomainAccount.Location = new Point(24, 408);
            this.mp_FileDomainAccount.Name = "GlobalCat_DatProf_43";
            this.mp_FileDomainAccount.Size = new Size(86, 13);
            this.mp_FileDomainAccount.TabIndex = 61;
            this.mp_FileDomainAccount.Text = "GlobalCat_DatProf_44";
            this.mp_FileDomain.AutoSize = true;
            this.mp_FileDomain.Location = new Point(24, 389);
            this.mp_FileDomain.Name = "GlobalCat_DatProf_45";
            this.mp_FileDomain.Size = new Size(87, 13);
            this.mp_FileDomain.TabIndex = 60;
            this.mp_FileDomain.Text = "GlobalCat_DatProf_46";
            this.lblAccountName.Location = new Point(191, 351);
            this.lblAccountName.Name = "GlobalCat_DatProf_47";
            this.lblAccountName.Size = new Size(300, 13);
            this.lblAccountName.TabIndex = 47;
            this.lblAccountName.Text = "GlobalCat_DatProf_48";
            this.mp_FileMachineName.AutoSize = true;
            this.mp_FileMachineName.Location = new Point(24, 370);
            this.mp_FileMachineName.Name = "GlobalCat_DatProf_49";
            this.mp_FileMachineName.Size = new Size(79, 13);
            this.mp_FileMachineName.TabIndex = 59;
            this.mp_FileMachineName.Text = "GlobalCat_DatProf_50";
            this.lblSourcePath.Location = new Point(191, 446);
            this.lblSourcePath.Name = "GlobalCat_DatProf_51";
            this.lblSourcePath.Size = new Size(300, 13);
            this.lblSourcePath.TabIndex = 58;
            this.lblSourcePath.Text = "GlobalCat_DatProf_52";
            this.mp_AssetID.AutoSize = true;
            this.mp_AssetID.Location = new Point(24, 256);
            this.mp_AssetID.Name = "GlobalCat_DatProf_53";
            this.mp_AssetID.Size = new Size(47, 13);
            this.mp_AssetID.TabIndex = 48;
            this.mp_AssetID.Text = "GlobalCat_DatProf_54";
            this.lblLoginID.Location = new Point(191, 427);
            this.lblLoginID.Name = "GlobalCat_DatProf_55";
            this.lblLoginID.Size = new Size(300, 13);
            this.lblLoginID.TabIndex = 57;
            this.lblLoginID.Text = "GlobalCat_DatProf_56";
            this.lblAssetID.Location = new Point(191, 256);
            this.lblAssetID.Name = "GlobalCat_DatProf_57";
            this.lblAssetID.Size = new Size(300, 13);
            this.lblAssetID.TabIndex = 49;
            this.lblAssetID.Text = "GlobalCat_DatProf_58";
            this.lblMachineAccount.Location = new Point(191, 408);
            this.lblMachineAccount.Name = "GlobalCat_DatProf_59";
            this.lblMachineAccount.Size = new Size(300, 13);
            this.lblMachineAccount.TabIndex = 56;
            this.lblMachineAccount.Text = "GlobalCat_DatProf_60";
            this.mp_FileName.AutoSize = true;
            this.mp_FileName.Location = new Point(24, 275);
            this.mp_FileName.Name = "GlobalCat_DatProf_61";
            this.mp_FileName.Size = new Size(91, 13);
            this.mp_FileName.TabIndex = 42;
            this.mp_FileName.Text = "GlobalCat_DatProf_62";
            this.mp_FileExt.AutoSize = true;
            this.mp_FileExt.Location = new Point(24, 313);
            this.mp_FileExt.Name = "GlobalCat_DatProf_63";
            this.mp_FileExt.Size = new Size(72, 13);
            this.mp_FileExt.TabIndex = 50;
            this.mp_FileExt.Text = "GlobalCat_DatProf_64";
            this.lblDomain.Location = new Point(191, 389);
            this.lblDomain.Name = "GlobalCat_DatProf_65";
            this.lblDomain.Size = new Size(300, 13);
            this.lblDomain.TabIndex = 55;
            this.lblDomain.Text = "GlobalCat_DatProf_66";
            this.lbl_DB_FileName.Location = new Point(191, 275);
            this.lbl_DB_FileName.Name = "GlobalCat_DatProf_67";
            this.lbl_DB_FileName.Size = new Size(300, 13);
            this.lbl_DB_FileName.TabIndex = 43;
            this.lbl_DB_FileName.Text = "GlobalCat_DatProf_68";
            this.lblFileExt.Location = new Point(191, 313);
            this.lblFileExt.Name = "GlobalCat_DatProf_69";
            this.lblFileExt.Size = new Size(300, 13);
            this.lblFileExt.TabIndex = 51;
            this.lblFileExt.Text = "GlobalCat_DatProf_70";
            this.lblMachineName.Location = new Point(191, 370);
            this.lblMachineName.Name = "GlobalCat_DatProf_71";
            this.lblMachineName.Size = new Size(300, 13);
            this.lblMachineName.TabIndex = 54;
            this.lblMachineName.Text = "GlobalCat_DatProf_72";
            this.mp_OriginalName.AutoSize = true;
            this.mp_OriginalName.Location = new Point(24, 294);
            this.mp_OriginalName.Name = "GlobalCat_DatProf_73";
            this.mp_OriginalName.Size = new Size(92, 13);
            this.mp_OriginalName.TabIndex = 52;
            this.mp_OriginalName.Text = "GlobalCat_DatProf_74";
            this.lblOriginalName.Location = new Point(191, 294);
            this.lblOriginalName.Name = "GlobalCat_DatProf_75";
            this.lblOriginalName.Size = new Size(300, 13);
            this.lblOriginalName.TabIndex = 53;
            this.lblOriginalName.Text = "GlobalCat_DatProf_76";
            this.lblHashCode.AutoSize = true;
            this.lblHashCode.Location = new Point(191, 237);
            this.lblHashCode.Name = "GlobalCat_DatProf_77";
            this.lblHashCode.Size = new Size(13, 13);
            this.lblHashCode.TabIndex = 29;
            this.lblHashCode.Text = "GlobalCat_DatProf_78";
            this.mp_HashCode.AutoSize = true;
            this.mp_HashCode.Location = new Point(24, 237);
            this.mp_HashCode.Name = "GlobalCat_DatProf_79";
            this.mp_HashCode.Size = new Size(92, 13);
            this.mp_HashCode.TabIndex = 28;
            this.mp_HashCode.Text = "GlobalCat_DatProf_80";
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new Point(191, 218);
            this.lblSize.Name = "GlobalCat_DatProf_81";
            this.lblSize.Size = new Size(42, 13);
            this.lblSize.TabIndex = 27;
            this.lblSize.Text = "GlobalCat_DatProf_82";
            this.mp_FileSize.AutoSize = true;
            this.mp_FileSize.Location = new Point(24, 218);
            this.mp_FileSize.Name = "GlobalCat_DatProf_83";
            this.mp_FileSize.Size = new Size(46, 13);
            this.mp_FileSize.TabIndex = 26;
            this.mp_FileSize.Text = "GlobalCat_DatProf_84";
            this.lblUploaded.AutoSize = true;
            this.lblUploaded.Location = new Point(191, 199);
            this.lblUploaded.Name = "GlobalCat_DatProf_85";
            this.lblUploaded.Size = new Size(110, 13);
            this.lblUploaded.TabIndex = 25;
            this.lblUploaded.Text = "GlobalCat_DatProf_86";
            this.mp_FileUploaded.AutoSize = true;
            this.mp_FileUploaded.Location = new Point(24, 199);
            this.mp_FileUploaded.Name = "GlobalCat_DatProf_87";
            this.mp_FileUploaded.Size = new Size(72, 13);
            this.mp_FileUploaded.TabIndex = 24;
            this.mp_FileUploaded.Text = "GlobalCat_DatProf_88";
            this.mp_FileDate.AutoSize = true;
            this.mp_FileDate.Location = new Point(24, 180);
            this.mp_FileDate.Name = "GlobalCat_DatProf_89";
            this.mp_FileDate.Size = new Size(49, 13);
            this.mp_FileDate.TabIndex = 22;
            this.mp_FileDate.Text = "GlobalCat_DatProf_90";
            this.lblTimestamp.AutoSize = true;
            this.lblTimestamp.Location = new Point(191, 180);
            this.lblTimestamp.Name = "GlobalCat_DatProf_91";
            this.lblTimestamp.Size = new Size(110, 13);
            this.lblTimestamp.TabIndex = 23;
            this.lblTimestamp.Text = "GlobalCat_DatProf_92";
            this.picImage.BackColor = Color.Black;
            this.picImage.Location = new Point(24, 3);
            this.picImage.Name = "GlobalCat_DatProf_93";
            this.picImage.Size = new Size(146, 93);
            this.picImage.SizeMode = PictureBoxSizeMode.StretchImage;
            this.picImage.TabIndex = 75;
            this.picImage.TabStop = false;
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add((Control)this.btnClose);
            this.HeaderPanel.Controls.Add((Control)this.lbl_MediaProfile);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "GlobalCat_DatProf_94";
            this.HeaderPanel.Size = new Size(527, 45);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.btnClose.AllowAnimations = true;
            this.btnClose.BackColor = Color.Transparent;
            this.btnClose.Dock = DockStyle.Right;
            this.btnClose.Image = (Image)Properties.Resources.close;
            this.btnClose.Location = new Point(482, 0);
            this.btnClose.Name = "GlobalCat_DatProf_95";
            this.btnClose.PaintBorder = false;
            this.btnClose.PaintDefaultBorder = false;
            this.btnClose.PaintDefaultFill = false;
            this.btnClose.RoundedCornersMask = (byte)15;
            this.btnClose.RoundedCornersRadius = 0;
            this.btnClose.Size = new Size(45, 45);
            this.btnClose.TabIndex = 1;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.lbl_MediaProfile.AutoSize = true;
            this.lbl_MediaProfile.BackColor = Color.Transparent;
            this.lbl_MediaProfile.Font = new Font("GlobalCat_DatProf_96", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lbl_MediaProfile.ForeColor = Color.White;
            this.lbl_MediaProfile.Location = new Point(4, 4);
            this.lbl_MediaProfile.Name = "GlobalCat_DatProf_97";
            this.lbl_MediaProfile.Size = new Size(113, 20);
            this.lbl_MediaProfile.TabIndex = 0;
            this.lbl_MediaProfile.Text = "GlobalCat_DatProf_98";
            this.lbl_MediaProfile.MouseDown += new MouseEventHandler(this.lbl_MediaProfile_MouseDown);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(527, 509);
            this.Controls.Add((Control)this.BodyPanel);
            this.Controls.Add((Control)this.HeaderPanel);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Name = "GlobalCat_DatProf_99";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "GlobalCat_DatProf_100";
            this.Load += new EventHandler(this.DataProfile_Load);
            this.BodyPanel.ResumeLayout(false);
            this.BodyPanel.PerformLayout();
            ((ISupportInitialize)this.picImage).EndInit();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
