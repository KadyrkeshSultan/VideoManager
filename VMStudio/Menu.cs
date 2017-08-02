using IStudioPlugin;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
using VIBlend.WinForms.Controls;

namespace VMStudio
{
    public class Menu
    {
        private vTreeNode ROOT;

        private vTreeView menuTree = new vTreeView();

        private Dictionary<string, IPlugin> _Plugins;

        private ICollection<IPlugin> Plugins;

        private Panel PluginPanel;

        public Menu(vTreeView tv, ICollection<IPlugin> plugins, Panel pnl)
        {
            PluginPanel = pnl;
            menuTree = tv;
            menuTree.NodeMouseUp += new vTreeViewMouseEventHandler(menuTree_NodeMouseUp);
            Plugins = plugins;
            _Plugins = new Dictionary<string, IPlugin>();
            foreach (IPlugin plugin in Plugins)
            {
                _Plugins.Add(plugin.PluginName, plugin);
            }
            InitTreeMenu();
        }

        private void Assets()
        {
            vTreeNode _vTreeNode = new vTreeNode(LangCtrl.GetString("mnode_Inventory", "Inventory"))
            {
                ImageIndex = 4,
                Tag = null
            };
            ROOT.Nodes.Add(_vTreeNode);
            vTreeNode _vTreeNode1 = new vTreeNode(LangCtrl.GetString("mnode_Assets", "Assets"))
            {
                ImageIndex = 5,
                Tag = "Asset_Plugin"
            };
            _vTreeNode.Nodes.Add(_vTreeNode1);
        }

        private void Config()
        {
            vTreeNode _vTreeNode = new vTreeNode(LangCtrl.GetString("mnode_Config", "Configuration"))
            {
                ImageIndex = 8,
                Tag = null
            };
            this.ROOT.Nodes.Add(_vTreeNode);
            vTreeNode _vTreeNode1 = new vTreeNode(LangCtrl.GetString("mnode_DataSets", "Data Sets"))
            {
                ImageIndex = 9,
                Tag = "Cfg_Plugin"
            };
            _vTreeNode.Nodes.Add(_vTreeNode1);
            vTreeNode _vTreeNode2 = new vTreeNode(LangCtrl.GetString("mnode_CameraProfiles", "Camera Profiles"))
            {
                ImageIndex = 13,
                Tag = "Camera_Plugin"
            };
            _vTreeNode.Nodes.Add(_vTreeNode2);
            vTreeNode _vTreeNode3 = new vTreeNode(LangCtrl.GetString("mnode_CUT", "Cite Utility Tool"))
            {
                ImageIndex = 21,
                Tag = "Cut_Plugin"
            };
            _vTreeNode.Nodes.Add(_vTreeNode3);
            vTreeNode _vTreeNode4 = new vTreeNode(LangCtrl.GetString("mnode_VISION", "Vision Camera Tool"))
            {
                ImageIndex = 23,
                Tag = "Plg_VisionCamera"
            };
            _vTreeNode.Nodes.Add(_vTreeNode4);
            vTreeNode _vTreeNode5 = new vTreeNode(LangCtrl.GetString("mnode_Cloud", "Cloud Storage"))
            {
                ImageIndex = 10,
                Tag = "Cfg_Cloud"
            };
            _vTreeNode.Nodes.Add(_vTreeNode5);
            vTreeNode _vTreeNode6 = new vTreeNode(LangCtrl.GetString("mnode_System", "System"))
            {
                ImageIndex = 11,
                Tag = null
            };
            _vTreeNode.Nodes.Add(_vTreeNode6);
            vTreeNode _vTreeNode7 = new vTreeNode(LangCtrl.GetString("mnode_Param", "Parameters"))
            {
                ImageIndex = 12,
                Tag = "Cfg_SysParams"
            };
            _vTreeNode6.Nodes.Add(_vTreeNode7);
            vTreeNode _vTreeNode8 = new vTreeNode(LangCtrl.GetString("mnode_FileMgr", "File Management"))
            {
                ImageIndex = 14,
                Tag = "Cfg_FileMgr"
            };
            _vTreeNode6.Nodes.Add(_vTreeNode8);
            vTreeNode _vTreeNode9 = new vTreeNode(LangCtrl.GetString("mnode_Restore", "Restore File"))
            {
                ImageIndex = 22,
                Tag = "Plg_Restore"
            };
            _vTreeNode6.Nodes.Add(_vTreeNode9);
            vTreeNode _vTreeNode10 = new vTreeNode(LangCtrl.GetString("mnode_EmailSettings", "Email Settings"))
            {
                ImageIndex = 15,
                Tag = "Cfg_Email"
            };
            _vTreeNode6.Nodes.Add(_vTreeNode10);
            vTreeNode _vTreeNode11 = new vTreeNode(LangCtrl.GetString("mnode_License", "Camera License"))
            {
                ImageIndex = 16,
                Tag = "Cfg_License"
            };
            _vTreeNode6.Nodes.Add(_vTreeNode11);
        }

        private void InitTreeMenu()
        {
            menuTree.Nodes.Clear();
            ROOT = new vTreeNode(LangCtrl.GetString("mnode_Root", "C3 Sentinel Management"))
            {
                ImageIndex = 0,
                Tag = null
            };
            menuTree.Nodes.Add(this.ROOT);
            Organization();
            Assets();
            Config();
            Logs();
            Reports();
        }

        private void Logs()
        {
            vTreeNode _vTreeNode = new vTreeNode(LangCtrl.GetString("mnode_Logs", "Logs"))
            {
                ImageIndex = 17,
                Tag = null
            };
            ROOT.Nodes.Add(_vTreeNode);
            vTreeNode _vTreeNode1 = new vTreeNode(LangCtrl.GetString("mnode_AccountLogs", "Account Logs"))
            {
                ImageIndex = 18,
                Tag = "Plg_ALog"
            };
            _vTreeNode.Nodes.Add(_vTreeNode1);
        }

        private void menuTree_NodeMouseUp(object sender, vTreeViewMouseEventArgs e)
        {
            vTreeNode node = e.Node;
            if (node.Tag != null)
            {
                string tag = (string)node.Tag;
                string str = tag;
                string str1 = str;
                if (str != null)
                {
                    switch (str1)
                    {
                        case "Accounts_Plugin":
                        case "Dept_Plugin":
                        case "Asset_Plugin":
                        case "Cfg_Plugin":
                        case "Cfg_Cloud":
                        case "Cfg_SysParams":
                        case "Cfg_FileMgr":
                        case "Camera_Plugin":
                        case "AD_Plugin":
                        case "Cfg_Email":
                        case "Cfg_License":
                        case "Rpt_Plugin":
                        case "Cut_Plugin":
                        case "Plg_Restore":
                        case "Plg_VisionCamera":
                        case "Plg_ALog":
                            {
                                if (!this._Plugins.ContainsKey(tag))
                                {
                                    break;
                                }
                                try
                                {
                                    this.PluginPanel.Controls.Clear();
                                    IPlugin item = this._Plugins[tag];
                                    this.PluginPanel.Controls.Add((Control)item);
                                    break;
                                }
                                catch
                                {
                                    break;
                                }
                                break;
                            }
                        default:
                            {
                                return;
                            }
                    }
                }
            }
        }

        private void Organization()
        {
            vTreeNode _vTreeNode = new vTreeNode(LangCtrl.GetString("mnode_Org", "Organization"))
            {
                ImageIndex = 1,
                Tag = null
            };
            this.ROOT.Nodes.Add(_vTreeNode);
            vTreeNode _vTreeNode1 = new vTreeNode(LangCtrl.GetString("mnode_Dept", "Departments"))
            {
                ImageIndex = 2,
                Tag = "Dept_Plugin"
            };
            _vTreeNode.Nodes.Add(_vTreeNode1);
            vTreeNode _vTreeNode2 = new vTreeNode(LangCtrl.GetString("mnode_Accounts", "Accounts"))
            {
                ImageIndex = 3,
                Tag = "Accounts_Plugin"
            };
            _vTreeNode.Nodes.Add(_vTreeNode2);
            vTreeNode _vTreeNode3 = new vTreeNode(LangCtrl.GetString("mnode_AD", "Active Directory"))
            {
                ImageIndex = 7,
                Tag = "AD_Plugin"
            };
            _vTreeNode.Nodes.Add(_vTreeNode3);
        }

        private void Reports()
        {
            vTreeNode _vTreeNode = new vTreeNode(LangCtrl.GetString("mnode_Reports", "Reports"))
            {
                ImageIndex = 19,
                Tag = null
            };
            this.ROOT.Nodes.Add(_vTreeNode);
            vTreeNode _vTreeNode1 = new vTreeNode(LangCtrl.GetString("mnode_SysReports", "System Reports"))
            {
                ImageIndex = 20,
                Tag = "Rpt_Plugin"
            };
            _vTreeNode.Nodes.Add(_vTreeNode1);
        }
    }
}