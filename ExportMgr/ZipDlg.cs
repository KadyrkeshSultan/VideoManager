using AppGlobal;
using ExportMgr.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;

namespace ExportMgr
{
    public class ZipDlg : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;

        public const int HT_CAPTION = 2;

        private const int CS_DROPSHADOW = 131072;

        private IContainer components;

        private Panel HeaderPanel;

        private Panel FormPanel;

        private Panel PwdPanel;

        private vTextBox txtPwd2;

        private Label lbl_ZipPwd2;

        private vTextBox txtPwd1;

        private Label lbl_ZipPwd1;

        private vCheckBox chk_PwdProtect;

        private vTextBox txtFileName;

        private Label lbl_ZipFileName;

        private Label lbl_SecureFile;

        private vButton btn_Cancel;

        private vButton btn_OK;

        private Label lblFilePath;

        private vCheckBox chk_ZipFile;

        private Panel ZipPanel;

        private Label lbl_Folder;

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

        public string FilePath
        {
            get;
            set;
        }

        public bool IsPwd
        {
            get;
            set;
        }

        public bool IsZipFile
        {
            get;
            set;
        }

        public string ZipName
        {
            get;
            set;
        }

        public string ZipPwd
        {
            get;
            set;
        }

        public ZipDlg()
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
            if (this.chk_ZipFile.Checked)
            {
                this.IsZipFile = true;
                if (this.chk_PwdProtect.Checked)
                {
                    if (string.IsNullOrEmpty(this.txtPwd1.Text) | !this.txtPwd1.Text.Equals(this.txtPwd2.Text))
                    {
                        MessageBox.Show(this, "Password required or password mismatch.", "Password", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                    this.IsPwd = true;
                    this.ZipPwd = this.txtPwd1.Text;
                }
                if (string.IsNullOrEmpty(this.txtFileName.Text))
                {
                    MessageBox.Show(this, "File name required.", "File Name", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                this.ZipName = this.txtFileName.Text;
            }
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void chk_PwdProtect_CheckedChanged(object sender, EventArgs e)
        {
            this.PwdPanel.Enabled = this.chk_PwdProtect.Checked;
        }

        private void chk_ZipFile_CheckedChanged(object sender, EventArgs e)
        {
            this.ZipPanel.Enabled = this.chk_ZipFile.Checked;
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
                ZipDlg.ReleaseCapture();
                ZipDlg.SendMessage(base.Handle, 161, 2, 0);
            }
        }

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void InitializeComponent()
        {
            this.HeaderPanel = new Panel();
            this.lbl_SecureFile = new Label();
            this.FormPanel = new Panel();
            this.chk_ZipFile = new vCheckBox();
            this.ZipPanel = new Panel();
            this.chk_PwdProtect = new vCheckBox();
            this.lbl_ZipFileName = new Label();
            this.txtFileName = new vTextBox();
            this.PwdPanel = new Panel();
            this.txtPwd2 = new vTextBox();
            this.lbl_ZipPwd2 = new Label();
            this.txtPwd1 = new vTextBox();
            this.lbl_ZipPwd1 = new Label();
            this.lbl_Folder = new Label();
            this.lblFilePath = new Label();
            this.btn_Cancel = new vButton();
            this.btn_OK = new vButton();
            this.HeaderPanel.SuspendLayout();
            this.FormPanel.SuspendLayout();
            this.ZipPanel.SuspendLayout();
            this.PwdPanel.SuspendLayout();
            base.SuspendLayout();
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.Controls.Add(this.lbl_SecureFile);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(360, 45);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.lbl_SecureFile.AutoSize = true;
            this.lbl_SecureFile.Font = new Font("Verdana", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_SecureFile.ForeColor = Color.White;
            this.lbl_SecureFile.Location = new Point(6, 11);
            this.lbl_SecureFile.Name = "lbl_SecureFile";
            this.lbl_SecureFile.Size = new Size(158, 18);
            this.lbl_SecureFile.TabIndex = 4;
            this.lbl_SecureFile.Text = "SECURE EXPORT";
            this.lbl_SecureFile.MouseDown += new MouseEventHandler(this.lbl_SecureFile_MouseDown);
            this.FormPanel.BorderStyle = BorderStyle.FixedSingle;
            this.FormPanel.Controls.Add(this.chk_ZipFile);
            this.FormPanel.Controls.Add(this.ZipPanel);
            this.FormPanel.Controls.Add(this.lbl_Folder);
            this.FormPanel.Controls.Add(this.lblFilePath);
            this.FormPanel.Controls.Add(this.btn_Cancel);
            this.FormPanel.Controls.Add(this.btn_OK);
            this.FormPanel.Dock = DockStyle.Fill;
            this.FormPanel.Location = new Point(0, 45);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new Size(360, 235);
            this.FormPanel.TabIndex = 1;
            this.chk_ZipFile.BackColor = Color.Transparent;
            this.chk_ZipFile.Location = new Point(126, 26);
            this.chk_ZipFile.Name = "chk_ZipFile";
            this.chk_ZipFile.Size = new Size(208, 24);
            this.chk_ZipFile.TabIndex = 9;
            this.chk_ZipFile.Text = "Export as Zip File";
            this.chk_ZipFile.UseVisualStyleBackColor = false;
            this.chk_ZipFile.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.chk_ZipFile.CheckedChanged += new EventHandler(this.chk_ZipFile_CheckedChanged);
            this.ZipPanel.Controls.Add(this.chk_PwdProtect);
            this.ZipPanel.Controls.Add(this.lbl_ZipFileName);
            this.ZipPanel.Controls.Add(this.txtFileName);
            this.ZipPanel.Controls.Add(this.PwdPanel);
            this.ZipPanel.Enabled = false;
            this.ZipPanel.Location = new Point(8, 56);
            this.ZipPanel.Name = "ZipPanel";
            this.ZipPanel.Size = new Size(337, 132);
            this.ZipPanel.TabIndex = 8;
            this.chk_PwdProtect.BackColor = Color.Transparent;
            this.chk_PwdProtect.Location = new Point(118, 33);
            this.chk_PwdProtect.Name = "chk_PwdProtect";
            this.chk_PwdProtect.Size = new Size(222, 24);
            this.chk_PwdProtect.TabIndex = 2;
            this.chk_PwdProtect.Text = "Password Protect File";
            this.chk_PwdProtect.UseVisualStyleBackColor = false;
            this.chk_PwdProtect.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.chk_PwdProtect.CheckedChanged += new EventHandler(this.chk_PwdProtect_CheckedChanged);
            this.lbl_ZipFileName.AutoSize = true;
            this.lbl_ZipFileName.Location = new Point(12, 9);
            this.lbl_ZipFileName.Name = "lbl_ZipFileName";
            this.lbl_ZipFileName.Size = new Size(54, 13);
            this.lbl_ZipFileName.TabIndex = 0;
            this.lbl_ZipFileName.Text = "File Name";
            this.txtFileName.BackColor = Color.White;
            this.txtFileName.BoundsOffset = new Size(1, 1);
            this.txtFileName.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtFileName.DefaultText = "";
            this.txtFileName.Location = new Point(118, 4);
            this.txtFileName.MaxLength = 64;
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.PasswordChar = '\0';
            this.txtFileName.ScrollBars = ScrollBars.None;
            this.txtFileName.SelectionLength = 0;
            this.txtFileName.SelectionStart = 0;
            this.txtFileName.Size = new Size(183, 23);
            this.txtFileName.TabIndex = 1;
            this.txtFileName.TextAlign = HorizontalAlignment.Left;
            this.txtFileName.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.PwdPanel.Controls.Add(this.txtPwd2);
            this.PwdPanel.Controls.Add(this.lbl_ZipPwd2);
            this.PwdPanel.Controls.Add(this.txtPwd1);
            this.PwdPanel.Controls.Add(this.lbl_ZipPwd1);
            this.PwdPanel.Enabled = false;
            this.PwdPanel.Location = new Point(0, 65);
            this.PwdPanel.Name = "PwdPanel";
            this.PwdPanel.Size = new Size(326, 64);
            this.PwdPanel.TabIndex = 3;
            this.txtPwd2.BackColor = Color.White;
            this.txtPwd2.BoundsOffset = new Size(1, 1);
            this.txtPwd2.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtPwd2.DefaultText = "";
            this.txtPwd2.Location = new Point(120, 32);
            this.txtPwd2.MaxLength = 64;
            this.txtPwd2.Name = "txtPwd2";
            this.txtPwd2.PasswordChar = '•';
            this.txtPwd2.ScrollBars = ScrollBars.None;
            this.txtPwd2.SelectionLength = 0;
            this.txtPwd2.SelectionStart = 0;
            this.txtPwd2.Size = new Size(183, 23);
            this.txtPwd2.TabIndex = 3;
            this.txtPwd2.TextAlign = HorizontalAlignment.Left;
            this.txtPwd2.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_ZipPwd2.AutoSize = true;
            this.lbl_ZipPwd2.Location = new Point(12, 37);
            this.lbl_ZipPwd2.Name = "lbl_ZipPwd2";
            this.lbl_ZipPwd2.Size = new Size(98, 13);
            this.lbl_ZipPwd2.TabIndex = 3;
            this.lbl_ZipPwd2.Text = "Re-Enter Password";
            this.txtPwd1.BackColor = Color.White;
            this.txtPwd1.BoundsOffset = new Size(1, 1);
            this.txtPwd1.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtPwd1.DefaultText = "";
            this.txtPwd1.Location = new Point(120, 3);
            this.txtPwd1.MaxLength = 64;
            this.txtPwd1.Name = "txtPwd1";
            this.txtPwd1.PasswordChar = '•';
            this.txtPwd1.ScrollBars = ScrollBars.None;
            this.txtPwd1.SelectionLength = 0;
            this.txtPwd1.SelectionStart = 0;
            this.txtPwd1.Size = new Size(183, 23);
            this.txtPwd1.TabIndex = 2;
            this.txtPwd1.TextAlign = HorizontalAlignment.Left;
            this.txtPwd1.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_ZipPwd1.AutoSize = true;
            this.lbl_ZipPwd1.Location = new Point(12, 8);
            this.lbl_ZipPwd1.Name = "lbl_ZipPwd1";
            this.lbl_ZipPwd1.Size = new Size(72, 13);
            this.lbl_ZipPwd1.TabIndex = 0;
            this.lbl_ZipPwd1.Text = "File Password";
            this.lbl_Folder.AutoSize = true;
            this.lbl_Folder.Location = new Point(20, 9);
            this.lbl_Folder.Name = "lbl_Folder";
            this.lbl_Folder.Size = new Size(36, 13);
            this.lbl_Folder.TabIndex = 7;
            this.lbl_Folder.Text = "Folder";
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Location = new Point(123, 9);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new Size(45, 13);
            this.lblFilePath.TabIndex = 6;
            this.lblFilePath.Text = "FilePath";
            this.btn_Cancel.AllowAnimations = true;
            this.btn_Cancel.BackColor = Color.Transparent;
            this.btn_Cancel.Location = new Point(235, 194);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.RoundedCornersMask = 15;
            this.btn_Cancel.RoundedCornersRadius = 0;
            this.btn_Cancel.Size = new Size(100, 30);
            this.btn_Cancel.TabIndex = 5;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Cancel.Click += new EventHandler(this.btn_Cancel_Click);
            this.btn_OK.AllowAnimations = true;
            this.btn_OK.BackColor = Color.Transparent;
            this.btn_OK.Location = new Point(128, 194);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.RoundedCornersMask = 15;
            this.btn_OK.RoundedCornersRadius = 0;
            this.btn_OK.Size = new Size(100, 30);
            this.btn_OK.TabIndex = 4;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = false;
            this.btn_OK.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_OK.Click += new EventHandler(this.btn_OK_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            base.ClientSize = new Size(360, 280);
            base.Controls.Add(this.FormPanel);
            base.Controls.Add(this.HeaderPanel);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "ZipDlg";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "ZipDlg";
            base.Load += new EventHandler(this.ZipDlg_Load);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            this.FormPanel.ResumeLayout(false);
            this.FormPanel.PerformLayout();
            this.ZipPanel.ResumeLayout(false);
            this.ZipPanel.PerformLayout();
            this.PwdPanel.ResumeLayout(false);
            this.PwdPanel.PerformLayout();
            base.ResumeLayout(false);
        }

        private void lbl_SecureFile_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void ZipDlg_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
            {
                this.HeaderPanel.BackgroundImage = Properties.Resources.topbar45;
            }
            LangCtrl.reText(this);
            this.lblFilePath.Text = this.FilePath;
            this.IsZipFile = false;
            this.IsPwd = false;
            this.ZipName = string.Empty;
            this.ZipPwd = string.Empty;
        }
    }
}