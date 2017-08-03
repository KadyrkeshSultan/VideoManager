using AppGlobal;
using VideoManager.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Management;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;

namespace VideoManager
{
    public class About : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;

        public const int HT_CAPTION = 2;

        private const int CS_DROPSHADOW = 131072;

        private IContainer components;

        private vButton btnCloseDlg;

        private Label lbl_Version;

        private PictureBox LogoPic;

        private Label lblHardware;

        private Panel HeaderPanel;

        private PictureBox pictureBox2;

        private Label lbl_About;

        private LinkLabel lnk_License;

        private Panel AboutPanel;

        private Panel MainPanel;

        private Panel LogoPanel;

        private PictureBox BrandPic;

        private Label Copyright;

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

        public About()
        {
            this.InitializeComponent();
        }

        private void About_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void About_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
            {
                this.LogoPic.Image = Properties.Resources.sm_wolfcom11;
                this.BrandPic.Visible = false;
                this.LogoPanel.Visible = true;
                this.lblHardware.ForeColor = Color.White;
                this.MainPanel.BackgroundImage = Properties.Resources.WCBG;
                this.btnCloseDlg.VIBlendTheme = VIBLEND_THEME.NERO;
                this.lnk_License.LinkColor = Color.Yellow;
                this.lbl_Version.ForeColor = Color.White;
                this.Copyright.ForeColor = Color.White;
                this.lbl_Version.Location = new Point(270, 6);
                this.lnk_License.Location = new Point(270, 33);
                this.HeaderPanel.BackgroundImage = Properties.Resources.topbar45;
            }
            this.GetMachineProfile();
            this.SetLanguage();
            this.lbl_Version.Text = string.Format(LangCtrl.GetString("lbl_Version", "Version {0}"), Application.ProductVersion);
        }

        private void About_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder3D(e.Graphics, ((Control)sender).ClientRectangle, Border3DStyle.RaisedOuter);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GetGraphicsCard()
        {
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DisplayConfiguration");
            string empty = string.Empty;
            foreach (ManagementObject managementObject in managementObjectSearcher.Get())
            {
                foreach (PropertyData property in managementObject.Properties)
                {
                    if (property.Value == null || !property.Name.Equals("Caption"))
                    {
                        continue;
                    }
                    empty = string.Concat(empty, string.Format("{0}", property.Value.ToString()));
                }
            }
            return empty;
        }

        private void GetMachineProfile()
        {
            string empty = string.Empty;
            using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = (new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem")).Get().GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    empty = ((ManagementObject)enumerator.Current)["Caption"].ToString();
                }
            }
            this.lblHardware.Text = string.Concat(empty, "\n");
            ManagementClass managementClass = new ManagementClass("Win32_ComputerSystem");
            if (managementClass.GetInstances().Count != 0)
            {
                foreach (ManagementObject instance in managementClass.GetInstances())
                {
                    Label label = this.lblHardware;
                    label.Text = string.Concat(label.Text, string.Format("{0} {1}\n", instance["Manufacturer"].ToString(), instance["Model"].ToString()));
                }
            }
            foreach (ManagementObject managementObject in (new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor")).Get())
            {
                this.lblHardware.Text = string.Concat(managementObject["Name"].ToString(), "\n");
            }
            Label label1 = this.lblHardware;
            label1.Text = string.Concat(label1.Text, this.method_0(), "\n");
            Label label2 = this.lblHardware;
            label2.Text = string.Concat(label2.Text, this.GetGraphicsCard(), "\n\n");
            Label label3 = this.lblHardware;
            label3.Text = string.Concat(label3.Text, string.Format(LangCtrl.GetString("lbl_MachineID", "Machine ID {0}\nIP {1}"), Global.MachineID, Global.IPAddress));
            Label label4 = this.lblHardware;
            label4.Text = string.Concat(label4.Text, "\n\n");
            Label label5 = this.lblHardware;
            label5.Text = string.Concat(label5.Text, string.Format(string.Concat(LangCtrl.GetString("lbl_MachineName", "Machine Name: {0}"), "\n"), Environment.MachineName));
            Label label6 = this.lblHardware;
            label6.Text = string.Concat(label6.Text, string.Format(string.Concat(LangCtrl.GetString("lbl_UserDomain", "Domain Name: {0}"), "\n"), Environment.UserDomainName));
            Label label7 = this.lblHardware;
            label7.Text = string.Concat(label7.Text, string.Format(LangCtrl.GetString("lbl_MachineAccount", "Machine Account: {0}"), Environment.UserName));
        }

        private void HeaderMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                About.ReleaseCapture();
                About.SendMessage(base.Handle, 161, 2, 0);
            }
        }

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(About));
            this.lbl_Version = new Label();
            this.lblHardware = new Label();
            this.lnk_License = new LinkLabel();
            this.AboutPanel = new Panel();
            this.MainPanel = new Panel();
            this.Copyright = new Label();
            this.LogoPic = new PictureBox();
            this.LogoPanel = new Panel();
            this.BrandPic = new PictureBox();
            this.HeaderPanel = new Panel();
            this.lbl_About = new Label();
            this.pictureBox2 = new PictureBox();
            this.btnCloseDlg = new vButton();
            this.AboutPanel.SuspendLayout();
            this.MainPanel.SuspendLayout();
            ((ISupportInitialize)this.LogoPic).BeginInit();
            ((ISupportInitialize)this.BrandPic).BeginInit();
            this.HeaderPanel.SuspendLayout();
            ((ISupportInitialize)this.pictureBox2).BeginInit();
            base.SuspendLayout();
            this.lbl_Version.AutoSize = true;
            this.lbl_Version.BackColor = Color.Transparent;
            this.lbl_Version.Font = new Font("Verdana", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_Version.ForeColor = Color.Black;
            this.lbl_Version.Location = new Point(356, 6);
            this.lbl_Version.Name = "lbl_Version";
            this.lbl_Version.Size = new Size(141, 18);
            this.lbl_Version.TabIndex = 3;
            this.lbl_Version.Text = "Version 5.0.5.1";
            this.lblHardware.BackColor = Color.Transparent;
            this.lblHardware.Location = new Point(270, 58);
            this.lblHardware.Name = "lblHardware";
            this.lblHardware.Size = new Size(317, 153);
            this.lblHardware.TabIndex = 5;
            this.lnk_License.AutoSize = true;
            this.lnk_License.BackColor = Color.Transparent;
            this.lnk_License.Location = new Point(356, 33);
            this.lnk_License.Name = "lnk_License";
            this.lnk_License.Size = new Size(101, 13);
            this.lnk_License.TabIndex = 9;
            this.lnk_License.TabStop = true;
            this.lnk_License.Text = "C3 Sentinel License";
            this.lnk_License.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnk_License_LinkClicked);
            this.AboutPanel.BorderStyle = BorderStyle.FixedSingle;
            this.AboutPanel.Controls.Add(this.MainPanel);
            this.AboutPanel.Dock = DockStyle.Fill;
            this.AboutPanel.Location = new Point(0, 45);
            this.AboutPanel.Name = "AboutPanel";
            this.AboutPanel.Size = new Size(593, 240);
            this.AboutPanel.TabIndex = 11;
            this.MainPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.MainPanel.Controls.Add(this.Copyright);
            this.MainPanel.Controls.Add(this.lblHardware);
            this.MainPanel.Controls.Add(this.LogoPic);
            this.MainPanel.Controls.Add(this.LogoPanel);
            this.MainPanel.Controls.Add(this.lbl_Version);
            this.MainPanel.Controls.Add(this.lnk_License);
            this.MainPanel.Controls.Add(this.BrandPic);
            this.MainPanel.Dock = DockStyle.Fill;
            this.MainPanel.Location = new Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new Size(591, 238);
            this.MainPanel.TabIndex = 11;
            this.MainPanel.Paint += new PaintEventHandler(this.MainPanel_Paint);
            this.Copyright.AutoSize = true;
            this.Copyright.BackColor = Color.Transparent;
            this.Copyright.Font = new Font("Times New Roman", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Copyright.Location = new Point(272, 216);
            this.Copyright.Name = "Copyright";
            this.Copyright.Size = new Size(89, 14);
            this.Copyright.TabIndex = 12;
            this.Copyright.Text = "Copyright ©2015";
            this.LogoPic.BackColor = Color.Transparent;
            this.LogoPic.Cursor = Cursors.Hand;
            this.LogoPic.Image = Properties.Resources.logo;
            this.LogoPic.Location = new Point(460, 212);
            this.LogoPic.Name = "LogoPic";
            this.LogoPic.Size = new Size(127, 22);
            this.LogoPic.SizeMode = PictureBoxSizeMode.StretchImage;
            this.LogoPic.TabIndex = 4;
            this.LogoPic.TabStop = false;
            this.LogoPic.Click += new EventHandler(this.LogoPic_Click);
            this.LogoPanel.BackColor = Color.Transparent;
            this.LogoPanel.BackgroundImage = Properties.Resources.wc_about;
            this.LogoPanel.BackgroundImageLayout = ImageLayout.None;
            this.LogoPanel.Location = new Point(2, 2);
            this.LogoPanel.Name = "LogoPanel";
            this.LogoPanel.Size = new Size(262, 227);
            this.LogoPanel.TabIndex = 10;
            this.LogoPanel.Visible = false;
            this.BrandPic.Image = Properties.Resources.About;
            this.BrandPic.Location = new Point(4, 6);
            this.BrandPic.Name = "BrandPic";
            this.BrandPic.Size = new Size(364, 141);
            this.BrandPic.TabIndex = 11;
            this.BrandPic.TabStop = false;
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.lbl_About);
            this.HeaderPanel.Controls.Add(this.pictureBox2);
            this.HeaderPanel.Controls.Add(this.btnCloseDlg);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(593, 45);
            this.HeaderPanel.TabIndex = 8;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.lbl_About.AutoSize = true;
            this.lbl_About.BackColor = Color.Transparent;
            this.lbl_About.Font = new Font("Verdana", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_About.ForeColor = Color.White;
            this.lbl_About.Location = new Point(45, 8);
            this.lbl_About.Name = "lbl_About";
            this.lbl_About.Size = new Size(197, 16);
            this.lbl_About.TabIndex = 4;
            this.lbl_About.Text = "About C3 Sentinel Catalog";
            this.lbl_About.MouseDown += new MouseEventHandler(this.lbl_About_MouseDown);
            this.pictureBox2.BackColor = Color.Transparent;
            this.pictureBox2.Image = Properties.Resources.catalog;
            this.pictureBox2.Location = new Point(3, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new Size(47, 38);
            this.pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.MouseDown += new MouseEventHandler(this.pictureBox2_MouseDown);
            this.btnCloseDlg.AllowAnimations = true;
            this.btnCloseDlg.BackColor = Color.Transparent;
            this.btnCloseDlg.DialogResult = DialogResult.Cancel;
            this.btnCloseDlg.Dock = DockStyle.Right;
            this.btnCloseDlg.Image = Properties.Resources.close;
            this.btnCloseDlg.Location = new Point(545, 0);
            this.btnCloseDlg.Name = "btnCloseDlg";
            this.btnCloseDlg.PaintBorder = false;
            this.btnCloseDlg.PaintDefaultBorder = false;
            this.btnCloseDlg.PaintDefaultFill = false;
            this.btnCloseDlg.RoundedCornersMask = 15;
            this.btnCloseDlg.RoundedCornersRadius = 0;
            this.btnCloseDlg.Size = new Size(48, 45);
            this.btnCloseDlg.TabIndex = 2;
            this.btnCloseDlg.UseVisualStyleBackColor = false;
            this.btnCloseDlg.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.CancelButton = this.btnCloseDlg;
            base.ClientSize = new Size(593, 285);
            base.Controls.Add(this.AboutPanel);
            base.Controls.Add(this.HeaderPanel);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Icon = (Icon)Resources.About.AboutIcon;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "About";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "About";
            base.FormClosing += new FormClosingEventHandler(this.About_FormClosing);
            base.Load += new EventHandler(this.About_Load);
            base.Paint += new PaintEventHandler(this.About_Paint);
            this.AboutPanel.ResumeLayout(false);
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            ((ISupportInitialize)this.LogoPic).EndInit();
            ((ISupportInitialize)this.BrandPic).EndInit();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((ISupportInitialize)this.pictureBox2).EndInit();
            base.ResumeLayout(false);
        }

        private void lbl_About_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void lnk_License_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            (new License()).ShowDialog(this);
        }

        private void LoadSettings()
        {
        }

        private void LogoPic_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start((Global.IS_WOLFCOM ? "http://www.wolfcomusa.com" : "http://www.hdprotech.com"));
            }
            catch
            {
            }
        }

        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {
        }

        private string method_0()
        {
            string str;
            using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = (new ManagementClass("Win32_ComputerSystem")).GetInstances().GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    ManagementObject current = (ManagementObject)enumerator.Current;
                    str = string.Concat(Convert.ToString(Math.Round(Convert.ToDouble(current.Properties["TotalPhysicalMemory"].Value) / 1073741824, 0)), ".0GB RAM");
                }
                else
                {
                    return "RAMsize";
                }
            }
            return str;
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
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