using AppGlobal;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;

namespace VideoPlayer2
{
    public class SaveTag : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;
        public const int HT_CAPTION = 2;
        private const int CS_DROPSHADOW = 131072;
        private IContainer components;
        private Panel FormPanel;
        private Panel HeaderPanel;
        private Label lbl_TagSaveTitle;
        private vTextBox txtDesc;
        private Label lbl_Desc;
        private vButton btn_OK;
        private vButton btn_Cancel;

        public string Desc {  get;  set; }

        protected override CreateParams CreateParams
        {
            
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ClassStyle |= 131072;
                return createParams;
            }
        }

        
        public SaveTag()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        
        private void SaveTag_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
                HeaderPanel.BackgroundImage = Properties.Resources.topbar45;
            LangCtrl.reText(this);
            txtDesc.Select();
        }

        
        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            HeaderMouseDown(e);
        }

        
        private void lbl_TagSaveTitle_MouseDown(object sender, MouseEventArgs e)
        {
            HeaderMouseDown(e);
        }

        
        private void HeaderMouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            ReleaseCapture();
            SendMessage(Handle, 161, 2, 0);
        }

        
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        
        private void btn_OK_Click(object sender, EventArgs e)
        {
            OK();
        }

        
        private void OK()
        {
            Desc = txtDesc.Text;
            DialogResult = DialogResult.OK;
        }

        
        private void txtDesc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return)
                return;
            OK();
            e.Handled = true;
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        
        private void InitializeComponent()
        {
            this.FormPanel = new Panel();
            this.btn_Cancel = new vButton();
            this.btn_OK = new vButton();
            this.txtDesc = new vTextBox();
            this.lbl_Desc = new Label();
            this.HeaderPanel = new Panel();
            this.lbl_TagSaveTitle = new Label();
            this.FormPanel.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            base.SuspendLayout();
            this.FormPanel.BorderStyle = BorderStyle.FixedSingle;
            this.FormPanel.Controls.Add(this.btn_Cancel);
            this.FormPanel.Controls.Add(this.btn_OK);
            this.FormPanel.Controls.Add(this.txtDesc);
            this.FormPanel.Controls.Add(this.lbl_Desc);
            this.FormPanel.Controls.Add(this.HeaderPanel);
            this.FormPanel.Dock = DockStyle.Fill;
            this.FormPanel.Location = new Point(0, 0);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new Size(419, 132);
            this.FormPanel.TabIndex = 0;
            this.btn_Cancel.AllowAnimations = true;
            this.btn_Cancel.BackColor = Color.Transparent;
            this.btn_Cancel.Location = new Point(304, 86);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.RoundedCornersMask = 15;
            this.btn_Cancel.Size = new Size(100, 30);
            this.btn_Cancel.TabIndex = 3;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Cancel.Click += new EventHandler(this.btn_Cancel_Click);
            this.btn_OK.AllowAnimations = true;
            this.btn_OK.BackColor = Color.Transparent;
            this.btn_OK.Location = new Point(198, 86);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.RoundedCornersMask = 15;
            this.btn_OK.Size = new Size(100, 30);
            this.btn_OK.TabIndex = 2;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = false;
            this.btn_OK.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_OK.Click += new EventHandler(this.btn_OK_Click);
            this.txtDesc.BackColor = Color.White;
            this.txtDesc.BoundsOffset = new Size(1, 1);
            this.txtDesc.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtDesc.DefaultText = "";
            this.txtDesc.Location = new Point(155, 57);
            this.txtDesc.MaxLength = 64;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.PasswordChar = '\0';
            this.txtDesc.ScrollBars = ScrollBars.None;
            this.txtDesc.SelectionLength = 0;
            this.txtDesc.SelectionStart = 0;
            this.txtDesc.Size = new Size(249, 23);
            this.txtDesc.TabIndex = 1;
            this.txtDesc.TextAlign = HorizontalAlignment.Left;
            this.txtDesc.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.txtDesc.KeyDown += new KeyEventHandler(this.txtDesc_KeyDown);
            this.lbl_Desc.AutoSize = true;
            this.lbl_Desc.Location = new Point(21, 62);
            this.lbl_Desc.Name = "lbl_Desc";
            this.lbl_Desc.Size = new Size(60, 13);
            this.lbl_Desc.TabIndex = 0;
            this.lbl_Desc.Text = "Description";
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.lbl_TagSaveTitle);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(417, 40);
            this.HeaderPanel.TabIndex = 0;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.lbl_TagSaveTitle.AutoSize = true;
            this.lbl_TagSaveTitle.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_TagSaveTitle.ForeColor = Color.White;
            this.lbl_TagSaveTitle.Location = new Point(12, 12);
            this.lbl_TagSaveTitle.Name = "lbl_TagSaveTitle";
            this.lbl_TagSaveTitle.Size = new Size(144, 16);
            this.lbl_TagSaveTitle.TabIndex = 0;
            this.lbl_TagSaveTitle.Text = "SAVE VIDEO MARK";
            this.lbl_TagSaveTitle.MouseDown += new MouseEventHandler(this.lbl_TagSaveTitle_MouseDown);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(419, 132);
            base.Controls.Add(this.FormPanel);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "SaveTag";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "SaveTag";
            base.Load += new EventHandler(this.SaveTag_Load);
            this.FormPanel.ResumeLayout(false);
            this.FormPanel.PerformLayout();
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            base.ResumeLayout(false);
        }
    }
}
