using AppGlobal;
using CatalogPanel.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;

namespace CatalogPanel
{
    public class Language : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;

        public const int HT_CAPTION = 2;

        private const int CS_DROPSHADOW = 131072;

        private IContainer components;

        private Panel HeaderPanel;

        private vButton btnCloseDlg;

        private Label lbl_Language;

        private PictureBox pictureBox1;

        private Panel MainPanel;

        private vComboBox cboLang;

        private Label lbl_SelectLang;

        private vButton btn_SetLanguage;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                CreateParams classStyle = createParams;
                classStyle.ClassStyle = classStyle.ClassStyle | 131072;
                return createParams;
            }
        }

        public Language()
        {
            this.InitializeComponent();
        }

        private void btn_SetLanguage_Click(object sender, EventArgs e)
        {
            ListItem selectedItem = this.cboLang.SelectedItem;
            Global.DefaultLanguage = string.Format("{0}|{1}", (string)selectedItem.Tag, selectedItem.Text);
            if (LangCtrl.SetLanguage((string)selectedItem.Tag))
            {
                this.SetLanguage();
                base.DialogResult = DialogResult.OK;
            }
        }

        private void btnCloseDlg_Click(object sender, EventArgs e)
        {
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

        private void HeaderMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Language.ReleaseCapture();
                Language.SendMessage(base.Handle, 161, 2, 0);
            }
        }

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void InitializeComponent()
        {
            this.MainPanel = new Panel();
            this.btn_SetLanguage = new vButton();
            this.cboLang = new vComboBox();
            this.lbl_SelectLang = new Label();
            this.HeaderPanel = new Panel();
            this.btnCloseDlg = new vButton();
            this.lbl_Language = new Label();
            this.pictureBox1 = new PictureBox();
            this.MainPanel.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            base.SuspendLayout();
            this.MainPanel.BorderStyle = BorderStyle.FixedSingle;
            this.MainPanel.Controls.Add(this.btn_SetLanguage);
            this.MainPanel.Controls.Add(this.cboLang);
            this.MainPanel.Controls.Add(this.lbl_SelectLang);
            this.MainPanel.Dock = DockStyle.Fill;
            this.MainPanel.Location = new Point(0, 40);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new Size(263, 113);
            this.MainPanel.TabIndex = 1;
            this.btn_SetLanguage.AllowAnimations = true;
            this.btn_SetLanguage.BackColor = Color.Transparent;
            this.btn_SetLanguage.Enabled = false;
            this.btn_SetLanguage.Location = new Point(93, 67);
            this.btn_SetLanguage.Name = "btn_SetLanguage";
            this.btn_SetLanguage.RoundedCornersMask = 15;
            this.btn_SetLanguage.Size = new Size(157, 30);
            this.btn_SetLanguage.TabIndex = 2;
            this.btn_SetLanguage.Text = "Set Language";
            this.btn_SetLanguage.UseVisualStyleBackColor = false;
            this.btn_SetLanguage.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_SetLanguage.Click += new EventHandler(this.btn_SetLanguage_Click);
            this.cboLang.BackColor = Color.White;
            this.cboLang.DefaultText = "";
            this.cboLang.DisplayMember = "";
            this.cboLang.DropDownList = true;
            this.cboLang.DropDownMaximumSize = new Size(1000, 1000);
            this.cboLang.DropDownMinimumSize = new Size(10, 10);
            this.cboLang.DropDownResizeDirection = SizingDirection.Both;
            this.cboLang.DropDownWidth = 236;
            this.cboLang.Location = new Point(14, 32);
            this.cboLang.Name = "cboLang";
            this.cboLang.RoundedCornersMaskListItem = 15;
            this.cboLang.Size = new Size(236, 23);
            this.cboLang.TabIndex = 1;
            this.cboLang.UseThemeBackColor = false;
            this.cboLang.UseThemeDropDownArrowColor = true;
            this.cboLang.ValueMember = "";
            this.cboLang.VIBlendScrollBarsTheme = VIBLEND_THEME.VISTABLUE;
            this.cboLang.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_SelectLang.AutoSize = true;
            this.lbl_SelectLang.Location = new Point(14, 15);
            this.lbl_SelectLang.Name = "lbl_SelectLang";
            this.lbl_SelectLang.Size = new Size(55, 13);
            this.lbl_SelectLang.TabIndex = 0;
            this.lbl_SelectLang.Text = "Language";
            this.HeaderPanel.BackgroundImage = Properties.Resources.header;
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.btnCloseDlg);
            this.HeaderPanel.Controls.Add(this.lbl_Language);
            this.HeaderPanel.Controls.Add(this.pictureBox1);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(263, 40);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.btnCloseDlg.AllowAnimations = true;
            this.btnCloseDlg.BackColor = Color.Transparent;
            this.btnCloseDlg.Dock = DockStyle.Right;
            this.btnCloseDlg.Image = Properties.Resources.close;
            this.btnCloseDlg.Location = new Point(223, 0);
            this.btnCloseDlg.Name = "btnCloseDlg";
            this.btnCloseDlg.PaintBorder = false;
            this.btnCloseDlg.PaintDefaultBorder = false;
            this.btnCloseDlg.PaintDefaultFill = false;
            this.btnCloseDlg.RoundedCornersMask = 15;
            this.btnCloseDlg.Size = new Size(40, 40);
            this.btnCloseDlg.TabIndex = 2;
            this.btnCloseDlg.UseVisualStyleBackColor = false;
            this.btnCloseDlg.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnCloseDlg.Click += new EventHandler(this.btnCloseDlg_Click);
            this.lbl_Language.AutoSize = true;
            this.lbl_Language.BackColor = Color.Transparent;
            this.lbl_Language.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_Language.ForeColor = Color.White;
            this.lbl_Language.Location = new Point(45, 4);
            this.lbl_Language.Name = "lbl_Language";
            this.lbl_Language.Size = new Size(77, 16);
            this.lbl_Language.TabIndex = 1;
            this.lbl_Language.Text = "Language";
            this.lbl_Language.MouseDown += new MouseEventHandler(this.lbl_Language_MouseDown);
            this.pictureBox1.BackColor = Color.Transparent;
            this.pictureBox1.Image = Properties.Resources.globe;
            this.pictureBox1.Location = new Point(4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(34, 34);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new MouseEventHandler(this.pictureBox1_MouseDown);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(263, 153);
            base.Controls.Add(this.MainPanel);
            base.Controls.Add(this.HeaderPanel);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "Language";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Language";
            base.Load += new EventHandler(this.Language_Load);
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((ISupportInitialize)this.pictureBox1).EndInit();
            base.ResumeLayout(false);
        }

        private void Language_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
            {
                this.HeaderPanel.BackgroundImage = Properties.Resources.topbar45;
                this.btnCloseDlg.VIBlendTheme = VIBLEND_THEME.NERO;
            }
            this.SetLanguage();
            this.cboLang.Items.Clear();
            List<CultureInfo> appLanguages = LangCtrl.GetAppLanguages();
            if (appLanguages.Count > 0)
            {
                foreach (CultureInfo appLanguage in appLanguages)
                {
                    ListItem listItem = new ListItem()
                    {
                        Text = appLanguage.DisplayName,
                        Tag = appLanguage.Name
                    };
                    this.cboLang.Items.Add(listItem);
                }
                string[] strArrays = Global.DefaultLanguage.Split(new char[] { '|' });
                FormCtrl.SetComboItem(this.cboLang, strArrays[1]);
                this.btn_SetLanguage.Enabled = true;
            }
        }

        private void lbl_Language_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            this.HeaderMouseDown(e);
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void SetLanguage()
        {
            LangCtrl.reText(this);
        }
    }
}