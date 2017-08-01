using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Unity;
using VMModels.Enums;

namespace GlobalCat
{
    public class AccountPanel : UserControl
    {
        private IContainer components;
        private Label AccountName;
        private Label AccountRank;
        private Label lblSecurity;

        
        public AccountPanel(string name, string data, SECURITY security)
        {
            InitializeComponent();
            Dock = DockStyle.Top;
            AccountName.Text = name;
            AccountRank.Text = data;
            lblSecurity.BackColor = Color.Transparent;
            lblSecurity.Text = LangCtrl.GetString("GlobalCat_AcctP_1", "GlobalCat_AcctP_2");
            switch (security)
            {
                case SECURITY.TOPSECRET:
                    lblSecurity.ForeColor = Color.Yellow;
                    lblSecurity.BackColor = Color.Red;
                    lblSecurity.Text = LangCtrl.GetString("GlobalCat_AcctP_3", "GlobalCat_AcctP_4");
                    break;
                case SECURITY.SECRET:
                    lblSecurity.ForeColor = Color.White;
                    lblSecurity.BackColor = Color.Orange;
                    lblSecurity.Text = LangCtrl.GetString("GlobalCat_AcctP_5", "GlobalCat_AcctP_6");
                    break;
                case SECURITY.OFFICIAL:
                    lblSecurity.ForeColor = Color.Black;
                    lblSecurity.BackColor = Color.Yellow;
                    lblSecurity.Text = LangCtrl.GetString("GlobalCat_AcctP_7", "GlobalCat_AcctP_8");
                    break;
            }
        }

        
        private void AccountPanel_Load(object sender, EventArgs e)
        {
            LangCtrl.reText(this);
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        
        private void InitializeComponent()
        {
            this.AccountName = new Label();
            this.AccountRank = new Label();
            this.lblSecurity = new Label();
            this.SuspendLayout();
            this.AccountName.AutoSize = true;
            this.AccountName.Font = new Font("GlobalCat_AcctP_9", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.AccountName.ForeColor = Color.White;
            this.AccountName.Location = new Point(4, 3);
            this.AccountName.Name = "GlobalCat_AcctP_10";
            this.AccountName.Size = new Size(78, 18);
            this.AccountName.TabIndex = 0;
            this.AccountName.Text = "GlobalCat_AcctP_11";
            this.AccountRank.AutoSize = true;
            this.AccountRank.Font = new Font("GlobalCat_AcctP_12", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.AccountRank.ForeColor = Color.White;
            this.AccountRank.Location = new Point(4, 25);
            this.AccountRank.Name = "GlobalCat_AcctP_13";
            this.AccountRank.Size = new Size(36, 13);
            this.AccountRank.TabIndex = 1;
            this.AccountRank.Text = "GlobalCat_AcctP_14";
            this.lblSecurity.Anchor = AnchorStyles.Right;
            this.lblSecurity.BorderStyle = BorderStyle.FixedSingle;
            this.lblSecurity.Font = new Font("GlobalCat_AcctP_15", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lblSecurity.ForeColor = Color.White;
            this.lblSecurity.Location = new Point(317, 3);
            this.lblSecurity.Name = "GlobalCat_AcctP_16";
            this.lblSecurity.Size = new Size(117, 23);
            this.lblSecurity.TabIndex = 2;
            this.lblSecurity.Text = "GlobalCat_AcctP_17";
            this.lblSecurity.TextAlign = ContentAlignment.MiddleLeft;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(64, 64, 64);
            this.Controls.Add((Control)this.lblSecurity);
            this.Controls.Add((Control)this.AccountRank);
            this.Controls.Add((Control)this.AccountName);
            this.Name = "GlobalCat_AcctP_18";
            this.Size = new Size(437, 41);
            this.Load += new EventHandler(this.AccountPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
