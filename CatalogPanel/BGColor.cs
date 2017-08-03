using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;

namespace CatalogPanel
{
    public class BGColor : Form
    {
        private IContainer components;

        private ColorPickerPanel ColorPick;

        public Color ColorValue
        {
            get;
            set;
        }

        public BGColor()
        {
            this.InitializeComponent();
        }

        private void BGColor_Load(object sender, EventArgs e)
        {
            this.Text = LangCtrl.GetString("dlg_BGColor", "Background Color");
        }

        private void ColorPick_SelectedColorChanged(object sender, EventArgs e)
        {
            this.ColorValue = this.ColorPick.SelectedColor;
            base.DialogResult = DialogResult.OK;
            base.Close();
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
            this.ColorPick = new ColorPickerPanel();
            base.SuspendLayout();
            this.ColorPick.Dock = DockStyle.Fill;
            this.ColorPick.Location = new Point(0, 0);
            this.ColorPick.Name = "ColorPick";
            this.ColorPick.SelectedColor = Color.Empty;
            this.ColorPick.Size = new Size(202, 250);
            this.ColorPick.TabIndex = 0;
            this.ColorPick.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.ColorPick.SelectedColorChanged += new EventHandler(this.ColorPick_SelectedColorChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(202, 247);
            base.Controls.Add(this.ColorPick);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.Name = "BGColor";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Background Color";
            base.Load += new EventHandler(this.BGColor_Load);
            base.ResumeLayout(false);
        }
    }
}