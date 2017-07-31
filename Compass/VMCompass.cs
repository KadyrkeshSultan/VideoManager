using Compass.Properties;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Compass
{
    public class VMCompass : UserControl
    {
        private string[] direction;
        private Image arrow;
        private int speed;
        private int bearing;
        private IContainer components;
        private PictureBox picture;

        
        public VMCompass()
        {
            direction = new string[17]
            {
                "VMCompass_1",
                "VMCompass_2",
                "VMCompass_3",
                "VMCompass_4",
                "VMCompass_5",
                "VMCompass_6",
                "VMCompass_7",
                "VMCompass_8",
                "VMCompass_9",
                "VMCompass_10",
                "VMCompass_11",
                "VMCompass_12",
                "VMCompass_13",
                "VMCompass_14",
                "VMCompass_15",
                "VMCompass_16",
                "VMCompass_17",
            };
            arrow = (Image)Resources.arrow;
            InitializeComponent();
        }

        
        public void SetBackgroundColor(Color clr)
        {
            picture.BackColor = clr;
            picture.Invalidate();
        }

        
        public void SetData(int speed, int bearing)
        {
            if (bearing >= 360)
                bearing = 0;
            if (bearing < 0 || bearing >= 360)
                return;
            if (speed < 0)
                speed = 0;
            this.speed = speed;
            this.bearing = bearing;
            picture.Invalidate();
        }

        
        public static Bitmap RotateImage(Image image, PointF offset, float angle)
        {
            Bitmap bitmap = new Bitmap(image.Width, image.Height);
            try
            {
                bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                Graphics graphics = Graphics.FromImage(bitmap);
                graphics.TranslateTransform(offset.X, offset.Y);
                graphics.RotateTransform(angle);
                graphics.TranslateTransform(-offset.X, -offset.Y);
                graphics.DrawImage(image, new PointF(0.0f, 0.0f));
            }
            catch (Exception ex)
            {
            }
            return bitmap;
        }

        
        private void picture_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            try
            {
                if (this.bearing <= 360 && this.bearing >= 0)
                {
                    Image image = (Image)VMCompass.RotateImage(this.arrow, new PointF((float)(this.arrow.Width / 2), (float)(this.arrow.Height / 2)), (float)this.bearing);
                    graphics.DrawImage(image, 0, 0, arrow.Width, arrow.Height);
                }
                if (speed >= 0)
                {
                    using (Font font = new Font("VMCompass_18", 10f, FontStyle.Bold, GraphicsUnit.Point))
                    {
                        Rectangle rectangle = new Rectangle(0, 35, 117, 46);
                        e.Graphics.DrawString(Convert.ToString(speed) + "VMCompass_19", font, Brushes.White, rectangle, new StringFormat()
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                }
                using (Font font = new Font("VMCompass_20", 10f, FontStyle.Bold, GraphicsUnit.Point))
                {
                    Rectangle rectangle = new Rectangle(0, 47, 117, 59);
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    string s = direction[(int)((double)this.bearing / 22.5)];
                    e.Graphics.DrawString(s, font, Brushes.White, (RectangleF)rectangle, format);
                }
            }
            catch (Exception ex)
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
            this.picture = new PictureBox();
            ((ISupportInitialize)this.picture).BeginInit();
            this.SuspendLayout();
            this.picture.Dock = DockStyle.Fill;
            this.picture.Image = (Image)Resources.face;
            this.picture.Location = new Point(0, 0);
            this.picture.Margin = new Padding(0);
            this.picture.Name = "VMCompass_21";
            this.picture.Size = new Size(117, 117);
            this.picture.TabIndex = 0;
            this.picture.TabStop = false;
            this.picture.Paint += new PaintEventHandler(this.picture_Paint);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Transparent;
            this.Controls.Add((Control)this.picture);
            this.Name = "VMCompass_22";
            this.Size = new Size(117, 117);
            ((ISupportInitialize)this.picture).EndInit();
            this.ResumeLayout(false);
        }
    }
}
