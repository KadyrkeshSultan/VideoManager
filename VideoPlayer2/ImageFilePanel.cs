using AppGlobal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VideoPlayer2.Properties;
using VMModels.Enums;

namespace VideoPlayer2
{
    public class ImageFilePanel : UserControl
    {
        private IContainer components;
        private Panel HeaderPanel;
        private FlowLayoutPanel ImgFileFlowPanel;
        private Label lbl_ImageFiles;

        
        public ImageFilePanel()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
        }

        
        private void ImageFilePanel_Load(object sender, EventArgs e)
        {
            if (!Global.IS_WOLFCOM)
                return;
            this.HeaderPanel.BackgroundImage = (Image)Properties.Resources.topbar45;
        }

        
        public void LoadImages(List<ImageFile> FileData)
        {
            bool b = false;
            if (Global.IsRights(Global.RightsProfile, UserRights.EXPORT))
                b = true;
            try
            {
                foreach (ImageFile imageFile in FileData)
                {
                    ImageFrame imageFrame = new ImageFrame();
                    imageFrame.CanExport(b);
                    if (Global.IsRedact && Global.IsRights(Global.RightsProfile, UserRights.REDACT))
                        imageFrame.CanRedact(true);
                    imageFrame.LoadData(imageFile.FileName, imageFile.Thumbnail);
                    this.ImgFileFlowPanel.Controls.Add((Control)imageFrame);
                }
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
            this.HeaderPanel = new Panel();
            this.ImgFileFlowPanel = new FlowLayoutPanel();
            this.lbl_ImageFiles = new Label();
            this.HeaderPanel.SuspendLayout();
            this.SuspendLayout();
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add((Control)this.lbl_ImageFiles);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(325, 44);
            this.HeaderPanel.TabIndex = 1;
            this.ImgFileFlowPanel.AutoScroll = true;
            this.ImgFileFlowPanel.Dock = DockStyle.Fill;
            this.ImgFileFlowPanel.Location = new Point(0, 44);
            this.ImgFileFlowPanel.Name = "ImgFileFlowPanel";
            this.ImgFileFlowPanel.Size = new Size(325, 356);
            this.ImgFileFlowPanel.TabIndex = 2;
            this.lbl_ImageFiles.AutoSize = true;
            this.lbl_ImageFiles.BackColor = Color.Transparent;
            this.lbl_ImageFiles.Font = new Font("Verdana", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lbl_ImageFiles.ForeColor = Color.White;
            this.lbl_ImageFiles.Location = new Point(4, 12);
            this.lbl_ImageFiles.Name = "lbl_ImageFiles";
            this.lbl_ImageFiles.Size = new Size(188, 18);
            this.lbl_ImageFiles.TabIndex = 0;
            this.lbl_ImageFiles.Text = "Selected Image Files";
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add((Control)this.ImgFileFlowPanel);
            this.Controls.Add((Control)this.HeaderPanel);
            this.Name = "ImageFilePanel";
            this.Size = new Size(325, 400);
            this.Load += new EventHandler(this.ImageFilePanel_Load);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
