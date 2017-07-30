using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMInterfaces;
using VMModels.Model;

namespace FileTreeCtrl2
{
    public class FileTree : UserControl
    {
        private vTreeNode ROOT;
        private vTreeNodeCollection TreeNode;
        private string[] Months;
        private int YEAR;
        private int MONTH;
        private IContainer components;
        private vTreeView vTree;
        private ImageList TreeImages;
        private ContextMenuStrip TreeMenu;
        private ToolStripMenuItem mnuTreeLines;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem mnu_RefreshIngestTree;

        public Guid AccountID {  get;  set; }

        public event DEL_ClearDate EVT_ClearDate;

        public event DEL_IngestDateSelected EVT_IngestDateSelected;

        
        public FileTree()
        {
            Months = new string[12]
            {
                "FileTree2_1",
                "FileTree2_2",
                "FileTree2_3",
                "FileTree2_4",
                "FileTree2_5",
                "FileTree2_6",
                "FileTree2_7",
                "FileTree2_8",
                "FileTree2_9",
                "FileTree2_10",
                "FileTree2_11",
                "FileTree2_12"
            };
            InitializeComponent();
            Dock = DockStyle.Fill;
        }

        
        private void FileTree_Load(object sender, EventArgs e)
        {
            this.SetLanguage();
        }

        
        private void SetLanguage()
        {
            LangCtrl.reText(this);
        }

        
        public void InitTree(string Title)
        {
            vTree.Nodes.Clear();
            ROOT = new vTreeNode(Title);
            ROOT.ImageIndex = 0;
            ROOT.Tag = null;
            vTree.Nodes.Add(this.ROOT);
            TreeNode = this.ROOT.Nodes;
            using (RPM_Account rpmAccount = new RPM_Account())
            {
                Account account = rpmAccount.GetAccount(this.AccountID);
                ROOT.Nodes.Add(new vTreeNode(string.Format("FileTree2_13", account.ToString(), account.BadgeNumber))
                {
                    ImageIndex = 1,
                    Tag = account.Id
                });
            }
        }

        
        private void ClearDateCallback()
        {
            if (EVT_ClearDate == null)
                return;
            EVT_ClearDate();
        }

        
        private void vTree_NodeMouseUp(object sender, vTreeViewMouseEventArgs e)
        {
            switch (e.Node.Depth)
            {
                case 0:
                    ClearDateCallback();
                    break;
                case 1:
                    ClearDateCallback();
                    if (e.Node.Nodes.Count != 0)
                        break;
                    using (VMContext VMContext = new VMContext())
                    {
                        IQueryable<DataFile> source = VMContext.DataFiles.Where(dataFile => dataFile.AccountId == AccountID);
                        Expression<Func<DataFile, int>> selector = dataFile => dataFile.FileAddedTimestamp.Value.Year;
                        using (List<int>.Enumerator enumerator = source.Select(selector).Distinct().ToList().GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                int current = enumerator.Current;
                                e.Node.Nodes.Add(new vTreeNode(string.Format("FileTree2_14", current))
                                {
                                    ImageIndex = 2,
                                    Tag = current
                                });
                            }
                            break;
                        }
                    }
                case 2:
                    ClearDateCallback();
                    if (e.Node.Nodes.Count != 0)
                        break;
                    AccountID = (Guid)e.Node.Parent.Tag;
                    YEAR = (int)e.Node.Tag;
                    using (VMContext VMContext = new VMContext())
                    {
                        IQueryable<DataFile> source = VMContext.DataFiles.Where(dataFile => dataFile.AccountId == AccountID && dataFile.FileAddedTimestamp.Value.Year == YEAR && dataFile.IsPurged == false);
                        Expression<Func<DataFile, int>> selector = dataFile => dataFile.FileAddedTimestamp.Value.Month;
                        using (List<int>.Enumerator enumerator = source.Select(selector).Distinct().ToList().GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                int current = enumerator.Current;
                                e.Node.Nodes.Add(new vTreeNode(string.Format("FileTree2_15", Months[current - 1]))
                                {
                                    ImageIndex = 3,
                                    Tag = current
                                });
                            }
                            break;
                        }
                    }
                case 3:
                    ClearDateCallback();
                    if (e.Node.Nodes.Count != 0)
                        break;
                    vTreeNode parent1 = e.Node.Parent;
                    YEAR = (int)parent1.Tag;
                    AccountID = (Guid)parent1.Parent.Tag;
                    MONTH = (int)e.Node.Tag;
                    using (VMContext VMContext = new VMContext())
                    {
                        IQueryable<DataFile> source = VMContext.DataFiles.Where(dataFile => dataFile.AccountId == AccountID && dataFile.FileAddedTimestamp.Value.Year == YEAR && dataFile.FileAddedTimestamp.Value.Month == MONTH && dataFile.IsPurged == false);
                        Expression<Func<DataFile, int>> selector = dataFile => dataFile.FileAddedTimestamp.Value.Day;
                        using (List<int>.Enumerator enumerator = source.Select(selector).Distinct().ToList().GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                int current = enumerator.Current;
                                DayOfWeek dayOfWeek = new DateTime(YEAR, MONTH, current).DayOfWeek;
                                e.Node.Nodes.Add(new vTreeNode(string.Format("FileTree2_16", current, dayOfWeek))
                                {
                                    ImageIndex = 4,
                                    Tag = current
                                });
                            }
                            break;
                        }
                    }
                case 4:
                    vTreeNode parent2 = e.Node.Parent;
                    MONTH = (int)parent2.Tag;
                    vTreeNode parent3 = parent2.Parent;
                    YEAR = (int)parent3.Tag;
                    AccountID = (Guid)parent3.Parent.Tag;
                    int tag = (int)e.Node.Tag;
                    if (tag <= 0 || tag > 31)
                        break;
                    Callback(new DateTime(YEAR, MONTH, tag, 0, 0, 0));
                    break;
            }
        }

        
        private void Callback(DateTime dt)
        {
            if (EVT_IngestDateSelected == null)
                return;
            EVT_IngestDateSelected(dt);
        }

        
        private void mnuTreeLines_Click(object sender, EventArgs e)
        {
            mnuTreeLines.Checked = vTree.ShowRootLines = !vTree.ShowRootLines;
        }

        
        private void mnu_RefreshIngestTree_Click(object sender, EventArgs e)
        {
            ClearDateCallback();
            InitTree("FileTree2_17");
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
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FileTree));
            this.vTree = new vTreeView();
            this.TreeImages = new ImageList(this.components);
            this.TreeMenu = new ContextMenuStrip(this.components);
            this.mnuTreeLines = new ToolStripMenuItem();
            this.toolStripMenuItem1 = new ToolStripSeparator();
            this.mnu_RefreshIngestTree = new ToolStripMenuItem();
            this.TreeMenu.SuspendLayout();
            this.SuspendLayout();
            this.vTree.AccessibleName = "FileTree2_18";
            this.vTree.AccessibleRole = AccessibleRole.List;
            this.vTree.ContextMenuStrip = this.TreeMenu;
            this.vTree.Dock = DockStyle.Fill;
            this.vTree.ImageList = this.TreeImages;
            this.vTree.Location = new Point(0, 0);
            this.vTree.Name = "FileTree2_19";
            this.vTree.ScrollPosition = new Point(0, 0);
            this.vTree.SelectedNode = (vTreeNode)null;
            this.vTree.Size = new Size(397, 498);
            this.vTree.TabIndex = 0;
            this.vTree.Text = "FileTree2_20";
            this.vTree.VIBlendScrollBarsTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.vTree.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.vTree.NodeMouseUp += new vTreeViewMouseEventHandler(this.vTree_NodeMouseUp);
            this.TreeImages.ImageStream = (ImageListStreamer)Resources.FileTree.Tree_ImageStream;
            this.TreeImages.TransparentColor = Color.Transparent;
            this.TreeImages.Images.SetKeyName(0, "FileTree2_22");
            this.TreeImages.Images.SetKeyName(1, "FileTree2_23");
            this.TreeImages.Images.SetKeyName(2, "FileTree2_24");
            this.TreeImages.Images.SetKeyName(3, "FileTree2_25");
            this.TreeImages.Images.SetKeyName(4, "FileTree2_26");
            this.TreeMenu.Items.AddRange(new ToolStripItem[3]
            {
        (ToolStripItem) this.mnuTreeLines,
        (ToolStripItem) this.toolStripMenuItem1,
        (ToolStripItem) this.mnu_RefreshIngestTree
            });
            this.TreeMenu.Name = "FileTree2_27";
            this.TreeMenu.Size = new Size(153, 76);
            this.mnuTreeLines.Name = "FileTree2_28";
            this.mnuTreeLines.Size = new Size(152, 22);
            this.mnuTreeLines.Text = "FileTree2_29";
            this.mnuTreeLines.Click += new EventHandler(this.mnuTreeLines_Click);
            this.toolStripMenuItem1.Name = "FileTree2_30";
            this.toolStripMenuItem1.Size = new Size(149, 6);
            this.mnu_RefreshIngestTree.Name = "FileTree2_31";
            this.mnu_RefreshIngestTree.Size = new Size(152, 22);
            this.mnu_RefreshIngestTree.Text = "FileTree2_32";
            this.mnu_RefreshIngestTree.Click += new EventHandler(this.mnu_RefreshIngestTree_Click);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add((Control)this.vTree);
            this.Name = "FileTree2_33";
            this.Size = new Size(397, 498);
            this.Load += new EventHandler(this.FileTree_Load);
            this.TreeMenu.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        public delegate void DEL_ClearDate();

        public delegate void DEL_IngestDateSelected(DateTime dt);
    }
}
