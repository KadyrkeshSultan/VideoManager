using System;
using System.Collections;
using System.Collections.Generic;
using VMModels.Enums;

namespace VMModels.Model
{
    public class DataFile
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid TrackingID { get; set; }
        public string UNCName { get; set; }
        public string UNCPath { get; set; }
        public string OriginalFileName { get; set; }
        public string FileExtension { get; set; }
        public string FileExtension2 { get; set; }
        public DateTime? FileTimestamp { get; set; }
        public DateTime? FileAddedTimestamp { get; set; }
        public byte[] Thumbnail { get; set; }
        public long FileSize { get; set; }
        public string GPS { get; set; }
        public bool? IsEncrypted { get; set; }
        public string ShortDesc { get; set; }
        public bool? IsPurged { get; set; }
        public DateTime? PurgeTimestamp { get; set; }
        public string PurgeFileName { get; set; }
        public string StoredFileName { get; set; }
        public string SetName { get; set; }
        public SECURITY Security { get; set; }
        public string Classification { get; set; }
        public int Rating { get; set; }
        public string RMSNumber { get; set; }
        public string CADNumber { get; set; }
        public bool IsIndefinite { get; set; }
        public string CloudID { get; set; }
        public string CloudMetadata { get; set; }
        public string MachineName { get; set; }
        public string MachineAccount { get; set; }
        public string LoginID { get; set; }
        public string UserDomain { get; set; }
        public string SourcePath { get; set; }
        public bool IsEvidence { get; set; }
        public string FileHashCode { get; set; }
        public HASH_ALGORITHM HashAlgorithm { get; set; }
        public virtual ICollection<FileMemo> FileMemos { get; set; }
        public virtual ICollection<VideoTag> VideoTags { get; set; }
        public virtual ICollection<DFClass> FileClasses { get; set; }

        public DataFile()
        {
            VideoTags = new HashSet<VideoTag>();
            FileMemos = new HashSet<FileMemo>();
            FileClasses = new HashSet<DFClass>();
        }
    }
}
