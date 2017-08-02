using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HDP_Progressbar
{
    public class Progressbar : UserControl
    {
        private TextProgressBar tBar;

        private IContainer components;

        public int Maximum
        {
            get
            {
                return tBar.Maximum;
            }
            set
            {
                tBar.Maximum = value;
            }
        }

        public int Minimum
        {
            get
            {
                return tBar.Minimum;
            }
            set
            {
                tBar.Minimum = value;
            }
        }

        public int Step
        {
            get
            {
                return tBar.Step;
            }
            set
            {
                tBar.Step = value;
            }
        }

        public new string Text
        {
            get
            {
                return tBar.Text;
            }
            set
            {
                tBar.Text = value;
            }
        }

        public int Value
        {
            get
            {
                return tBar.Value;
            }
            set
            {
                tBar.Value = value;
            }
        }

        public Progressbar()
        {
            tBar = new TextProgressBar();
            components = null;
            InitializeComponent();
            tBar.Dock = DockStyle.Fill;
            Controls.Add(tBar);
        }

        protected override void Dispose(bool disposing)
        {
            if ((!disposing ? false : this.components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            Name = "Progressbar";
            Size = new Size(163, 24);
            ResumeLayout(false);
        }
    }
}