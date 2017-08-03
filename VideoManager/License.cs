using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;

namespace VideoManager
{
    public class License : Form
    {
        private IContainer components;

        private vRichTextBox txtLic;

        public License()
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
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(License));
            this.txtLic = new vRichTextBox();
            base.SuspendLayout();
            this.txtLic.AllowAnimations = false;
            this.txtLic.AllowFocused = false;
            this.txtLic.AllowHighlight = false;
            this.txtLic.BackColor = Color.White;
            this.txtLic.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtLic.Dock = DockStyle.Fill;
            this.txtLic.GleamWidth = 1;
            this.txtLic.Location = new Point(0, 0);
            this.txtLic.MaxLength = 2147483647;
            this.txtLic.Multiline = true;
            this.txtLic.Name = "txtLic";
            this.txtLic.Readonly = true;
            this.txtLic.Size = new Size(416, 516);
            this.txtLic.TabIndex = 0;
            this.txtLic.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(416, 516);
            base.Controls.Add(this.txtLic);
            base.Icon = (Icon)Resources.License.LicenseIcon;
            base.Name = "License";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "License";
            base.Load += new EventHandler(this.License_Load);
            base.ResumeLayout(false);
        }

        private void License_Load(object sender, EventArgs e)
        {
            this.Text = LangCtrl.GetString("dlg_License", "License");
            if (File.Exists("license.txt"))
            {
                using (StreamReader streamReader = new StreamReader("license.txt"))
                {
                    this.txtLic.Text = streamReader.ReadToEnd();
                }
            }
        }
    }
}