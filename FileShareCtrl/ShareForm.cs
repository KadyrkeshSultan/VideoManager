using AppGlobal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMInterfaces;
using VMModels.Model;

namespace FileShareCtrl
{
    public class ShareForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;
        public const int HT_CAPTION = 2;
        private const int CS_DROPSHADOW = 131072;
        private List<Guid> FileList;
        private Guid AccountID;
        private IContainer components;
        private Panel FormPanel;
        private Panel HeaderPanel;
        private PictureBox picShare;
        private vButton btn_Close;
        private Label lbl_ShareFiles;
        private vButton btn_OK;
        private Label lblAccount;
        private Label lblShareStatus;
        private vProgressBar progBar;
        private vRadioButton btnByValue;
        private vRadioButton btnByReference;

        protected override CreateParams CreateParams
        {
            
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ClassStyle |= 131072;
                return createParams;
            }
        }

        
        public ShareForm()
        {
            FileList = new List<Guid>();
            AccountID = Guid.Empty;
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        
        private void ShareForm_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
                HeaderPanel.BackgroundImage = (Image)System.Resources.topbar45;
            LangCtrl.reText(this);
            DisplayStats();
        }

        
        private void DisplayStats()
        {
            using (RPM_Account rpmAccount = new RPM_Account())
            {
                Account account = rpmAccount.GetAccount(AccountID);
                lblAccount.Text = string.Format(LangCtrl.GetString("ShareForm_1", "ShareForm_2"), account.ToString(), account.BadgeNumber);
                lblShareStatus.Text = string.Format(LangCtrl.GetString("ShareForm_3", "ShareForm_4"), FileList.Count);
            }
        }

        
        public void ShareFiles(List<Guid> filelist, Guid Account)
        {
            FileList = filelist;
            AccountID = Account;
        }

        
        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            HeaderMouseDown(e);
        }

        
        private void HeaderMouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            ReleaseCapture();
            SendMessage(Handle, 161, 2, 0);
        }

        
        private void picShare_MouseDown(object sender, MouseEventArgs e)
        {
            HeaderMouseDown(e);
        }

        
        private void lbl_ShareFiles_MouseDown(object sender, MouseEventArgs e)
        {
            HeaderMouseDown(e);
        }

        
        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        
        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (btnByValue.Checked)
                CopySelectedFiles();
            else
                ReferenceSelectedFiles();
        }

        
        private void ReferenceSelectedFiles()
        {
        }

        
        private void CopySelectedFiles()
        {
            btn_Close.Enabled = false;
            btn_OK.Enabled = false;
            progBar.Visible = true;
            progBar.Maximum = this.FileList.Count;
            progBar.Invalidate();
            Application.DoEvents();
            using (RPM_DataFile rpmDataFile = new RPM_DataFile())
            {
                int num = 1;
                foreach (Guid file in this.FileList)
                {
                    progBar.Value = num++;
                    progBar.Invalidate();
                    rpmDataFile.GetDataFile(file).AccountId = AccountID;
                }
            }
            Close();
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        
        private void InitializeComponent()
        {
            this.FormPanel = new Panel();
            this.btnByValue = new vRadioButton();
            this.btnByReference = new vRadioButton();
            this.progBar = new vProgressBar();
            this.lblAccount = new Label();
            this.lblShareStatus = new Label();
            this.btn_OK = new vButton();
            this.btn_Close = new vButton();
            this.HeaderPanel = new Panel();
            this.lbl_ShareFiles = new Label();
            this.picShare = new PictureBox();
            this.FormPanel.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((ISupportInitialize)this.picShare).BeginInit();
            this.SuspendLayout();
            this.FormPanel.BorderStyle = BorderStyle.FixedSingle;
            this.FormPanel.Controls.Add((Control)this.btnByValue);
            this.FormPanel.Controls.Add((Control)this.btnByReference);
            this.FormPanel.Controls.Add((Control)this.progBar);
            this.FormPanel.Controls.Add((Control)this.lblAccount);
            this.FormPanel.Controls.Add((Control)this.lblShareStatus);
            this.FormPanel.Controls.Add((Control)this.btn_OK);
            this.FormPanel.Controls.Add((Control)this.btn_Close);
            this.FormPanel.Controls.Add((Control)this.HeaderPanel);
            this.FormPanel.Dock = DockStyle.Fill;
            this.FormPanel.Location = new Point(0, 0);
            this.FormPanel.Name = "ShareForm_7";
            this.FormPanel.Size = new Size(368, 204);
            this.FormPanel.TabIndex = 0;
            this.btnByValue.BackColor = Color.Transparent;
            this.btnByValue.Flat = true;
            this.btnByValue.Image = (Image)null;
            this.btnByValue.Location = new Point(237, 94);
            this.btnByValue.Name = "ShareForm_8";
            this.btnByValue.Size = new Size(111, 24);
            this.btnByValue.TabIndex = 7;
            this.btnByValue.Text = "ShareForm_9";
            this.btnByValue.UseVisualStyleBackColor = false;
            this.btnByValue.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnByReference.BackColor = Color.Transparent;
            this.btnByReference.Checked = true;
            this.btnByReference.Flat = true;
            this.btnByReference.Image = (Image)null;
            this.btnByReference.Location = new Point(24, 94);
            this.btnByReference.Name = "ShareForm_10";
            this.btnByReference.Size = new Size(157, 24);
            this.btnByReference.TabIndex = 6;
            this.btnByReference.TabStop = true;
            this.btnByReference.Text = "ShareForm_11";
            this.btnByReference.UseVisualStyleBackColor = false;
            this.btnByReference.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.progBar.BackColor = Color.Transparent;
            this.progBar.Location = new Point(24, (int)sbyte.MaxValue);
            this.progBar.Name = "ShareForm_12";
            this.progBar.RoundedCornersMask = (byte)15;
            this.progBar.Size = new Size(324, 15);
            this.progBar.TabIndex = 5;
            this.progBar.Text = "ShareForm_13";
            this.progBar.Value = 0;
            this.progBar.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.progBar.Visible = false;
            this.lblAccount.AutoSize = true;
            this.lblAccount.Location = new Point(24, 72);
            this.lblAccount.Name = "ShareForm_14";
            this.lblAccount.Size = new Size(47, 13);
            this.lblAccount.TabIndex = 4;
            this.lblAccount.Text = "ShareForm_15";
            this.lblShareStatus.AutoSize = true;
            this.lblShareStatus.Location = new Point(24, 50);
            this.lblShareStatus.Name = "ShareForm_16";
            this.lblShareStatus.Size = new Size(56, 13);
            this.lblShareStatus.TabIndex = 3;
            this.lblShareStatus.Text = "ShareForm_17";
            this.btn_OK.AllowAnimations = true;
            this.btn_OK.BackColor = Color.Transparent;
            this.btn_OK.Location = new Point(142, 157);
            this.btn_OK.Name = "ShareForm_18";
            this.btn_OK.RoundedCornersMask = (byte)15;
            this.btn_OK.Size = new Size(100, 30);
            this.btn_OK.TabIndex = 2;
            this.btn_OK.Text = "ShareForm_19";
            this.btn_OK.UseVisualStyleBackColor = false;
            this.btn_OK.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_OK.Click += new EventHandler(this.btn_OK_Click);
            this.btn_Close.AllowAnimations = true;
            this.btn_Close.BackColor = Color.Transparent;
            this.btn_Close.Location = new Point(248, 157);
            this.btn_Close.Name = "ShareForm_20";
            this.btn_Close.RoundedCornersMask = (byte)15;
            this.btn_Close.Size = new Size(100, 30);
            this.btn_Close.TabIndex = 1;
            this.btn_Close.Text = "ShareForm_21";
            this.btn_Close.UseVisualStyleBackColor = false;
            this.btn_Close.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Close.Click += new EventHandler(this.btn_Close_Click);
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add((Control)this.lbl_ShareFiles);
            this.HeaderPanel.Controls.Add((Control)this.picShare);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "ShareForm_22";
            this.HeaderPanel.Size = new Size(366, 40);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.lbl_ShareFiles.AutoSize = true;
            this.lbl_ShareFiles.BackColor = Color.Transparent;
            this.lbl_ShareFiles.Font = new Font("ShareForm_23", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lbl_ShareFiles.ForeColor = Color.White;
            this.lbl_ShareFiles.Location = new Point(57, 8);
            this.lbl_ShareFiles.Name = "ShareForm_24";
            this.lbl_ShareFiles.Size = new Size(60, 16);
            this.lbl_ShareFiles.TabIndex = 1;
            this.lbl_ShareFiles.Text = "ShareForm_25";
            this.lbl_ShareFiles.MouseDown += new MouseEventHandler(this.lbl_ShareFiles_MouseDown);
            this.picShare.BackColor = Color.Transparent;
            this.picShare.Image = (Image)Resources.share;
            this.picShare.Location = new Point(6, 3);
            this.picShare.Name = "ShareForm_26";
            this.picShare.Size = new Size(34, 34);
            this.picShare.SizeMode = PictureBoxSizeMode.CenterImage;
            this.picShare.TabIndex = 0;
            this.picShare.TabStop = false;
            this.picShare.MouseDown += new MouseEventHandler(this.picShare_MouseDown);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(368, 204);
            this.Controls.Add((Control)this.FormPanel);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Name = "ShareForm_27";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "ShareForm_28";
            this.Load += new EventHandler(this.ShareForm_Load);
            this.FormPanel.ResumeLayout(false);
            this.FormPanel.PerformLayout();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((ISupportInitialize)this.picShare).EndInit();
            this.ResumeLayout(false);
        }
    }
}
