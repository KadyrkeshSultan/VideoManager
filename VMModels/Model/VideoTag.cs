using System;

namespace VMModels.Model
{
    public class VideoTag
    {
        public Guid Id { get; set; }
        public long StartFrame { get; set; }
        public double? StartTime { get; set; }
        public long EndFrame { get; set; }
        public double? EndTime { get; set; }
        public string ShortDesc { get; set; }
        public string Memo { get; set; }
        public virtual DataFile DataFile { get; set; }
    }
}
