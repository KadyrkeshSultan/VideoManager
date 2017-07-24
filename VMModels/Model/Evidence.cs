using System;

namespace VMModels.Model
{
    public class Evidence
    {
        public Guid Id { get; set; }
        public string ItemType { get; set; }
        public string ItemDesc { get; set; }
        public string FileNumber { get; set; }
        public DateTime? DateIn { get; set; }
        public DateTime? DateOut { get; set; }
        public string Memo { get; set; }
        public byte[] Picture { get; set; }
    }
}
