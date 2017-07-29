using AppGlobal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMInterfaces;
using VMModels.Model;

namespace AccountCtrl2
{
    public class AcctCtrl : UserControl
    {
        private vTreeNode ROOT;
        private vTreeNodeCollection TreeNode;
        private IContainer components;
        private vTreeView vTree;
        private Panel SearchPanel;
        private vListBox vAcctList;
        private ImageList TreeImages;
        private vButton btn_Find;
        private vTextBox txtLastName;
        private Label lbl_LastName;
        private vTextBox txtBadgeNumber;
        private Label lbl_BadgeNumber;
        private ImageList AcctImages;
        private ContextMenuStrip TreeMenu;
        private ToolStripMenuItem mnu_RefreshDeptTree;
        private ToolStripMenuItem mnu_ShowTreeLines;
        private ToolStripMenuItem mnu_ClearAccountList;
        private vButton btn_ResetList;

        public bool IsShowCurrentAccount {  get;  set; }

        public event AcctCtrl.DEL_NodeCallback EVT_NodeCallback;

        
        public AcctCtrl()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            IsShowCurrentAccount = false;
        }

        
        private void AcctCtrl_Load(object sender, EventArgs e)
        {
            SetLanguage();
        }

        
        private void SetLanguage()
        {
            LangCtrl.reText(this);
        }

        
        public void InitTree()
        {
            SetLanguage();
            vTree.Nodes.Clear();
            vAcctList.Items.Clear();
            NodeRecord nodeRecord = new NodeRecord();
            nodeRecord.Name = LangCtrl.GetString("AccountCtrl2_AcctCtrl_1", "AccountCtrl2_AcctCtrl_1");
            nodeRecord.RecType = NodeType.ROOT_NODE;
            nodeRecord.ImgIdx = 0;
            nodeRecord.RecIdx = Guid.Empty;
            ROOT = new vTreeNode(nodeRecord.Name);
            ROOT.ImageIndex = 0;
            ROOT.Tag = nodeRecord;
            vTree.Nodes.Add(ROOT);
            TreeNode = ROOT.Nodes;
            if (LoadDepartments() > 0)
            {
                btn_ResetList.Visible = false;
                CheckUnassigned();
            }
            vAcctList.SelectedIndex = -1;
        }

        
        public void ClearList()
        {
            vAcctList.SelectedIndex = -1;
        }

        
        private void CheckUnassigned()
        {
            using (RPM_Account rpmAccount = new RPM_Account())
            {
                if (rpmAccount.GetUnassignedList().Count <= 0)
                    return;
                NodeRecord nodeRecord = new NodeRecord();
                nodeRecord.ImgIdx = 3;
                nodeRecord.Name = "AccountCtrl2_AcctCtrl_3";
                nodeRecord.RecIdx = Guid.Empty;
                nodeRecord.SubIdx = Guid.Empty;
                nodeRecord.RecType = NodeType.UNASSIGNED;
                ROOT.Nodes.Add(new vTreeNode(LangCtrl.GetString("AccountCtrl2_AcctCtrl_4", "AccountCtrl2_AcctCtrl_5"))
                {
                    Tag = nodeRecord,
                    ImageIndex = nodeRecord.ImgIdx
                });
            }
        }

        
        private int LoadDepartments()
        {
            int num = 0;
            using (RPM_Dept rpmDept = new RPM_Dept())
            {
                List<Department> deptList = rpmDept.GetDeptList();
                num = deptList.Count;
                if (num > 0)
                {
                    vTree.Visible = true;
                    foreach (Department department in deptList)
                    {
                        NodeRecord nodeRecord = new NodeRecord();
                        nodeRecord.ImgIdx = 1;
                        nodeRecord.Name = department.Name;
                        nodeRecord.RecIdx = department.Id;
                        nodeRecord.SubIdx = Guid.Empty;
                        nodeRecord.RecType = NodeType.DEPT;
                        ROOT.Nodes.Add(new vTreeNode(department.Name)
                        {
                            Tag = nodeRecord,
                            ImageIndex = nodeRecord.ImgIdx
                        });
                    }
                }
                else
                {
                    vTree.Visible = false;
                    LoadList(Guid.Empty, Guid.Empty);
                }
            }
            return num;
        }

        
        public void InitList()
        {
        }

        
        private void Callback(object sender, CmdAccountPickerEventArgs args)
        {
            if (EVT_NodeCallback == null)
                return;
            EVT_NodeCallback(this, args);
        }

        
        private void AcctCtrl_Resize(object sender, EventArgs e)
        {
            vAcctList.SuspendLayout();
            vTree.Width = (int)((double)this.Width / 2.25);
            vAcctList.ResumeLayout();
        }

        
        private void vTree_NodeMouseUp(object sender, vTreeViewMouseEventArgs e)
        {
            if (e.Node.Depth <= 0)
                return;
            vAcctList.SelectedIndex = -1;
            NodeRecord tag = (NodeRecord)e.Node.Tag;
            if (e.Node.Nodes.Count == 0 && e.Node.Depth == 1)
            {
                if (tag.RecType == NodeType.DEPT)
                {
                    using (RPM_Dept rpmDept = new RPM_Dept())
                    {
                        List<Substation> substationList = rpmDept.GetSubstationList(tag.RecIdx);
                        if (substationList != null)
                        {
                            foreach (Substation substation in substationList)
                                e.Node.Nodes.Add(new vTreeNode(substation.Name)
                                {
                                    Tag = new NodeRecord()
                                    {
                                        ImgIdx = 2,
                                        Name = substation.Name,
                                        RecIdx = tag.RecIdx,
                                        SubIdx = substation.Id,
                                        RecType = NodeType.SUBSTATION
                                    },
                                    ImageIndex = 2
                                });
                        }
                    }
                }
                if (tag.RecType == NodeType.UNASSIGNED)
                {
                    tag.RecIdx = Guid.Empty;
                    tag.SubIdx = Guid.Empty;
                }
            }
            LoadList(tag.RecIdx, tag.SubIdx);
        }

        
        private void LoadList(Guid deptId, Guid subId)
        {
            Cursor = Cursors.WaitCursor;
            vAcctList.Items.Clear();
            using (RPM_Account rpmAccount = new RPM_Account())
            {
                List<Account> accountList1 = new List<Account>();
                List<Account> accountList2 = !(deptId == Guid.Empty) || !(subId == Guid.Empty) ? rpmAccount.GetAccountList(deptId, subId) : rpmAccount.GetUnassignedList();
                if (accountList2 != null)
                {
                    if (accountList2.Count > 0)
                    {
                        foreach (Account rec in accountList2)
                        {
                            if (IsShowCurrentAccount)
                                LoadRecord(rec);
                            else if (Global.GlobalAccount.Id != rec.Id)
                                LoadRecord(rec);
                        }
                    }
                }
            }
            Cursor = Cursors.Default;
        }

        
        private void LoadRecord(Account rec)
        {
            this.vAcctList.Items.Add(new ListItem()
            {
                Text = string.Format("AccountCtrl2_AcctCtrl_6", rec.ToString(), rec.BadgeNumber, !string.IsNullOrEmpty(rec.Rank) ? string.Format("AccountCtrl2_AcctCtrl_7", rec.Rank) : string.Empty),
                Tag = new NodeRecord()
                {
                    RecIdx = rec.Id,
                    Name = rec.ToString(),
                    BadgeNumber = rec.BadgeNumber,
                    RecType = NodeType.ACCOUNT
                },
                ImageIndex = 0
            });
        }

        
        private void mnu_ShowTreeLines_Click(object sender, EventArgs e)
        {
            vTree.ShowRootLines = !vTree.ShowRootLines;
        }

        
        private void vAcctList_SelectedItemChanged(object sender, EventArgs e)
        {
            ListItem selectedItem = vAcctList.SelectedItem;
            if (selectedItem == null)
                return;
            Callback(this, new CmdAccountPickerEventArgs((NodeRecord)selectedItem.Tag));
        }

        
        private void btn_Find_Click(object sender, EventArgs e)
        {
            vAcctList.Items.Clear();
            vAcctList.SuspendLayout();
            Cursor = Cursors.WaitCursor;
            using (RPM_Account rpmAccount = new RPM_Account())
            {
                List<Account> accountList = rpmAccount.AccountSearch(txtBadgeNumber.Text, txtLastName.Text);
                txtBadgeNumber.Text = txtLastName.Text = string.Empty;
                if (accountList.Count > 0)
                {
                    vAcctList.SelectedIndex = -1;
                    foreach (Account rec in accountList)
                    {
                        if (IsShowCurrentAccount)
                            LoadRecord(rec);
                        else if (Global.GlobalAccount.Id != rec.Id)
                            LoadRecord(rec);
                    }
                }
            }
            vAcctList.ResumeLayout();
            Cursor = Cursors.Default;
        }

        
        private void vAcctList_DoubleClick(object sender, EventArgs e)
        {
        }

        
        private void mnu_RefreshDeptTree_Click(object sender, EventArgs e)
        {
            InitTree();
            vAcctList.Items.Clear();
        }

        
        private void mnu_ClearList_Click(object sender, EventArgs e)
        {
        }

        
        private void ClearAccountList()
        {
            vAcctList.SelectedIndex = -1;
            vAcctList.Items.Clear();
        }

        
        private void mnu_ClearAccountList_Click(object sender, EventArgs e)
        {
            ClearAccountList();
        }

        
        private void btn_ResetList_Click(object sender, EventArgs e)
        {
            LoadList(Guid.Empty, Guid.Empty);
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        
        private void InitializeComponent()
        {
            // TODO: Сделать ресурсные файлы
            components = new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(AcctCtrl));
            this.vTree = new vTreeView();
            this.TreeMenu = new ContextMenuStrip(this.components);
            this.mnu_RefreshDeptTree = new ToolStripMenuItem();
            this.mnu_ShowTreeLines = new ToolStripMenuItem();
            this.mnu_ClearAccountList = new ToolStripMenuItem();
            this.TreeImages = new ImageList(this.components);
            this.SearchPanel = new Panel();
            this.btn_ResetList = new vButton();
            this.btn_Find = new vButton();
            this.txtLastName = new vTextBox();
            this.lbl_LastName = new Label();
            this.txtBadgeNumber = new vTextBox();
            this.lbl_BadgeNumber = new Label();
            this.vAcctList = new vListBox();
            this.AcctImages = new ImageList(this.components);
            this.TreeMenu.SuspendLayout();
            this.SearchPanel.SuspendLayout();
            this.SuspendLayout();
            this.vTree.AccessibleName = "AccountCtrl2_AcctCtrl_8";
            this.vTree.AccessibleRole = AccessibleRole.List;
            this.vTree.BorderColor = Color.White;
            this.vTree.ContextMenuStrip = this.TreeMenu;
            this.vTree.Dock = DockStyle.Left;
            this.vTree.ImageList = this.TreeImages;
            this.vTree.Location = new Point(0, 0);
            this.vTree.Name = "AccountCtrl2_AcctCtrl_9";
            this.vTree.ScrollPosition = new Point(0, 0);
            this.vTree.SelectedNode = (vTreeNode)null;
            this.vTree.Size = new Size(189, 402);
            this.vTree.TabIndex = 0;
            this.vTree.Text = "AccountCtrl2_AcctCtrl_10";
            this.vTree.UseThemeBackColor = false;
            this.vTree.UseThemeBorderColor = false;
            this.vTree.VIBlendScrollBarsTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.vTree.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.vTree.NodeMouseUp += new vTreeViewMouseEventHandler(this.vTree_NodeMouseUp);
            this.TreeMenu.Items.AddRange(new ToolStripItem[3]
            {
        (ToolStripItem) this.mnu_RefreshDeptTree,
        (ToolStripItem) this.mnu_ShowTreeLines,
        (ToolStripItem) this.mnu_ClearAccountList
            });
            this.TreeMenu.Name = "AccountCtrl2_AcctCtrl_11";
            this.TreeMenu.Size = new Size(171, 70);
            this.mnu_RefreshDeptTree.Name = "AccountCtrl2_AcctCtrl_12";
            this.mnu_RefreshDeptTree.Size = new Size(170, 22);
            this.mnu_RefreshDeptTree.Text = "AccountCtrl2_AcctCtrl_13";
            this.mnu_RefreshDeptTree.Click += new EventHandler(this.mnu_RefreshDeptTree_Click);
            this.mnu_ShowTreeLines.Name = "AccountCtrl2_AcctCtrl_14";
            this.mnu_ShowTreeLines.Size = new Size(170, 22);
            this.mnu_ShowTreeLines.Text = "AccountCtrl2_AcctCtrl_15";
            this.mnu_ShowTreeLines.Click += new EventHandler(this.mnu_ShowTreeLines_Click);
            this.mnu_ClearAccountList.Name = "AccountCtrl2_AcctCtrl_16";
            this.mnu_ClearAccountList.Size = new Size(170, 22);
            this.mnu_ClearAccountList.Text = "AccountCtrl2_AcctCtrl_17";
            this.mnu_ClearAccountList.Click += new EventHandler(this.mnu_ClearAccountList_Click);
            this.TreeImages.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("TreeImages.ImageStream");
            this.TreeImages.TransparentColor = Color.Transparent;
            this.TreeImages.Images.SetKeyName(0, "AccountCtrl2_AcctCtrl_19");
            this.TreeImages.Images.SetKeyName(1, "AccountCtrl2_AcctCtrl_20");
            this.TreeImages.Images.SetKeyName(2, "AccountCtrl2_AcctCtrl_21");
            this.TreeImages.Images.SetKeyName(3, "AccountCtrl2_AcctCtrl_22");
            this.SearchPanel.Controls.Add((Control)this.btn_ResetList);
            this.SearchPanel.Controls.Add((Control)this.btn_Find);
            this.SearchPanel.Controls.Add((Control)this.txtLastName);
            this.SearchPanel.Controls.Add((Control)this.lbl_LastName);
            this.SearchPanel.Controls.Add((Control)this.txtBadgeNumber);
            this.SearchPanel.Controls.Add((Control)this.lbl_BadgeNumber);
            this.SearchPanel.Dock = DockStyle.Top;
            this.SearchPanel.Location = new Point(189, 0);
            this.SearchPanel.Name = "AccountCtrl2_AcctCtrl_23";
            this.SearchPanel.Size = new Size(250, 100);
            this.SearchPanel.TabIndex = 1;
            this.btn_ResetList.AllowAnimations = true;
            this.btn_ResetList.BackColor = Color.Transparent;
            this.btn_ResetList.Location = new Point(10, 67);
            this.btn_ResetList.Name = "AccountCtrl2_AcctCtrl_23";
            this.btn_ResetList.RoundedCornersMask = (byte)15;
            this.btn_ResetList.RoundedCornersRadius = 0;
            this.btn_ResetList.Size = new Size(106, 24);
            this.btn_ResetList.TabIndex = 5;
            this.btn_ResetList.Text = "AccountCtrl2_AcctCtrl_23";
            this.btn_ResetList.UseVisualStyleBackColor = false;
            this.btn_ResetList.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_ResetList.Click += new EventHandler(this.btn_ResetList_Click);
            this.btn_Find.AllowAnimations = true;
            this.btn_Find.BackColor = Color.Transparent;
            this.btn_Find.Image = (Image)Properties.Resources.search;
            this.btn_Find.ImageAlign = ContentAlignment.MiddleLeft;
            this.btn_Find.Location = new Point(122, 67);
            this.btn_Find.Name = "AccountCtrl2_AcctCtrl_24";
            this.btn_Find.RoundedCornersMask = (byte)15;
            this.btn_Find.RoundedCornersRadius = 0;
            this.btn_Find.Size = new Size(116, 24);
            this.btn_Find.TabIndex = 4;
            this.btn_Find.Text = "AccountCtrl2_AcctCtrl_25";
            this.btn_Find.UseVisualStyleBackColor = false;
            this.btn_Find.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btn_Find.Click += new EventHandler(this.btn_Find_Click);
            this.txtLastName.BackColor = Color.White;
            this.txtLastName.BoundsOffset = new Size(1, 1);
            this.txtLastName.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtLastName.DefaultText = "";
            this.txtLastName.Location = new Point(122, 37);
            this.txtLastName.MaxLength = 32;
            this.txtLastName.Name = "AccountCtrl2_AcctCtrl_26";
            this.txtLastName.PasswordChar = char.MinValue;
            this.txtLastName.ScrollBars = ScrollBars.None;
            this.txtLastName.SelectionLength = 0;
            this.txtLastName.SelectionStart = 0;
            this.txtLastName.Size = new Size(116, 23);
            this.txtLastName.TabIndex = 3;
            this.txtLastName.TextAlign = HorizontalAlignment.Left;
            this.txtLastName.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_LastName.Location = new Point(7, 42);
            this.lbl_LastName.Name = "AccountCtrl2_AcctCtrl_27";
            this.lbl_LastName.Size = new Size(110, 13);
            this.lbl_LastName.TabIndex = 2;
            this.lbl_LastName.Text = "AccountCtrl2_AcctCtrl_28";
            this.txtBadgeNumber.BackColor = Color.White;
            this.txtBadgeNumber.BoundsOffset = new Size(1, 1);
            this.txtBadgeNumber.ControlBorderColor = Color.FromArgb(39, 39, 39);
            this.txtBadgeNumber.DefaultText = "";
            this.txtBadgeNumber.Location = new Point(122, 8);
            this.txtBadgeNumber.MaxLength = 12;
            this.txtBadgeNumber.Name = "AccountCtrl2_AcctCtrl_29";
            this.txtBadgeNumber.PasswordChar = char.MinValue;
            this.txtBadgeNumber.ScrollBars = ScrollBars.None;
            this.txtBadgeNumber.SelectionLength = 0;
            this.txtBadgeNumber.SelectionStart = 0;
            this.txtBadgeNumber.Size = new Size(116, 23);
            this.txtBadgeNumber.TabIndex = 1;
            this.txtBadgeNumber.TextAlign = HorizontalAlignment.Left;
            this.txtBadgeNumber.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.lbl_BadgeNumber.Location = new Point(7, 13);
            this.lbl_BadgeNumber.Name = "AccountCtrl2_AcctCtrl_30";
            this.lbl_BadgeNumber.Size = new Size(110, 13);
            this.lbl_BadgeNumber.TabIndex = 0;
            this.lbl_BadgeNumber.Text = "AccountCtrl2_AcctCtrl_31";
            this.vAcctList.BorderColor = Color.White;
            this.vAcctList.Dock = DockStyle.Fill;
            this.vAcctList.ImageList = this.AcctImages;
            this.vAcctList.ItemHeight = 24;
            this.vAcctList.Location = new Point(189, 100);
            this.vAcctList.Name = "AccountCtrl2_AcctCtrl_32";
            this.vAcctList.RoundedCornersMaskListItem = (byte)15;
            this.vAcctList.Size = new Size(250, 302);
            this.vAcctList.TabIndex = 2;
            this.vAcctList.VIBlendScrollBarsTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.vAcctList.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.vAcctList.SelectedItemChanged += new EventHandler(this.vAcctList_SelectedItemChanged);
            this.vAcctList.DoubleClick += new EventHandler(this.vAcctList_DoubleClick);
            this.AcctImages.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("AcctImages.ImageStream");
            this.AcctImages.TransparentColor = Color.Transparent;
            this.AcctImages.Images.SetKeyName(0, "AccountCtrl2_AcctCtrl_34");
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add((Control)this.vAcctList);
            this.Controls.Add((Control)this.SearchPanel);
            this.Controls.Add((Control)this.vTree);
            this.Name = "AccountCtrl2_AcctCtrl_35";
            this.Size = new Size(439, 402);
            this.Load += new EventHandler(this.AcctCtrl_Load);
            this.Resize += new EventHandler(this.AcctCtrl_Resize);
            this.TreeMenu.ResumeLayout(false);
            this.SearchPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        public delegate void DEL_NodeCallback(object sender, CmdAccountPickerEventArgs args);
    }
}
