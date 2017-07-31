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
            mnu_ViewImageFile.Text = LangCtrl.GetString("VideoPlay_ImgFrame_1", "VideoPlay_ImgFrame_2");
            mnu_ExportImage.Text = LangCtrl.GetString("VideoPlay_ImgFrame_3", "VideoPlay_ImgFrame_4");
            mnu_RedactImage.Text = LangCtrl.GetString("VideoPlay_ImgFrame_5", "VideoPlay_ImgFrame_6");
            mnu_RemoveImage.Text = LangCtrl.GetString("VideoPlay_ImgFrame_7", "VideoPlay_ImgFrame_8");
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
                Logger.Logging.WriteAccountLog(VMGlobal.LOG_ACTION.EXPORT, string.Format("VideoPlay_ImgFrame_9", fileName), Global.GlobalAccount.Id);
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
                Global.Log("VideoPlay_ImgFrame_11", "VideoPlay_ImgFrame_12");
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = path;
                if (!File.Exists(FileName))
                    return;
                Global.Log("VideoPlay_ImgFrame_13", string.Format("VideoPlay_ImgFrame_14", FileName));
                startInfo.Arguments = "VideoPlay_ImgFrame_15" + FileName + "VideoPlay_ImgFrame_16";
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
            this.components = (IContainer)new Container();
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
            this.SuspendLayout();
            this.pic.ContextMenuStrip = this.contextMenuStrip1;
            this.pic.Cursor = Cursors.Hand;
            this.pic.Dock = DockStyle.Fill;
            this.pic.Location = new Point(0, 0);
            this.pic.Name = "VideoPlay_ImgFrame_17";
            this.pic.Size = new Size(160, 100);
            this.pic.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pic.TabIndex = 0;
            this.pic.TabStop = false;
            this.pic.DoubleClick += new EventHandler(this.pic_DoubleClick);
            this.contextMenuStrip1.Items.AddRange(new ToolStripItem[5]
            {
        (ToolStripItem) this.mnu_ViewImageFile,
        (ToolStripItem) this.toolStripMenuItem1,
        (ToolStripItem) this.mnu_RemoveImage,
        (ToolStripItem) this.mnu_ExportImage,
        (ToolStripItem) this.mnu_RedactImage
            });
            this.contextMenuStrip1.Name = "VideoPlay_ImgFrame_18";
            this.contextMenuStrip1.Size = new Size(204, 98);
            this.mnu_ViewImageFile.Name = "VideoPlay_ImgFrame_19";
            this.mnu_ViewImageFile.Size = new Size(203, 22);
            this.mnu_ViewImageFile.Text = "VideoPlay_ImgFrame_20";
            this.mnu_ViewImageFile.Click += new EventHandler(this.mnu_ViewImageFile_Click);
            this.toolStripMenuItem1.Name = "VideoPlay_ImgFrame_21";
            this.toolStripMenuItem1.Size = new Size(200, 6);
            this.mnu_RemoveImage.Name = "VideoPlay_ImgFrame_22";
            this.mnu_RemoveImage.Size = new Size(203, 22);
            this.mnu_RemoveImage.Text = "VideoPlay_ImgFrame_23";
            this.mnu_RemoveImage.Click += new EventHandler(this.mnu_RemoveImage_Click);
            this.mnu_ExportImage.Name = "VideoPlay_ImgFrame_24";
            this.mnu_ExportImage.Size = new Size(203, 22);
            this.mnu_ExportImage.Text = "VideoPlay_ImgFrame_25";
            this.mnu_ExportImage.Click += new EventHandler(this.mnu_ExportImage_Click);
            this.mnu_RedactImage.Name = "VideoPlay_ImgFrame_26";
            this.mnu_RedactImage.Size = new Size(203, 22);
            this.mnu_RedactImage.Text = "VideoPlay_ImgFrame_27";
            this.mnu_RedactImage.Visible = false;
            this.mnu_RedactImage.Click += new EventHandler(this.mnu_RedactImage_Click);
            this.saveFileDialog.DefaultExt = "VideoPlay_ImgFrame_28";
            this.saveFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            this.saveFileDialog.Title = "VideoPlay_ImgFrame_30";
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            this.Controls.Add((Control)this.pic);
            this.Name = "VideoPlay_ImgFrame_31";
            this.Size = new Size(160, 100);
            this.Load += new EventHandler(this.ImageFrame_Load);
            ((ISupportInitialize)this.pic).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
