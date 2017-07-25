using System;
using System.Windows.Forms;

namespace VideoManager
{
    public partial class Download : Form
    {
        private string FolderPath;
        private bool IsCancel;
        private DateTime StartTimestamp;
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


        public Download(string path, Guid accountId)
        {
            InitializeComponent();
            this.FolderPath = string.Empty;
            this.CameraDriveID = string.Empty;
        }
    }
}
