using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Speech.Synthesis;
using System.Windows.Forms;
using TXTextControl;
using Unity;
using VMInterfaces;
using VMModels.Model;

namespace MemoEditor
{
    public class EditorForm : Form
    {
        public string RTF;
        public int Hash;
        private bool IsReadOnly;
        public string DocText;
        private SpeechSynthesizer synth;
        private IContainer components;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem menu_File;
        private ToolStripMenuItem mnu_LoadFile;
        private ToolStripMenuItem mnu_Save;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem mnu_Close;
        private ToolStripMenuItem printToolStripMenuItem;
        private ToolStripMenuItem mnuPrintPreview;
        private ToolStripMenuItem mnuPrint;
        private ToolStripMenuItem menu_Speech;
        private ToolStripMenuItem mnu_PlayFile;
        private ToolStripMenuItem mnu_StopPlayback;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem mnu_Voices;
        private ToolStripComboBox cboVoices;
        private ToolStripMenuItem mnu_PlaySpeed;
        private ToolStripMenuItem mnu_Faster;
        private ToolStripMenuItem mnu_Slower;
        private ToolStripMenuItem mnu_NormalSpeed;
        private TextControl textControl1;
        private RulerBar rulerBar1;
        private TXTextControl.StatusBar statusBar1;
        private RulerBar rulerBar2;
        private ContextMenuStrip EditMenu;
        private ToolStripMenuItem mnu_StartRedact;
        private ToolStripMenuItem mnu_EndRedact;

        public string FileDocument {  get;  set; }

        private List<DOCData> DocList {  get;  set; }

        private bool PageBreak {  get;  set; }

        
        public EditorForm(List<DOCData> lst, bool PgBreaks, string Doc)
        {
            RTF = string.Empty;
            DocText = string.Empty;
            synth = new SpeechSynthesizer();
            InitializeComponent();
            DocList = lst;
            PageBreak = PgBreaks;
            FileDocument = Doc;
            IsReadOnly = true;
            Height = SystemInformation.VirtualScreen.Height - 60;
            Top = 10;
        }

        
        public EditorForm(int SlideNumber)
        {
            RTF = string.Empty;
            DocText = string.Empty;
            synth = new SpeechSynthesizer();
            InitializeComponent();
            DocList = new List<DOCData>();
            textControl1.CreateControl();
            Text = string.Format("Memo Editor [Slide {0}]", SlideNumber);
            Height = SystemInformation.VirtualScreen.Height - 60;
            Top = 10;
        }

        
        public EditorForm()
        {
            RTF = string.Empty;
            DocText = string.Empty;
            synth = new SpeechSynthesizer();
            InitializeComponent();
            textControl1.CreateControl();
        }

        
        private void SetLanguage()
        {
            LangCtrl.reText(this);
            this.printToolStripMenuItem.Text = LangCtrl.GetString("printToolStripMenuItem", "Print");
            this.mnuPrint.Text = LangCtrl.GetString("mnuPrint", "Print Document");
            this.mnu_Save.Text = LangCtrl.GetString("mnu_Save", "Save");
            this.mnu_LoadFile.Text = LangCtrl.GetString("mnu_LoadFile", "Load File");
            this.mnu_Close.Text = LangCtrl.GetString("mnu_Close", "Close");
            this.menu_Speech.Text = LangCtrl.GetString("menu_Speech", "Speech");
            this.mnu_PlayFile.Text = LangCtrl.GetString("mnu_PlayFile", "Play File");
            this.mnu_StopPlayback.Text = LangCtrl.GetString("mnu_StopPlayback", "Stop Playback");
            this.mnu_Voices.Text = LangCtrl.GetString("mnu_Voices", "Voices");
            this.mnu_PlaySpeed.Text = LangCtrl.GetString("mnu_PlaySpeed", "Play Speed");
            this.mnu_Faster.Text = LangCtrl.GetString("mnu_Faster", "Faster");
            this.mnu_Slower.Text = LangCtrl.GetString("mnu_Slower", "Slower");
            this.mnu_NormalSpeed.Text = LangCtrl.GetString("mnu_NormalSpeed", "Normal");
            this.menu_File.Text = LangCtrl.GetString("menu_File", "File");
            this.mnuPrintPreview.Text = LangCtrl.GetString("mnuPrintPreview", "Print Preview");
        }

        
        private void EditorForm_Load(object sender, EventArgs e)
        {
            SetLanguage();
            Text = LangCtrl.GetString("dlg_MemoEditor", "Memo Editor");
            LoadVoices();
            if (DocList.Equals(null))
                DocList = new List<DOCData>();
            if (DocList.Count > 0)
            {
                int num = 0;
                mnu_LoadFile.Visible = false;
                mnu_Save.Visible = false;
                try
                {
                    using (RPM_DataFile rpmDataFile = new RPM_DataFile())
                    {
                        foreach (DOCData doc in DocList)
                        {
                            FileMemo memo = rpmDataFile.GetMemo(doc.RecId);
                            if (PageBreak)
                                textControl1.Append(memo.Memo, StringStreamType.RichTextFormat, AppendSettings.StartWithNewSection);
                            else
                                textControl1.Append(memo.Memo, StringStreamType.RichTextFormat, AppendSettings.None);
                            IsReadOnly = true;
                            ++num;
                        }
                        textControl1.EditMode = EditMode.ReadOnly;
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                }
            }
            else
            {
                try
                {
                    if (!RTF.Equals(string.Empty))
                        textControl1.Load(RTF, StringStreamType.RichTextFormat);
                }
                catch
                {
                }
            }
            string stringData;
            textControl1.Save(out stringData, StringStreamType.RichTextFormat);
            Hash = stringData.GetHashCode();
            textControl1.BackgroundStyle = BackgroundStyle.ColorScheme;
            textControl1.BorderStyle = TXTextControl.BorderStyle.None;
        }

        
        private void mnu_Save_Click(object sender, EventArgs e)
        {
            string stringData = string.Empty;
            textControl1.Save(out stringData, StringStreamType.RichTextFormat);
            RTF = stringData;
        }

        
        private void mnu_LoadFile_Click(object sender, EventArgs e)
        {
            try
            {
                int num = (int)textControl1.Load(StreamType.HTMLFormat | StreamType.RichTextFormat | StreamType.PlainText | StreamType.MSWord | StreamType.AdobePDF);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        
        private void EditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopSpeech();
            DialogResult = DialogResult.Cancel;
            if (IsReadOnly)
                return;
            textControl1.Save(out this.RTF, StringStreamType.RichTextFormat);
            DocText = textControl1.Text;
            if (Hash == RTF.GetHashCode() || MessageBox.Show(this, LangCtrl.GetString("msg_DocChanged", "Document has changed. Do you want to save?"), "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            DialogResult = DialogResult.OK;
        }

        
        private void mnu_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        private void mnuPrintPreview_Click(object sender, EventArgs e)
        {
            this.textControl1.PrintPreview(this.FileDocument);
        }

        
        private void mnuPrint_Click(object sender, EventArgs e)
        {
            this.textControl1.Print(this.FileDocument);
        }

        
        private void mnu_PlayFile_Click(object sender, EventArgs e)
        {
            this.PlayFile();
            this.mnu_PlayFile.Enabled = false;
        }

        
        private void PlayFile()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += (DoWorkEventHandler)((o, e) =>
            {
                try
                {
                    synth = new SpeechSynthesizer();
                    synth.SetOutputToDefaultAudioDevice();
                    synth.Speak(textControl1.Text);
                }
                catch (Exception ex)
                {
                }
            });
            backgroundWorker.RunWorkerAsync();
        }

        
        private void mnu_StopPlayback_Click(object sender, EventArgs e)
        {
            this.StopSpeech();
        }

        
        private void StopSpeech()
        {
            this.mnu_PlayFile.Enabled = true;
            this.synth.SpeakAsyncCancelAll();
        }

        
        private void cboVoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.synth.SelectVoice(this.cboVoices.Text);
        }

        
        private void LoadVoices()
        {
            this.cboVoices.Items.Clear();
            foreach (InstalledVoice installedVoice in this.synth.GetInstalledVoices())
            {
                VoiceInfo voiceInfo = installedVoice.VoiceInfo;
                this.cboVoices.Items.Add((object)new ListViewItem()
                {
                    Text = voiceInfo.Name,
                    Tag = ((object)voiceInfo)
                }.Text);
            }
            this.cboVoices.SelectedIndex = 0;
        }

        
        private void mnu_Faster_Click(object sender, EventArgs e)
        {
            if (this.synth.Rate + 2 > 10)
                return;
            this.synth.Rate = this.synth.Rate + 2;
        }

        
        private void mnu_Slower_Click(object sender, EventArgs e)
        {
            if (this.synth.Rate - 2 < -10)
                return;
            this.synth.Rate = this.synth.Rate - 2;
        }

        
        private void mnu_NormalSpeed_Click(object sender, EventArgs e)
        {
            this.synth.Rate = 0;
        }

        
        private void mnu_StartRedact_Click(object sender, EventArgs e)
        {
            this.textControl1.Selection.TextBackColor = Color.LightGray;
        }

        
        private void mnu_EndRedact_Click(object sender, EventArgs e)
        {
            this.textControl1.Selection.TextBackColor = Color.White;
        }

        
        private void textControl1_KeyUp(object sender, KeyEventArgs e)
        {
            int keyValue = e.KeyValue;
            if (!(e.Control & e.KeyValue == 82))
                return;
            if (this.textControl1.Selection.TextBackColor.Name.Equals("ffd3d3d3"))
                this.textControl1.Selection.TextBackColor = Color.White;
            else
                this.textControl1.Selection.TextBackColor = Color.LightGray;
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_LoadFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnu_Close = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPrintPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_Speech = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_PlayFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_StopPlayback = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnu_Voices = new System.Windows.Forms.ToolStripMenuItem();
            this.cboVoices = new System.Windows.Forms.ToolStripComboBox();
            this.mnu_PlaySpeed = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_Faster = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_Slower = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_NormalSpeed = new System.Windows.Forms.ToolStripMenuItem();
            this.EditMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnu_StartRedact = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_EndRedact = new System.Windows.Forms.ToolStripMenuItem();
            this.rulerBar1 = new TXTextControl.RulerBar();
            this.statusBar1 = new TXTextControl.StatusBar();
            this.rulerBar2 = new TXTextControl.RulerBar();
            this.menuStrip1.SuspendLayout();
            this.EditMenu.SuspendLayout();
            this.SuspendLayout();
            ToolStripItemCollection items = this.menuStrip1.Items;
            ToolStripItem[] menuFile = new ToolStripItem[] { this.menu_File, this.printToolStripMenuItem, this.menu_Speech };
            this.menuStrip1.Items.AddRange(menuFile);
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(936, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            ToolStripItemCollection dropDownItems = this.menu_File.DropDownItems;
            ToolStripItem[] mnuLoadFile = new ToolStripItem[] { this.mnu_LoadFile, this.mnu_Save, this.toolStripMenuItem1, this.mnu_Close };
            this.menu_File.DropDownItems.AddRange(mnuLoadFile);
            this.menu_File.Name = "menu_File";
            this.menu_File.Size = new Size(37, 20);
            this.menu_File.Text = "File";
            this.mnu_LoadFile.Name = "mnu_LoadFile";
            this.mnu_LoadFile.Size = new Size(103, 22);
            this.mnu_LoadFile.Text = "Load";
            this.mnu_LoadFile.Click += new EventHandler(this.mnu_LoadFile_Click);
            this.mnu_Save.Name = "mnu_Save";
            this.mnu_Save.Size = new Size(103, 22);
            this.mnu_Save.Text = "Save";
            this.mnu_Save.Click += new EventHandler(this.mnu_Save_Click);
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new Size(100, 6);
            this.mnu_Close.Name = "mnu_Close";
            this.mnu_Close.Size = new Size(103, 22);
            this.mnu_Close.Text = "Close";
            this.mnu_Close.Click += new EventHandler(this.mnu_Close_Click);
            ToolStripItemCollection toolStripItemCollections = this.printToolStripMenuItem.DropDownItems;
            ToolStripItem[] toolStripItemArray = new ToolStripItem[] { this.mnuPrintPreview, this.mnuPrint };
            this.printToolStripMenuItem.DropDownItems.AddRange(toolStripItemArray);
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new Size(44, 20);
            this.printToolStripMenuItem.Text = "Print";
            this.mnuPrintPreview.Name = "mnuPrintPreview";
            this.mnuPrintPreview.Size = new Size(180, 22);
            this.mnuPrintPreview.Text = "Print Preview...";
            this.mnuPrintPreview.Click += new EventHandler(this.mnuPrintPreview_Click);
            this.mnuPrint.Name = "mnuPrint";
            this.mnuPrint.Size = new Size(180, 22);
            this.mnuPrint.Text = "Print Document(s)...";
            this.mnuPrint.Click += new EventHandler(this.mnuPrint_Click);
            ToolStripItemCollection dropDownItems1 = this.menu_Speech.DropDownItems;
            ToolStripItem[] mnuPlayFile = new ToolStripItem[] { this.mnu_PlayFile, this.mnu_StopPlayback, this.toolStripMenuItem2, this.mnu_Voices, this.mnu_PlaySpeed };
            this.menu_Speech.DropDownItems.AddRange(mnuPlayFile);
            this.menu_Speech.Name = "menu_Speech";
            this.menu_Speech.Size = new Size(57, 20);
            this.menu_Speech.Text = "Speech";
            this.mnu_PlayFile.Name = "mnu_PlayFile";
            this.mnu_PlayFile.Size = new Size(148, 22);
            this.mnu_PlayFile.Text = "Play File";
            this.mnu_PlayFile.Click += new EventHandler(this.mnu_PlayFile_Click);
            this.mnu_StopPlayback.Name = "mnu_StopPlayback";
            this.mnu_StopPlayback.Size = new Size(148, 22);
            this.mnu_StopPlayback.Text = "Stop Playback";
            this.mnu_StopPlayback.Click += new EventHandler(this.mnu_StopPlayback_Click);
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new Size(145, 6);
            this.mnu_Voices.DropDownItems.AddRange(new ToolStripItem[] { this.cboVoices });
            this.mnu_Voices.Name = "mnu_Voices";
            this.mnu_Voices.Size = new Size(148, 22);
            this.mnu_Voices.Text = "Voices";
            this.cboVoices.Name = "cboVoices";
            this.cboVoices.Size = new Size(121, 23);
            this.cboVoices.SelectedIndexChanged += new EventHandler(this.cboVoices_SelectedIndexChanged);
            ToolStripItemCollection toolStripItemCollections1 = this.mnu_PlaySpeed.DropDownItems;
            ToolStripItem[] mnuFaster = new ToolStripItem[] { this.mnu_Faster, this.mnu_Slower, this.mnu_NormalSpeed };
            this.mnu_PlaySpeed.DropDownItems.AddRange(mnuFaster);
            this.mnu_PlaySpeed.Name = "mnu_PlaySpeed";
            this.mnu_PlaySpeed.Size = new Size(148, 22);
            this.mnu_PlaySpeed.Text = "Play Speed";
            this.mnu_Faster.Name = "mnu_Faster";
            this.mnu_Faster.Size = new Size(114, 22);
            this.mnu_Faster.Text = "Faster";
            this.mnu_Faster.Click += new EventHandler(this.mnu_Faster_Click);
            this.mnu_Slower.Name = "mnu_Slower";
            this.mnu_Slower.Size = new Size(114, 22);
            this.mnu_Slower.Text = "Slower";
            this.mnu_Slower.Click += new EventHandler(this.mnu_Slower_Click);
            this.mnu_NormalSpeed.Name = "mnu_NormalSpeed";
            this.mnu_NormalSpeed.Size = new Size(114, 22);
            this.mnu_NormalSpeed.Text = "Normal";
            this.mnu_NormalSpeed.Click += new EventHandler(this.mnu_NormalSpeed_Click);
            this.textControl1.ContextMenuStrip = this.EditMenu;
            this.textControl1.Dock = DockStyle.Fill;
            this.textControl1.Font = new Font("Arial", 10f);
            this.textControl1.Location = new Point(0, 24);
            this.textControl1.Name = "textControl1";
            this.textControl1.RulerBar = this.rulerBar1;
            this.textControl1.Size = new Size(936, 904);
            this.textControl1.StatusBar = this.statusBar1;
            this.textControl1.TabIndex = 1;
            this.textControl1.VerticalRulerBar = this.rulerBar2;
            this.textControl1.KeyUp += new KeyEventHandler(this.textControl1_KeyUp);
            ToolStripItemCollection items1 = this.EditMenu.Items;
            ToolStripItem[] mnuStartRedact = new ToolStripItem[] { this.mnu_StartRedact, this.mnu_EndRedact };
            this.EditMenu.Items.AddRange(mnuStartRedact);
            this.EditMenu.Name = "EditMenu";
            this.EditMenu.Size = new Size(141, 48);
            this.mnu_StartRedact.Name = "mnu_StartRedact";
            this.mnu_StartRedact.Size = new Size(140, 22);
            this.mnu_StartRedact.Text = "Redact Text";
            this.mnu_StartRedact.Click += new EventHandler(this.mnu_StartRedact_Click);
            this.mnu_EndRedact.Name = "mnu_EndRedact";
            this.mnu_EndRedact.Size = new Size(140, 22);
            this.mnu_EndRedact.Text = "Clear Redact";
            this.mnu_EndRedact.Click += new EventHandler(this.mnu_EndRedact_Click);
            this.rulerBar1.Dock = DockStyle.Top;
            this.rulerBar1.Location = new Point(0, 24);
            this.rulerBar1.Name = "rulerBar1";
            this.rulerBar1.Size = new Size(936, 25);
            this.rulerBar1.TabIndex = 3;
            this.rulerBar1.Text = "rulerBar1";
            this.statusBar1.BackColor = SystemColors.Control;
            this.statusBar1.Dock = DockStyle.Bottom;
            this.statusBar1.Location = new Point(0, 928);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new Size(936, 22);
            this.statusBar1.TabIndex = 2;
            this.rulerBar2.Alignment = RulerBarAlignment.Left;
            this.rulerBar2.Dock = DockStyle.Left;
            this.rulerBar2.Location = new Point(0, 49);
            this.rulerBar2.Name = "rulerBar2";
            this.rulerBar2.Size = new Size(25, 879);
            this.rulerBar2.TabIndex = 4;
            this.rulerBar2.Text = "rulerBar2";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 741);
            this.Controls.Add(this.rulerBar2);
            this.Controls.Add(this.rulerBar1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.textControl1);
            this.Icon = Resources.EditorForm.EditorFormIcon;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EditorForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Memo Editor";
            this.FormClosing += new FormClosingEventHandler(this.EditorForm_FormClosing);
            this.Load += new System.EventHandler(this.EditorForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.EditMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
