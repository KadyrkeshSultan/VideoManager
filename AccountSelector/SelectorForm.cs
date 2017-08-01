using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Unity;
using AccountCtrl2;

namespace AccountSelector
{
    public class SelectorForm : Form
    {
        private AcctCtrl accounts;
        private IContainer components;

        public bool IsShowCurrentAccount {  get;  set; }

        public event DEL_AccountSelectorCallback EVT_AccountSelectorCallback;

        
        public SelectorForm()
        {
            accounts = new AcctCtrl();
            InitializeComponent();
        }

        
        private void SelectorForm_Load(object sender, EventArgs e)
        {
            LangCtrl.GetString("dlg_AccountSelector", "Account Selector");
            accounts.EVT_NodeCallback += new AcctCtrl.DEL_NodeCallback(accounts_EVT_NodeCallback);
            accounts.InitTree();
            accounts.IsShowCurrentAccount = this.IsShowCurrentAccount;
            Controls.Add(accounts);
        }

        
        private void accounts_EVT_NodeCallback(object sender, CmdAccountPickerEventArgs args)
        {
            Callback(this, new CmdAccountSelectorEventArgs(args.nodeRec.RecIdx));
            Close();
        }

        
        private void Callback(object sender, CmdAccountSelectorEventArgs args)
        {
            if (EVT_AccountSelectorCallback == null)
                return;
            EVT_AccountSelectorCallback(this, args);
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(476, 408);
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.Name = "Selector Form";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Account Selector";
            this.Load += new EventHandler(this.SelectorForm_Load);
            this.ResumeLayout(false);
        }

        public delegate void DEL_AccountSelectorCallback(object sender, CmdAccountSelectorEventArgs args);
    }
}
