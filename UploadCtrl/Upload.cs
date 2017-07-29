using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadCtrl
{
    // TODO : Тут не все методы, многих элементов формы нету
    public class Upload
    {
        public bool IsRawName;
        private Guid AccountID;
        private string MachineName;
        private string MachineAccount;
        private string MachineDomain;
        public int FileCount;
        private string SourcePath;
        private string Server;
        private string TargetPath;
        private string RelativePath;
        private bool IsRunning;
        private string[] fileEntries;
        private int i;
        private string TimeStamp;
        private Dictionary<string, bool> FileList;
        public bool CancelFlag;
        private string[] FileNames;
        private string DataPath;
        private string UNCRoot;
        private string SourceFileName;
        private string TargetFileName;

        public string SETID { get; set; }
        public string Filter { get; set; }
        public bool DeleteSource { get; set; }
        public bool ShowTimestamp { get; set; }

        public event Upload.IntDelegate FileCopyProgress;
        public event Upload.DEL_UploadComplete EVT_UploadComplete;
        public event Upload.DEL_UploadCallback EVT_UploadCallback;

        public Upload()
        {
            this.AccountID = Guid.Empty;
            this.MachineName = Environment.MachineName;
            this.MachineAccount = Environment.UserName;
            this.MachineDomain = Environment.UserDomainName;
            this.SourcePath = string.Empty;
            this.Server = string.Empty;
            this.TargetPath = string.Empty;
            this.RelativePath = string.Empty;
            this.TimeStamp = string.Empty;
            this.FileList = new Dictionary<string, bool>();
            this.DataPath = string.Empty;
            this.UNCRoot = string.Empty;
            this.SourceFileName = string.Empty;
            this.TargetFileName = string.Empty;
        }

        public delegate void IntDelegate(int Int);

        public delegate void DEL_UploadComplete();

        public delegate void DEL_UploadCallback(object sender, CmdEventArgs args);
    }
}
