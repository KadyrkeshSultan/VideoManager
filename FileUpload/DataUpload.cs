using AppGlobal;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Unity;
using UploadCtrl;

namespace FileUpload
{
    public class DataUpload : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;

        public const int HT_CAPTION = 2;

        private const int CS_DROPSHADOW = 131072;

        public string[] FileNames;

        public Guid Account_ID = Guid.Empty;

        private bool IsOK;

        private IContainer components;

        private Panel HeaderPanel;

        private Label lbl_Upload;

        private OpenFileDialog openFileDialog1;

        private Upload upload1;

        private LinkLabel lnk_CancelUpload;

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

        public string SetID
        {
            get;
            set;
        }

        public DataUpload(Guid Id)
        {
            this.InitializeComponent();
            this.Account_ID = Id;
        }

        public DataUpload(Guid Id, string setid)
        {
            this.InitializeComponent();
            this.Account_ID = Id;
            this.SetID = setid;
        }

        public DataUpload()
        {
            this.InitializeComponent();
        }

        private void CloseUpload()
        {
            if (this.IsOK)
            {
                base.DialogResult = DialogResult.OK;
            }
            base.Close();
        }

        private void DataUpload_Load(object sender, EventArgs e)
        {
            this.IsOK = false;
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.FileName = "";
            if (this.openFileDialog1.ShowDialog(this) != DialogResult.OK)
            {
                this.CloseUpload();
            }
            else
            {
                bool flag = false;
                if (MessageBox.Show(this, "Import files using original file name?", "File Name", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    flag = true;
                }
                this.upload1.DeleteSource = false;
                this.upload1.EVT_UploadComplete += new Upload.DEL_UploadComplete(this.upload1_EVT_UploadComplete);
                this.upload1.CopyFiles(this.openFileDialog1.FileNames, Global.UNCServer, Global.RelativePath, this.Account_ID, this.SetID, flag);
                this.IsOK = true;
            }
            this.SetLanguage();
        }

        private void DataUpload_Paint(object sender, PaintEventArgs e)
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

        private void HeaderMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DataUpload.ReleaseCapture();
                DataUpload.SendMessage(base.Handle, 161, 2, 0);
            }
        }

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void InitializeComponent()
        {
            this.HeaderPanel = new Panel();
            this.lnk_CancelUpload = new LinkLabel();
            this.lbl_Upload = new Label();
            this.openFileDialog1 = new OpenFileDialog();
            this.upload1 = new Upload();
            this.HeaderPanel.SuspendLayout();
            base.SuspendLayout();
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.Controls.Add(this.lnk_CancelUpload);
            this.HeaderPanel.Controls.Add(this.lbl_Upload);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(372, 22);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.lnk_CancelUpload.Anchor = AnchorStyles.Right;
            this.lnk_CancelUpload.AutoSize = true;
            this.lnk_CancelUpload.BackColor = Color.Transparent;
            this.lnk_CancelUpload.LinkColor = Color.White;
            this.lnk_CancelUpload.Location = new Point(267, 4);
            this.lnk_CancelUpload.Name = "lnk_CancelUpload";
            this.lnk_CancelUpload.Size = new Size(96, 13);
            this.lnk_CancelUpload.TabIndex = 1;
            this.lnk_CancelUpload.TabStop = true;
            this.lnk_CancelUpload.Text = "Cancel File Upload";
            this.lnk_CancelUpload.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnk_CancelUpload_LinkClicked);
            this.lbl_Upload.AutoSize = true;
            this.lbl_Upload.BackColor = Color.Transparent;
            this.lbl_Upload.ForeColor = Color.White;
            this.lbl_Upload.Location = new Point(6, 5);
            this.lbl_Upload.Name = "lbl_Upload";
            this.lbl_Upload.Size = new Size(60, 13);
            this.lbl_Upload.TabIndex = 0;
            this.lbl_Upload.Text = "File Upload";
            this.lbl_Upload.MouseDown += new MouseEventHandler(this.lbl_Upload_MouseDown);
            this.openFileDialog1.FileName = "openFileDialog1";
            this.upload1.BackColor = Color.Transparent;
            this.upload1.DeleteSource = false;
            this.upload1.Dock = DockStyle.Fill;
            this.upload1.Filter = null;
            this.upload1.Location = new Point(0, 22);
            this.upload1.Margin = new Padding(0);
            this.upload1.Name = "upload1";
            this.upload1.ShowTimestamp = false;
            this.upload1.Size = new Size(372, 91);
            this.upload1.TabIndex = 1;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(372, 113);
            base.Controls.Add(this.upload1);
            base.Controls.Add(this.HeaderPanel);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "DataUpload";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Upload";
            base.Load += new EventHandler(this.DataUpload_Load);
            base.Paint += new PaintEventHandler(this.DataUpload_Paint);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            base.ResumeLayout(false);
        }

        private void lbl_Upload_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            base.Close();
        }

        private void lnk_CancelUpload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.upload1.CancelFlag = true;
            this.lnk_CancelUpload.Enabled = false;
            this.lnk_CancelUpload.Text = "Upload Canceled";
        }

        public void LoadFileNames(string[] FileData, Guid Id)
        {
            this.FileNames = FileData;
            this.Account_ID = Id;
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void SetLanguage()
        {
            LangCtrl.reText(this);
        }

        private void upload_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void upload1_EVT_UploadComplete()
        {
            this.CloseUpload();
        }
    }
}