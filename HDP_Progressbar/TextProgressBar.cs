using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HDP_Progressbar
{
    public class TextProgressBar : ProgressBar
    {
        private Color mTextForeColor;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                if ((Environment.OSVersion.Platform != PlatformID.Win32NT ? false : Environment.OSVersion.Version.Major >= 6))
                {
                    CreateParams exStyle = createParams;
                    exStyle.ExStyle = exStyle.ExStyle | 33554432;
                }
                return createParams;
            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                this.Refresh();
            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color TextForeColor
        {
            get
            {
                return this.mTextForeColor;
            }
            set
            {
                mTextForeColor = value;
            }
        }

        public TextProgressBar()
        {
            mTextForeColor = ForeColor;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 15)
            {
                Graphics graphic = CreateGraphics();
                try
                {
                    SolidBrush solidBrush = new SolidBrush(this.mTextForeColor);
                    try
                    {
                        SizeF sizeF = graphic.MeasureString(this.Text, SystemFonts.DefaultFont);
                        graphic.DrawString(this.Text, SystemFonts.DefaultFont, solidBrush, ((float)base.Width - sizeF.Width) / 2f, ((float)base.Height - sizeF.Height) / 2f);
                    }
                    finally
                    {
                        if (solidBrush != null)
                        {
                            solidBrush.Dispose();
                        }
                    }
                }
                finally
                {
                    if (graphic != null)
                    {
                        graphic.Dispose();
                    }
                }
            }
        }
    }
}