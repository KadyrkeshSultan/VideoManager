using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CatalogPanel
{
    public class ScanImg : UserControl
    {
        private IContainer components;

        private PictureBox pic;

        public ScanImg()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pic = new PictureBox();
            ((ISupportInitialize)this.pic).BeginInit();
            base.SuspendLayout();
            this.pic.Dock = DockStyle.Fill;
            this.pic.Location = new Point(0, 0);
            this.pic.Margin = new Padding(0);
            this.pic.Name = "pic";
            this.pic.Size = new Size(80, 100);
            this.pic.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pic.TabIndex = 0;
            this.pic.TabStop = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.BorderStyle = BorderStyle.FixedSingle;
            base.Controls.Add(this.pic);
            base.Name = "ScanImg";
            base.Size = new Size(80, 100);
            base.Load += new EventHandler(this.ScanImg_Load);
            ((ISupportInitialize)this.pic).EndInit();
            base.ResumeLayout(false);
        }

        private void ScanImg_Load(object sender, EventArgs e)
        {
        }

        public void SetImage(Image img)
        {
            this.pic.Image = img;
        }
    }
}