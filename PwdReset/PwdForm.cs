using AppGlobal;
using VMSUtil;
using PwdReset.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMInterfaces;

namespace PwdReset
{
    public class PwdForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;

        public const int HT_CAPTION = 2;

        private const int CS_DROPSHADOW = 131072;

        private IContainer components;

        private Panel HeaderPanel;

        private PictureBox PwdPic;

        private PageSetupDialog pageSetupDialog1;

        private Panel MainPanel;

        private vButton btnClose;

        private Label lbl_ResetPwd;

        private vTextBox txtOldPwd;

        private vButton btn_ChangePwd;

        private Label lbl_OldPassword;

        private vTextBox txtPwd2;

        private Label lbl_NewPwd;

        private Label lbl_RetypePwd;

        private vTextBox txtPwd1;

        private vTextBox vTextBox_0;

        private Label lbl_OldPIN;

        private vTextBox vTextBox_1;

        private Label lbl_NewPIN;

        private Label lblLine;

        private vButton btn_ChangePIN;

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

        public PwdForm()
        {
            InitializeComponent();
        }

        private void btn_ChangePIN_Click(object sender, EventArgs e)
        {
            Guid id = Global.GlobalAccount.Id;
            bool flag = false;
            if (!string.IsNullOrEmpty(this.vTextBox_1.Text))
            {
                using (RPM_Account rPMAccount = new RPM_Account())
                {
                    flag = rPMAccount.UpdatePIN(id, this.vTextBox_0.Text, this.vTextBox_1.Text);
                }
                if (flag)
                {
                    MessageBox.Show(this, LangCtrl.GetString("txt_PINChanged", "PIN changed."), "PIN", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    FormCtrl.ClearForm(this.MainPanel);
                    return;
                }
                MessageBox.Show(this, LangCtrl.GetString("txt_PINnotChanged", "PIN NOT changed."), "PIN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.vTextBox_1.Select();
            }
        }

        private void btn_ChangePwd_Click(object sender, EventArgs e)
        {
            Guid id = Global.GlobalAccount.Id;
            bool flag = false;
            if (!this.txtPwd1.Text.Equals(this.txtPwd2.Text))
            {
                MessageBox.Show(this, LangCtrl.GetString("msg_PwdMatch", "Passwords do not match."), "Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtPwd1.Select();
            }
            else if (!Utility.CheckPassword(this.txtPwd1.Text))
            {
                MessageBox.Show(this, Utility.PwdMsg, "Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtPwd1.Select();
                return;
            }
            if (string.IsNullOrEmpty(this.txtPwd1.Text))
            {
                MessageBox.Show(this, LangCtrl.GetString("txt_PwdEmpty", "Password cannot be empty."), "Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtPwd1.Select();
                return;
            }
            if (!this.txtPwd1.Text.Equals(this.txtPwd2.Text))
            {
                MessageBox.Show(this, LangCtrl.GetString("txt_PwdMatch", "Passwords do not match."), "Password", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.txtPwd1.Select();
                return;
            }
            using (RPM_Account rPMAccount = new RPM_Account())
            {
                string str = CryptoIO.MD5Encrypt(this.txtPwd1.Text);
                string str1 = CryptoIO.MD5Encrypt(this.txtOldPwd.Text);
                flag = rPMAccount.UpdatePassword(id, str1, str);
            }
            if (!flag)
            {
                MessageBox.Show(this, LangCtrl.GetString("txt_PwdNotChanged", "Password NOT changed. Please check old password."), "Password", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            MessageBox.Show(this, LangCtrl.GetString("txt_PwdChanged", "Password changed."), "Password", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            FormCtrl.ClearForm(this.MainPanel);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
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
                PwdForm.ReleaseCapture();
                PwdForm.SendMessage(base.Handle, 161, 2, 0);
            }
        }

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void InitializeComponent()
        {
            this.pageSetupDialog1 = new PageSetupDialog();
            this.MainPanel = new Panel();
            this.btn_ChangePIN = new vButton();
            this.vTextBox_0 = new vTextBox();
            this.lbl_OldPIN = new Label();
            this.vTextBox_1 = new vTextBox();
            this.lbl_NewPIN = new Label();
            this.lblLine = new Label();
            this.txtOldPwd = new vTextBox();
            this.btn_ChangePwd = new vButton();
            this.lbl_OldPassword = new Label();
            this.txtPwd1 = new vTextBox();
            this.txtPwd2 = new vTextBox();
            this.lbl_RetypePwd = new Label();
            this.lbl_NewPwd = new Label();
            this.HeaderPanel = new Panel();
            this.btnClose = new vButton();
            this.lbl_ResetPwd = new Label();
            this.PwdPic = new PictureBox();
            this.MainPanel.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((ISupportInitialize)this.PwdPic).BeginInit();
            base.SuspendLayout();
            this.MainPanel.BorderStyle = BorderStyle.FixedSingle;
            this.MainPanel.Controls.Add(this.btn_ChangePIN);
            this.MainPanel.Controls.Add(this.vTextBox_0);
            this.MainPanel.Controls.Add(this.lbl_OldPIN);
            this.MainPanel.Controls.Add(this.vTextBox_1);
            this.MainPanel.Controls.Add(this.lbl_NewPIN);
            this.MainPanel.Controls.Add(this.lblLine);
            this.MainPanel.Controls.Add(this.txtOldPwd);
            this.MainPanel.Controls.Add(this.btn_ChangePwd);
            this.MainPanel.Controls.Add(this.lbl_OldPassword);
            this.MainPanel.Controls.Add(this.txtPwd1);
            this.MainPanel.Controls.Add(this.txtPwd2);
            this.MainPanel.Controls.Add(this.lbl_RetypePwd);
            this.MainPanel.Controls.Add(this.lbl_NewPwd);
            this.MainPanel.Dock = DockStyle.Fill;
            this.MainPanel.Location = new Point(0, 40);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new Size(362, 246);
            this.MainPanel.TabIndex = 0;
            this.btn_ChangePIN.AllowAnimations = true;
            this.btn_ChangePIN.BackColor = Color.Transparent;
            this.btn_ChangePIN.Location = new Point(159, 199);
            this.btn_ChangePIN.Name = "btn_ChangePIN";
            this.btn_ChangePIN.RoundedCornersMask = 15;
            this.btn_ChangePIN.Size = new Size(150, 30);
            this.btn_ChangePIN.TabIndex = 12;
            this.btn_ChangePIN.Text = "Change PIN";
            this.btn_ChangePIN.UseVisualStyleBackColor = false;
            this.btn_ChangePIN.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_ChangePIN.Click += new EventHandler(this.btn_ChangePIN_Click);
            this.vTextBox_0.BackColor = Color.White;
            this.vTextBox_0.BoundsOffset = new Size(1, 1);
            this.vTextBox_0.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.vTextBox_0.DefaultText = "";
            this.vTextBox_0.Location = new Point(159, 141);
            this.vTextBox_0.MaxLength = 12;
            this.vTextBox_0.Name = "txtOldPIN";
            this.vTextBox_0.PasswordChar = '\0';
            this.vTextBox_0.ScrollBars = ScrollBars.None;
            this.vTextBox_0.SelectionLength = 0;
            this.vTextBox_0.SelectionStart = 0;
            this.vTextBox_0.Size = new Size(150, 23);
            this.vTextBox_0.TabIndex = 9;
            this.vTextBox_0.TextAlign = HorizontalAlignment.Left;
            this.vTextBox_0.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_OldPIN.AutoSize = true;
            this.lbl_OldPIN.Location = new Point(10, 146);
            this.lbl_OldPIN.Name = "lbl_OldPIN";
            this.lbl_OldPIN.Size = new Size(44, 13);
            this.lbl_OldPIN.TabIndex = 8;
            this.lbl_OldPIN.Text = "Old PIN";
            this.vTextBox_1.BackColor = Color.White;
            this.vTextBox_1.BoundsOffset = new Size(1, 1);
            this.vTextBox_1.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.vTextBox_1.DefaultText = "";
            this.vTextBox_1.Location = new Point(159, 170);
            this.vTextBox_1.MaxLength = 12;
            this.vTextBox_1.Name = "txtNewPIN";
            this.vTextBox_1.PasswordChar = '\0';
            this.vTextBox_1.ScrollBars = ScrollBars.None;
            this.vTextBox_1.SelectionLength = 0;
            this.vTextBox_1.SelectionStart = 0;
            this.vTextBox_1.Size = new Size(150, 23);
            this.vTextBox_1.TabIndex = 11;
            this.vTextBox_1.TextAlign = HorizontalAlignment.Left;
            this.vTextBox_1.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_NewPIN.AutoSize = true;
            this.lbl_NewPIN.Location = new Point(10, 175);
            this.lbl_NewPIN.Name = "lbl_NewPIN";
            this.lbl_NewPIN.Size = new Size(50, 13);
            this.lbl_NewPIN.TabIndex = 10;
            this.lbl_NewPIN.Text = "New PIN";
            this.lblLine.BackColor = Color.Gray;
            this.lblLine.Location = new Point(6, 131);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new Size(343, 1);
            this.lblLine.TabIndex = 7;
            this.txtOldPwd.BackColor = Color.White;
            this.txtOldPwd.BoundsOffset = new Size(1, 1);
            this.txtOldPwd.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtOldPwd.DefaultText = "";
            this.txtOldPwd.Location = new Point(159, 5);
            this.txtOldPwd.MaxLength = 32;
            this.txtOldPwd.Name = "txtOldPwd";
            this.txtOldPwd.PasswordChar = '•';
            this.txtOldPwd.ScrollBars = ScrollBars.None;
            this.txtOldPwd.SelectionLength = 0;
            this.txtOldPwd.SelectionStart = 0;
            this.txtOldPwd.Size = new Size(150, 23);
            this.txtOldPwd.TabIndex = 1;
            this.txtOldPwd.TextAlign = HorizontalAlignment.Left;
            this.txtOldPwd.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_ChangePwd.AllowAnimations = true;
            this.btn_ChangePwd.BackColor = Color.Transparent;
            this.btn_ChangePwd.Location = new Point(159, 93);
            this.btn_ChangePwd.Name = "btn_ChangePwd";
            this.btn_ChangePwd.RoundedCornersMask = 15;
            this.btn_ChangePwd.Size = new Size(150, 30);
            this.btn_ChangePwd.TabIndex = 6;
            this.btn_ChangePwd.Text = "Change Password";
            this.btn_ChangePwd.UseVisualStyleBackColor = false;
            this.btn_ChangePwd.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_ChangePwd.Click += new EventHandler(this.btn_ChangePwd_Click);
            this.lbl_OldPassword.AutoSize = true;
            this.lbl_OldPassword.Location = new Point(10, 10);
            this.lbl_OldPassword.Name = "lbl_OldPassword";
            this.lbl_OldPassword.Size = new Size(72, 13);
            this.lbl_OldPassword.TabIndex = 0;
            this.lbl_OldPassword.Text = "Old Password";
            this.txtPwd1.BackColor = Color.White;
            this.txtPwd1.BoundsOffset = new Size(1, 1);
            this.txtPwd1.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtPwd1.DefaultText = "";
            this.txtPwd1.Location = new Point(159, 34);
            this.txtPwd1.MaxLength = 32;
            this.txtPwd1.Name = "txtPwd1";
            this.txtPwd1.PasswordChar = '•';
            this.txtPwd1.ScrollBars = ScrollBars.None;
            this.txtPwd1.SelectionLength = 0;
            this.txtPwd1.SelectionStart = 0;
            this.txtPwd1.Size = new Size(150, 23);
            this.txtPwd1.TabIndex = 3;
            this.txtPwd1.TextAlign = HorizontalAlignment.Left;
            this.txtPwd1.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.txtPwd2.BackColor = Color.White;
            this.txtPwd2.BoundsOffset = new Size(1, 1);
            this.txtPwd2.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtPwd2.DefaultText = "";
            this.txtPwd2.Location = new Point(159, 63);
            this.txtPwd2.MaxLength = 32;
            this.txtPwd2.Name = "txtPwd2";
            this.txtPwd2.PasswordChar = '•';
            this.txtPwd2.ScrollBars = ScrollBars.None;
            this.txtPwd2.SelectionLength = 0;
            this.txtPwd2.SelectionStart = 0;
            this.txtPwd2.Size = new Size(150, 23);
            this.txtPwd2.TabIndex = 5;
            this.txtPwd2.TextAlign = HorizontalAlignment.Left;
            this.txtPwd2.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_RetypePwd.AutoSize = true;
            this.lbl_RetypePwd.Location = new Point(10, 68);
            this.lbl_RetypePwd.Name = "lbl_RetypePwd";
            this.lbl_RetypePwd.Size = new Size(115, 13);
            this.lbl_RetypePwd.TabIndex = 4;
            this.lbl_RetypePwd.Text = "Retype New Password";
            this.lbl_NewPwd.AutoSize = true;
            this.lbl_NewPwd.Location = new Point(10, 39);
            this.lbl_NewPwd.Name = "lbl_NewPwd";
            this.lbl_NewPwd.Size = new Size(78, 13);
            this.lbl_NewPwd.TabIndex = 2;
            this.lbl_NewPwd.Text = "New Password";
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.btnClose);
            this.HeaderPanel.Controls.Add(this.lbl_ResetPwd);
            this.HeaderPanel.Controls.Add(this.PwdPic);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(362, 40);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.btnClose.AllowAnimations = true;
            this.btnClose.BackColor = Color.Transparent;
            this.btnClose.Dock = DockStyle.Right;
            this.btnClose.Image = Resources.close;
            this.btnClose.Location = new Point(315, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaintBorder = false;
            this.btnClose.PaintDefaultBorder = false;
            this.btnClose.PaintDefaultFill = false;
            this.btnClose.RoundedCornersMask = 15;
            this.btnClose.RoundedCornersRadius = 0;
            this.btnClose.Size = new Size(47, 40);
            this.btnClose.TabIndex = 0;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.lbl_ResetPwd.AutoSize = true;
            this.lbl_ResetPwd.BackColor = Color.Transparent;
            this.lbl_ResetPwd.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_ResetPwd.ForeColor = Color.White;
            this.lbl_ResetPwd.Location = new Point(49, 6);
            this.lbl_ResetPwd.Name = "lbl_ResetPwd";
            this.lbl_ResetPwd.Size = new Size(150, 16);
            this.lbl_ResetPwd.TabIndex = 0;
            this.lbl_ResetPwd.Text = "RESET PASSWORD";
            this.lbl_ResetPwd.TextAlign = ContentAlignment.MiddleLeft;
            this.lbl_ResetPwd.MouseDown += new MouseEventHandler(this.lbl_ResetPwd_MouseDown);
            this.PwdPic.BackColor = Color.Transparent;
            this.PwdPic.BackgroundImage = Resources.Password;
            this.PwdPic.Location = new Point(7, 4);
            this.PwdPic.Margin = new Padding(0);
            this.PwdPic.Name = "PwdPic";
            this.PwdPic.Size = new Size(32, 32);
            this.PwdPic.SizeMode = PictureBoxSizeMode.CenterImage;
            this.PwdPic.TabIndex = 0;
            this.PwdPic.TabStop = false;
            this.PwdPic.MouseDown += new MouseEventHandler(this.PwdPic_MouseDown);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(362, 286);
            base.Controls.Add(this.MainPanel);
            base.Controls.Add(this.HeaderPanel);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "PwdForm";
            base.StartPosition = FormStartPosition.CenterParent;
            base.Load += new EventHandler(this.PwdForm_Load);
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((ISupportInitialize)this.PwdPic).EndInit();
            base.ResumeLayout(false);
        }

        private void lbl_ResetPwd_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void PwdForm_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
            {
                this.HeaderPanel.BackgroundImage = Resources.topbar45;
                this.btnClose.VIBlendTheme = VIBLEND_THEME.NERO;
            }
            this.txtOldPwd.Select();
            this.SetLanguage();
        }

        private void PwdPic_MouseDown(object sender, MouseEventArgs e)
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
    }
}