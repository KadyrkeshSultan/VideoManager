using VideoManager.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMModels.Model;
using VMInterfaces;

namespace VideoManager
{
    public class RegDB : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;

        public const int HT_CAPTION = 2;

        private const int CS_DROPSHADOW = 131072;

        private string connectionString = string.Empty;

        private IContainer components;

        private Panel HeaderPanel;

        private PictureBox pictureBox1;

        private Label label1;

        private GroupBox groupBox2;

        private vCheckBox chk_LocalDB;

        private Panel DBPanel;

        private vCheckBox chk_Security;

        private vTextBox Catalog;

        private Label label3;

        private vTextBox Pwd2;

        private vTextBox UserID;

        private Label label5;

        private Label label11;

        private Label label12;

        private Label label4;

        private vTextBox Pwd1;

        private vTextBox DataSource;

        private vButton btn_Save;

        private vButton btn_Test;

        private vButton btnClose;

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

        public RegDB()
        {
            this.InitializeComponent();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (!this.Pwd1.Text.Equals(this.Pwd2.Text))
            {
                MessageBox.Show(this, "Passwords do not match.", "Password", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                DBProfileData dBProfileDatum = new DBProfileData()
                {
                    IsLocalDB = this.chk_LocalDB.Checked,
                    Catalog = this.Catalog.Text,
                    DataSource = this.DataSource.Text,
                    Password = this.Pwd1.Text,
                    PersistSecurityInfo = this.chk_Security.Checked,
                    UserId = this.UserID.Text
                };
                string str = Path.Combine(VMGlobal.ProfilePath, VMGlobal.ProfileFile);
                if (!Directory.Exists(VMGlobal.ProfilePath))
                {
                    Directory.CreateDirectory(VMGlobal.ProfilePath);
                    Network.SetAcl(VMGlobal.ProfilePath);
                }
                if (Directory.Exists(VMGlobal.ProfilePath))
                {
                    FileCrypto.Save(dBProfileDatum, str);
                    if (File.Exists(str))
                    {
                        MessageBox.Show(this, "C3 Sentinel connection profile saved.", "Profile", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        base.DialogResult = DialogResult.OK;
                        base.Close();
                        return;
                    }
                }
            }
        }

        private void btn_Test_Click(object sender, EventArgs e)
        {
            if (this.TestConnection())
            {
                MessageBox.Show(this, "Data connection successful.", "Database", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            MessageBox.Show(this, "Data connection failed.", "Database", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void chk_LocalDB_CheckedChanged(object sender, EventArgs e)
        {
            this.DBPanel.Enabled = !this.chk_LocalDB.Checked;
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
                RegDB.ReleaseCapture();
                RegDB.SendMessage(base.Handle, 161, 2, 0);
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
            this.label1 = new Label();
            this.groupBox2 = new GroupBox();
            this.chk_LocalDB = new vCheckBox();
            this.DBPanel = new Panel();
            this.chk_Security = new vCheckBox();
            this.Catalog = new vTextBox();
            this.label3 = new Label();
            this.Pwd2 = new vTextBox();
            this.UserID = new vTextBox();
            this.label5 = new Label();
            this.label11 = new Label();
            this.label12 = new Label();
            this.label4 = new Label();
            this.Pwd1 = new vTextBox();
            this.DataSource = new vTextBox();
            this.btn_Save = new vButton();
            this.btn_Test = new vButton();
            this.btnClose = new vButton();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            this.HeaderPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.DBPanel.SuspendLayout();
            base.SuspendLayout();
            this.pictureBox1.BackColor = Color.Transparent;
            this.pictureBox1.Image = Properties.Resources.Database;
            this.pictureBox1.Location = new Point(3, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(50, 50);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new MouseEventHandler(this.pictureBox1_MouseDown);
            this.HeaderPanel.BackgroundImage = Properties.Resources.header;
            this.HeaderPanel.Controls.Add(this.btnClose);
            this.HeaderPanel.Controls.Add(this.label1);
            this.HeaderPanel.Controls.Add(this.pictureBox1);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(372, 54);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.label1.AutoSize = true;
            this.label1.BackColor = Color.Transparent;
            this.label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.ForeColor = Color.White;
            this.label1.Location = new Point(60, 4);
            this.label1.Name = "label1";
            this.label1.Size = new Size(189, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "C3 Sentinel Database Connection";
            this.label1.MouseDown += new MouseEventHandler(this.label1_MouseDown);
            this.groupBox2.Controls.Add(this.chk_LocalDB);
            this.groupBox2.Controls.Add(this.DBPanel);
            this.groupBox2.Location = new Point(9, 62);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(355, 228);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Database";
            this.chk_LocalDB.BackColor = Color.Transparent;
            this.chk_LocalDB.Location = new Point(128, 11);
            this.chk_LocalDB.Name = "chk_LocalDB";
            this.chk_LocalDB.Size = new Size(205, 24);
            this.chk_LocalDB.TabIndex = 0;
            this.chk_LocalDB.Text = "Local Database";
            this.chk_LocalDB.UseVisualStyleBackColor = false;
            this.chk_LocalDB.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.chk_LocalDB.CheckedChanged += new EventHandler(this.chk_LocalDB_CheckedChanged);
            this.DBPanel.Controls.Add(this.chk_Security);
            this.DBPanel.Controls.Add(this.Catalog);
            this.DBPanel.Controls.Add(this.label3);
            this.DBPanel.Controls.Add(this.Pwd2);
            this.DBPanel.Controls.Add(this.UserID);
            this.DBPanel.Controls.Add(this.label5);
            this.DBPanel.Controls.Add(this.label11);
            this.DBPanel.Controls.Add(this.label12);
            this.DBPanel.Controls.Add(this.label4);
            this.DBPanel.Controls.Add(this.Pwd1);
            this.DBPanel.Controls.Add(this.DataSource);
            this.DBPanel.Location = new Point(5, 39);
            this.DBPanel.Name = "DBPanel";
            this.DBPanel.Size = new Size(345, 184);
            this.DBPanel.TabIndex = 0;
            this.chk_Security.BackColor = Color.Transparent;
            this.chk_Security.Checked = true;
            this.chk_Security.CheckState = CheckState.Checked;
            this.chk_Security.Location = new Point(124, 151);
            this.chk_Security.Name = "chk_Security";
            this.chk_Security.Size = new Size(205, 24);
            this.chk_Security.TabIndex = 9;
            this.chk_Security.Text = "Persist Security Information";
            this.chk_Security.UseVisualStyleBackColor = false;
            this.chk_Security.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.Catalog.BackColor = Color.White;
            this.Catalog.BoundsOffset = new Size(1, 1);
            this.Catalog.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.Catalog.DefaultText = "";
            this.Catalog.Location = new Point(124, 34);
            this.Catalog.MaxLength = 128;
            this.Catalog.Name = "Catalog";
            this.Catalog.PasswordChar = '\0';
            this.Catalog.ScrollBars = ScrollBars.None;
            this.Catalog.SelectionLength = 0;
            this.Catalog.SelectionStart = 0;
            this.Catalog.Size = new Size(205, 23);
            this.Catalog.TabIndex = 3;
            this.Catalog.TextAlign = HorizontalAlignment.Left;
            this.Catalog.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(3, 10);
            this.label3.Name = "label3";
            this.label3.Size = new Size(87, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Database Server";
            this.Pwd2.BackColor = Color.White;
            this.Pwd2.BoundsOffset = new Size(1, 1);
            this.Pwd2.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.Pwd2.DefaultText = "";
            this.Pwd2.Location = new Point(124, 121);
            this.Pwd2.MaxLength = 128;
            this.Pwd2.Name = "Pwd2";
            this.Pwd2.PasswordChar = '•';
            this.Pwd2.ScrollBars = ScrollBars.None;
            this.Pwd2.SelectionLength = 0;
            this.Pwd2.SelectionStart = 0;
            this.Pwd2.Size = new Size(205, 23);
            this.Pwd2.TabIndex = 8;
            this.Pwd2.TextAlign = HorizontalAlignment.Left;
            this.Pwd2.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.UserID.BackColor = Color.White;
            this.UserID.BoundsOffset = new Size(1, 1);
            this.UserID.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.UserID.DefaultText = "";
            this.UserID.Location = new Point(124, 63);
            this.UserID.MaxLength = 128;
            this.UserID.Name = "UserID";
            this.UserID.PasswordChar = '\0';
            this.UserID.ScrollBars = ScrollBars.None;
            this.UserID.SelectionLength = 0;
            this.UserID.SelectionStart = 0;
            this.UserID.Size = new Size(205, 23);
            this.UserID.TabIndex = 4;
            this.UserID.TextAlign = HorizontalAlignment.Left;
            this.UserID.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(3, 68);
            this.label5.Name = "label5";
            this.label5.Size = new Size(64, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Login Name";
            this.label11.AutoSize = true;
            this.label11.Location = new Point(3, 97);
            this.label11.Name = "label11";
            this.label11.Size = new Size(53, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "Password";
            this.label12.AutoSize = true;
            this.label12.Location = new Point(3, 126);
            this.label12.Name = "label12";
            this.label12.Size = new Size(90, 13);
            this.label12.TabIndex = 7;
            this.label12.Text = "Retype Password";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(3, 39);
            this.label4.Name = "label4";
            this.label4.Size = new Size(84, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Database Name";
            this.Pwd1.BackColor = Color.White;
            this.Pwd1.BoundsOffset = new Size(1, 1);
            this.Pwd1.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.Pwd1.DefaultText = "";
            this.Pwd1.Location = new Point(124, 92);
            this.Pwd1.MaxLength = 128;
            this.Pwd1.Name = "Pwd1";
            this.Pwd1.PasswordChar = '•';
            this.Pwd1.ScrollBars = ScrollBars.None;
            this.Pwd1.SelectionLength = 0;
            this.Pwd1.SelectionStart = 0;
            this.Pwd1.Size = new Size(205, 23);
            this.Pwd1.TabIndex = 6;
            this.Pwd1.TextAlign = HorizontalAlignment.Left;
            this.Pwd1.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.DataSource.BackColor = Color.White;
            this.DataSource.BoundsOffset = new Size(1, 1);
            this.DataSource.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.DataSource.DefaultText = "";
            this.DataSource.Location = new Point(124, 5);
            this.DataSource.MaxLength = 128;
            this.DataSource.Name = "DataSource";
            this.DataSource.PasswordChar = '\0';
            this.DataSource.ScrollBars = ScrollBars.None;
            this.DataSource.SelectionLength = 0;
            this.DataSource.SelectionStart = 0;
            this.DataSource.Size = new Size(205, 23);
            this.DataSource.TabIndex = 1;
            this.DataSource.TextAlign = HorizontalAlignment.Left;
            this.DataSource.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Save.AllowAnimations = true;
            this.btn_Save.BackColor = Color.Transparent;
            this.btn_Save.Location = new Point(259, 296);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.RoundedCornersMask = 15;
            this.btn_Save.Size = new Size(100, 30);
            this.btn_Save.TabIndex = 7;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = false;
            this.btn_Save.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Save.Click += new EventHandler(this.btn_Save_Click);
            this.btn_Test.AllowAnimations = true;
            this.btn_Test.BackColor = Color.Transparent;
            this.btn_Test.Location = new Point(14, 296);
            this.btn_Test.Name = "btn_Test";
            this.btn_Test.RoundedCornersMask = 15;
            this.btn_Test.Size = new Size(147, 30);
            this.btn_Test.TabIndex = 8;
            this.btn_Test.Text = "Test Connection";
            this.btn_Test.UseVisualStyleBackColor = false;
            this.btn_Test.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Test.Click += new EventHandler(this.btn_Test_Click);
            this.btnClose.AllowAnimations = true;
            this.btnClose.BackColor = Color.Transparent;
            this.btnClose.Image = Properties.Resources.close;
            this.btnClose.Location = new Point(344, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaintBorder = false;
            this.btnClose.PaintDefaultBorder = false;
            this.btnClose.PaintDefaultFill = false;
            this.btnClose.RoundedCornersMask = 15;
            this.btnClose.RoundedCornersRadius = 0;
            this.btnClose.Size = new Size(28, 28);
            this.btnClose.TabIndex = 3;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(372, 335);
            base.Controls.Add(this.btn_Test);
            base.Controls.Add(this.btn_Save);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.HeaderPanel);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "RegDB";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "RegDB";
            base.Load += new EventHandler(this.RegDB_Load);
            base.Paint += new PaintEventHandler(this.RegDB_Paint);
            ((ISupportInitialize)this.pictureBox1).EndInit();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.DBPanel.ResumeLayout(false);
            this.DBPanel.PerformLayout();
            base.ResumeLayout(false);
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        public void LoadProfile()
        {
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void RegDB_Load(object sender, EventArgs e)
        {
            string str = Path.Combine(VMGlobal.ProfilePath, VMGlobal.ProfileFile);
            if (Directory.Exists(VMGlobal.ProfilePath))
            {
                try
                {
                    DBProfileData dBProfileDatum = (DBProfileData)FileCrypto.LoadConfig(str);
                    this.chk_LocalDB.Checked = dBProfileDatum.IsLocalDB;
                    this.chk_Security.Checked = dBProfileDatum.PersistSecurityInfo;
                    this.DataSource.Text = dBProfileDatum.DataSource;
                    this.Catalog.Text = dBProfileDatum.Catalog;
                    this.UserID.Text = dBProfileDatum.UserId;
                    vTextBox pwd1 = this.Pwd1;
                    vTextBox pwd2 = this.Pwd2;
                    string password = dBProfileDatum.Password;
                    string str1 = password;
                    pwd2.Text = password;
                    pwd1.Text = str1;
                }
                catch
                {
                }
            }
        }

        private void RegDB_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder3D(e.Graphics, ((Control)sender).ClientRectangle, Border3DStyle.RaisedOuter);
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private bool TestConnection()
        {
            bool flag = false;
            this.Cursor = Cursors.WaitCursor;
            if (!this.chk_LocalDB.Checked)
            {
                object[] text = new object[] { this.DataSource.Text, this.Catalog.Text, this.chk_Security.Checked, this.UserID.Text, this.Pwd1.Text };
                this.connectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info={2};User ID={3};Password={4}", text);
            }
            else
            {
                this.connectionString = "C3Sentinel";
            }
            VMGlobal.SetTestConnection(this.connectionString);
            try
            {
                using (RPM_Account rPMAccount = new RPM_Account())
                {
                    rPMAccount.AccountCount();
                    flag = true;
                }
            }
            catch (Exception exception)
            {
            }
            this.Cursor = Cursors.Default;
            return flag;
        }
    }
}