using AppGlobal;
using CDBurnCtrl;
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
    public class ExportCD : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;

        public const int HT_CAPTION = 2;

        private const int CS_DROPSHADOW = 131072;

        private CDCtrl cd = new CDCtrl();

        private IContainer components;

        private Panel HeaderPanel;

        private vButton btnClose;

        private Panel BodyPanel;

        private PictureBox pictureBox1;

        private Label lbl_BurnDisc;

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

        public string RootFolder
        {
            get;
            set;
        }

        public ExportCD()
        {
            this.InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void cd_EVT_CDAction(bool IsRecording)
        {
            this.btnClose.Enabled = !IsRecording;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ExportCD_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.cd.EVT_CDAction -= new CDCtrl.DEL_CDAction(this.cd_EVT_CDAction);
        }

        private void ExportCD_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
            {
                this.btnClose.VIBlendTheme = VIBLEND_THEME.NERO;
                this.HeaderPanel.BackgroundImage = Properties.Resources.topbar45;
            }
            this.lbl_BurnDisc.Text = LangCtrl.GetString("lbl_BurnDisc", "EXPORT TO DISC");
            this.cd.EVT_CDAction -= new CDCtrl.DEL_CDAction(this.cd_EVT_CDAction);
            this.cd.EVT_CDAction += new CDCtrl.DEL_CDAction(this.cd_EVT_CDAction);
            this.cd.Dock = DockStyle.Fill;
            this.BodyPanel.Controls.Add(this.cd);
            this.cd.AddFolders(this.RootFolder);
        }

        private void HeaderMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ExportCD.ReleaseCapture();
                ExportCD.SendMessage(base.Handle, 161, 2, 0);
            }
        }

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void InitializeComponent()
        {
            this.BodyPanel = new Panel();
            this.HeaderPanel = new Panel();
            this.lbl_BurnDisc = new Label();
            this.pictureBox1 = new PictureBox();
            this.btnClose = new vButton();
            this.HeaderPanel.SuspendLayout();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            base.SuspendLayout();
            this.BodyPanel.BorderStyle = BorderStyle.FixedSingle;
            this.BodyPanel.Dock = DockStyle.Fill;
            this.BodyPanel.Location = new Point(0, 45);
            this.BodyPanel.Name = "BodyPanel";
            this.BodyPanel.Size = new Size(443, 189);
            this.BodyPanel.TabIndex = 1;
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.lbl_BurnDisc);
            this.HeaderPanel.Controls.Add(this.pictureBox1);
            this.HeaderPanel.Controls.Add(this.btnClose);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(443, 45);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.lbl_BurnDisc.AutoSize = true;
            this.lbl_BurnDisc.BackColor = Color.Transparent;
            this.lbl_BurnDisc.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_BurnDisc.ForeColor = Color.White;
            this.lbl_BurnDisc.Location = new Point(63, 4);
            this.lbl_BurnDisc.Name = "lbl_BurnDisc";
            this.lbl_BurnDisc.Size = new Size(133, 16);
            this.lbl_BurnDisc.TabIndex = 2;
            this.lbl_BurnDisc.Text = "EXPORT TO DISC";
            this.lbl_BurnDisc.MouseDown += new MouseEventHandler(this.lbl_BurnDisc_MouseDown);
            this.pictureBox1.BackColor = Color.Transparent;
            this.pictureBox1.Image = Properties.Resources.burndisc;
            this.pictureBox1.Location = new Point(4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(40, 40);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new MouseEventHandler(this.pictureBox1_MouseDown);
            this.btnClose.AllowAnimations = true;
            this.btnClose.BackColor = Color.Transparent;
            this.btnClose.Dock = DockStyle.Right;
            this.btnClose.Image = Properties.Resources.close;
            this.btnClose.Location = new Point(398, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaintBorder = false;
            this.btnClose.PaintDefaultBorder = false;
            this.btnClose.PaintDefaultFill = false;
            this.btnClose.RoundedCornersMask = 15;
            this.btnClose.RoundedCornersRadius = 0;
            this.btnClose.Size = new Size(45, 45);
            this.btnClose.TabIndex = 0;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(443, 234);
            base.Controls.Add(this.BodyPanel);
            base.Controls.Add(this.HeaderPanel);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "ExportCD";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "ExportCD";
            base.FormClosing += new FormClosingEventHandler(this.ExportCD_FormClosing);
            base.Load += new EventHandler(this.ExportCD_Load);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((ISupportInitialize)this.pictureBox1).EndInit();
            base.ResumeLayout(false);
        }

        private void lbl_BurnDisc_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    }
}