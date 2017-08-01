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
                Text = LangCtrl.GetString("GlobalCatForm_1", "GlobalCatForm_2");
                LangCtrl.reText(this);
                gcFileCount.Text = string.Format(LangCtrl.GetString("GlobalCatForm_3", "GlobalCatForm_4"), 0);
                gc_Accounts.Text = string.Format(LangCtrl.GetString("GlobalCatForm_5", "GlobalCatForm_6"), 0);
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
            Global.Log("GlobalCatForm_7", "GlobalCatForm_8");
            ResetForm();
        }

        
        private void ResetForm()
        {
            try
            {
                FormCtrl.ClearForm(this);
                ResetDates();
                gcFileCount.Text = string.Format(LangCtrl.GetString("GlobalCatForm_9", "GlobalCatForm_10"), 0);
                gc_Accounts.Text = string.Format(LangCtrl.GetString("GlobalCatForm_11", "GlobalCatForm_12"), 0);
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
                int num = (int)MessageBox.Show(this, LangCtrl.GetString("GlobalCatForm_13", "GlobalCatForm_14"), "GlobalCatForm_15", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
                                        str = string.Format("GlobalCatForm_16", account.ToString(), account.BadgeNumber);
                                        string data = string.Format("GlobalCatForm_17", account.Rank);
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
                            label.Font = new Font("GlobalCatForm_18", 16f);
                            label.Text = LangCtrl.GetString("GlobalCatForm_19", "GlobalCatForm_20");
                            DataPanel.Controls.Add(label);
                        }
                    }
                    gcFileCount.Text = string.Format(LangCtrl.GetString("GlobalCatForm_21", "GlobalCatForm_22"), num1);
                    gc_Accounts.Text = string.Format(LangCtrl.GetString("GlobalCatForm_23", "GlobalCatForm_24"), num2);
                }
                Global.Log("GlobalCatForm_25", "GlobalCatForm_26" + string.Format("GlobalCatForm_27", FromDate.Value.Value, ToDate.Value.Value, txtRMS.Text, txtCAD.Text, chk_FilterEvidence.Checked, num2, num1));
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
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
                }, "GlobalCatForm_28");
            }
            catch
            {
            }
        }

        
        private void LoadRegData()
        {
            try
            {
                WindowPos windowPos = Registry.GetWindowPos("GlobalCatForm_29");
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
            ListItem listItem11 = new ListItem();
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
            this.SuspendLayout();
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add((Control)this.gc_Accounts);
            this.HeaderPanel.Controls.Add((Control)this.gcFileCount);
            this.HeaderPanel.Controls.Add((Control)this.btn_Reset);
            this.HeaderPanel.Controls.Add((Control)this.btn_Search);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "GlobalCatForm_29";
            this.HeaderPanel.Size = new Size(884, 40);
            this.HeaderPanel.TabIndex = 0;
            this.gc_Accounts.AutoSize = true;
            this.gc_Accounts.BackColor = Color.Transparent;
            this.gc_Accounts.ForeColor = Color.White;
            this.gc_Accounts.Location = new Point(321, 21);
            this.gc_Accounts.Name = "GlobalCatForm_30";
            this.gc_Accounts.Size = new Size(64, 13);
            this.gc_Accounts.TabIndex = 3;
            this.gc_Accounts.Text = "GlobalCatForm_31";
            this.gcFileCount.AutoSize = true;
            this.gcFileCount.BackColor = Color.Transparent;
            this.gcFileCount.ForeColor = Color.White;
            this.gcFileCount.Location = new Point(321, 4);
            this.gcFileCount.Name = "GlobalCatForm_32";
            this.gcFileCount.Size = new Size(40, 13);
            this.gcFileCount.TabIndex = 2;
            this.gcFileCount.Text = "GlobalCatForm_33";
            this.btn_Reset.AllowAnimations = true;
            this.btn_Reset.BackColor = Color.Transparent;
            this.btn_Reset.Location = new Point(110, 4);
            this.btn_Reset.Name = "GlobalCatForm_34";
            this.btn_Reset.RoundedCornersMask = (byte)15;
            this.btn_Reset.RoundedCornersRadius = 0;
            this.btn_Reset.Size = new Size(100, 30);
            this.btn_Reset.TabIndex = 1;
            this.btn_Reset.Text = "GlobalCatForm_35";
            this.btn_Reset.UseVisualStyleBackColor = false;
            this.btn_Reset.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Reset.Click += new EventHandler(this.btn_Reset_Click);
            this.btn_Search.AllowAnimations = true;
            this.btn_Search.BackColor = Color.Transparent;
            this.btn_Search.Location = new Point(4, 4);
            this.btn_Search.Name = "GlobalCatForm_36";
            this.btn_Search.RoundedCornersMask = (byte)15;
            this.btn_Search.RoundedCornersRadius = 0;
            this.btn_Search.Size = new Size(100, 30);
            this.btn_Search.TabIndex = 0;
            this.btn_Search.Text = "GlobalCatForm_37";
            this.btn_Search.UseVisualStyleBackColor = false;
            this.btn_Search.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Search.Click += new EventHandler(this.btn_Search_Click);
            this.SearchPanel.BackColor = Color.White;
            this.SearchPanel.BorderStyle = BorderStyle.FixedSingle;
            this.SearchPanel.Controls.Add((Control)this.gc_Search);
            this.SearchPanel.Controls.Add((Control)this.btnByIngestDate);
            this.SearchPanel.Controls.Add((Control)this.btnByFileDate);
            this.SearchPanel.Controls.Add((Control)this.txtSet);
            this.SearchPanel.Controls.Add((Control)this.gc_Set);
            this.SearchPanel.Controls.Add((Control)this.chk_FilterEvidence);
            this.SearchPanel.Controls.Add((Control)this.txtCAD);
            this.SearchPanel.Controls.Add((Control)this.gc_CADId);
            this.SearchPanel.Controls.Add((Control)this.txtRMS);
            this.SearchPanel.Controls.Add((Control)this.gc_RMSId);
            this.SearchPanel.Controls.Add((Control)this.gc_Days);
            this.SearchPanel.Controls.Add((Control)this.ToDate);
            this.SearchPanel.Controls.Add((Control)this.gc_EndDate);
            this.SearchPanel.Controls.Add((Control)this.FromDate);
            this.SearchPanel.Controls.Add((Control)this.gc_StartDate);
            this.SearchPanel.Controls.Add((Control)this.lblLine1);
            this.SearchPanel.Controls.Add((Control)this.cboDays);
            this.SearchPanel.Dock = DockStyle.Left;
            this.SearchPanel.Location = new Point(0, 40);
            this.SearchPanel.Name = "GlobalCatForm_38";
            this.SearchPanel.Size = new Size(323, 622);
            this.SearchPanel.TabIndex = 1;
            this.gc_Search.AutoSize = true;
            this.gc_Search.Location = new Point(17, 57);
            this.gc_Search.Name = "GlobalCatForm_39";
            this.gc_Search.Size = new Size(106, 13);
            this.gc_Search.TabIndex = 48;
            this.gc_Search.Text = "GlobalCatForm_40";
            this.btnByIngestDate.BackColor = Color.Transparent;
            this.btnByIngestDate.Flat = true;
            this.btnByIngestDate.Image = (Image)null;
            this.btnByIngestDate.Location = new Point(145, 67);
            this.btnByIngestDate.Name = "GlobalCatForm_41";
            this.btnByIngestDate.Size = new Size(162, 24);
            this.btnByIngestDate.TabIndex = 47;
            this.btnByIngestDate.Text = "GlobalCatForm_42";
            this.btnByIngestDate.UseVisualStyleBackColor = false;
            this.btnByIngestDate.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnByFileDate.BackColor = Color.Transparent;
            this.btnByFileDate.Checked = true;
            this.btnByFileDate.Flat = true;
            this.btnByFileDate.Image = (Image)null;
            this.btnByFileDate.Location = new Point(145, 39);
            this.btnByFileDate.Name = "GlobalCatForm_43";
            this.btnByFileDate.Size = new Size(162, 24);
            this.btnByFileDate.TabIndex = 46;
            this.btnByFileDate.TabStop = true;
            this.btnByFileDate.Text = "GlobalCatForm_44";
            this.btnByFileDate.UseVisualStyleBackColor = false;
            this.btnByFileDate.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.txtSet.BackColor = Color.White;
            this.txtSet.BoundsOffset = new Size(1, 1);
            this.txtSet.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtSet.DefaultText = "";
            this.txtSet.Location = new Point(145, 212);
            this.txtSet.MaxLength = 32;
            this.txtSet.Name = "GlobalCatForm_45";
            this.txtSet.PasswordChar = char.MinValue;
            this.txtSet.ScrollBars = ScrollBars.None;
            this.txtSet.SelectionLength = 0;
            this.txtSet.SelectionStart = 0;
            this.txtSet.Size = new Size(128, 23);
            this.txtSet.TabIndex = 45;
            this.txtSet.TextAlign = HorizontalAlignment.Left;
            this.txtSet.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.gc_Set.Location = new Point(16, 217);
            this.gc_Set.Name = "GlobalCatForm_46";
            this.gc_Set.Size = new Size(120, 13);
            this.gc_Set.TabIndex = 44;
            this.gc_Set.Text = "GlobalCatForm_47";
            this.chk_FilterEvidence.BackColor = Color.Transparent;
            this.chk_FilterEvidence.Location = new Point(145, 241);
            this.chk_FilterEvidence.Name = "GlobalCatForm_48";
            this.chk_FilterEvidence.Size = new Size(162, 24);
            this.chk_FilterEvidence.TabIndex = 43;
            this.chk_FilterEvidence.Text = "GlobalCatForm_49";
            this.chk_FilterEvidence.UseVisualStyleBackColor = false;
            this.chk_FilterEvidence.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.txtCAD.BackColor = Color.White;
            this.txtCAD.BoundsOffset = new Size(1, 1);
            this.txtCAD.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtCAD.DefaultText = "";
            this.txtCAD.Location = new Point(145, 183);
            this.txtCAD.MaxLength = 32;
            this.txtCAD.Name = "GlobalCatForm_50";
            this.txtCAD.PasswordChar = char.MinValue;
            this.txtCAD.ScrollBars = ScrollBars.None;
            this.txtCAD.SelectionLength = 0;
            this.txtCAD.SelectionStart = 0;
            this.txtCAD.Size = new Size(128, 23);
            this.txtCAD.TabIndex = 40;
            this.txtCAD.TextAlign = HorizontalAlignment.Left;
            this.txtCAD.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.gc_CADId.Location = new Point(17, 188);
            this.gc_CADId.Name = "GlobalCatForm_51";
            this.gc_CADId.Size = new Size(120, 13);
            this.gc_CADId.TabIndex = 39;
            this.gc_CADId.Text = "GlobalCatForm_52";
            this.txtRMS.BackColor = Color.White;
            this.txtRMS.BoundsOffset = new Size(1, 1);
            this.txtRMS.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtRMS.DefaultText = "";
            this.txtRMS.Location = new Point(145, 155);
            this.txtRMS.MaxLength = 32;
            this.txtRMS.Name = "GlobalCatForm_53";
            this.txtRMS.PasswordChar = char.MinValue;
            this.txtRMS.ScrollBars = ScrollBars.None;
            this.txtRMS.SelectionLength = 0;
            this.txtRMS.SelectionStart = 0;
            this.txtRMS.Size = new Size(128, 23);
            this.txtRMS.TabIndex = 38;
            this.txtRMS.TextAlign = HorizontalAlignment.Left;
            this.txtRMS.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.gc_RMSId.Location = new Point(17, 160);
            this.gc_RMSId.Name = "GlobalCatForm_54";
            this.gc_RMSId.Size = new Size(120, 13);
            this.gc_RMSId.TabIndex = 37;
            this.gc_RMSId.Text = "GlobalCatForm_55";
            this.gc_Days.AutoSize = true;
            this.gc_Days.Location = new Point(214, 15);
            this.gc_Days.Name = "GlobalCatForm_56";
            this.gc_Days.Size = new Size(31, 13);
            this.gc_Days.TabIndex = 25;
            this.gc_Days.Text = "GlobalCatForm_57";
            this.ToDate.BackColor = Color.White;
            this.ToDate.BorderColor = Color.Black;
            this.ToDate.Culture = new CultureInfo("");
            this.ToDate.DefaultDateTimeFormat = DefaultDateTimePatterns.Custom;
            this.ToDate.DropDownMaximumSize = new Size(1000, 1000);
            this.ToDate.DropDownMinimumSize = new Size(10, 10);
            this.ToDate.DropDownResizeDirection = SizingDirection.None;
            this.ToDate.FormatValue = "";
            this.ToDate.Location = new Point(145, (int)sbyte.MaxValue);
            this.ToDate.MaxDate = new DateTime(2100, 1, 1, 0, 0, 0, 0);
            this.ToDate.MinDate = new DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.ToDate.Name = "GlobalCatForm_58";
            this.ToDate.ShowGrip = false;
            this.ToDate.Size = new Size(128, 23);
            this.ToDate.TabIndex = 30;
            this.ToDate.Text = "GlobalCatForm_59";
            this.ToDate.UseThemeBackColor = false;
            this.ToDate.UseThemeDropDownArrowColor = true;
            this.ToDate.Value = new DateTime(2017, 08, 01);
            this.ToDate.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.ToDate.ValueChanged += new EventHandler(this.ToDate_ValueChanged);
            this.gc_EndDate.Location = new Point(17, 132);
            this.gc_EndDate.Name = "GlobalCatForm_60";
            this.gc_EndDate.Size = new Size(120, 13);
            this.gc_EndDate.TabIndex = 29;
            this.gc_EndDate.Text = "GlobalCatForm_61";
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
            this.FromDate.Name = "GlobalCatForm_62";
            this.FromDate.ShowGrip = false;
            this.FromDate.Size = new Size(128, 23);
            this.FromDate.TabIndex = 28;
            this.FromDate.Text = "GlobalCatForm_63";
            this.FromDate.UseThemeBackColor = false;
            this.FromDate.UseThemeDropDownArrowColor = true;
            this.FromDate.Value = new DateTime(2017, 08, 01);
            this.FromDate.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.FromDate.ValueChanged += new EventHandler(this.FromDate_ValueChanged);
            this.gc_StartDate.Location = new Point(17, 104);
            this.gc_StartDate.Name = "GlobalCatForm_64";
            this.gc_StartDate.Size = new Size(120, 13);
            this.gc_StartDate.TabIndex = 27;
            this.gc_StartDate.Text = "GlobalCatForm_65";
            this.lblLine1.BorderStyle = BorderStyle.Fixed3D;
            this.lblLine1.Location = new Point(17, 34);
            this.lblLine1.Name = "GlobalCatForm_66";
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
            listItem1.RoundedCornersMask = (byte)15;
            listItem1.Text = "GlobalCatForm_67";
            listItem2.RoundedCornersMask = (byte)15;
            listItem2.Text = "GlobalCatForm_68";
            listItem3.RoundedCornersMask = (byte)15;
            listItem3.Text = "GlobalCatForm_69";
            listItem4.RoundedCornersMask = (byte)15;
            listItem4.Text = "GlobalCatForm_70";
            listItem5.RoundedCornersMask = (byte)15;
            listItem5.Text = "GlobalCatForm_71";
            listItem6.RoundedCornersMask = (byte)15;
            listItem6.Text = "GlobalCatForm_72";
            listItem7.RoundedCornersMask = (byte)15;
            listItem7.Text = "GlobalCatForm_73";
            listItem8.RoundedCornersMask = (byte)15;
            listItem8.Text = "GlobalCatForm_74";
            listItem9.RoundedCornersMask = (byte)15;
            listItem9.Text = "GlobalCatForm_75";
            listItem10.RoundedCornersMask = (byte)15;
            listItem10.Text = "GlobalCatForm_76";
            listItem11.RoundedCornersMask = (byte)15;
            listItem11.Text = "GlobalCatForm_77";
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
            this.cboDays.Items.Add(listItem11);
            this.cboDays.Location = new Point(145, 5);
            this.cboDays.Name = "GlobalCatForm_78";
            this.cboDays.RoundedCornersMaskListItem = (byte)15;
            this.cboDays.Size = new Size(64, 23);
            this.cboDays.TabIndex = 24;
            this.cboDays.Text = "GlobalCatForm_79";
            this.cboDays.UseThemeBackColor = false;
            this.cboDays.UseThemeDropDownArrowColor = true;
            this.cboDays.ValueMember = "";
            this.cboDays.VIBlendScrollBarsTheme = VIBLEND_THEME.VISTABLUE;
            this.cboDays.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.cboDays.SelectedIndexChanged += new EventHandler(this.cboDays_SelectedIndexChanged);
            this.DataPanel.AutoScroll = true;
            this.DataPanel.Dock = DockStyle.Fill;
            this.DataPanel.Location = new Point(323, 40);
            this.DataPanel.Name = "GlobalCatForm_80";
            this.DataPanel.Size = new Size(561, 622);
            this.DataPanel.TabIndex = 2;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(884, 662);
            this.Controls.Add((Control)this.DataPanel);
            this.Controls.Add((Control)this.SearchPanel);
            this.Controls.Add((Control)this.HeaderPanel);
            this.Icon = (Icon)Resources.GlobalCatForm.GlobalCatIcon;
            this.MinimumSize = new Size(850, 650);
            this.Name = "GlobalCatForm_81";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "GlobalCatForm_82";
            this.FormClosing += new FormClosingEventHandler(this.GlobalCatForm_FormClosing);
            this.Load += new EventHandler(this.GlobalCatForm_Load);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            this.SearchPanel.ResumeLayout(false);
            this.SearchPanel.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
