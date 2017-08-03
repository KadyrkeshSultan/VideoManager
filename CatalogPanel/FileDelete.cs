using AppGlobal;
using CatalogPanel.Properties;
using Logger;
using SlideCtrl2;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMInterfaces;
using VMModels.Model;

namespace CatalogPanel
{
    public class FileDelete : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;

        public const int HT_CAPTION = 2;

        private const int CS_DROPSHADOW = 131072;

        private FlowLayoutPanel FilePanel;

        private Guid Account_ID;

        private Thread thread;

        private bool IsCancel;

        private IContainer components;

        private Panel HeaderPanel;

        private Panel FormPanel;

        private vProgressBar progBar;

        private Label msgText;

        private Label lbl_FileDelete;

        private vButton btn_Cancel;

        private PictureBox pic;

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

        public FileDelete()
        {
            this.InitializeComponent();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.IsCancel = true;
            this.msgText.Text = LangCtrl.GetString("msg_CancelDelete", "Cancel file delete...");
            this.btn_Cancel.Enabled = false;
            Application.DoEvents();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FileDelete_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
            {
                this.HeaderPanel.BackgroundImage = Properties.Resources.topbar45;
            }
            LangCtrl.reText(this);
            this.IsCancel = false;
            if (this.FilePanel != null)
            {
                this.msgText.Text = LangCtrl.GetString("msg_DeletingFiles", "Deleting selected files...");
                this.thread = new Thread(new ThreadStart(this.Run));
                this.thread.Start();
            }
        }

        private void HeaderMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FileDelete.ReleaseCapture();
                FileDelete.SendMessage(base.Handle, 161, 2, 0);
            }
        }

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        public void InitDelete(ref FlowLayoutPanel flp, Guid AId)
        {
            this.FilePanel = flp;
            this.Account_ID = AId;
        }

        private void InitializeComponent()
        {
            this.HeaderPanel = new Panel();
            this.lbl_FileDelete = new Label();
            this.FormPanel = new Panel();
            this.pic = new PictureBox();
            this.btn_Cancel = new vButton();
            this.msgText = new Label();
            this.progBar = new vProgressBar();
            this.HeaderPanel.SuspendLayout();
            this.FormPanel.SuspendLayout();
            ((ISupportInitialize)this.pic).BeginInit();
            base.SuspendLayout();
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.lbl_FileDelete);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(400, 40);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.lbl_FileDelete.AutoSize = true;
            this.lbl_FileDelete.Font = new Font("Verdana", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_FileDelete.ForeColor = Color.White;
            this.lbl_FileDelete.Location = new Point(7, 11);
            this.lbl_FileDelete.Name = "lbl_FileDelete";
            this.lbl_FileDelete.Size = new Size(131, 18);
            this.lbl_FileDelete.TabIndex = 0;
            this.lbl_FileDelete.Text = "DELETE FILES";
            this.lbl_FileDelete.MouseDown += new MouseEventHandler(this.lbl_FileDelete_MouseDown);
            this.FormPanel.BackColor = Color.White;
            this.FormPanel.BorderStyle = BorderStyle.FixedSingle;
            this.FormPanel.Controls.Add(this.pic);
            this.FormPanel.Controls.Add(this.btn_Cancel);
            this.FormPanel.Controls.Add(this.msgText);
            this.FormPanel.Controls.Add(this.progBar);
            this.FormPanel.Dock = DockStyle.Fill;
            this.FormPanel.Location = new Point(0, 40);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new Size(400, 142);
            this.FormPanel.TabIndex = 1;
            this.pic.BackColor = Color.Transparent;
            this.pic.Location = new Point(29, 61);
            this.pic.Name = "pic";
            this.pic.Size = new Size(121, 67);
            this.pic.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pic.TabIndex = 3;
            this.pic.TabStop = false;
            this.btn_Cancel.AllowAnimations = true;
            this.btn_Cancel.BackColor = Color.Transparent;
            this.btn_Cancel.Location = new Point(269, 98);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.RoundedCornersMask = 15;
            this.btn_Cancel.RoundedCornersRadius = 0;
            this.btn_Cancel.Size = new Size(100, 30);
            this.btn_Cancel.TabIndex = 2;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Cancel.Click += new EventHandler(this.btn_Cancel_Click);
            this.msgText.AutoSize = true;
            this.msgText.Location = new Point(29, 41);
            this.msgText.Name = "msgText";
            this.msgText.Size = new Size(38, 13);
            this.msgText.TabIndex = 1;
            this.msgText.Text = "Ready";
            this.progBar.BackColor = Color.Transparent;
            this.progBar.Location = new Point(29, 9);
            this.progBar.Name = "progBar";
            this.progBar.RoundedCornersMask = 15;
            this.progBar.RoundedCornersRadius = 0;
            this.progBar.Size = new Size(340, 25);
            this.progBar.TabIndex = 0;
            this.progBar.Text = "vProgressBar1";
            this.progBar.Value = 0;
            this.progBar.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(400, 182);
            base.Controls.Add(this.FormPanel);
            base.Controls.Add(this.HeaderPanel);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "FileDelete";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "FileDelete";
            base.Load += new EventHandler(this.FileDelete_Load);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            this.FormPanel.ResumeLayout(false);
            this.FormPanel.PerformLayout();
            ((ISupportInitialize)this.pic).EndInit();
            base.ResumeLayout(false);
        }

        private void lbl_FileDelete_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool ReleaseCapture();

        private void Run()
        {
            int num = 0;
            using (RPM_GlobalConfig rPMGlobalConfig = new RPM_GlobalConfig())
            {
                GlobalConfig configRecord = rPMGlobalConfig.GetConfigRecord("FILEDELETE_MINDAYS");
                if (configRecord != null)
                {
                    num = Convert.ToInt32(configRecord.Value);
                }
            }
            int num1 = 0;
            for (int i1 = 0; i1 < this.FilePanel.Controls.Count; i1++)
            {
                if (((Slide)this.FilePanel.Controls[i1]).sRecord.IsSelected)
                {
                    num1++;
                }
            }
            int num2 = 0;
            this.progBar.Maximum = num1;
            try
            {
                bool flag = false;
                if (num > 0)
                {
                    int num3 = 0;
                    base.BeginInvoke(new MethodInvoker(() => {
                        for (int i = this.FilePanel.Controls.Count - 1; i >= 0; i--)
                        {
                            Slide item = (Slide)this.FilePanel.Controls[i];
                            if (item.sRecord.IsSelected)
                            {
                                this.msgText.Text = item.sRecord.dRecord.ShortDesc;
                                this.pic.Image = Utilities.ByteArrayToImage(item.sRecord.dRecord.Thumbnail);
                                num2++;
                                this.progBar.Value = num2;
                                flag = true;
                                if (!item.sRecord.dRecord.IsIndefinite & !item.sRecord.dRecord.IsEvidence && item.DeleteSlideFile(num))
                                {
                                    num3++;
                                    this.FilePanel.Controls.Remove(item);
                                }
                            }
                            if (this.IsCancel)
                            {
                                break;
                            }
                        }
                        string str = string.Format(LangCtrl.GetString("txt_FilesDeleted", "Files Deleted: {0}"), num3);
                        if (num2 != num3 && flag)
                        {
                            str = string.Concat(str, LangCtrl.GetString("txt_FilesDeleted2", "\nSome files could not be deleted. Minimum delete days."));
                        }
                        MessageBox.Show(this, str, "Files", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        base.Close();
                    }));
                }
            }
            catch (Exception exception)
            {
                Logger.Logging.WriteSystemLog(VMGlobal.LOG_ACTION.CODE_ERROR, exception.Message, this.Account_ID);
            }
            base.BeginInvoke(new MethodInvoker(() => base.Close()));
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public void SetCount(int c, string desc, Image img)
        {
            this.pic.Image = img;
            this.msgText.Text = desc;
            this.progBar.Value = c;
        }

        public void SetMaxSlides(int max)
        {
            this.progBar.Maximum = max;
        }
    }
}