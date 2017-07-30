using AppGlobal;
using MemoEditor;
using MemoList.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMInterfaces;
using VMModels.Model;

namespace MemoList
{
    public class MemoForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 161;
        public const int HT_CAPTION = 2;
        private const int CS_DROPSHADOW = 131072;
        public DataFile dRecord;
        private Guid RecId;
        private string Document;
        private List<DOCData> DocList;
        private IContainer components;
        private ImageList imageList1;
        private Panel HeaderPanel;
        private vButton btnCloseDlg;
        private vCheckedListBox ListBox;
        private vButton btn_View;
        private CheckBox chk_PageBreaks;
        private ContextMenuStrip ListMenu;
        private ToolStripMenuItem mnuSelectAll;
        private ToolStripMenuItem mnuClearSelect;

        protected override CreateParams CreateParams
        {
            
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ClassStyle |= 131072;
                return createParams;
            }
        }

        
        public MemoForm(Guid Id, string DocDesc)
        {
            dRecord = new DataFile();
            RecId = Guid.Empty;
            Document = string.Empty;
            DocList = new List<DOCData>();
            InitializeComponent();
            Document = DocDesc;
            if (!(Id != Guid.Empty))
                return;
            RecId = Id;
        }

        
        public MemoForm()
        {
            dRecord = new DataFile();
            RecId = Guid.Empty;
            Document = string.Empty;
            DocList = new List<DOCData>();
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        
        private void MemoForm_Load(object sender, EventArgs e)
        {
            if (Global.IS_WOLFCOM)
                this.HeaderPanel.BackgroundImage = (Image)Properties.Resources.topbar45;
            LangCtrl.reText(this);
            if (dRecord != null)
            {
                ListBox.Items.Clear();
                using (RPM_DataFile rpmDataFile = new RPM_DataFile())
                {
                    dRecord = rpmDataFile.GetDataFile(RecId);
                    foreach (FileMemo fileMemo in dRecord.FileMemos)
                        ListBox.Items.Add(new ListItem()
                        {
                            Text = fileMemo.ShortDesc,
                            Description = string.Format("MemoList_1", fileMemo.Timestamp, fileMemo.AccountName, fileMemo.BadgeNumber),
                            Tag = fileMemo.Id
                        });
                }
            }
            SetLanguage();
        }

        
        private void SetLanguage()
        {
            LangCtrl.reText(this);
        }

        
        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            HeaderMouseDown(e);
        }

        
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
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

        
        private void btn_View_Click(object sender, EventArgs e)
        {
            DocList.Clear();
            for (int index = 0; index < ListBox.Items.Count; ++index)
            {
                if (ListBox.Items[index].IsChecked.Value)
                    DocList.Add(new DOCData()
                    {
                        RecId = (Guid)ListBox.Items[index].Tag,
                        Header = ListBox.Items[index].Description
                    });
            }
            if (DocList.Count <= 0)
                return;
            int num = (int)new EditorForm(DocList, chk_PageBreaks.Checked, Document).ShowDialog(this);
        }

        
        private void mnuSelectAll_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < ListBox.Items.Count; ++index)
                ListBox.Items[index].IsChecked = new bool?(true);
        }

        
        private void mnuClearSelect_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < ListBox.Items.Count; ++index)
                ListBox.Items[index].IsChecked = new bool?(false);
        }

        
        private void btnCloseDlg_Click(object sender, EventArgs e)
        {
            Close();
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        
        private void InitializeComponent()
        {
            this.components = (IContainer)new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MemoForm));
            this.imageList1 = new ImageList(this.components);
            this.ListBox = new vCheckedListBox();
            this.ListMenu = new ContextMenuStrip(this.components);
            this.mnuSelectAll = new ToolStripMenuItem();
            this.mnuClearSelect = new ToolStripMenuItem();
            this.btnCloseDlg = new vButton();
            this.HeaderPanel = new Panel();
            this.chk_PageBreaks = new CheckBox();
            this.btn_View = new vButton();
            this.ListMenu.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            this.SuspendLayout();
            this.imageList1.ImageStream = (ImageListStreamer)Resources.MemoForm.imageList1_ImageStream;
            this.imageList1.TransparentColor = Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "MemoList_2");
            this.ListBox.CheckOnClick = true;
            this.ListBox.ContextMenuStrip = this.ListMenu;
            this.ListBox.Dock = DockStyle.Fill;
            this.ListBox.ItemHeight = 36;
            this.ListBox.Location = new Point(0, 45);
            this.ListBox.Name = "MemoList_3";
            this.ListBox.RoundedCornersMaskListItem = (byte)15;
            this.ListBox.SelectionMode = SelectionMode.One;
            this.ListBox.Size = new Size(550, 555);
            this.ListBox.TabIndex = 2;
            this.ListBox.VIBlendScrollBarsTheme = VIBLEND_THEME.VISTABLUE;
            this.ListBox.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.ListMenu.Items.AddRange(new ToolStripItem[2]
            {
        (ToolStripItem) this.mnuSelectAll,
        (ToolStripItem) this.mnuClearSelect
            });
            this.ListMenu.Name = "MemoList_4";
            this.ListMenu.Size = new Size(187, 48);
            this.mnuSelectAll.Name = "MemoList_5";
            this.mnuSelectAll.Size = new Size(186, 22);
            this.mnuSelectAll.Text = "MemoList_6";
            this.mnuSelectAll.Click += new EventHandler(this.mnuSelectAll_Click);
            this.mnuClearSelect.Name = "MemoList_7";
            this.mnuClearSelect.Size = new Size(186, 22);
            this.mnuClearSelect.Text = "MemoList_8";
            this.mnuClearSelect.Click += new EventHandler(this.mnuClearSelect_Click);
            this.btnCloseDlg.AllowAnimations = true;
            this.btnCloseDlg.BackColor = Color.Transparent;
            this.btnCloseDlg.DialogResult = DialogResult.Cancel;
            this.btnCloseDlg.Dock = DockStyle.Right;
            this.btnCloseDlg.Image = (Image)Properties.Resources.close;
            this.btnCloseDlg.Location = new Point(505, 0);
            this.btnCloseDlg.Name = "MemoList_9";
            this.btnCloseDlg.PaintBorder = false;
            this.btnCloseDlg.PaintDefaultBorder = false;
            this.btnCloseDlg.PaintDefaultFill = false;
            this.btnCloseDlg.RoundedCornersMask = (byte)15;
            this.btnCloseDlg.RoundedCornersRadius = 0;
            this.btnCloseDlg.Size = new Size(45, 45);
            this.btnCloseDlg.TabIndex = 0;
            this.btnCloseDlg.UseVisualStyleBackColor = false;
            this.btnCloseDlg.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnCloseDlg.Click += new EventHandler(this.btnCloseDlg_Click);
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.Controls.Add((Control)this.chk_PageBreaks);
            this.HeaderPanel.Controls.Add((Control)this.btn_View);
            this.HeaderPanel.Controls.Add((Control)this.btnCloseDlg);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "MemoList_10";
            this.HeaderPanel.Size = new Size(550, 45);
            this.HeaderPanel.TabIndex = 1;
            this.HeaderPanel.MouseDown += new MouseEventHandler(this.HeaderPanel_MouseDown);
            this.chk_PageBreaks.AutoSize = true;
            this.chk_PageBreaks.BackColor = Color.Transparent;
            this.chk_PageBreaks.ForeColor = Color.White;
            this.chk_PageBreaks.Location = new Point(155, 13);
            this.chk_PageBreaks.Name = "MemoList_11";
            this.chk_PageBreaks.Size = new Size(116, 17);
            this.chk_PageBreaks.TabIndex = 3;
            this.chk_PageBreaks.Text = "MemoList_12";
            this.chk_PageBreaks.UseVisualStyleBackColor = false;
            this.btn_View.AllowAnimations = true;
            this.btn_View.BackColor = Color.Transparent;
            this.btn_View.Image = (Image)Properties.Resources.search;
            this.btn_View.ImageAlign = ContentAlignment.MiddleLeft;
            this.btn_View.Location = new Point(13, 6);
            this.btn_View.Name = "MemoList_13";
            this.btn_View.RoundedCornersMask = (byte)15;
            this.btn_View.RoundedCornersRadius = 0;
            this.btn_View.Size = new Size(136, 30);
            this.btn_View.TabIndex = 2;
            this.btn_View.Text = "MemoList_14";
            this.btn_View.UseVisualStyleBackColor = false;
            this.btn_View.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_View.Click += new EventHandler(this.btn_View_Click);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = (IButtonControl)this.btnCloseDlg;
            this.ClientSize = new Size(550, 600);
            this.Controls.Add((Control)this.ListBox);
            this.Controls.Add((Control)this.HeaderPanel);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Icon = (Icon)Resources.MemoForm.MemoListIcon;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MemoList_15";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "MemoList_16";
            this.Load += new EventHandler(this.MemoForm_Load);
            this.ListMenu.ResumeLayout(false);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
