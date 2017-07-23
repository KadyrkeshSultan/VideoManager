using System;

namespace VideoManager.Model
{
    class CaseMemo
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public string BadgeNumber { get; set; }
        public DateTime Timestamp { get; set; }
        public string ShortDesc { get; set; }
        public string Memo { get; set; }
    }
}
