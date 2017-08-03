using AppGlobal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Resources;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMInterfaces;
using VMModels.Model;

namespace VideoManager
{
    public class EmailForm : Form
    {
        private string Pwd = string.Empty;

        private string string_0 = string.Empty;

        private int Port = 587;

        private bool IsEnabled;

        private bool IsSSL;

        private int SendCount;

        private List<string> FailedList = new List<string>();

        private IContainer components;

        private Panel HeaderPanel;

        private vRichTextBox txtBody;

        private vTextBox txtSubject;

        private Label lbl_EmailSubject;

        private vTextBox txtFrom;

        private Label lbl_EmailFrom;

        private vCheckedListBox AccountList;

        private vButton btnSendEmails;

        private vButton btn_Close;

        private ContextMenuStrip ListMenu;

        private ToolStripMenuItem mnu_SelectAllAccounts;

        private ToolStripMenuItem mnu_UnselectAllAccounts;

        private ImageList imageList1;

        private vProgressBar progBar;

        public EmailForm()
        {
            this.InitializeComponent();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            this.progBar.Value = 0;
            this.progBar.Visible = true;
            this.FailedList.Clear();
            int num = 0;
            foreach (ListItem item in this.AccountList.Items)
            {
                if (!item.IsChecked.Value)
                {
                    continue;
                }
                num++;
            }
            this.progBar.Maximum = num;
            int num1 = 0;
            foreach (ListItem listItem in this.AccountList.Items)
            {
                if (!listItem.IsChecked.Value)
                {
                    continue;
                }
                this.sendEmail(((Account)listItem.Tag).Email.Trim());
                int num2 = num1 + 1;
                num1 = num2;
                this.progBar.Value = num2;
                Application.DoEvents();
            }
            string empty = string.Empty;
            if (this.FailedList.Count <= 0)
            {
                empty = string.Format("Emails sent: {0}", this.SendCount);
            }
            else
            {
                empty = string.Format("Emails Sent: {0}\nSome emails failed:\n", this.SendCount);
                foreach (string failedList in this.FailedList)
                {
                    empty = string.Concat(empty, failedList, "\n");
                }
            }
            MessageBox.Show(this, empty, "Email", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            this.progBar.Value = 0;
            this.progBar.Visible = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EmailForm_Load(object sender, EventArgs e)
        {
            this.LoadAccounts();
            this.txtFrom.Text = Global.GlobalAccount.Email;
            this.txtSubject.Select();
            this.SetLanguage();
            this.LoadProfile();
        }

        private string GetCfg(string Key, bool Decrypt)
        {
            string empty = string.Empty;
            using (RPM_GlobalConfig rPMGlobalConfig = new RPM_GlobalConfig())
            {
                GlobalConfig globalConfig = new GlobalConfig();
                globalConfig = rPMGlobalConfig.GetConfigRecord(Key);
                if (globalConfig != null)
                {
                    empty = globalConfig.Value;
                    if (Decrypt)
                    {
                        try
                        {
                            empty = CryptoIO.Decrypt(empty);
                        }
                        catch (Exception exception)
                        {
                            empty = string.Empty;
                        }
                    }
                }
            }
            return empty;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(EmailForm));
            this.HeaderPanel = new Panel();
            this.btnSendEmails = new vButton();
            this.btn_Close = new vButton();
            this.txtBody = new vRichTextBox();
            this.txtSubject = new vTextBox();
            this.lbl_EmailSubject = new Label();
            this.txtFrom = new vTextBox();
            this.lbl_EmailFrom = new Label();
            this.AccountList = new vCheckedListBox();
            this.ListMenu = new ContextMenuStrip(this.components);
            this.mnu_SelectAllAccounts = new ToolStripMenuItem();
            this.mnu_UnselectAllAccounts = new ToolStripMenuItem();
            this.imageList1 = new ImageList(this.components);
            this.progBar = new vProgressBar();
            this.HeaderPanel.SuspendLayout();
            this.ListMenu.SuspendLayout();
            base.SuspendLayout();
            this.HeaderPanel.Controls.Add(this.progBar);
            this.HeaderPanel.Controls.Add(this.btnSendEmails);
            this.HeaderPanel.Controls.Add(this.btn_Close);
            this.HeaderPanel.Controls.Add(this.txtBody);
            this.HeaderPanel.Controls.Add(this.txtSubject);
            this.HeaderPanel.Controls.Add(this.lbl_EmailSubject);
            this.HeaderPanel.Controls.Add(this.txtFrom);
            this.HeaderPanel.Controls.Add(this.lbl_EmailFrom);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(521, 289);
            this.HeaderPanel.TabIndex = 0;
            this.btnSendEmails.AllowAnimations = true;
            this.btnSendEmails.BackColor = Color.Transparent;
            this.btnSendEmails.ImageAlign = ContentAlignment.MiddleLeft;
            this.btnSendEmails.Location = new Point(127, 227);
            this.btnSendEmails.Name = "btnSendEmails";
            this.btnSendEmails.RoundedCornersMask = 15;
            this.btnSendEmails.RoundedCornersRadius = 0;
            this.btnSendEmails.Size = new Size(166, 30);
            this.btnSendEmails.TabIndex = 6;
            this.btnSendEmails.Text = "Send Emails";
            this.btnSendEmails.UseVisualStyleBackColor = false;
            this.btnSendEmails.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnSendEmails.Click += new EventHandler(this.btnSend_Click);
            this.btn_Close.AllowAnimations = true;
            this.btn_Close.BackColor = Color.Transparent;
            this.btn_Close.Location = new Point(409, 227);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.RoundedCornersMask = 15;
            this.btn_Close.RoundedCornersRadius = 0;
            this.btn_Close.Size = new Size(100, 30);
            this.btn_Close.TabIndex = 5;
            this.btn_Close.Text = "Close";
            this.btn_Close.UseVisualStyleBackColor = false;
            this.btn_Close.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Close.Click += new EventHandler(this.btn_Close_Click);
            this.txtBody.AllowAnimations = false;
            this.txtBody.AllowFocused = false;
            this.txtBody.AllowHighlight = false;
            this.txtBody.BackColor = Color.White;
            this.txtBody.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtBody.GleamWidth = 1;
            this.txtBody.Location = new Point(127, 71);
            this.txtBody.MaxLength = 2147483647;
            this.txtBody.Multiline = true;
            this.txtBody.Name = "txtBody";
            this.txtBody.Readonly = false;
            this.txtBody.Size = new Size(382, 149);
            this.txtBody.TabIndex = 4;
            this.txtBody.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.txtSubject.BackColor = Color.White;
            this.txtSubject.BoundsOffset = new Size(1, 1);
            this.txtSubject.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtSubject.DefaultText = "";
            this.txtSubject.Location = new Point(127, 41);
            this.txtSubject.MaxLength = 128;
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.PasswordChar = '\0';
            this.txtSubject.ScrollBars = ScrollBars.None;
            this.txtSubject.SelectionLength = 0;
            this.txtSubject.SelectionStart = 0;
            this.txtSubject.Size = new Size(235, 23);
            this.txtSubject.TabIndex = 3;
            this.txtSubject.TextAlign = HorizontalAlignment.Left;
            this.txtSubject.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_EmailSubject.Location = new Point(13, 46);
            this.lbl_EmailSubject.Name = "lbl_EmailSubject";
            this.lbl_EmailSubject.Size = new Size(110, 13);
            this.lbl_EmailSubject.TabIndex = 2;
            this.lbl_EmailSubject.Text = "Subject";
            this.txtFrom.BackColor = Color.White;
            this.txtFrom.BoundsOffset = new Size(1, 1);
            this.txtFrom.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtFrom.DefaultText = "";
            this.txtFrom.Location = new Point(127, 12);
            this.txtFrom.MaxLength = 32767;
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.PasswordChar = '\0';
            this.txtFrom.Readonly = true;
            this.txtFrom.ScrollBars = ScrollBars.None;
            this.txtFrom.SelectionLength = 0;
            this.txtFrom.SelectionStart = 0;
            this.txtFrom.Size = new Size(235, 23);
            this.txtFrom.TabIndex = 1;
            this.txtFrom.TextAlign = HorizontalAlignment.Left;
            this.txtFrom.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_EmailFrom.Location = new Point(13, 17);
            this.lbl_EmailFrom.Name = "lbl_EmailFrom";
            this.lbl_EmailFrom.Size = new Size(110, 13);
            this.lbl_EmailFrom.TabIndex = 0;
            this.lbl_EmailFrom.Text = "From";
            this.AccountList.CheckOnClick = false;
            this.AccountList.ContextMenuStrip = this.ListMenu;
            this.AccountList.Dock = DockStyle.Fill;
            this.AccountList.ImageList = this.imageList1;
            this.AccountList.Location = new Point(0, 289);
            this.AccountList.Name = "AccountList";
            this.AccountList.RoundedCornersMaskListItem = 15;
            this.AccountList.SelectionMode = SelectionMode.One;
            this.AccountList.Size = new Size(521, 191);
            this.AccountList.TabIndex = 1;
            this.AccountList.Text = "vCheckedListBox1";
            this.AccountList.VIBlendScrollBarsTheme = VIBLEND_THEME.VISTABLUE;
            this.AccountList.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            ToolStripItemCollection items = this.ListMenu.Items;
            ToolStripItem[] mnuSelectAllAccounts = new ToolStripItem[] { this.mnu_SelectAllAccounts, this.mnu_UnselectAllAccounts };
            this.ListMenu.Items.AddRange(mnuSelectAllAccounts);
            this.ListMenu.Name = "ListMenu";
            this.ListMenu.Size = new Size(196, 48);
            this.mnu_SelectAllAccounts.Name = "mnu_SelectAllAccounts";
            this.mnu_SelectAllAccounts.Size = new Size(195, 22);
            this.mnu_SelectAllAccounts.Text = "Select All Accounts";
            this.mnu_SelectAllAccounts.Click += new EventHandler(this.mnu_SelectAllAccounts_Click);
            this.mnu_UnselectAllAccounts.Name = "mnu_UnselectAllAccounts";
            this.mnu_UnselectAllAccounts.Size = new Size(195, 22);
            this.mnu_UnselectAllAccounts.Text = "Un-Select All Accounts";
            this.mnu_UnselectAllAccounts.Click += new EventHandler(this.mnu_UnselectAllAccounts_Click);
            this.imageList1.ImageStream = (ImageListStreamer)Resources.EmailForm.imageList1_ImageStream;
            this.imageList1.TransparentColor = Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "people.png");
            this.progBar.BackColor = Color.Transparent;
            this.progBar.Location = new Point(127, 264);
            this.progBar.Name = "progBar";
            this.progBar.RoundedCornersMask = 15;
            this.progBar.RoundedCornersRadius = 0;
            this.progBar.Size = new Size(382, 19);
            this.progBar.TabIndex = 7;
            this.progBar.Value = 0;
            this.progBar.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.progBar.Visible = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(521, 480);
            base.Controls.Add(this.AccountList);
            base.Controls.Add(this.HeaderPanel);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Icon = (Icon)Resources.EmailForm.EmailFormIcon;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "EmailForm";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Alert Emails";
            base.Load += new EventHandler(this.EmailForm_Load);
            this.HeaderPanel.ResumeLayout(false);
            this.ListMenu.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void ListCheck(bool b)
        {
            foreach (ListItem item in this.AccountList.Items)
            {
                item.IsChecked = new bool?(b);
            }
        }

        private void LoadAccounts()
        {
            List<Account> accounts = new List<Account>();
            using (RPM_Account rPMAccount = new RPM_Account())
            {
                accounts = rPMAccount.GetAlertList();
            }
            if (accounts.Count > 0)
            {
                foreach (Account account in accounts)
                {
                    ListItem listItem = new ListItem()
                    {
                        Text = string.Format("{0}  •  {1}  •  {2}", account.ToString(), account.Email, account.BadgeNumber),
                        Tag = account,
                        ImageIndex = 0,
                        IsChecked = new bool?(false)
                    };
                    this.AccountList.Items.Add(listItem);
                }
            }
        }

        private void LoadProfile()
        {
            this.IsEnabled = Convert.ToBoolean(this.GetCfg("MAIL_ALERT_ENABLED", false));
            if (this.IsEnabled)
            {
                this.txtFrom.Text = this.GetCfg("MAIL_LOGIN_ID", true);
                this.Pwd = this.GetCfg("MAIL_PASSWORD", true);
                this.string_0 = this.GetCfg("MAIL_CLIENT", false);
                this.Port = Convert.ToInt32(this.GetCfg("MAIL_PORT", false));
                this.IsSSL = Convert.ToBoolean(this.GetCfg("MAIL_SSL", false));
            }
        }

        private void mnu_SelectAllAccounts_Click(object sender, EventArgs e)
        {
            this.ListCheck(true);
        }

        private void mnu_UnselectAllAccounts_Click(object sender, EventArgs e)
        {
            this.ListCheck(false);
        }

        private void sendEmail(string to)
        {
            string string0 = this.string_0;
            int port = this.Port;
            bool isSSL = this.IsSSL;
            string str = this.txtFrom.Text.Trim();
            string pwd = this.Pwd;
            string str1 = to;
            string str2 = this.txtSubject.Text.Trim();
            string str3 = this.txtBody.Text.Trim();
            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(str);
                    mailMessage.To.Add(str1);
                    mailMessage.Subject = str2;
                    mailMessage.Body = str3;
                    mailMessage.IsBodyHtml = true;
                    using (SmtpClient smtpClient = new SmtpClient(string0, port))
                    {
                        smtpClient.Timeout = 3000;
                        smtpClient.Credentials = new NetworkCredential(str, pwd);
                        smtpClient.EnableSsl = isSSL;
                        smtpClient.Send(mailMessage);
                        this.SendCount++;
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                this.FailedList.Add(to);
            }
        }

        private void SetLanguage()
        {
            LangCtrl.reText(this);
            this.Text = LangCtrl.GetString("dlg_AlertEmails", "Alert Emails");
        }
    }
}