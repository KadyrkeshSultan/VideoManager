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
            string path1 = Path.Combine(rec.UNCName, rec.UNCPath);
            if (!path1.Contains("VideoPlay_ImgPanel_1"))
            {
                if (!path1.StartsWith("VideoPlay_ImgPanel_2"))
                    path1 = "VideoPlay_ImgPanel_3" + path1;
            }
            else if (path1.Contains("VideoPlay_ImgPanel_4") && !path1.Contains("VideoPlay_ImgPanel_5"))
                path1 = path1.Replace("VideoPlay_ImgPanel_6", "VideoPlay_ImgPanel_7");
            FilePath = Path.Combine(path1, rec.StoredFileName);
            Id = rec.Id;
            ToolTip toolTip = new ToolTip();
            timestamp = rec.FileAddedTimestamp.ToString();
            toolTip.SetToolTip(pic, string.Format("VideoPlay_ImgPanel_8", rec.FrameNumber));
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
            if (MessageBox.Show(this, "VideoPlay_ImgPanel_9", "VideoPlay_ImgPanel_10", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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
                Logger.Logging.WriteAccountLog(VMGlobal.LOG_ACTION.EXPORT, string.Format("VideoPlay_ImgPanel_11", fileName), Global.GlobalAccount.Id);
            }
            catch
            {
            }
        }

        
        private void mnu_RedactImage_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "VideoPlay_ImgPanel_12");
            if (!File.Exists(path))
                return;
            try
            {
                Global.Log("VideoPlay_ImgPanel_13", "VideoPlay_ImgPanel_14");
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = path;
                if (!File.Exists(this.FilePath))
                    return;
                Global.Log("VideoPlay_ImgPanel_15", string.Format("VideoPlay_ImgPanel_16", FilePath));
                startInfo.Arguments = "VideoPlay_ImgPanel_17" + FilePath + "VideoPlay_ImgPanel_18";
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
            this.components = (IContainer)new Container();
            this.pic = new PictureBox();
            this.contextMenuStrip1 = new ContextMenuStrip(this.components);
            this.mnu_ViewSnapshot = new ToolStripMenuItem();
            this.mnu_DeleteSnapshot = new ToolStripMenuItem();
            this.mnuExportImage = new ToolStripMenuItem();
            this.mnu_RedactImage = new ToolStripMenuItem();
            this.saveFileDialog = new SaveFileDialog();
            ((ISupportInitialize)this.pic).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            this.pic.BackColor = Color.Black;
            this.pic.ContextMenuStrip = this.contextMenuStrip1;
            this.pic.Dock = DockStyle.Fill;
            this.pic.Location = new Point(0, 0);
            this.pic.Name = "VideoPlay_ImgPanel_19";
            this.pic.Size = new Size(160, 100);
            this.pic.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pic.TabIndex = 0;
            this.pic.TabStop = false;
            this.pic.DoubleClick += new EventHandler(this.pic_DoubleClick);
            this.contextMenuStrip1.Items.AddRange(new ToolStripItem[4]
            {
        (ToolStripItem) this.mnu_ViewSnapshot,
        (ToolStripItem) this.mnu_DeleteSnapshot,
        (ToolStripItem) this.mnuExportImage,
        (ToolStripItem) this.mnu_RedactImage
            });
            this.contextMenuStrip1.Name = "VideoPlay_ImgPanel_20";
            this.contextMenuStrip1.Size = new Size(161, 92);
            this.mnu_ViewSnapshot.Name = "VideoPlay_ImgPanel_21";
            this.mnu_ViewSnapshot.Size = new Size(160, 22);
            this.mnu_ViewSnapshot.Text = "VideoPlay_ImgPanel_22";
            this.mnu_ViewSnapshot.Click += new EventHandler(this.mnu_ViewSnapshot_Click);
            this.mnu_DeleteSnapshot.Name = "VideoPlay_ImgPanel_23";
            this.mnu_DeleteSnapshot.Size = new Size(160, 22);
            this.mnu_DeleteSnapshot.Text = "VideoPlay_ImgPanel_24";
            this.mnu_DeleteSnapshot.Click += new EventHandler(this.mnu_DeleteSnapshot_Click);
            this.mnuExportImage.Name = "VideoPlay_ImgPanel_25";
            this.mnuExportImage.Size = new Size(160, 22);
            this.mnuExportImage.Text = "VideoPlay_ImgPanel_26";
            this.mnuExportImage.Click += new EventHandler(this.mnuExportImage_Click);
            this.mnu_RedactImage.Name = "VideoPlay_ImgPanel_27";
            this.mnu_RedactImage.Size = new Size(160, 22);
            this.mnu_RedactImage.Text = "VideoPlay_ImgPanel_28";
            this.mnu_RedactImage.Visible = false;
            this.mnu_RedactImage.Click += new EventHandler(this.mnu_RedactImage_Click);
            this.saveFileDialog.DefaultExt = "VideoPlay_ImgPanel_30";
            this.saveFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add((Control)this.pic);
            this.Name = "VideoPlay_ImgPanel_32";
            this.Size = new Size(160, 100);
            this.Load += new EventHandler(this.ImagePanel_Load);
            ((ISupportInitialize)this.pic).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
