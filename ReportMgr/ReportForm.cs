using AppGlobal;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;
using ReportMgr.Properties;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMInterfaces;
using VMModels.Enums;
using VMModels.Model;
using WinReg;

namespace ReportMgr
{
    public class ReportForm : Form
    {
        private ReportDocument rDoc;

        private System.Timers.Timer timer;

        private bool IsAdmin;

        private bool IsAdminReport;

        private IContainer components;

        private Panel HeaderPanel;

        private PictureBox IconPic;

        private StatusStrip statusStrip1;

        private BindingSource AccountBindingSource;

        private vButton btn_ClearReport;

        private vComboBox cboReports;

        private CrystalReportViewer crystalReportViewer1;

        private ImageList imageList1;

        private Label lblUserAccount;

        private vComboBox cboSysReports;

        private ToolStripStatusLabel txtReportTitle;

        private ToolStripStatusLabel txtAuthor;

        private Guid AccountID
        {
            get;
            set;
        }

        private string AccountPath
        {
            get;
            set;
        }

        private string Database
        {
            get;
            set;
        }

        private string DBPwd
        {
            get;
            set;
        }

        private bool IsEnabled
        {
            get;
            set;
        }

        private string String_0
        {
            get;
            set;
        }

        private string SystemPath
        {
            get;
            set;
        }

        public ReportForm()
        {
            InitializeComponent();
        }

        private void btn_ClearReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.cboReports.SelectedIndex = -1;
                this.cboReports.SelectedItem = null;
                this.cboSysReports.SelectedIndex = -1;
                this.cboSysReports.SelectedItem = null;
                this.crystalReportViewer1.ReportSource = null;
                this.crystalReportViewer1.Refresh();
                this.txtReportTitle.Text = string.Empty;
                this.txtAuthor.Text = string.Empty;
            }
            catch
            {
            }
        }

        private void Callback()
        {
            if (this.EVT_CloseReport != null)
            {
                this.EVT_CloseReport();
            }
        }

        private void cboReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtReportTitle.Text = string.Empty;
            this.txtAuthor.Text = string.Empty;
            this.IsAdminReport = false;
            if (this.cboReports.SelectedIndex > -1)
            {
                try
                {
                    ListItem selectedItem = this.cboReports.SelectedItem;
                    if (selectedItem != null)
                    {
                        string tag = (string)selectedItem.Tag;
                        if (File.Exists(tag))
                        {
                            this.cboReports.CloseDropDown();
                            this.RunReport(tag);
                        }
                    }
                }
                catch
                {
                }
                this.cboSysReports.SelectedIndex = -1;
                this.cboReports.CloseDropDown();
            }
        }

        private void cboSysReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtReportTitle.Text = string.Empty;
            this.txtAuthor.Text = string.Empty;
            this.IsAdminReport = true;
            if (this.cboSysReports.SelectedIndex > -1)
            {
                try
                {
                    ListItem selectedItem = this.cboSysReports.SelectedItem;
                    if (selectedItem != null)
                    {
                        string tag = (string)selectedItem.Tag;
                        if (File.Exists(tag))
                        {
                            this.cboReports.CloseDropDown();
                            this.RunReport(tag);
                        }
                    }
                }
                catch
                {
                }
                this.cboReports.SelectedIndex = -1;
                this.cboSysReports.CloseDropDown();
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

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ReportForm));
            this.imageList1 = new ImageList(this.components);
            this.statusStrip1 = new StatusStrip();
            this.txtReportTitle = new ToolStripStatusLabel();
            this.txtAuthor = new ToolStripStatusLabel();
            this.crystalReportViewer1 = new CrystalReportViewer();
            this.HeaderPanel = new Panel();
            this.cboSysReports = new vComboBox();
            this.lblUserAccount = new Label();
            this.btn_ClearReport = new vButton();
            this.cboReports = new vComboBox();
            this.IconPic = new PictureBox();
            this.AccountBindingSource = new BindingSource(this.components);
            this.statusStrip1.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((ISupportInitialize)this.IconPic).BeginInit();
            ((ISupportInitialize)this.AccountBindingSource).BeginInit();
            base.SuspendLayout();
            this.imageList1.ImageStream = (ImageListStreamer)Resources.ReportForm.imageList1_ImageStream;
            this.imageList1.TransparentColor = Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "reports2.png");
            ToolStripItemCollection items = this.statusStrip1.Items;
            ToolStripItem[] toolStripItemArray = new ToolStripItem[] { this.txtReportTitle, this.txtAuthor };
            this.statusStrip1.Items.AddRange(toolStripItemArray);
            this.statusStrip1.Location = new Point(0, 640);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(984, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            this.txtReportTitle.Name = "txtReportTitle";
            this.txtReportTitle.Size = new Size(48, 17);
            this.txtReportTitle.Text = "Ready...";
            this.txtAuthor.Name = "txtAuthor";
            this.txtAuthor.Size = new Size(0, 17);
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = Cursors.Default;
            this.crystalReportViewer1.Dock = DockStyle.Fill;
            this.crystalReportViewer1.Location = new Point(0, 61);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.Size = new Size(984, 579);
            this.crystalReportViewer1.TabIndex = 2;
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.cboSysReports);
            this.HeaderPanel.Controls.Add(this.lblUserAccount);
            this.HeaderPanel.Controls.Add(this.btn_ClearReport);
            this.HeaderPanel.Controls.Add(this.cboReports);
            this.HeaderPanel.Controls.Add(this.IconPic);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(984, 61);
            this.HeaderPanel.TabIndex = 0;
            this.cboSysReports.BackColor = Color.White;
            this.cboSysReports.DefaultText = "Select System Report...";
            this.cboSysReports.DisplayMember = "";
            this.cboSysReports.DropDownList = true;
            this.cboSysReports.DropDownMaximumSize = new Size(300, 400);
            this.cboSysReports.DropDownMinimumSize = new Size(225, 150);
            this.cboSysReports.DropDownResizeDirection = SizingDirection.Both;
            this.cboSysReports.DropDownWidth = 225;
            this.cboSysReports.ImageList = this.imageList1;
            this.cboSysReports.Location = new Point(55, 32);
            this.cboSysReports.Name = "cboSysReports";
            this.cboSysReports.RoundedCornersMaskListItem = 15;
            this.cboSysReports.Size = new Size(225, 23);
            this.cboSysReports.TabIndex = 2;
            this.cboSysReports.UseThemeBackColor = false;
            this.cboSysReports.UseThemeDropDownArrowColor = true;
            this.cboSysReports.ValueMember = "";
            this.cboSysReports.VIBlendScrollBarsTheme = VIBLEND_THEME.VISTABLUE;
            this.cboSysReports.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.cboSysReports.Visible = false;
            this.cboSysReports.SelectedIndexChanged += new EventHandler(this.cboSysReports_SelectedIndexChanged);
            this.lblUserAccount.Anchor = AnchorStyles.Right;
            this.lblUserAccount.BackColor = Color.Transparent;
            this.lblUserAccount.ForeColor = Color.White;
            this.lblUserAccount.ImageAlign = ContentAlignment.MiddleRight;
            this.lblUserAccount.Location = new Point(723, 9);
            this.lblUserAccount.Name = "lblUserAccount";
            this.lblUserAccount.Size = new Size(249, 17);
            this.lblUserAccount.TabIndex = 3;
            this.lblUserAccount.Text = "Account";
            this.btn_ClearReport.AllowAnimations = true;
            this.btn_ClearReport.BackColor = Color.Transparent;
            this.btn_ClearReport.Location = new Point(286, 6);
            this.btn_ClearReport.Name = "btn_ClearReport";
            this.btn_ClearReport.RoundedCornersMask = 15;
            this.btn_ClearReport.RoundedCornersRadius = 0;
            this.btn_ClearReport.Size = new Size(164, 23);
            this.btn_ClearReport.TabIndex = 2;
            this.btn_ClearReport.Text = "Clear Report";
            this.btn_ClearReport.UseVisualStyleBackColor = false;
            this.btn_ClearReport.VIBlendTheme = VIBLEND_THEME.METROBLUE;
            this.btn_ClearReport.Click += new EventHandler(this.btn_ClearReport_Click);
            this.cboReports.BackColor = Color.White;
            this.cboReports.DefaultText = "Select Account Report...";
            this.cboReports.DisplayMember = "";
            this.cboReports.DropDownList = true;
            this.cboReports.DropDownMaximumSize = new Size(300, 400);
            this.cboReports.DropDownMinimumSize = new Size(225, 150);
            this.cboReports.DropDownResizeDirection = SizingDirection.Both;
            this.cboReports.DropDownWidth = 225;
            this.cboReports.ImageList = this.imageList1;
            this.cboReports.Location = new Point(55, 6);
            this.cboReports.Name = "cboReports";
            this.cboReports.RoundedCornersMaskListItem = 15;
            this.cboReports.Size = new Size(225, 23);
            this.cboReports.TabIndex = 1;
            this.cboReports.UseThemeBackColor = false;
            this.cboReports.UseThemeDropDownArrowColor = true;
            this.cboReports.ValueMember = "";
            this.cboReports.VIBlendScrollBarsTheme = VIBLEND_THEME.VISTABLUE;
            this.cboReports.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.cboReports.SelectedIndexChanged += new EventHandler(this.cboReports_SelectedIndexChanged);
            this.IconPic.BackColor = Color.Transparent;
            this.IconPic.Image = Properties.Resources.reports2;
            this.IconPic.Location = new Point(12, 6);
            this.IconPic.Name = "IconPic";
            this.IconPic.Size = new Size(34, 34);
            this.IconPic.TabIndex = 0;
            this.IconPic.TabStop = false;
            this.AccountBindingSource.DataSource = typeof(Account);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(984, 662);
            base.Controls.Add(this.crystalReportViewer1);
            base.Controls.Add(this.statusStrip1);
            base.Controls.Add(this.HeaderPanel);
            base.Icon = (Icon)Resources.ReportForm.ReportFormIcon;
            this.MinimumSize = new Size(800, 600);
            base.Name = "ReportForm";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Account Reports";
            base.FormClosing += new FormClosingEventHandler(this.ReportForm_FormClosing);
            base.Load += new EventHandler(this.ReportForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.HeaderPanel.ResumeLayout(false);
            ((ISupportInitialize)this.IconPic).EndInit();
            ((ISupportInitialize)this.AccountBindingSource).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LoadAccountReports()
        {
            try
            {
                this.cboReports.Items.Clear();
                string[] files = Directory.GetFiles(this.AccountPath, "*.rpt", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < (int)files.Length; i++)
                {
                    string str = files[i];
                    ListItem listItem = new ListItem()
                    {
                        Tag = str,
                        ImageIndex = 0,
                        Text = Path.GetFileName(str)
                    };
                    this.cboReports.Items.Add(listItem);
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void LoadAdminReports()
        {
            try
            {
                this.cboSysReports.Items.Clear();
                string[] files = Directory.GetFiles(this.SystemPath, "*.rpt", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < (int)files.Length; i++)
                {
                    string str = files[i];
                    ListItem listItem = new ListItem()
                    {
                        Tag = str,
                        ImageIndex = 0,
                        Text = Path.GetFileName(str)
                    };
                    this.cboSysReports.Items.Add(listItem);
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void LoadConfig()
        {
            using (RPM_GlobalConfig rPMGlobalConfig = new RPM_GlobalConfig())
            {
                GlobalConfig globalConfig = new GlobalConfig();
                globalConfig = rPMGlobalConfig.GetConfigRecord("REPORT_ACCOUNT_PATH");
                if (globalConfig != null)
                {
                    this.AccountPath = globalConfig.Value;
                }
                globalConfig = rPMGlobalConfig.GetConfigRecord("REPORT_SYSTEM_PATH");
                if (globalConfig != null)
                {
                    this.SystemPath = globalConfig.Value;
                }
                globalConfig = rPMGlobalConfig.GetConfigRecord("REPORT_DB_ACCOUNT");
                if (globalConfig != null)
                {
                    this.String_0 = CryptoIO.Decrypt(globalConfig.Value);
                }
                globalConfig = rPMGlobalConfig.GetConfigRecord("REPORT_DATABASE");
                if (globalConfig != null)
                {
                    this.Database = CryptoIO.Decrypt(globalConfig.Value);
                }
                globalConfig = rPMGlobalConfig.GetConfigRecord("REPORT_PWD");
                if (globalConfig != null)
                {
                    this.DBPwd = CryptoIO.Decrypt(globalConfig.Value);
                }
                globalConfig = rPMGlobalConfig.GetConfigRecord("REPORT_SYS_ENABLED");
                if (globalConfig != null)
                {
                    this.IsEnabled = Convert.ToBoolean(globalConfig.Value);
                }
            }
        }

        private void LoadRegData()
        {
            WindowPos windowPos = Registry.GetWindowPos("ReportWindow");
            if (windowPos.Height == 0 || windowPos.Width == 0)
            {
                base.StartPosition = FormStartPosition.CenterScreen;
                base.Width = 800;
                base.Height = 600;
                return;
            }
            base.Width = windowPos.Width;
            base.Height = windowPos.Height;
            base.Location = new Point(windowPos.PosX, windowPos.PosY);
        }

        private void ReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveRegData();
            this.Callback();
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
            {
                this.HeaderPanel.BackgroundImage = Properties.Resources.header80;
                this.btn_ClearReport.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            }
            this.Text = LangCtrl.GetString("dlg_ReportForm", "Account Reports");
            this.btn_ClearReport.Text = LangCtrl.GetString("btn_ClearReport", "Clear Report");
            this.cboSysReports.DefaultText = LangCtrl.GetString("cboSysReports", "Select System Report...");
            this.cboReports.DefaultText = LangCtrl.GetString("cboReports", "Select Account Report...");
            this.HeaderPanel.Height = 40;
            this.AccountID = Global.GlobalAccount.Id;
            this.lblUserAccount.Text = string.Format("Account: {0} • {1}", Global.GlobalAccount.ToString(), Global.GlobalAccount.BadgeNumber);
            if (Global.GlobalAccount.SystemRole == SYSTEM_ROLE.ADMIN || Global.GlobalAccount.SystemRole == SYSTEM_ROLE.DELEGATE)
            {
                this.cboSysReports.Visible = true;
                this.HeaderPanel.Height = 61;
                this.IsAdmin = true;
            }
            try
            {
                this.rDoc = new ReportDocument();
                this.LoadConfig();
                this.timer = new System.Timers.Timer(1000)
                {
                    Enabled = true
                };
                this.timer.Elapsed -= new ElapsedEventHandler(this.timer_Elapsed);
                this.timer.Elapsed += new ElapsedEventHandler(this.timer_Elapsed);
                this.timer.Start();
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            this.LoadRegData();
        }

        private void RunReport(string RptFile)
        {
            try
            {
                this.rDoc = new ReportDocument();
                this.rDoc.Load(RptFile);
                this.txtReportTitle.Text = this.rDoc.SummaryInfo.ReportTitle;
                this.txtAuthor.Text = this.rDoc.SummaryInfo.ReportAuthor;
                ConnectionInfo connectionInfo = new ConnectionInfo()
                {
                    Password = this.DBPwd,
                    ServerName = this.Database,
                    UserID = this.String_0,
                    DatabaseName = "C3Sentinel"
                };
                foreach (Table table in this.rDoc.Database.Tables)
                {
                    table.LogOnInfo.ConnectionInfo = connectionInfo;
                    table.ApplyLogOnInfo(table.LogOnInfo);
                }
                this.rDoc.SetDatabaseLogon(this.String_0, this.DBPwd);
                this.rDoc.ReadRecords();
                this.rDoc.Refresh();
                this.crystalReportViewer1.ReportSource = this.rDoc;
                if (!this.IsAdminReport)
                {
                    string str = string.Concat("{", Convert.ToString(this.AccountID), "}");
                    this.rDoc.SetParameterValue("@AccountID", str);
                }
                this.crystalReportViewer1.Update();
                this.crystalReportViewer1.Refresh();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void SaveRegData()
        {
            WindowPos windowPo = new WindowPos()
            {
                Width = base.Width,
                Height = base.Height,
                PosX = base.Location.X,
                PosY = base.Location.Y
            };
            Registry.SetWindowPos(windowPo, "ReportWindow");
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.timer.Stop();
            this.timer.Elapsed -= new ElapsedEventHandler(this.timer_Elapsed);
            if (this.IsEnabled)
            {
                this.LoadAccountReports();
                if (this.IsAdmin)
                {
                    this.LoadAdminReports();
                }
            }
        }

        public event ReportForm.DEL_CloseReport EVT_CloseReport;

        public delegate void DEL_CloseReport();
    }
}