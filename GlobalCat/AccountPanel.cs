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
            lblSecurity.Text = LangCtrl.GetString("sec_Unclassified", "Unclassified");
            switch (security)
            {
                case SECURITY.TOPSECRET:
                    {
                        lblSecurity.ForeColor = Color.Yellow;
                        lblSecurity.BackColor = Color.Red;
                        lblSecurity.Text = LangCtrl.GetString("sec_TopSecret", "Top Secret");
                        return;
                    }
                case SECURITY.SECRET:
                    {
                        lblSecurity.ForeColor = Color.White;
                        lblSecurity.BackColor = Color.Orange;
                        lblSecurity.Text = LangCtrl.GetString("sec_Secret", "Secret");
                        return;
                    }
                case SECURITY.OFFICIAL:
                    {
                        lblSecurity.ForeColor = Color.Black;
                        lblSecurity.BackColor = Color.Yellow;
                        lblSecurity.Text = LangCtrl.GetString("sec_Official", "Official");
                        return;
                    }
                default:
                    {
                        return;
                    }
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
            base.SuspendLayout();
            this.AccountName.AutoSize = true;
            this.AccountName.Font = new Font("Verdana", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.AccountName.ForeColor = Color.White;
            this.AccountName.Location = new Point(4, 3);
            this.AccountName.Name = "AccountName";
            this.AccountName.Size = new Size(78, 18);
            this.AccountName.TabIndex = 0;
            this.AccountName.Text = "Account";
            this.AccountRank.AutoSize = true;
            this.AccountRank.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.AccountRank.ForeColor = Color.White;
            this.AccountRank.Location = new Point(4, 25);
            this.AccountRank.Name = "AccountRank";
            this.AccountRank.Size = new Size(36, 13);
            this.AccountRank.TabIndex = 1;
            this.AccountRank.Text = "Rank";
            this.lblSecurity.Anchor = AnchorStyles.Right;
            this.lblSecurity.BorderStyle = BorderStyle.FixedSingle;
            this.lblSecurity.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblSecurity.ForeColor = Color.White;
            this.lblSecurity.Location = new Point(317, 3);
            this.lblSecurity.Name = "lblSecurity";
            this.lblSecurity.Size = new Size(117, 23);
            this.lblSecurity.TabIndex = 2;
            this.lblSecurity.Text = "Security";
            this.lblSecurity.TextAlign = ContentAlignment.MiddleLeft;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(64, 64, 64);
            base.Controls.Add(this.lblSecurity);
            base.Controls.Add(this.AccountRank);
            base.Controls.Add(this.AccountName);
            base.Name = "AccountPanel";
            base.Size = new Size(437, 41);
            base.Load += new EventHandler(this.AccountPanel_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
