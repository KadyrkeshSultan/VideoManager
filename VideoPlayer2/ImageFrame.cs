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

namespace VideoPlayer2
{
    public class ImageFrame : UserControl
    {
        private string FileName;
        private IContainer components;
        private PictureBox pic;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem mnu_ViewImageFile;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem mnu_RemoveImage;
        private ToolStripMenuItem mnu_ExportImage;
        private SaveFileDialog saveFileDialog;
        private ToolStripMenuItem mnu_RedactImage;

        
        public ImageFrame()
        {
            FileName = string.Empty;
            InitializeComponent();
        }

        
        public void CanExport(bool b)
        {
            if (b)
                return;
            mnu_ExportImage.Visible = false;
        }

        
        public void CanRedact(bool b)
        {
            mnu_RedactImage.Visible = b;
        }

        
        private void ImageFrame_Load(object sender, EventArgs e)
        {
            mnu_ViewImageFile.Text = LangCtrl.GetString("mnu_ViewImageFile", "View Image File");
            mnu_ExportImage.Text = LangCtrl.GetString("mnu_ExportImage", "Export Image");
            mnu_RedactImage.Text = LangCtrl.GetString("mnu_RedactImage", "Redact Image");
            mnu_RemoveImage.Text = LangCtrl.GetString("mnu_RemoveImage", "Remove Image from List");
        }

        
        public void LoadData(string filename, Image img)
        {
            pic.Image = img;
            FileName = filename;
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
                imgViewer2.OpenImage(this.FileName);
                int num = (int)imgViewer2.ShowDialog((IWin32Window)this);
            }
            catch
            {
            }
        }

        
        private void mnu_ViewImageFile_Click(object sender, EventArgs e)
        {
            ViewImage();
        }

        
        private void mnu_RemoveImage_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        
        private void mnu_ExportImage_Click(object sender, EventArgs e)
        {
            saveFileDialog.FileName = Path.GetFileName(this.FileName);
            if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
                return;
            try
            {
                string fileName = saveFileDialog.FileName;
                File.Copy(this.FileName, fileName);
                Logger.Logging.WriteAccountLog(VMGlobal.LOG_ACTION.EXPORT, string.Format("Export File: {0}", fileName), Global.GlobalAccount.Id);
            }
            catch
            {
            }
        }

        
        private void mnu_RedactImage_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "VideoPlay_ImgFrame_10");
            if (!File.Exists(path))
                return;
            try
            {
                Global.Log("REDACT", "Redact External File");
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = path;
                if (!File.Exists(FileName))
                    return;
                Global.Log("REDACT", string.Format("Image File: {0}", FileName));
                startInfo.Arguments = string.Concat("\"", this.FileName, "\"");
                Process.Start(startInfo);
            }
            catch
            {
            }
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
            this.mnu_ViewImageFile = new ToolStripMenuItem();
            this.toolStripMenuItem1 = new ToolStripSeparator();
            this.mnu_RemoveImage = new ToolStripMenuItem();
            this.mnu_ExportImage = new ToolStripMenuItem();
            this.mnu_RedactImage = new ToolStripMenuItem();
            this.saveFileDialog = new SaveFileDialog();
            ((ISupportInitialize)this.pic).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            base.SuspendLayout();
            this.pic.ContextMenuStrip = this.contextMenuStrip1;
            this.pic.Cursor = Cursors.Hand;
            this.pic.Dock = DockStyle.Fill;
            this.pic.Location = new Point(0, 0);
            this.pic.Name = "pic";
            this.pic.Size = new Size(160, 100);
            this.pic.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pic.TabIndex = 0;
            this.pic.TabStop = false;
            this.pic.DoubleClick += new EventHandler(this.pic_DoubleClick);
            ToolStripItemCollection items = this.contextMenuStrip1.Items;
            ToolStripItem[] mnuViewImageFile = new ToolStripItem[] { this.mnu_ViewImageFile, this.toolStripMenuItem1, this.mnu_RemoveImage, this.mnu_ExportImage, this.mnu_RedactImage };
            this.contextMenuStrip1.Items.AddRange(mnuViewImageFile);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new Size(204, 98);
            this.mnu_ViewImageFile.Name = "mnu_ViewImageFile";
            this.mnu_ViewImageFile.Size = new Size(203, 22);
            this.mnu_ViewImageFile.Text = "View Image File...";
            this.mnu_ViewImageFile.Click += new EventHandler(this.mnu_ViewImageFile_Click);
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new Size(200, 6);
            this.mnu_RemoveImage.Name = "mnu_RemoveImage";
            this.mnu_RemoveImage.Size = new Size(203, 22);
            this.mnu_RemoveImage.Text = "Remove Image from List";
            this.mnu_RemoveImage.Click += new EventHandler(this.mnu_RemoveImage_Click);
            this.mnu_ExportImage.Name = "mnu_ExportImage";
            this.mnu_ExportImage.Size = new Size(203, 22);
            this.mnu_ExportImage.Text = "Export Image";
            this.mnu_ExportImage.Click += new EventHandler(this.mnu_ExportImage_Click);
            this.mnu_RedactImage.Name = "mnu_RedactImage";
            this.mnu_RedactImage.Size = new Size(203, 22);
            this.mnu_RedactImage.Text = "Redact Image";
            this.mnu_RedactImage.Visible = false;
            this.mnu_RedactImage.Click += new EventHandler(this.mnu_RedactImage_Click);
            this.saveFileDialog.DefaultExt = "JPG";
            this.saveFileDialog.Filter = "JPEG File|*.jpg";
            this.saveFileDialog.Title = "Save Image File";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.Controls.Add(this.pic);
            base.Name = "ImageFrame";
            base.Size = new Size(160, 100);
            base.Load += new EventHandler(this.ImageFrame_Load);
            ((ISupportInitialize)this.pic).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}
