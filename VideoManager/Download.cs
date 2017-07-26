using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace VideoManager
{
    public partial class Download : Form
    {
        private string FolderPath;
        private bool IsCancel;
        private DateTime StartTimestamp;
        private Stopwatch stopwatch;
        private int FileCount;
        private string CameraDriveID;
        private Label lbl_DL_FileName;
        private Label lbl_DL_FileExt;
        private Label lbl_DL_Timestamp;
        private Label lbl_DL_FileSize;
        private Label lbl_DLTime;
        private Label lblFileName;
        private Label lblFileExt;
        private Label lblTimestamp;
        private Label lblFileSize;
        private Label lblSourcePath;
        private Label lblTimespan;

        public Guid Account_ID { get; set; }


        public Download(string path, Guid accountid)
        {
            this.FolderPath = string.Empty;
            this.CameraDriveID = string.Empty;
            InitializeComponent();
            this.stopwatch = new Stopwatch();
            this.stopwatch.Start();
            this.StartTimestamp = DateTime.Now;

            try
            {
                Guid accountId = new Assets().GetAccountID(Path.GetPathRoot(path));
                this.Account_ID = !(accountId != Guid.Empty) ? accountid : accountId;
                this.FolderPath = path;
            }
        }
    }
}
