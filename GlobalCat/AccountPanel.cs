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
            lblSecurity.Text = LangCtrl.GetString("GlobalCat_AcctPanel_1", "GlobalCat_AcctPanel_2");
            switch (security)
            {
                case SECURITY.TOPSECRET:
                    lblSecurity.ForeColor = Color.Yellow;
                    lblSecurity.BackColor = Color.Red;
                    lblSecurity.Text = LangCtrl.GetString("GlobalCat_AcctPanel_3", "GlobalCat_AcctPanel_4");
                    break;
                case SECURITY.SECRET:
                    lblSecurity.ForeColor = Color.White;
                    lblSecurity.BackColor = Color.Orange;
                    lblSecurity.Text = LangCtrl.GetString("GlobalCat_AcctPanel_5", "GlobalCat_AcctPanel_6");
                    break;
                case SECURITY.OFFICIAL:
                    lblSecurity.ForeColor = Color.Black;
                    lblSecurity.BackColor = Color.Yellow;
                    lblSecurity.Text = LangCtrl.GetString("GlobalCat_AcctPanel_7", "GlobalCat_AcctPanel_8");
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
            this.AccountName.Font = new Font("GlobalCat_AcctPanel_9", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.AccountName.ForeColor = Color.White;
            this.AccountName.Location = new Point(4, 3);
            this.AccountName.Name = "GlobalCat_AcctPanel_10";
            this.AccountName.Size = new Size(78, 18);
            this.AccountName.TabIndex = 0;
            this.AccountName.Text = "GlobalCat_AcctPanel_11";
            this.AccountRank.AutoSize = true;
            this.AccountRank.Font = new Font("GlobalCat_AcctPanel_12", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.AccountRank.ForeColor = Color.White;
            this.AccountRank.Location = new Point(4, 25);
            this.AccountRank.Name = "GlobalCat_AcctPanel_13";
            this.AccountRank.Size = new Size(36, 13);
            this.AccountRank.TabIndex = 1;
            this.AccountRank.Text = "GlobalCat_AcctPanel_14";
            this.lblSecurity.Anchor = AnchorStyles.Right;
            this.lblSecurity.BorderStyle = BorderStyle.FixedSingle;
            this.lblSecurity.Font = new Font("GlobalCat_AcctPanel_15", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblSecurity.ForeColor = Color.White;
            this.lblSecurity.Location = new Point(317, 3);
            this.lblSecurity.Name = "GlobalCat_AcctPanel_16";
            this.lblSecurity.Size = new Size(117, 23);
            this.lblSecurity.TabIndex = 2;
            this.lblSecurity.Text = "GlobalCat_AcctPanel_17";
            this.lblSecurity.TextAlign = ContentAlignment.MiddleLeft;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(64, 64, 64);
            this.Controls.Add((Control)this.lblSecurity);
            this.Controls.Add((Control)this.AccountRank);
            this.Controls.Add((Control)this.AccountName);
            this.Name = "GlobalCat_AcctPanel_18";
            this.Size = new Size(437, 41);
            this.Load += new EventHandler(this.AccountPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
