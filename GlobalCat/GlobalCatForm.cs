using AppGlobal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMInterfaces;
using VMModels.Enums;
using VMModels.Model;
using WinReg;

namespace GlobalCat
{
    public class GlobalCatForm : Form
    {
        private IContainer components;
        private Panel HeaderPanel;
        private vButton btn_Reset;
        private vButton btn_Search;
        private Panel SearchPanel;
        private Panel DataPanel;
        private vCheckBox chk_FilterEvidence;
        private vTextBox txtCAD;
        private Label gc_CADId;
        private vTextBox txtRMS;
        private Label gc_RMSId;
        private Label gc_Days;
        private vDateTimePicker ToDate;
        private Label gc_EndDate;
        private vDateTimePicker FromDate;
        private Label gc_StartDate;
        private Label lblLine1;
        private vComboBox cboDays;
        private Label gcFileCount;
        private Label gc_Accounts;
        private vTextBox txtSet;
        private Label gc_Set;
        private vRadioButton btnByIngestDate;
        private vRadioButton btnByFileDate;
        private Label gc_Search;

        
        public GlobalCatForm()
        {
            InitializeComponent();
        }

        
        private void GlobalCatForm_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
                this.HeaderPanel.BackgroundImage = Properties.Resources.topbar45;
            try
            {
                Text = LangCtrl.GetString("dlg_GlobalCatalog", "Global Catalog Search");
                LangCtrl.reText(this);
                gcFileCount.Text = string.Format(LangCtrl.GetString("gcFileCount", "Files: {0}"), 0);
                gc_Accounts.Text = string.Format(LangCtrl.GetString("gc_Accounts", "Accounts: {0}"), 0);
                LoadRegData();
                ResetDates();
            }
            catch
            {
            }
        }

        
        private void cboDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboDays.SelectedIndex <= -1)
                    return;
                FromDate.Value = new DateTime?(DateTime.Now.AddDays((double)-Convert.ToInt32(cboDays.Text)));
                ToDate.Value = new DateTime?(DateTime.Now);
            }
            catch
            {
            }
        }

        
        private void ResetDates()
        {
            try
            {
                cboDays.SelectedIndex = 7;
                FromDate.Value = new DateTime?(DateTime.Now.AddDays(-30.0));
                ToDate.Value = new DateTime?(DateTime.Now);
            }
            catch
            {
            }
        }

        
        private void btn_Reset_Click(object sender, EventArgs e)
        {
            Global.Log("SEARCH", "Clear Search Parameters");
            ResetForm();
        }

        
        private void ResetForm()
        {
            try
            {
                FormCtrl.ClearForm(this);
                ResetDates();
                gcFileCount.Text = string.Format(LangCtrl.GetString("gcFileCount", "Files: {0}"), 0);
                gc_Accounts.Text = string.Format(LangCtrl.GetString("gc_Accounts", "Accounts: {0}"), 0);
                DataPanel.Controls.Clear();
                btnByFileDate.Checked = true;
            }
            catch
            {
            }
        }

        
        private bool ValidateSearch()
        {
            bool flag = true;
            if (string.IsNullOrEmpty(txtCAD.Text) && string.IsNullOrEmpty(txtRMS.Text) && string.IsNullOrEmpty(txtSet.Text))
            {
                MessageBox.Show(this, LangCtrl.GetString("gc_msg1", "Search requires either RMS or CAD number."), "Search", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                txtRMS.Select();
                flag = false;
            }
            return flag;
        }

        
        private void btn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                int num1 = 0;
                int num2 = 0;
                if (ValidateSearch())
                {
                    DataPanel.Controls.Clear();
                    DataPanel.SuspendLayout();
                    SECURITY security = Global.GlobalAccount.Security;
                    Guid guid = Guid.Empty;
                    List<DataFile> dataFileList1 = new List<DataFile>();
                    string str = string.Empty;
                    using (RPM_DataFile rpmDataFile = new RPM_DataFile())
                    {
                        List<DataFile> dataFileList2 = rpmDataFile.QryGlobalCatalog(btnByFileDate.Checked, FromDate.Value.Value, ToDate.Value.Value, txtRMS.Text, txtCAD.Text, txtSet.Text, chk_FilterEvidence.Checked);
                        if (dataFileList2.Count > 0)
                        {
                            foreach (DataFile df in dataFileList2)
                            {
                                if (guid != df.AccountId)
                                {
                                    guid = df.AccountId;
                                    using (RPM_Account rpmAccount = new RPM_Account())
                                    {
                                        Account account = rpmAccount.GetAccount(df.AccountId);
                                        str = string.Format("{0} • {1}", account.ToString(), account.BadgeNumber);
                                        string data = string.Format("Rank: {0}", account.Rank);
                                        AccountPanel accountPanel = new AccountPanel(str, data, account.Security);
                                        DataPanel.Controls.Add(accountPanel);
                                        DataPanel.Controls.SetChildIndex(accountPanel, 0);
                                        ++num2;
                                    }
                                }
                                if (security <= df.Security)
                                {
                                    CatalogObject catalogObject = new CatalogObject(num1 + 1, df, str);
                                    DataPanel.Controls.Add(catalogObject);
                                    DataPanel.Controls.SetChildIndex(catalogObject, 0);
                                    ++num1;
                                }
                            }
                        }
                        else
                        {
                            Label label = new Label();
                            label.Dock = DockStyle.Top;
                            label.Font = new Font("Verdana", 16f);
                            label.Text = LangCtrl.GetString("gc_NoResults", "NO RESULTS FOUND");
                            DataPanel.Controls.Add(label);
                        }
                    }
                    gcFileCount.Text = string.Format(LangCtrl.GetString("gcFileCount", "Files: {0}"), num1);
                    gc_Accounts.Text = string.Format(LangCtrl.GetString("gc_Accounts", "Accounts: {0}"), num2);
                }
                object[] objArray = new object[] { FromDate.Value.Value, ToDate.Value.Value, txtRMS.Text, txtCAD.Text, chk_FilterEvidence.Checked, num2, num1 };
                string str2 = string.Format("{0} to {1} RMS: {2} CAD: {3} Evidence: {4}   RESULT> Accounts: {5} Files: {6}", objArray);
                Global.Log("SEARCH GC", string.Concat("Global Catalog> ", str2));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            DataPanel.ResumeLayout();
        }

        
        private void FromDate_ValueChanged(object sender, EventArgs e)
        {
            DateTime? nullable1 = FromDate.Value;
            DateTime? nullable2 = ToDate.Value;
            if ((nullable1.HasValue & nullable2.HasValue ? (nullable1.GetValueOrDefault() > nullable2.GetValueOrDefault() ? 1 : 0) : 0) == 0)
                return;
            FromDate.Value = ToDate.Value;
        }

        
        private void ToDate_ValueChanged(object sender, EventArgs e)
        {
            DateTime? nullable1 = ToDate.Value;
            DateTime? nullable2 = FromDate.Value;
            if ((nullable1.HasValue & nullable2.HasValue ? (nullable1.GetValueOrDefault() < nullable2.GetValueOrDefault() ? 1 : 0) : 0) == 0)
                return;
            ToDate.Value = FromDate.Value;
        }

        
        private void SaveRegData()
        {
            try
            {
                Registry.SetWindowPos(new WindowPos()
                {
                    Width = Width,
                    Height = Height,
                    PosX = Location.X,
                    PosY = Location.Y
                }, "GlobalCatWindow");
            }
            catch
            {
            }
        }

        
        private void LoadRegData()
        {
            try
            {
                WindowPos windowPos = Registry.GetWindowPos("GlobalCatWindow");
                if (windowPos.Height != 0 && windowPos.Width != 0)
                {
                    Width = windowPos.Width;
                    Height = windowPos.Height;
                    Location = new Point(windowPos.PosX, windowPos.PosY);
                }
                else
                {
                    StartPosition = FormStartPosition.CenterScreen;
                    Width = 800;
                    Height = 600;
                }
            }
            catch
            {
            }
        }

        
        private void GlobalCatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveRegData();
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        
        private void InitializeComponent()
        {
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
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(GlobalCatForm));
            this.HeaderPanel = new Panel();
            this.gc_Accounts = new Label();
            this.gcFileCount = new Label();
            this.btn_Reset = new vButton();
            this.btn_Search = new vButton();
            this.SearchPanel = new Panel();
            this.gc_Search = new Label();
            this.btnByIngestDate = new vRadioButton();
            this.btnByFileDate = new vRadioButton();
            this.txtSet = new vTextBox();
            this.gc_Set = new Label();
            this.chk_FilterEvidence = new vCheckBox();
            this.txtCAD = new vTextBox();
            this.gc_CADId = new Label();
            this.txtRMS = new vTextBox();
            this.gc_RMSId = new Label();
            this.gc_Days = new Label();
            this.ToDate = new vDateTimePicker();
            this.gc_EndDate = new Label();
            this.FromDate = new vDateTimePicker();
            this.gc_StartDate = new Label();
            this.lblLine1 = new Label();
            this.cboDays = new vComboBox();
            this.DataPanel = new Panel();
            this.HeaderPanel.SuspendLayout();
            this.SearchPanel.SuspendLayout();
            base.SuspendLayout();
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.gc_Accounts);
            this.HeaderPanel.Controls.Add(this.gcFileCount);
            this.HeaderPanel.Controls.Add(this.btn_Reset);
            this.HeaderPanel.Controls.Add(this.btn_Search);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(884, 40);
            this.HeaderPanel.TabIndex = 0;
            this.gc_Accounts.AutoSize = true;
            this.gc_Accounts.BackColor = Color.Transparent;
            this.gc_Accounts.ForeColor = Color.White;
            this.gc_Accounts.Location = new Point(321, 21);
            this.gc_Accounts.Name = "gc_Accounts";
            this.gc_Accounts.Size = new Size(64, 13);
            this.gc_Accounts.TabIndex = 3;
            this.gc_Accounts.Text = "Accounts: 0";
            this.gcFileCount.AutoSize = true;
            this.gcFileCount.BackColor = Color.Transparent;
            this.gcFileCount.ForeColor = Color.White;
            this.gcFileCount.Location = new Point(321, 4);
            this.gcFileCount.Name = "gcFileCount";
            this.gcFileCount.Size = new Size(40, 13);
            this.gcFileCount.TabIndex = 2;
            this.gcFileCount.Text = "Files: 0";
            this.btn_Reset.AllowAnimations = true;
            this.btn_Reset.BackColor = Color.Transparent;
            this.btn_Reset.Location = new Point(110, 4);
            this.btn_Reset.Name = "btn_Reset";
            this.btn_Reset.RoundedCornersMask = 15;
            this.btn_Reset.RoundedCornersRadius = 0;
            this.btn_Reset.Size = new Size(100, 30);
            this.btn_Reset.TabIndex = 1;
            this.btn_Reset.Text = "Reset";
            this.btn_Reset.UseVisualStyleBackColor = false;
            this.btn_Reset.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Reset.Click += new EventHandler(this.btn_Reset_Click);
            this.btn_Search.AllowAnimations = true;
            this.btn_Search.BackColor = Color.Transparent;
            this.btn_Search.Location = new Point(4, 4);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.RoundedCornersMask = 15;
            this.btn_Search.RoundedCornersRadius = 0;
            this.btn_Search.Size = new Size(100, 30);
            this.btn_Search.TabIndex = 0;
            this.btn_Search.Text = "Search";
            this.btn_Search.UseVisualStyleBackColor = false;
            this.btn_Search.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Search.Click += new EventHandler(this.btn_Search_Click);
            this.SearchPanel.BackColor = Color.White;
            this.SearchPanel.BorderStyle = BorderStyle.FixedSingle;
            this.SearchPanel.Controls.Add(this.gc_Search);
            this.SearchPanel.Controls.Add(this.btnByIngestDate);
            this.SearchPanel.Controls.Add(this.btnByFileDate);
            this.SearchPanel.Controls.Add(this.txtSet);
            this.SearchPanel.Controls.Add(this.gc_Set);
            this.SearchPanel.Controls.Add(this.chk_FilterEvidence);
            this.SearchPanel.Controls.Add(this.txtCAD);
            this.SearchPanel.Controls.Add(this.gc_CADId);
            this.SearchPanel.Controls.Add(this.txtRMS);
            this.SearchPanel.Controls.Add(this.gc_RMSId);
            this.SearchPanel.Controls.Add(this.gc_Days);
            this.SearchPanel.Controls.Add(this.ToDate);
            this.SearchPanel.Controls.Add(this.gc_EndDate);
            this.SearchPanel.Controls.Add(this.FromDate);
            this.SearchPanel.Controls.Add(this.gc_StartDate);
            this.SearchPanel.Controls.Add(this.lblLine1);
            this.SearchPanel.Controls.Add(this.cboDays);
            this.SearchPanel.Dock = DockStyle.Left;
            this.SearchPanel.Location = new Point(0, 40);
            this.SearchPanel.Name = "SearchPanel";
            this.SearchPanel.Size = new Size(323, 622);
            this.SearchPanel.TabIndex = 1;
            this.gc_Search.AutoSize = true;
            this.gc_Search.Location = new Point(17, 57);
            this.gc_Search.Name = "gc_Search";
            this.gc_Search.Size = new Size(106, 13);
            this.gc_Search.TabIndex = 48;
            this.gc_Search.Text = "Date Search Options";
            this.btnByIngestDate.BackColor = Color.Transparent;
            this.btnByIngestDate.Flat = true;
            this.btnByIngestDate.Image = null;
            this.btnByIngestDate.Location = new Point(145, 67);
            this.btnByIngestDate.Name = "btnByIngestDate";
            this.btnByIngestDate.Size = new Size(162, 24);
            this.btnByIngestDate.TabIndex = 47;
            this.btnByIngestDate.Text = "By Ingest Date";
            this.btnByIngestDate.UseVisualStyleBackColor = false;
            this.btnByIngestDate.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnByFileDate.BackColor = Color.Transparent;
            this.btnByFileDate.Checked = true;
            this.btnByFileDate.Flat = true;
            this.btnByFileDate.Image = null;
            this.btnByFileDate.Location = new Point(145, 39);
            this.btnByFileDate.Name = "btnByFileDate";
            this.btnByFileDate.Size = new Size(162, 24);
            this.btnByFileDate.TabIndex = 46;
            this.btnByFileDate.TabStop = true;
            this.btnByFileDate.Text = "By Original File Date";
            this.btnByFileDate.UseVisualStyleBackColor = false;
            this.btnByFileDate.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.txtSet.BackColor = Color.White;
            this.txtSet.BoundsOffset = new Size(1, 1);
            this.txtSet.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtSet.DefaultText = "";
            this.txtSet.Location = new Point(145, 212);
            this.txtSet.MaxLength = 32;
            this.txtSet.Name = "txtSet";
            this.txtSet.PasswordChar = '\0';
            this.txtSet.ScrollBars = ScrollBars.None;
            this.txtSet.SelectionLength = 0;
            this.txtSet.SelectionStart = 0;
            this.txtSet.Size = new Size(128, 23);
            this.txtSet.TabIndex = 45;
            this.txtSet.TextAlign = HorizontalAlignment.Left;
            this.txtSet.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.gc_Set.Location = new Point(16, 217);
            this.gc_Set.Name = "gc_Set";
            this.gc_Set.Size = new Size(120, 13);
            this.gc_Set.TabIndex = 44;
            this.gc_Set.Text = "Set ID";
            this.chk_FilterEvidence.BackColor = Color.Transparent;
            this.chk_FilterEvidence.Location = new Point(145, 241);
            this.chk_FilterEvidence.Name = "chk_FilterEvidence";
            this.chk_FilterEvidence.Size = new Size(162, 24);
            this.chk_FilterEvidence.TabIndex = 43;
            this.chk_FilterEvidence.Text = "Marked as Evidence";
            this.chk_FilterEvidence.UseVisualStyleBackColor = false;
            this.chk_FilterEvidence.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.txtCAD.BackColor = Color.White;
            this.txtCAD.BoundsOffset = new Size(1, 1);
            this.txtCAD.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtCAD.DefaultText = "";
            this.txtCAD.Location = new Point(145, 183);
            this.txtCAD.MaxLength = 32;
            this.txtCAD.Name = "txtCAD";
            this.txtCAD.PasswordChar = '\0';
            this.txtCAD.ScrollBars = ScrollBars.None;
            this.txtCAD.SelectionLength = 0;
            this.txtCAD.SelectionStart = 0;
            this.txtCAD.Size = new Size(128, 23);
            this.txtCAD.TabIndex = 40;
            this.txtCAD.TextAlign = HorizontalAlignment.Left;
            this.txtCAD.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.gc_CADId.Location = new Point(17, 188);
            this.gc_CADId.Name = "gc_CADId";
            this.gc_CADId.Size = new Size(120, 13);
            this.gc_CADId.TabIndex = 39;
            this.gc_CADId.Text = "CAD ID";
            this.txtRMS.BackColor = Color.White;
            this.txtRMS.BoundsOffset = new Size(1, 1);
            this.txtRMS.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtRMS.DefaultText = "";
            this.txtRMS.Location = new Point(145, 155);
            this.txtRMS.MaxLength = 32;
            this.txtRMS.Name = "txtRMS";
            this.txtRMS.PasswordChar = '\0';
            this.txtRMS.ScrollBars = ScrollBars.None;
            this.txtRMS.SelectionLength = 0;
            this.txtRMS.SelectionStart = 0;
            this.txtRMS.Size = new Size(128, 23);
            this.txtRMS.TabIndex = 38;
            this.txtRMS.TextAlign = HorizontalAlignment.Left;
            this.txtRMS.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.gc_RMSId.Location = new Point(17, 160);
            this.gc_RMSId.Name = "gc_RMSId";
            this.gc_RMSId.Size = new Size(120, 13);
            this.gc_RMSId.TabIndex = 37;
            this.gc_RMSId.Text = "RMS ID";
            this.gc_Days.AutoSize = true;
            this.gc_Days.Location = new Point(214, 15);
            this.gc_Days.Name = "gc_Days";
            this.gc_Days.Size = new Size(31, 13);
            this.gc_Days.TabIndex = 25;
            this.gc_Days.Text = "Days";
            this.ToDate.BackColor = Color.White;
            this.ToDate.BorderColor = Color.Black;
            this.ToDate.Culture = new CultureInfo("");
            this.ToDate.DefaultDateTimeFormat = DefaultDateTimePatterns.Custom;
            this.ToDate.DropDownMaximumSize = new Size(1000, 1000);
            this.ToDate.DropDownMinimumSize = new Size(10, 10);
            this.ToDate.DropDownResizeDirection = SizingDirection.None;
            this.ToDate.FormatValue = "";
            this.ToDate.Location = new Point(145, 127);
            this.ToDate.MaxDate = new DateTime(2100, 1, 1, 0, 0, 0, 0);
            this.ToDate.MinDate = new DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.ToDate.Name = "ToDate";
            this.ToDate.ShowGrip = false;
            this.ToDate.Size = new Size(128, 23);
            this.ToDate.TabIndex = 30;
            this.ToDate.Text = "07/14/2014 17:46:50";
            this.ToDate.UseThemeBackColor = false;
            this.ToDate.UseThemeDropDownArrowColor = true;
            this.ToDate.Value = new DateTime(2017, 08, 02);
            this.ToDate.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.ToDate.ValueChanged += new EventHandler(this.ToDate_ValueChanged);
            this.gc_EndDate.Location = new Point(17, 132);
            this.gc_EndDate.Name = "gc_EndDate";
            this.gc_EndDate.Size = new Size(120, 13);
            this.gc_EndDate.TabIndex = 29;
            this.gc_EndDate.Text = "To Date/Time";
            this.FromDate.BackColor = Color.White;
            this.FromDate.BorderColor = Color.Black;
            this.FromDate.Culture = new CultureInfo("");
            this.FromDate.DefaultDateTimeFormat = DefaultDateTimePatterns.Custom;
            this.FromDate.DropDownMaximumSize = new Size(1000, 1000);
            this.FromDate.DropDownMinimumSize = new Size(10, 10);
            this.FromDate.DropDownResizeDirection = SizingDirection.None;
            this.FromDate.FormatValue = "";
            this.FromDate.Location = new Point(145, 99);
            this.FromDate.MaxDate = new DateTime(2100, 1, 1, 0, 0, 0, 0);
            this.FromDate.MinDate = new DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.FromDate.Name = "FromDate";
            this.FromDate.ShowGrip = false;
            this.FromDate.Size = new Size(128, 23);
            this.FromDate.TabIndex = 28;
            this.FromDate.Text = "07/14/2014 17:46:50";
            this.FromDate.UseThemeBackColor = false;
            this.FromDate.UseThemeDropDownArrowColor = true;
            this.FromDate.Value = new DateTime(2017, 8, 02);
            this.FromDate.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.FromDate.ValueChanged += new EventHandler(this.FromDate_ValueChanged);
            this.gc_StartDate.Location = new Point(17, 104);
            this.gc_StartDate.Name = "gc_StartDate";
            this.gc_StartDate.Size = new Size(120, 13);
            this.gc_StartDate.TabIndex = 27;
            this.gc_StartDate.Text = "From Date/Time";
            this.lblLine1.BorderStyle = BorderStyle.Fixed3D;
            this.lblLine1.Location = new Point(17, 34);
            this.lblLine1.Name = "lblLine1";
            this.lblLine1.Size = new Size(290, 1);
            this.lblLine1.TabIndex = 26;
            this.cboDays.BackColor = Color.White;
            this.cboDays.DefaultText = "";
            this.cboDays.DisplayMember = "";
            this.cboDays.DropDownHeight = 240;
            this.cboDays.DropDownList = true;
            this.cboDays.DropDownMaximumSize = new Size(150, 300);
            this.cboDays.DropDownMinimumSize = new Size(150, 240);
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
            this.cboDays.Location = new Point(145, 5);
            this.cboDays.Name = "cboDays";
            this.cboDays.RoundedCornersMaskListItem = 15;
            this.cboDays.Size = new Size(64, 23);
            this.cboDays.TabIndex = 24;
            this.cboDays.Text = "1";
            this.cboDays.UseThemeBackColor = false;
            this.cboDays.UseThemeDropDownArrowColor = true;
            this.cboDays.ValueMember = "";
            this.cboDays.VIBlendScrollBarsTheme = VIBLEND_THEME.VISTABLUE;
            this.cboDays.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.cboDays.SelectedIndexChanged += new EventHandler(this.cboDays_SelectedIndexChanged);
            this.DataPanel.AutoScroll = true;
            this.DataPanel.Dock = DockStyle.Fill;
            this.DataPanel.Location = new Point(323, 40);
            this.DataPanel.Name = "DataPanel";
            this.DataPanel.Size = new Size(561, 622);
            this.DataPanel.TabIndex = 2;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(884, 662);
            base.Controls.Add(this.DataPanel);
            base.Controls.Add(this.SearchPanel);
            base.Controls.Add(this.HeaderPanel);
            base.Icon = (Icon)Resources.GlobalCatForm.GlobalCatIcon;
            this.MinimumSize = new Size(850, 650);
            base.Name = "GlobalCatForm";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Global Catalog Search";
            base.FormClosing += new FormClosingEventHandler(this.GlobalCatForm_FormClosing);
            base.Load += new EventHandler(this.GlobalCatForm_Load);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            this.SearchPanel.ResumeLayout(false);
            this.SearchPanel.PerformLayout();
            base.ResumeLayout(false);
        }
    }
}
