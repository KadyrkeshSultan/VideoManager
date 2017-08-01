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
                "Jan", "Feb",
                "Mar", "Apr",
                "May", "Jun",
                "Jul", "Aug",
                "Sep", "Oct",
                "Nov", "Dec"
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
                ROOT.Nodes.Add(new vTreeNode(string.Format("{0} [{1}]", account.ToString(), account.BadgeNumber))
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
                                e.Node.Nodes.Add(new vTreeNode(string.Format("{0}", current))
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
                                e.Node.Nodes.Add(new vTreeNode(string.Format("{0}", Months[current - 1]))
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
                                e.Node.Nodes.Add(new vTreeNode(string.Format("{0} • {1}", current, dayOfWeek))
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
            InitTree("Catalog by Ingest Date");
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        
        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FileTree));
            this.vTree = new vTreeView();
            this.TreeImages = new ImageList(this.components);
            this.TreeMenu = new ContextMenuStrip(this.components);
            this.mnuTreeLines = new ToolStripMenuItem();
            this.toolStripMenuItem1 = new ToolStripSeparator();
            this.mnu_RefreshIngestTree = new ToolStripMenuItem();
            this.TreeMenu.SuspendLayout();
            base.SuspendLayout();
            this.vTree.AccessibleName = "TreeView";
            this.vTree.AccessibleRole = AccessibleRole.List;
            this.vTree.ContextMenuStrip = this.TreeMenu;
            this.vTree.Dock = DockStyle.Fill;
            this.vTree.ImageList = this.TreeImages;
            this.vTree.Location = new Point(0, 0);
            this.vTree.Name = "vTree";
            this.vTree.ScrollPosition = new Point(0, 0);
            this.vTree.SelectedNode = null;
            this.vTree.Size = new Size(397, 498);
            this.vTree.TabIndex = 0;
            this.vTree.Text = "vTreeView1";
            this.vTree.VIBlendScrollBarsTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.vTree.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.vTree.NodeMouseUp += new vTreeViewMouseEventHandler(this.vTree_NodeMouseUp);
            this.TreeImages.ImageStream = (ImageListStreamer)Resources.FileTree.Tree_ImageStream;
            this.TreeImages.TransparentColor = Color.Transparent;
            this.TreeImages.Images.SetKeyName(0, "global.png");
            this.TreeImages.Images.SetKeyName(1, "person2.png");
            this.TreeImages.Images.SetKeyName(2, "year.png");
            this.TreeImages.Images.SetKeyName(3, "month.png");
            this.TreeImages.Images.SetKeyName(4, "day.png");
            ToolStripItemCollection items = this.TreeMenu.Items;
            ToolStripItem[] mnuRefreshIngestTree = new ToolStripItem[] { this.mnuTreeLines, this.toolStripMenuItem1, this.mnu_RefreshIngestTree };
            this.TreeMenu.Items.AddRange(mnuRefreshIngestTree);
            this.TreeMenu.Name = "TreeMenu";
            this.TreeMenu.Size = new Size(153, 76);
            this.mnuTreeLines.Name = "mnuTreeLines";
            this.mnuTreeLines.Size = new Size(152, 22);
            this.mnuTreeLines.Text = "Tree Lines";
            this.mnuTreeLines.Click += new EventHandler(this.mnuTreeLines_Click);
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new Size(149, 6);
            this.mnu_RefreshIngestTree.Name = "mnu_RefreshIngestTree";
            this.mnu_RefreshIngestTree.Size = new Size(152, 22);
            this.mnu_RefreshIngestTree.Text = "Refresh List";
            this.mnu_RefreshIngestTree.Click += new EventHandler(this.mnu_RefreshIngestTree_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.vTree);
            base.Name = "FileTree";
            base.Size = new Size(397, 498);
            base.Load += new EventHandler(this.FileTree_Load);
            this.TreeMenu.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public delegate void DEL_ClearDate();

        public delegate void DEL_IngestDateSelected(DateTime dt);
    }
}
