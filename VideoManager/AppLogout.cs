using AppGlobal;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;

namespace VideoManager
{
    public class AppLogout : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;

        public const int HT_CAPTION = 2;

        private const int CS_DROPSHADOW = 131072;

        private int Count;

        private IContainer components;

        private Panel FormPanel;

        private Panel HeaderPanel;

        private vButton btn_OK;

        private vTextBox txtPwd;

        private Label lbl_Password;

        private Label lbl_Logout;

        private vButton btn_Cancel;

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

        public AppLogout()
        {
            this.InitializeComponent();
        }

        private void AppLogout_Load(object sender, EventArgs e)
        {
            LangCtrl.reText(this);
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.CancelPwd();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            string str = CryptoIO.MD5Encrypt(this.txtPwd.Text);
            if (Global.GlobalAccount.Password.Equals(str))
            {
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
            this.Count++;
            this.txtPwd.Text = string.Empty;
            this.txtPwd.Select();
            if (this.Count > 3)
            {
                this.CancelPwd();
            }
        }

        private void CancelPwd()
        {
            base.DialogResult = DialogResult.Cancel;
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
                AppLogout.ReleaseCapture();
                AppLogout.SendMessage(base.Handle, 161, 2, 0);
            }
        }

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void InitializeComponent()
        {
            this.FormPanel = new Panel();
            this.HeaderPanel = new Panel();
            this.lbl_Logout = new Label();
            this.lbl_Password = new Label();
            this.txtPwd = new vTextBox();
            this.btn_OK = new vButton();
            this.btn_Cancel = new vButton();
            this.FormPanel.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            base.SuspendLayout();
            this.FormPanel.BorderStyle = BorderStyle.FixedSingle;
            this.FormPanel.Controls.Add(this.btn_Cancel);
            this.FormPanel.Controls.Add(this.btn_OK);
            this.FormPanel.Controls.Add(this.txtPwd);
            this.FormPanel.Controls.Add(this.lbl_Password);
            this.FormPanel.Controls.Add(this.HeaderPanel);
            this.FormPanel.Dock = DockStyle.Fill;
            this.FormPanel.Location = new Point(0, 0);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new Size(382, 173);
            this.FormPanel.TabIndex = 0;
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.Controls.Add(this.lbl_Logout);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(380, 40);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.lbl_Logout.AutoSize = true;
            this.lbl_Logout.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_Logout.ForeColor = Color.White;
            this.lbl_Logout.Location = new Point(12, 12);
            this.lbl_Logout.Name = "lbl_Logout";
            this.lbl_Logout.Size = new Size(189, 20);
            this.lbl_Logout.TabIndex = 0;
            this.lbl_Logout.Text = "LOGOUT PASSWORD";
            this.lbl_Logout.MouseDown += new MouseEventHandler(this.lbl_Logout_MouseDown);
            this.lbl_Password.AutoSize = true;
            this.lbl_Password.Location = new Point(16, 66);
            this.lbl_Password.Name = "lbl_Password";
            this.lbl_Password.Size = new Size(53, 13);
            this.lbl_Password.TabIndex = 1;
            this.lbl_Password.Text = "Password";
            this.txtPwd.BackColor = Color.White;
            this.txtPwd.BoundsOffset = new Size(1, 1);
            this.txtPwd.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtPwd.DefaultText = "";
            this.txtPwd.Location = new Point(136, 61);
            this.txtPwd.MaxLength = 32767;
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '•';
            this.txtPwd.ScrollBars = ScrollBars.None;
            this.txtPwd.SelectionLength = 0;
            this.txtPwd.SelectionStart = 0;
            this.txtPwd.Size = new Size(207, 23);
            this.txtPwd.TabIndex = 2;
            this.txtPwd.TextAlign = HorizontalAlignment.Left;
            this.txtPwd.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_OK.AllowAnimations = true;
            this.btn_OK.BackColor = Color.Transparent;
            this.btn_OK.Location = new Point(136, 103);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.RoundedCornersMask = 15;
            this.btn_OK.Size = new Size(100, 30);
            this.btn_OK.TabIndex = 3;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = false;
            this.btn_OK.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_OK.Click += new EventHandler(this.btn_OK_Click);
            this.btn_Cancel.AllowAnimations = true;
            this.btn_Cancel.BackColor = Color.Transparent;
            this.btn_Cancel.Location = new Point(243, 103);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.RoundedCornersMask = 15;
            this.btn_Cancel.Size = new Size(100, 30);
            this.btn_Cancel.TabIndex = 4;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Cancel.Click += new EventHandler(this.btn_Cancel_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(382, 173);
            base.Controls.Add(this.FormPanel);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "AppLogout";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "AppLogout";
            base.Load += new EventHandler(this.AppLogout_Load);
            this.FormPanel.ResumeLayout(false);
            this.FormPanel.PerformLayout();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            base.ResumeLayout(false);
        }

        private void lbl_Logout_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    }
}