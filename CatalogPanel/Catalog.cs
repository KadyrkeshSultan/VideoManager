using AccountSelector;
using AppGlobal;
using CatalogPanel.Properties;
using ExportMgr;
using FileShareCtrl;
using FileTreeCtrl;
using FileUpload;
using Logger;
using PwdReset;
using SetTreeCtrl;
using SlideCtrl2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VideoPlayer2;
using VMInterfaces;
using VMModels.Enums;
using VMModels.Model;

namespace CatalogPanel
{
    public class Catalog : UserControl, IDisposable
    {
        public Guid Account_ID = Guid.Empty;

        private SetCtrl CatalogSet;

        private FileTree fTree;

        private bool IsRetentionPolicy;

        private bool IsDeleteAllowed;

        private ToolTip tt = new ToolTip();

        private bool IsByDate;

        private DateTime FileDate1;

        private DateTime FileDate2;

        private Stopwatch stopwatch = new Stopwatch();

        private bool IsNoResults;

        private int SlideCount;

        private bool IsOpen = true;

        private IContainer components;

        private Panel HeaderPanel;

        private vButton btn_Panel;

        private vNavPane NavPanel;

        private vNavPaneItem Nav_Search;

        private vNavPaneItem Nav_Info;

        private vNavPaneItem Nav_Sets;

        private FlowLayoutPanel FilePanel;

        private Label lbl_SelectedCount;

        private Label lbl_SlideCount;

        private TableLayoutPanel tableLayoutPanel1;

        private Label lbl_Selected;

        private Label lbl_Files;

        private PictureBox SetPic;

        private vButton btn_Deselect;

        private vButton btn_QuickSearch;

        private vComboBox cboDays;

        private Label lblLine1;

        private ContextMenuStrip FilePanelMenu;

        private ToolStripMenuItem mnu_BackgroudColor;

        private vComboBox cboClassification;

        private Label lbl_Classification;

        private vDateTimePicker ToDate;

        private Label lbl_EndDate;

        private vDateTimePicker FromDate;

        private Label lbl_StartDate;

        private ToolStripMenuItem mnu_AddFile;

        private vRatingControl Rating;

        private vTextBox txtPhrase;

        private vButton btn_ResetSearch;

        private vComboBox cboSecurity;

        private Label lbl_Security;

        private ImageList imageList1;

        private vButton btnScanner;

        private Label lblVLine;

        private ToolStripSeparator toolStripMenuItem1;

        private ToolStripMenuItem mnu_ClearSelections;

        private ToolStripMenuItem mnu_ScanDoc;

        private vComboBox cboFileType;

        private Label lbl_ByFileExt;

        private Label lbl_Days;

        private Label lblTime;

        private Label lbl_Sec;

        private ToolStripMenuItem mnu_ResetForm;

        private vTextBox txtCAD;

        private Label lbl_CADId;

        private vTextBox txtRMS;

        private Label lbl_RMSId;

        private vButton btnPassword;

        private Panel OptionPanel;

        private Label label1;

        private vButton btnLanguage;

        private vCheckBox chk_FilterEvidence;

        private vProgressBar progBar;

        private vButton btnRedact;

        private vCheckBox chkRating;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                CreateParams exStyle = createParams;
                exStyle.ExStyle = exStyle.ExStyle | 33554432;
                return createParams;
            }
        }

        public Catalog()
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        private void btn_Deselect_Click(object sender, EventArgs e)
        {
            this.ClearSelections();
        }

        private void btn_FullSearch_Click(object sender, EventArgs e)
        {
            (new Thread(new ThreadStart(this.LoadSlides))).Start();
        }

        private void btn_Panel_Click(object sender, EventArgs e)
        {
            this.IsOpen = !this.IsOpen;
            this.NavPanel.Visible = this.IsOpen;
        }

        private void btn_QuickSearch_Click(object sender, EventArgs e)
        {
            this.SetPic.Visible = false;
            this.QuickSearch();
        }

        private void btn_ResetSearch_Click(object sender, EventArgs e)
        {
            this.ResetForm();
        }

        private void btnLanguage_Click(object sender, EventArgs e)
        {
            if ((new Language()).ShowDialog(this) == DialogResult.OK)
            {
                this.SetLanguage();
                this.UpdateLanguageCallback();
            }
        }

        private void btnPassword_Click(object sender, EventArgs e)
        {
            (new PwdForm()).ShowDialog(this);
        }

        private void btnRedact_Click(object sender, EventArgs e)
        {
            string str = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Redact/VideoEditor.exe");
            if (File.Exists(str))
            {
                try
                {
                    Global.Log("REDACT", LangCtrl.GetString("log_RedactExtFile", "Redact External File"));
                    Process.Start(str);
                }
                catch
                {
                }
            }
        }

        private void btnScanner_Click(object sender, EventArgs e)
        {
            this.ScanDoc();
        }

        private void Catalog_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
            {
                this.HeaderPanel.BackgroundImage = Properties.Resources.topbar45;
                this.lbl_Files.ForeColor = Color.White;
                this.lbl_Selected.ForeColor = Color.White;
                this.lbl_Sec.ForeColor = Color.White;
                this.lbl_SlideCount.ForeColor = Color.White;
                this.lbl_SelectedCount.ForeColor = Color.White;
                this.lblTime.ForeColor = Color.White;
                this.progBar.VIBlendTheme = VIBLEND_THEME.NERO;
            }
            this.LoadFileTree();
            this.LoadSetTree();
            this.ResetDates();
            this.LoadClassifications();
            this.LoadFileTypes();
            this.LoadSecurity();
            if (Global.IsRedact && Global.IsRights(Global.RightsProfile, UserRights.REDACT))
            {
                this.btnRedact.Visible = true;
            }
            this.SecurityProfile();
            this.CheckRetentionPolicy();
            this.SetLanguage();
            if (this.Account_ID.Equals(Global.GlobalAccount.Id))
            {
                this.OptionPanel.Visible = true;
            }
        }

        private void CatalogSet_EVT_ClearPanel()
        {
            this.ResetForm();
        }

        private void CatalogSet_EVT_TreeNodeCallback(object sender, CmdEventArgs e)
        {
            this.LoadSetSlides(e.NodeRec.Name);
        }

        private void cboDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboDays.SelectedIndex > -1)
            {
                int num = Convert.ToInt32(this.cboDays.Text);
                vDateTimePicker fromDate = this.FromDate;
                DateTime now = DateTime.Now;
                fromDate.Value = new DateTime?(now.AddDays((double)(-num)));
                this.ToDate.Value = new DateTime?(DateTime.Now);
            }
        }

        public void CheckRetentionPolicy()
        {
            using (RPM_GlobalConfig rPMGlobalConfig = new RPM_GlobalConfig())
            {
                GlobalConfig configRecord = rPMGlobalConfig.GetConfigRecord("RET_ENABLED");
                if (configRecord != null)
                {
                    this.IsRetentionPolicy = Convert.ToBoolean(configRecord.Value);
                }
                configRecord = rPMGlobalConfig.GetConfigRecord("FILEDELETE_MINDAYS");
                if (configRecord != null && Convert.ToInt32(configRecord.Value) > 0)
                {
                    this.IsDeleteAllowed = true;
                }
            }
        }

        private void chkRating_CheckedChanged(object sender, EventArgs e)
        {
            this.Rating.Enabled = this.chkRating.Checked;
        }

        private void chkRating_CheckedChanged_1(object sender, EventArgs e)
        {
        }

        private void ClearSelections()
        {
            try
            {
                foreach (Control control in this.FilePanel.Controls)
                {
                    Slide slide = (Slide)control;
                    if (!slide.sRecord.IsSelected)
                    {
                        continue;
                    }
                    slide.ClearSelection();
                }
            }
            catch
            {
            }
            this.SlideCount = 0;
            this.UpdateData();
            this.SetPic.Visible = false;
            GC.Collect();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FileDelete()
        {
            if (this.SlideCount <= 0)
            {
                MessageBox.Show(this, LangCtrl.GetString("msg_SelectFiles", "Select files to delete."), "Delete", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (MessageBox.Show(this, LangCtrl.GetString("msg_DeleteSlides", "Delete selected files?"), "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                FileDelete fileDelete = new FileDelete();
                fileDelete.InitDelete(ref this.FilePanel, this.Account_ID);
                fileDelete.ShowDialog(this);
                this.ClearSelections();
                return;
            }
        }

        private void FilePanel_ControlAdded(object sender, ControlEventArgs e)
        {
            this.UpdateData();
        }

        private void FilePanel_ControlRemoved(object sender, ControlEventArgs e)
        {
            try
            {
                Slide control = (Slide)e.Control;
                control.EVT_SlideCallback -= new Slide.DEL_SlideCallback(this.s_EVT_SlideCallback);
                control.EVT_PackageCallback -= new Slide.DEL_PackageCallback(this.s_EVT_PackageCallback);
            }
            catch
            {
            }
            this.FilePanel.SuspendLayout();
            this.SlideCount = 0;
            int num = 1;
            foreach (Control control1 in this.FilePanel.Controls)
            {
                Slide slide = (Slide)control1;
                int num1 = num;
                num = num1 + 1;
                slide.UpdateSlideNumber(num1);
            }
            this.GetSelectedCount();
            this.FilePanel.ResumeLayout();
        }

        private void FilePanel_Resize(object sender, EventArgs e)
        {
            this.FilePanel.AutoSize = true;
            this.FilePanel.Invalidate();
        }

        private void FilePanel_Scroll(object sender, ScrollEventArgs e)
        {
            base.OnScroll(e);
        }

        private void FromDate_ValueChanged(object sender, EventArgs e)
        {
            DateTime? value = this.FromDate.Value;
            DateTime? nullable = this.ToDate.Value;
            if ((value.HasValue & nullable.HasValue ? value.GetValueOrDefault() > nullable.GetValueOrDefault() : false))
            {
                this.FromDate.Value = this.ToDate.Value;
            }
        }

        private void fTree_EVT_DateSelectCallback(object sender, CmdDateSelectEventArgs e)
        {
            if (string.IsNullOrEmpty(e.date))
            {
                this.FilePanel.Controls.Clear();
                return;
            }
            this.IsByDate = true;
            DateTime dateTime = DateTime.Parse(e.date);
            this.FileDate1 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
            this.FileDate2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 99);
            this.LoadByDate();
        }

        private void GetSelectedCount()
        {
            this.SlideCount = 0;
            foreach (Control control in this.FilePanel.Controls)
            {
                if (!((Slide)control).sRecord.IsSelected)
                {
                    continue;
                }
                this.SlideCount++;
            }
        }

        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ListItem listItem = new ListItem();
            ListItem listItem1 = new ListItem();
            ListItem listItem2 = new ListItem();
            ListItem listItem3 = new ListItem();
            ListItem listItem4 = new ListItem();
            ListItem listItem5 = new ListItem();
            ListItem listItem6 = new ListItem();
            ListItem listItem7 = new ListItem();
            ListItem listItem8 = new ListItem();
            ListItem listItem9 = new ListItem();
            ListItem listItem10 = new ListItem();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Catalog));
            this.NavPanel = new vNavPane();
            this.imageList1 = new ImageList(this.components);
            this.FilePanel = new FlowLayoutPanel();
            this.FilePanelMenu = new ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new ToolStripSeparator();
            this.mnu_BackgroudColor = new ToolStripMenuItem();
            this.mnu_AddFile = new ToolStripMenuItem();
            this.mnu_ScanDoc = new ToolStripMenuItem();
            this.mnu_ClearSelections = new ToolStripMenuItem();
            this.mnu_ResetForm = new ToolStripMenuItem();
            this.Nav_Search = new vNavPaneItem();
            this.chkRating = new vCheckBox();
            this.chk_FilterEvidence = new vCheckBox();
            this.txtCAD = new vTextBox();
            this.lbl_CADId = new Label();
            this.txtRMS = new vTextBox();
            this.lbl_RMSId = new Label();
            this.lbl_Days = new Label();
            this.cboFileType = new vComboBox();
            this.lbl_ByFileExt = new Label();
            this.cboSecurity = new vComboBox();
            this.lbl_Security = new Label();
            this.btn_ResetSearch = new vButton();
            this.txtPhrase = new vTextBox();
            this.Rating = new vRatingControl();
            this.cboClassification = new vComboBox();
            this.lbl_Classification = new Label();
            this.ToDate = new vDateTimePicker();
            this.lbl_EndDate = new Label();
            this.FromDate = new vDateTimePicker();
            this.lbl_StartDate = new Label();
            this.lblLine1 = new Label();
            this.btn_QuickSearch = new vButton();
            this.cboDays = new vComboBox();
            this.Nav_Info = new vNavPaneItem();
            this.Nav_Sets = new vNavPaneItem();
            this.HeaderPanel = new Panel();
            this.progBar = new vProgressBar();
            this.OptionPanel = new Panel();
            this.btnRedact = new vButton();
            this.btnLanguage = new vButton();
            this.label1 = new Label();
            this.btnPassword = new vButton();
            this.lblVLine = new Label();
            this.btnScanner = new vButton();
            this.btn_Deselect = new vButton();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.lblTime = new Label();
            this.SetPic = new PictureBox();
            this.lbl_SelectedCount = new Label();
            this.lbl_Selected = new Label();
            this.lbl_Files = new Label();
            this.lbl_SlideCount = new Label();
            this.lbl_Sec = new Label();
            this.btn_Panel = new vButton();
            this.NavPanel.SuspendLayout();
            this.FilePanelMenu.SuspendLayout();
            this.Nav_Search.ItemPanel.SuspendLayout();
            this.Nav_Search.SuspendLayout();
            this.Nav_Info.SuspendLayout();
            this.Nav_Sets.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            this.OptionPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((ISupportInitialize)this.SetPic).BeginInit();
            base.SuspendLayout();
            this.NavPanel.Controls.Add(this.Nav_Search);
            this.NavPanel.Controls.Add(this.Nav_Info);
            this.NavPanel.Controls.Add(this.Nav_Sets);
            this.NavPanel.Dock = DockStyle.Left;
            this.NavPanel.ImageList = this.imageList1;
            this.NavPanel.Items.Add(this.Nav_Search);
            this.NavPanel.Items.Add(this.Nav_Info);
            this.NavPanel.Items.Add(this.Nav_Sets);
            this.NavPanel.Location = new Point(0, 46);
            this.NavPanel.Name = "NavPanel";
            this.NavPanel.Size = new Size(290, 589);
            this.NavPanel.TabIndex = 1;
            this.NavPanel.VIBlendTheme = VIBLEND_THEME.OFFICE2010BLACK;
            this.imageList1.ImageStream = (ImageListStreamer)Resources.Catalog.imageList1_ImageStream;
            this.imageList1.TransparentColor = Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "search.png");
            this.imageList1.Images.SetKeyName(1, "fileinfo.png");
            this.imageList1.Images.SetKeyName(2, "group.png");
            this.FilePanel.AllowDrop = true;
            this.FilePanel.AutoScroll = true;
            this.FilePanel.AutoSize = true;
            this.FilePanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.FilePanel.BackColor = Color.White;
            this.FilePanel.BorderStyle = BorderStyle.FixedSingle;
            this.FilePanel.ContextMenuStrip = this.FilePanelMenu;
            this.FilePanel.Dock = DockStyle.Fill;
            this.FilePanel.Location = new Point(290, 46);
            this.FilePanel.Name = "FilePanel";
            this.FilePanel.Size = new Size(481, 589);
            this.FilePanel.TabIndex = 2;
            this.FilePanel.TabStop = true;
            this.FilePanel.Scroll += new ScrollEventHandler(this.FilePanel_Scroll);
            this.FilePanel.ControlAdded += new ControlEventHandler(this.FilePanel_ControlAdded);
            this.FilePanel.ControlRemoved += new ControlEventHandler(this.FilePanel_ControlRemoved);
            this.FilePanel.Resize += new EventHandler(this.FilePanel_Resize);
            ToolStripItemCollection items = this.FilePanelMenu.Items;
            ToolStripItem[] mnuBackgroudColor = new ToolStripItem[] { this.mnu_BackgroudColor, this.mnu_AddFile, this.mnu_ScanDoc, this.toolStripMenuItem1, this.mnu_ClearSelections, this.mnu_ResetForm };
            this.FilePanelMenu.Items.AddRange(mnuBackgroudColor);
            this.FilePanelMenu.Name = "FilePanelMenu";
            this.FilePanelMenu.Size = new Size(175, 120);
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new Size(171, 6);
            this.mnu_BackgroudColor.Image = Properties.Resources.color;
            this.mnu_BackgroudColor.Name = "mnu_BackgroudColor";
            this.mnu_BackgroudColor.Size = new Size(174, 22);
            this.mnu_BackgroudColor.Text = "Background Color";
            this.mnu_BackgroudColor.Click += new EventHandler(this.mnu_BackgroudColor_Click);
            this.mnu_AddFile.Image = Properties.Resources.savefile;
            this.mnu_AddFile.Name = "mnu_AddFile";
            this.mnu_AddFile.Size = new Size(174, 22);
            this.mnu_AddFile.Text = "Add File...";
            this.mnu_AddFile.Click += new EventHandler(this.mnu_AddFile_Click);
            this.mnu_ScanDoc.Image = Properties.Resources.scanner1;
            this.mnu_ScanDoc.Name = "mnu_ScanDoc";
            this.mnu_ScanDoc.Size = new Size(174, 22);
            this.mnu_ScanDoc.Text = "Scan Document...";
            this.mnu_ScanDoc.Click += new EventHandler(this.mnu_ScanDoc_Click);
            this.mnu_ClearSelections.Image = Properties.Resources.clear;
            this.mnu_ClearSelections.Name = "mnu_ClearSelections";
            this.mnu_ClearSelections.Size = new Size(174, 22);
            this.mnu_ClearSelections.Text = "Clear All Selections";
            this.mnu_ClearSelections.Click += new EventHandler(this.mnu_ClearSelections_Click);
            this.mnu_ResetForm.Image = Properties.Resources.clear2;
            this.mnu_ResetForm.Name = "mnu_ResetForm";
            this.mnu_ResetForm.Size = new Size(174, 22);
            this.mnu_ResetForm.Text = "Reset";
            this.mnu_ResetForm.Click += new EventHandler(this.mnu_ResetForm_Click);
            this.Nav_Search.BackColor = Color.White;
            this.Nav_Search.HeaderText = "Search • File Date";
            this.Nav_Search.Image = Properties.Resources.search;
            this.Nav_Search.ItemPanel.AutoScroll = true;
            this.Nav_Search.ItemPanel.Controls.Add(this.chkRating);
            this.Nav_Search.ItemPanel.Controls.Add(this.chk_FilterEvidence);
            this.Nav_Search.ItemPanel.Controls.Add(this.txtCAD);
            this.Nav_Search.ItemPanel.Controls.Add(this.lbl_CADId);
            this.Nav_Search.ItemPanel.Controls.Add(this.txtRMS);
            this.Nav_Search.ItemPanel.Controls.Add(this.lbl_RMSId);
            this.Nav_Search.ItemPanel.Controls.Add(this.lbl_Days);
            this.Nav_Search.ItemPanel.Controls.Add(this.cboFileType);
            this.Nav_Search.ItemPanel.Controls.Add(this.lbl_ByFileExt);
            this.Nav_Search.ItemPanel.Controls.Add(this.cboSecurity);
            this.Nav_Search.ItemPanel.Controls.Add(this.lbl_Security);
            this.Nav_Search.ItemPanel.Controls.Add(this.btn_ResetSearch);
            this.Nav_Search.ItemPanel.Controls.Add(this.txtPhrase);
            this.Nav_Search.ItemPanel.Controls.Add(this.Rating);
            this.Nav_Search.ItemPanel.Controls.Add(this.cboClassification);
            this.Nav_Search.ItemPanel.Controls.Add(this.lbl_Classification);
            this.Nav_Search.ItemPanel.Controls.Add(this.ToDate);
            this.Nav_Search.ItemPanel.Controls.Add(this.lbl_EndDate);
            this.Nav_Search.ItemPanel.Controls.Add(this.FromDate);
            this.Nav_Search.ItemPanel.Controls.Add(this.lbl_StartDate);
            this.Nav_Search.ItemPanel.Controls.Add(this.lblLine1);
            this.Nav_Search.ItemPanel.Controls.Add(this.btn_QuickSearch);
            this.Nav_Search.ItemPanel.Controls.Add(this.cboDays);
            this.Nav_Search.ItemPanel.Location = new Point(1, 30);
            this.Nav_Search.ItemPanel.Name = "ItemPanel";
            this.Nav_Search.ItemPanel.Size = new Size(288, 498);
            this.Nav_Search.ItemPanel.TabIndex = 1;
            this.Nav_Search.Location = new Point(0, 0);
            this.Nav_Search.Name = "Nav_Search";
            this.Nav_Search.Size = new Size(290, 529);
            this.Nav_Search.TabIndex = 0;
            this.Nav_Search.Text = "Search - File Date";
            this.Nav_Search.TooltipText = "Search • File Date";
            this.chkRating.BackColor = Color.Transparent;
            this.chkRating.Location = new Point(16, 280);
            this.chkRating.Name = "chkRating";
            this.chkRating.Size = new Size(120, 24);
            this.chkRating.TabIndex = 24;
            this.chkRating.Text = "By Rating";
            this.chkRating.UseVisualStyleBackColor = false;
            this.chkRating.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.chkRating.CheckedChanged += new EventHandler(this.chkRating_CheckedChanged);
            this.chk_FilterEvidence.BackColor = Color.Transparent;
            this.chk_FilterEvidence.Location = new Point(16, 308);
            this.chk_FilterEvidence.Name = "chk_FilterEvidence";
            this.chk_FilterEvidence.Size = new Size(256, 24);
            this.chk_FilterEvidence.TabIndex = 23;
            this.chk_FilterEvidence.Text = "Marked as Evidence";
            this.chk_FilterEvidence.UseVisualStyleBackColor = false;
            this.chk_FilterEvidence.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.txtCAD.BackColor = Color.White;
            this.txtCAD.BoundsOffset = new Size(1, 1);
            this.txtCAD.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtCAD.DefaultText = "";
            this.txtCAD.Location = new Point(144, 252);
            this.txtCAD.MaxLength = 32;
            this.txtCAD.Name = "txtCAD";
            this.txtCAD.PasswordChar = '\0';
            this.txtCAD.ScrollBars = ScrollBars.None;
            this.txtCAD.SelectionLength = 0;
            this.txtCAD.SelectionStart = 0;
            this.txtCAD.Size = new Size(128, 23);
            this.txtCAD.TabIndex = 18;
            this.txtCAD.TextAlign = HorizontalAlignment.Left;
            this.txtCAD.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_CADId.Location = new Point(16, 257);
            this.lbl_CADId.Name = "lbl_CADId";
            this.lbl_CADId.Size = new Size(120, 13);
            this.lbl_CADId.TabIndex = 17;
            this.lbl_CADId.Text = "CAD ID";
            this.txtRMS.BackColor = Color.White;
            this.txtRMS.BoundsOffset = new Size(1, 1);
            this.txtRMS.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtRMS.DefaultText = "";
            this.txtRMS.Location = new Point(144, 224);
            this.txtRMS.MaxLength = 32;
            this.txtRMS.Name = "txtRMS";
            this.txtRMS.PasswordChar = '\0';
            this.txtRMS.ScrollBars = ScrollBars.None;
            this.txtRMS.SelectionLength = 0;
            this.txtRMS.SelectionStart = 0;
            this.txtRMS.Size = new Size(128, 23);
            this.txtRMS.TabIndex = 16;
            this.txtRMS.TextAlign = HorizontalAlignment.Left;
            this.txtRMS.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_RMSId.Location = new Point(16, 229);
            this.lbl_RMSId.Name = "lbl_RMSId";
            this.lbl_RMSId.Size = new Size(120, 13);
            this.lbl_RMSId.TabIndex = 15;
            this.lbl_RMSId.Text = "RMS ID";
            this.lbl_Days.AutoSize = true;
            this.lbl_Days.Location = new Point(213, 56);
            this.lbl_Days.Name = "lbl_Days";
            this.lbl_Days.Size = new Size(31, 13);
            this.lbl_Days.TabIndex = 3;
            this.lbl_Days.Text = "Days";
            this.cboFileType.BackColor = Color.White;
            this.cboFileType.DefaultText = "";
            this.cboFileType.DisplayMember = "";
            this.cboFileType.DropDownList = true;
            this.cboFileType.DropDownMaximumSize = new Size(1000, 1000);
            this.cboFileType.DropDownMinimumSize = new Size(10, 10);
            this.cboFileType.DropDownResizeDirection = SizingDirection.Both;
            this.cboFileType.DropDownWidth = 128;
            this.cboFileType.Location = new Point(144, 196);
            this.cboFileType.Name = "cboFileType";
            this.cboFileType.RoundedCornersMaskListItem = 15;
            this.cboFileType.Size = new Size(128, 23);
            this.cboFileType.TabIndex = 14;
            this.cboFileType.UseThemeBackColor = false;
            this.cboFileType.UseThemeDropDownArrowColor = true;
            this.cboFileType.ValueMember = "";
            this.cboFileType.VIBlendScrollBarsTheme = VIBLEND_THEME.VISTABLUE;
            this.cboFileType.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_ByFileExt.Location = new Point(16, 201);
            this.lbl_ByFileExt.Name = "lbl_ByFileExt";
            this.lbl_ByFileExt.Size = new Size(120, 13);
            this.lbl_ByFileExt.TabIndex = 13;
            this.lbl_ByFileExt.Text = "By File Type";
            this.cboSecurity.BackColor = Color.White;
            this.cboSecurity.DefaultText = "";
            this.cboSecurity.DisplayMember = "";
            this.cboSecurity.DropDownList = true;
            this.cboSecurity.DropDownMaximumSize = new Size(1000, 1000);
            this.cboSecurity.DropDownMinimumSize = new Size(10, 10);
            this.cboSecurity.DropDownResizeDirection = SizingDirection.Both;
            this.cboSecurity.DropDownWidth = 128;
            this.cboSecurity.Location = new Point(144, 168);
            this.cboSecurity.Name = "cboSecurity";
            this.cboSecurity.RoundedCornersMaskListItem = 15;
            this.cboSecurity.Size = new Size(128, 23);
            this.cboSecurity.TabIndex = 12;
            this.cboSecurity.UseThemeBackColor = false;
            this.cboSecurity.UseThemeDropDownArrowColor = true;
            this.cboSecurity.ValueMember = "";
            this.cboSecurity.VIBlendScrollBarsTheme = VIBLEND_THEME.VISTABLUE;
            this.cboSecurity.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_Security.Location = new Point(16, 173);
            this.lbl_Security.Name = "lbl_Security";
            this.lbl_Security.Size = new Size(120, 13);
            this.lbl_Security.TabIndex = 11;
            this.lbl_Security.Text = "Security Level";
            this.btn_ResetSearch.AllowAnimations = true;
            this.btn_ResetSearch.BackColor = Color.Transparent;
            this.btn_ResetSearch.Image = Properties.Resources.reset;
            this.btn_ResetSearch.ImageAlign = ContentAlignment.MiddleLeft;
            this.btn_ResetSearch.Location = new Point(10, 10);
            this.btn_ResetSearch.Name = "btn_ResetSearch";
            this.btn_ResetSearch.RoundedCornersMask = 15;
            this.btn_ResetSearch.RoundedCornersRadius = 0;
            this.btn_ResetSearch.Size = new Size(128, 30);
            this.btn_ResetSearch.TabIndex = 0;
            this.btn_ResetSearch.Text = "Reset";
            this.btn_ResetSearch.UseVisualStyleBackColor = false;
            this.btn_ResetSearch.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_ResetSearch.Click += new EventHandler(this.btn_ResetSearch_Click);
            this.txtPhrase.BackColor = Color.White;
            this.txtPhrase.BoundsOffset = new Size(1, 1);
            this.txtPhrase.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtPhrase.DefaultText = "Word or Phrase...";
            this.txtPhrase.Location = new Point(16, 342);
            this.txtPhrase.MaxLength = 1024;
            this.txtPhrase.Multiline = true;
            this.txtPhrase.Name = "txtPhrase";
            this.txtPhrase.PasswordChar = '\0';
            this.txtPhrase.ScrollBars = ScrollBars.None;
            this.txtPhrase.SelectionLength = 0;
            this.txtPhrase.SelectionStart = 0;
            this.txtPhrase.Size = new Size(256, 62);
            this.txtPhrase.TabIndex = 22;
            this.txtPhrase.TextAlign = HorizontalAlignment.Left;
            this.txtPhrase.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.Rating.Enabled = false;
            this.Rating.Location = new Point(144, 280);
            this.Rating.Name = "Rating";
            this.Rating.Shape = vRatingControlShapes.Star;
            this.Rating.Size = new Size(128, 23);
            this.Rating.TabIndex = 20;
            this.Rating.Value = 0f;
            this.Rating.ValueIndicatorSize = 11;
            this.Rating.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.cboClassification.BackColor = Color.White;
            this.cboClassification.DefaultText = "";
            this.cboClassification.DisplayMember = "";
            this.cboClassification.DropDownHeight = 250;
            this.cboClassification.DropDownList = true;
            this.cboClassification.DropDownMaximumSize = new Size(1000, 1000);
            this.cboClassification.DropDownMinimumSize = new Size(10, 10);
            this.cboClassification.DropDownResizeDirection = SizingDirection.Both;
            this.cboClassification.DropDownWidth = 128;
            this.cboClassification.Location = new Point(144, 140);
            this.cboClassification.Name = "cboClassification";
            this.cboClassification.RoundedCornersMaskListItem = 15;
            this.cboClassification.Size = new Size(128, 23);
            this.cboClassification.TabIndex = 10;
            this.cboClassification.UseThemeBackColor = false;
            this.cboClassification.UseThemeDropDownArrowColor = true;
            this.cboClassification.ValueMember = "";
            this.cboClassification.VIBlendScrollBarsTheme = VIBLEND_THEME.VISTABLUE;
            this.cboClassification.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_Classification.Location = new Point(16, 145);
            this.lbl_Classification.Name = "lbl_Classification";
            this.lbl_Classification.Size = new Size(120, 13);
            this.lbl_Classification.TabIndex = 9;
            this.lbl_Classification.Text = "Classification";
            this.ToDate.BackColor = Color.White;
            this.ToDate.BorderColor = Color.Black;
            this.ToDate.Culture = new CultureInfo("");
            this.ToDate.DefaultDateTimeFormat = DefaultDateTimePatterns.Custom;
            this.ToDate.DropDownMaximumSize = new Size(1000, 1000);
            this.ToDate.DropDownMinimumSize = new Size(10, 10);
            this.ToDate.DropDownResizeDirection = SizingDirection.None;
            this.ToDate.FormatValue = "";
            this.ToDate.Location = new Point(144, 112);
            this.ToDate.MaxDate = new DateTime(2100, 1, 1, 0, 0, 0, 0);
            this.ToDate.MinDate = new DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.ToDate.Name = "ToDate";
            this.ToDate.ShowGrip = false;
            this.ToDate.Size = new Size(128, 23);
            this.ToDate.TabIndex = 8;
            this.ToDate.Text = "07/14/2014 17:46:50";
            this.ToDate.UseThemeBackColor = false;
            this.ToDate.UseThemeDropDownArrowColor = true;
            this.ToDate.Value = new DateTime(2017, 08, 03);
            this.ToDate.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.ToDate.ValueChanged += new EventHandler(this.ToDate_ValueChanged);
            this.lbl_EndDate.Location = new Point(16, 117);
            this.lbl_EndDate.Name = "lbl_EndDate";
            this.lbl_EndDate.Size = new Size(120, 13);
            this.lbl_EndDate.TabIndex = 7;
            this.lbl_EndDate.Text = "To Date/Time";
            this.FromDate.BackColor = Color.White;
            this.FromDate.BorderColor = Color.Black;
            this.FromDate.Culture = new CultureInfo("");
            this.FromDate.DefaultDateTimeFormat = DefaultDateTimePatterns.Custom;
            this.FromDate.DropDownMaximumSize = new Size(1000, 1000);
            this.FromDate.DropDownMinimumSize = new Size(10, 10);
            this.FromDate.DropDownResizeDirection = SizingDirection.None;
            this.FromDate.FormatValue = "";
            this.FromDate.Location = new Point(144, 84);
            this.FromDate.MaxDate = new DateTime(2100, 1, 1, 0, 0, 0, 0);
            this.FromDate.MinDate = new DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.FromDate.Name = "FromDate";
            this.FromDate.ShowGrip = false;
            this.FromDate.Size = new Size(128, 23);
            this.FromDate.TabIndex = 6;
            this.FromDate.Text = "07/14/2014 17:46:50";
            this.FromDate.UseThemeBackColor = false;
            this.FromDate.UseThemeDropDownArrowColor = true;
            this.FromDate.Value = new DateTime(2017, 08, 03);
            this.FromDate.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.FromDate.ValueChanged += new EventHandler(this.FromDate_ValueChanged);
            this.lbl_StartDate.Location = new Point(16, 89);
            this.lbl_StartDate.Name = "lbl_StartDate";
            this.lbl_StartDate.Size = new Size(120, 13);
            this.lbl_StartDate.TabIndex = 5;
            this.lbl_StartDate.Text = "From Date/Time";
            this.lblLine1.BorderStyle = BorderStyle.Fixed3D;
            this.lblLine1.Location = new Point(16, 75);
            this.lblLine1.Name = "lblLine1";
            this.lblLine1.Size = new Size(255, 1);
            this.lblLine1.TabIndex = 4;
            this.btn_QuickSearch.AllowAnimations = true;
            this.btn_QuickSearch.BackColor = Color.Transparent;
            this.btn_QuickSearch.Image = Properties.Resources.search;
            this.btn_QuickSearch.ImageAlign = ContentAlignment.MiddleLeft;
            this.btn_QuickSearch.Location = new Point(144, 10);
            this.btn_QuickSearch.Name = "btn_QuickSearch";
            this.btn_QuickSearch.RoundedCornersMask = 15;
            this.btn_QuickSearch.RoundedCornersRadius = 0;
            this.btn_QuickSearch.Size = new Size(128, 30);
            this.btn_QuickSearch.TabIndex = 1;
            this.btn_QuickSearch.Text = "Search";
            this.btn_QuickSearch.UseVisualStyleBackColor = false;
            this.btn_QuickSearch.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_QuickSearch.Click += new EventHandler(this.btn_QuickSearch_Click);
            this.cboDays.BackColor = Color.White;
            this.cboDays.DefaultText = "";
            this.cboDays.DisplayMember = "";
            this.cboDays.DropDownList = true;
            this.cboDays.DropDownMaximumSize = new Size(1000, 1000);
            this.cboDays.DropDownMinimumSize = new Size(10, 10);
            this.cboDays.DropDownResizeDirection = SizingDirection.Both;
            this.cboDays.DropDownWidth = 64;
            listItem.RoundedCornersMask = 15;
            listItem.Text = "1";
            listItem1.RoundedCornersMask = 15;
            listItem1.Text = "2";
            listItem2.RoundedCornersMask = 15;
            listItem2.Text = "3";
            listItem3.RoundedCornersMask = 15;
            listItem3.Text = "5";
            listItem4.RoundedCornersMask = 15;
            listItem4.Text = "10";
            listItem5.RoundedCornersMask = 15;
            listItem5.Text = "15";
            listItem6.RoundedCornersMask = 15;
            listItem6.Text = "20";
            listItem7.RoundedCornersMask = 15;
            listItem7.Text = "30";
            listItem8.RoundedCornersMask = 15;
            listItem8.Text = "60";
            listItem9.RoundedCornersMask = 15;
            listItem9.Text = "90";
            listItem10.RoundedCornersMask = 15;
            listItem10.Text = "120";
            this.cboDays.Items.Add(listItem);
            this.cboDays.Items.Add(listItem1);
            this.cboDays.Items.Add(listItem2);
            this.cboDays.Items.Add(listItem3);
            this.cboDays.Items.Add(listItem4);
            this.cboDays.Items.Add(listItem5);
            this.cboDays.Items.Add(listItem6);
            this.cboDays.Items.Add(listItem7);
            this.cboDays.Items.Add(listItem8);
            this.cboDays.Items.Add(listItem9);
            this.cboDays.Items.Add(listItem10);
            this.cboDays.Location = new Point(144, 46);
            this.cboDays.Name = "cboDays";
            this.cboDays.RoundedCornersMaskListItem = 15;
            this.cboDays.Size = new Size(64, 23);
            this.cboDays.TabIndex = 2;
            this.cboDays.Text = "1";
            this.cboDays.UseThemeBackColor = false;
            this.cboDays.UseThemeDropDownArrowColor = true;
            this.cboDays.ValueMember = "";
            this.cboDays.VIBlendScrollBarsTheme = VIBLEND_THEME.VISTABLUE;
            this.cboDays.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.cboDays.SelectedIndexChanged += new EventHandler(this.cboDays_SelectedIndexChanged);
            this.Nav_Info.BackColor = Color.White;
            this.Nav_Info.HeaderText = "Search • Upload Date";
            this.Nav_Info.Image = (Image)Resources.Catalog.Nav_Info_Image;
            this.Nav_Info.ImageIndex = 1;
            this.Nav_Info.ItemPanel.AutoScroll = true;
            this.Nav_Info.ItemPanel.Location = new Point(1, 30);
            this.Nav_Info.ItemPanel.Name = "ItemPanel";
            this.Nav_Info.ItemPanel.Size = new Size(288, 0);
            this.Nav_Info.ItemPanel.TabIndex = 1;
            this.Nav_Info.Location = new Point(0, 529);
            this.Nav_Info.Name = "Nav_Info";
            this.Nav_Info.Size = new Size(290, 30);
            this.Nav_Info.TabIndex = 1;
            this.Nav_Info.Text = "vNavPaneItem2";
            this.Nav_Info.TooltipText = "Search • Upload Date";
            this.Nav_Sets.BackColor = Color.White;
            this.Nav_Sets.HeaderText = "Search • Sets";
            this.Nav_Sets.Image = (Image)Resources.Catalog.Nav_Sets_Image;
            this.Nav_Sets.ImageIndex = 2;
            this.Nav_Sets.ItemPanel.AutoScroll = true;
            this.Nav_Sets.ItemPanel.Location = new Point(1, 30);
            this.Nav_Sets.ItemPanel.Name = "ItemPanel";
            this.Nav_Sets.ItemPanel.Size = new Size(288, 0);
            this.Nav_Sets.ItemPanel.TabIndex = 1;
            this.Nav_Sets.Location = new Point(0, 559);
            this.Nav_Sets.Name = "Nav_Sets";
            this.Nav_Sets.Size = new Size(290, 30);
            this.Nav_Sets.TabIndex = 2;
            this.Nav_Sets.Text = "vNavPaneItem3";
            this.Nav_Sets.TooltipText = "Search • Sets";
            this.HeaderPanel.BackColor = Color.Silver;
            this.HeaderPanel.BorderStyle = BorderStyle.FixedSingle;
            this.HeaderPanel.Controls.Add(this.progBar);
            this.HeaderPanel.Controls.Add(this.OptionPanel);
            this.HeaderPanel.Controls.Add(this.lblVLine);
            this.HeaderPanel.Controls.Add(this.btnScanner);
            this.HeaderPanel.Controls.Add(this.btn_Deselect);
            this.HeaderPanel.Controls.Add(this.tableLayoutPanel1);
            this.HeaderPanel.Controls.Add(this.btn_Panel);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Margin = new Padding(0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(771, 46);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.Paint += new PaintEventHandler(this.HeaderPanel_Paint);
            this.progBar.BackColor = Color.Transparent;
            this.progBar.Location = new Point(295, 19);
            this.progBar.Name = "progBar";
            this.progBar.RoundedCornersMask = 15;
            this.progBar.RoundedCornersRadius = 0;
            this.progBar.Size = new Size(211, 20);
            this.progBar.TabIndex = 6;
            this.progBar.Text = "vProgressBar1";
            this.progBar.Value = 0;
            this.progBar.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.progBar.Visible = false;
            this.OptionPanel.BackColor = Color.Transparent;
            this.OptionPanel.Controls.Add(this.btnRedact);
            this.OptionPanel.Controls.Add(this.btnLanguage);
            this.OptionPanel.Controls.Add(this.label1);
            this.OptionPanel.Controls.Add(this.btnPassword);
            this.OptionPanel.Location = new Point(156, 3);
            this.OptionPanel.Name = "OptionPanel";
            this.OptionPanel.Size = new Size(133, 38);
            this.OptionPanel.TabIndex = 5;
            this.OptionPanel.Visible = false;
            this.btnRedact.AllowAnimations = true;
            this.btnRedact.BackColor = Color.Transparent;
            this.btnRedact.Image = Properties.Resources.palette;
            this.btnRedact.Location = new Point(97, 1);
            this.btnRedact.Name = "btnRedact";
            this.btnRedact.PaintDefaultFill = false;
            this.btnRedact.RoundedCornersMask = 15;
            this.btnRedact.RoundedCornersRadius = 0;
            this.btnRedact.Size = new Size(36, 36);
            this.btnRedact.TabIndex = 5;
            this.btnRedact.UseVisualStyleBackColor = false;
            this.btnRedact.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnRedact.Visible = false;
            this.btnRedact.Click += new EventHandler(this.btnRedact_Click);
            this.btnLanguage.AllowAnimations = true;
            this.btnLanguage.BackColor = Color.Transparent;
            this.btnLanguage.Image = Properties.Resources.globe;
            this.btnLanguage.Location = new Point(51, 1);
            this.btnLanguage.Name = "btnLanguage";
            this.btnLanguage.PaintDefaultFill = false;
            this.btnLanguage.RoundedCornersMask = 15;
            this.btnLanguage.RoundedCornersRadius = 0;
            this.btnLanguage.Size = new Size(36, 36);
            this.btnLanguage.TabIndex = 4;
            this.btnLanguage.UseVisualStyleBackColor = false;
            this.btnLanguage.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnLanguage.Click += new EventHandler(this.btnLanguage_Click);
            this.label1.BackColor = Color.FromArgb(64, 64, 64);
            this.label1.Dock = DockStyle.Left;
            this.label1.Location = new Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(1, 38);
            this.label1.TabIndex = 5;
            this.btnPassword.AllowAnimations = true;
            this.btnPassword.BackColor = Color.Transparent;
            this.btnPassword.Image = Properties.Resources.Password;
            this.btnPassword.Location = new Point(9, 0);
            this.btnPassword.Name = "btnPassword";
            this.btnPassword.PaintDefaultFill = false;
            this.btnPassword.RoundedCornersMask = 15;
            this.btnPassword.RoundedCornersRadius = 0;
            this.btnPassword.Size = new Size(36, 36);
            this.btnPassword.TabIndex = 3;
            this.btnPassword.UseVisualStyleBackColor = false;
            this.btnPassword.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnPassword.Click += new EventHandler(this.btnPassword_Click);
            this.lblVLine.BackColor = Color.FromArgb(64, 64, 64);
            this.lblVLine.Location = new Point(95, 4);
            this.lblVLine.Name = "lblVLine";
            this.lblVLine.Size = new Size(1, 36);
            this.lblVLine.TabIndex = 4;
            this.btnScanner.AllowAnimations = true;
            this.btnScanner.BackColor = Color.Transparent;
            this.btnScanner.Image = Properties.Resources.scanner;
            this.btnScanner.Location = new Point(110, 4);
            this.btnScanner.Name = "btnScanner";
            this.btnScanner.PaintDefaultFill = false;
            this.btnScanner.RoundedCornersMask = 15;
            this.btnScanner.RoundedCornersRadius = 0;
            this.btnScanner.Size = new Size(36, 36);
            this.btnScanner.TabIndex = 2;
            this.btnScanner.UseVisualStyleBackColor = false;
            this.btnScanner.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btnScanner.Click += new EventHandler(this.btnScanner_Click);
            this.btn_Deselect.AllowAnimations = true;
            this.btn_Deselect.BackColor = Color.Transparent;
            this.btn_Deselect.Image = Properties.Resources.redo32;
            this.btn_Deselect.Location = new Point(48, 4);
            this.btn_Deselect.Name = "btn_Deselect";
            this.btn_Deselect.PaintDefaultFill = false;
            this.btn_Deselect.RoundedCornersMask = 15;
            this.btn_Deselect.RoundedCornersRadius = 0;
            this.btn_Deselect.Size = new Size(36, 36);
            this.btn_Deselect.TabIndex = 1;
            this.btn_Deselect.UseVisualStyleBackColor = false;
            this.btn_Deselect.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btn_Deselect.Click += new EventHandler(this.btn_Deselect_Click);
            this.tableLayoutPanel1.Anchor = AnchorStyles.Right;
            this.tableLayoutPanel1.BackColor = Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50f));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115f));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 75f));
            this.tableLayoutPanel1.Controls.Add(this.lblTime, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.SetPic, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_SelectedCount, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbl_Selected, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbl_Files, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_SlideCount, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_Sec, 1, 2);
            this.tableLayoutPanel1.Location = new Point(526, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333f));
            this.tableLayoutPanel1.Size = new Size(240, 43);
            this.tableLayoutPanel1.TabIndex = 3;
            this.lblTime.AutoSize = true;
            this.lblTime.Dock = DockStyle.Fill;
            this.lblTime.Location = new Point(168, 28);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new Size(69, 15);
            this.lblTime.TabIndex = 5;
            this.lblTime.Text = "0.0";
            this.lblTime.TextAlign = ContentAlignment.MiddleRight;
            this.SetPic.Dock = DockStyle.Fill;
            this.SetPic.Image = Properties.Resources.Set;
            this.SetPic.Location = new Point(0, 0);
            this.SetPic.Margin = new Padding(0);
            this.SetPic.Name = "SetPic";
            this.tableLayoutPanel1.SetRowSpan(this.SetPic, 3);
            this.SetPic.Size = new Size(50, 43);
            this.SetPic.SizeMode = PictureBoxSizeMode.CenterImage;
            this.SetPic.TabIndex = 4;
            this.SetPic.TabStop = false;
            this.SetPic.Visible = false;
            this.lbl_SelectedCount.Dock = DockStyle.Fill;
            this.lbl_SelectedCount.Location = new Point(168, 14);
            this.lbl_SelectedCount.Name = "lbl_SelectedCount";
            this.lbl_SelectedCount.Size = new Size(69, 14);
            this.lbl_SelectedCount.TabIndex = 1;
            this.lbl_SelectedCount.Text = "0";
            this.lbl_SelectedCount.TextAlign = ContentAlignment.MiddleRight;
            this.lbl_Selected.AutoSize = true;
            this.lbl_Selected.Dock = DockStyle.Fill;
            this.lbl_Selected.Location = new Point(53, 14);
            this.lbl_Selected.Name = "lbl_Selected";
            this.lbl_Selected.Size = new Size(109, 14);
            this.lbl_Selected.TabIndex = 3;
            this.lbl_Selected.Text = "Selected";
            this.lbl_Selected.TextAlign = ContentAlignment.MiddleRight;
            this.lbl_Files.AutoSize = true;
            this.lbl_Files.Dock = DockStyle.Fill;
            this.lbl_Files.Location = new Point(53, 0);
            this.lbl_Files.Name = "lbl_Files";
            this.lbl_Files.Size = new Size(109, 14);
            this.lbl_Files.TabIndex = 4;
            this.lbl_Files.Text = "Files";
            this.lbl_Files.TextAlign = ContentAlignment.MiddleRight;
            this.lbl_SlideCount.Dock = DockStyle.Fill;
            this.lbl_SlideCount.Location = new Point(168, 0);
            this.lbl_SlideCount.Name = "lbl_SlideCount";
            this.lbl_SlideCount.Size = new Size(69, 14);
            this.lbl_SlideCount.TabIndex = 2;
            this.lbl_SlideCount.Text = "0";
            this.lbl_SlideCount.TextAlign = ContentAlignment.MiddleRight;
            this.lbl_Sec.AutoSize = true;
            this.lbl_Sec.Dock = DockStyle.Fill;
            this.lbl_Sec.Location = new Point(53, 28);
            this.lbl_Sec.Name = "lbl_Sec";
            this.lbl_Sec.Size = new Size(109, 15);
            this.lbl_Sec.TabIndex = 6;
            this.lbl_Sec.Text = "Seconds";
            this.lbl_Sec.TextAlign = ContentAlignment.MiddleRight;
            this.btn_Panel.AllowAnimations = true;
            this.btn_Panel.BackColor = Color.Transparent;
            this.btn_Panel.Image = Properties.Resources.closeleft;
            this.btn_Panel.Location = new Point(6, 4);
            this.btn_Panel.Name = "btn_Panel";
            this.btn_Panel.PaintDefaultFill = false;
            this.btn_Panel.RoundedCornersMask = 15;
            this.btn_Panel.RoundedCornersRadius = 0;
            this.btn_Panel.Size = new Size(36, 36);
            this.btn_Panel.TabIndex = 0;
            this.btn_Panel.UseVisualStyleBackColor = false;
            this.btn_Panel.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.btn_Panel.Click += new EventHandler(this.btn_Panel_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.FilePanel);
            base.Controls.Add(this.NavPanel);
            base.Controls.Add(this.HeaderPanel);
            base.Name = "Catalog";
            base.Size = new Size(771, 635);
            base.Load += new EventHandler(this.Catalog_Load);
            this.NavPanel.ResumeLayout(false);
            this.FilePanelMenu.ResumeLayout(false);
            this.Nav_Search.ItemPanel.ResumeLayout(false);
            this.Nav_Search.ItemPanel.PerformLayout();
            this.Nav_Search.ResumeLayout(false);
            this.Nav_Info.ResumeLayout(false);
            this.Nav_Sets.ResumeLayout(false);
            this.HeaderPanel.ResumeLayout(false);
            this.OptionPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((ISupportInitialize)this.SetPic).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LoadByDate()
        {
            base.BeginInvoke(new MethodInvoker(() => {
                this.FilePanel.Controls.Clear();
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    using (RPM_DataFile rPMDataFile = new RPM_DataFile())
                    {
                        List<DataFile> dataFiles = new List<DataFile>();
                        dataFiles = (!this.IsByDate ? rPMDataFile.LoadByDate(this.Account_ID, this.FromDate.Value.Value, this.ToDate.Value.Value) : rPMDataFile.LoadByDate(this.Account_ID, this.FileDate1, this.FileDate2));
                        (new Thread(new ParameterizedThreadStart(this.LoadSlideList))).Start(dataFiles);
                    }
                }
                catch (Exception exception)
                {
                    Logger.Logging.WriteSystemLog(VMGlobal.LOG_ACTION.CODE_ERROR, exception.Message, this.Account_ID);
                }
                this.Cursor = Cursors.Default;
                this.IsByDate = false;
            }));
        }

        private void LoadClassifications()
        {
            try
            {
                this.cboClassification.Items.Clear();
                Global.ClassificationDays.Clear();
                ListItem listItem = new ListItem()
                {
                    Text = LangCtrl.GetString("cbo_ShowAll", "[Show All]")
                };
                this.cboClassification.Items.Add(listItem);
                using (RPM_GeneralData rPMGeneralDatum = new RPM_GeneralData())
                {
                    List<Classification> classificationList = rPMGeneralDatum.GetClassificationList();
                    if (classificationList.Count > 0)
                    {
                        foreach (Classification classification in classificationList)
                        {
                            listItem = new ListItem()
                            {
                                Text = classification.Name,
                                Tag = classification.Id
                            };
                            this.cboClassification.Items.Add(listItem);
                            if (!classification.IsRetention)
                            {
                                continue;
                            }
                            Global.ClassificationDays.Add(classification.Name, classification.Days);
                        }
                    }
                }
            }
            catch
            {
            }
            this.cboClassification.SelectedIndex = 0;
        }

        private void LoadFileTree()
        {
            this.fTree = new FileTree()
            {
                Account_ID = this.Account_ID
            };
            this.fTree.EVT_DateSelectCallback -= new FileTree.DEL_DateSelectCallback(this.fTree_EVT_DateSelectCallback);
            this.fTree.EVT_DateSelectCallback += new FileTree.DEL_DateSelectCallback(this.fTree_EVT_DateSelectCallback);
            this.Nav_Info.ItemPanel.Controls.Add(this.fTree);
        }

        private void LoadFileTypes()
        {
            this.cboFileType.Items.Clear();
            ListItem listItem = new ListItem()
            {
                Text = LangCtrl.GetString("cbo_ShowAll", "[Show All]")
            };
            this.cboFileType.Items.Add(listItem);
            using (RPM_DataFile rPMDataFile = new RPM_DataFile())
            {
                foreach (string fileExtension in rPMDataFile.GetFileExtensions())
                {
                    listItem = new ListItem()
                    {
                        Text = fileExtension
                    };
                    this.cboFileType.Items.Add(listItem);
                }
            }
            this.cboFileType.SelectedIndex = 0;
        }

        private void LoadSecurity()
        {
            this.cboSecurity.Items.Clear();
            ListItem listItem = new ListItem()
            {
                Text = LangCtrl.GetString("cbo_ShowAll", "[Show All]")
            };
            this.cboSecurity.Items.Add(listItem);
            foreach (string str in AccountSecurity.MaxSecurityLevel(Global.GlobalAccount.Security))
            {
                listItem = new ListItem()
                {
                    Text = str
                };
                this.cboSecurity.Items.Add(listItem);
            }
            this.cboSecurity.SelectedIndex = 0;
        }

        private void LoadSetSlides(string SetName)
        {
            base.BeginInvoke(new MethodInvoker(() => {
                try
                {
                    using (RPM_DataFile rPMDataFile = new RPM_DataFile())
                    {
                        List<DataFile> set = rPMDataFile.GetSet(this.Account_ID, SetName);
                        (new Thread(new ParameterizedThreadStart(this.LoadSlideList))).Start(set);
                    }
                }
                catch (Exception exception)
                {
                    Logger.Logging.WriteSystemLog(VMGlobal.LOG_ACTION.CODE_ERROR, exception.Message, this.Account_ID);
                }
            }));
        }

        private void LoadSetTree()
        {
            this.CatalogSet = new SetCtrl(this.Account_ID);
            this.CatalogSet.EVT_TreeNodeCallback += new SetCtrl.DEL_TreeNodeCallback(this.CatalogSet_EVT_TreeNodeCallback);
            this.CatalogSet.EVT_ClearPanel += new SetCtrl.DEL_ClearPanel(this.CatalogSet_EVT_ClearPanel);
            this.Nav_Sets.ItemPanel.Controls.Add(this.CatalogSet);
        }

        private void LoadSlideList(object obj)
        {
            this.CheckRetentionPolicy();
            base.BeginInvoke(new MethodInvoker(() => {
                this.progBar.Visible = true;
                this.FilePanel.Controls.Clear();
                this.UpdateData();
                Application.DoEvents();
                this.Cursor = Cursors.WaitCursor;
                List<DataFile> dataFiles = (List<DataFile>)obj;
                this.stopwatch.Reset();
                this.stopwatch.Start();
                this.FilePanel.Controls.Clear();
                if (dataFiles.Count > 350)
                {
                    MessageBox.Show(this, LangCtrl.GetString("txt_LargeResults", "Result set too large, please refine your search parameters."), "Search", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                if (dataFiles.Count > 0)
                {
                    this.progBar.Maximum = dataFiles.Count;
                    int num = 0;
                    foreach (DataFile dataFile in dataFiles)
                    {
                        if (Global.GlobalAccount.Security > dataFile.Security)
                        {
                            continue;
                        }
                        try
                        {
                            SlideRecord slideRecord = new SlideRecord();
                            if (!this.IsRetentionPolicy || !Global.ClassificationDays.ContainsKey(dataFile.Classification))
                            {
                                slideRecord.ClassDays = -10000;
                            }
                            else
                            {
                                slideRecord.ClassDays = Global.ClassificationDays[dataFile.Classification];
                            }
                            slideRecord.IsDelete = this.IsDeleteAllowed;
                            slideRecord.dRecord = dataFile;
                            slideRecord.SlideNumber = num + 1;
                            slideRecord.IsSelected = false;
                            Slide slide = new Slide(slideRecord);
                            this.FilePanel.Controls.Add(slide);
                            slide.EVT_SlideCallback += new Slide.DEL_SlideCallback(this.s_EVT_SlideCallback);
                            slide.EVT_PackageCallback += new Slide.DEL_PackageCallback(this.s_EVT_PackageCallback);
                        }
                        catch (Exception exception)
                        {
                            Logger.Logging.WriteSystemLog(VMGlobal.LOG_ACTION.CODE_ERROR, exception.Message, this.Account_ID);
                        }
                        num++;
                        this.progBar.Value = num;
                        if (num % 10 != 0)
                        {
                            continue;
                        }
                        this.FilePanel.Refresh();
                        Application.DoEvents();
                    }
                }
                this.IsNoResults = false;
                if (this.FilePanel.Controls.Count == 0)
                {
                    Label label = new Label()
                    {
                        Font = new Font(FontFamily.GenericSerif, 20f),
                        Height = 30,
                        Width = 400,
                        Text = LangCtrl.GetString("txt_NoResults", "No Results Found!")
                    };
                    this.lbl_SlideCount.Text = "0";
                    this.IsNoResults = true;
                    this.FilePanel.Controls.Add(label);
                }
                this.FilePanel.ResumeLayout();
                this.FilePanel.Refresh();
                Application.DoEvents();
                this.Cursor = Cursors.Default;
                this.stopwatch.Stop();
                TimeSpan elapsed = this.stopwatch.Elapsed;
                this.lblTime.Text = string.Format("{0:0}:{1:00}.{2:000}", elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds);
                this.UpdateData();
                this.progBar.Visible = false;
                this.progBar.Value = 0;
            }));
        }

        private void LoadSlides()
        {
            base.BeginInvoke(new MethodInvoker(() => {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    using (RPM_DataFile rPMDataFile = new RPM_DataFile())
                    {
                        CatalogFilter catalogFilter = new CatalogFilter()
                        {
                            AccountID = this.Account_ID,
                            StartDate = this.FromDate.Value.Value,
                            EndDate = this.ToDate.Value.Value,
                            RMS = this.txtRMS.Text,
                            CAD = this.txtCAD.Text,
                            IsEvidence = this.chk_FilterEvidence.Checked,
                            IsSecurityFilter = false
                        };
                        if (this.cboSecurity.SelectedIndex > 0)
                        {
                            catalogFilter.IsSecurityFilter = true;
                            catalogFilter.SecurityLevel = AccountSecurity.GetByString(this.cboSecurity.Text);
                        }
                        else
                        {
                            catalogFilter.SecurityLevel = Global.GlobalAccount.Security;
                        }
                        if (this.cboClassification.SelectedIndex > 0)
                        {
                            catalogFilter.Classification = this.cboClassification.Text;
                        }
                        if (this.chkRating.Checked)
                        {
                            catalogFilter.Rating = (int)this.Rating.Value;
                        }
                        if (this.cboFileType.SelectedIndex > 0)
                        {
                            catalogFilter.FileType = this.cboFileType.Text;
                        }
                        catalogFilter.WordPhrase = this.txtPhrase.Text;
                        List<DataFile> dataFiles = rPMDataFile.QueryDataFile(catalogFilter);
                        (new Thread(new ParameterizedThreadStart(this.LoadSlideList))).Start(dataFiles);
                    }
                }
                catch (Exception exception)
                {
                    Logger.Logging.WriteSystemLog(VMGlobal.LOG_ACTION.CODE_ERROR, exception.Message, this.Account_ID);
                }
                this.Cursor = Cursors.Default;
            }));
        }

        private void mnu_AddFile_Click(object sender, EventArgs e)
        {
            (new DataUpload(this.Account_ID, "")).ShowDialog(this);
        }

        private void mnu_BackgroudColor_Click(object sender, EventArgs e)
        {
            BGColor bGColor = new BGColor()
            {
                ColorValue = this.FilePanel.BackColor
            };
            if (bGColor.ShowDialog(this) == DialogResult.OK)
            {
                this.FilePanel.BackColor = bGColor.ColorValue;
            }
        }

        private void mnu_ClearSelections_Click(object sender, EventArgs e)
        {
            this.ClearSelections();
        }

        private void mnu_ResetForm_Click(object sender, EventArgs e)
        {
            this.ResetForm();
        }

        private void mnu_ScanDoc_Click(object sender, EventArgs e)
        {
            this.ScanDoc();
        }

        private void ProcessExport(string filepath)
        {
            try
            {
                List<Guid> guids = new List<Guid>();
                foreach (Control control in this.FilePanel.Controls)
                {
                    Slide slide = (Slide)control;
                    if (!slide.sRecord.IsSelected)
                    {
                        continue;
                    }
                    guids.Add(slide.sRecord.dRecord.Id);
                }
                if (guids.Count > 0)
                {
                    ExportForm exportForm = new ExportForm()
                    {
                        FileID = guids,
                        FolderPath = filepath
                    };
                    exportForm.Show(this);
                }
            }
            catch (Exception exception)
            {
                string message = exception.Message;
            }
        }

        private void QuickSearch()
        {
            (new Thread(new ThreadStart(this.LoadSlides))).Start();
        }

        public void Reload_Lookups()
        {
            this.LoadClassifications();
            this.LoadFileTypes();
            this.LoadSecurity();
        }

        private void RemoveSlides()
        {
            try
            {
                for (int i = this.FilePanel.Controls.Count - 1; i >= 0; i--)
                {
                    Slide item = (Slide)this.FilePanel.Controls[i];
                    if (item.sRecord.IsSelected)
                    {
                        this.FilePanel.Controls.Remove(item);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Logging.WriteSystemLog(VMGlobal.LOG_ACTION.CODE_ERROR, exception.Message, this.Account_ID);
            }
        }

        private void ResetDates()
        {
            vDateTimePicker fromDate = this.FromDate;
            DateTime now = DateTime.Now;
            fromDate.Value = new DateTime?(now.AddDays(-30));
            this.ToDate.Value = new DateTime?(DateTime.Now);
        }

        private void ResetForm()
        {
            this.FilePanel.Controls.Clear();
            this.lblTime.Text = "0:00.000";
            this.lbl_SlideCount.Text = "0";
            this.lbl_SelectedCount.Text = "0";
            FormCtrl.ClearForm(this);
            this.ResetDates();
            this.CatalogSet.ResetDates();
            this.Rating.Value = 0f;
            this.cboClassification.SelectedIndex = 0;
            this.cboFileType.SelectedIndex = 0;
            this.cboSecurity.SelectedIndex = 0;
            GC.Collect();
        }

        private void s_EVT_PackageCallback(object sender, CmdPackageEventArgs e)
        {
            this.ProcessExport(e.@value);
        }

        private void s_EVT_SlideCallback(object sender, CmdSlideEventArgs e)
        {
            this.UpdateSlideData(e.SlideRecord);
            switch (e.SlideRecord.Action)
            {
                case ACTION.SELECT:
                    {
                        this.GetSelectedCount();
                        break;
                    }
                case ACTION.SHARE:
                    {
                        e.SlideRecord.Action = ACTION.NONE;
                        this.ShareFiles();
                        break;
                    }
                case ACTION.RANGE:
                    {
                        this.SelectRange(e.SlideRecord.SlideNumber);
                        break;
                    }
            }
            this.btn_Deselect.Enabled = this.SlideCount != 0;
            this.SetPic.Visible = this.SlideCount > 1;
            this.UpdateData();
        }

        private void ScanDoc()
        {
            Scanner scanner = new Scanner()
            {
                AccountID = this.Account_ID
            };
            scanner.ShowDialog(this);
        }

        private void SecurityProfile()
        {
            if (!Global.IsRights(Global.RightsProfile, UserRights.IMPORT))
            {
                this.mnu_AddFile.Visible = false;
            }
            if (!Global.IsRights(Global.RightsProfile, UserRights.SCAN))
            {
                this.mnu_ScanDoc.Visible = false;
                this.btnScanner.Visible = false;
            }
        }

        private void selector_EVT_AccountSelectorCallback(object sender, CmdAccountSelectorEventArgs e)
        {
            List<Guid> guids = new List<Guid>();
            foreach (Control control in this.FilePanel.Controls)
            {
                Slide slide = (Slide)control;
                if (!slide.sRecord.IsSelected)
                {
                    continue;
                }
                guids.Add(slide.GetRecordID());
            }
            bool flag = false;
            string empty = string.Empty;
            using (RPM_Account rPMAccount = new RPM_Account())
            {
                Account account = rPMAccount.GetAccount(e.@value);
                empty = string.Format("{0} • {1}", account.ToString(), account.BadgeNumber);
                if (Global.IsRights(account.ApplicationRights, UserRights.SHARE))
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                MessageBox.Show(this, string.Format(LangCtrl.GetString("txt_NoShareRights", "Account: {0}\nShare rights not enabled."), empty), "Account", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (guids.Count > 0)
            {
                ShareForm shareForm = new ShareForm();
                shareForm.ShareFiles(guids, e.@value);
                shareForm.ShowDialog(this);
                return;
            }
        }

        private void SelectRange(int EndSlide)
        {
            int num = 1;
            bool flag = false;
            this.FilePanel.SuspendLayout();
            foreach (Control control in this.FilePanel.Controls)
            {
                Slide slide = (Slide)control;
                if (slide.sRecord.IsSelected)
                {
                    flag = true;
                }
                else if (flag && num <= EndSlide)
                {
                    slide.SelectSlide();
                }
                num++;
            }
            this.FilePanel.ResumeLayout();
            this.GetSelectedCount();
        }

        public void SetLanguage()
        {
            LangCtrl.reText(this);
            this.txtPhrase.DefaultText = LangCtrl.GetString("ui_WordPhrase", "Word or Phrase...");
            this.Nav_Info.HeaderText = LangCtrl.GetString("Nav_Info", "Search • File Date");
            this.Nav_Search.HeaderText = LangCtrl.GetString("Nav_Search", "Search • Upload Date");
            this.Nav_Sets.HeaderText = LangCtrl.GetString("Nav_Sets", "Search • Sets");
            this.mnu_BackgroudColor.Text = LangCtrl.GetString("mnu_BackgroudColor", "Background Color");
            this.mnu_AddFile.Text = LangCtrl.GetString("mnu_AddFile", "Add File");
            this.mnu_ScanDoc.Text = LangCtrl.GetString("mnu_ScanDoc", "Scan Document");
            this.mnu_ClearSelections.Text = LangCtrl.GetString("mnu_ClearSelections", "Clear Selections");
            this.mnu_ResetForm.Text = LangCtrl.GetString("mnu_ResetForm", "Reset");
            this.SetTooltips();
            LangCtrl.reText(this.FilePanelMenu);
        }

        private void SetTooltips()
        {
            this.tt.RemoveAll();
            this.tt.UseFading = true;
            this.tt.SetToolTip(this.btn_Panel, LangCtrl.GetString("tt_Panel", "Show/Hide Search Panel"));
            this.tt.SetToolTip(this.btn_Deselect, LangCtrl.GetString("tt_Deselect", "Unselect all Files"));
            this.tt.SetToolTip(this.btnScanner, LangCtrl.GetString("tt_Scanner", "Scan Documents"));
            this.tt.SetToolTip(this.btnPassword, LangCtrl.GetString("tt_Password", "Change password or PIN"));
            this.tt.SetToolTip(this.btnLanguage, LangCtrl.GetString("tt_Language", "Change Language"));
            this.tt.SetToolTip(this.btnRedact, LangCtrl.GetString("tt_Redact", "Redact Media File"));
        }

        private void ShareFiles()
        {
            SelectorForm selectorForm = new SelectorForm();
            selectorForm.EVT_AccountSelectorCallback -= new SelectorForm.DEL_AccountSelectorCallback(this.selector_EVT_AccountSelectorCallback);
            selectorForm.EVT_AccountSelectorCallback += new SelectorForm.DEL_AccountSelectorCallback(this.selector_EVT_AccountSelectorCallback);
            selectorForm.ShowDialog(this);
            selectorForm.EVT_AccountSelectorCallback -= new SelectorForm.DEL_AccountSelectorCallback(this.selector_EVT_AccountSelectorCallback);
        }

        void System.IDisposable.Dispose()
        {
            try
            {
                this.fTree.EVT_DateSelectCallback -= new FileTree.DEL_DateSelectCallback(this.fTree_EVT_DateSelectCallback);
                this.CatalogSet.EVT_TreeNodeCallback -= new SetCtrl.DEL_TreeNodeCallback(this.CatalogSet_EVT_TreeNodeCallback);
                this.CatalogSet.EVT_ClearPanel -= new SetCtrl.DEL_ClearPanel(this.CatalogSet_EVT_ClearPanel);
            }
            catch
            {
            }
        }

        private void ToDate_ValueChanged(object sender, EventArgs e)
        {
            DateTime? value = this.ToDate.Value;
            DateTime? nullable = this.FromDate.Value;
            if ((value.HasValue & nullable.HasValue ? value.GetValueOrDefault() < nullable.GetValueOrDefault() : false))
            {
                this.ToDate.Value = this.FromDate.Value;
            }
        }

        private void UpdateData()
        {
            if (!this.IsNoResults)
            {
                this.lbl_SlideCount.Text = string.Format("{0}", this.FilePanel.Controls.Count);
            }
            else
            {
                this.lbl_SlideCount.Text = "0";
            }
            this.lbl_SelectedCount.Text = string.Format("{0}", this.SlideCount);
        }

        private void UpdateLanguageCallback()
        {
            if (this.EVT_UpdateLanguage != null)
            {
                this.EVT_UpdateLanguage();
            }
        }

        private void UpdateSelectedSlideRecords(SlideRecord rec)
        {
            bool flag = false;
            if (rec.IsDescUpdate && this.SlideCount > 1 && MessageBox.Show(this, LangCtrl.GetString("txt_UpdateDesc", "Update description for selected files?"), LangCtrl.GetString("txt_ShortDesc", "Short Description"), MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                flag = true;
            }
            try
            {
                this.Cursor = Cursors.WaitCursor;
                using (RPM_DataFile rPMDataFile = new RPM_DataFile())
                {
                    foreach (Control control in this.FilePanel.Controls)
                    {
                        Slide slide = (Slide)control;
                        slide.sRecord.Action = ACTION.NONE;
                        if (!slide.sRecord.IsSelected)
                        {
                            continue;
                        }
                        DataFile dataFile = rPMDataFile.GetDataFile(slide.sRecord.dRecord.Id);
                        dataFile.Classification = rec.dRecord.Classification;
                        dataFile.Security = rec.dRecord.Security;
                        if (flag)
                        {
                            dataFile.ShortDesc = rec.dRecord.ShortDesc;
                        }
                        dataFile.SetName = rec.dRecord.SetName;
                        dataFile.Rating = rec.dRecord.Rating;
                        dataFile.CADNumber = (rec.dRecord.CADNumber);
                        dataFile.RMSNumber = rec.dRecord.RMSNumber;
                        dataFile.IsIndefinite = rec.dRecord.IsIndefinite;
                        dataFile.IsEvidence = rec.dRecord.IsEvidence;
                        rPMDataFile.SaveUpdate(dataFile);
                        rPMDataFile.Save();
                        slide.UpdateSlideData(dataFile);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Logging.WriteSystemLog(VMGlobal.LOG_ACTION.CODE_ERROR, exception.Message, this.Account_ID);
            }
            this.Cursor = Cursors.Default;
        }

        private void UpdateSlideData(SlideRecord rec)
        {
            foreach (Control control in this.FilePanel.Controls)
            {
                Slide slide = (Slide)control;
                if (!slide.sRecord.IsSelected)
                {
                    continue;
                }
                switch (rec.Action)
                {
                    case ACTION.DELETE:
                        {
                            slide.sRecord.Action = ACTION.NONE;
                            this.FileDelete();
                            continue;
                        }
                    case ACTION.REMOVE:
                        {
                            this.RemoveSlides();
                            continue;
                        }
                    case ACTION.PLAYLIST:
                        {
                            this.VideoPlaylist();
                            slide.sRecord.Action = ACTION.NONE;
                            continue;
                        }
                    case ACTION.UPDATE:
                        {
                            rec.IsDelete = this.IsDeleteAllowed;
                            this.UpdateSelectedSlideRecords(rec);
                            continue;
                        }
                    default:
                        {
                            continue;
                        }
                }
            }
        }

        private void VideoPlaylist()
        {
            this.Cursor = Cursors.WaitCursor;
            List<Guid> guids = new List<Guid>();
            foreach (Control control in this.FilePanel.Controls)
            {
                Slide slide = (Slide)control;
                if (!slide.sRecord.IsSelected)
                {
                    continue;
                }
                slide.sRecord.Action = ACTION.NONE;
                guids.Add(slide.GetRecordID());
            }
            Logger.Logging.WriteSystemLog(VMGlobal.LOG_ACTION.VIDEO, LangCtrl.GetString("log_OpenVideoPlayer", "Open Video Player"), this.Account_ID);
            try
            {
                VideoForm videoForm = new VideoForm();
                videoForm.LoadVideoList(guids, this.Account_ID);
                videoForm.ShowDialog(this);
            }
            catch (Exception exception)
            {
                string message = exception.Message;
            }
            this.Cursor = Cursors.Default;
        }

        public event Catalog.DEL_UpdateLanguage EVT_UpdateLanguage;

        public delegate void DEL_UpdateLanguage();
    }
}