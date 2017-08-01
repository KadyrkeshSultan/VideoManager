using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VideoPlayer2
{
    public class NoGPS : UserControl
    {
        private IContainer components;
        private PictureBox pictureBox1;

        
        public NoGPS()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
        }

        
        private void NoGPS_Load(object sender, EventArgs e)
        {
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        
        private void InitializeComponent()
        {
            this.pictureBox1 = new PictureBox();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            this.SuspendLayout();
            this.pictureBox1.Dock = DockStyle.Fill;
            this.pictureBox1.Image = (Image)Properties.Resources.no;
            this.pictureBox1.Location = new Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(257, 305);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add((Control)this.pictureBox1);
            this.Name = "NoGPS";
            this.Size = new Size(257, 305);
            this.Load += new EventHandler(this.NoGPS_Load);
            ((ISupportInitialize)this.pictureBox1).EndInit();
            this.ResumeLayout(false);
        }
    }
}
