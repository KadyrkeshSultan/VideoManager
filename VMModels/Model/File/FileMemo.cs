using System;

namespace VMModels.Model
{
    public class FileMemo
    {
        public Guid Id { get; set; }
        public string AccountName { get; set; }
        public string BudgeNumber { get; set; }
        public DateTime? Timestamp { get; set; }
        public string ShortDesc { get; set; }
        public string Memo { get; set; }
        public string Text { get; set; }
        public virtual DataFile DataFile { get; set; }
    }
}
