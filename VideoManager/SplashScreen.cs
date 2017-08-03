using AppGlobal;
using VideoManager.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;

namespace VideoManager
{
    public class SplashScreen : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;

        public const int HT_CAPTION = 2;

        private const int CS_DROPSHADOW = 131072;

        private IContainer components;

        private Panel FormPanel;

        private PictureBox SplashLogo;

        private Label lbl_Version;

        private vProgressBar progBar;

        private PictureBox PromoPic;

        private Label lbl_Copyright;

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

        public SplashScreen()
        {
            this.InitializeComponent();
        }

        public void CloseForm()
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

        private void FormPanel_Paint(object sender, PaintEventArgs e)
        {
        }

        private void InitializeComponent()
        {
            this.FormPanel = new Panel();
            this.PromoPic = new PictureBox();
            this.lbl_Copyright = new Label();
            this.progBar = new vProgressBar();
            this.lbl_Version = new Label();
            this.SplashLogo = new PictureBox();
            this.FormPanel.SuspendLayout();
            ((ISupportInitialize)this.PromoPic).BeginInit();
            ((ISupportInitialize)this.SplashLogo).BeginInit();
            base.SuspendLayout();
            this.FormPanel.BackgroundImage = Properties.Resources.splash;
            this.FormPanel.BorderStyle = BorderStyle.FixedSingle;
            this.FormPanel.Controls.Add(this.PromoPic);
            this.FormPanel.Controls.Add(this.lbl_Copyright);
            this.FormPanel.Controls.Add(this.progBar);
            this.FormPanel.Controls.Add(this.lbl_Version);
            this.FormPanel.Controls.Add(this.SplashLogo);
            this.FormPanel.Dock = DockStyle.Fill;
            this.FormPanel.Location = new Point(0, 0);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new Size(380, 306);
            this.FormPanel.TabIndex = 0;
            this.FormPanel.Paint += new PaintEventHandler(this.FormPanel_Paint);
            this.PromoPic.BackColor = Color.Transparent;
            this.PromoPic.Dock = DockStyle.Bottom;
            this.PromoPic.Image = Properties.Resources.CiteCam;
            this.PromoPic.Location = new Point(0, 199);
            this.PromoPic.Name = "PromoPic";
            this.PromoPic.Size = new Size(378, 105);
            this.PromoPic.SizeMode = PictureBoxSizeMode.StretchImage;
            this.PromoPic.TabIndex = 10;
            this.PromoPic.TabStop = false;
            this.lbl_Copyright.AutoSize = true;
            this.lbl_Copyright.BackColor = Color.Transparent;
            this.lbl_Copyright.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lbl_Copyright.Location = new Point(184, 86);
            this.lbl_Copyright.Name = "lbl_Copyright";
            this.lbl_Copyright.Size = new Size(124, 12);
            this.lbl_Copyright.TabIndex = 9;
            this.lbl_Copyright.Text = "Copyright ©2015 HD Protech";
            this.progBar.BackColor = Color.Transparent;
            this.progBar.Location = new Point(11, 160);
            this.progBar.Name = "progBar";
            this.progBar.RoundedCornersMask = 15;
            this.progBar.RoundedCornersRadius = 0;
            this.progBar.Size = new Size(349, 15);
            this.progBar.TabIndex = 7;
            this.progBar.Value = 10;
            this.progBar.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.lbl_Version.AutoSize = true;
            this.lbl_Version.BackColor = Color.Transparent;
            this.lbl_Version.Location = new Point(184, 69);
            this.lbl_Version.Name = "lbl_Version";
            this.lbl_Version.Size = new Size(78, 13);
            this.lbl_Version.TabIndex = 5;
            this.lbl_Version.Text = "Version 5.0.0.8";
            this.SplashLogo.BackColor = Color.Transparent;
            this.SplashLogo.Image = Properties.Resources.About;
            this.SplashLogo.Location = new Point(11, 11);
            this.SplashLogo.Name = "SplashLogo";
            this.SplashLogo.Size = new Size(349, 269);
            this.SplashLogo.TabIndex = 4;
            this.SplashLogo.TabStop = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(380, 306);
            base.Controls.Add(this.FormPanel);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "SplashScreen";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "SplashScreen";
            base.TopMost = true;
            base.FormClosing += new FormClosingEventHandler(this.SplashScreen_FormClosing);
            base.Load += new EventHandler(this.SplashScreen_Load);
            this.FormPanel.ResumeLayout(false);
            this.FormPanel.PerformLayout();
            ((ISupportInitialize)this.PromoPic).EndInit();
            ((ISupportInitialize)this.SplashLogo).EndInit();
            base.ResumeLayout(false);
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool ReleaseCapture();

        private void Run()
        {
            for (int i = 10; i < 100; i++)
            {
                this.progBar.Value = i;
                Thread.Sleep(75);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void SplashScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (double i = 1; i > 0.1; i -= 0.1)
            {
                base.Opacity = i;
                Thread.Sleep(125);
            }
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            string directoryName = Path.GetDirectoryName(Application.ExecutablePath);
            string str = Path.Combine(directoryName, "promo.png");
            try
            {
                if (File.Exists(str))
                {
                    this.PromoPic.Image = Image.FromFile(str);
                }
                else if (Global.IS_WOLFCOM)
                {
                    this.SplashLogo.Image = Properties.Resources.wc_about;
                    this.lbl_Version.Location = new Point(236, 130);
                    this.lbl_Copyright.Location = new Point(236, 147);
                    this.progBar.VIBlendTheme = VIBLEND_THEME.NERO;
                    this.progBar.Location = new Point(11, 250);
                    this.PromoPic.Visible = false;
                    this.FormPanel.BackgroundImage = Properties.Resources.WCBG;
                    this.lbl_Version.ForeColor = Color.White;
                    this.lbl_Copyright.ForeColor = Color.White;
                }
            }
            catch (Exception exception)
            {
                this.PromoPic.Image = null;
            }
            this.lbl_Version.Text = string.Format("Version {0}", Application.ProductVersion);
            (new Thread(new ThreadStart(this.Run))).Start();
        }
    }
}