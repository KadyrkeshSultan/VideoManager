using AccountCtrl2;
using AppGlobal;
using VideoManager.Properties;
using VMStudio;
using CatalogPanel;
using Cite;
using GlobalCat;
using LibUsbDotNet.DeviceNotify;
using LibUsbDotNet.DeviceNotify.Info;
using ReportMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using Unity;
using USBDetect;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using WinReg;
using WinSparkleDotNet;
using Wolfcom.Vision;
using VMModels.Enums;
using VMInterfaces;
using VMModels.Model;

namespace VideoManager
{
    public class MainForm : Form
    {
        private bool IsDetectCamera;

        private VisionManager _manager;

        private VisionDevice _device;

        private Mutex mutex;

        private string mutexName = "COM.HDPROTECH.C3.CATALOG";

        private WinSparkle wSparkle = new WinSparkle();

        private Guid AcctID = Guid.Empty;

        private bool IsLoggedOn;

        private AcctCtrl AccountTree = new AcctCtrl();

        private CiteLib cLib = new CiteLib();

        private bool IsOpenCenter;

        private bool IsSync = true;

        private string AdmPwd = "312";

        private IDeviceNotifier devNotifier;

        private bool IsCiteCamera;

        private bool IsMounting;

        private int BatteryPercent;

        private int DiskSize;

        private ToolTip tt = new ToolTip();

        private DetectFolder df = new DetectFolder();

        private Catalog cat1;

        private vTabPage tab1;

        private bool IsUpdateEnabled;

        private bool IsAutoUpdate;

        private bool IsReport;

        private ReportForm report;

        private IContainer components;

        private StatusStrip statusStrip1;

        private Panel HeaderPanel;

        private vButton btn_Logout;

        private vButton btn_About;

        private vTabControl MainTabControl;

        private vControlBox vControlBox;

        private PictureBox LogoBox;

        private vButton btnStudio;

        private vButton btnReports;

        private vButton btn_Help;

        private PictureBox USBPic;

        private PictureBox CamPic;

        private vProgressBar Battery;

        private Label lbl_Battery;

        private vButton btnAlertEmails;

        private vButton btnChkUpdates;

        private Panel MenuPanel;

        private ToolStripStatusLabel StatusMsg;

        private vButton btnGlobalSearch;

        private ListBox lstDevices;

        private Panel panel1;

        private TableLayoutPanel tableLayoutPanel1;

        private Panel BatteryPanel;

        public bool IsRegistered
        {
            get;
            set;
        }

        public MainForm()
        {
            try
            {
                this.mutex = Mutex.OpenExisting(this.mutexName);
                MessageBox.Show("C3 Partner Catalog is already running.", "C3 Catalog", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                Environment.Exit(0);
            }
            catch (Exception exception)
            {
                this.mutex = new Mutex(true, this.mutexName);
            }
            Licensing.LicenseContent = "0g0g634655384616543010g0d6aecca0330e04d0e089d90af5b167a1b80|og8GT3jOP3gGXBo1FoQ9xIVER7KGToXOBP07RoWIzh94AgnMYFZlCPEvLHY3/Qm2L+058lx9iRXUKiQct62Spkwf0PI//PUXJ2AW2g03CaAA+r+rGOT6/IEkJAHEcCH/o9l3YDkIObsg2k+ZBCPM3i7WRxU5ANRD0q/AN5AIzuU=";
            this.InitializeComponent();
        }

        private void AccountTab()
        {
            this.MainTabControl.SuspendLayout();
            this.MainTabControl.EnableCloseButtons = true;
            this.MainTabControl.TabPages.Clear();
            this.tab1 = new vTabPage(string.Format(LangCtrl.GetString("tab_MyCatalog", "My Catalog • {0}"), Global.GlobalAccount.ToString()))
            {
                Tag = Guid.Empty,
                EnableCloseButton = false
            };
            this.cat1 = new Catalog()
            {
                Account_ID = Global.GlobalAccount.Id
            };
            this.tab1.Controls.Add(this.cat1);
            this.cat1.EVT_UpdateLanguage += new Catalog.DEL_UpdateLanguage(this.cat1_EVT_UpdateLanguage);
            this.MainTabControl.TabPages.Add(this.tab1);
            this.MainTabControl.ResumeLayout();
            this.MainTabControl.Refresh();
        }

        private void aCtrl_EVT_NodeCallback(object sender, CmdAccountPickerEventArgs e)
        {
            this.vControlBox.CloseDropDown();
            Guid recIdx = e.nodeRec.RecIdx;
            bool flag = false;
            int num = 0;
            this.MainTabControl.SuspendLayout();
            using (IEnumerator<vTabPage> enumerator = this.MainTabControl.TabPages.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        vTabPage current = enumerator.Current;
                        if (current.Tag == null || !current.Tag.Equals(recIdx))
                        {
                            num++;
                        }
                        else
                        {
                            flag = true;
                            current.TabControl.SelectedIndex = num;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (!flag)
            {
                vTabPage _vTabPage = new vTabPage(string.Format("{0} • {1}", e.nodeRec.Name, e.nodeRec.BadgeNumber))
                {
                    Tag = recIdx,
                    EnableCloseButton = true
                };
                Catalog catalog = new Catalog()
                {
                    Account_ID = e.nodeRec.RecIdx
                };
                _vTabPage.Controls.Add(catalog);
                this.MainTabControl.TabPages.Add(_vTabPage);
                _vTabPage.TabControl.SelectedIndex = this.MainTabControl.TabPages.Count - 1;
            }
            this.MainTabControl.ResumeLayout();
        }

        private void AutoUpdate()
        {
            this.wSparkle.Init();
            this.wSparkle.SetAppDetails("HD Protech", "C3 Sentinel Client", Application.ProductVersion);
            if (this.IsUpdateEnabled && this.IsAutoUpdate)
            {
                this.RunUpdate();
            }
        }

        private void btn_About_Click(object sender, EventArgs e)
        {
            try
            {
                (new About()).ShowDialog(this);
            }
            catch (Exception exception)
            {
                string message = exception.Message;
            }
        }

        private void btn_AlertEmails_Click(object sender, EventArgs e)
        {
            (new EmailForm()).ShowDialog(this);
        }

        private void btn_Help_Click(object sender, EventArgs e)
        {
            string str = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Client.chm");
            Help.ShowHelp(this, str);
        }

        private void btn_Logout_Click(object sender, EventArgs e)
        {
            if (this.LogoutPwd())
            {
                return;
            }
            if (this.IsRegistered)
            {
                this.RegDeviceEvent(false);
                this.USBPic.Visible = false;
            }
            this.Log_Logout();
            this.MainTabControl.Visible = false;
            this.CatalogLogin();
            this.MainTabControl.Visible = true;
            this.btnStudio.Visible = false;
            if (Global.GlobalAccount.SystemRole == SYSTEM_ROLE.ADMIN && Global.IsRights(Global.RightsProfile, UserRights.SYSTEM))
            {
                this.btnStudio.Visible = true;
            }
        }

        private void btnChkUpdates_Click(object sender, EventArgs e)
        {
            this.RunUpdate();
        }

        private void btnGlobalSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.IsRegistered)
                {
                    this.USBPic.Visible = false;
                    this.RegDeviceEvent(false);
                }
                Global.Log("OPEN", LangCtrl.GetString("log_OpenGlobalSearch", "Open Global Catalog For Search"));
                GlobalCatForm globalCatForm = new GlobalCatForm();
                if (globalCatForm != null)
                {
                    globalCatForm.ShowDialog(this);
                }
                Global.Log("CLOSE", LangCtrl.GetString("log_CloseGlobalSearch", "Open Global Catalog For Search"));
                if (!this.IsRegistered)
                {
                    this.RegDeviceEvent(true);
                    this.USBPic.Visible = true;
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            if (this.report != null && this.IsReport)
            {
                this.report.Focus();
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            this.report = new ReportForm();
            this.report.EVT_CloseReport -= new ReportForm.DEL_CloseReport(this.report_EVT_CloseReport);
            this.report.EVT_CloseReport += new ReportForm.DEL_CloseReport(this.report_EVT_CloseReport);
            this.IsReport = true;
            this.report.Show();
            this.Cursor = Cursors.Default;
        }

        private void btnStudio_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.IsRegistered)
                {
                    this.USBPic.Visible = false;
                    this.RegDeviceEvent(false);
                    this.cLib.RegDeviceEvent(false);
                    this.IsDetectCamera = false;
                }
                this.Cursor = Cursors.WaitCursor;
                (new VMMgr()).ShowDialog(this);
                this.Cursor = Cursors.Default;
                if (!this.IsRegistered)
                {
                    this.RegDeviceEvent(true);
                    this.USBPic.Visible = true;
                    this.cLib.RegDeviceEvent(true);
                    this.IsDetectCamera = true;
                }
                this.LoadGlobalConfig();
                this.SecurityProfile();
            }
            catch (Exception exception)
            {
                string message = exception.Message;
            }
        }

        private void cat1_EVT_UpdateLanguage()
        {
            this.SetLanguage();
            this.tab1.Text = string.Format(LangCtrl.GetString("tab_MyCatalog", "My Catalog • {0}"), Global.GlobalAccount.ToString());
        }

        private void CatalogLogin()
        {
            try
            {
                this.IsLoggedOn = false;
                if ((new Login()).ShowDialog(this) == DialogResult.Cancel)
                {
                    this.KillPID();
                }
                if (this.AcctID != Global.GlobalAccount.Id)
                {
                    if (Global.GlobalAccount.SystemRole == SYSTEM_ROLE.ADMIN)
                    {
                        this.btnStudio.Visible = true;
                    }
                    this.AcctID = Global.GlobalAccount.Id;
                    this.AccountTree.InitTree();
                    this.AccountTab();
                }
                this.CheckGlobalCatalogRights();
                if (!Global.GlobalAccount.IsGroupOverride)
                {
                    int rightsProfile = Global.RightsProfile;
                    using (RPM_Groups rPMGroup = new RPM_Groups())
                    {
                        List<AccountGroup> groupsByAccount = rPMGroup.GetGroupsByAccount(Global.GlobalAccount.Id);
                        if (groupsByAccount.Count > 0)
                        {
                            foreach (AccountGroup accountGroup in groupsByAccount)
                            {
                                if (!accountGroup.IsEnabled)
                                {
                                    continue;
                                }
                                Global.RightsProfile = rightsProfile & accountGroup.ApplicationRights;
                            }
                        }
                    }
                }
                this.vControlBox.Visible = false;
                this.LoadGlobalConfig();
                this.SecurityProfile();
                this.AutoUpdate();
                this.IsLoggedOn = true;
                if (!this.IsRegistered)
                {
                    this.RegDeviceEvent(true);
                    this.USBPic.Visible = true;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void CheckGlobalCatalogRights()
        {
            this.btnGlobalSearch.Visible = false;
            if ((Global.GlobalAccount.SystemRole == SYSTEM_ROLE.ADMIN || Global.GlobalAccount.SystemRole == SYSTEM_ROLE.SUPER) && Global.IsRights(Global.RightsProfile, UserRights.GLOBAL_CAT))
            {
                this.btnGlobalSearch.Visible = true;
            }
        }

        private void CiteCameraParameters()
        {
            using (RPM_GlobalConfig rPMGlobalConfig = new RPM_GlobalConfig())
            {
                GlobalConfig configRecord = rPMGlobalConfig.GetConfigRecord("CITE_SYNC_TIME");
                if (configRecord != null)
                {
                    this.IsSync = Convert.ToBoolean(configRecord.Value);
                }
                configRecord = rPMGlobalConfig.GetConfigRecord("CITE_ADMINPWD");
                if (configRecord != null)
                {
                    this.AdmPwd = CryptoIO.Decrypt(configRecord.Value);
                }
            }
        }

        private void cLib_EVT_DevActionCallback(object sender, CmdCiteEventArgs e)
        {
            switch (e.action)
            {
                case DEV_ACTIONS.CONNECTED:
                case DEV_ACTIONS.NO_ACTION:
                    {
                        return;
                    }
                case DEV_ACTIONS.DISCONNECTED:
                    {
                        if (this.IsMounting)
                        {
                            return;
                        }
                        this.ShowBatteryStats(false);
                        return;
                    }
                case DEV_ACTIONS.IS_CITE:
                    {
                        this.IsMounting = true;
                        this.IsCiteCamera = true;
                        this.CamPic.Visible = true;
                        this.StatusMsg.Text = "Cite Camera Connected. Mounting Camera...";
                        if (this.IsSync)
                        {
                            this.cLib.SyncCameraTime = true;
                        }
                        this.cLib.CheckAdminPassword(this.AdmPwd, true);
                        return;
                    }
                case DEV_ACTIONS.IS_USB:
                    {
                        this.IsMounting = false;
                        this.StatusMsg.Text = "Camera Device Mounted...";
                        this.ShowBatteryStats(true);
                        return;
                    }
                default:
                    {
                        return;
                    }
            }
        }

        private void CommandArgs()
        {
            if ((int)Environment.GetCommandLineArgs().Length > 1 && Environment.GetCommandLineArgs()[1].ToUpper().Equals("-DB"))
            {
                RegDB regDB = new RegDB();
                regDB.LoadProfile();
                regDB.ShowDialog(this);
            }
        }

        private void ConnectDevice()
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new MethodInvoker(this.ConnectDevice));
            }
            this._device = this.lstDevices.SelectedItem as VisionDevice;
        }

        private void DeviceMounted(string drive)
        {
        }

        private void df_EVT_FolderScan(string folder)
        {
            base.BeginInvoke(new MethodInvoker(() => {
                if (!string.IsNullOrEmpty(folder))
                {
                    this.df.EVT_FolderScan -= new DetectFolder.DEL_FolderScan(this.df_EVT_FolderScan);
                    (new Download(folder, Global.GlobalAccount.Id)).ShowDialog(this);
                    this.CamPic.Visible = false;
                    this.Battery.Visible = false;
                    this.lbl_Battery.Visible = false;
                    this.StatusMsg.Text = string.Empty;
                }
            }));
        }

        private void DisconnectDevice()
        {
            this._device = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Global_EVT_DeviceDetection(bool on_off)
        {
            this.RegDeviceEvent(on_off);
        }

        private void InitGlobalData()
        {
            Global.LockLogin = false;
            VMGlobal.IPAddress = (Network.IPAddress());
            Global.IPAddress = (VMGlobal.IPAddress);
            VMGlobal.MachineID = FingerPrint.Value();
            Global.MachineID = VMGlobal.MachineID;
            VMGlobal.DomainName = Environment.UserDomainName;
            VMGlobal.MachineAccount = Environment.UserName;
            VMGlobal.MachineName = Environment.MachineName;
            this.LoadGlobalConfig();
            this.SetLanguage();
        }

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MainForm));
            this.statusStrip1 = new StatusStrip();
            this.StatusMsg = new ToolStripStatusLabel();
            this.MainTabControl = new vTabControl();
            this.HeaderPanel = new Panel();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.LogoBox = new PictureBox();
            this.USBPic = new PictureBox();
            this.CamPic = new PictureBox();
            this.BatteryPanel = new Panel();
            this.Battery = new vProgressBar();
            this.lbl_Battery = new Label();
            this.lstDevices = new ListBox();
            this.MenuPanel = new Panel();
            this.btnChkUpdates = new vButton();
            this.panel1 = new Panel();
            this.btn_Help = new vButton();
            this.btnAlertEmails = new vButton();
            this.btn_About = new vButton();
            this.btnGlobalSearch = new vButton();
            this.btnStudio = new vButton();
            this.btnReports = new vButton();
            this.btn_Logout = new vButton();
            this.vControlBox = new vControlBox();
            this.statusStrip1.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((ISupportInitialize)this.LogoBox).BeginInit();
            ((ISupportInitialize)this.USBPic).BeginInit();
            ((ISupportInitialize)this.CamPic).BeginInit();
            this.BatteryPanel.SuspendLayout();
            this.Battery.SuspendLayout();
            this.MenuPanel.SuspendLayout();
            this.btnChkUpdates.SuspendLayout();
            base.SuspendLayout();
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.StatusMsg });
            this.statusStrip1.Location = new Point(0, 540);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(784, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            this.StatusMsg.Name = "StatusMsg";
            this.StatusMsg.Size = new Size(0, 17);
            this.MainTabControl.AllowAnimations = true;
            this.MainTabControl.Dock = DockStyle.Fill;
            this.MainTabControl.Location = new Point(0, 80);
            this.MainTabControl.Margin = new Padding(0);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.Padding = new Padding(0, 45, 0, 0);
            this.MainTabControl.Size = new Size(784, 460);
            this.MainTabControl.TabAlignment = vTabPageAlignment.Top;
            this.MainTabControl.TabIndex = 3;
            this.MainTabControl.TabsAreaBackColor = Color.FromArgb(89, 89, 89);
            this.MainTabControl.TabsInitialOffset = 5;
            this.MainTabControl.TabsShape = TabsShape.VisualStudio;
            this.MainTabControl.VIBlendTheme = VIBLEND_THEME.NERO;
            this.MainTabControl.Visible = false;
            this.MainTabControl.SelectedIndexChanged += new EventHandler(this.MainTabControl_SelectedIndexChanged);
            this.HeaderPanel.BackgroundImage = Properties.Resources.header;
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.tableLayoutPanel1);
            this.HeaderPanel.Controls.Add(this.lstDevices);
            this.HeaderPanel.Controls.Add(this.MenuPanel);
            this.HeaderPanel.Controls.Add(this.vControlBox);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(784, 80);
            this.HeaderPanel.TabIndex = 0;
            this.tableLayoutPanel1.BackColor = Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 8f));
            this.tableLayoutPanel1.Controls.Add(this.LogoBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.USBPic, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.CamPic, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.BatteryPanel, 0, 0);
            this.tableLayoutPanel1.Dock = DockStyle.Right;
            this.tableLayoutPanel1.Location = new Point(528, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            this.tableLayoutPanel1.Size = new Size(256, 80);
            this.tableLayoutPanel1.TabIndex = 14;
            this.LogoBox.BackColor = Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.LogoBox, 4);
            this.LogoBox.Cursor = Cursors.Hand;
            this.LogoBox.Dock = DockStyle.Fill;
            this.LogoBox.Image = Properties.Resources.logo;
            this.LogoBox.Location = new Point(0, 40);
            this.LogoBox.Margin = new Padding(0);
            this.LogoBox.Name = "LogoBox";
            this.LogoBox.Size = new Size(248, 40);
            this.LogoBox.SizeMode = PictureBoxSizeMode.CenterImage;
            this.LogoBox.TabIndex = 7;
            this.LogoBox.TabStop = false;
            this.LogoBox.Click += new EventHandler(this.LogoBox_Click);
            this.USBPic.BackColor = Color.Transparent;
            this.USBPic.Dock = DockStyle.Fill;
            this.USBPic.Image = Properties.Resources.usb1;
            this.USBPic.Location = new Point(189, 3);
            this.USBPic.Name = "USBPic";
            this.USBPic.Size = new Size(56, 34);
            this.USBPic.SizeMode = PictureBoxSizeMode.CenterImage;
            this.USBPic.TabIndex = 9;
            this.USBPic.TabStop = false;
            this.USBPic.DoubleClick += new EventHandler(this.USBPic_DoubleClick);
            this.CamPic.BackColor = Color.Transparent;
            this.CamPic.Dock = DockStyle.Fill;
            this.CamPic.Image = Properties.Resources.camlens2;
            this.CamPic.Location = new Point(127, 3);
            this.CamPic.Name = "CamPic";
            this.CamPic.Size = new Size(56, 34);
            this.CamPic.SizeMode = PictureBoxSizeMode.CenterImage;
            this.CamPic.TabIndex = 10;
            this.CamPic.TabStop = false;
            this.CamPic.Visible = false;
            this.tableLayoutPanel1.SetColumnSpan(this.BatteryPanel, 2);
            this.BatteryPanel.Controls.Add(this.Battery);
            this.BatteryPanel.Location = new Point(3, 3);
            this.BatteryPanel.Name = "BatteryPanel";
            this.BatteryPanel.Size = new Size(118, 34);
            this.BatteryPanel.TabIndex = 11;
            this.Battery.Anchor = AnchorStyles.Left;
            this.Battery.BackColor = Color.Transparent;
            this.Battery.Controls.Add(this.lbl_Battery);
            this.Battery.Location = new Point(0, 8);
            this.Battery.Name = "Battery";
            this.Battery.RoundedCornersMask = 15;
            this.Battery.RoundedCornersRadius = 0;
            this.Battery.Size = new Size(126, 23);
            this.Battery.TabIndex = 11;
            this.Battery.Value = 0;
            this.Battery.VIBlendTheme = VIBLEND_THEME.OFFICE2010BLACK;
            this.Battery.Visible = false;
            this.lbl_Battery.Anchor = AnchorStyles.Left;
            this.lbl_Battery.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_Battery.ForeColor = Color.White;
            this.lbl_Battery.Location = new Point(-4, 0);
            this.lbl_Battery.Name = "lbl_Battery";
            this.lbl_Battery.Size = new Size(120, 23);
            this.lbl_Battery.TabIndex = 0;
            this.lbl_Battery.Text = "0%";
            this.lbl_Battery.TextAlign = ContentAlignment.MiddleCenter;
            this.lbl_Battery.Visible = false;
            this.lstDevices.FormattingEnabled = true;
            this.lstDevices.Location = new Point(348, 6);
            this.lstDevices.Name = "lstDevices";
            this.lstDevices.Size = new Size(44, 30);
            this.lstDevices.TabIndex = 8;
            this.lstDevices.Visible = false;
            this.MenuPanel.BackColor = Color.Transparent;
            this.MenuPanel.Controls.Add(this.btnChkUpdates);
            this.MenuPanel.Controls.Add(this.btn_Help);
            this.MenuPanel.Controls.Add(this.btnAlertEmails);
            this.MenuPanel.Controls.Add(this.btn_About);
            this.MenuPanel.Controls.Add(this.btnGlobalSearch);
            this.MenuPanel.Controls.Add(this.btnStudio);
            this.MenuPanel.Controls.Add(this.btnReports);
            this.MenuPanel.Controls.Add(this.btn_Logout);
            this.MenuPanel.Location = new Point(12, 6);
            this.MenuPanel.Name = "MenuPanel";
            this.MenuPanel.Size = new Size(380, 40);
            this.MenuPanel.TabIndex = 13;
            this.btnChkUpdates.AllowAnimations = true;
            this.btnChkUpdates.BackColor = Color.Transparent;
            this.btnChkUpdates.Controls.Add(this.panel1);
            this.btnChkUpdates.Dock = DockStyle.Left;
            this.btnChkUpdates.Image = Properties.Resources.sys_update;
            this.btnChkUpdates.Location = new Point(280, 0);
            this.btnChkUpdates.Name = "btnChkUpdates";
            this.btnChkUpdates.PaintBorder = false;
            this.btnChkUpdates.PaintDefaultBorder = false;
            this.btnChkUpdates.PaintDefaultFill = false;
            this.btnChkUpdates.RoundedCornersMask = 15;
            this.btnChkUpdates.RoundedCornersRadius = 0;
            this.btnChkUpdates.Size = new Size(40, 40);
            this.btnChkUpdates.TabIndex = 7;
            this.btnChkUpdates.UseVisualStyleBackColor = false;
            this.btnChkUpdates.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnChkUpdates.Click += new EventHandler(this.btnChkUpdates_Click);
            this.panel1.Location = new Point(6, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(200, 31);
            this.panel1.TabIndex = 15;
            this.btn_Help.AllowAnimations = true;
            this.btn_Help.BackColor = Color.Transparent;
            this.btn_Help.Dock = DockStyle.Left;
            this.btn_Help.Image = Properties.Resources.help;
            this.btn_Help.Location = new Point(240, 0);
            this.btn_Help.Name = "btn_Help";
            this.btn_Help.PaintBorder = false;
            this.btn_Help.PaintDefaultBorder = false;
            this.btn_Help.PaintDefaultFill = false;
            this.btn_Help.RoundedCornersMask = 15;
            this.btn_Help.RoundedCornersRadius = 0;
            this.btn_Help.Size = new Size(40, 40);
            this.btn_Help.TabIndex = 6;
            this.btn_Help.UseVisualStyleBackColor = false;
            this.btn_Help.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Help.Click += new EventHandler(this.btn_Help_Click);
            this.btnAlertEmails.AllowAnimations = true;
            this.btnAlertEmails.BackColor = Color.Transparent;
            this.btnAlertEmails.Dock = DockStyle.Left;
            this.btnAlertEmails.Image = Properties.Resources.emails;
            this.btnAlertEmails.Location = new Point(200, 0);
            this.btnAlertEmails.Name = "btnAlertEmails";
            this.btnAlertEmails.PaintBorder = false;
            this.btnAlertEmails.PaintDefaultBorder = false;
            this.btnAlertEmails.PaintDefaultFill = false;
            this.btnAlertEmails.RoundedCornersMask = 15;
            this.btnAlertEmails.RoundedCornersRadius = 0;
            this.btnAlertEmails.Size = new Size(40, 40);
            this.btnAlertEmails.TabIndex = 5;
            this.btnAlertEmails.UseVisualStyleBackColor = false;
            this.btnAlertEmails.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnAlertEmails.Visible = false;
            this.btnAlertEmails.Click += new EventHandler(this.btn_AlertEmails_Click);
            this.btn_About.AllowAnimations = true;
            this.btn_About.BackColor = Color.Transparent;
            this.btn_About.Dock = DockStyle.Left;
            this.btn_About.Image = Properties.Resources.about32;
            this.btn_About.Location = new Point(160, 0);
            this.btn_About.Name = "btn_About";
            this.btn_About.PaintBorder = false;
            this.btn_About.PaintDefaultBorder = false;
            this.btn_About.PaintDefaultFill = false;
            this.btn_About.RoundedCornersMask = 15;
            this.btn_About.RoundedCornersRadius = 0;
            this.btn_About.Size = new Size(40, 40);
            this.btn_About.TabIndex = 4;
            this.btn_About.UseVisualStyleBackColor = false;
            this.btn_About.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_About.Click += new EventHandler(this.btn_About_Click);
            this.btnGlobalSearch.AllowAnimations = true;
            this.btnGlobalSearch.BackColor = Color.Transparent;
            this.btnGlobalSearch.Dock = DockStyle.Left;
            this.btnGlobalSearch.Image = Properties.Resources.globalsearch;
            this.btnGlobalSearch.Location = new Point(120, 0);
            this.btnGlobalSearch.Name = "btnGlobalSearch";
            this.btnGlobalSearch.PaintBorder = false;
            this.btnGlobalSearch.PaintDefaultBorder = false;
            this.btnGlobalSearch.PaintDefaultFill = false;
            this.btnGlobalSearch.RoundedCornersMask = 15;
            this.btnGlobalSearch.RoundedCornersRadius = 0;
            this.btnGlobalSearch.Size = new Size(40, 40);
            this.btnGlobalSearch.TabIndex = 3;
            this.btnGlobalSearch.UseVisualStyleBackColor = false;
            this.btnGlobalSearch.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnGlobalSearch.Visible = false;
            this.btnGlobalSearch.Click += new EventHandler(this.btnGlobalSearch_Click);
            this.btnStudio.AllowAnimations = true;
            this.btnStudio.BackColor = Color.Transparent;
            this.btnStudio.Dock = DockStyle.Left;
            this.btnStudio.Image = Properties.Resources.settings;
            this.btnStudio.Location = new Point(80, 0);
            this.btnStudio.Name = "btnStudio";
            this.btnStudio.PaintBorder = false;
            this.btnStudio.PaintDefaultBorder = false;
            this.btnStudio.PaintDefaultFill = false;
            this.btnStudio.RoundedCornersMask = 15;
            this.btnStudio.RoundedCornersRadius = 0;
            this.btnStudio.Size = new Size(40, 40);
            this.btnStudio.TabIndex = 2;
            this.btnStudio.UseVisualStyleBackColor = false;
            this.btnStudio.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnStudio.Visible = false;
            this.btnStudio.Click += new EventHandler(this.btnStudio_Click);
            this.btnReports.AllowAnimations = true;
            this.btnReports.BackColor = Color.Transparent;
            this.btnReports.Dock = DockStyle.Left;
            this.btnReports.Image = Properties.Resources.reports;
            this.btnReports.Location = new Point(40, 0);
            this.btnReports.Name = "btnReports";
            this.btnReports.PaintBorder = false;
            this.btnReports.PaintDefaultBorder = false;
            this.btnReports.PaintDefaultFill = false;
            this.btnReports.RoundedCornersMask = 15;
            this.btnReports.RoundedCornersRadius = 0;
            this.btnReports.Size = new Size(40, 40);
            this.btnReports.TabIndex = 1;
            this.btnReports.UseVisualStyleBackColor = false;
            this.btnReports.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnReports.Visible = false;
            this.btnReports.Click += new EventHandler(this.btnReports_Click);
            this.btn_Logout.AllowAnimations = true;
            this.btn_Logout.BackColor = Color.Transparent;
            this.btn_Logout.Dock = DockStyle.Left;
            this.btn_Logout.Image = Properties.Resources.logout;
            this.btn_Logout.Location = new Point(0, 0);
            this.btn_Logout.Name = "btn_Logout";
            this.btn_Logout.PaintBorder = false;
            this.btn_Logout.PaintDefaultBorder = false;
            this.btn_Logout.PaintDefaultFill = false;
            this.btn_Logout.RoundedCornersMask = 15;
            this.btn_Logout.RoundedCornersRadius = 0;
            this.btn_Logout.Size = new Size(40, 40);
            this.btn_Logout.TabIndex = 0;
            this.btn_Logout.UseVisualStyleBackColor = false;
            this.btn_Logout.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Logout.Click += new EventHandler(this.btn_Logout_Click);
            this.vControlBox.BackColor = Color.White;
            this.vControlBox.BorderColor = Color.Black;
            this.vControlBox.ContentControl = null;
            this.vControlBox.DropDownHeight = 350;
            this.vControlBox.DropDownMaximumSize = new Size(600, 500);
            this.vControlBox.DropDownMinimumSize = new Size(350, 300);
            this.vControlBox.DropDownResizeDirection = SizingDirection.Both;
            this.vControlBox.DropDownWidth = 550;
            this.vControlBox.Location = new Point(11, 50);
            this.vControlBox.MinimumSize = new Size(250, 23);
            this.vControlBox.Name = "vControlBox";
            this.vControlBox.Size = new Size(281, 23);
            this.vControlBox.TabIndex = 3;
            this.vControlBox.Text = "Groups and Accounts";
            this.vControlBox.UseThemeBackColor = false;
            this.vControlBox.UseThemeDropDownArrowColor = true;
            this.vControlBox.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.vControlBox.Visible = false;
            this.vControlBox.Enter += new EventHandler(this.vControlBox_Enter);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(784, 562);
            base.Controls.Add(this.MainTabControl);
            base.Controls.Add(this.HeaderPanel);
            base.Controls.Add(this.statusStrip1);
            base.Icon = (Icon)Resources.MainForm.MainFormIcon;
            this.MinimumSize = new Size(800, 600);
            base.Name = "MainForm";
            this.Text = "C3 Sentinel Catalog";
            base.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
            base.FormClosed += new FormClosedEventHandler(this.MainForm_FormClosed);
            base.Load += new EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.HeaderPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((ISupportInitialize)this.LogoBox).EndInit();
            ((ISupportInitialize)this.USBPic).EndInit();
            ((ISupportInitialize)this.CamPic).EndInit();
            this.BatteryPanel.ResumeLayout(false);
            this.Battery.ResumeLayout(false);
            this.MenuPanel.ResumeLayout(false);
            this.btnChkUpdates.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitializeUI()
        {
            this.AccountTree.EVT_NodeCallback += new AcctCtrl.DEL_NodeCallback(this.aCtrl_EVT_NodeCallback);
            this.vControlBox.ContentControl = this.AccountTree;
        }

        private void KillPID()
        {
            Process.GetCurrentProcess().Kill();
        }

        private void LoadGlobalConfig()
        {
            using (RPM_GlobalConfig rPMGlobalConfig = new RPM_GlobalConfig())
            {
                GlobalConfig configRecord = rPMGlobalConfig.GetConfigRecord("UNC_NAME");
                if (configRecord != null)
                {
                    Global.UNCServer = configRecord.Value;
                    Global.PrimaryUNCServer = configRecord.Value;
                }
                configRecord = rPMGlobalConfig.GetConfigRecord("WINDOW_POSITION");
                if (configRecord != null)
                {
                    this.IsOpenCenter = Convert.ToBoolean(configRecord.Value);
                }
                configRecord = rPMGlobalConfig.GetConfigRecord("UNC_PATH");
                if (configRecord != null)
                {
                    Global.RelativePath = configRecord.Value;
                    Global.PrimaryRelativePath = configRecord.Value;
                }
                StoreCheck.CheckAccountStorage();
                Global.IsEmail = false;
                configRecord = rPMGlobalConfig.GetConfigRecord("MAIL_ALERT_ENABLED");
                if (configRecord != null)
                {
                    Global.IsEmail = Convert.ToBoolean(configRecord.Value);
                }
                Global.DefaultLanguage = "en-US|English (United States)";
                configRecord = rPMGlobalConfig.GetConfigRecord("COUNTRY_CODE");
                if (configRecord != null)
                {
                    Global.DefaultLanguage = configRecord.Value;
                }
                this.btnChkUpdates.Visible = false;
                configRecord = rPMGlobalConfig.GetConfigRecord("UPDATES");
                if (configRecord != null && Convert.ToBoolean(configRecord.Value))
                {
                    this.btnChkUpdates.Visible = true;
                    this.IsUpdateEnabled = true;
                }
                configRecord = rPMGlobalConfig.GetConfigRecord("AUTO_UPDATES");
                if (configRecord != null && Convert.ToBoolean(configRecord.Value))
                {
                    this.IsAutoUpdate = true;
                }
                configRecord = rPMGlobalConfig.GetConfigRecord("NTP_SERVER");
                if (configRecord != null && !string.IsNullOrEmpty(configRecord.Value))
                {
                    (new NTPTime()).method_0(configRecord.Value);
                }
            }
            using (RPM_License rPMLicense = new RPM_License())
            {
                Global.CameraLicense = rPMLicense.GetLicenseCount();
            }
            if (this.cat1 != null)
            {
                this.cat1.Reload_Lookups();
            }
        }

        private void LoadRegData()
        {
            try
            {
                if (this.IsOpenCenter)
                {
                    base.StartPosition = FormStartPosition.CenterScreen;
                }
                else
                {
                    WindowPos windowPos = Registry.GetWindowPos("AppWindow");
                    if (windowPos.Height == 0 || windowPos.Width == 0)
                    {
                        base.StartPosition = FormStartPosition.CenterScreen;
                        base.Width = 800;
                        base.Height = 600;
                    }
                    else
                    {
                        base.Width = windowPos.Width;
                        base.Height = windowPos.Height;
                        base.Location = new Point(windowPos.PosX, windowPos.PosY);
                    }
                }
            }
            catch
            {
            }
        }

        private void Log_Logout()
        {
            using (RPM_Account rPMAccount = new RPM_Account())
            {
                rPMAccount.Logout();
            }
        }

        private void LogoBox_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start((Global.IS_WOLFCOM ? "http://www.wolfcomusa.com" : "http://www.hdprotech.com"));
            }
            catch
            {
            }
        }

        private bool LogoutPwd()
        {
            bool flag = true;
            using (RPM_GlobalConfig rPMGlobalConfig = new RPM_GlobalConfig())
            {
                GlobalConfig configRecord = rPMGlobalConfig.GetConfigRecord("LOGOUT_PWD_ENABLED");
                if (configRecord == null)
                {
                    flag = false;
                }
                else if (!Convert.ToBoolean(configRecord.Value))
                {
                    flag = false;
                }
                else if ((new AppLogout()).ShowDialog(this) == DialogResult.OK)
                {
                    flag = false;
                }
            }
            return flag;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.KillPID();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.LogoutPwd())
            {
                e.Cancel = true;
                return;
            }
            this.wSparkle.Cleanup();
            this.Log_Logout();
            this.RegDeviceEvent(false);
            this.SaveRegData();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Global.IsVisionCamera = false;
            this.CommandArgs();
            SplashScreen splashScreen = new SplashScreen();
            splashScreen.Show();
            Application.DoEvents();
            if (Global.IS_WOLFCOM)
            {
                this.HeaderPanel.BackgroundImage = Properties.Resources.header80;
                this.statusStrip1.BackColor = Color.FromArgb(64, 64, 64);
                this.LogoBox.Image = Properties.Resources.wolfcom_bar;
                this.LogoBox.Size = new Size(160, 21);
                this.LogoBox.SizeMode = PictureBoxSizeMode.StretchImage;
                this.LogoBox.Location = new Point(620, 3);
            }
            DBConnection dBConnection = new DBConnection();
            if (!dBConnection.method_0())
            {
                this.KillPID();
            }
            else if (!dBConnection.TestConnection())
            {
                splashScreen.CloseForm();
                MessageBox.Show(this, "DATABASE ERROR:\nCan not connect to C3 Sentinel Database!\nPlease contact your system administrator.", "Database", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.KillPID();
            }
            this.InitGlobalData();
            this.InitializeUI();
            if (splashScreen != null)
            {
                splashScreen.CloseForm();
            }
            this.CatalogLogin();
            this.LoadRegData();
            if (Global.GlobalAccount.SystemRole == SYSTEM_ROLE.ADMIN && Global.IsRights(Global.RightsProfile, UserRights.SYSTEM))
            {
                this.btnStudio.Visible = true;
            }
            if (Directory.Exists(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Redact")))
            {
                Global.IsRedact = true;
            }
            this.CheckGlobalCatalogRights();
            this.devNotifier = DeviceNotifier.OpenDeviceNotifier();
            this.AccountTab();
            this.MainTabControl.Visible = true;
            this.RegDeviceEvent(true);
            this.RegCameras();
        }

        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            vTabPage selectedTab = (sender as vTabControl).SelectedTab;
            if (selectedTab != null)
            {
                Guid tag = (Guid)selectedTab.Tag;
                if (tag != Guid.Empty)
                {
                    StoreCheck.CheckAccountStorage(tag);
                    return;
                }
                StoreCheck.CheckAccountStorage();
            }
        }

        private void onDevNotify(object sender, DeviceNotifyEventArgs e)
        {
            try
            {
                EventType eventType = e.EventType;
                DeviceType deviceType = e.DeviceType;
                IUsbDeviceNotifyInfo device = e.Device;
                IVolumeNotifyInfo volume = e.Volume;
                if (eventType == EventType.DeviceRemoveComplete && volume != null)
                {
                    this.CamPic.Visible = false;
                    this.Battery.Visible = false;
                    this.lbl_Battery.Visible = false;
                    this.StatusMsg.Text = string.Empty;
                }
                if (eventType == EventType.DeviceArrival && volume != null)
                {
                    this.USBDeviceDriveDetect(volume.Letter);
                }
            }
            catch (Exception exception)
            {
            }
        }

        private void RegCameras()
        {
            try
            {
                if (Global.IS_WOLFCOM)
                {
                    this.StatusMsg.ForeColor = Color.White;
                }
                this.IsDetectCamera = true;
                this.lstDevices.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) => this.ConnectDevice());
                this._manager = new VisionManager();
                this._manager.DeviceInserted += new EventHandler((object sender, EventArgs e) => base.Invoke(new MethodInvoker(this.UpdateDeviceList)));
                this._manager.DeviceRemoved += new EventHandler((object sender, EventArgs e) => base.Invoke(new MethodInvoker(this.UpdateDeviceList)));
                this._manager.DeviceMounted += new EventHandler<MountedEventArgs>((object sender, MountedEventArgs e) => base.Invoke(new MethodInvoker(() => this.DeviceMounted(e.Drive))));
                this._manager.ScanDevices();
                this.CiteCameraParameters();
                this.cLib.RegDeviceEvent(true);
                this.cLib.EVT_DevActionCallback += new CiteLib.DEL_DevActionCallback(this.cLib_EVT_DevActionCallback);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void RegDeviceEvent(bool b)
        {
            try
            {
                if (this.devNotifier != null)
                {
                    switch (b)
                    {
                        case false:
                            {
                                this.IsRegistered = false;
                                this.devNotifier.OnDeviceNotify -= new EventHandler<DeviceNotifyEventArgs>(this.onDevNotify);
                                this.USBPic.Visible = false;
                                break;
                            }
                        case true:
                            {
                                this.IsRegistered = true;
                                this.devNotifier.OnDeviceNotify -= new EventHandler<DeviceNotifyEventArgs>(this.onDevNotify);
                                this.devNotifier.OnDeviceNotify += new EventHandler<DeviceNotifyEventArgs>(this.onDevNotify);
                                this.USBPic.Visible = true;
                                break;
                            }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void report_EVT_CloseReport()
        {
            this.IsReport = false;
            this.report.EVT_CloseReport -= new ReportForm.DEL_CloseReport(this.report_EVT_CloseReport);
            this.report = null;
        }

        private void RunUpdate()
        {
            if (!Global.IS_WOLFCOM)
            {
                this.wSparkle.SetAppCastUrl("ftp://ftp.hdprotech.com/C3Sentinel/Client/appcast.xml");
            }
            else
            {
                this.wSparkle.SetAppCastUrl("ftp://ftp.hdprotech.com/Wolfcom/Client/appcast.xml");
            }
            this.wSparkle.CheckUpdateWithUi();
        }

        private void SaveRegData()
        {
            if (!this.IsOpenCenter)
            {
                WindowPos windowPo = new WindowPos()
                {
                    Width = base.Width,
                    Height = base.Height,
                    PosX = base.Location.X,
                    PosY = base.Location.Y
                };
                Registry.SetWindowPos(windowPo, "AppWindow");
            }
        }

        private void SecurityProfile()
        {
            this.vControlBox.Visible = false;
            if (Global.IsRights(Global.RightsProfile, UserRights.VIEWCAT))
            {
                this.vControlBox.Visible = true;
            }
            this.btnReports.Visible = false;
            if (Global.IsRights(Global.RightsProfile, UserRights.REPORTS))
            {
                this.btnReports.Visible = true;
            }
            this.btnAlertEmails.Visible = false;
            if (Global.IsEmail && !string.IsNullOrEmpty(Global.GlobalAccount.Email) && Global.IsRights(Global.RightsProfile, UserRights.SEND_EMAIL))
            {
                this.btnAlertEmails.Visible = true;
            }
        }

        private void SetLanguage()
        {
            string defaultLanguage = Global.DefaultLanguage;
            char[] chrArray = new char[] { '|' };
            if (LangCtrl.SetLanguage(defaultLanguage.Split(chrArray)[0]))
            {
                LangCtrl.reText(this);
            }
            this.SetTooltips();
        }

        private void SetTooltips()
        {
            this.tt.RemoveAll();
            this.tt.SetToolTip(this.btn_About, LangCtrl.GetString("tt_About", "About C3 Sentinel"));
            this.tt.SetToolTip(this.btnAlertEmails, LangCtrl.GetString("tt_AlertEmails", "Send Alert Email"));
            this.tt.SetToolTip(this.btn_Help, LangCtrl.GetString("tt_Help", "C3 Sentinel Help"));
            this.tt.SetToolTip(this.btn_Logout, LangCtrl.GetString("tt_Logout", "Logout C3 Sentinel"));
            this.tt.SetToolTip(this.btnChkUpdates, LangCtrl.GetString("tt_ChkUpdates", "Check for Software Updates"));
            this.tt.SetToolTip(this.btnReports, LangCtrl.GetString("tt_Reports", "Reports"));
            this.tt.SetToolTip(this.btnStudio, LangCtrl.GetString("tt_Studio", "C3 Sentinel Management Studio"));
            this.tt.SetToolTip(this.btnGlobalSearch, LangCtrl.GetString("tt_GlobalSearch", "Global Catalog Search"));
        }

        private void ShowBatteryStats(bool b)
        {
            try
            {
                if (!b)
                {
                    this.CamPic.Visible = false;
                    this.Battery.Visible = false;
                    this.Battery.Value = 0;
                    this.lbl_Battery.Visible = false;
                    this.lbl_Battery.Text = string.Format(LangCtrl.GetString("lbl_Battery", "Battery {0}%"), 0);
                    this.StatusMsg.Text = "";
                    Global.Camera_Battery = 0;
                }
                else
                {
                    this.CamPic.Visible = true;
                    this.Battery.Visible = true;
                    this.BatteryPercent = this.cLib.BatteryPercent;
                    this.Battery.Value = this.BatteryPercent;
                    this.Battery.Refresh();
                    this.lbl_Battery.Visible = true;
                    this.lbl_Battery.Text = string.Format(LangCtrl.GetString("lbl_Battery", "Battery {0}%"), this.cLib.BatteryPercent);
                    this.lbl_Battery.Refresh();
                    Global.Camera_Battery = this.BatteryPercent;
                    Global.Camera_SerialNum = this.cLib.CamSerial;
                    this.DiskSize = this.cLib.FreeDiskSpace;
                    Global.Camera_Disk = this.DiskSize;
                }
            }
            catch
            {
            }
        }

        private void UpdateDeviceList()
        {
            if (this.IsDetectCamera)
            {
                if (base.InvokeRequired)
                {
                    base.Invoke(new MethodInvoker(this.UpdateDeviceList));
                }
                this.lstDevices.Items.Clear();
                this.lstDevices.Items.AddRange(this._manager.Devices);
                if ((int)this._manager.Devices.Length == 1)
                {
                    this._device = this._manager.Devices[0];
                    Global.VisionSN = this._device.SerialNo;
                }
                if (this._device != null && this._manager.Devices.Contains<VisionDevice>(this._device))
                {
                    Global.IsVisionCamera = true;
                    this.lstDevices.SelectedItem = this._device;
                    this._device.Mount();
                    return;
                }
                this.DisconnectDevice();
            }
        }

        public void USBDeviceDriveDetect(string drive)
        {
            if (!string.IsNullOrEmpty(drive) && this.IsLoggedOn && !string.IsNullOrEmpty(drive))
            {
                this.df.EVT_FolderScan -= new DetectFolder.DEL_FolderScan(this.df_EVT_FolderScan);
                this.df.EVT_FolderScan += new DetectFolder.DEL_FolderScan(this.df_EVT_FolderScan);
                List<string> strs = new List<string>()
                {
                    "DCIM",
                    "100MEDIA"
                };
                using (RPM_Camera rPMCamera = new RPM_Camera())
                {
                    List<CameraFolder> folderList = rPMCamera.GetFolderList();
                    if (folderList.Count > 0)
                    {
                        foreach (CameraFolder cameraFolder in folderList)
                        {
                            strs.Add(cameraFolder.Folder);
                        }
                    }
                }
                string[] array = strs.ToArray();
                if (!this.IsCiteCamera)
                {
                    this.CamPic.Visible = false;
                    this.Battery.Visible = false;
                    this.lbl_Battery.Visible = false;
                    this.StatusMsg.Text = string.Empty;
                }
                else
                {
                    try
                    {
                        string str = string.Format("{0}:\\DCIM", drive);
                        if (Directory.Exists(str))
                        {
                            Directory.CreateDirectory(str).Attributes = FileAttributes.Hidden | FileAttributes.System | FileAttributes.Directory;
                        }
                        str = string.Format("{0}:\\DAILYLOG", drive);
                        if (Directory.Exists(str))
                        {
                            Directory.CreateDirectory(str).Attributes = FileAttributes.Hidden | FileAttributes.System | FileAttributes.Directory;
                        }
                    }
                    catch
                    {
                    }
                }
                if (!File.Exists(string.Format("{0}://CSETUP.EXE", drive)))
                {
                    this.df.ScanFolders(drive, array, true);
                }
                this.IsCiteCamera = false;
            }
        }

        private void USBPic_DoubleClick(object sender, EventArgs e)
        {
        }

        private void vControlBox_Enter(object sender, EventArgs e)
        {
            this.AccountTree.InitTree();
        }
    }
}