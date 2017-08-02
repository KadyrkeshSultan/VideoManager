using FileUpload;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMInterfaces;
using VMModels.Model;

namespace SetTreeCtrl
{
    public class SetCtrl : UserControl
    {
        private vTreeNode ROOT;

        private vTreeNodeCollection TreeNode;

        private bool IsLines = true;

        private IContainer components;

        private Panel SetHeaderPanel;

        private Label lbl_SetID;

        private vButton btn_FindSet;

        private vTextBox txtSetID;

        private vTreeView vTree;

        private ImageList TreeImages;

        private Panel DatePanel;

        private vDateTimePicker ToDate;

        private Label lbl_EndDate;

        private vDateTimePicker FromDate;

        private Label lbl_StartDate;

        private Label lblLine;

        private vButton btn_ResetSets;

        private ContextMenuStrip TreeMenu;

        private ToolStripMenuItem mnu_NewSetFile;

        private ToolStripMenuItem clearSetSearchToolStripMenuItem;

        private ToolStripMenuItem mnu_TreeLines;

        private ToolStripMenuItem expandAllToolStripMenuItem;

        private ToolStripSeparator toolStripMenuItem1;

        private ToolStripMenuItem mnu_RefreshSets;

        public Guid AccountID
        {
            get;
            set;
        }

        public SetCtrl()
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        public SetCtrl(Guid AcctId)
        {
            this.InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.AccountID = AcctId;
        }

        private void btn_FindSet_Click(object sender, EventArgs e)
        {
            this.LoadNodes();
        }

        private void btn_ResetSets_Click(object sender, EventArgs e)
        {
            this.mnu_NewSetFile.Visible = false;
            FormCtrl.ClearForm(this);
            this.InitTree();
            this.ClearPanelCallback();
            this.ResetDates();
        }

        private void Callback(object sender, CmdEventArgs e)
        {
            if (this.EVT_TreeNodeCallback != null)
            {
                this.EVT_TreeNodeCallback(this, e);
            }
        }

        private void ClearPanelCallback()
        {
            if (this.EVT_ClearPanel != null)
            {
                this.EVT_ClearPanel();
            }
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
            this.components = new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SetCtrl));
            this.SetHeaderPanel = new Panel();
            this.btn_ResetSets = new vButton();
            this.lblLine = new Label();
            this.DatePanel = new Panel();
            this.ToDate = new vDateTimePicker();
            this.lbl_EndDate = new Label();
            this.FromDate = new vDateTimePicker();
            this.lbl_StartDate = new Label();
            this.btn_FindSet = new vButton();
            this.txtSetID = new vTextBox();
            this.lbl_SetID = new Label();
            this.vTree = new vTreeView();
            this.TreeMenu = new ContextMenuStrip(this.components);
            this.mnu_NewSetFile = new ToolStripMenuItem();
            this.clearSetSearchToolStripMenuItem = new ToolStripMenuItem();
            this.mnu_TreeLines = new ToolStripMenuItem();
            this.expandAllToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem1 = new ToolStripSeparator();
            this.mnu_RefreshSets = new ToolStripMenuItem();
            this.TreeImages = new ImageList(this.components);
            this.SetHeaderPanel.SuspendLayout();
            this.DatePanel.SuspendLayout();
            this.TreeMenu.SuspendLayout();
            base.SuspendLayout();
            this.SetHeaderPanel.Controls.Add(this.btn_ResetSets);
            this.SetHeaderPanel.Controls.Add(this.lblLine);
            this.SetHeaderPanel.Controls.Add(this.DatePanel);
            this.SetHeaderPanel.Controls.Add(this.btn_FindSet);
            this.SetHeaderPanel.Controls.Add(this.txtSetID);
            this.SetHeaderPanel.Controls.Add(this.lbl_SetID);
            this.SetHeaderPanel.Dock = DockStyle.Top;
            this.SetHeaderPanel.Location = new Point(0, 0);
            this.SetHeaderPanel.Name = "SetHeaderPanel";
            this.SetHeaderPanel.Size = new Size(290, 161);
            this.SetHeaderPanel.TabIndex = 1;
            this.btn_ResetSets.AllowAnimations = true;
            this.btn_ResetSets.BackColor = Color.Transparent;
            this.btn_ResetSets.Location = new Point(11, 102);
            this.btn_ResetSets.Name = "btn_ResetSets";
            this.btn_ResetSets.RoundedCornersMask = 15;
            this.btn_ResetSets.RoundedCornersRadius = 0;
            this.btn_ResetSets.Size = new Size(128, 30);
            this.btn_ResetSets.TabIndex = 7;
            this.btn_ResetSets.Text = "Reset";
            this.btn_ResetSets.UseVisualStyleBackColor = false;
            this.btn_ResetSets.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_ResetSets.Click += new EventHandler(this.btn_ResetSets_Click);
            this.lblLine.BackColor = Color.Gray;
            this.lblLine.Location = new Point(9, 141);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new Size(272, 1);
            this.lblLine.TabIndex = 6;
            this.DatePanel.Controls.Add(this.ToDate);
            this.DatePanel.Controls.Add(this.lbl_EndDate);
            this.DatePanel.Controls.Add(this.FromDate);
            this.DatePanel.Controls.Add(this.lbl_StartDate);
            this.DatePanel.Location = new Point(3, 31);
            this.DatePanel.Name = "DatePanel";
            this.DatePanel.Size = new Size(286, 61);
            this.DatePanel.TabIndex = 4;
            this.ToDate.BackColor = Color.White;
            this.ToDate.BorderColor = Color.Black;
            this.ToDate.Culture = new CultureInfo("");
            this.ToDate.DefaultDateTimeFormat = DefaultDateTimePatterns.Custom;
            this.ToDate.DropDownMaximumSize = new Size(1000, 1000);
            this.ToDate.DropDownMinimumSize = new Size(10, 10);
            this.ToDate.DropDownResizeDirection = SizingDirection.None;
            this.ToDate.FormatValue = "";
            this.ToDate.Location = new Point(142, 34);
            this.ToDate.MaxDate = new DateTime(2100, 1, 1, 0, 0, 0, 0);
            this.ToDate.MinDate = new DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.ToDate.Name = "ToDate";
            this.ToDate.ShowGrip = false;
            this.ToDate.Size = new Size(128, 23);
            this.ToDate.TabIndex = 15;
            this.ToDate.Text = "07/14/2014 17:46:50";
            this.ToDate.UseThemeBackColor = false;
            this.ToDate.UseThemeDropDownArrowColor = true;
            this.ToDate.Value = new DateTime(2017, 08, 03);
            this.ToDate.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_EndDate.AutoSize = true;
            this.lbl_EndDate.Location = new Point(9, 39);
            this.lbl_EndDate.Name = "lbl_EndDate";
            this.lbl_EndDate.Size = new Size(74, 13);
            this.lbl_EndDate.TabIndex = 14;
            this.lbl_EndDate.Text = "To Date/Time";
            this.FromDate.BackColor = Color.White;
            this.FromDate.BorderColor = Color.Black;
            this.FromDate.Culture = new CultureInfo("");
            this.FromDate.DefaultDateTimeFormat = DefaultDateTimePatterns.Custom;
            this.FromDate.DropDownMaximumSize = new Size(1000, 1000);
            this.FromDate.DropDownMinimumSize = new Size(10, 10);
            this.FromDate.DropDownResizeDirection = SizingDirection.None;
            this.FromDate.FormatValue = "";
            this.FromDate.Location = new Point(142, 5);
            this.FromDate.MaxDate = new DateTime(2100, 1, 1, 0, 0, 0, 0);
            this.FromDate.MinDate = new DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.FromDate.Name = "FromDate";
            this.FromDate.ShowGrip = false;
            this.FromDate.Size = new Size(128, 23);
            this.FromDate.TabIndex = 13;
            this.FromDate.Text = "07/14/2014 17:46:50";
            this.FromDate.UseThemeBackColor = false;
            this.FromDate.UseThemeDropDownArrowColor = true;
            this.FromDate.Value = new DateTime(2017, 08, 03);
            this.FromDate.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_StartDate.AutoSize = true;
            this.lbl_StartDate.Location = new Point(9, 10);
            this.lbl_StartDate.Name = "lbl_StartDate";
            this.lbl_StartDate.Size = new Size(84, 13);
            this.lbl_StartDate.TabIndex = 12;
            this.lbl_StartDate.Text = "From Date/Time";
            this.btn_FindSet.AllowAnimations = true;
            this.btn_FindSet.BackColor = Color.Transparent;
            this.btn_FindSet.Location = new Point(145, 102);
            this.btn_FindSet.Name = "btn_FindSet";
            this.btn_FindSet.RoundedCornersMask = 15;
            this.btn_FindSet.RoundedCornersRadius = 0;
            this.btn_FindSet.Size = new Size(128, 30);
            this.btn_FindSet.TabIndex = 3;
            this.btn_FindSet.Text = "Find Sets";
            this.btn_FindSet.UseVisualStyleBackColor = false;
            this.btn_FindSet.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_FindSet.Click += new EventHandler(this.btn_FindSet_Click);
            this.txtSetID.BackColor = Color.White;
            this.txtSetID.BoundsOffset = new Size(1, 1);
            this.txtSetID.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtSetID.DefaultText = "";
            this.txtSetID.Location = new Point(145, 6);
            this.txtSetID.MaxLength = 64;
            this.txtSetID.Name = "txtSetID";
            this.txtSetID.PasswordChar = '\0';
            this.txtSetID.ScrollBars = ScrollBars.None;
            this.txtSetID.SelectionLength = 0;
            this.txtSetID.SelectionStart = 0;
            this.txtSetID.Size = new Size(128, 23);
            this.txtSetID.TabIndex = 2;
            this.txtSetID.TextAlign = HorizontalAlignment.Left;
            this.txtSetID.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_SetID.AutoSize = true;
            this.lbl_SetID.Location = new Point(9, 11);
            this.lbl_SetID.Name = "lbl_SetID";
            this.lbl_SetID.Size = new Size(70, 13);
            this.lbl_SetID.TabIndex = 0;
            this.lbl_SetID.Text = "Set Name/ID";
            this.vTree.AccessibleName = "TreeView";
            this.vTree.AccessibleRole = AccessibleRole.List;
            this.vTree.BorderColor = Color.Transparent;
            this.vTree.ContextMenuStrip = this.TreeMenu;
            this.vTree.Dock = DockStyle.Fill;
            this.vTree.ImageList = this.TreeImages;
            this.vTree.Location = new Point(0, 161);
            this.vTree.Name = "vTree";
            this.vTree.ScrollPosition = new Point(0, 0);
            this.vTree.SelectedNode = null;
            this.vTree.ShowRootLines = true;
            this.vTree.Size = new Size(290, 363);
            this.vTree.TabIndex = 2;
            this.vTree.UseThemeBorderColor = false;
            this.vTree.VIBlendScrollBarsTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.vTree.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.vTree.NodeMouseUp += new vTreeViewMouseEventHandler(this.vTree_NodeMouseUp);
            ToolStripItemCollection items = this.TreeMenu.Items;
            ToolStripItem[] mnuNewSetFile = new ToolStripItem[] { this.mnu_NewSetFile, this.clearSetSearchToolStripMenuItem, this.mnu_TreeLines, this.expandAllToolStripMenuItem, this.toolStripMenuItem1, this.mnu_RefreshSets };
            TreeMenu.Items.AddRange(mnuNewSetFile);
            this.TreeMenu.Name = "TreeMenu";
            this.TreeMenu.Size = new Size(234, 120);
            this.mnu_NewSetFile.Name = "mnu_NewSetFile";
            this.mnu_NewSetFile.Size = new Size(233, 22);
            this.mnu_NewSetFile.Text = "Add New File to Selected Set...";
            this.mnu_NewSetFile.Visible = false;
            this.mnu_NewSetFile.Click += new EventHandler(this.mnu_NewSetFile_Click);
            this.clearSetSearchToolStripMenuItem.Name = "clearSetSearchToolStripMenuItem";
            this.clearSetSearchToolStripMenuItem.Size = new Size(233, 22);
            this.clearSetSearchToolStripMenuItem.Text = "Clear Set Search";
            this.mnu_TreeLines.Name = "mnu_TreeLines";
            this.mnu_TreeLines.Size = new Size(233, 22);
            this.mnu_TreeLines.Text = "Tree Lines";
            this.mnu_TreeLines.Click += new EventHandler(this.mnu_TreeLines_Click);
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            this.expandAllToolStripMenuItem.Size = new Size(233, 22);
            this.expandAllToolStripMenuItem.Text = "Expand All";
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new Size(230, 6);
            this.mnu_RefreshSets.Name = "mnu_RefreshSets";
            this.mnu_RefreshSets.Size = new Size(233, 22);
            this.mnu_RefreshSets.Text = "Refresh Sets";
            this.mnu_RefreshSets.Click += new EventHandler(this.mnu_RefreshSets_Click);
            this.TreeImages.ImageStream = (ImageListStreamer)Resources.SetCtrl.TreeImages_ImageStream;
            this.TreeImages.TransparentColor = Color.Transparent;
            this.TreeImages.Images.SetKeyName(0, "group.png");
            this.TreeImages.Images.SetKeyName(1, "groups.png");
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.Controls.Add(this.vTree);
            base.Controls.Add(this.SetHeaderPanel);
            base.Name = "SetCtrl";
            base.Size = new Size(290, 524);
            base.Load += new EventHandler(this.SetCtrl_Load);
            this.SetHeaderPanel.ResumeLayout(false);
            this.SetHeaderPanel.PerformLayout();
            this.DatePanel.ResumeLayout(false);
            this.DatePanel.PerformLayout();
            this.TreeMenu.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void InitTree()
        {
            try
            {
                this.vTree.Nodes.Clear();
                NodeRecord nodeRecord = new NodeRecord()
                {
                    Name = LangCtrl.GetString("node_CatSets", "Catalog Sets"),
                    RecType = NodeType.ROOT_NODE,
                    ImgIdx = 0,
                    RecIdx = Guid.Empty
                };
                this.ROOT = new vTreeNode(nodeRecord.Name)
                {
                    ImageIndex = 0,
                    Tag = nodeRecord
                };
                this.vTree.Nodes.Add(this.ROOT);
                this.TreeNode = this.ROOT.Nodes;
            }
            catch
            {
            }
        }

        private void LoadNodes()
        {
            this.ROOT.Nodes.Clear();
            try
            {
                using (RPM_DataFile rPMDataFile = new RPM_DataFile())
                {
                    Guid accountID = this.AccountID;
                    string text = this.txtSetID.Text;
                    DateTime value = this.FromDate.Value.Value;
                    DateTime? nullable = this.ToDate.Value;
                    foreach (DataFile setList in rPMDataFile.GetSetList(accountID, text, value, nullable.Value))
                    {
                        if (string.IsNullOrEmpty(setList.SetName))
                        {
                            continue;
                        }
                        NodeRecord nodeRecord = new NodeRecord()
                        {
                            Name = setList.SetName,
                            RecType = NodeType.PARENT,
                            ImgIdx = 1
                        };
                        vTreeNode _vTreeNode = new vTreeNode(nodeRecord.Name)
                        {
                            ImageIndex = 1,
                            Tag = nodeRecord,
                            TooltipText = string.Format("{0}\n{1}\n{2}", setList.FileTimestamp, setList.Classification, setList.ShortDesc)
                        };
                        this.ROOT.Nodes.Add(_vTreeNode);
                    }
                }
            }
            catch
            {
            }
        }

        private void mnu_NewSetFile_Click(object sender, EventArgs e)
        {
            try
            {
                vTreeNode selectedNode = this.vTree.SelectedNode;
                if (selectedNode.Selected && selectedNode.Depth == 1)
                {
                    NodeRecord tag = (NodeRecord)selectedNode.Tag;
                    if (!string.IsNullOrEmpty(tag.Name) && (new DataUpload(this.AccountID, tag.Name)).ShowDialog(this) == DialogResult.OK)
                    {
                        this.Callback(this, new CmdEventArgs(tag));
                    }
                }
            }
            catch
            {
            }
        }

        private void mnu_RefreshSets_Click(object sender, EventArgs e)
        {
            this.LoadNodes();
        }

        private void mnu_TreeLines_Click(object sender, EventArgs e)
        {
            this.IsLines = !this.IsLines;
            this.vTree.ShowRootLines = this.IsLines;
        }

        public void ResetDates()
        {
            vDateTimePicker fromDate = this.FromDate;
            DateTime now = DateTime.Now;
            fromDate.Value = new DateTime?(now.AddDays(-30));
            this.ToDate.Value = new DateTime?(DateTime.Now);
        }

        private void SetCtrl_Load(object sender, EventArgs e)
        {
            this.InitTree();
            this.SetLanguage();
            this.ResetDates();
        }

        private void SetLanguage()
        {
            LangCtrl.reText(this);
            this.mnu_NewSetFile.Text = LangCtrl.GetString("mnu_NewSetFile", "Add New File to Selected Set...");
            this.mnu_RefreshSets.Text = LangCtrl.GetString("mnu_RefreshSets", "Refresh Set List");
            this.mnu_TreeLines.Text = LangCtrl.GetString("mnu_TreeLines", "Tree Lines");
            this.expandAllToolStripMenuItem.Text = LangCtrl.GetString("expandAllToolStripMenuItem", "Expand All");
            this.clearSetSearchToolStripMenuItem.Text = LangCtrl.GetString("clearSetSearchToolStripMenuItem", "Clear Set Search");
        }

        private void vTree_NodeMouseUp(object sender, vTreeViewMouseEventArgs e)
        {
            if (e.MouseEventArgs.Button == MouseButtons.Left)
            {
                if (e.Node.Nodes.Count == 0 && e.Node.Depth == 1)
                {
                    this.mnu_NewSetFile.Visible = true;
                    NodeRecord tag = (NodeRecord)e.Node.Tag;
                    this.Callback(this, new CmdEventArgs(tag));
                    return;
                }
                this.mnu_NewSetFile.Visible = false;
                this.ClearPanelCallback();
            }
        }

        public event SetCtrl.DEL_ClearPanel EVT_ClearPanel;

        public event SetCtrl.DEL_TreeNodeCallback EVT_TreeNodeCallback;

        public delegate void DEL_ClearPanel();

        public delegate void DEL_TreeNodeCallback(object sender, CmdEventArgs e);
    }
}