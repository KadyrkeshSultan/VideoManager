using AppGlobal;
using VideoManager.Properties;
using VMSUtil;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMInterfaces;
using VMModels.Model;

namespace VideoManager
{
    public class PwdReset : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;

        public const int HT_CAPTION = 2;

        private const int CS_DROPSHADOW = 131072;

        private IContainer components;

        private Panel HeaderPanel;

        private PictureBox pictureBox1;

        private Label lbl_ResetPassword;

        private Label lbl_NewPwd;

        private vTextBox txtPwd1;

        private vTextBox txtPwd2;

        private Label lbl_ConfirmPwd;

        private vButton btn_Cancel;

        private vButton btn_OK;

        public Guid AccountID
        {
            get;
            set;
        }

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

        public PwdReset()
        {
            this.InitializeComponent();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (!this.txtPwd1.Text.Equals(this.txtPwd2.Text))
            {
                MessageBox.Show(this, LangCtrl.GetString("msg_PwdMatch", "Passwords do not match."), "Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtPwd1.Select();
                return;
            }
            if (!Utility.CheckPassword(this.txtPwd1.Text))
            {
                MessageBox.Show(this, Utility.PwdMsg, "Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtPwd1.Select();
                return;
            }
            if (!this.txtPwd1.Text.Equals(this.txtPwd2.Text) || string.IsNullOrEmpty(this.txtPwd1.Text))
            {
                MessageBox.Show(this, "Password Error", "Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                vTextBox _vTextBox = this.txtPwd1;
                vTextBox _vTextBox1 = this.txtPwd2;
                string empty = string.Empty;
                string str = empty;
                _vTextBox1.Text = empty;
                _vTextBox.Text = str;
                this.txtPwd1.Select();
            }
            else if (this.AccountID != Guid.Empty)
            {
                string str1 = CryptoIO.MD5Encrypt(this.txtPwd1.Text);
                using (RPM_Account rPMAccount = new RPM_Account())
                {
                    Account account = rPMAccount.GetAccount(this.AccountID);
                    account.Password = str1;
                    account.IsPwdReset = new bool?(false);
                    rPMAccount.InsertUpdate(account);
                    rPMAccount.Save();
                }
                base.DialogResult = DialogResult.OK;
                base.Close();
                return;
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

        private void HeaderMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PwdReset.ReleaseCapture();
                PwdReset.SendMessage(base.Handle, 161, 2, 0);
            }
        }

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void InitializeComponent()
        {
            this.pictureBox1 = new PictureBox();
            this.HeaderPanel = new Panel();
            this.lbl_ResetPassword = new Label();
            this.lbl_NewPwd = new Label();
            this.txtPwd1 = new vTextBox();
            this.txtPwd2 = new vTextBox();
            this.lbl_ConfirmPwd = new Label();
            this.btn_Cancel = new vButton();
            this.btn_OK = new vButton();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            this.HeaderPanel.SuspendLayout();
            base.SuspendLayout();
            this.pictureBox1.BackColor = Color.Transparent;
            this.pictureBox1.Image = Properties.Resources.Password;
            this.pictureBox1.Location = new Point(5, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(36, 36);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new MouseEventHandler(this.pictureBox1_MouseDown);
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.lbl_ResetPassword);
            this.HeaderPanel.Controls.Add(this.pictureBox1);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(378, 40);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.lbl_ResetPassword.AutoSize = true;
            this.lbl_ResetPassword.BackColor = Color.Transparent;
            this.lbl_ResetPassword.Font = new Font("Verdana", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_ResetPassword.ForeColor = Color.White;
            this.lbl_ResetPassword.Location = new Point(48, 13);
            this.lbl_ResetPassword.Name = "lbl_ResetPassword";
            this.lbl_ResetPassword.Size = new Size(123, 16);
            this.lbl_ResetPassword.TabIndex = 1;
            this.lbl_ResetPassword.Text = "Reset Password";
            this.lbl_ResetPassword.MouseDown += new MouseEventHandler(this.lbl_ResetPassword_MouseDown);
            this.lbl_NewPwd.Location = new Point(13, 54);
            this.lbl_NewPwd.Name = "lbl_NewPwd";
            this.lbl_NewPwd.Size = new Size(110, 13);
            this.lbl_NewPwd.TabIndex = 0;
            this.lbl_NewPwd.Text = "New Password";
            this.txtPwd1.BackColor = Color.White;
            this.txtPwd1.BoundsOffset = new Size(1, 1);
            this.txtPwd1.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtPwd1.DefaultText = "";
            this.txtPwd1.Location = new Point(131, 49);
            this.txtPwd1.MaxLength = 32;
            this.txtPwd1.Name = "txtPwd1";
            this.txtPwd1.PasswordChar = '•';
            this.txtPwd1.ScrollBars = ScrollBars.None;
            this.txtPwd1.SelectionLength = 0;
            this.txtPwd1.SelectionStart = 0;
            this.txtPwd1.Size = new Size(208, 23);
            this.txtPwd1.TabIndex = 1;
            this.txtPwd1.TextAlign = HorizontalAlignment.Left;
            this.txtPwd1.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.txtPwd2.BackColor = Color.White;
            this.txtPwd2.BoundsOffset = new Size(1, 1);
            this.txtPwd2.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtPwd2.DefaultText = "";
            this.txtPwd2.Location = new Point(131, 78);
            this.txtPwd2.MaxLength = 32;
            this.txtPwd2.Name = "txtPwd2";
            this.txtPwd2.PasswordChar = '•';
            this.txtPwd2.ScrollBars = ScrollBars.None;
            this.txtPwd2.SelectionLength = 0;
            this.txtPwd2.SelectionStart = 0;
            this.txtPwd2.Size = new Size(208, 23);
            this.txtPwd2.TabIndex = 3;
            this.txtPwd2.TextAlign = HorizontalAlignment.Left;
            this.txtPwd2.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_ConfirmPwd.Location = new Point(13, 83);
            this.lbl_ConfirmPwd.Name = "lbl_ConfirmPwd";
            this.lbl_ConfirmPwd.Size = new Size(110, 13);
            this.lbl_ConfirmPwd.TabIndex = 2;
            this.lbl_ConfirmPwd.Text = "Retype Password";
            this.btn_Cancel.AllowAnimations = true;
            this.btn_Cancel.BackColor = Color.Transparent;
            this.btn_Cancel.Location = new Point(239, 119);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.RoundedCornersMask = 15;
            this.btn_Cancel.Size = new Size(100, 30);
            this.btn_Cancel.TabIndex = 5;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Cancel.Click += new EventHandler(this.btn_Cancel_Click);
            this.btn_OK.AllowAnimations = true;
            this.btn_OK.BackColor = Color.Transparent;
            this.btn_OK.Location = new Point(133, 119);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.RoundedCornersMask = 15;
            this.btn_OK.Size = new Size(100, 30);
            this.btn_OK.TabIndex = 4;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = false;
            this.btn_OK.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_OK.Click += new EventHandler(this.btn_OK_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(378, 161);
            base.Controls.Add(this.btn_OK);
            base.Controls.Add(this.btn_Cancel);
            base.Controls.Add(this.txtPwd2);
            base.Controls.Add(this.lbl_ConfirmPwd);
            base.Controls.Add(this.txtPwd1);
            base.Controls.Add(this.lbl_NewPwd);
            base.Controls.Add(this.HeaderPanel);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "PwdReset";
            base.StartPosition = FormStartPosition.CenterParent;
            base.Load += new EventHandler(this.PwdReset_Load);
            base.Paint += new PaintEventHandler(this.PwdReset_Paint);
            ((ISupportInitialize)this.pictureBox1).EndInit();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            base.ResumeLayout(false);
        }

        private void lbl_ResetPassword_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void PwdReset_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
            {
                this.HeaderPanel.BackgroundImage = Properties.Resources.topbar45;
            }
            this.SetLanguage();
        }

        private void PwdReset_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder3D(e.Graphics, ((Control)sender).ClientRectangle, Border3DStyle.RaisedOuter);
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