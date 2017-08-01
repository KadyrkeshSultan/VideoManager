using AppGlobal;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;

namespace FileTreeCtrl
{
    public class FileTree : UserControl
    {
        private vTreeNode ROOT;
        private vTreeNodeCollection TreeNode;
        private bool IsLines;
        private IContainer components;
        private ImageList imageList1;
        private Panel PanelHeader;
        private vTreeView vTree;
        private Label lblDOY;
        private Label lbl_DayOfYear;
        private ContextMenuStrip TreeMenu;
        private ToolStripMenuItem mnu_ClearSelection;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem mnu_RefreshView;
        private ToolStripMenuItem mnu_TreeLines;

        private string RootPath {  get;  set; }

        public Guid Account_ID {  get;  set; }

        public event DEL_DateSelectCallback EVT_DateSelectCallback;

        
        public FileTree()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
        }

        
        private void SetLanguage()
        {
            LangCtrl.reText(this);
            mnu_ClearSelection.Text = LangCtrl.GetString("mnu_ClearSelection", "Clear Selection");
            mnu_RefreshView.Text = LangCtrl.GetString("mnu_RefreshView", "Refresh View");
            mnu_TreeLines.Text = LangCtrl.GetString("mnu_TreeLines", "Tree Lines");
        }

        
        private void FileTree_Load(object sender, EventArgs e)
        {
            SetLanguage();
            InitTree();
            LoadYears();
        }

        
        private void InitTree()
        {
            vTree.Nodes.Clear();
            NodeRecord nodeRecord = new NodeRecord();
            nodeRecord.Name = LangCtrl.GetString("node_FilesByDate", "Files By Date Uploaded");
            nodeRecord.RecType = NodeType.ROOT_NODE;
            nodeRecord.ImgIdx = 0;
            nodeRecord.FolderPath = string.Empty;
            ROOT = new vTreeNode(nodeRecord.Name);
            ROOT.ImageIndex = 0;
            ROOT.Tag = nodeRecord;
            vTree.Nodes.Add(ROOT);
            TreeNode = ROOT.Nodes;
        }

        
        private void LoadYears()
        {
            string str = string.Format("{0}\\{1}", Global.RelativePath, Account_ID);
            string uNCServer = Global.UNCServer;
            if (!uNCServer.Contains(":"))
            {
                if (!uNCServer.StartsWith("\\\\"))
                {
                    uNCServer = string.Concat("\\\\", uNCServer);
                }
            }
            else if (uNCServer.Contains(":") && !uNCServer.Contains(":\\"))
            {
                uNCServer = uNCServer.Replace(":", ":\\");
            }
            RootPath = Path.Combine(uNCServer, str);
            if (Directory.Exists(RootPath))
            {
                string[] directories = Directory.GetDirectories(RootPath);
                if (directories.Length > 0)
                {
                    string[] strArrays = directories;
                    for (int i = 0; i < (int)strArrays.Length; i++)
                    {
                        string str1 = strArrays[i];
                        string fileName = Path.GetFileName(str1);
                        NodeRecord nodeRecord = new NodeRecord()
                        {
                            Name = fileName,
                            ImgIdx = 1,
                            RecType = NodeType.YEAR,
                            FolderPath = str1
                        };
                        vTreeNode _vTreeNode = new vTreeNode(fileName)
                        {
                            Tag = nodeRecord,
                            ImageIndex = 1
                        };
                        ROOT.Nodes.Add(_vTreeNode);
                    }
                }
            }
        }

        
        public DateTime ParseDateFromPath(string path, DATETYPE dType)
        {
            DateTime dateTime = DateTime.Now;
            try
            {
                if (dType == DATETYPE.MONTH)
                {
                    string[] strArray = path.Substring(path.Length - 7, 7).Split('\\');
                    dateTime = new DateTime(Convert.ToInt32(strArray[0]), Convert.ToInt32(strArray[1]), 1);
                }
                if (dType == DATETYPE.DAY)
                {
                    string[] strArray = path.Substring(path.Length - 10, 10).Split('\\');
                    dateTime = new DateTime(Convert.ToInt32(strArray[0]), Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]));
                }
            }
            catch
            {
            }
            return dateTime;
        }

        
        private void vTree_NodeMouseUp(object sender, vTreeViewMouseEventArgs e)
        {
            this.lblDOY.Text = "";
            if (e.Node.Depth == 0)
                Callback(this, new CmdDateSelectEventArgs(string.Empty));
            if (e.Node.Nodes.Count == 0 && e.Node.Depth == 1)
            {
                NodeRecord tag = (NodeRecord)e.Node.Tag;
                if (Directory.Exists(RootPath))
                {
                    try
                    {
                        string[] directories = Directory.GetDirectories(tag.FolderPath);
                        if (directories.Length > 0)
                        {
                            foreach (string path in directories)
                            {
                                string fileName = Path.GetFileName(path);
                                e.Node.Nodes.Add(new vTreeNode(fileName)
                                {
                                    Tag = new NodeRecord()
                                    {
                                        Name = fileName,
                                        ImgIdx = 2,
                                        RecType = NodeType.MONTH,
                                        FolderPath = path,
                                        date = this.ParseDateFromPath(path, DATETYPE.MONTH)
                                    },
                                    ImageIndex = 2
                                });
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                this.lblDOY.Text = "";
            }
            if (e.Node.Nodes.Count == 0 && e.Node.Depth == 2)
            {
                NodeRecord tag = (NodeRecord)e.Node.Tag;
                if (Directory.Exists(tag.FolderPath))
                {
                    try
                    {
                        string[] directories = Directory.GetDirectories(tag.FolderPath);
                        if (directories.Length > 0)
                        {
                            foreach (string path in directories)
                            {
                                string fileName = Path.GetFileName(path);
                                NodeRecord nodeRecord = new NodeRecord();
                                nodeRecord.ImgIdx = 3;
                                nodeRecord.RecType = NodeType.DAY;
                                nodeRecord.FolderPath = path;
                                nodeRecord.date = this.ParseDateFromPath(path, DATETYPE.DAY);
                                nodeRecord.Name = string.Format("{0} • {1}", fileName, nodeRecord.date.DayOfWeek);
                                e.Node.Nodes.Add(new vTreeNode(nodeRecord.Name)
                                {
                                    Tag = nodeRecord,
                                    ImageIndex = 3
                                });
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                this.lblDOY.Text = "";
            }
            if (e.Node.Depth != 3)
                return;
            try
            {
                NodeRecord tag = (NodeRecord)e.Node.Tag;
                lblDOY.Text = string.Format("{0} • {1}", tag.date.DayOfYear, tag.date.DayOfWeek);
                Callback(this, new CmdDateSelectEventArgs(tag.date.ToString()));
            }
            catch
            {
            }
        }

        
        private void mnu_ClearSelection_Click(object sender, EventArgs e)
        {
        }

        
        private void mnu_RefreshView_Click(object sender, EventArgs e)
        {
            InitTree();
            LoadYears();
        }

        
        private void mnu_TreeLines_Click(object sender, EventArgs e)
        {
            IsLines = !this.IsLines;
            vTree.ShowRootLines = this.IsLines;
        }

        
        private void Callback(object sender, CmdDateSelectEventArgs args)
        {
            if (EVT_DateSelectCallback == null)
                return;
            EVT_DateSelectCallback(this, args);
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
            this.imageList1 = new ImageList(this.components);
            this.PanelHeader = new Panel();
            this.lblDOY = new Label();
            this.lbl_DayOfYear = new Label();
            this.vTree = new vTreeView();
            this.TreeMenu = new ContextMenuStrip(this.components);
            this.mnu_ClearSelection = new ToolStripMenuItem();
            this.toolStripMenuItem1 = new ToolStripSeparator();
            this.mnu_RefreshView = new ToolStripMenuItem();
            this.mnu_TreeLines = new ToolStripMenuItem();
            this.PanelHeader.SuspendLayout();
            this.TreeMenu.SuspendLayout();
            base.SuspendLayout();
            this.imageList1.ImageStream = (ImageListStreamer)Resources.FileTree.imageList1_ImageStream;
            this.imageList1.TransparentColor = Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "catalog.png");
            this.imageList1.Images.SetKeyName(1, "year.png");
            this.imageList1.Images.SetKeyName(2, "month.png");
            this.imageList1.Images.SetKeyName(3, "day.png");
            this.PanelHeader.Controls.Add(this.lblDOY);
            this.PanelHeader.Controls.Add(this.lbl_DayOfYear);
            this.PanelHeader.Dock = DockStyle.Top;
            this.PanelHeader.Location = new Point(0, 0);
            this.PanelHeader.Name = "PanelHeader";
            this.PanelHeader.Size = new Size(321, 34);
            this.PanelHeader.TabIndex = 0;
            this.lblDOY.AutoSize = true;
            this.lblDOY.Location = new Point(158, 11);
            this.lblDOY.Name = "lblDOY";
            this.lblDOY.Size = new Size(13, 13);
            this.lblDOY.TabIndex = 5;
            this.lblDOY.Text = "0";
            this.lbl_DayOfYear.AutoSize = true;
            this.lbl_DayOfYear.Location = new Point(16, 11);
            this.lbl_DayOfYear.Name = "lbl_DayOfYear";
            this.lbl_DayOfYear.Size = new Size(81, 13);
            this.lbl_DayOfYear.TabIndex = 4;
            this.lbl_DayOfYear.Text = "Day of the Year";
            this.vTree.AccessibleName = "TreeView";
            this.vTree.AccessibleRole = AccessibleRole.List;
            this.vTree.BorderColor = Color.Transparent;
            this.vTree.ContextMenuStrip = this.TreeMenu;
            this.vTree.Dock = DockStyle.Fill;
            this.vTree.ImageList = this.imageList1;
            this.vTree.Location = new Point(0, 34);
            this.vTree.Name = "vTree";
            this.vTree.ScrollPosition = new Point(0, 0);
            this.vTree.SelectedNode = null;
            this.vTree.Size = new Size(321, 418);
            this.vTree.TabIndex = 1;
            this.vTree.Text = "vTreeView1";
            this.vTree.UseThemeBorderColor = false;
            this.vTree.VIBlendScrollBarsTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.vTree.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.vTree.NodeMouseUp += new vTreeViewMouseEventHandler(this.vTree_NodeMouseUp);
            ToolStripItemCollection items = this.TreeMenu.Items;
            ToolStripItem[] mnuClearSelection = new ToolStripItem[] { this.mnu_ClearSelection, this.toolStripMenuItem1, this.mnu_RefreshView, this.mnu_TreeLines };
            this.TreeMenu.Items.AddRange(mnuClearSelection);
            this.TreeMenu.Name = "TreeMenu";
            this.TreeMenu.Size = new Size(153, 76);
            this.mnu_ClearSelection.Name = "mnu_ClearSelection";
            this.mnu_ClearSelection.Size = new Size(152, 22);
            this.mnu_ClearSelection.Text = "Clear Selection";
            this.mnu_ClearSelection.Click += new EventHandler(this.mnu_ClearSelection_Click);
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new Size(149, 6);
            this.mnu_RefreshView.Name = "mnu_RefreshView";
            this.mnu_RefreshView.Size = new Size(152, 22);
            this.mnu_RefreshView.Text = "Refresh View";
            this.mnu_RefreshView.Click += new EventHandler(this.mnu_RefreshView_Click);
            this.mnu_TreeLines.Name = "mnu_TreeLines";
            this.mnu_TreeLines.Size = new Size(152, 22);
            this.mnu_TreeLines.Text = "Tree Lines";
            this.mnu_TreeLines.Click += new EventHandler(this.mnu_TreeLines_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.Controls.Add(this.vTree);
            base.Controls.Add(this.PanelHeader);
            base.Name = "FileTree";
            base.Size = new Size(321, 452);
            base.Load += new EventHandler(this.FileTree_Load);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            this.TreeMenu.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public delegate void DEL_DateSelectCallback(object sender, CmdDateSelectEventArgs args);
    }
}
