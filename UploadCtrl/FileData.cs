using System;
using System.Drawing;

namespace UploadCtrl
{
    public class FileData
    {
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public string Server { get; set; }
        public string RelativePath { get; set; }
        public string OriginalFileName { get; set; }
        public DateTime FileTimestamp { get; set; }
        public long FileSize { get; set; }
        public string FileHashCode { get; set; }
        public Image Thumbnail { get; set; }
        public bool IsDat { get; set; }
    }
}
