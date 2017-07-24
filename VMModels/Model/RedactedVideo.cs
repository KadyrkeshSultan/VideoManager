using System;

namespace VMModels.Model
{
    public class RedactedVideo
    {
        public Guid Id { get; set; }
        public Guid ParentID { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string FileName { get; set; }
        public string UNCPath { get; set; }
        public byte[] Thumbnail { get; set; }
        public long FileSize { get; set; }
        public long FileHash { get; set; }
        public string MachineName { get; set; }
        public string MachineAccount { get; set; }
        public string LoginID { get; set; }
        public string UserDomain { get; set; }
        public bool IsEvidence { get; set; }
    }
}
