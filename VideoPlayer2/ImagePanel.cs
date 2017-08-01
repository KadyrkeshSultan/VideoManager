using AppGlobal;
using ImgViewer;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Unity;
using VMInterfaces;
using VMModels.Model;

namespace VideoPlayer2
{
    public class ImagePanel : UserControl
    {
        private string FilePath;
        private string timestamp;
        private Guid Id;
        private Snapshot sRec;
        private IContainer components;
        private PictureBox pic;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem mnu_ViewSnapshot;
        private ToolStripMenuItem mnu_DeleteSnapshot;
        private ToolStripMenuItem mnuExportImage;
        private SaveFileDialog saveFileDialog;
        private ToolStripMenuItem mnu_RedactImage;

        
        public ImagePanel()
        {
            FilePath = string.Empty;
            timestamp = string.Empty;
            Id = Guid.Empty;
            sRec = new Snapshot();
            InitializeComponent();
        }

        
        public int FrameNumber()
        {
            return sRec.FrameNumber;
        }

        
        public void IsRedact(bool b)
        {
            if (!b)
                return;
            mnu_RedactImage.Visible = true;
        }

        
        public void SetImage(Snapshot rec, Image img)
        {
            sRec = rec;
            pic.Image = img;
            string str = Path.Combine(rec.UNCName, rec.UNCPath);
            if (!str.Contains(":"))
            {
                if (!str.StartsWith("\\\\"))
                {
                    str = string.Concat("\\\\", str);
                }
            }
            else if (str.Contains(":") && !str.Contains(":\\"))
            {
                str = str.Replace(":", ":\\");
            }
            FilePath = Path.Combine(str, rec.StoredFileName);
            Id = rec.Id;
            ToolTip toolTip = new ToolTip();
            timestamp = rec.FileAddedTimestamp.ToString();
            toolTip.SetToolTip(pic, string.Format("Video Frame: {0}", rec.FrameNumber));
        }

        
        private void pic_DoubleClick(object sender, EventArgs e)
        {
            ViewImage();
        }

        
        private void ViewImage()
        {
            try
            {
                ImgViewer2 imgViewer2 = new ImgViewer2();
                imgViewer2.OpenImage(this.FilePath, this.sRec);
                int num = (int)imgViewer2.ShowDialog((IWin32Window)this);
            }
            catch
            {
            }
        }

        
        private void mnu_ViewSnapshot_Click(object sender, EventArgs e)
        {
            ViewImage();
        }

        
        private void mnu_DeleteSnapshot_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Delete snapshot?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            using (RPM_Snapshot rpmSnapshot = new RPM_Snapshot())
            {
                if (File.Exists(this.FilePath))
                {
                    try
                    {
                        File.Delete(this.FilePath);
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                    }
                }
                if (File.Exists(this.FilePath))
                    return;
                rpmSnapshot.Delete(this.Id);
                rpmSnapshot.Save();
                this.Dispose();
            }
        }

        
        public void CanExport(bool b)
        {
            if (b)
                return;
            mnuExportImage.Visible = false;
        }

        
        private void mnuExportImage_Click(object sender, EventArgs e)
        {
            this.saveFileDialog.FileName = Path.GetFileName(this.FilePath);
            if (this.saveFileDialog.ShowDialog((IWin32Window)this) != DialogResult.OK)
                return;
            try
            {
                string fileName = this.saveFileDialog.FileName;
                File.Copy(this.FilePath, fileName);
                Logger.Logging.WriteAccountLog(VMGlobal.LOG_ACTION.EXPORT, string.Format("Export Snapshot: {0}", fileName), Global.GlobalAccount.Id);
            }
            catch
            {
            }
        }

        
        private void mnu_RedactImage_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Redact/VideoEditor.exe");
            if (!File.Exists(path))
                return;
            try
            {
                Global.Log("REDACT", "Redact External File");
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = path;
                if (!File.Exists(this.FilePath))
                    return;
                Global.Log("REDACT", string.Format("Image File: {0}", this.FilePath));
                startInfo.Arguments = string.Concat("\"", this.FilePath, "\"");
                Process.Start(startInfo);
            }
            catch
            {
            }
        }

        
        private void ImagePanel_Load(object sender, EventArgs e)
        {
            LangCtrl.reText((Control)this.contextMenuStrip1);
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        
        private void InitializeComponent()
        {
            this.components = new Container();
            this.pic = new PictureBox();
            this.contextMenuStrip1 = new ContextMenuStrip(this.components);
            this.mnu_ViewSnapshot = new ToolStripMenuItem();
            this.mnu_DeleteSnapshot = new ToolStripMenuItem();
            this.mnuExportImage = new ToolStripMenuItem();
            this.mnu_RedactImage = new ToolStripMenuItem();
            this.saveFileDialog = new SaveFileDialog();
            ((ISupportInitialize)this.pic).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            base.SuspendLayout();
            this.pic.BackColor = Color.Black;
            this.pic.ContextMenuStrip = this.contextMenuStrip1;
            this.pic.Dock = DockStyle.Fill;
            this.pic.Location = new Point(0, 0);
            this.pic.Name = "pic";
            this.pic.Size = new Size(160, 100);
            this.pic.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pic.TabIndex = 0;
            this.pic.TabStop = false;
            this.pic.DoubleClick += new EventHandler(this.pic_DoubleClick);
            ToolStripItemCollection items = this.contextMenuStrip1.Items;
            ToolStripItem[] mnuViewSnapshot = new ToolStripItem[] { this.mnu_ViewSnapshot, this.mnu_DeleteSnapshot, this.mnuExportImage, this.mnu_RedactImage };
            this.contextMenuStrip1.Items.AddRange(mnuViewSnapshot);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new Size(161, 92);
            this.mnu_ViewSnapshot.Name = "mnu_ViewSnapshot";
            this.mnu_ViewSnapshot.Size = new Size(160, 22);
            this.mnu_ViewSnapshot.Text = "View Snapshot...";
            this.mnu_ViewSnapshot.Click += new EventHandler(this.mnu_ViewSnapshot_Click);
            this.mnu_DeleteSnapshot.Name = "mnu_DeleteSnapshot";
            this.mnu_DeleteSnapshot.Size = new Size(160, 22);
            this.mnu_DeleteSnapshot.Text = "Delete Snapshot";
            this.mnu_DeleteSnapshot.Click += new EventHandler(this.mnu_DeleteSnapshot_Click);
            this.mnuExportImage.Name = "mnuExportImage";
            this.mnuExportImage.Size = new Size(160, 22);
            this.mnuExportImage.Text = "Export Image";
            this.mnuExportImage.Click += new EventHandler(this.mnuExportImage_Click);
            this.mnu_RedactImage.Name = "mnu_RedactImage";
            this.mnu_RedactImage.Size = new Size(160, 22);
            this.mnu_RedactImage.Text = "Redact Image";
            this.mnu_RedactImage.Visible = false;
            this.mnu_RedactImage.Click += new EventHandler(this.mnu_RedactImage_Click);
            this.saveFileDialog.DefaultExt = "JPG";
            this.saveFileDialog.Filter = "JPEG File|*.jpg";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.pic);
            base.Name = "ImagePanel";
            base.Size = new Size(160, 100);
            base.Load += new EventHandler(this.ImagePanel_Load);
            ((ISupportInitialize)this.pic).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}
