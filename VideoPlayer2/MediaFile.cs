using System;
using VMModels.Enums;

namespace VideoPlayer2
{
    public class MediaFile
    {
        public string FileName {  get;  set; }

        public SECURITY Security {  get;  set; }

        public bool IsEvidence {  get;  set; }

        public DateTime FileDate {  get;  set; }

        public string Classification {  get;  set; }

        public string Ext2 {  get;  set; }

        public Guid FileID {  get;  set; }

        public string UNCName {  get;  set; }

        public string UNCPath {  get;  set; }

        public string Set {  get;  set; }
        
    }
}
