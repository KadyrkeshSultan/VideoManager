using AppGlobal;
using VMStudio.Properties;
using IStudioPlugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using WinReg;

namespace VMStudio
{
    public class VMMgr : Form
    {
        private Menu tMenu;

        private IContainer components;

        private Panel HeaderPanel;

        private StatusStrip statusStrip1;

        private vSplitContainer vSplitContainer1;

        private vTreeView MenuTree;

        private ImageList TreeImages;

        private PictureBox LogoPic;

        private Panel PluginPanel;

        private PictureBox pictureBox1;

        private Label lbl_StudioTitle;

        private vButton btnHelp;

        public VMMgr()
        {
            //TODO: License
            Licensing.LicenseContent = "0g0g634655384616543010g0d6aecca0330e04d0e089d90af5b167a1b80|og8GT3jOP3gGXBo1FoQ9xIVER7KGToXOBP07RoWIzh94AgnMYFZlCPEvLHY3/Qm2L+058lx9iRXUKiQct62Spkwf0PI//PUXJ2AW2g03CaAA+r+rGOT6/IEkJAHEcCH/o9l3YDkIObsg2k+ZBCPM3i7WRxU5ANRD0q/AN5AIzuU=";
            this.InitializeComponent();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            string str = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Admin.chm");
            Help.ShowHelp(this, str);
        }

        private void VMMgr_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.SaveRegData();
            }
            catch
            {
            }
        }

        private void VMMgr_Load(object sender, EventArgs e)
        {
            this.SetLanguage();
            if (!Global.IS_WOLFCOM)
            {
                this.LogoPic.SizeMode = PictureBoxSizeMode.CenterImage;
            }
            else
            {
                this.HeaderPanel.BackgroundImage = Properties.Resources.topbar45;
                this.LogoPic.Image = Properties.Resources.wolfcom_bar;
            }
            string str = Path.Combine(Directory.GetCurrentDirectory(), "Studio");
            ICollection<IPlugin> plugins = VMMgr.LoadPlugins(str);
            this.tMenu = new Menu(this.MenuTree, plugins, this.PluginPanel);
            this.LoadRegData();
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
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(VMMgr));
            this.statusStrip1 = new StatusStrip();
            this.vSplitContainer1 = new vSplitContainer();
            this.MenuTree = new vTreeView();
            this.TreeImages = new ImageList(this.components);
            this.PluginPanel = new Panel();
            this.HeaderPanel = new Panel();
            this.LogoPic = new PictureBox();
            this.btnHelp = new vButton();
            this.lbl_StudioTitle = new Label();
            this.pictureBox1 = new PictureBox();
            this.vSplitContainer1.Panel1.SuspendLayout();
            this.vSplitContainer1.Panel2.SuspendLayout();
            this.vSplitContainer1.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((ISupportInitialize)this.LogoPic).BeginInit();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            base.SuspendLayout();
            this.statusStrip1.Location = new Point(0, 540);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(784, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            this.vSplitContainer1.AllowAnimations = true;
            this.vSplitContainer1.Dock = DockStyle.Fill;
            this.vSplitContainer1.Location = new Point(0, 45);
            this.vSplitContainer1.Name = "vSplitContainer1";
            this.vSplitContainer1.Orientation = Orientation.Horizontal;
            this.vSplitContainer1.Panel1.BackColor = Color.White;
            this.vSplitContainer1.Panel1.BorderColor = Color.Silver;
            this.vSplitContainer1.Panel1.Controls.Add(this.MenuTree);
            this.vSplitContainer1.Panel1.Location = new Point(0, 0);
            this.vSplitContainer1.Panel1.Name = "Panel1";
            this.vSplitContainer1.Panel1.Size = new Size(247, 495);
            this.vSplitContainer1.Panel1.TabIndex = 1;
            this.vSplitContainer1.Panel2.BackColor = Color.White;
            this.vSplitContainer1.Panel2.BorderColor = Color.Silver;
            this.vSplitContainer1.Panel2.Controls.Add(this.PluginPanel);
            this.vSplitContainer1.Panel2.Location = new Point(254, 0);
            this.vSplitContainer1.Panel2.Name = "Panel2";
            this.vSplitContainer1.Panel2.Size = new Size(530, 495);
            this.vSplitContainer1.Panel2.TabIndex = 2;
            this.vSplitContainer1.Size = new Size(784, 495);
            this.vSplitContainer1.SplitterDistance = 250;
            this.vSplitContainer1.StyleKey = "Splitter";
            this.vSplitContainer1.TabIndex = 2;
            this.vSplitContainer1.Text = "vSplitContainer1";
            this.vSplitContainer1.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.MenuTree.AccessibleName = "TreeView";
            this.MenuTree.AccessibleRole = AccessibleRole.List;
            this.MenuTree.Dock = DockStyle.Fill;
            this.MenuTree.ImageList = this.TreeImages;
            this.MenuTree.ItemHeight = 20;
            this.MenuTree.Location = new Point(0, 0);
            this.MenuTree.Name = "MenuTree";
            this.MenuTree.ScrollPosition = new Point(0, 0);
            this.MenuTree.SelectedNode = null;
            this.MenuTree.ShowRootLines = true;
            this.MenuTree.Size = new Size(247, 495);
            this.MenuTree.TabIndex = 0;
            this.MenuTree.TreeIndent = 25;
            this.MenuTree.VIBlendScrollBarsTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.MenuTree.VIBlendTheme = VIBLEND_THEME.OFFICE2010SILVER;
            this.TreeImages.ImageStream = (ImageListStreamer)Resources.VMMgr.TreeImages_ImageStream;
            this.TreeImages.TransparentColor = Color.Transparent;
            this.TreeImages.Images.SetKeyName(0, "networkcomputer.png");
            this.TreeImages.Images.SetKeyName(1, "org.png");
            this.TreeImages.Images.SetKeyName(2, "building.png");
            this.TreeImages.Images.SetKeyName(3, "accounts.png");
            this.TreeImages.Images.SetKeyName(4, "assets.png");
            this.TreeImages.Images.SetKeyName(5, "manufacturer.png");
            this.TreeImages.Images.SetKeyName(6, "link.png");
            this.TreeImages.Images.SetKeyName(7, "activedirectory.png");
            this.TreeImages.Images.SetKeyName(8, "settings.png");
            this.TreeImages.Images.SetKeyName(9, "settings32.png");
            this.TreeImages.Images.SetKeyName(10, "cloud.png");
            this.TreeImages.Images.SetKeyName(11, "tools.png");
            this.TreeImages.Images.SetKeyName(12, "sys_settings2.png");
            this.TreeImages.Images.SetKeyName(13, "camera.png");
            this.TreeImages.Images.SetKeyName(14, "storage.png");
            this.TreeImages.Images.SetKeyName(15, "emailCfg.png");
            this.TreeImages.Images.SetKeyName(16, "license.png");
            this.TreeImages.Images.SetKeyName(17, "systemlogs.png");
            this.TreeImages.Images.SetKeyName(18, "account_logs.png");
            this.TreeImages.Images.SetKeyName(19, "reports.png");
            this.TreeImages.Images.SetKeyName(20, "reports2.png");
            this.TreeImages.Images.SetKeyName(21, "device.png");
            this.TreeImages.Images.SetKeyName(22, "restore.png");
            this.TreeImages.Images.SetKeyName(23, "camera.png");
            this.PluginPanel.AutoScroll = true;
            this.PluginPanel.Dock = DockStyle.Fill;
            this.PluginPanel.Location = new Point(0, 0);
            this.PluginPanel.Name = "PluginPanel";
            this.PluginPanel.Size = new Size(530, 495);
            this.PluginPanel.TabIndex = 0;
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.BackgroundImageLayout = ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.LogoPic);
            this.HeaderPanel.Controls.Add(this.btnHelp);
            this.HeaderPanel.Controls.Add(this.lbl_StudioTitle);
            this.HeaderPanel.Controls.Add(this.pictureBox1);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(784, 45);
            this.HeaderPanel.TabIndex = 0;
            this.LogoPic.BackColor = Color.Transparent;
            this.LogoPic.Dock = DockStyle.Right;
            this.LogoPic.Image = Properties.Resources.logo;
            this.LogoPic.Location = new Point(535, 0);
            this.LogoPic.Margin = new Padding(4);
            this.LogoPic.Name = "LogoPic";
            this.LogoPic.Padding = new Padding(0, 6, 4, 6);
            this.LogoPic.Size = new Size(249, 45);
            this.LogoPic.SizeMode = PictureBoxSizeMode.StretchImage;
            this.LogoPic.TabIndex = 0;
            this.LogoPic.TabStop = false;
            this.btnHelp.AllowAnimations = true;
            this.btnHelp.BackColor = Color.Transparent;
            this.btnHelp.Image = Properties.Resources.help;
            this.btnHelp.Location = new Point(273, 3);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.PaintBorder = false;
            this.btnHelp.PaintDefaultBorder = false;
            this.btnHelp.PaintDefaultFill = false;
            this.btnHelp.RoundedCornersMask = 15;
            this.btnHelp.RoundedCornersRadius = 0;
            this.btnHelp.Size = new Size(40, 40);
            this.btnHelp.TabIndex = 3;
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.lbl_StudioTitle.AutoSize = true;
            this.lbl_StudioTitle.BackColor = Color.Transparent;
            this.lbl_StudioTitle.ForeColor = Color.White;
            this.lbl_StudioTitle.Location = new Point(51, 7);
            this.lbl_StudioTitle.Name = "lbl_StudioTitle";
            this.lbl_StudioTitle.Size = new Size(168, 13);
            this.lbl_StudioTitle.TabIndex = 2;
            this.lbl_StudioTitle.Text = "System Configuration and Settings";
            this.pictureBox1.BackColor = Color.Transparent;
            this.pictureBox1.Image = Properties.Resources.studio_2;
            this.pictureBox1.Location = new Point(4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(40, 38);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(784, 562);
            base.Controls.Add(this.vSplitContainer1);
            base.Controls.Add(this.statusStrip1);
            base.Controls.Add(this.HeaderPanel);
            base.Icon = (Icon)Resources.VMMgr.VMMgrIcon;
            this.MinimumSize = new Size(800, 600);
            base.Name = "VMMgr";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "C3 Sentinel Studio";
            base.FormClosing += new FormClosingEventHandler(this.VMMgr_FormClosing);
            base.Load += new EventHandler(this.VMMgr_Load);
            this.vSplitContainer1.Panel1.ResumeLayout(false);
            this.vSplitContainer1.Panel2.ResumeLayout(false);
            this.vSplitContainer1.ResumeLayout(false);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((ISupportInitialize)this.LogoPic).EndInit();
            ((ISupportInitialize)this.pictureBox1).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public static ICollection<IPlugin> LoadPlugins(string path)
        {
            string[] files = null;
            if (!Directory.Exists(path))
            {
                return null;
            }
            files = Directory.GetFiles(path, "*.dll");
            ICollection<Assembly> assemblies = new List<Assembly>((int)files.Length);
            string[] strArrays = files;
            for (int i = 0; i < (int)strArrays.Length; i++)
            {
                string str = strArrays[i];
                assemblies.Add(Assembly.Load(AssemblyName.GetAssemblyName(str)));
            }
            Type type = typeof(IPlugin);
            ICollection<Type> types = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly == null)
                {
                    continue;
                }
                Type[] typeArray = assembly.GetTypes();
                for (int j = 0; j < (int)typeArray.Length; j++)
                {
                    Type type1 = typeArray[j];
                    if (!type1.IsInterface && !type1.IsAbstract && type1.GetInterface(type.FullName) != null)
                    {
                        types.Add(type1);
                    }
                }
            }
            ICollection<IPlugin> plugins = new List<IPlugin>(types.Count);
            foreach (Type type2 in types)
            {
                plugins.Add((IPlugin)Activator.CreateInstance(type2));
            }
            return plugins;
        }

        private void LoadRegData()
        {
            try
            {
                WindowPos windowPos = Registry.GetWindowPos("StudioWindow");
                if (windowPos.Height == 0 || windowPos.Width == 0)
                {
                    base.StartPosition = FormStartPosition.CenterScreen;
                    base.Width = 800;
                    base.Height = 600;
                }
                else
                {
                    base.Width = windowPos.Width;
                    base.Height = windowPos.Height;
                    base.Location = new Point(windowPos.PosX, windowPos.PosY);
                }
            }
            catch
            {
            }
        }

        private void SaveRegData()
        {
            try
            {
                WindowPos windowPo = new WindowPos()
                {
                    Width = base.Width,
                    Height = base.Height,
                    PosX = base.Location.X,
                    PosY = base.Location.Y
                };
                Registry.SetWindowPos(windowPo, "StudioWindow");
            }
            catch
            {
            }
        }

        private void SetLanguage()
        {
            LangCtrl.reText(this);
            this.Text = LangCtrl.GetString("dlg_Studio", "C3 Sentinel Studio");
        }
    }
}