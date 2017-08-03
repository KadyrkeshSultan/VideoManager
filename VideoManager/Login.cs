using AppGlobal;
using VideoManager.Properties;
using Logger;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMInterfaces;
using VMModels.Model;

namespace VideoManager
{
    public class Login : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;

        public const int HT_CAPTION = 2;

        private const int CS_DROPSHADOW = 131072;

        private int MaxTrys = 4;

        private int TryCount;

        private IContainer components;

        private Panel HeaderPanel;

        private vButton btn_Cancel;

        private vButton btn_Login;

        private Label lbl_LoginID;

        private Label lbl_Password;

        private vTextBox vTextBox_0;

        private vTextBox txtPwd;

        private Label lbl_C3Catalog;

        private PictureBox pictureBox1;

        private Label lbl_loginMsg;

        private Panel FormPanel;

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

        public Login()
        {
            this.InitializeComponent();
        }

        private void AppLogin()
        {
            try
            {
                this.lbl_loginMsg.Text = LangCtrl.GetString("lbl_loginMsg", "Authenticating login...");
                this.lbl_loginMsg.Refresh();
                this.TryCount++;
                if (this.TryCount >= this.MaxTrys)
                {
                    Logger.Logging.WriteAccountLog(VMGlobal.LOG_ACTION.LOGON_COUNT, this.vTextBox_0.Text, Guid.Empty);
                    Process.GetCurrentProcess().Kill();
                    base.DialogResult = DialogResult.Cancel;
                    base.Close();
                }
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    using (RPM_Account rPMAccount = new RPM_Account())
                    {
                        Account account = rPMAccount.Authenticate(vTextBox_0.Text, CryptoIO.MD5Encrypt(this.txtPwd.Text));
                        if (account == null)
                        {
                            Logger.Logging.WriteAccountLog(VMGlobal.LOG_ACTION.LOGON_FAILED, this.vTextBox_0.Text, Guid.Empty);
                        }
                        else
                        {
                            Global.LoginIDName = (this.vTextBox_0.Text);
                            Global.GlobalAccount = account;
                            Global.RightsProfile = account.ApplicationRights;
                            this.Cursor = Cursors.Default;
                            base.DialogResult = DialogResult.OK;
                            if (account.IsPwdReset.Value)
                            {
                                PwdReset pwdReset = new PwdReset()
                                {
                                    AccountID = account.Id
                                };
                                if (pwdReset.ShowDialog(this) == DialogResult.Cancel)
                                {
                                    Logger.Logging.WriteAccountLog(VMGlobal.LOG_ACTION.PASSWORD, string.Concat("Password reset canceled: ", this.vTextBox_0.Text), account.Id);
                                    base.DialogResult = DialogResult.Cancel;
                                    base.Close();
                                }
                                Logger.Logging.WriteAccountLog(VMGlobal.LOG_ACTION.PASSWORD, string.Concat("Password reset: ", this.vTextBox_0.Text), account.Id);
                            }
                            base.DialogResult = DialogResult.OK;
                            base.Close();
                        }
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception exception)
            {
            }
            if (!Global.LockLogin)
            {
                this.vTextBox_0.Text = string.Empty;
            }
            this.lbl_loginMsg.Text = string.Empty;
            this.txtPwd.Text = string.Empty;
            this.vTextBox_0.Select();
            this.Cursor = Cursors.Default;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            this.AppLogin();
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
                Login.ReleaseCapture();
                Login.SendMessage(base.Handle, 161, 2, 0);
            }
        }

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Login));
            this.btn_Cancel = new vButton();
            this.btn_Login = new vButton();
            this.lbl_LoginID = new Label();
            this.lbl_Password = new Label();
            this.vTextBox_0 = new vTextBox();
            this.txtPwd = new vTextBox();
            this.lbl_loginMsg = new Label();
            this.FormPanel = new Panel();
            this.HeaderPanel = new Panel();
            this.pictureBox1 = new PictureBox();
            this.lbl_C3Catalog = new Label();
            this.FormPanel.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            base.SuspendLayout();
            this.btn_Cancel.AllowAnimations = true;
            this.btn_Cancel.BackColor = Color.Transparent;
            this.btn_Cancel.DialogResult = DialogResult.Cancel;
            this.btn_Cancel.Location = new Point(159, 93);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.RoundedCornersMask = 15;
            this.btn_Cancel.RoundedCornersRadius = 0;
            this.btn_Cancel.Size = new Size(100, 30);
            this.btn_Cancel.TabIndex = 6;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Cancel.Click += new EventHandler(this.btn_Cancel_Click);
            this.btn_Login.AllowAnimations = true;
            this.btn_Login.BackColor = Color.Transparent;
            this.btn_Login.Location = new Point(53, 93);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.RoundedCornersMask = 15;
            this.btn_Login.RoundedCornersRadius = 0;
            this.btn_Login.Size = new Size(100, 30);
            this.btn_Login.TabIndex = 5;
            this.btn_Login.Text = "Login";
            this.btn_Login.UseVisualStyleBackColor = false;
            this.btn_Login.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Login.Click += new EventHandler(this.btn_Login_Click);
            this.lbl_LoginID.Location = new Point(9, 13);
            this.lbl_LoginID.Name = "lbl_LoginID";
            this.lbl_LoginID.Size = new Size(90, 13);
            this.lbl_LoginID.TabIndex = 1;
            this.lbl_LoginID.Text = "Login ID";
            this.lbl_Password.Location = new Point(12, 44);
            this.lbl_Password.Name = "lbl_Password";
            this.lbl_Password.Size = new Size(90, 13);
            this.lbl_Password.TabIndex = 3;
            this.lbl_Password.Text = "Password";
            this.vTextBox_0.BackColor = Color.White;
            this.vTextBox_0.BoundsOffset = new Size(1, 1);
            this.vTextBox_0.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.vTextBox_0.DefaultText = "";
            this.vTextBox_0.Location = new Point(109, 8);
            this.vTextBox_0.MaxLength = 32;
            this.vTextBox_0.Name = "txtLoginID";
            this.vTextBox_0.PasswordChar = '\0';
            this.vTextBox_0.ScrollBars = ScrollBars.None;
            this.vTextBox_0.SelectionLength = 0;
            this.vTextBox_0.SelectionStart = 0;
            this.vTextBox_0.Size = new Size(150, 23);
            this.vTextBox_0.TabIndex = 2;
            this.vTextBox_0.TextAlign = HorizontalAlignment.Left;
            this.vTextBox_0.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.txtPwd.BackColor = Color.White;
            this.txtPwd.BoundsOffset = new Size(1, 1);
            this.txtPwd.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtPwd.DefaultText = "";
            this.txtPwd.Location = new Point(109, 39);
            this.txtPwd.MaxLength = 32;
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '•';
            this.txtPwd.ScrollBars = ScrollBars.None;
            this.txtPwd.SelectionLength = 0;
            this.txtPwd.SelectionStart = 0;
            this.txtPwd.Size = new Size(150, 23);
            this.txtPwd.TabIndex = 4;
            this.txtPwd.TextAlign = HorizontalAlignment.Left;
            this.txtPwd.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.txtPwd.KeyDown += new KeyEventHandler(this.txtPwd_KeyDown);
            this.lbl_loginMsg.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lbl_loginMsg.Location = new Point(109, 71);
            this.lbl_loginMsg.Name = "lbl_loginMsg";
            this.lbl_loginMsg.Size = new Size(150, 12);
            this.lbl_loginMsg.TabIndex = 7;
            this.lbl_loginMsg.TextAlign = ContentAlignment.MiddleLeft;
            this.FormPanel.BorderStyle = BorderStyle.FixedSingle;
            this.FormPanel.Controls.Add(this.btn_Cancel);
            this.FormPanel.Controls.Add(this.lbl_loginMsg);
            this.FormPanel.Controls.Add(this.btn_Login);
            this.FormPanel.Controls.Add(this.txtPwd);
            this.FormPanel.Controls.Add(this.lbl_LoginID);
            this.FormPanel.Controls.Add(this.vTextBox_0);
            this.FormPanel.Controls.Add(this.lbl_Password);
            this.FormPanel.Dock = DockStyle.Fill;
            this.FormPanel.Location = new Point(0, 40);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new Size(293, 142);
            this.FormPanel.TabIndex = 8;
            this.FormPanel.Paint += new PaintEventHandler(this.panel1_Paint);
            this.HeaderPanel.BackgroundImage = Properties.Resources.header;
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.pictureBox1);
            this.HeaderPanel.Controls.Add(this.lbl_C3Catalog);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(293, 40);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.pictureBox1.BackColor = Color.Transparent;
            this.pictureBox1.Dock = DockStyle.Left;
            this.pictureBox1.Image = Properties.Resources.catalog2;
            this.pictureBox1.Location = new Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(32, 40);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new MouseEventHandler(this.pictureBox1_MouseDown);
            this.lbl_C3Catalog.AutoSize = true;
            this.lbl_C3Catalog.BackColor = Color.Transparent;
            this.lbl_C3Catalog.Font = new Font("Verdana", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_C3Catalog.ForeColor = Color.White;
            this.lbl_C3Catalog.Location = new Point(37, 4);
            this.lbl_C3Catalog.Name = "lbl_C3Catalog";
            this.lbl_C3Catalog.Size = new Size(150, 16);
            this.lbl_C3Catalog.TabIndex = 2;
            this.lbl_C3Catalog.Text = "C3 Sentinel Catalog";
            this.lbl_C3Catalog.MouseDown += new MouseEventHandler(this.lbl_C3Catalog_MouseDown);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(293, 182);
            base.Controls.Add(this.FormPanel);
            base.Controls.Add(this.HeaderPanel);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Icon = (Icon)Resources.Login.LoginIcon;
            base.Name = "Login";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Login";
            base.Load += new EventHandler(this.Login_Load);
            base.Paint += new PaintEventHandler(this.Login_Paint);
            this.FormPanel.ResumeLayout(false);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((ISupportInitialize)this.pictureBox1).EndInit();
            base.ResumeLayout(false);
        }

        private void lbl_C3Catalog_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void Login_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
            {
                this.HeaderPanel.BackgroundImage = Properties.Resources.topbar45;
            }
            this.SetLanguage();
            this.lbl_loginMsg.Text = string.Empty;
            base.TopMost = true;
            if (Global.LockLogin)
            {
                this.vTextBox_0.Text = Environment.UserName;
                this.vTextBox_0.Enabled = false;
            }
        }

        private void Login_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder3D(e.Graphics, ((Control)sender).ClientRectangle, Border3DStyle.RaisedOuter);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void SetLanguage()
        {
            LangCtrl.reText(this);
        }

        private void txtPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.AppLogin();
                e.Handled = true;
            }
        }
    }
}